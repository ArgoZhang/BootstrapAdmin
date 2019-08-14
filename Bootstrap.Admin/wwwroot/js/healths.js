$(function () {
    var healthStatus = ['<span class="badge badge-md badge-pill badge-danger">不健康</span>', '<span class="badge badge-md badge-pill badge-warning">亚健康</span>', '<span class="badge badge-md badge-pill badge-success">健康</span>'];
    var StatusFormatter = function (value) {
        return healthStatus[value];
    };

    var cate = { "db": "数据库", "file": "组件文件", "mem": "内存", "Gitee": "Gitee 接口", "gc": "垃圾回收器" };
    var CategoryFormatter = function (value) {
        return cate[value];
    };

    var $table = $('#tbCheck').smartTable({
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
                    return '<button class="detail btn btn-info"><i class="fa fa-info"></i><span>明细</span></button>';
                },
                events: {
                    'click .detail': function (e, value, row, index) {
                        if ($.isEmptyObject(row.Data)) return;

                        var content = $.map(row.Data, function (v, name) {
                            return { name: name, value: v };
                        });
                        // 弹出 modal 健康检查明细窗口
                        $checkDetail.bootstrapTable('load', content);
                        $('#dialogNew').modal('show');
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

    // init detail Table
    var $checkDetail = $('#checkDetail').smartTable({
        sidePagination: "client",
        showToggle: false,
        showRefresh: false,
        showColumns: false,
        columns: [
            { title: "检查项", field: "name" },
            { title: "值", field: "value" }
        ]
    });
});