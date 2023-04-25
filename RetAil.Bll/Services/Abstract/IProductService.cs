using RetAil.Bll.Dtos;

namespace RetAil.Bll.Services.Abstract;

public interface IProductService
{
    Task<Guid> Create(ProductDto categoryDto);
    Task<ProductDto> GetById(Guid id, Guid userId);
    Task<List<ProductDto>> GetAll(Guid userId);
    Task Update(ProductDto categoryDto);
    Task Delete(Guid id, Guid userId);
}