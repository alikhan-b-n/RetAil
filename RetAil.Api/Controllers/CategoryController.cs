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

public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IAuthService _authService;

    public CategoryController(ICategoryService categoryService, IAuthService authService)
    {
        _categoryService = categoryService;
        _authService = authService;
    }

    [HttpGet("api/categories")]
    public async Task<IActionResult> GetAll()
    {
        var user = await _authService.GetUserByHeader(Request.Headers[HeaderNames.Authorization]!);
        try
        {
            var success = await _categoryService.GetAll(user.Id);
            var response = success.Select(x => new CategoryResponse(x.Name, x.Id));
            return Ok(response);
        }
        catch (ArgumentException e)
        {
            return NotFound($"Categories are not found");
        }
    }

    [Authorize]
    [HttpGet("api/categories/{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var User = await _authService.GetUserByHeader(Request.Headers[HeaderNames.Authorization]!);
        try
        {
            var success = await _categoryService.GetById(id, User.Id);
            return Ok(new CategoryResponse(success.Name, success.Id));
        }
        catch (ArgumentException e)
        {
            return NotFound($"Category {id} is not found");
        }
    }

    [Authorize]
    [HttpPost("api/categories")]
    public async Task<IActionResult> Create([FromBody] CategoryParameter categoryParameter)
    {
        var userId = await _authService.GetUserByHeader(Request.Headers[HeaderNames.Authorization]!);
        var id = await _categoryService.Create(new CategoryDto
        {
            Name = categoryParameter.Name,
            UserId = userId.Id
        });
        return Ok(new
        {
            Id = id
        });
    }
    [Authorize]
    [HttpDelete("api/categories/{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var User = await _authService.GetUserByHeader(Request.Headers[HeaderNames.Authorization]!);
        try
        {
            await _categoryService.Delete(id, User.Id);
            return Ok("Success");
        }
        catch (ArgumentException e)
        {
            return NotFound($"Category {id} is not found");
        }
    }
    [Authorize]
    [HttpPut("api/categories/{id:guid}")]
    public async Task<IActionResult> Update(CategoryParameter categoryParameter, [FromRoute] Guid id)
    {
        var user = await _authService.GetUserByHeader(Request.Headers[HeaderNames.Authorization]!);
        try
        {
            await _categoryService.Update(new CategoryDto
            {
                Name = categoryParameter.Name,
                Id = id,
                UserId = user.Id
                
            });
            return Ok("Success");
        }
        catch (ArgumentException e)
        {
            return NotFound($"Category {id} is not found");
        }
    }

    [Authorize]
    [HttpGet("api/categories/{id:guid}/products")]
    public async Task<IActionResult> GetAllProducts([FromRoute] Guid id)
    {
        var user = await _authService.GetUserByHeader(Request.Headers[HeaderNames.Authorization]!);
        var products = await _categoryService.GetAllProducts(id, user.Id);
        return Ok(products.Select(x =>
            new ProductResponse(x.Price, x.Title, x.Details,x.Id)));
    }
}