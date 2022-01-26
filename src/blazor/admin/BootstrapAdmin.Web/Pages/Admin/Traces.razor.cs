// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Models;

namespace BootstrapAdmin.Web.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Traces
{
    private List<int> PageItemsSource { get; } = new List<int> { 20, 40, 80, 100, 200 };

    private TraceSearchModel TraceSearchModel { get; } = new();

    [Inject]
    [NotNull]
    private ITrace? TraceService { get; set; }

    private Task<QueryData<Trace>> OnQueryAsync(QueryPageOptions options)
    {
        var ret = new QueryData<Trace>()
        {
            IsSorted = true,
            IsFiltered = true,
            IsSearch = true
        };

        var filter = new TraceFilter
        {
            UserName = TraceSearchModel.UserName,
            RequestUrl = TraceSearchModel.RequestUrl,
            Ip = TraceSearchModel.Ip,
            Star = TraceSearchModel.LogTime.Start,
            End = TraceSearchModel.LogTime.End,
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
        var (Items, ItemsCount) = TraceService.GetAll(options.SearchText, filter, options.PageIndex, options.PageItems, sortList);

        ret.TotalCount = ItemsCount;
        ret.Items = Items;
        ret.IsAdvanceSearch = true;
        return Task.FromResult(ret);
    }
}
