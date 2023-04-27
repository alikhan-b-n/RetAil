using RetAil.Bll.Dtos;
using RetAil.Bll.Services.Abstract;
using RetAil.Dal.Entities;
using RetAil.Dal.Providers.Abstract;

namespace RetAil.Bll.Services;

public class ProductService : IProductService
{
    private readonly IProductProvider _productProvider;
    private readonly IUserProvider _userProvider;
    private readonly ICategoryProvider _categoryProvider;

    public ProductService(IProductProvider productProvider, IUserProvider userProvider, ICategoryProvider categoryProvider)
    {
        _productProvider = productProvider;
        _userProvider = userProvider;
        _categoryProvider = categoryProvider;
    }

    public async Task<Guid> Create(ProductDto productDto)
    {
        var userEntity = await _userProvider.GetById(productDto.UserId);
        var categoryEntity = await _categoryProvider.GetById(productDto.CategoryId);
        ProductEntity? productEntity = new ProductEntity
        {
            Title = productDto.Title,
            Details = productDto.Details,
            Price = productDto.Price,
            UserEntity = userEntity,
            CategoryEntity = categoryEntity
        };
        await _productProvider.Create(productEntity);
        categoryEntity.ProductEntities.Add(productEntity);
        await _categoryProvider.Update(categoryEntity);
        return productEntity.Id;
    }

    public async Task<ProductDto> GetById(Guid id, Guid userId)
    {
        try
        {
            var product = await _productProvider.GetById(id);
            return userId == product.UserEntity.Id
                ? new ProductDto
                {
                    Price = product.Price, 
                    Details = product.Details, 
                    Title = product.Title, 
                    Id = product.Id
                }
                : throw new ArgumentException("You don't have access");
        }
        catch
        {
            throw new ArgumentException("error, not found");
        }
    }

    public async Task<List<ProductDto>> GetAll(Guid userId)
    {
        try
        {
            var products = await _productProvider.GetAllById(userId);
            return products.Select(x =>
                    new ProductDto
                        { Price = x.Price, Details = x.Details, Title = x.Title, Id = x.Id, UserId = x.UserEntity.Id })
                .ToList();
        }
        catch
        {
            throw new ArgumentException("error, not found");
        }
    }

    public async Task Update(ProductDto productDto)
    {
        try
        {
            var productEntity = await _productProvider.GetById(productDto.Id);
            if (productDto.UserId == productEntity.UserEntity.Id)
            {
                productEntity.Price = productDto.Price;
                productEntity.Details = productDto.Details;
                productEntity.Title = productDto.Title;
                await _productProvider.Update(productEntity);
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
            var productEntity = await _productProvider.GetById(id);
            if (userId == productEntity.UserEntity.Id)
                await _productProvider.Delete(id);
        }
        catch
        {
            throw new ArgumentException("error");
        }
    }
}