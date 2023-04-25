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
            UserEntity = userEntity
        };

        await _categoryProvider.Create(categoryEntity);
        return categoryEntity.Id;
    }


    public async Task<CategoryDto> GetById(Guid id, Guid userId)
    {
        try
        {
            var category = await _categoryProvider.GetById(id); 
            if(userId==category.UserEntity.Id) return new CategoryDto { Name = category.Name, Id = category.Id, UserId = category.UserEntity.Id };
            throw new ArgumentException("You don't have access");
        }
        catch
        {
            throw new ArgumentException("error, not found");
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
            if(categoryEntity.UserEntity.Id == categoryDto.UserId)
            {
                categoryEntity.Name = categoryDto.Name;
                await _categoryProvider.Update(categoryEntity);
            }
        }
        catch
        {
            throw new ArgumentException("error");
        }
    }

    public async Task Delete(Guid id, Guid userId)
    {
        try
        {
            var categoryEntity = await _categoryProvider.GetById(id);
            if(categoryEntity.UserEntity.Id == userId) await _categoryProvider.Delete(id);
        }
        catch
        {
            throw new ArgumentException("error");
        }
    }
}