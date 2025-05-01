using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserLogic _userLogic) : ControllerBase
{
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        var result = await _userLogic.Login(loginDto);
        if (result == null)
        {
            return Unauthorized();
        }
        return Ok(new ApiResponse<LoginResponseDto>(result));
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationDto)
    {
        var result = await _userLogic.Register(registrationDto);
        return Ok(new ApiResponse<RegistrationResponseDto>(result));
    }

}
