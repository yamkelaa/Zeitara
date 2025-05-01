using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IUserLogic
    {
        Task<LoginResponseDto?> Login(LoginRequestDto loginRequest);
        Task<RegistrationResponseDto> Register(RegistrationRequestDto request);
    }
}
