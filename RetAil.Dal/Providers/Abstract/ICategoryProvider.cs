using RetAil.Dal.Entities;

namespace RetAil.Dal.Providers.Abstract;

public interface ICategoryProvider : ICrudProvider<CategoryEntity>
{
    public Task<List<CategoryEntity>> GetAllById(Guid id);

    public Task<List<ProductEntity>> GetAllProducts(Guid id, Guid userId) ;
}