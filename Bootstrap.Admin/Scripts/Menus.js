$(function () {
    var bsa = new BootstrapAdmin({
        url: '../api/Menus',
        dataEntity: new DataEntity({
            map: {
                ID: "menuID",
                ParentId: "parentId",
                ParentName: "parentName",
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
                        var dialog = $('#dialogRole');
                        dialog.find('.modal-title').text($.format('{0}-角色授权窗口', row.Name));
                        dialog.find('form').html(data);
                        dialog.modal('show');
                    });
                }
            }, {
                id: 'btnSubmitRole',
                click: function (row) {
                    var menuId = row.ID;
                    var dialog = $('#dialogRole');
                    var roleIds = dialog.find('input:checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    Role.saveRolesByMenuId(menuId, roleIds, { modal: 'dialogRole' });
                }
            }]
        }
    });

    $('table').smartTable({
        url: '../api/Menus',            //请求后台的URL（*）
        sortName: 'Order',
        queryParams: function (params) { return $.extend(params, { parentName: $('#txt_parent_menus_name').val(), name: $("#txt_menus_name").val(), category: $('#sel_menus_category').val() }); },           //传递参数（*）
        columns: [{ checkbox: true },
            { title: "Id", field: "ID", events: bsa.idEvents(), formatter: BootstrapAdmin.idFormatter },
            { title: "父级菜单", field: "ParentName", sortable: true },
            { title: "菜单名称", field: "Name", sortable: true },
            { title: "菜单序号", field: "Order", sortable: true },
            { title: "菜单图标", field: "Icon", sortable: false },
            { title: "菜单路径", field: "Url", sortable: false },
            { title: "菜单类别", field: "CategoryName", sortable: true }
        ]
    });

    // validate
    $('#dataForm').autoValidate({
        name: {
            required: true,
            maxlength: 50
        },
        order: {
            digits: true
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

    var $dialog = $('#dialogNew');
    var $pickIcon = $('#pickIcon');
    var $iconList = $('#iconTab').find('div.fontawesome-icon-list');
    var $dialogNew = $dialog.find('div.modal-dialog');
    var $dialogIcon = $('#dialogIcon');
    var $dialogMenu = $('#dialogSubMenu').find('.modal-content');
    var $btnSubmitMenu = $('#btnSubmitMenu');
    var $btnPickIcon = $('#btnIcon');
    var $inputIcon = $('#icon');
    var $nestMenu = $('#nestable_menu');
    var $nestMenuInput = $nestMenu.find('div.dd3-content');
    var $parentMenuID = $('#parentId');
    var $parentMenuName = $('#parentName');
    $nestMenuInput.find('label:first').hide();

    $iconList.find('ul li').addClass('col-md-3 col-sm-4 col-sm-6');
    $iconList.on('click', 'div.fa-hover a, ul li', function () {
        $pickIcon.attr('class', $(this).find('i, span:first').attr('class'));
        return false;
    });

    $btnPickIcon.on('click', function () {
        $dialogIcon.show();
    });

    $dialogIcon.find('div.modal-header, div.modal-footer').on('click', 'button', function () {
        $dialogIcon.hide();
    });

    $dialogIcon.find('div.modal-footer').on('click', 'button:last', function () {
        var icon = $pickIcon.attr('class');
        $('#icon').val(icon);
        $('#btnIcon').find('i').attr('class', icon);
    });

    // 排序按钮
    $('#btnMenuOrder').on('click', function () {
        $dialogNew.hide();
        $btnSubmitMenu.data('type', 'order');
        $nestMenuInput.find('label:last').find('input').hide();
        $nestMenu.find('li.dd-item').hide().remove('[data-id="0"]');
        $nestMenu.find('li[data-category="' + $('#category').selectpicker('val') + '"]').show();
        // handler new menu
        var did = $('#menuID').val();
        if (did == "") did = 0;
        if (did == 0) {
            var menuName = $('#name').val();
            var menuCate = $('select').selectpicker('val');
            if (menuName == "") menuName = "新建菜单-未命名";
            $nestMenu.find('ol.dd-list:first').append($.format('<li class="dd-item dd3-item" data-id="0" data-category="{1}"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><label><span>{0}</span></label></div></li>', menuName, menuCate));
        }
        $nestMenu.find('li[data-id="' + did + '"] > div.dd3-content span').addClass('active');
        $dialogMenu.show().adjustDialog();
    });

    // 选择父节点按钮
    $('#btnMenuParent').on('click', function () {
        $dialogNew.hide();
        $btnSubmitMenu.data('type', 'parent');
        $nestMenuInput.find('label:last').find('input').show();
        $nestMenu.find('li.dd-item').hide().remove('[data-id="0"]');
        $nestMenu.find('li[data-category="' + $('#category').selectpicker('val') + '"]').show();
        $dialogMenu.show().adjustDialog();
    });

    $dialogMenu.find('div.modal-header, div.modal-footer').on('click', 'button', function () {
        // remove active css
        $nestMenu.find('li span').removeClass('active');
        $dialogMenu.hide();
        $dialogNew.show();
    });

    $btnSubmitMenu.on('click', function () {
        var type = $(this).data('type');
        switch (type) {
            case "parent":
                $('#parentId').val($('.dd3-content :radio:checked').val());
                $('#parentName').val($('.dd3-content :radio:checked').next('span').text());
                break;
            case "order":
                var data = $('#nestable_menu').nestable('serialize');
                var mid = $('#menuID').val();
                for (index in data) {
                    if (data[index].id == mid || data[index] == 0) {
                        $('#order').val(10 + index * 10);
                        break;
                    }
                }
                break;
            default:
                break;
        }
    });

    // Dialog shown event
    $dialog.on('show.bs.modal', function () {
        var icon = $inputIcon.val();
        if (icon == "") icon = "fa fa-dashboard";
        $btnPickIcon.find('i').attr('class', icon);
    });

    $nestMenu.nestable();

    // select
    $('select').selectpicker();
});