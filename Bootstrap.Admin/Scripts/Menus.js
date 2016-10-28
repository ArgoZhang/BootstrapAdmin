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
                    Role.getRolesByMenuId(row.ID, function (data) {
                        $("#dialogRole .modal-title").text($.format('{0}-角色授权窗口', row.Name));
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
                    Role.saveRolesByMenuId(menuId, roleIds, { modal: 'dialogRole' });
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

    $('.fontawesome-icon-list ul li').addClass('col-md-3 col-sm-4 col-sm-6');

    $('.fontawesome-icon-list .fa-hover a, .fontawesome-icon-list ul li').click(function () {
        $('.icon-content .modal-footer i').attr('class', $(this).children('i, span:first').attr('class'));
        return false;
    });

    $('.form-group .input-group .input-group-btn .btn').click(function () {
        $('.icon-content').show();
    });

    $('.icon-content button:not(:last)').click(function () {
        $('.icon-content').hide();
    });

    $('.icon-content button:last').click(function () {
        var icon = $('.icon-content .modal-footer i').attr('class');
        window.console.log(icon);
        $('.icon-content').hide();
        $('.form-group .input-group input.disabled').val(icon);
        $('.form-group .input-group .input-group-btn .btn i').attr('class', icon);
    });
});