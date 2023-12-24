using App.API.Configuration.Authorization;
using App.API.Modules.Categories.Requests;
using App.Modules.Categories.Application.Categories.EditCategory;
using App.Modules.Categories.Application.Categories.GetCategories;
using App.Modules.Categories.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.Categories;

[Authorize]
[Route("categories")]
[ApiController]
public class CategoriesController(ICategoriesModule categoriesModule) : ControllerBase
{
    [HttpGet]
    [NoPermissionRequired]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList()
    {
        var categories = await categoriesModule.ExecuteQueryAsync(new GetCategoriesQuery());

        return Ok(categories);
    }

    [HttpPatch("{categoryId}")]
    [HasPermission(CategoriesPermissions.ManageCategories)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Edit(Guid categoryId, EditCategoryRequest request)
    {
        await categoriesModule.ExecuteCommandAsync(new EditCategoryCommand(categoryId, request.Title, request.IconUrl));

        return Accepted();
    }
}
