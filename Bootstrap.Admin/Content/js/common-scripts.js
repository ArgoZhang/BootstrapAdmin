$(function () {
    var $sidebar = $("#sidebar");
    var $main = $('#main-content');

    $('#nav-accordion').dcAccordion({
        eventType: 'click',
        autoClose: true,
        saveState: true,
        disableLink: true,
        speed: 'slow',
        showCount: false,
        autoExpand: true,
        //        cookie: 'dcjq-accordion-1',
        classExpand: 'dcjq-current-parent'
    });

    $("#gotoTop").on('click', function (e) {
        e.preventDefault();
        $('#main-content, .content-body, body').animate({
            scrollTop: 0
        }, 200);
    });

    $('#websiteFooter').text($('#footer').val());

    $sidebar.on('click', 'a', function () {
        var o = ($(this).offset());
        diff = 300 - o.top;
        if (diff > 0)
            $sidebar.scrollTo("-=" + Math.abs(diff), 500);
        else
            $sidebar.scrollTo("+=" + Math.abs(diff), 500);
    });

    $('.sidebar-toggle-box').on('click', function () {
        if ($sidebar.is(":visible") === true) {
            $sidebar.hide();
            $main.addClass('closed').removeClass('open');
        } else {
            $sidebar.show();
            $main.addClass('open').removeClass('closed');
        }
    });

    // custom scrollbar
    if (!$.browser.versions.ios) $("#sidebar").niceScroll({ styler: "fb", cursorcolor: "#e8403f", cursorwidth: '3', cursorborderradius: '10px', background: '#404040', spacebarenabled: false, cursorborder: '', scrollspeed: 60 });

    // load widget data
    $.bc({
        url: '../api/Notifications/',
        swal: false,
        method: 'GET',
        callback: function (result) {
            $('#logoutNoti').text(result.NewUsersCount);

            // tasks
            // new users
            $('#msgHeaderTask').text(result.NewUsersCount);
            $('#msgHeaderTaskBadge').text(result.NewUsersCount);
            var htmlUserTemplate = '<li><a href="../Admin/Tasks?id={3}"><span class="desc">{0}-{2}</span><span class="percent">{1}%</span></span><div class="progress progress-striped"><div class="progress-bar" role="progressbar" aria-valuenow="{1}" aria-valuemin="0" aria-valuemax="100" style="width: {1}%"><span class="sr-only">{1}% 完成</span></div></div></a></li>';
            var html = result.Tasks.map(function (u) {
                return $.format(htmlUserTemplate, u.TaskName, u.TaskProgress, u.AssignDisplayName, u.ID);
            }).join('');
            $(html).insertAfter($('#msgHeaderTaskContent'));

            // new users
            $('#msgHeaderUser').text(result.NewUsersCount);
            $('#msgHeaderUserBadge').text(result.NewUsersCount);
            htmlUserTemplate = '<li><a href="../Admin/Notifications"><span class="label label-success"><i class="fa fa-plus"></i></span><div title="{2}" class="content">{1}({0})</div><span class="small italic">{3}</span></a></li>';
            html = result.Users.map(function (u) {
                return $.format(htmlUserTemplate, u.UserName, u.DisplayName, u.Description, u.Period);
            }).join('');
            $(html).insertAfter($('#msgHeaderUserContent'));

            // apps
            $('#msgHeaderApp').text(result.AppExceptionsCount);
            $('#msgHeaderAppBadge').text(result.AppExceptionsCount);
            htmlUserTemplate = '<li><a href="../Admin/Exceptions"><span class="label label-warning"><i class="fa fa-bug"></i></span><div title="{1}" class="content">{0}</div><span class="small italic">{2}</span></a></li>';
            html = result.Apps.map(function (u) {
                return $.format(htmlUserTemplate, u.ExceptionType, u.Message, u.Period);
            }).join('');
            $(html).insertAfter($('#msgHeaderAppContent'));

            // dbs
            $('#msgHeaderDb').text(result.DbExceptionsCount);
            $('#msgHeaderDbBadge').text(result.DbExceptionsCount);
            htmlUserTemplate = '<li><a href="../Admin/Exceptions"><span class="label label-danger"><i class="fa fa-bolt"></i></span><div title="{1}" class="content">{0}</div><span class="small italic">{2}</span></a></li>';
            html = result.Dbs.map(function (u) {
                return $.format(htmlUserTemplate, u.ErrorPage, u.Message, u.Period);
            }).join('');
            $(html).insertAfter($('#msgHeaderDbContent'));

            // messages
            $('#msgHeaderMsg').text(result.MessagesCount);
            $('#msgHeaderMsgBadge').text(result.MessagesCount);
            htmlUserTemplate = '<li><a href="../Admin/Messages?id={0}"><span class="photo"><img alt="avatar" src="{1}"></span><span class="subject"><span class="from">{2}</span><span class="time">{4}</span></span><span class="message" title="{5}">{3}</span></a></li>';
            html = result.Messages.map(function (u) {
                return $.format(htmlUserTemplate, u.ID, u.FromIcon, u.FromDisplayName, u.Title, u.Period, u.Content);
            }).join('');
            $(html).insertAfter($('#msgHeaderMsgContent'));
        }
    });
});