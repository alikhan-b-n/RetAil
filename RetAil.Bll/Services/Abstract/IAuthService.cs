using RetAil.Bll.Dtos;

namespace RetAil.Bll.Services.Abstract;

public interface IAuthService
{
    Task<string> SignUp(UserSignUpDto userSignUpDto);
    Task<string> SignIn(UserSignInDto userSignInDto); 
    Task<UserDto> GetUserByHeader(string[] headers);
}

