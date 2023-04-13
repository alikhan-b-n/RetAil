using RetAil.Bll.Dtos;

namespace RetAil.Bll.Services.Abstract;

public interface IUserService
{
    Task<UserDto> GetUserById(Guid id);
    Task<UserDto> GetUserByLogin(string login);
}