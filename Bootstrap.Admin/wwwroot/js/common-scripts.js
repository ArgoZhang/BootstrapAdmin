(function ($) {
    var cascadeMenu = function (menus) {
        var html = "";
        $.each(menus, function (index, menu) {
            if (menu.Menus.length == 0) {
                html += $.format('<li class="dd-item dd3-item" data-id="{0}" data-order="{4}" data-category="{3}"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><div class="checkbox"><label><input type="checkbox" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><div class="radio"><label><input type="radio" name="menu" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><span class="menuType">{5}</span><span class="menuOrder">{4}</span></div></li>', menu.Id, menu.Icon, menu.Name, menu.Category, menu.Order, menu.CategoryName);
            }
            else {
                html += $.format('<li class="dd-item dd3-item" data-id="{0}" data-order="{5}" data-category="{3}"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><div class="checkbox"><label><input type="checkbox" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><div class="radio"><label><input type="radio" name="menu" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><span class="menuType">{6}</span><span class="menuOrder">{5}</span></div><ol class="dd-list">{4}</ol></li>', menu.Id, menu.Icon, menu.Name, menu.Category, cascadeSubMenu(menu.Menus), menu.Order, menu.CategoryName);
            }
        });
        return html;
    };

    var cascadeSubMenu = function (menus) {
        var html = ""
        $.each(menus, function (index, menu) {
            html += $.format('<li class="dd-item dd3-item" data-id="{0}" data-order="{4}" data-category="{3}"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><div class="checkbox"><label><input type="checkbox" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><div class="radio"><label><input type="radio" name="menu" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><span class="menuType">{5}</span><span class="menuOrder">{4}</span></div></li>', menu.Id, menu.Icon, menu.Name, menu.Category, menu.Order, menu.CategoryName);
        });
        return html;
    };

    $.fn.extend({
        nestMenu: function (callback) {
            var $this = $(this);
            $.bc({
                id: 0, url: Menu.url, data: { type: "user" }, swal: false,
                callback: function (result) {
                    var html = "";
                    if ($.isArray(result)) html = cascadeMenu(result);
                    $this.find('ol:first').html(html);
                    $this.nestable();
                    callback();
                }
            });
        },
        resetWidget: function () {
            var widgets = $(this).children('li');
            widgets.each(function () {
                var widget = $(this).children('ul');
                if (widget.children().length === 3) return;
                var last = widget.children(':last');
                while (widget.children().length > 3) {
                    widget.children(':eq(2)').remove();
                }
            });
            return $(this);
        }
    });

    $.extend({
        pullNotification: function () {
            if ($('.notify-row').length == 0) return;

            var uri = "ws://" + window.location.host + $.formatUrl("WS");
            var socket = new WebSocket(uri);
            socket.onmessage = function (e) {
                var result = JSON.parse(e.data);
                for (index in result) {
                    var cate = result[index].Category;
                    var msg = result[index].Message;
                    switch (cate) {
                        case "Notification":
                            toastr.error(msg, "应用程序出现错误");
                            break;
                        case "Users":
                            toastr.info(msg, "新用户注册");
                            break;
                    }
                };
                if (result.length > 0) $.reloadWidget();
            };
        },
        reloadWidget: function () {
            if ($('.notify-row').length == 0) return;
            $.bc({
                url: Notifications.url,
                swal: false,
                method: 'GET',
                callback: function (result) {
                    $('#logoutNoti').text(result.NewUsersCount);
                    $('.notify-row').resetWidget();
                    // tasks
                    // new users
                    $('#msgHeaderTask').text(result.TasksCount);
                    $('#msgHeaderTaskBadge').text(result.TasksCount);
                    var htmlUserTemplate = '<li><a href="../Admin/Tasks?id={3}"><span class="desc">{0}-{2}</span><span class="percent">{1}%</span></span><div class="progress progress-striped"><div class="progress-bar" role="progressbar" aria-valuenow="{1}" aria-valuemin="0" aria-valuemax="100" style="width: {1}%"><span class="sr-only">{1}% 完成</span></div></div></a></li>';
                    var html = result.Tasks.map(function (u) {
                        return $.format(htmlUserTemplate, u.TaskName, u.TaskProgress, u.AssignDisplayName, u.Id);
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
                        return $.format(htmlUserTemplate, u.Id, u.FromIcon, u.FromDisplayName, u.Title, u.Period, u.Content);
                    }).join('');
                    $(html).insertAfter($('#msgHeaderMsgContent'));
                }
            });
        }
    });
})(jQuery);

$(function () {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "progressBar": true,
        "positionClass": "toast-bottom-right",
        "onclick": null,
        "showDuration": "600",
        "hideDuration": "2000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    var $sidebar = $("#sidebar");
    var $main = $('#main-content');
    var $breadNav = $('#breadNav');

    $('#nav-accordion').dcAccordion({
        autoExpand: true
    });

    // custom scrollbar
    if (!$.browser.versions.ios) $("#sidebar").niceScroll({ styler: "fb", cursorcolor: "#e8403f", cursorwidth: '3', cursorborderradius: '10px', background: '#404040', spacebarenabled: false, cursorborder: '', scrollspeed: 60 });

    $("#gotoTop").on('click', function (e) {
        e.preventDefault();
        $('#main-content, .content-body, body').animate({
            scrollTop: 0
        }, 200);
    });
    $('[data-toggle="tooltip"]').tooltip();

    // breadcrumb
    var arch = $('#nav-accordion').find('a.active').last();
    $breadNav.text(arch.text() || $('title').text());
    var top = (arch.offset() || { top: 0 }).top;
    if (top > 0) {
        var middle = $('header').outerHeight() + $sidebar.outerHeight() / 2;
        if (top > middle) $sidebar.animate({ scrollTop: top + arch.outerHeight() / 2 - middle }, 500);
    }

    $sidebar.on('click', 'a.dcjq-parent', function () {
        var o = ($(this).offset());
        diff = 110 - o.top;
        if (diff > 0)
            $sidebar.scrollTo("-=" + Math.abs(diff), 500);
        else
            $sidebar.scrollTo("+=" + Math.abs(diff), 500);
    });

    $('.sidebar-toggle-box').on('click', function () {
        if ($sidebar.is(":visible") === true) {
            $main.addClass('closed').removeClass('open');
            $sidebar.parent().toggleClass('open')
            $(window).width() <= 768 ? setTimeout(function () { $sidebar.hide() }, 400) : $sidebar.hide();
        } else {
            $sidebar.show()
            $sidebar.parent().toggleClass('open')
            $(window).width() <= 768 ? setTimeout(function () { $main.addClass('open').removeClass('closed') }, 400) : $main.addClass('open').removeClass('closed');
        }
    });

    $('.lgbDropdown').lgbDropdown();

    // load widget data
    $.reloadWidget();
    $.pullNotification();
});