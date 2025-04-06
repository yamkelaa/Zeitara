using Domain.Enum;

namespace Domain.Entities;
public class User
{
public int User_Id { get; set; }
public required string Username { get; set; }
public required string User_Firstname { get; set; }
public required string User_Lastname {  get; set; }
public required string User_Password { get; set; }
public int Age { get; set; }
public required string Gender { get; set; }
public required Address Address { get; set; }

}
