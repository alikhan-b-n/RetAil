using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RetAil.Api.VIewModels;
using RetAil.Api.VIewModels.Params;
using RetAil.Api.VIewModels.Responses;
using RetAil.Bll.Dtos;
using RetAil.Bll.Services;
using RetAil.Bll.Services.Abstract;

namespace RetAil.Api.Controllers;
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IAuthService _authService;

    public ProductController(IProductService productService, IAuthService authService)
    {
        _productService = productService;
        _authService = authService;
    }

    [HttpGet("api/products")]
    public async Task<IActionResult> GetAll()
    {
        var user = await _authService.GetUserByHeader(Request.Headers[HeaderNames.Authorization]!);
        try
        {
            var products = await _productService.GetAll(user.Id);
            var response = products.Select(x => new ProductResponse(x.Price,x.Title, x.Details, x.Id));
            return Ok(response);
        }
        catch
        {
            return NotFound("Products are not found");
        }
    }

    [HttpGet("api/products/{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var user = await _authService.GetUserByHeader(Request.Headers[HeaderNames.Authorization]!);
        try
        {
            var product = await _productService.GetById(id, user.Id);
            return Ok(new ProductResponse(product.Price, product.Title, product.Details, product.Id));
        }
        catch(ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("api/products")]
    public async Task<IActionResult> Create([FromBody] ProductParams productParams)
    {
        var User = await _authService.GetUserByHeader(Request.Headers[HeaderNames.Authorization]!);
        var id = await _productService.Create(new ProductDto
        {
            Title = productParams.Title,
            Details = productParams.Details,
            Price = productParams.Price,
            UserId = User.Id
        });
        return Ok(new 
        {
            Id = id
        });
    }

    [HttpPut("api/products/{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ProductParams productParams)
    {
        var User = await _authService.GetUserByHeader(Request.Headers[HeaderNames.Authorization]!);
        try
        {
            await _productService.Update(new ProductDto
            {
                Id = id,
                Title = productParams.Title,
                Details = productParams.Details,
                Price = productParams.Price,
                UserId = User.Id
            });
            return Ok("Success");
        }
        catch
        {
            return NotFound($"Product with {id} is not found");
        }
    }

    [HttpDelete("api/products/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var User = await _authService.GetUserByHeader(Request.Headers[HeaderNames.Authorization]!);
        try
        {
            await _productService.Delete(id, User.Id);
            return Ok("Success");
        }
        catch
        {
            return NotFound($"Product with {id} is not found");
        }
    }
}