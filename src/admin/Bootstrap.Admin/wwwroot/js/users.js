$(function () {
    var $dialogRole = $('#dialogRole');
    var $dialogRoleHeader = $('#myRoleModalLabel');
    var $dialogRoleForm = $('#roleForm');
    var $dialogGroup = $("#dialogGroup");
    var $dialogGroupHeader = $('#myGroupModalLabel');
    var $dialogGroupForm = $('#groupForm');
    var $dialogReset = $('#dialogReset');
    var $dialogResetHeader = $('#myResetModalLabel');
    var $resetReason = $('#resetReason');
    var $table = $('table');

    $table.lgbTable({
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
                        id: row.Id, url: Role.url, query: { type: "user" }, method: "post", htmlTemplate: CheckboxHtmlTemplate,
                        callback: function (result) {
                            var htmlTemplate = this.htmlTemplate;
                            var html = $.map(result, function (element, index) {
                                return $.format(htmlTemplate, element.Id, element.RoleName, element.Checked, element.Description);
                            }).join('');
                            $dialogRoleHeader.text($.format('{0}-角色授权窗口', row.DisplayName));
                            $dialogRoleForm.html(html).find('[data-toggle="tooltip"]').each(function (index, label) {
                                if (label.title === "") label.title = "未设置";
                            }).tooltip();
                            $dialogRoleForm.find('.form-checkbox').lgbCheckbox();
                            $dialogRole.modal('show');
                        }
                    });
                },
                '#btn_assignGroup': function (row) {
                    $.bc({
                        id: row.Id, url: Group.url, query: { type: "user" }, method: "post", htmlTemplate: CheckboxHtmlTemplate,
                        callback: function (result) {
                            var htmlTemplate = this.htmlTemplate;
                            var html = $.map(result, function (element, index) {
                                return $.format(htmlTemplate, element.Id, element.GroupName, element.Checked, element.Description);
                            }).join('');
                            $dialogGroupHeader.text($.format('{0}-部门授权窗口', row.DisplayName));
                            $dialogGroupForm.html(html).find('[data-toggle="tooltip"]').each(function (index, label) {
                                if (label.title === "") label.title = "未设置";
                            }).tooltip();
                            $dialogGroupForm.find('.form-checkbox').lgbCheckbox();
                            $dialogGroup.modal('show');
                        }
                    });
                },
                '#btnSubmitRole': function (row) {
                    var userId = row.Id;
                    var roleIds = $dialogRole.find(':checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray();
                    $.bc({ id: userId, url: User.url, method: 'put', data: roleIds, query: { type: "role" }, title: Role.title, modal: '#dialogRole' });
                },
                '#btnSubmitGroup': function (row) {
                    var userId = row.Id;
                    var groupIds = $dialogGroup.find(':checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray();
                    $.bc({ id: userId, url: User.url, method: 'put', data: groupIds, query: { type: "group" }, title: Group.title, modal: '#dialogGroup' });
                },
                '#btnReset': function (row) {
                    $.bc({ id: row.UserName, url: 'api/Register', method: 'put', data: { password: $('#resetPassword').val() }, modal: "#dialogReset", title: "重置密码", callback: function (result) { if (result) $table.bootstrapTable('refresh'); } });
                }
            },
            callback: function (data) {
                if (data && data.success && data.oper === 'save' && data.data[0].UserName === $('#userDisplayName').attr('data-userName')) {
                    $('.username').text(data.data[0].DisplayName);
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
            sortName: 'RegisterTime',
            sortOrder: "desc",
            queryParams: function (params) { return $.extend(params, { name: $("#txt_search_name").val(), displayName: $('#txt_display_name').val() }); },           //传递参数（*）
            columns: [
                { title: "登录名称", field: "UserName", sortable: true },
                { title: "显示名称", field: "DisplayName", sortable: true },
                { title: "注册时间", field: "RegisterTime", sortable: true },
                { title: "授权时间", field: "ApprovedTime", sortable: true },
                { title: "授权人", field: "ApprovedBy", sortable: true },
                { title: "说明", field: "Description", sortable: false }
            ],
            editButtons: {
                events: {
                    'click .reset': function (e, value, row, index) {
                        $table.bootstrapTable('uncheckAll');
                        $table.bootstrapTable('check', index);
                        $dialogResetHeader.text($.format("{0} - 重置密码窗口", row.UserName));
                        $.bc({
                            id: row.UserName, url: User.url, method: 'post', query: { type: "reset" }, callback: function (result) {
                                if ($.isArray(result)) {
                                    var reason = result.map(function (v, index) {
                                        return $.format("{0}: {1}", v.Key, v.Value);
                                    }).join('\n');
                                    $resetReason.text(reason);
                                    $dialogReset.modal('show');
                                }
                            }
                        });
                    }
                },
                formatter: function (value, row, index) {
                    var $this = this.clone();
                    if (row.IsReset === 0) {
                        $this.find('button.reset').remove();
                    }
                    return $this.html();
                }
            },
            exportOptions: {
                fileName: "用户数据",
                ignoreColumn: [0, 7]
            }
        }
    });
});