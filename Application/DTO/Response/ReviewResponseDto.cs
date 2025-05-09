namespace Application.DTO.Response
{
    public class ReviewResponseDto
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public required string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
