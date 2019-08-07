using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.Models.WishListViewModel
{
    public class WishListIndexViewModel
    {
        public List<WishList> WishList { get; set; }
        public WishList wishList { get; set; }
        public Book Book { get; set; }
    }
}
