$(function () {
    $.extend({
        sendHealths: function (data) {
            $.bc({ url: 'api/Healths', data: JSON.stringify(data), method: 'post' });
        }
    });
    var healthStatus = ['<button class="btn btn-danger"><i class="fa fa-times-circle"></i><span>不健康</span></button>', '<button class="btn btn-warning"><i class="fa fa-exclamation-circle"></i><span>亚健康</span></button>', '<button class="btn btn-success"><i class="fa fa-check-circle"></i><span>健康</span></button>'];
    var StatusFormatter = function (value) {
        return healthStatus[value];
    };

    var cate = { "db": "数据库", "file": "组件文件", "mem": "内存", "Gitee": "Gitee 接口", "gc": "垃圾回收器", "dotnet-runtime": "运行时", "environment": "环境变量" };
    var CategoryFormatter = function (value) {
        return cate[value];
    };

    var ExceptionFormatter = function (value) {
        return value ? JSON.stringify(value) : null;
    };

    var $table = $('#tbCheck').smartTable({
        pagination: false,
        showToggle: false,
        showRefresh: false,
        showColumns: false,
        toolbar: false,
        search: false,
        columns: [
            { title: "分类", field: "Name", formatter: CategoryFormatter },
            { title: "描述", field: "Description" },
            { title: "异常信息", field: "Exception", formatter: ExceptionFormatter },
            { title: "耗时", field: "Duration" },
            { title: "检查结果", field: "Status", formatter: StatusFormatter },
            {
                title: "明细数据", field: "Data", formatter: function (value, row, index) {
                    return '<button class="detail btn btn-info"><i class="fa fa-info-circle"></i><span>明细</span></button>';
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
            // async send result to cloud
            $.sendHealths(result);
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
        pagination: false,
        showToggle: false,
        showRefresh: false,
        showColumns: false,
        columns: [
            { title: "检查项", field: "name" },
            {
                title: "值", field: "value", formatter: function (value, row) {
                    if (row.name === "Exception") value = $.format("<span class='text-danger'>{0}</span>", value);
                    if (row.name === "dotnet --info") {
                        value = value.replace(/\r\n/g, "<br>");
                        value = value.replace(/\n/g, "<br>");
                    }
                    return value;
                }
            }
        ]
    });
});