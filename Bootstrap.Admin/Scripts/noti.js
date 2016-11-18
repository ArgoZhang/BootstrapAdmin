$(function () {
    var htmlNewUsers = '<li class="list-primary"><i class="fa fa-ellipsis-v"></i><div class="task-title notifi"><span class="task-title-sp">{0}</span><span class="task-value">{1}</span><span class="task-time">{2}</span><div class="pull-right hidden-phone"><button class="btn btn-success btn-xs fa fa-check" data-id="{3}" data-result="1"></button><button class="btn btn-danger btn-xs fa fa-remove" data-id="{3}" data-result="0" data-placement="left" data-original-title="拒绝授权"></button></div></div></li>';
    var htmlApp = '<li class="list-warning"><i class="fa fa-ellipsis-v"></i><div class="task-title notifi"><span class="task-title-sp">{0}</span><span class="task-value">{1}</span><span class="task-time">{2}</span><div class="pull-right hidden-phone"><a href="../Admin/Exceptions?id={3}" class="btn btn-warning btn-xs fa fa-bug"></a></div></div></li>';
    var htmlDb = '<li class="list-danger"><i class="fa fa-ellipsis-v"></i><div class="task-title notifi"><span class="task-title-sp">{0}</span><span class="task-value">{1}</span><span class="task-time">{2}</span><div class="pull-right hidden-phone"><a href="../Admin/Exceptions?id={3}" class="btn btn-danger btn-xs fa fa-database"></a></div></div></li>';

    function listData(options) {
        options = $.extend({ animation: true, ctl: $('a.fa-refresh') }, options);
        var category = options.ctl.length == 1 ? options.ctl.attr('data-category') : "all";
        if (options.animation) options.ctl.toggleClass('fa-spin');
        Notifications.retrieveNotifies(category, function (result) {
            if (result) {
                if (category == 'newusers' || category == 'all') {
                    var content = result.Users.map(function (noti) {
                        return $.format(htmlNewUsers, noti.UserName, noti.Description, noti.RegisterTime, noti.ID);
                    }).join('');
                    $('#tasks-users').html(content);
                }
                if (category == 'apps' || category == 'all') {
                    var content = result.Apps.map(function (noti) {
                        return $.format(htmlApp, noti.Title, noti.Content, noti.RegisterTime, noti.ID);
                    }).join('');
                    $('#tasks-app').html(content);
                }
                if (category == 'dbs' || category == 'all') {
                    var content = result.Dbs.map(function (noti) {
                        return $.format(htmlDb, noti.Title, noti.Content, noti.RegisterTime, noti.ID);
                    }).join('');
                    $('#tasks-db').html(content);
                }
            }
            if (options.animation) options.ctl.toggleClass('fa-spin');
        });
    }
    listData();

    $('a.fa-refresh').on('click', function () {
        listData({ ctl: $(this) });
    });

    $('#tasks-users').on('click', 'button', function () {
        var id = $(this).attr('data-id');
        var result = $(this).attr('data-result');
        User.processUser(id, result, function (result) {
            var refresh = $('#tasks-users').parentsUntil('div.panel').last().prev().find('a.fa-refresh');
            listData(refresh);
        });
    });
});