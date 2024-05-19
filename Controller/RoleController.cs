using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blogapi.Controller
{
    [ApiController]
    //[Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        [HttpGet("v1/role/{id:int}")]
        public async Task<IActionResult> GetById([FromServices] BlogDataContext context, [FromRoute] int id )
        {
            var role =  await context.Roles.FirstOrDefaultAsync(x => x.Id == id );

            if (role == null )
                return NotFound();

            return  Ok(role);
        }

       [HttpPost("v1/role")]
       public async Task<IActionResult> PostAsync([FromServices] BlogDataContext context, [FromBody]  Role model)
       {
        try{
        //   var roles = new Role{
        //     Id = 0,
        //     Name = role.Name,
        //     Slug = role.Slug
        //   };


         await context.Roles.AddAsync(model);
         await context.SaveChangesAsync();

          return Created($"v1/role/{model.Id}",model);
        }
        catch(DbUpdateException ex)
        {
          return StatusCode(500,"00EXR1 - Não foi possivel adicionar um Role");
        }
        catch(Exception ex)
        {//por questao de trakingg podemos colocar esse codigo nas Exception , ou seja quando der esse erro, se informar o codigo podemos procurar com o ctrl+f 
          return StatusCode(500," 01EXR2 - Erro interno do sistema");
        }

        
       }
        [HttpDelete("v1/role/{id:int}")]
       public async Task<IActionResult> Delete([FromServices]BlogDataContext context, [FromRoute] int id)
       {
        try{
          var role = await context.Roles.FirstOrDefaultAsync(x=> x.Id == id);

          if(role == null)
             return NotFound();


            context.Roles.Remove(role);
             await context.SaveChangesAsync();

             return Ok();
        }
        catch(DbUpdateException ex)
        {
          return StatusCode(500, "03EXR1 - Não foi possivel deletar uma Role");
        }

       }
    }
}