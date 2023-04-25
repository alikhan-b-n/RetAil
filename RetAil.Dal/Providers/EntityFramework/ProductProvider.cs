using Microsoft.EntityFrameworkCore;
using RetAil.Dal.Entities;
using RetAil.Dal.Providers.Abstract;

namespace RetAil.Dal.Providers.EntityFramework;

public class ProductProvider : BaseProvider<ProductEntity>, IProductProvider
{
    private readonly ApplicationContext _applicationContext;

    public ProductProvider(ApplicationContext applicationContext) : base(applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<List<ProductEntity>> GetAllById(Guid id)
    {
        return await _applicationContext.ProductEntities.Where(x => x.UserEntity.Id == id).ToListAsync();
    }
    
}