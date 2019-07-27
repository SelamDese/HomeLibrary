using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.Models
{
    public class Borrow
    {
        [Key]
        public int BorrowId { get; set; }
        public DateTime? DateBorrowed { get; set; }
        public DateTime? DateReturned { get; set; } 
        public Book books { get; set; }
        public int BookId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public ApplicationUser User { get; set; }
    }
}
