using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.Models.BookViewModel
{
    public class BookCategoriesViewModel
    {
        public List<GroupedBooks> GroupedBooks { get; set; }
        public Book book { get; set; }
    }
}
