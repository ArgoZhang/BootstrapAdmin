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
                id: 'btn_assignUser',
                click: function (row) {
                    User.getUsersByGroupeId(row.ID, function (data) {
                        $("#dialogUser .modal-title").text($.format('{0}-用户授权窗口', row.GroupName));
                        $('#dialogUser form').html(data);
                        $('#dialogUser').modal('show');
                    });
                }
            }, {
                id: 'btnSubmitRole',
                click: function (row) {
                    var groupId = row.ID;
                    var roleIds = $('#dialogRole :checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    Role.saveRolesByGroupId(groupId, roleIds, { modal: 'dialogRole' });
                }
            }, {
                id: 'btnSubmitUser',
                click: function (row) {
                    var groupId = row.ID;
                    var userIds = $('#dialogUser :checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    User.saveUsersByGroupId(groupId, userIds, { modal: 'dialogUser' });
                }
            }]
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