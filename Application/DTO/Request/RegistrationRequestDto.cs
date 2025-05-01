namespace Application.DTO.Request;

public class RegistrationRequestDto
{
    public required string Username {  get; set; }
    public required string User_FirstName { get; set; }
    public required string User_LastName { get; set; }
    public required string Password { get; set; }
    public int Age { get; set; }
    public required string Gender { get; set; }
    public required string Street { get; set; }
    public required string Suburb { get; set; }
    public required string Province { get; set; }
    public required string Postal_Code { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
}
