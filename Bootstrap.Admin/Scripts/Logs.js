$(function () {
    $('table').smartTable({
        url: '../api/Logs',
        sortName: 'OperationType',
        queryParams: function (params) { return $.extend(params, { operateType: $("#txt_operate_type").val(), OperateTimeStart: $("#txt_operate_start").val(), OperateTimeEnd: $("#txt_operate_end").val() }); },
        columns: [{ checkbox: true },
            { title: "操作类型", field: "CRUD", sortable: true },
            { title: "用户名称", field: "UserName", sortable: false },
            { title: "操作时间", field: "LogTime", sortable: false },
            { title: "操作IP", field: "ClientIp", sortable: false },
            { title: "Url", field: "RequestUrl", sortable: false },
            { title: "备注", field: "ClientAgent", sortable: false }
        ]
    });

    $('input[type="datetime"]').parent().datetimepicker({
        locale: "zh-cn",
        format: "YYYY-MM-DD"
    });
});