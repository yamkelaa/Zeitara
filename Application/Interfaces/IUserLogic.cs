using Application.DTO.Request;
using Application.DTO.Response;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserLogic
    {
        /// <summary>
        /// Authenticates a user with provided credentials
        /// </summary>
        /// <param name="loginRequest">Contains username and password</param>
        /// <returns>Login response with user details if successful, null otherwise</returns>
        Task<LoginResponseDto?> Login(LoginRequestDto loginRequest);

        /// <summary>
        /// Registers a new user with provided details
        /// </summary>
        /// <param name="request">Contains all required user registration data</param>
        /// <returns>Registration response with success status and user details</returns>
        Task<RegistrationResponseDto> Register(RegistrationRequestDto request);
    }
}