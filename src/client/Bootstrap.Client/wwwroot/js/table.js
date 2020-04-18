$(function () {
    $('#modalTable').smartTable({
        search: false,
        columns: [
            { title: "示例属性1", field: "Item1", sortable: true },
            { title: "示例属性2", field: "Item2", sortable: true },
            { title: "示例属性3", field: "Item3", sortable: true, formatter: function (value) { return value === 0 ? "系统使用" : "自定义"; } }
        ],
    });

    $('table:first').lgbTable({
        url: 'api/Dummy',
        dataBinder: {
            map: {
                Id: "#dummyID",
                Item1: "#item1",
                Item2: "#item2",
                Item3: "#item3"
            },
            events: {
                '.info': function (row) {
                    $('#tableDemo').modal('show');
                },
            }
        },
        smartTable: {
            search: true,
            sortName: 'item1',
            queryParams: function (params) {
                return $.extend(params, {
                    item1: $('#item_query_1').val(),
                    item2: $("#item_query_2").val(),
                    item3: $("#item_query_3").val()
                });
            },
            columns: [
                { title: "示例属性1", field: "Item1", sortable: false },
                { title: "示例属性2", field: "Item2", sortable: false },
                { title: "示例属性3", field: "Item3", sortable: true, formatter: function (value) { return value === 0 ? "系统使用" : "自定义"; } }
            ],
            exportOptions: {
                fileName: "下载示例文件",
                ignoreColumn: [0, 5]
            }
        }
    });
});