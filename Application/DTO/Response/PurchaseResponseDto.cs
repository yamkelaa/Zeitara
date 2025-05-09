namespace Application.DTO.Response
{
    public class PurchaseResponseDto
    {
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public required string Status { get; set; }
        public DateTime PurchaseDate { get; set; }
        public required string ProductDisplayName { get; set; }
        public required string ImageUrl { get; set; }
        public bool IsLiked { get; set; }
        public bool IsInCart { get; set; }
        public bool IsPurchased { get; set; }
        public decimal? UserRating { get; set; }
    }

}
