$(function () {
    var bsa = new BootstrapAdmin({
        url: '../api/Roles',
        dataEntity: new DataEntity({
            map: {
                ID: "roleID",
                RoleName: "roleName",
                Description: "roleDesc"
            }
        })
    });

    $('table').smartTable({
        url: '../api/Roles',            //请求后台的URL（*）
        sortName: 'RoleName',
        queryParams: function (params) { return $.extend(params, { roleName: $("#txt_search_name").val(), description: $("#txt_role_desc").val() }); },           //传递参数（*）
        columns: [{ checkbox: true },
            { title: "Id", field: "ID", events: bsa.idEvents(), formatter: BootstrapAdmin.idFormatter },
            { title: "角色名称", field: "RoleName", sortable: true },
            { title: "角色描述", field: "Description", sortable: false }
        ]
    });

    // validate
    $('#dataForm').autoValidate({
        roleName: {
            required: true,
            maxlength: 50
        }
    });
});