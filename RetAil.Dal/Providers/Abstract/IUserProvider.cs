using RetAil.Dal.Entities;

namespace RetAil.Dal.Providers.Abstract;

public interface IUserProvider : ICrudProvider<UserEntity>
{
    Task<UserEntity?> GetByLogin(string login);
    Task<UserEntity?> GetByPassword(string passwordDash);
}

