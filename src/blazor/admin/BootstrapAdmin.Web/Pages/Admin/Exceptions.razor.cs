using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.Web.Pages.Admin;

public partial class Exceptions
{
    private List<int> PageItemsSource { get; } = new List<int> { 5, 20, 40, 80, 100, 200 };

    [Inject]
    [NotNull]
    private IException? ExceptionService { get; set; }

    private Task<QueryData<Error>> OnQueryAsync(QueryPageOptions options)
    {
        var ret = new QueryData<Error>()
        {
            IsSorted = true,
            IsFiltered = true,
            IsSearch = true
        };

        var items = ExceptionService.GetAll(options.SearchText, options.PageIndex, options.PageItems, options.SortName, options.SortOrder.ToString());

        ret.TotalCount = items.ItemsCount;
        ret.Items = items.Items;
        return Task.FromResult(ret);
    }
}
