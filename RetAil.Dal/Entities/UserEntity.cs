namespace RetAil.Dal.Entities;

public class UserEntity : BaseEntity
{
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public string Username { get; set; }
}