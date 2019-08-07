using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.Models.BorrowViewModel
{
    public class BorrowIndexViewModel
    {
        public List<Borrow> Borrow { get; set; }
        public Borrow borrow { get; set; }
        public Book Book { get; set; }
    }
}
