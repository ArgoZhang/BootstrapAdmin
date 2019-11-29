$(function () {
    $('table').lgbTable({
        url: Dicts.url,
        dataBinder: {
            map: {
                Id: "#dictID",
                Category: "#dictCate",
                Name: "#dictName",
                Code: "#dictCode",
                Define: "#dictDefine"
            }
        },
        smartTable: {
            sortName: 'Category',
            queryParams: function (params) { return $.extend(params, { category: $('#txt_dict_cate').val(), name: $("#txt_dict_name").val(), define: $("#txt_dict_define").val() }); },
            columns: [
                { title: "字典标签", field: "Category", sortable: true },
                { title: "字典名称", field: "Name", sortable: true },
                { title: "字典代码", field: "Code", sortable: true },
                { title: "字典类型", field: "Define", sortable: true, formatter: function (value) { return value === 0 ? "系统使用" : "自定义"; } }
            ],
            exportOptions: {
                fileName: "数据字典",
                ignoreColumn: [0, 5]
            }
        }
    });

    // autocomplete
    $.bc({
        url: "api/Category/RetrieveDictCategorys",
        callback: function (result) {
            $('#txt_dict_cate').typeahead({
                source: result,
                showHintOnFocus: 'all',
                fitToElement: true,
                items: 'all'
            });
        }
    });
});