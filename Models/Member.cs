using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class Member
    {
        [Key]
        public int MemberId { get; set; }

        [Required]
        public string ?FullName { get; set; }

        public string ?Email { get; set; }

        public string ?Phone { get; set; }

        public ICollection<Loan> ?Loans { get; set; }
    }
}
