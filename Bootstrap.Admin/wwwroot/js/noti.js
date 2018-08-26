$(function () {
    var $table = $('table').smartTable({
        url: Notifications.url + "newusers",
        sidePagination: "client",
        showToggle: false,
        showRefresh: false,
        showColumns: false,
        columns: [
            { title: "登陆名称", field: "UserName" },
            { title: "显示名称", field: "DisplayName" },
            { title: "说明信息", field: "Description" },
            { title: "注册时间", field: "RegisterTime" },
            {
                title: "操作", field: "Id", formatter: function (value, row, index, field) {
                    return $.format('<button class="btn btn-success" data-toggle="tooltip" data-id="{0}" data-result="1" title="同意授权"><i class="fa fa-check"></i></button> <button class="btn btn-danger" data-toggle="tooltip" data-id="{0}" data-result="0" title="拒绝授权"><i class="fa fa-remove"></i></button>', value);
                }
            }
        ]
    }).on('click', 'button[data-id]', function () {
        var $this = $(this);
        var id = $this.attr('data-id');
        var result = $this.attr('data-result');
        $.bc({
            id: id, url: User.url, method: "PUT", data: { type: "user", userIds: result }, title: result == "1" ? "授权用户" : "拒绝用户",
            callback: function (result) {
                $table.bootstrapTable('refresh');
                $.pullNotification($('.header .nav').reloadWidget());
            }
        });
    });

    $('#refreshUsers').tooltip().on('click', function () {
        $table.bootstrapTable('refresh');
    });
});