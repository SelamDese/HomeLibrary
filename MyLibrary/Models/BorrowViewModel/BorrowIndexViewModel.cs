using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.Models.BorrowViewModel
{
    public class BorrowIndexViewModel
    {
        public List<Book> Book { get; set; }
        public List<Borrow> Borrow { get; set; }
        public Borrow borrow { get; set; }
        public Book book { get; set; }
    }
}
