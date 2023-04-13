using Microsoft.EntityFrameworkCore;
using RetAil.Dal.Entities;
using RetAil.Dal.Providers.Abstract;

namespace RetAil.Dal.Providers.EntityFramework;

public class UserProvider : BaseProvider<UserEntity>, IUserProvider
{
    private readonly ApplicationContext _applicationContext;

    public UserProvider(ApplicationContext applicationContext) : base(applicationContext)
    {
        _applicationContext = applicationContext;
    }


    public async Task<UserEntity?> GetByLogin(string? login)
    {
        return await _applicationContext.Users.FirstOrDefaultAsync(x => x.Login == login);
    }

    public async Task<UserEntity?> GetByPassword(string passwordHash)
    {
        return await _applicationContext.Users.FirstOrDefaultAsync(x => x.PasswordHash==passwordHash);
    }
}