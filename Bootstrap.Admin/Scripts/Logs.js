$(function () {
    $('table').smartTable({
        url: '../api/Logs',            //请求后台的URL（*）
        sortName: 'OperationType',
        queryParams: function (params) { return $.extend(params, { operateType: $("#txt_operate_type").val(), OperateTimeStart: $("#txt_operate_start").val(), OperateTimeEnd: $("#txt_operate_end").val() }); },           //传递参数（*）
        columns: [{ checkbox: true },
            { title: "操作类型", field: "CRUD", sortable: true },
            { title: "用户名称", field: "UserName", sortable: false },
            {
                title: "操作时间", field: "LogTime", sortable: false,
                formatter: function (value, row, index) {
                    return value.substring(0, 19).replace("T", " ");
                }
            },
            { title: "操作IP", field: "ClientIp", sortable: false },
            { title: "备注", field: "ClientAgent", sortable: false },
            { title: "操作模块", field: "RequestUrl", sortable: false }
        ]
    });
    var log = new LogPlugin();
});