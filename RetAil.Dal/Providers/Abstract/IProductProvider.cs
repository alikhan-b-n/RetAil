using RetAil.Dal.Entities;

namespace RetAil.Dal.Providers.Abstract;

public interface IProductProvider : ICrudProvider<ProductEntity>
{
    public Task<List<ProductEntity>> GetAllById(Guid id);
}