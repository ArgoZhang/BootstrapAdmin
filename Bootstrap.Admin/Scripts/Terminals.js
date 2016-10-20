$(function () {
    var dataEntity = new DataEntity({
        map: {
            ID: "terminalId",
            Name: "terminalName",
            ClientIP: "clientIP",
            ClientPort: "clientPort",
            ServerIP: "serverIP",
            ServerPort: "serverPort",
            DeviceIP: "deviceIP",
            DevicePort: "devicePort",
            DatabaseName: "databaseName",
            DatabaseUserName: "databaseUserName",
            DatabasePassword: "databasePassword"
        }
    });

    var extender = new ExtenderChecker({
        url: '../api/Terminals',
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
        url: '../api/Terminals',            //请求后台的URL（*）
        method: 'get',                      //请求方式（*）
        toolbar: '#toolbar',                //工具按钮用哪个容器
        striped: true,                      //是否显示行间隔色
        cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
        pagination: true,                   //是否显示分页（*）
        sortable: true,                     //是否启用排序
        sortName: 'Name',
        sortOrder: "asc",                   //排序方式
        queryParams: function (params) { return $.extend(params, { name: $("#txt_search_name").val(), ip: $("#txt_search_ip").val() }); },           //传递参数（*）
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
            { title: "Id", field: "ID", events: idEvents, formatter: ExtenderChecker.idFormatter },
            { title: "输入口名称", field: "Name", sortable: true },
            { title: "工控机IP", field: "ClientIP", sortable: false },
            { title: "工控机Port", field: "ClientPort", sortable: false },
            { title: "服务器IP", field: "ServerIP", sortable: false },
            { title: "服务器Port", field: "ServerPort", sortable: false },
            { title: "比对设备IP", field: "DeviceIP", sortable: false },
            { title: "比对设备Port", field: "DevicePort", sortable: false },
            { title: "数据库名称", field: "DatabaseName", sortable: false },
            { title: "数据库用户名", field: "DatabaseUserName", sortable: false },
            { title: "数据库密码", field: "DatabasePassword", sortable: false }
        ]
    });

    // validate
    $("#dataForm").validate({
        ignore: "ignore",
        rules: {
            terminalName: {
                required: true,
                maxlength: 50
            },
            clientIP: {
                required: true,
                ip: true,
                maxlength: 15
            },
            clientPort: {
                required: true,
                digits: true,
                range: [0, 65535],
                maxlength: 5
            },
            serverIP: {
                required: true,
                ip: true,
                maxlength: 15
            },
            serverPort: {
                required: true,
                digits: true,
                range: [1000, 65535],
                minlength: 4
            },
            deviceIP: {
                required: true,
                ip: true,
                maxlength: 15
            },
            devicePort: {
                required: true,
                digits: true,
                range: [1000, 65535],
                minlength: 4
            },
            databaseName: {
                maxlength: 50
            },
            databaseUserName: {
                maxlength: 50
            },
            databasePassword: {
                maxlength: 50
            }
        },
        unhighlight: function (element, errorClass, validClass) {
            $.validator.defaults.unhighlight(element, errorClass, validClass);
            $(element).popover('destroy');
        },
        errorPlacement: function (label, element) {
            $(element).popover('destroy');
            $(element).popover({
                animation: true,
                delay: { "show": 100, "hide": 100 },
                container: 'form',
                trigger: 'manual',
                content: $(label).text(),
                placement: 'auto'
            });
            $(element).popover('show');
        }
    });
});