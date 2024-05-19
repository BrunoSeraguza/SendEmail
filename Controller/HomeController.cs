using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using blogapi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace blogapi.Controller
{
    [ApiController]
    [Route("")]
    [ApiKey]
    public class HomeController:ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        => Ok( "HOMOLOGACAO");

        [HttpPost]
        public IActionResult Post()
        => Ok();
    }
}