using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using System.Diagnostics;

namespace Application.Logic;

public class UserLogic(IUnitOfWork _unitOfWork) : IUserLogic
{
    public async Task<LoginResponseDto?> Login(LoginRequestDto loginRequest)
    {
        var user = await _unitOfWork.Users.FindAsync(u => u.Username == loginRequest.Username);
        Debug.WriteLine(user);
        var matchedUser = user.FirstOrDefault();
        if (matchedUser == null)
        {
            return null;
        }
        if (matchedUser.User_Password != loginRequest.Password)
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
}