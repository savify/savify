using App.API.Configuration.Authorization;
using App.Modules.Categories.Application.CategoriesSynchronisationProcessing.SynchroniseCategories;
using App.Modules.Categories.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Modules.Categories;

[Authorize]
[ApiController]
[Route("categories/sync")]
public class CategoriesSynchronisationController(ICategoriesModule categoriesModule) : ControllerBase
{
    [HttpPost]
    [HasPermission(CategoriesPermissions.ManageCategories)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> SynchroniseCategories()
    {
        var result = await categoriesModule.ExecuteCommandAsync(new SynchroniseCategoriesCommand());

        return Created("", new
        {
            result.Status
        });
    }
}
