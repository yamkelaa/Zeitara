namespace Application.DTO.Response;

public class RegistrationResponseDto
{
    public bool Success { get; set; }
    public required string Message { get; set; } = string.Empty;
    public int? User_Id { get; set; }
    public string? Username { get; set;}
}
