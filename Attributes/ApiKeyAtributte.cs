using Microsoft.AspNetCore.Mvc.Filters;
using blogapi.Controller;
using Microsoft.AspNetCore.Mvc;

namespace blogapi.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    
{
    public async Task OnActionExecutedAsync(ActionExecutedContext context, ActionExecutionDelegate next)
    {
          throw new NotImplementedException();
    }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {          
               if(!context.HttpContext.Request.Query.TryGetValue(Configuration.ApiKeyName, out var extratedApikey)){
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = "Api não encontrada"
            };
            return;         
        }

        if(!Configuration.ApiKey.Equals(extratedApikey))
        {
            context.Result = new ContentResult(){
                StatusCode = 403,
                Content ="Acesso não autorizado"
            };
            return;
        }
        await next();
        }
    }
}