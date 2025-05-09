namespace Domain.Entities
{
    public class ProductLike
    {
        public int LikeId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }
}
