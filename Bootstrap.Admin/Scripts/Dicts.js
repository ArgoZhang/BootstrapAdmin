$(function () {
    var bsa = new BootstrapAdmin({
        url: '../api/Dicts',
        dataEntity: new DataEntity({
            map: {
                ID: "dictID",
                Category: "dictCate",
                Name: "dictName",
                Code: "dictCode",
                Define:"dictDefine"
            }
        })
    });

    $('table').smartTable({
        url: '../api/Dicts',            //请求后台的URL（*）
        sortName: 'Category',
        queryParams: function (params) { return $.extend(params, { name: $("#txt_dict_name").val(), category: $("#txt_dict_cate").val() }); },
        columns: [{ checkbox: true },
            { title: "Id", field: "ID", events: bsa.idEvents(), formatter: BootstrapAdmin.idFormatter },
            { title: "字典种类", field: "Category", sortable: true },
            { title: "字典名称", field: "Name", sortable: false },
            { title: "字典代码", field: "Code", sortable: true },
            { title: "字典定义", field: "Define", sortable: false }
        ]
    });

    // validate
    $('#dataForm').autoValidate({
        dictCate: {
            required: true,
            maxlength: 50
        },
        dictName: {
            required: true,
            maxlength: 50
        },
        dictCode: {
            required: true,
            maxlength: 50
        },
        dictDefine: {
            required: false,
            maxlength: 50
        }
    });
});