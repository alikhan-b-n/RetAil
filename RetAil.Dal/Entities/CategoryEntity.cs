namespace RetAil.Dal.Entities;

public class CategoryEntity : BaseEntity
{
    public string Name { get; set; }
    
    public UserEntity UserEntity { get; set; }

}