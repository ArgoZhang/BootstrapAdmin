$(function () {
    var $dialogRole = $('#dialogRole');
    var $dialogRoleHeader = $('#myRoleModalLabel');
    var $dialogRoleForm = $('#roleForm');
    var $dialogGroup = $("#dialogGroup");
    var $dialogGroupHeader = $('#myGroupModalLabel');
    var $dialogGroupForm = $('#groupForm');

    $('table').lgbTable({
        url: User.url,
        dataBinder: {
            map: {
                Id: "#userID",
                UserName: "#userName",
                Password: "#password",
                DisplayName: "#displayName",
                NewPassword: "#confirm"
            },
            events: {
                '#btn_assignRole': function (row) {
                    $.bc({
                        id: row.Id, url: Role.url, data: { type: "user" }, swal: false,
                        callback: function (result) {
                            var htmlTemplate = this.htmlTemplate;
                            var html = $.map(result, function (element, index) {
                                return $.format(htmlTemplate, element.Id, element.RoleName, element.Checked, element.Description);
                            }).join('');
                            $dialogRoleHeader.text($.format('{0}-角色授权窗口', row.DisplayName));
                            $dialogRoleForm.html(html).find('[data-toggle="tooltip"]').each(function (index, label) {
                                if (label.title === "") label.title = "未设置";
                            }).tooltip();
                            $dialogRole.modal('show');
                        }
                    });
                },
                '#btn_assignGroup': function (row) {
                    $.bc({
                        id: row.Id, url: Group.url, data: { type: "user" }, swal: false,
                        callback: function (result) {
                            var htmlTemplate = this.htmlTemplate;
                            var html = $.map(result, function (element, index) {
                                return $.format(htmlTemplate, element.Id, element.GroupName, element.Checked, element.Description);
                            }).join('');
                            $dialogGroupHeader.text($.format('{0}-部门授权窗口', row.DisplayName));
                            $dialogGroupForm.html(html).find('[data-toggle="tooltip"]').each(function (index, label) {
                                if (label.title === "") label.title = "未设置";
                            }).tooltip();
                            $dialogGroup.modal('show');
                        }
                    });
                },
                '#btnSubmitRole': function (row) {
                    var userId = row.Id;
                    var roleIds = $dialogRole.find(':checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    $.bc({ id: userId, url: Role.url, method: 'PUT', data: { type: "user", roleIds: roleIds }, title: Role.title, modal: '#dialogRole' });
                },
                '#btnSubmitGroup': function (row) {
                    var userId = row.Id;
                    var groupIds = $dialogGroup.find(':checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    $.bc({ id: userId, url: Group.url, method: 'PUT', data: { type: "user", groupIds: groupIds }, title: Group.title, modal: '#dialogGroup' });
                }
            },
            callback: function (data) {
                if (data && data.success && data.oper === 'save' && data.data.UserName === $('#userDisplayName').attr('data-userName')) {
                    $('#userDisplayName').text(data.data.DisplayName);
                }
                if (data && data.oper === 'create') {
                    $('#userName').prop('readonly', false).removeClass("ignore");
                }
                else if (data && data.oper === 'edit') {
                    $('#userName').prop('readonly', true).addClass("ignore");
                }
            }
        },
        smartTable: {
            sortName: 'DisplayName',
            sortOrder: "asc",
            queryParams: function (params) { return $.extend(params, { name: $("#txt_search_name").val(), displayName: $('#txt_display_name').val() }); },           //传递参数（*）
            columns: [
                { title: "登陆名称", field: "UserName", sortable: true },
                { title: "显示名称", field: "DisplayName", sortable: true },
                { title: "注册时间", field: "RegisterTime", sortable: true },
                { title: "授权时间", field: "ApprovedTime", sortable: true },
                { title: "授权人", field: "ApprovedBy", sortable: true },
                { title: "说明", field: "Description", sortable: false }
            ]
        }
    });
});