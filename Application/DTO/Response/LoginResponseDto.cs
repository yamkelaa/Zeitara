namespace Application.DTO.Response;

public class LoginResponseDto
{
    public int User_Id { get; set; }
    public required string Username { get; set; }
    public required string FullName { get; set; }
}
