using Azure;
using Blog.Data;
using Blog.Models;
using blogapi.ViewModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SendEmail.ViewModel.Posts;

namespace SendEmail.Controller;

    public class PostController : ControllerBase
    {
        [HttpGet("v1/posts")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context,
            [FromQuery] int page,
            [FromQuery] int pageSize = 20)
        {
            try{
            var count = await context.Posts.CountAsync();
            //var post = await context.Posts.ToListAsync(); //retorna td em lista

            // var post = context.Posts.AsNoTracking().Select( x => new {
            //      x.Id,
            //       x.Title,
            //        x.Summary           
            // }).ToListAsync(); // retorna apenas propriedades 

            var post = await context.Posts.AsNoTracking()          
            .Include(x=> x.Author)
            .Include(x=> x.Category)
            .Select( x => new ListPostViewModel{
                Id = x.Id,
                Tittle  = x.Title,
                Slug = x.Slug,
                LastUpdate = x.LastUpdateDate,
                Category = x.Category.Name,
                Author = $"{x.Author.Name} ({x.Author.Email})"
            //Inlcuindo paginação, skip para pegar a pagina atual e take para pegar o tamanho das paginas
            }).Skip(page*pageSize).Take(pageSize)
            .ToListAsync(); // retorna as propriedades com os relacionamentos inclusos

            return Ok(new ResultViewModel<dynamic>( new
            {
                total = count,
                page,
                pageSize,
                post
            }));             
        }catch(Exception ex)
        {
            return StatusCode(500, new ResultViewModel<List<Post>>("Falha interna no servidor"));
        }
        }

        [HttpGet("v1/posts/{id:int}")]
        public async Task<IActionResult> GetDetails(
            [FromServices] BlogDataContext context,
            [FromRoute]int id)
        {
            try{
               var result = await context.Posts.AsNoTracking()
                .Include(x=> x.Author)
                //.ThenInclude(x => x.Roles)
                .Include(x=> x.Category).FirstOrDefaultAsync(x=>x.Id == id);

                if(result == null)
                {
                    return NotFound(new ResultViewModel<Post>("Conteudo não encontrado") );
                }

                return Ok(new ResultViewModel<Post>(result));
            }catch(Exception ex)
            {
                return StatusCode(500,new ResultViewModel<Post>("Falha interna no servidor"));
            }

        }

        [HttpGet("v1/posts/category/{category}")]
        public async Task<IActionResult> GetCategory(
        [FromRoute] string category,
        [FromServices] BlogDataContext context,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10
        )
        {
            try{
                var count = await context.Posts.AsNoTracking().CountAsync();
                var result = await context.Posts.Include(x=> x.Author).Include(x=> x.Category)
                .Where(x=> x.Category.Slug == category).Select(x=> new ListPostViewModel{
                    Id = x.Id,  
                    Tittle = x.Title,
                    Slug = x.Slug,
                    Category = x.Category.Name,
                    LastUpdate = x.LastUpdateDate,
                    Author = $"{x.Author.Name},{x.Author.Email}",

                }).Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x=> x.LastUpdate)               
                .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(new {
                    total = count,
                    page,
                    pageSize,
                    result
                }));
            }catch(Exception ex)
            {
                return StatusCode(400, new ResultViewModel<List<Post>>("Erro interno no sistema"));
            }
        }
    }

    
