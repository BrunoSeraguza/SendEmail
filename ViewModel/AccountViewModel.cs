using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace blogapi.ViewModel
{
    public class AccountViewModel
    {
        [Required(ErrorMessage ="O nome é obrigatorio")]
        [MaxLength(40,ErrorMessage ="o nome deve ter no maximo 40 char")]
        [MinLength(5, ErrorMessage ="o nome deve ter no minimo 5 char")]
        public string Nome { get; set; }    


        [Required(ErrorMessage ="O email é obrigatorio")]
        public string Email { get; set; }
    }
}