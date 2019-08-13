$(function () {
    var $table = $('table').smartTable({
        sidePagination: "client",
        showToggle: false,
        showRefresh: false,
        showColumns: false,
        columns: [
            { title: "分类", field: "name", sortable: true },
            { title: "描述", field: "Description", sortable: true },
            { title: "异常信息", field: "Exception", sortable: true },
            { title: "明细数据", field: "Data", sortable: true },
            { title: "检查结果", field: "Status", sortable: false },
            { title: "耗时", field: "Duration", sortable: true }
        ]
    });

    $.bc({
        url: 'healths',
        callback: function (result) {
            console.log(result);
        }
    });
});