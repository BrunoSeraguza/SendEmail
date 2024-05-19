using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;

namespace blogapi.Extensios
{
    public static class ExtensionRoleState
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var result = new List<Claim>{
                new (ClaimTypes.Name, user.Email)
            };

            result.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Slug) ));

            return result;

        }
    }
}