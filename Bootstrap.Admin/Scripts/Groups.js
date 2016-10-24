$(function () {
    var bsa = new BootstrapAdmin({
        url: '../api/Groups',
        dataEntity: new DataEntity({
            map: {
                ID: "groupID",
                GroupName: "groupName",
                Description: "groupDesc"
            }
        })
    });

    $('table').smartTable({
        url: '../api/Groups',            //请求后台的URL（*）
        sortName: 'GroupName',
        queryParams: function (params) { return $.extend(params, { groupName: $("#txt_search_name").val(), description: $("#txt_group_desc").val() }); },           //传递参数（*）
        columns: [{ checkbox: true },
            { title: "Id", field: "ID", events: bsa.idEvents(), formatter: BootstrapAdmin.idFormatter },
            { title: "部门名称", field: "GroupName", sortable: true },
            { title: "部门描述", field: "Description", sortable: true }
        ]
    });

    // validate
    $('#dataForm').autoValidate({
        groupName: {
            required: true,
            maxlength: 50
        }
    });
});