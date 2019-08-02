using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.Models.BookViewModel
{
    public class BookCreateViewModel
    {
        public Book Book { get; set; }

        public List<SelectListItem> BookTypeList { get; set; }
        public List<SelectListItem> AuthorList { get; set; }
    }
}
