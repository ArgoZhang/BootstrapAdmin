$(function () {
    var bsa = new BootstrapAdmin({
        url: '../api/Dicts',
        dataEntity: new DataEntity({
            map: {
                ID: "dictID",
                Category: "dictCate",
                Name: "dictName",
                Code: "dictCode",
                Define: "dictDefine"
            }
        })
    });

    $('table').smartTable({
        url: '../api/Dicts',            //请求后台的URL（*）
        sortName: 'Category',
        queryParams: function (params) { return $.extend(params, { category: $('#txt_dict_cate').val(), name: $("#txt_dict_name").val(), define: $("#txt_dict_define").val() }); },
        columns: [{ checkbox: true },
            { title: "Id", field: "ID", events: bsa.idEvents(), formatter: BootstrapAdmin.idFormatter },
            { title: "字典分项", field: "Category", sortable: true },
            { title: "字典名称", field: "Name", sortable: true },
            { title: "字典代码", field: "Code", sortable: false },
            { title: "字典类别", field: "DefineName", sortable: true }
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