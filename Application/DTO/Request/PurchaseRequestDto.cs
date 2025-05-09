using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class PurchaseRequestDto : InteractionRequestDto
    {
        [Required]
        public new int Quantity { get; set; } 
    }
}
