using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.Models.BookViewModel
{
    public class BookEditViewModel
    {
        public Book Book { get; set; }
        public List<Category> CategoryList { get; set; }
        public List<Author> AvailableAuthors { get; set; }
        public List<SelectListItem> CategoryOptions =>
            CategoryList?.Select(a => new SelectListItem(a.CategoryName, a.CategoryId.ToString())).ToList();
        public List<SelectListItem> AuthorOptions =>
            AvailableAuthors?.Select(a => new SelectListItem(a.FullName, a.AuthorId.ToString())).ToList();
    }
}
