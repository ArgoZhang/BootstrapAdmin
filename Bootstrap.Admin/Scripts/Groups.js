$(function () {
    var $dialogUser = $("#dialogUser");
    var $dialogUserHeader = $('#myUserModalLabel');
    var $dialogUserForm = $('#userForm');
    var $dialogRole = $('#dialogRole');
    var $dialogRoleHeader = $('#myRoleModalLabel');
    var $dialogRoleForm = $('#roleForm');

    var bsa = new BootstrapAdmin({
        url: Group.url,
        dataEntity: new DataEntity({
            map: {
                Id: "groupID",
                GroupName: "groupName",
                Description: "groupDesc"
            }
        }),
        click: {
            assign: [{
                id: 'btn_assignRole',
                click: function (row) {
                    $.bc({
                        Id: row.Id, url: Role.url, data: { type: "group" }, swal: false,
                        callback: function (result) {
                            var htmlTemplate = this.htmlTemplate;
                            var html = $.map(result, function (element, index) {
                                return $.format(htmlTemplate, element.Id, element.RoleName, element.Checked, element.Description);
                            }).join('')
                            $dialogRoleHeader.text($.format('{0}-角色授权窗口', row.GroupName));
                            $dialogRoleForm.html(html).find('[role="tooltip"]').each(function (index, label) {
                                if (label.title == "") label.title = "未设置";
                            }).lgbTooltip();
                            $dialogRole.modal('show');
                        }
                    });
                }
            }, {
                id: 'btn_assignUser',
                click: function (row) {
                    $.bc({
                        Id: row.Id, url: User.url, data: { type: "group" }, swal: false,
                        callback: function (result) {
                            var htmlTemplate = this.htmlTemplate;
                            var html = $.map(result, function (element, index) {
                                return $.format(htmlTemplate, element.Id, element.DisplayName, element.Checked, element.UserName);
                            }).join('');
                            $dialogUserHeader.text($.format('{0}-用户授权窗口', row.GroupName));
                            $dialogUserForm.html(html).find('[role="tooltip"]').each(function (index, label) {
                                if (label.title == "") label.title = "未设置";
                            }).lgbTooltip();
                            $dialogUser.modal('show');
                        }
                    });
                }
            }, {
                id: 'btnSubmitRole',
                click: function (row) {
                    var groupId = row.Id;
                    var roleIds = $dialogRole.find('input:checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    $.bc({ Id: groupId, url: Role.url, method: "PUT", data: { type: "group", roleIds: roleIds }, title: Role.title, modal: 'dialogRole' });
                }
            }, {
                id: 'btnSubmitUser',
                click: function (row) {
                    var groupId = row.Id;
                    var userIds = $dialogUser.find(':checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    $.bc({ Id: groupId, url: User.url, method: "PUT", data: { type: "group", userIds: userIds }, title: User.title, modal: 'dialogUser' });
                }
            }]
        }
    });

    $('table').smartTable({
        url: Group.url,            //请求后台的URL（*）
        sortName: 'GroupName',
        queryParams: function (params) { return $.extend(params, { groupName: $("#txt_search_name").val(), description: $("#txt_group_desc").val() }); },           //传递参数（*）
        columns: [{ checkbox: true },
            { title: "编辑", field: "Id", events: bsa.idEvents(), formatter: BootstrapAdmin.idFormatter },
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