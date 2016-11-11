$(function () {
    var bsa = new BootstrapAdmin({
        url: '../api/Exceptions',
        dataEntity: new DataEntity({
            map: {
                ID: "ID",
                AppDomainName: "appDomainName",
                ErrorPage: "errorPage",
                UserID: "userId",
                UserIp: "userIp",
                Message: "message",
                StackTrace: "stackTrace",
                LogTime:"logTime"
            }
        }), 
    })
    $('table').smartTable({
        url: '../api/Exceptions',            //请求后台的URL（*）
        sortName: 'AppDomainName',
        queryParams: function (params) { return $.extend(params, {  OperateTimeStart: $("#txt_operate_start").val(), OperateTimeEnd: $("#txt_operate_end").val() }); },           //传递参数（*）
        columns: [{ checkbox: true },
            { title: "APP域名", field: "AppDomainName", sortable: true },
            { title: "错误页", field: "ErrorPage", sortable: false },
            {
                title: "用户名", field: "UserID", sortable: false,
            },
            { title: "用户IP", field: "UserIp", sortable: false },
            { title: "备注", field: "Message", sortable: false },
            { title: "栈记录", field: "StackTrace", sortable: false },
            { title: "异常捕获时间", field: "LogTime", sortable: false,
                formatter: function (value, row, index) {
                    return value.substring(0, 19).replace("T", " ");
                }
            }
        ]
    });

    $('input[type="datetime"]').parent().datetimepicker({
        locale: "zh-cn",
        format: "YYYY-MM-DD"
    });
});