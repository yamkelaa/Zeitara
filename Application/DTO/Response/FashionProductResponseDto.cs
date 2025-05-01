namespace Application.DTO.Response;

public class FashionProductResponseDto
{
    public required int Id { get; set; }
    public required string Gender { get; set; }
    public required string MasterCategory { get; set; }
    public required string SubCategory { get; set; }
    public required string ArticleType { get; set; }
    public required string BaseColour { get; set; }
    public required string Season { get; set; }
    public short? Year { get; set; }
    public string? Usage { get; set; }
    public required string ProductDisplayName { get; set; }
    public required string ImageUrl { get; set; }
}
