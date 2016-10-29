$(function () {
    var bsa = new BootstrapAdmin({
        url: '../api/Roles',
        dataEntity: new DataEntity({
            map: {
                ID: "roleID",
                RoleName: "roleName",
                Description: "roleDesc"
            }
        }),
        click: {
            assign: [{
                id: 'btn_assignUser',
                click: function (row) {
                    User.getUsersByRoleId(row.ID, function (data) {
                        $("#dialogUser.modal-title").text($.format('{0}-用户授权窗口', row.RoleName));
                        $('#dialogUser form').html(data);
                        $('#dialogUser').modal('show');
                    })
                }
            }, {
                id: 'btn_assignGroup',
                click: function (row) {
                    Group.getGroupsByRoleId(row.ID, function (data) {
                        $("#dialogGroup.modal-title").text($.format('{0}-部门授权窗口', row.RoleName));
                        $('#dialogGroup form').html(data);
                        $('#dialogGroup').modal('show');
                    })
                }
            }, {
                id: 'btnSubmitRoleUser',
                click: function (row) {
                    var roleId = row.ID;
                    var userIds = $('#dialogUser :checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    User.saveUsersByRoleId(roleId, userIds, { modal: 'dialogUser' });
                }
            },
            {
                id: 'btnSubmitRoleGroup',
                click: function (row) {
                    var roleId = row.ID;
                    var groupIds = $('#dialogGroup :checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    Group.saveGroupsByRoleId(roleId, groupIds, function (result) {
                        if (result) {
                            $('#dialogGroup').modal('hide');
                            swal("成功", "修改部门", "success");
                        }
                        else {
                            swal("失败", "修改部门", "error");
                        }
                    });
                }
            }]
        }
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