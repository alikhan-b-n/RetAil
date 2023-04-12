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
    private readonly SecretOptions _secretOptions;

    public AuthService(IUserProvider userProvider, IOptions<SecretOptions> secretOptions)
    {
        _userProvider = userProvider;
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
        if (_userProvider.GetByLogin(userSignInDto.Login) != _userProvider.GetByPassword(userSignInDto.Password))
        {
            throw new ArgumentException("error, passport is not correct");
        }
        if(_userProvider.GetByLogin(userSignInDto.Login)==null)
        {
            throw new ArgumentException("error, Login is not found");
        }
        
        return GenerateToken(userSignInDto.Login, userSignInDto.Username);

    }

    private string GenerateToken(string login, string username)
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
}