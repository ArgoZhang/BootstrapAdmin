$(function () {
    var bsa = new BootstrapAdmin({
        url: '../api/Menus',
        dataEntity: new DataEntity({
            map: {
                ID: "menuID",
                ParentId: "parentId",
                Name: "name",
                Order: "order",
                Icon: "icon",
                Url: "url",
                Category: "category"
            }
        }),
        click: {
            assign: [{
                id: 'btn_assignRole',
                click: function (row) {
                    Role.getRolesByMenuId(row.ID, function (roles) {
                        $("#dialogRole .modal-title").text($.format('{0}-角色授权窗口', row.Name));
                        var data = $.map(roles, function (element, index) {
                            if (element.IsSelect == 1) {
                                return $.format('<div class="checkbox"><label><input type="checkbox" value="{0}" checked="checked">{1}</label></div>', element.ID, element.RoleName);
                            } else if (element.IsSelect == 0) {
                                return $.format('<div class="checkbox"><label><input type="checkbox" value="{0}">{1}</label></div>', element.ID, element.RoleName);
                            }
                        }).join('');
                        $('#dialogRole form').html(data);
                        $('#dialogRole').modal('show');
                    });
                }
            }, {
                id: 'btnSubmitUserRole',
                click: function (row) {
                    var menuId = row.ID;
                    var roleIds = $('#dialogRole :checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    Role.saveRolesByMenuId(menuId, roleIds, function (result) {
                        if (result) {
                            $('#dialogRole').modal("hide");
                            swal("成功", "修改角色", "success");
                        } else {
                            swal("失败", "修改角色", "error");
                        }
                    });
                }
            }]
        }
    });

    $('table').smartTable({
        url: '../api/Menus',            //请求后台的URL（*）
        sortName: 'UserName',
        queryParams: function (params) { return $.extend(params, { name: $("#txt_menus_name").val(), category: $('#txt_menus_category').val() }); },           //传递参数（*）
        columns: [{ checkbox: true },
            { title: "Id", field: "ID", events: bsa.idEvents(), formatter: BootstrapAdmin.idFormatter },
            { title: "父级Id", field: "ParentId", sortable: false },
            { title: "菜单名称", field: "Name", sortable: true },
            { title: "菜单序号", field: "Order", sortable: false },
            { title: "菜单图标", field: "Icon", sortable: false },
            { title: "菜单路径", field: "Url", sortable: false },
            { title: "菜单类别", field: "Category", sortable: false }
        ]
    });

    // validate
    $('#dataForm').autoValidate({
        name: {
            required: true,
            maxlength: 50
        },
        icon: {
            required: true,
            maxlength: 50
        },
        url: {
            required: true,
            maxlength: 50
        },
        category: {
            required: true,
            maxlength: 50
        }
    });
});