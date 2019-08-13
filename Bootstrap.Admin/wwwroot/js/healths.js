$(function () {
    var healthStatus = ["不健康", "亚健康", "健康"];
    var StatusFormatter = function (value) {
        return healthStatus[value];
    };

    var cate = { "db": "数据库", "file": "组件文件", "mem": "内存", "Gitee": "Gitee 接口", "gc": "垃圾回收器" };
    var CategoryFormatter = function (value) {
        return cate[value];
    };

    var $table = $('table').smartTable({
        sidePagination: "client",
        showToggle: false,
        showRefresh: false,
        showColumns: false,
        columns: [
            { title: "分类", field: "Name", formatter: CategoryFormatter },
            { title: "描述", field: "Description" },
            { title: "异常信息", field: "Exception" },
            { title: "耗时", field: "Duration" },
            { title: "检查结果", field: "Status", formatter: StatusFormatter },
            {
                title: "明细数据", field: "Data", formatter: function (value, row, index) {
                    return '<button class="detail btn btn-info" data-trigger="focus" data-container="body" data-toggle="popover" data-placement="left"><i class="fa fa-info"></i><span>明细</span></button>';
                },
                events: {
                    'click .detail': function (e, value, row, index) {
                        if ($.isEmptyObject(row.Data)) return;

                        var $button = $(e.currentTarget);
                        if (!$button.data($.fn.popover.Constructor.DATA_KEY)) {
                            var content = $.map(row.Data, function (v, name) {
                                return $.format("<tr><td>{0}</td><td>{1}</td></tr>", name, v);
                            }).join("");
                            content = $.format('<div class="bootstrap-table"><div class="fixed-table-container"><div class="fixed-table-body"><table class="table table-bordered table-hover table-sm"><thead><tr><th><div class="th-inner"><b>检查项</b><div></th><th><div class="th-inner"><b>检查值</b></div></th></tr></thead><tbody>{0}</tbody></table></div></div></div>', content);
                            $button.popover({ title: cate[row.Name], html: true, content: content, placement: $(window).width() < 768 ? "bottom" : "left" });
                        }
                        $button.popover('show');
                    }
                }
            }
        ]
    });

    $table.bootstrapTable('showLoading');
    $('#checkTotalEplsed').text('--');
    $.bc({
        url: 'healths',
        callback: function (result) {
            var data = $.map(result.Keys, function (name) {
                return { Name: name, Duration: result.Report.Entries[name].Duration, Status: result.Report.Entries[name].Status, Exception: result.Report.Entries[name].Exception, Description: result.Report.Entries[name].Description, Data: result.Report.Entries[name].Data };
            });
            $table.bootstrapTable('hideLoading');
            $table.bootstrapTable('load', data);
            $('#checkTotalEplsed').text(result.Report.TotalDuration);
            $.footer();
        }
    });
});