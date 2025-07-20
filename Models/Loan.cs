using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models
{
    public class Loan
    {
        [Key]
        public int LoanId { get; set; }

        // Foreign keys
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public Book ?Book { get; set; }

        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public Member ?Member { get; set; }

        public DateTime LoanDate { get; set; }

        public DateTime? ReturnDate { get; set; }
    }
}
