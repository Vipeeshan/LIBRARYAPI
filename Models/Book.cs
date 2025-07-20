using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        public string ?Title { get; set; }

        public string ?Author { get; set; }

        public DateTime PublishedDate { get; set; }

        // Foreign key
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category ?Category { get; set; }

        public ICollection<Loan> ?Loans { get; set; }
    }
}
