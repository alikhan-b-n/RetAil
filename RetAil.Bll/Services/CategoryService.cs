using RetAil.Bll.Dtos;
using RetAil.Bll.Services.Abstract;
using RetAil.Dal.Entities;
using RetAil.Dal.Providers.Abstract;
using RetAil.Dal.Providers.EntityFramework;

namespace RetAil.Bll.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryProvider _categoryProvider;
    private readonly IUserProvider _userProvider;

    public CategoryService(ICategoryProvider categoryProvider, IUserProvider userProvider)
    {
        _categoryProvider = categoryProvider;
        _userProvider = userProvider;
    }


    public async Task<Guid> Create(CategoryDto categoryDto)
    {
        var userEntity = await _userProvider.GetById(categoryDto.UserId);
        var categoryEntity = new CategoryEntity
        {
            Name = categoryDto.Name,
            UserEntity = userEntity,
        };

        await _categoryProvider.Create(categoryEntity);
        return categoryEntity.Id;
    }


    public async Task<CategoryDto> GetById(Guid id, Guid userId)
    {
        try
        {
            var category = await _categoryProvider.GetById(id);
            if (category.UserEntity.Id != userId)
            {
                throw new ArgumentException("You don't have access");
            }

            return new CategoryDto { Name = category.Name, Id = category.Id, UserId = category.UserEntity.Id };
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException(e.Message);
        }
    }

    public async Task<List<CategoryDto>> GetAll(Guid userId)
    {
        var categories = await _categoryProvider.GetAllById(userId);
        return categories.Select(x => new CategoryDto { Name = x.Name, Id = x.Id, UserId = x.UserEntity.Id }).ToList();
    }

    public async Task Update(CategoryDto categoryDto)
    {
        try
        {
            var categoryEntity = await _categoryProvider.GetById(categoryDto.Id);
            if (categoryEntity.UserEntity.Id != categoryDto.UserId)
            {
                throw new ArgumentException("You don't have access");
            }

            categoryEntity.Name = categoryDto.Name;
            await _categoryProvider.Update(categoryEntity);
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException(e.Message);
        }
    }

    public async Task Delete(Guid id, Guid userId)
    {
        try
        {
            var categoryEntity = await _categoryProvider.GetById(id);
            if (categoryEntity.UserEntity.Id != userId)
            {
                throw new ArgumentException("You don't have access");
            }

            await _categoryProvider.Delete(id);
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException(e.Message);
        }
    }

    public async Task<List<ProductDto>> GetAllProducts(Guid id, Guid userId)
    {
        try
        {
            var productsEntities = await _categoryProvider.GetAllProducts(id, userId);
            return productsEntities.Select(x => new ProductDto
                    { Details = x.Details, Id = x.Id, Price = x.Price, Title = x.Title, UserId = x.UserEntity.Id })
                .ToList();
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException(e.Message);
        }
    }
}