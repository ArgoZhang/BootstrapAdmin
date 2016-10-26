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
        })
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