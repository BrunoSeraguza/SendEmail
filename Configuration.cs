using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace blogapi.Controller
{

    public static class Configuration//smtp
    {
        public static string JwtToken { get; set; } = "sivbupasmd23n5k4bnt3btj4bt4";
        public static string ApiKeyName { get; set;} = "api_key";
        public static string ApiKey { get; set;} = "bruno_api_IHdji/KGTsfi/uNmg==";
        public static SmtpConfiguration Smtp = new();
        public static string ImageBase64 { get; set; } = "https://localhost:0000/Images/";

        public class SmtpConfiguration
        {
            public string Host { get;  set; } 
            public int Port { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
        }
    }
}