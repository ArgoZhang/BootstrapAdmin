$(function () {
    var url = '../api/Tasks/';

    var htmlTask = '<li class="list-primary"><i class="fa fa-ellipsis-v"></i><div class="task-title notifi"><span class="task-title-sp">{0}</span><span class="task-value">{1}</span><span class="task-time">{2}</span>';
    htmlTask += '<div class="pull-right hidden-phone"><button class="btn btn-success btn-xs fa fa-check" data-id="{3}" data-result="1"></button><button class="btn btn-danger btn-xs fa fa-remove" data-id="{3}" data-result="0" data-placement="left" data-original-title="拒绝授权"></button></div></div></li>';

    function listData(options) {
        options = $.extend({ url: url, animation: true, ctl: $('a.fa-refresh') }, options);
        if (options.animation) options.ctl.toggleClass('fa-spin');
        $.ajax({
            url: options.url,
            type: 'GET',
            success: function (result) {
                if (result) {
                    var content = result.Users.map(function (task) {
                        var t = new Date(task.AssignTime).format('yyyy-MM-dd HH:mm:ss');
                        return $.format(htmlTask, task.TaskName, task.UserName, t, task.ID);
                    }).join('');
                    $('#list-task').html(content);
                }
                if (options.animation) options.ctl.toggleClass('fa-spin');
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (options.animation) options.ctl.toggleClass('fa-spin');
            }
        });
    }
    listData();

    $('a.fa-refresh').on('click', function () {
        listData({ ctl: $(this) });
    });
})