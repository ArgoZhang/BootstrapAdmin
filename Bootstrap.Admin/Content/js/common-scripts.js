$(function () {
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

    // breadcrumb
    var breadcrumb = $('.sidebar-menu > li > a.active > span').text();
    if (breadcrumb === "") $('.breadcrumb > li + li').hide();
    else $('.breadcrumb > li + li').text(breadcrumb);

    $(".go-top").click(function (e) {
        e.preventDefault();
        $('#main-content, .content-body, body').animate({
            scrollTop: 0
        }, 200);
    });

    $('#websiteFooter').text($('#footer').val());

    $('#sidebar .sub-menu > a').click(function () {
        var o = ($(this).offset());
        diff = 300 - o.top;
        if (diff > 0)
            $("#sidebar").scrollTo("-=" + Math.abs(diff), 500);
        else
            $("#sidebar").scrollTo("+=" + Math.abs(diff), 500);
    });

    $('.sidebar-toggle-box').click(function () {
        if ($('#sidebar').is(":visible") === true) {
            $(".sidebar").addClass("sidebar-closed");
        } else {
            $(".sidebar").removeClass("sidebar-closed");
            $("#sidebar").show();
        }
    });

    // custom scrollbar
    if (!$.browser.versions.ios) $("#sidebar").niceScroll({ styler: "fb", cursorcolor: "#e8403f", cursorwidth: '3', cursorborderradius: '10px', background: '#404040', spacebarenabled: false, cursorborder: '', scrollspeed: 60 });

    // load widget data
    bd({
        url: '../api/Notifications/',
        swal: false,
        method: 'GET',
        callback: function (result) {
            $('#logoutNoti').text(result.NewUsersCount);

            // new users
            $('#msgHeaderUser').text(result.NewUsersCount);
            $('#msgHeaderUserBadge').text(result.NewUsersCount);
            var htmlUserTemplate = '<li><a href="../Admin/Notifications"><span class="label label-success"><i class="fa fa-plus"></i></span><div title="{2}">{1}({0})</div><span class="small italic">{3}</span></a></li>';
            var html = result.Users.map(function (u) {
                return $.format(htmlUserTemplate, u.UserName, u.DisplayName, u.Description, u.Period);
            }).join('');
            $(html).insertAfter($('#msgHeaderUserContent'));

            // apps
            $('#msgHeaderApp').text(result.AppExceptionsCount);
            $('#msgHeaderAppBadge').text(result.AppExceptionsCount);
            htmlUserTemplate = '<li><a href="../Admin/Exceptions"><span class="label label-warning"><i class="fa fa-bug"></i></span><div title="{1}">{0}</div><span class="small italic">{2}</span></a></li>';
            html = result.Apps.map(function (u) {
                return $.format(htmlUserTemplate, u.ExceptionType, u.Message, u.Period);
            }).join('');
            $(html).insertAfter($('#msgHeaderAppContent'));

            // dbs
            $('#msgHeaderDb').text(result.DbExceptionsCount);
            $('#msgHeaderDbBadge').text(result.DbExceptionsCount);
            htmlUserTemplate = '<li><a href="../Admin/Exceptions"><span class="label label-danger"><i class="fa fa-bolt"></i></span><div title="{1}">{0}</div><span class="small italic">{2}</span></a></li>';
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
            console.log(result);
        }
    });
});