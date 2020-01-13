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
    var $sidebar = $('.sidebar .nav-sidebar');

    var initNestMenu = function () {
        $nestMenuInput = $nestMenu.find('div.dd3-content');
        $nestMenuInput.children('.checkbox').hide();
    };

    var state = [];
    var $table = $('table');
    $table.lgbTable({
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
                Application: "#app"
            },
            events: {
                '#btn_assignRole': function (row) {
                    $.bc({
                        id: row.Id, url: Role.url, query: { type: "menu" }, method: "post", htmlTemplate: CheckboxHtmlTemplate,
                        callback: function (result) {
                            var htmlTemplate = this.htmlTemplate;
                            var html = $.map(result, function (element, index) {
                                return $.format(htmlTemplate, element.Id, element.RoleName, element.Checked, element.Description);
                            }).join('');
                            $dialogRoleHeader.text($.format('{0}-角色授权窗口', row.Name));
                            $dialogRoleForm.html(html).find('[data-toggle="tooltip"]').each(function (index, label) {
                                if (label.title === "") label.title = "未设置";
                            }).tooltip();
                            $dialogRoleForm.find('.form-checkbox').lgbCheckbox();
                            $dialogRole.modal('show');
                        }
                    });
                },
                '#btnSubmitRole': function (row) {
                    var menuId = row.Id;
                    var roleIds = $dialogRole.find('input:checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray();
                    $.bc({ id: menuId, url: Menu.url, method: "put", data: roleIds, title: Role.title, modal: '#dialogRole' });
                }
            },
            callback: function (result) {
                if (result.oper === "create") {
                    $('#app').lgbSelect('enable');
                }
                if (result.oper === "edit") {
                    var valid = result.data && result.data.ParentId === "0" && hasNodes(result.data.Id);
                    $('#app').lgbSelect(valid ? 'enable' : 'disabled');
                }
                if (!result.success) return;
                if ((result.oper === "save") || result.oper === "del") {
                    if (result.data.filter(function (element) {
                        return element.Category === "0";
                    }).length > 0) refreshSidebar();
                    $nestMenu.nestMenu(initNestMenu);
                }
            }
        },
        smartTable: {
            pageSize: 100,
            pageList: [100, 200, 400],
            sortName: 'Order',
            queryParams: function (params) { return $.extend(params, { parentName: $('#txt_parent_menus_name').val().trim(), name: $("#txt_menus_name").val().trim(), category: $('#sel_menus_category').val(), isresource: $('#sel_menus_res').val(), appId: $('#sel_app').val() }); },           //传递参数（*）
            exportOptions: {
                fileName: "菜单数据",
                ignoreColumn: [0, 9]
            },
            columns: [
                {
                    title: "菜单名称", field: "Name", sortable: true, formatter: function (value, row, index) {
                        return $.format('<span class="menu">{0}</span>', value);
                    },
                    events: {
                        'click .menu': function (e, value, row, index) {
                            var $plus = $(e.target).prev();
                            if ($plus.hasClass('fa')) {
                                $plus.trigger('click');
                            }
                            return false;
                        }
                    }
                },
                { title: "菜单序号", field: "Order", sortable: true },
                {
                    title: "菜单图标", field: "Icon", sortable: false, align: 'center', formatter: function (value, row, index) {
                        return value ? $.format('<i class="text-info {0}"></i>', value) : "";
                    }
                },
                { title: "菜单路径", field: "Url", sortable: false },
                { title: "菜单类别", field: "CategoryName", sortable: true },
                {
                    title: "目标", field: "Target", sortable: true, formatter: function (value, row, index) {
                        return $('#target').getTextByValue(value);
                    }
                },
                {
                    title: "菜单类型", field: "IsResource", sortable: true, formatter: function (value, row, index) {
                        return $('#isRes').getTextByValue(value);
                    }
                },
                {
                    title: "所属应用", field: "Application", sortable: true, formatter: function (value, row, index) {
                        return $('#app').getTextByValue(value);
                    }
                }
            ],
            idField: "Id",
            rootParentId: "0",
            treeShowField: 'Name',
            parentIdField: 'ParentId',
            onPostBody: function () {
                if ($('#txt_menus_name').val() !== '' || $('#sel_menus_res').val() === '1' || $('#sel_menus_res').val() === '2') {
                    this.treeShowField = false;
                }
                else {
                    this.treeShowField = 'Name';
                }
                var bt = $table.data('bootstrap.table');
                if (bt) {
                    bt.treeEnable = !!this.treeShowField;
                }

                $table.treegrid({
                    treeColumn: 1,
                    expanderExpandedClass: 'fa fa-chevron-circle-down',
                    expanderCollapsedClass: 'fa fa-chevron-circle-down',
                    onChange: function () {
                        $table.bootstrapTable('resetWidth');
                    }
                });
                $table.treegrid('getRootNodes').treegrid('expand');
            }
        }
    });

    var hasNodes = function (idValue) {
        var nodes = $table.bootstrapTable('getData').filter(function (row, index, data) {
            return idValue == row["ParentId"];
        });
        return nodes.length === 0;
    };

    // validate
    $('#dataForm').on('click', '[data-method]', function () {
        var $this = $(this);
        var $input = $this.parent().prev();
        switch ($this.attr('data-method')) {
            case 'clear':
                $input.val("");
                if ($input.attr('id') === 'parentName') {
                    // 判断是否有子项
                    var valid = hasNodes($("#menuID").val());
                    $('#app').lgbSelect(valid ? 'enable' : 'disabled');
                }
                break;
            case 'sel':
                $input.select();
                break;
        }
    });

    // clear parentID value
    // bug https://gitee.com/LongbowEnterprise/dashboard/issues?id=I12E3S
    $parentMenuName.next().find('button[data-method="clear"]').on('click', function () {
        $parentMenuID.val('');
    });

    $btnPickIcon.on('click', function () {
        $dialogNew.find('[data-toggle="LgbValidate"] [aria-describedby]').tooltip('hide');
        $dialogNew.hide();
        var icon = $inputIcon.val();
        if (icon) $pickIcon.attr('class', icon);
        $dialogIcon.show();
        $scroll.resize();
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
        $nestMenu.find('li[data-resource!="0"]').addClass('is-disabled').find(':radio').prop('disabled', true)
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
                // 父级菜单不可以是资源或者按钮类型
                var pId = $('.dd3-content :radio:checked').val();
                var check = $.remoteValidate('api/Category/ValidateParentMenuById/' + pId);
                if (check) {
                    $parentMenuID.val(pId);
                    $parentMenuName.val($('.dd3-content :radio:checked').next('span').text());
                    $('#app').lgbSelect('disabled');
                }
                else {
                    return false;
                }
                break;
            case "order":
                var data = $nestMenu.find('li:visible');
                var mid = $('#menuID').val();
                for (var index in data) {
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

    function refreshSidebar() {
        // 获取当前菜单
        var id = $sidebar.find('.nav-link.active').parent().attr('id');
        $.bc({
            url: Menu.sidebar,
            contentType: 'text/html',
            dataType: 'html',
            callback: function (result) {
                if (result) {
                    $sidebar.html(result);
                    // reactive menu
                    $sidebar.find('.nav-item[id="' + id + '"] .nav-link').addClass('active');
                }
            }
        });
    };

    var $scroll = null;
    $.bc({
        url: Menu.iconView,
        contentType: 'text/html',
        dataType: 'html',
        callback: function (result) {
            if (result) {
                var $html = $dialogIcon.find('.modal-body').html(result);
                var $iconList = $('div.fontawesome-icon-list').on('click', 'div.fa-hover a, ul li', function () {
                    $pickIcon.attr('class', $(this).find('i, span:first').attr('class'));
                    return false;
                });
                $iconList.find('ul li').addClass('col-xl-2 col-md-3 col-sm-4 col-6');
                $iconList.find('div').addClass('col-xl-2 col-6');
                $('[data-spy="scroll"]').each(function () {
                    $(this).scrollspy({ target: $(this).attr('data-target') });
                });

                if (!$.browser.versions.ios) $scroll = $html.find('.fa-nav .nav').overlayScrollbars({ className: 'os-theme-light', scrollbars: { autoHide: 'leave' } });
            }
        }
    });

    // autocomplete
    $.bc({
        url: "api/Category/RetrieveMenus",
        callback: function (result) {
            $('#txt_menus_name').typeahead({
                source: result,
                showHintOnFocus: 'all',
                fitToElement: true,
                items: 'all'
            });
        }
    });

    $.bc({
        url: "api/Category/RetrieveParentMenus",
        callback: function (result) {
            $('#txt_parent_menus_name').typeahead({
                source: result,
                showHintOnFocus: 'all',
                fitToElement: true,
                items: 'all'
            });
        }
    });

    // 所属应用更新是联动菜单类别
    var $app = $('#app').on('changed.lgbSelect', function (e) {
        var defaultVal = $app.attr('data-default-val');
        var val = defaultVal === $app.val() ? '0' : '1';
        $category.lgbSelect('val', val);
    })

    if ($.isFunction($.validator)) {
        $.validator.addMethod("menuChild", function (value, element) {
            var id = $("#menuID").val();
            var check = id === "" || value === "菜单" || $.remoteValidate('api/Category/ValidateMenuBySubMenu/' + id);
            return check;
        }, "拥有子菜单时菜单类型不可更改为资源或者按钮");
    }
});