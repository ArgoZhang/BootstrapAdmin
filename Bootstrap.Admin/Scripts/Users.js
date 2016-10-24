$(function () {
    var dataEntity = new DataEntity({
        map: {
            ID: "userID",
            UserName: "userName",
            Password: "password",
            DisplayName: "displayName"
        }
    });

    var bsa = new BootstrapAdmin({
        url: '../api/Users',
        dataEntity: dataEntity,
        click: {
            query: 'btn_query',
            create: 'btn_add',
            edit: 'btn_edit',
            del: 'btn_delete',
            save: 'btnSubmit'
        }
    });

    $('table').smartTable({
        url: '../api/Users',            //请求后台的URL（*）
        sortName: 'UserName',
        queryParams: function (params) { return $.extend(params, { name: $("#txt_search_name").val() }); },           //传递参数（*）
        columns: [{ checkbox: true },
            { title: "Id", field: "ID", events: bsa.idEvents(), formatter: BootstrapAdmin.idFormatter },
            { title: "用户名称", field: "UserName", sortable: true }
        ]
    });

    // validate
    $('#dataForm').autoValidate({
        userName: {
            required: true,
            maxlength: 50
        },
        password: {
            required: true,
            maxlength: 50
        },
        confirm: {
            required: true,
            equalTo: "#password"
        },
        displayName: {
            required: true,
            maxlength: 50
        }
    });

    //TODO: 客户端点击保存用户后，要更新页面右上角用户显示名称
});