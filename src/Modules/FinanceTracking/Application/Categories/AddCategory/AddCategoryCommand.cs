using App.Modules.FinanceTracking.Application.Configuration.Commands;
using Newtonsoft.Json;

namespace App.Modules.FinanceTracking.Application.Categories.AddCategory;

[method: JsonConstructor]
public class AddCategoryCommand(
    Guid id,
    Guid correlationId,
    Guid categoryId,
    string externalCategoryId)
    : InternalCommandBase(id, correlationId)
{
    internal Guid CategoryId { get; } = categoryId;

    internal string ExternalCategoryId { get; } = externalCategoryId;
}
