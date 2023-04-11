using Microsoft.EntityFrameworkCore;
using RetAil.Dal.Entities;
using RetAil.Dal.Providers.Abstract;

namespace RetAil.Dal.Providers.EntityFramework;

public class CategoryProvider : BaseProvider<CategoryEntity>, ICategoryProvider
{
    private readonly ApplicationContext _applicationContext;

    public CategoryProvider(ApplicationContext _applicationContext) : base(_applicationContext)
    {
        this._applicationContext = _applicationContext;
    }
    
}