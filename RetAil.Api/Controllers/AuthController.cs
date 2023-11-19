using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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
        string success;
        try
        {
            success = await _authService.SignUp(new UserSignUpDto
            {
                Username = userSignUpViewModel.Username,
                Login = userSignUpViewModel.Login,
                Password = userSignUpViewModel.Password
            });
        }
        catch (Exception e)
        {
            return BadRequest("Error");
        }

        return Ok(success);
    }

    [HttpPost("api/user/signin")]
    public async Task<IActionResult> Login([FromBody] UserSignInViewModel userSignInViewModel)
    {
        string success;
        try
        {
            success = await _authService.SignIn(new UserSignInDto
            {
                Login = userSignInViewModel.Login,
                Password = userSignInViewModel.Password
            });
        }
        catch (Exception e)
        {
            return BadRequest("Not found");
        }

        return Ok(success);
    }


    [Authorize]
    [HttpGet("api/user/get")]
    public async Task<IActionResult> GetUser()
    {
        try
        {
            ///Save for future projects
            return Ok(await _authService.GetUserByHeader(Request.Headers[HeaderNames.Authorization]!));
        }
        catch (Exception e)
        {
            return NotFound("User is not found, wrong token");
        }
    }
}