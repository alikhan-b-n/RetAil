using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RetAil.Bll.Dtos;
using RetAil.Bll.Services.Abstract;
using RetAil.Contracts.Options;
using RetAil.Dal.Entities;
using RetAil.Dal.Providers.Abstract;

namespace RetAil.Bll.Services;

public class AuthService : IAuthService
{
    private readonly IUserProvider _userProvider;
    private readonly IUserService _userService;
    private readonly SecretOptions _secretOptions;

    public AuthService(IUserProvider userProvider, IOptions<SecretOptions> secretOptions, IUserService userService)
    {
        _userProvider = userProvider;
        _userService = userService;
        _secretOptions = secretOptions.Value;
    }

    public async Task<string> SignUp(UserSignUpDto userSignUpDto)
    {
        if (await _userProvider.GetByLogin(userSignUpDto.Login) != null) 
            throw new ArgumentException("error, login already been taken");
        
        await _userProvider.Create(new UserEntity
        {
            Login = userSignUpDto.Login,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(userSignUpDto.Password),
            Username = userSignUpDto.Username
        });
        
        return GenerateToken(userSignUpDto.Login, userSignUpDto.Username);
    }

    public async Task<string> SignIn(UserSignInDto userSignInDto)
    {
        var userEntity = await _userProvider.GetByLogin(userSignInDto.Login);
        if (userEntity == null) throw new ArgumentException("error, login is not found");
        var user = userEntity;
        
        if (BCrypt.Net.BCrypt.Verify(userSignInDto.Password, user?.PasswordHash))
        {
            return GenerateToken(userSignInDto.Login, user?.Username);
        }
        
        throw new ArgumentException("error, passport is not correct");
    }

    public async Task<UserDto> GetUserByHeader(string[] headers)
    {
        var token = headers[0].Replace("Bearer ", "");
        var login = DecryptToken(token).Login;

        return await _userService.GetUserByLogin(login);
    } 

    private string GenerateToken(string? login, string? username)
    {
        var key = Encoding.ASCII.GetBytes(_secretOptions.JwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, login)
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256)
        };
    
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }
    
    private (string Login, string Username) DecryptToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        var tokenS = handler.ReadToken(token) as JwtSecurityToken;

        if (tokenS?.Claims is List<Claim> claims)
        {
            return new ValueTuple<string, string>(claims[0].Value, claims[1].Value);
        }

        throw new ArgumentException();
    }
}