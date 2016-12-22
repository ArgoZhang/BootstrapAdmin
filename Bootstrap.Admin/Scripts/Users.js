$(function () {
    var $dialogRole = $('#dialogRole');
    var $dialogRoleHeader = $('#myRoleModalLabel');
    var $dialogRoleForm = $('#roleForm');
    var $dialogGroup = $("#dialogGroup");
    var $dialogGroupHeader = $('#myGroupModalLabel');
    var $dialogGroupForm = $('#groupForm');

    var bsa = new BootstrapAdmin({
        url: User.url,
        dataEntity: new DataEntity({
            map: {
                ID: "userID",
                UserName: "userName",
                Password: "password",
                DisplayName: "displayName",
                NewPassword: "confirm"
            }
        }),
        click: {
            assign: [{
                id: 'btn_assignRole',
                click: function (row) {
                    $.bc({
                        Id: row.ID, url: Role.url, data: { type: "user" }, swal: false,
                        callback: function (result) {
                            var htmlTemplate = this.htmlTemplate;
                            var html = $.map(result, function (element, index) {
                                return $.format(htmlTemplate, element.ID, element.RoleName, element.Checked, element.Description);
                            }).join('')
                            $dialogRoleHeader.text($.format('{0}-角色授权窗口', row.DisplayName));
                            $dialogRoleForm.html(html);
                            $dialogRole.modal('show');
                        }
                    });
                }
            }, {
                id: 'btn_assignGroup',
                click: function (row) {
                    $.bc({
                        Id: row.ID, url: Group.url, data: { type: "user" }, swal: false,
                        callback: function (result) {
                            var htmlTemplate = this.htmlTemplate;
                            var html = $.map(result, function (element, index) {
                                return $.format(htmlTemplate, element.ID, element.GroupName, element.Checked, element.Description);
                            }).join('');
                            $dialogGroupHeader.text($.format('{0}-部门授权窗口', row.DisplayName));
                            $dialogGroupForm.html(html);
                            $dialogGroup.modal('show');
                        }
                    });
                }
            }, {
                id: 'btnSubmitRole',
                click: function (row) {
                    var userId = row.ID;
                    var roleIds = $dialogRole.find(':checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    $.bc({ Id: userId, url: Role.url, method: 'PUT', data: { type: "user", roleIds: roleIds }, title: Role.title, modal: 'dialogRole' });
                }
            }, {
                id: 'btnSubmitGroup',
                click: function (row) {
                    var userId = row.ID;
                    var groupIds = $dialogGroup.find(':checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    $.bc({ Id: userId, url: Group.url, method: 'PUT', data: { type: "user", groupIds: groupIds }, title: Group.title, modal: 'dialogGroup' });
                }
            }]
        },
        callback: function (data) {
            if (data && data.success && data.oper === 'save' && data.data.ID === $('#userId').val()) {
                $('#userDisplayName').text(data.data.DisplayName);
            }
            if (data && data.oper === 'create') {
                $('#userName').removeProp('readonly').removeClass("ignore");
            }
            else if (data && data.oper === 'edit') {
                $('#userName').prop('readonly', true).addClass("ignore");
            }
        }
    });

    $('table').smartTable({
        url: User.url,            //请求后台的URL（*）
        sortName: 'DisplayName',
        queryParams: function (params) { return $.extend(params, { name: $("#txt_search_name").val(), displayName: $('#txt_display_name').val() }); },           //传递参数（*）
        columns: [{ checkbox: true },
            { title: "Id", field: "ID", events: bsa.idEvents(), formatter: BootstrapAdmin.idFormatter },
            { title: "登陆名称", field: "UserName", sortable: true },
            { title: "显示名称", field: "DisplayName", sortable: true }
        ]
    });

    // validate
    $('#dataForm').autoValidate({
        userName: {
            required: true,
            maxlength: 50,
            remote: {
                url: "../api/Users/",
                type: "PUT",
                data: {
                    UserStatus: 9
                }
            }
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
    }, {
        userName: {
            remote: "此登陆名称已存在"
        }
    });
});