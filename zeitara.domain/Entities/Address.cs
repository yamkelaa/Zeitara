namespace Domain.Entities;

public class Address
{
    public int Address_Id { get; set; }
    public int User_Id { get; set; }
    public required string Street { get; set; }
    public required string Suburb { get; set; }
    public required string Province { get; set; }
    public required string Postal_Code { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }

}
