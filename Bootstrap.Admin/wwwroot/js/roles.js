$(function () {
    var $dialogUser = $("#dialogUser");
    var $dialogUserHeader = $('#myUserModalLabel');
    var $dialogUserForm = $('#userForm');
    var $dialogGroup = $("#dialogGroup");
    var $dialogGroupHeader = $('#myGroupModalLabel');
    var $dialogGroupForm = $('#groupForm');
    var $dialogMenu = $('#dialogMenu');
    var $dialogMenuHeader = $('#myMenuModalLabel');
    var $dialogSubMenu = $('#dialogSubMenu').find('.modal-content');
    var $btnSubmitMenu = $('#btnSubmitMenu');
    var $nestMenu = $('#nestable_menu');
    var $nestMenuInput = $nestMenu.find('div.dd3-content');

    $('table').lgbTable({
        url: Role.url,
        dataBinder: {
            map: {
                Id: "#roleID",
                RoleName: "#roleName",
                Description: "#roleDesc"
            },
            events: {
                '#btn_assignUser': function (row) {
                    $.bc({
                        id: row.Id, url: User.url, data: { type: "role" }, method: "post",
                        callback: function (result) {
                            var htmlTemplate = this.htmlTemplate;
                            var html = $.map(result, function (element, index) {
                                return $.format(htmlTemplate, element.Id, element.DisplayName, element.Checked, element.UserName);
                            }).join('');
                            $dialogUserHeader.text($.format('{0}-用户授权窗口', row.RoleName));
                            $dialogUserForm.html(html).find('[data-toggle="tooltip"]').each(function (index, label) {
                                if (label.title === "") label.title = "未设置";
                            }).tooltip();
                            $dialogUser.modal('show');
                        }
                    });
                },
                '#btn_assignGroup': function (row) {
                    $.bc({
                        id: row.Id, url: Group.url, data: { type: "role" }, method: "post",
                        callback: function (result) {
                            var htmlTemplate = this.htmlTemplate;
                            var html = $.map(result, function (element, index) {
                                return $.format(htmlTemplate, element.Id, element.GroupName, element.Checked, element.Description);
                            }).join('');
                            $dialogGroupHeader.text($.format('{0}-部门授权窗口', row.RoleName));
                            $dialogGroupForm.html(html).find('[data-toggle="tooltip"]').each(function (index, label) {
                                if (label.title === "") label.title = "未设置";
                            }).tooltip();
                            $dialogGroup.modal('show');
                        }
                    });
                },
                '#btn_assignMenu': function (row) {
                    $.bc({
                        id: row.Id, url: Menu.url, data: { type: "role" }, method: "post",
                        callback: function (result) {
                            $dialogMenuHeader.text($.format('{0}-菜单授权窗口', row.RoleName));
                            $btnSubmitMenu.data('type', 'menu');
                            // set checkbox status
                            var menus = $nestMenu.find('input:checkbox');
                            menus.attr('checked', false);
                            $.each(result, function (index, item) {
                                var selector = $.format('[value={0}]', item.Id);
                                menus.filter(selector).attr('checked', true);
                            });
                            $dialogSubMenu.show();
                            $dialogMenu.modal('show');
                        }
                    });
                },
                '#btnSubmitUser': function (row) {
                    var roleId = row.Id;
                    var userIds = $dialogUser.find(':checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    $.bc({ id: roleId, url: User.url, method: "put", data: { type: "role", userIds: userIds }, modal: '#dialogUser', title: User.title });
                },
                '#btnSubmitGroup': function (row) {
                    var roleId = row.Id;
                    var groupIds = $dialogGroup.find(':checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    $.bc({ id: roleId, url: Group.url, method: "put", data: { type: "role", groupIds: groupIds }, modal: '#dialogGroup', title: Group.title });
                },
                '#btnSubmitMenu': function (row) {
                    var roleId = row.Id;
                    var type = $btnSubmitMenu.data('type');
                    switch (type) {
                        case "menu":
                            var menuIds = $nestMenuInput.find('input:checkbox:checked').map(function (index, element) {
                                return $(element).val();
                            }).toArray().join(',');
                            break;
                        default:
                            break;
                    }
                    $.bc({ id: roleId, url: Menu.url, method: "put", data: { type: "role", menuIds: menuIds }, modal: '#dialogMenu', title: Menu.title });
                }
            }
        },
        smartTable: {
            sortName: 'RoleName',
            queryParams: function (params) { return $.extend(params, { roleName: $("#txt_search_name").val(), description: $("#txt_role_desc").val() }); },           //传递参数（*）
            columns: [
                { title: "角色名称", field: "RoleName", sortable: true },
                { title: "角色描述", field: "Description", sortable: false }
            ]
        }
    });

    $nestMenu.nestMenu(function () {
        $nestMenuInput = $nestMenu.find('div.dd3-content');
        $nestMenuInput.on('click', ':checkbox', function () {
            var val = $(this).prop('checked');
            var child = $(this).parent().parent().next();
            if (child.hasClass('dd-list')) {
                child.find(':checkbox').prop('checked', val);
            }
        }).children('.radio').hide();
    });
});