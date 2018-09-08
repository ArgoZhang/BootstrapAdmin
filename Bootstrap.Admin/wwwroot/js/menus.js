$(function () {
    var $dialog = $('#dialogNew');
    var $pickIcon = $('#pickIcon');
    var $dialogNew = $dialog;
    var $dialogIcon = $('#dialogIcon');
    var $dialogMenu = $('#dialogMenu');
    var $dialogRole = $('#dialogRole');
    var $dialogRoleHeader = $('#myRoleModalLabel');
    var $dialogRoleForm = $('#roleForm');
    var $btnSubmitMenu = $('#btnSubmitMenu');
    var $btnPickIcon = $('#btnIcon');
    var $inputIcon = $('#icon');
    var $nestMenu = $('#nestable_menu');
    var $nestMenuInput = $nestMenu.find('div.dd3-content');
    var $parentMenuID = $('#parentId');
    var $parentMenuName = $('#parentName');
    var $category = $('#category');

    var initNestMenu = function () {
        $nestMenuInput = $nestMenu.find('div.dd3-content');
        $nestMenuInput.children('.checkbox').hide();
    };

    var state = [];

    $('table').lgbTable({
        url: Menu.url,
        dataBinder: {
            map: {
                Id: "#menuID",
                ParentId: "#parentId",
                ParentName: "#parentName",
                Name: "#name",
                Order: "#order",
                Icon: "#icon",
                Url: "#url",
                Category: "#category",
                Target: "#target",
                IsResource: "#isRes",
                ApplicationCode: "#app"
            },
            events: {
                '#btn_assignRole': function (row) {
                    $.bc({
                        id: row.Id, url: Role.url, data: { type: "menu" },
                        callback: function (result) {
                            var htmlTemplate = this.htmlTemplate;
                            var html = $.map(result, function (element, index) {
                                return $.format(htmlTemplate, element.Id, element.RoleName, element.Checked, element.Description);
                            }).join('');
                            $dialogRoleHeader.text($.format('{0}-角色授权窗口', row.Name));
                            $dialogRoleForm.html(html).find('[data-toggle="tooltip"]').each(function (index, label) {
                                if (label.title === "") label.title = "未设置";
                            }).tooltip();
                            $dialogRole.modal('show');
                        }
                    });
                },
                '#btnSubmitRole': function (row) {
                    var menuId = row.Id;
                    var roleIds = $dialogRole.find('input:checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    $.bc({ id: menuId, url: Role.url, method: "PUT", data: { type: "menu", roleIds: roleIds }, title: Role.title, modal: '#dialogRole', info: true });
                }
            },
            callback: function (result) {
                if (!result.success) return;
                if ((result.oper === "save") || result.oper === "del") {
                    $nestMenu.nestMenu(initNestMenu);
                }
            }
        },
        smartTable: {
            sortName: 'Order',
            queryParams: function (params) { return $.extend(params, { parentName: $('#txt_parent_menus_name').val(), name: $("#txt_menus_name").val(), category: $('#sel_menus_category').val(), isresource: $('#sel_menus_res').val() }); },           //传递参数（*）
            columns: [
                { title: "父级菜单", field: "ParentName", sortable: true },
                { title: "菜单名称", field: "Name", sortable: true },
                { title: "菜单序号", field: "Order", sortable: true },
                {
                    title: "菜单图标", field: "Icon", sortable: false, align: 'center', formatter: function (value, row, index) {
                        if (value) {
                            return $.format('<i class="text-info {0}"></i>', value);
                        }
                        return "";
                    }
                },
                { title: "菜单路径", field: "Url", sortable: false },
                { title: "菜单类别", field: "CategoryName", sortable: true },
                {
                    title: "目标", field: "Target", sortable: true, formatter: function (value, row, index) {
                        var ret = value;
                        switch (value) {
                            case "_self":
                                ret = "本窗口";
                                break;
                            case "_blank":
                                ret = "新窗口";
                                break;
                            case "_parent":
                                ret = "父级窗口";
                                break;
                            case "_top":
                                ret = "顶级窗口";
                                break;
                            default:
                                break;
                        }
                        return ret;
                    }
                },
                {
                    title: "菜单类型", field: "IsResource", sortable: true, formatter: function (value, row, index) {
                        return value === 0 ? "菜单" : "资源";
                    }
                },
                {
                    title: "所属应用", field: "ApplicationCode", sortable: true, formatter: function (value, row, index) {
                        return $('#app').next().find('[data-val="' + value + '"]:first').text();
                    }
                }
            ]
        }
    });

    // validate
    $('#dataForm').on('click', '[data-method]', function () {
        var $this = $(this);
        var $input = $this.parent().prev();
        switch ($this.attr('data-method')) {
            case 'clear':
                $input.val("");
                break;
            case 'sel':
                $input.select();
                break;
        }
    });

    $btnPickIcon.on('click', function () {
        $dialogNew.find('[data-toggle="LgbValidate"] [aria-describedby]').tooltip('hide');
        $dialogNew.hide();
        var icon = $inputIcon.val();
        if (icon) $pickIcon.attr('class', icon);
        $dialogIcon.show();
    });

    $dialogIcon.find('div.modal-header, div.modal-footer').on('click', 'button', function () {
        $dialogIcon.hide();
        $dialogNew.show();
    });

    $dialogIcon.find('div.modal-footer').on('click', 'button:last', function () {
        var icon = $pickIcon.attr('class');
        $inputIcon.val(icon);
        $btnPickIcon.find('i').attr('class', icon);
    });

    // 排序按钮
    $('#btnMenuOrder').on('click', function () {
        $btnSubmitMenu.data('type', 'order');
        $nestMenuInput.find('label:last').find('input').hide();
        $nestMenu.find('li.dd-item').hide().remove('[data-id="0"]');
        $nestMenu.find('li[data-category="' + $category.val() + '"]').show();
        // handler new menu
        var did = $('#menuID').val();
        if (did === "") did = 0;
        if (did === 0) {
            var menuName = $('#name').val();
            var menuCate = $category.val();
            if (menuName === "") menuName = "新建菜单-未命名";
            $nestMenu.find('ol.dd-list:first').append($.format('<li class="dd-item dd3-item" data-id="0" data-order="10" data-category="{1}"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><label><span>{0}</span></label></div></li>', menuName, menuCate));
        }
        $nestMenu.find('li[data-id="' + did + '"] > div.dd3-content span').addClass('active');
        showDialog();
    });

    // 选择父节点按钮
    $('#btnMenuParent').on('click', function () {
        $btnSubmitMenu.data('type', 'parent');
        $nestMenuInput.find('label:last').find('input').show();
        $nestMenu.find('li.dd-item').hide().remove('[data-id="0"]');
        $nestMenu.find('li[data-category="' + $category.val() + '"]').show();
        showDialog();
    });

    function showDialog() {
        state.push({ css: $('body').attr("class"), style: $('body').attr("style") });
        $dialogNew.find('[data-toggle="LgbValidate"] [aria-describedby]').tooltip('hide');
        $dialogNew.hide();
        $dialogMenu.modal('show');
    };

    $dialogMenu.on('hidden.bs.modal', function () {
        var sta = state.pop();
        $('body').attr('class', sta.css);
        $('body').attr('style', sta.style);
        $dialogNew.show();
    });

    $btnSubmitMenu.on('click', function () {
        $nestMenu.find('li span').removeClass('active');
        var type = $(this).data('type');
        switch (type) {
            case "parent":
                $parentMenuID.val($('.dd3-content :radio:checked').val());
                $parentMenuName.val($('.dd3-content :radio:checked').next('span').text());
                break;
            case "order":
                var data = $nestMenu.find('li:visible');
                var mid = $('#menuID').val();
                for (index in data) {
                    var $data = $(data[index]);
                    if ($data.attr('data-id') === mid || $data.attr('data-id') === 0) {
                        if (index > 0) index--;
                        $('#order').val($(data[index]).attr('data-order'));
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
        if (icon === "") icon = "fa fa-dashboard";
        $btnPickIcon.find('i').attr('class', icon);
    });

    $nestMenu.nestMenu(initNestMenu);

    $.bc({
        url: Menu.iconView,
        contentType: 'text/html',
        dataType: 'html',
        method: 'GET',
        callback: function (result) {
            if (result) {
                $dialogIcon.find('.modal-body').html(result);
                var $iconList = $('div.fontawesome-icon-list').on('click', 'div.fa-hover a, ul li', function () {
                    $pickIcon.attr('class', $(this).find('i, span:first').attr('class'));
                    return false;
                });
                $iconList.find('ul li').addClass('col-xl-2 col-md-3 col-sm-4 col-6');
                $iconList.find('div').addClass('col-xl-2 col-6');
                $('[data-spy="scroll"]').each(function () {
                    $(this).scrollspy({ target: $(this).attr('data-target') });
                });
            }
        }
    });
});