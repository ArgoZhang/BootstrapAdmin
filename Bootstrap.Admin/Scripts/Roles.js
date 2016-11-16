$(function () {
    var $dialogUser = $("#dialogUser");
    var $dialogGroup = $("#dialogGroup");
    var $dialogMenu = $('#dialogMenu');
    var $dialogSubMenu = $('#dialogSubMenu').find('.modal-content');
    var $btnSubmitMenu = $('#btnSubmitMenu');
    var $nestMenu = $('#nestable_menu');
    var $nestMenuInput = $nestMenu.find('div.dd3-content');
    $nestMenuInput.find('label:last').hide();

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
                        $dialogUser.find("div.modal-header").find('h4').text($.format('{0}-用户授权窗口', row.RoleName));
                        $dialogUser.find('form').html(data);
                        $dialogUser.modal('show');
                    })
                }
            }, {
                id: 'btn_assignGroup',
                click: function (row) {
                    Group.getGroupsByRoleId(row.ID, function (data) {
                        $dialogGroup.find("div.modal-header").find("h4").text($.format('{0}-部门授权窗口', row.RoleName));
                        $dialogGroup.find('form').html(data);
                        $dialogGroup.modal('show');
                    })
                }
            }, {
                id: 'btnSubmitUser',
                click: function (row) {
                    var roleId = row.ID;
                    var userIds = $dialogUser.find(':checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    User.saveUsersByRoleId(roleId, userIds, { modal: 'dialogUser' });
                }
            },
            {
                id: 'btnSubmitGroup',
                click: function (row) {
                    var roleId = row.ID;
                    var groupIds = $dialogGroup.find(':checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    Group.saveGroupsByRoleId(roleId, groupIds, { modal: 'dialogGroup' });
                }
            },
            {
                id: 'btn_assignMenu',
                click: function (row) {
                    Menu.getMenusByRoleId(row.ID, function (data) {
                        $dialogSubMenu.find("div.modal-header").find('h4').text($.format('{0}-菜单授权窗口', row.RoleName));
                        $btnSubmitMenu.data('type', 'menu');
                        // set checkbox status
                        var menus = $nestMenu.find('input:checkbox');
                        menus.removeProp('checked');
                        $.each(data, function (index, item) {
                            var selector = $.format('[value={0}]', item.ID);
                            menus.filter(selector).prop('checked', 'checked');
                        });
                        $dialogSubMenu.show();
                        $dialogMenu.modal('show');
                    })
                }
            },
            {
                id: 'btnSubmitMenu',
                click: function (row) {
                    var roleId = row.ID;
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
                    Menu.saveMenusByRoleId(roleId, menuIds, { modal: 'dialogMenu' });
                }
            }]
        }
    });

    $nestMenu.nestable();

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