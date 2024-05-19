using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;

namespace blogapi.Extensios
{
    static class ExtensionModelState
    {
       public static List<string> GetErrors(this ModelStateDictionary modelState)
       {
          var result = new List<string>();

          foreach(var error in modelState.Values)
          {
            foreach(var erro in error.Errors)
            {
                 result.Add(erro.ErrorMessage);
            }
          }
          return result;
       }
    }
}