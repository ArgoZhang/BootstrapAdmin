$(function () {
    var url = 'api/Logs/';

    $('.card-body table').smartTable({
        url: url,
        sortName: 'LogTime',
        sortOrder: 'desc',
        queryParams: function (params) { return $.extend(params, { operateType: $("#txt_operate_type").val(), OperateTimeStart: $("#txt_operate_start").val(), OperateTimeEnd: $("#txt_operate_end").val() }); },
        columns: [
            { title: "操作类型", field: "CRUD", sortable: true },
            { title: "用户名称", field: "UserName", sortable: true },
            { title: "操作时间", field: "LogTime", sortable: true },
            { title: "操作IP", field: "ClientIp", sortable: true },
            { title: "Url", field: "RequestUrl", sortable: true },
            { title: "备注", field: "ClientAgent", sortable: false }
        ]
    });
});