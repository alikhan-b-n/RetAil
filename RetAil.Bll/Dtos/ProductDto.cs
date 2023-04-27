namespace RetAil.Bll.Dtos;

public class ProductDto
{
    public string Title { get; set; }
    public string Details { get; set; }
    public decimal Price { get; set; }
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }

    public Guid CategoryId { get; set; }
}