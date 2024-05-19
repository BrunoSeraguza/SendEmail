using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Models;
using blogapi.Extensios;
using blogapi.ViewModel;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace blogapi.Controller
{
    [ApiController]
    //[Route("[v1/categories]")]
    public class CategoryController : ControllerBase
    {
       [HttpGet("v1/categories/{id:int}")]
       public  async Task<IActionResult> GetById([FromServices]BlogDataContext context, [FromRoute] int id)
       {
        try{
          var category =  await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

          if(category == null)
               return NotFound(new ResultViewModel<Category>("Categoria nao encontrada"));

          return Ok(new ResultViewModel<Category>(category));
        }catch
        {
          return StatusCode(500,new ResultViewModel<Category>("registro nao encontrado"));
        }
       }

       [HttpPost("v1/categories")]
       public async Task<IActionResult> PostAsync([FromServices] BlogDataContext context, [FromBody] EditorCreateCategoryViewModel model)
       {
        try{
          if(!ModelState.IsValid)
            return NotFound(new ResultViewModel<Category>(ModelState.GetErrors()));

          var category = new Category{
            Id = 0,
            Name = model.Name,
            Slug = model.Slug
          };

         await context.Categories.AddAsync(category);
         await context.SaveChangesAsync();

          return Created($"v1/categories/{category.Id}",new ResultViewModel<Category>(category));
        }
        catch(DbUpdateException ex)
        {
          return StatusCode(500,new ResultViewModel<Category>("01EXC1 - Não foi possivel adicionar uma categoria"));
        }
        
       }

       [HttpPut("v1/categories/{id:int}")]
       public async Task<IActionResult> PutAsync(
         [FromServices] BlogDataContext context,
         [FromRoute] int id,
         [FromBody] Category model)
       {
    
        try{
         var category = await context.Categories.FirstOrDefaultAsync(x=> x.Id == id);

         if(category == null)
               return NotFound(new ResultViewModel<Category>(ModelState.GetErrors()));

         category.Name = model.Name;
         category.Slug = model.Slug;

         context.Categories.Update(category);
         await context.SaveChangesAsync();

         return Ok();
        } catch(DbUpdateException ex)
        {
          return StatusCode(500,new ResultViewModel<Category>("02EXC2 - Não foi possivel alterar uma categoria"));
        }
        catch(Exception ex)
        {//por questao de trakingg podemos colocar esse codigo nas Exception , ou seja quando der esse erro, se informar o codigo podemos procurar com o ctrl+f 
          return StatusCode(500,new ResultViewModel<Category>(" 02EXC3 - Erro interno do sistema"));
        }
         
       }

       [HttpDelete("v1/categories/{id:int}")]
       public async Task<IActionResult> DeleteAsync([FromServices] BlogDataContext context, [FromRoute] int id )
       { 
        try{
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if(category == null)
                  return NotFound();

            context.Categories.Remove(category);
            await context.SaveChangesAsync();      

            return Ok();
        }
              catch(DbUpdateException ex)
        {
          return StatusCode(500,new ResultViewModel<Category>("03EXC1 - Não foi possivel excluir uma categoria"));
        }
        catch(Exception ex)
        {//por questao de trakingg, voce jovem padawan pode colocar esse codigo nas Exception
         // ou seja quando der esse erro, se informar o codigo podemos procurar com o ctrl+f 
          return StatusCode(500,new ResultViewModel<Category>(" 03EXC2 - Erro interno do sistema"));
        }
       }

       [HttpGet("v1/categories/")]
       public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context)
       {
       // User.Identity.IsAuthenticated;
        try{
          var categories = await context.Categories.ToListAsync();

          return Ok(new ResultViewModel<List<Category>>(categories));
        }catch
        {
          return StatusCode(500, new ResultViewModel<Category>(
            "Error de comunicacao com o servidor"));
        }
       }
    }
}