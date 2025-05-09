using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public required string Comment { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }

        public int? PurchaseId { get; set; }
    }
}

