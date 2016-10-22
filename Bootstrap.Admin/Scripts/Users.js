$(function () {
    var dataEntity = new DataEntity({
        map: {
            ID: "userID",
            UserName: "userName",
            Password: "password"
        }
    });

    var extender = new BootstrapAdmin({
        url: '../api/Users',
        dataEntity: dataEntity,
        click: {
            query: 'btn_query',
            create: 'btn_add',
            edit: 'btn_edit',
            del: 'btn_delete',
            save: 'btnSubmit'
        }
    });

    idEvents = {
        'click .edit': function (e, value, row, index) {
            dataEntity.load(row);
            $('table').bootstrapTable('uncheckAll');
            $('table').bootstrapTable('check', index);
            $("#dialogNew").modal("show");
        }
    };

    $('table').bootstrapTable({
        url: '../api/Users',            //请求后台的URL（*）
        method: 'get',                      //请求方式（*）
        toolbar: '#toolbar',                //工具按钮用哪个容器
        striped: true,                      //是否显示行间隔色
        cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
        pagination: true,                   //是否显示分页（*）
        sortable: true,                     //是否启用排序
        sortName: 'UserName',
        sortOrder: "asc",                   //排序方式
        queryParams: function (params) { return $.extend(params, { name: $("#txt_search_name").val() }); },           //传递参数（*）
        sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
        pageNumber: 1,                      //初始化加载第一页，默认第一页
        pageSize: 10,                       //每页的记录行数（*）
        pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
        search: false,                      //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
        strictSearch: false,
        showColumns: true,                  //是否显示所有的列
        showRefresh: true,                  //是否显示刷新按钮
        minimumCountColumns: 2,             //最少允许的列数
        clickToSelect: false,               //是否启用点击选中行
        //height: 500,                      //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
        idField: "Id",
        uniqueId: "Id",                     //每一行的唯一标识，一般为主键列
        showToggle: true,                   //是否显示详细视图和列表视图的切换按钮
        cardView: false,                    //是否显示详细视图
        detailView: false,                  //是否显示父子表
        clickToSelect: false,
        columns: [{ checkbox: true },
            { title: "Id", field: "ID", events: idEvents, formatter: BootstrapAdmin.idFormatter },
            { title: "用户名称", field: "UserName", sortable: true }
        ]
    });

    // validate
    $('#dataForm').autoValidate({
        userName: {
            required: true,
            maxlength: 50
        },
        password: {
            required: true,
            maxlength: 50
        },
        confirm: {
            required: true,
            equalTo: "#password"
        }
    });
});