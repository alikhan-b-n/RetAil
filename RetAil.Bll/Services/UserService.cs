using RetAil.Bll.Dtos;
using RetAil.Bll.Services.Abstract;
using RetAil.Dal.Providers.Abstract;

namespace RetAil.Bll.Services;

public class UserService : IUserService
{
    private readonly IUserProvider _userProvider;

    public UserService(IUserProvider userProvider)
    {
        _userProvider = userProvider;
    }

    public async Task<UserDto> GetUserById(Guid id)
    {
        var user = await _userProvider.GetById(id);

        return new UserDto(user.Id, user.Login, user.Username);
    }

    public async Task<UserDto> GetUserByLogin(string login)
    {
        var user = await _userProvider.GetByLogin(login);
        if (user == null) throw new Exception("user not found");
        
        return new UserDto(user.Id, user.Login, user.Username);
    }
}