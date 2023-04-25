using RetAil.Bll.Dtos;

namespace RetAil.Bll.Services.Abstract;

public interface ICategoryService
{ 
    Task<Guid> Create(CategoryDto categoryDto);
    Task<CategoryDto> GetById(Guid id, Guid userId);
    Task<List<CategoryDto>> GetAll(Guid userId);
    Task Update(CategoryDto categoryDto);
    Task Delete(Guid id, Guid userId);
    
}