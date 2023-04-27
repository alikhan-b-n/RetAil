namespace RetAil.Bll.Dtos;

public class CategoryDto
{
    public string Name { get; set; }
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    
    public List<ProductDto> ProductDtos { get; set; }
}