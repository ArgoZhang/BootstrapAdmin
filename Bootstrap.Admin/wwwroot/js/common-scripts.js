(function ($) {
    var cascadeMenu = function (menus) {
        var html = "";
        $.each(menus, function (index, menu) {
            if (menu.Menus.length === 0) {
                html += $.format('<li class="dd-item dd3-item" data-id="{0}" data-order="{4}" data-category="{3}"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><div class="checkbox"><label><input type="checkbox" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><div class="radio"><label><input type="radio" name="menu" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><span class="menuType">{5}</span><span class="menuOrder">{4}</span></div></li>', menu.Id, menu.Icon, menu.Name, menu.Category, menu.Order, menu.CategoryName);
            }
            else {
                html += $.format('<li class="dd-item dd3-item" data-id="{0}" data-order="{5}" data-category="{3}"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><div class="checkbox"><label><input type="checkbox" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><div class="radio"><label><input type="radio" name="menu" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><span class="menuType">{6}</span><span class="menuOrder">{5}</span></div><ol class="dd-list">{4}</ol></li>', menu.Id, menu.Icon, menu.Name, menu.Category, cascadeSubMenu(menu.Menus), menu.Order, menu.CategoryName);
            }
        });
        return html;
    };

    var cascadeSubMenu = function (menus) {
        var html = "";
        $.each(menus, function (index, menu) {
            html += $.format('<li class="dd-item dd3-item" data-id="{0}" data-order="{4}" data-category="{3}"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><div class="checkbox"><label><input type="checkbox" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><div class="radio"><label><input type="radio" name="menu" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><span class="menuType">{5}</span><span class="menuOrder">{4}</span></div></li>', menu.Id, menu.Icon, menu.Name, menu.Category, menu.Order, menu.CategoryName);
        });
        return html;
    };

    var setBadge = function (source) {
        var data = $.extend({
            TasksCount: 0,
            AppExceptionsCount: 0,
            DbExceptionsCount: 0,
            MessagesCount: 0,
            NewUsersCount: 0
        }, source);
        $('#msgHeaderTaskBadge').text(data.TasksCount === 0 ? "" : data.TasksCount);
        $('#msgHeaderUserBadge').text(data.NewUsersCount === 0 ? "" : data.NewUsersCount);
        $('#msgHeaderAppBadge').text(data.AppExceptionsCount === 0 ? "" : data.AppExceptionsCount);
        $('#msgHeaderDbBadge').text(data.DbExceptionsCount === 0 ? "" : data.DbExceptionsCount);
        $('#msgHeaderMsgBadge').text(data.MessagesCount === 0 ? "" : data.MessagesCount);
        $('#logoutNoti').text(data.NewUsersCount === 0 ? "" : data.NewUsersCount);
    };

    $.fn.extend({
        nestMenu: function (callback) {
            var $this = $(this);
            $.bc({
                id: 0, url: Menu.url, query: { type: "user" }, method: "post",
                callback: function (result) {
                    var html = "";
                    if ($.isArray(result)) html = cascadeMenu(result);
                    $this.find('ol:first').html(html);
                    $this.nestable();
                    callback();
                }
            });
        },
        clearWidgetItems: function () {
            setBadge(false);
            this.children('.dropdown').each(function () {
                $(this).children('.dropdown-menu').each(function () {
                    $(this).children('a').remove();
                });
            });
            return this;
        },
        reloadWidget: function () {
            if (this.length === 0) return this;
            var that = this;
            $.bc({
                url: Notifications.url,
                callback: function (result) {
                    that.clearWidgetItems();
                    if (!result) return;

                    setBadge(result);

                    // tasks
                    $('#msgHeaderTask').text(result.TasksCount);
                    var htmlUserTemplate = '<a class="dropdown-item" href="{4}?id={3}"><span class="desc">{0}-{2}</span><span class="percent">{1}%</span></span><div class="progress progress-striped"><div class="progress-bar" role="progressbar" aria-valuenow="{1}" aria-valuemin="0" aria-valuemax="100" style="width: {1}%"><span class="sr-only">{1}% 完成</span></div></div></a>';
                    var html = result.Tasks.map(function (u) {
                        return $.format(htmlUserTemplate, u.TaskName, u.TaskProgress, u.AssignDisplayName, u.Id, $.formatUrl('Admin/Tasks'));
                    }).join('');
                    $(html).insertAfter($('#msgHeaderTaskContent'));

                    // new users
                    $('#msgHeaderUser').text(result.NewUsersCount);
                    htmlUserTemplate = '<a class="dropdown-item" href="{4}"><span class="label label-success"><i class="fa fa-plus"></i></span><div title="{2}" class="content">{1}({0})</div><span class="small italic">{3}</span></a>';
                    html = result.Users.map(function (u) {
                        return $.format(htmlUserTemplate, u.UserName, u.DisplayName, u.Description, u.Period, $.formatUrl('Admin/Notifications'));
                    }).join('');
                    $(html).insertAfter($('#msgHeaderUserContent'));

                    // apps
                    $('#msgHeaderApp').text(result.AppExceptionsCount);
                    htmlUserTemplate = '<a class="dropdown-item" href="{3}"><span class="label label-warning"><i class="fa fa-bug"></i></span><div title="{1}" class="content">{0}</div><span class="small italic">{2}</span></a>';
                    html = result.Apps.map(function (u) {
                        return $.format(htmlUserTemplate, u.ExceptionType, u.Message, u.Period, $.formatUrl('Admin/Exceptions'));
                    }).join('');
                    $(html).insertAfter($('#msgHeaderAppContent'));

                    // dbs
                    $('#msgHeaderDb').text(result.DbExceptionsCount);
                    htmlUserTemplate = '<a class="dropdown-item" href="{3}"><span class="label label-danger"><i class="fa fa-bolt"></i></span><div title="{1}" class="content">{0}</div><span class="small italic">{2}</span></a>';
                    html = result.Dbs.map(function (u) {
                        return $.format(htmlUserTemplate, u.ErrorPage, u.Message, u.Period, $.formatUrl('Admin/Exceptions'));
                    }).join('');
                    $(html).insertAfter($('#msgHeaderDbContent'));

                    // messages
                    $('#msgHeaderMsg').text(result.MessagesCount);
                    htmlUserTemplate = '<a class="dropdown-item" href="{6}?id={0}"><span class="photo"><img alt="avatar" src="{1}"></span><span class="subject"><span class="from">{2}</span><span class="time">{4}</span></span><span class="message" title="{5}">{3}</span></a>';
                    html = result.Messages.map(function (u) {
                        return $.format(htmlUserTemplate, u.Id, u.FromIcon, u.FromDisplayName, u.Title, u.Period, u.Content, $.formatUrl('Admin/Messages'));
                    }).join('');
                    $(html).insertAfter($('#msgHeaderMsgContent'));
                }
            });
            return this;
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
        "timeOut": "4000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    var $sideMenu = $(".sidebar");

    $sideMenu.dcAccordion({
        autoExpand: true
    });

    $("#gotoTop").on('click', function (e) {
        e.preventDefault();
        $('.main-content, html, body').animate({
            scrollTop: 0
        }, 200);
    });

    // breadcrumb
    var $breadNav = $('#breadNav');
    var arch = $sideMenu.find('a.active').last();
    $breadNav.removeClass('d-none').text(arch.text() || $('title').text());

    $.fn.extend({
        autoScrollSidebar: function (options) {
            var option = $.extend({ target: null, offsetTop: 0 }, options);
            var $navItem = option.target;
            if ($navItem === null) return this;

            // sidebar scroll animate
            var middle = this.outerHeight() / 2;
            var top = 0;
            if (this.hasClass('mCustomScrollbar')) {
                top = $navItem.offset().top - $('header').outerHeight() + option.offsetTop;
                if (top > middle) {
                    this.mCustomScrollbar('scrollTo', top - middle);
                }
            }
            else {
                top = $navItem.offset().top + option.offsetTop;
                if (top > middle)
                    this.animate({
                        scrollTop: top - middle
                    });
            }
            return this;
        },
        addNiceScroll: function () {
            if ($.browser.versions.ios && $(window).width() > 768) {
                this.css('overflow', 'auto');
            }
            else if (!$.browser.versions.ios && $(window).width() > 768) {
                this.mCustomScrollbar({ theme: 'minimal' });
            }
            else {
                this.mCustomScrollbar('destroy');
            }
            return this;
        }
    });

    // custom scrollbar
    var $sidebar = $('aside').addNiceScroll().autoScrollSidebar({ target: arch.parent(), offsetTop: arch.parent().innerHeight() / 2 });

    $sideMenu.on('click', 'a.dcjq-parent', function () {
        var $this = $(this);
        if (!$.browser.versions.ios && $(window).width() > 768) {
            setTimeout(function () {
                var offsetScroll = parseInt($this.parents('.mCSB_container').css('top').replace('px', ''));
                $sidebar.autoScrollSidebar({ target: $this.parent(), offsetTop: 25.5 - offsetScroll });
            }, 600);
        }
        else if ($.browser.versions.ios && $(window).width() > 768) {
            var offsetScroll = parseInt($this.parents('aside').scrollTop());
            $sidebar.autoScrollSidebar({ target: $this.parent(), offsetTop: 25.5 + offsetScroll });
        }
    });

    $('.sidebar-toggle-box').on('click', function () {
        $('body').toggleClass('sidebar-open');
    });

    // load widget data
    $('.header .nav').reloadWidget().notifi({
        url: 'NotiHub',
        callback: function (result) {
            var cate = result.Category;
            var msg = result.Message;
            switch (cate) {
                case "DB":
                    toastr.error(msg, "数据库操作发生异常");
                    break;
                case "Users":
                    toastr.success(msg, "新用户注册");
                    break;
                case "App":
                    toastr.warning(msg, "应用程序发生异常");
                    break;
            }
            if (result) this.reloadWidget();
        }
    });

    $(window).on('resize', function () {
        $sidebar.addNiceScroll();
    });
});