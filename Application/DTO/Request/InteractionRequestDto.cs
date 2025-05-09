namespace Application.DTO.Request
{
    public class InteractionRequestDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
    }
}
