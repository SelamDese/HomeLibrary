using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.Models
{
    public class GroupedBooks
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int BookCount { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}