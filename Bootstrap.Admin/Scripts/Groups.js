$(function () {
    var bsa = new BootstrapAdmin({
        url: '../api/Groups',
        dataEntity: new DataEntity({
            map: {
                ID: "groupID",
                GroupName: "groupName",
                Description: "groupDesc"               
            }
        }),
        click: {
            assign: [{
                id: 'btn_assignRole',
                click: function (row) {
                    Role.getRolesByGroupId(row.ID, function (data) {
                        $("#dialogRole .modal-title").text($.format('{0}-角色授权窗口', row.GroupName));
                        $('#dialogRole form').html(data);
                        $('#dialogRole').modal('show');
                    });
                }
            }, {
                id: 'btn_assignGroup',
                click: function (row) {
                    var userId = row.ID;
                }
            }, {
                id: 'btnSubmitUserRole',
                click: function (row) {
                    var userId = row.ID;
                    var roleIds = $('#dialogRole :checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    Role.saveRolesByGroupId(userId, roleIds, function (result) {
                        if (result) {
                            $('#dialogRole').modal("hide");
                            swal("成功", "修改角色", "success");
                        } else {
                            swal("失败", "修改角色", "error");
                        }

                    });
                }
            }]
        },
        success: function (src, data) {
            if (src === 'save' && data.ID === $('#userId').val()) {
                $('.username').text(data.DisplayName);
            }
        }
    });


    $('table').smartTable({
        url: '../api/Groups',            //请求后台的URL（*）
        sortName: 'GroupName',
        queryParams: function (params) { return $.extend(params, { groupName: $("#txt_search_name").val(), description: $("#txt_group_desc").val() }); },           //传递参数（*）
        columns: [{ checkbox: true },
            { title: "Id", field: "ID", events: bsa.idEvents(), formatter: BootstrapAdmin.idFormatter },
            { title: "部门名称", field: "GroupName", sortable: true },
            { title: "部门描述", field: "Description", sortable: false }
        ]
    });

    // validate
    $('#dataForm').autoValidate({
        groupName: {
            required: true,
            maxlength: 50
        }
    });
});