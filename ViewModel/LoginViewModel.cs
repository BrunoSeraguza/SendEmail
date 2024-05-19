using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApi.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Usuario Obrigatorio")]
        [EmailAddress(ErrorMessage = "Email invalido")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Senha Obrigatoria")]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}