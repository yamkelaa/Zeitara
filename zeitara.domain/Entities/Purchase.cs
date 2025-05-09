using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Completed";

        [Required]
        public DateTime PurchaseDate { get; set; }
    }
}
