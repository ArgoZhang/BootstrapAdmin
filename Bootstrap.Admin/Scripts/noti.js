$(function () {
    var htmlNewUsers = '<li class="list-primary"><i class="fa fa-ellipsis-v"></i><div class="task-title notifi"><span class="task-title-sp">{0}</span><span class="task-value">{1}</span><span class="task-time">{2}</span><div class="pull-right hidden-phone"><button class="btn btn-success btn-xs fa fa-check" data-id="{3}" data-result="1"></button><button class="btn btn-danger btn-xs fa fa-remove" data-id="{3}" data-result="0" data-placement="left" data-original-title="拒绝授权"></button></div></div></li>';

    function listData(options) {
        options = $.extend({ animation: true, ctl: $('a.fa-refresh') }, options);
        if (options.animation) options.ctl.toggleClass('fa-spin');
        bd({
            Id: 'newusers',
            url: '../api/Notifications/',
            method: 'GET',
            swal: false,
            callback: function (result) {
                if (result) {
                    var content = result.Users.map(function (noti) {
                        return $.format(htmlNewUsers, noti.UserName, noti.Description, noti.RegisterTime, noti.ID);
                    }).join('');
                    $('#tasks-users').html(content);
                }
                if (options.animation) options.ctl.toggleClass('fa-spin');
            }
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
            listData({ ctl: $('#refreshUsers') });
        });
    });
});