using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Models;
using BootstrapAdmin.Web.Utils;

namespace BootstrapAdmin.Web.Pages.Admin;

public partial class Exceptions
{
    private List<int> PageItemsSource { get; } = new List<int> { 20, 40, 80, 100, 200 };

    private ErrorSearchModel ErrorSearchModel { get; set; } = new ErrorSearchModel();

    [Inject]
    [NotNull]
    private IException? ExceptionService { get; set; }

    [NotNull]
    private List<SelectedItem>? CategroyLookup { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        CategroyLookup = LookupHelper.GetExceptionCategory();
    }

    private Task<QueryData<Error>> OnQueryAsync(QueryPageOptions options)
    {
        var ret = new QueryData<Error>()
        {
            IsSorted = true,
            IsFiltered = true,
            IsSearch = true
        };

        var filter = new ExceptionFilter
        {
            Category = ErrorSearchModel.Category,
            UserId = ErrorSearchModel.UserId,
            ErrorPage = ErrorSearchModel.ErrorPage,
            Star = ErrorSearchModel.LogTime.Start,
            End = ErrorSearchModel.LogTime.End,
        };

        var sortList = new List<string>();
        if (options.SortOrder != SortOrder.Unset && !string.IsNullOrEmpty(options.SortName))
        {
            sortList.Add($"{options.SortName} {options.SortOrder}");
        }
        else if (options.SortList != null)
        {
            sortList.AddRange(options.SortList);
        }
        var (Items, ItemsCount) = ExceptionService.GetAll(options.SearchText, filter, options.PageIndex, options.PageItems, sortList);

        ret.TotalCount = ItemsCount;
        ret.Items = Items;
        ret.IsAdvanceSearch = true;
        return Task.FromResult(ret);
    }
}
