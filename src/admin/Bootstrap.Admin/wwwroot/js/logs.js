$(function () {
    var url = 'api/Logs';
    var $data = $('#requestData');
    var $dialog = $('#dialogRequestData');

    $('.card-body table').smartTable({
        url: url,
        sortName: 'LogTime',
        sortOrder: 'desc',
        toolbar: false,
        search: false,
        queryParams: function (params) { return $.extend(params, { operateType: $("#txt_operate_type").val(), OperateTimeStart: $("#txt_operate_start").val(), OperateTimeEnd: $("#txt_operate_end").val() }); },
        columns: [
            { title: "操作类型", field: "CRUD", sortable: true },
            { title: "用户名称", field: "UserName", sortable: true },
            { title: "操作时间", field: "LogTime", sortable: true },
            { title: "登录主机", field: "Ip", sortable: true },
            { title: "操作地点", field: "City" },
            { title: "浏览器", field: "Browser" },
            { title: "操作系统", field: "OS" },
            { title: "操作页面", field: "RequestUrl", sortable: true },
            {
                title: "请求数据", field: "RequestData", formatter: function (value, row, index) {
                    return '<button class="detail btn btn-info"><i class="fa fa-info"></i><span>明细</span></button>';
                },
                events: {
                    'click .detail': function (e, value, row, index) {
                        $data.html($.syntaxHighlight($.safeHtml(row.RequestData)));
                        $dialog.modal('show');
                    }
                }
            }
        ],
        exportOptions: {
            fileName: "操作日志数据",
            ignoreColumn: [8]
        }
    });
});