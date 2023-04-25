namespace RetAil.Dal.Entities;

public class ProductEntity : BaseEntity
{
    public string Title { get; set; }
    public string Details { get; set; }
    public decimal Price { get; set; }

    public UserEntity UserEntity { get; set; }

}