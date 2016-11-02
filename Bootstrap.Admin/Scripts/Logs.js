$(function () {
    //隐藏掉操作按钮
    $("#toolbar").hide();

    var bsa = new BootstrapAdmin({
        url: '../api/Logs',
        dataEntity: new DataEntity({
            map: {
                ID: "logID",
                OperationType: "operateType",
                UserID: "userId",
                OperationTime: "operateTime",
                operateIp: "OperationIp"
            }
        })
    });
    $('table').smartTable({
        url: '../api/Logs',            //请求后台的URL（*）
        sortName: 'OperationType',
        queryParams: function (params) { return $.extend(params, { operateType: $("#txt_operate_type").val(), OperateTimeStart: $("#txt_operate_start").val(), OperateTimeEnd: $("#txt_operate_end").val() }); },           //传递参数（*）
        columns: [{ checkbox: true },
            { title: "Id", field: "ID", events: bsa.idEvents(), formatter: BootstrapAdmin.idFormatter, },
            { title: "操作类型", field: "OperationType", sortable: true },
            { title: "用户名称", field: "UserName", sortable: false },
            {
                title: "操作时间", field: "OperationTime", sortable: false,
                formatter: function (value, row, index) {
                    return value.substring(0,19).replace("T", " ");
                }
            },
            { title: "操作IP", field: "OperationIp", sortable: false },
            { title: "备注", field: "Remark", sortable: false }
        ]
    });
});