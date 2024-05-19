using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace blogapi.ViewModel
{
    public class EditorCreateCategoryViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(40, ErrorMessage ="O nome deve conter de 3 a 40 characteres",MinimumLength = 3)]
        public string Name { get; set; } 
        [Required(ErrorMessage ="O slug é obrigatório")]
        [StringLength(40 , ErrorMessage ="O slug deve conter de 5 a 40 characteres",MinimumLength = 5)]
        public string  Slug { get; set; }
    }
}