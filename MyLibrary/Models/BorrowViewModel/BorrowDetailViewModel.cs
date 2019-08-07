using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.Models.BorrowViewModel
{
    public class BorrowDetailViewModel
    {
        public Borrow borrow { get; set; }
        public WishList wishLists { get; set; }
        public Book Book { get; set; }
        public bool IsBorrowed { get; set; }
        public bool IsWishListed { get; set; }
    }
}
