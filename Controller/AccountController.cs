using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Models;
using blogapi.Extensios;
using blogapi.ViewModel;
using BlogApi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

using blogapi.Service;
using blogapi.ViewModel.Accounts;
using BlogApi.ViewModel.Accounts;
using SendEmail.ViewModel;
using System.Text.RegularExpressions;
using System.Drawing;

namespace blogapi.Controller
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        //injecao de dependencia 
        private readonly TokenService _tokenService;

        //agora meu servico tem uma dependecia e eu nao preciso ficar instanciando 
        public AccountController(TokenService token)
        {
            _tokenService = token;
            
        }

        [HttpPost("v1/accounts")]
        public async Task<IActionResult> Post([FromBody] AccountViewModel model,
        [FromServices] BlogDataContext context,
          [FromServices] EmailService emailService )
        {
            if(!ModelState.IsValid)
                    return  BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

                var user = new User{
                    Name = model.Nome,
                    Email = model.Email,
                    Slug = model.Email.Replace("@","-").Replace(".","-"),
                    Bio = "test",
                    Image = "test"
                 
                };

               var passowrd = PasswordGenerator.Generate(25);
               user.PasswordHash = PasswordHasher.Hash(passowrd);

               try{
                 await context.Users.AddAsync(user);
                 await context.SaveChangesAsync();

                 emailService.SendEmail(
                  user.Name,
                  user.Email,
                   "Email enviado com sucesso!",
                   $"Sua senha é <strong>{passowrd}</strong>"
                   );

                 return Ok(new ResultViewModel<dynamic>(new {
                    user = user.Email, passowrd
                 }));
               }
                catch(Exception ex)
               {        
                return StatusCode(500, new ResultViewModel<string>(ex.Message));
               }    
        }

        [HttpPost("v1/accounts/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model, 
        [FromServices] BlogDataContext context,
        [FromServices]TokenService token )
        {
            if(!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            
            var user = await context.Users.AsNoTracking() // .Include(x => x.Roles)
           .FirstOrDefaultAsync(x => x.Email == model.Email);

            if(user == null)
                return StatusCode(401, new ResultViewModel<string>("Usuario nao encontrado"));

              if(!PasswordHasher.Verify(user.PasswordHash , model.Password) )  
                return StatusCode(401, new ResultViewModel<string>("Usuario ou senha incorretos "));
          try
          {
          var hashToken = token.GenerateToken(user);
          return Ok(new ResultViewModel<string>(hashToken,null));

          }
          catch(Exception ex)
          {
            return StatusCode(500 , new ResultViewModel<string>($"Erro interno do sistema {ex.Message}"));
          }
        }
        [HttpPost("v1/accounts/UploadImage")]
        public async Task<IActionResult> UploadImage(
          [FromBody] UploadImageViewModel model,
          [FromServices] BlogDataContext context
        ) 
        {
          var fileName = $"{Guid.NewGuid().ToString()}.jpg";
          var data = new Regex(@"data:image\/[a-z]+;base64,").Replace(model.Base64Image,"");
          var bytes = Convert.FromBase64String(data);

            try
            {
                await System.IO.File.WriteAllBytesAsync($"wwwroot/Images/{fileName}", bytes);
            }
            catch
            {
               return StatusCode(500, new ResultViewModel<string>("Falha interna no sistema"));
            }

          var user = await context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
                 
         if(user == null)
         {
          return NotFound(new ResultViewModel<Category>("Usuario nao encontrado"));
         }

         user.Image = Configuration.ImageBase64 + fileName;

            try
            {
                  context.Users.Update(user);
                  await context.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                  return StatusCode(500, new ResultViewModel<string>("Falha interna no sistema"));
            }

         return Ok(new ResultViewModel<string>("Imagem alterada com sucesso!!!",null));        
        }

        [Authorize(Roles ="user")]
        [HttpGet("v1/user")]
        public IActionResult GetUser()=> Ok(User.Identity.Name);
        [Authorize(Roles = "admin")]
        [HttpGet("v1/admin")]
        public IActionResult GetAdmin() =>Ok(User.Identity.Name);
        [Authorize(Roles = "author")]
        [HttpGet("v1/author")]
        public IActionResult GetAuthor() => Ok(User.Identity.Name);

    }
}
