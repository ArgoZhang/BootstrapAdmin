$(function () {
    var $btnRefreshUser = $('#refreshUsers');
    var htmlNewUsersHeader = '<li class="task-header"><div class="task-title notifi"><span class="task-title-sp">登陆名称</span><span class="task-value">显示名称/备注</span><span class="task-time">注册时间</span><div class="pull-right task-oper">操作</div></div></li>';
    var htmlNewUsers = '<li class="list-primary"><i class="fa fa-ellipsis-v"></i><div class="task-title notifi"><span class="task-title-sp">{0}</span><span class="task-value">{4}：{1}</span><span class="task-time">{2}</span><div class="pull-right hidden-phone"><button class="btn btn-success btn-xs fa fa-check" role="tooltip" data-id="{3}" data-result="1" title="同意授权"></button><button class="btn btn-danger btn-xs fa fa-remove" role="tooltip" data-id="{3}" data-result="0" title="拒绝授权"></button></div></div></li>';

    function listData() {
        $btnRefreshUser.toggleClass('fa-spin');
        var $taskUsers = $('#tasks-users');
        $taskUsers.html(htmlNewUsersHeader);
        $.bc({
            Id: 'newusers', url: Notifications.url, method: 'GET', swal: false,
            callback: function (result) {
                if (result) {
                    var content = result.Users.map(function (noti) {
                        return $.format(htmlNewUsers, noti.UserName, noti.Description, noti.RegisterTime, noti.ID, noti.DisplayName);
                    }).join('');
                    $taskUsers.append(content);
                    $('#tasks-users').find('[role="tooltip"]').lgbTooltip();
                }
                $btnRefreshUser.toggleClass('fa-spin');
            }
        });
    }
    listData();

    $btnRefreshUser.lgbTooltip().on('click', function () {
        listData();
    });

    $('#tasks-users').on('click', 'button', function () {
        var id = $(this).attr('data-id');
        var result = $(this).attr('data-result');
        $.bc({
            Id: id, url: User.url, method: "PUT", data: { type: "user", userIds: result }, title: result == "1" ? "授权用户" : "拒绝用户",
            callback: function (result) {
                listData({ ctl: $('#refreshUsers') });
            }
        });
    });
});