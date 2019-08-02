using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.Models
{
    public class WishList
    {
        [Key]
        public int WishListId { get; set; }
        public int BookId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public Book book { get; set; }
        public ApplicationUser User { get; set; }
    }
}
