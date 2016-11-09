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
                id: 'btnSubmitRole',
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

    $('.fontawesome-icon-list ul li').addClass('col-md-3 col-sm-4 col-sm-6');

    $('.fontawesome-icon-list .fa-hover a, .fontawesome-icon-list ul li').click(function () {
        $('.icon-content .modal-footer i').attr('class', $(this).children('i, span:first').attr('class'));
        return false;
    });

    $('#btnIcon').click(function () {
        $('.icon-content').show();
    });

    $('.icon-content button').click(function () {
        $('.icon-content').hide();
    });

    $('.icon-content button:last').click(function () {
        var icon = $('.icon-content .modal-footer i').attr('class');
        $('#icon').val(icon);
        $('#btnIcon i').attr('class', icon);
    });

    // 排序按钮
    $('#btnMenuOrder').click(function () {
        $('#dialogNew div.modal-dialog').hide();
        $('.menu-content button:last').data('type', 'order');
        // handler new menu
        if ($('#menuID').val() == "") {
            var menuName = $('#name').val();
            if (menuName == "") menuName = "新建菜单-未命名";
            $('div.dd > ol.dd-list').append($.format('<li class="dd-item dd3-item" data-id="0"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><label><span>{0}</span></label></div></li>', menuName));
        }
        var did = $('#menuID').val();
        if (did == "") did = 0;
        $('div.dd input').hide();
        $('div.dd li[data-id="' + did + '"] span').addClass('active');
        $('div.dd > ol.dd-list > li.dd-item').remove('[data-id="0"]');
        $('div.dd > ol.dd-list > li.dd-item').hide();
        $('div.dd > ol.dd-list > li[data-category="' + $('#category').selectpicker('val') + '"]').show();
        $('.menu-content').show();
    });

    // 选择父节点按钮
    $('#btnMenuParent').click(function () {
        $('#dialogNew div.modal-dialog').hide();
        $('.menu-content button:last').data('type', 'parent');
        $('.menu-content').show();
        $('li.dd-item').remove('[data-id="0"]');
        $('div.dd :checkbox').hide();
        $('div.dd > ol.dd-list > li.dd-item').hide();
        $('div.dd > ol.dd-list > li[data-category="' + $('#category').selectpicker('val') + '"]').show();
        $('div.dd :radio').show();
    });

    $('.menu-content button').click(function () {
        // remove active css
        $('div.dd li span').removeClass('active');
        $('.menu-content').hide();
        $('#dialogNew div.modal-dialog').show();
    });

    $('.menu-content button:last').click(function () {
        var type = $('.menu-content button:last').data('type');
        switch (type) {
            case "parent":
                $('#parentId').val($('.dd3-content :radio:checked').val());
                $('#parentName').val($('.dd3-content :radio:checked').next('span').text());
                break;
            case "order":
                var data = $('#nestable_menu').nestable('serialize');
                var mid = $('#menuID').val();
                for (index in data) {
                    window.console.log(index);
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

    $('#nestable_menu').nestable();

    // select
    $('select').selectpicker();
});