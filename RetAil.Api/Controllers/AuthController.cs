using Microsoft.AspNetCore.Mvc;
using RetAil.Api.VIewModels;
using RetAil.Bll.Dtos;
using RetAil.Bll.Services.Abstract;

namespace RetAil.Api.Controllers;

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("api/user/signup")]
    public async Task<IActionResult> Register([FromBody] UserSignUpViewModel userSignUpViewModel)
    {
        return Ok(await _authService.SignUp(new UserSignUpDto
        {
            Username = userSignUpViewModel.Username,
            Login = userSignUpViewModel.Login,
            Password = userSignUpViewModel.Password
        }));
    }
}