using Microsoft.EntityFrameworkCore;
using RetAil.Dal.Entities;
using RetAil.Dal.Providers.Abstract;

namespace RetAil.Dal.Providers.EntityFramework;

public class CategoryProvider : BaseProvider<CategoryEntity>, ICategoryProvider
{
    private readonly ApplicationContext _applicationContext;

    public CategoryProvider(ApplicationContext applicationContext) : base(applicationContext)
    {
        this._applicationContext = applicationContext;
    }

    public async Task<List<CategoryEntity>> GetAllById(Guid id)
    {
        return await _applicationContext.CategoryEntities.Where(x => x.UserEntity.Id == id).ToListAsync();
    }
}