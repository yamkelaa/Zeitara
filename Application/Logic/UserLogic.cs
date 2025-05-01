using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using System.Diagnostics;

namespace Application.Logic;

public class UserLogic(IUnitOfWork _unitOfWork) : IUserLogic
{
    public async Task<LoginResponseDto?> Login(LoginRequestDto loginRequest)
    {
        var user = await _unitOfWork.Users.FindAsync(u => u.Username == loginRequest.Username);
        Debug.WriteLine(user);
        var matchedUser = user.FirstOrDefault();
        if (matchedUser == null || matchedUser.User_Password != loginRequest.Password)
        {
            return null;
        }

        return new LoginResponseDto
        {
            User_Id = matchedUser.User_Id,
            Username = matchedUser.Username,
            FullName = $"{matchedUser.User_Firstname} {matchedUser.User_Lastname}",
        };
    }

    public async Task<RegistrationResponseDto> Register(RegistrationRequestDto request)
    {
        var existingUser = await _unitOfWork.Users.FindAsync(u => u.Username == request.Username);
        if (existingUser.Any())
        {
            return new RegistrationResponseDto
            {
                Success = false,
                Message = "Username already exists."
            };
        }

        var user = new User
        {
            Username = request.Username,
            User_Firstname = request.User_FirstName,
            User_Lastname = request.User_LastName,
            User_Password = request.Password,
            Age = request.Age,
            Gender = request.Gender,
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        
        var address = new Address
        {
            Street = request.Street,
            Suburb = request.Suburb,
            Province = request.Province,
            Postal_Code = request.Postal_Code,
            City = request.City,
            Country = request.Country,
            User_Id = user.User_Id
        };

        await _unitOfWork.Addresses.AddAsync(address);
        await _unitOfWork.SaveChangesAsync();

        return new RegistrationResponseDto
        {
            Success = true,
            Message = "Registration successful.",
            User_Id = user.User_Id,
            Username = user.Username
        };
    }

}
