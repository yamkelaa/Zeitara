namespace Application.DTO.Request
{
    public class ReviewRequestDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public required string Comment { get; set; }
        public int? PurchaseId { get; set; }
    }
}

