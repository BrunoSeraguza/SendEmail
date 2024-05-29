using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace SendEmail.ViewModel.Posts
{
    public class ListPostViewModel
    {
        public int Id { get; set; }
        public string Tittle { get; set; }
        public string  Slug { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Category  { get; set; }
        public string Author { get; set; }
    }
}