(function ($) {
    $.fn.extend({
        autoScrollSidebar: function (options) {
            var option = $.extend({ target: null, offsetTop: 0 }, options);
            var $navItem = option.target;
            if ($navItem === null || $navItem.length === 0) return this;

            // sidebar scroll animate
            var middle = this.outerHeight() / 2;
            var top = $navItem.offset().top + option.offsetTop - this.offset().top;
            var $scrollInstance = this[0]["__overlayScrollbars__"];
            if (top > middle) {
                if ($scrollInstance) $scrollInstance.scroll({ x: 0, y: top - middle }, 500, "swing");
                else this.animate({ scrollTop: top - middle });
            }
            return this;
        },
        addNiceScroll: function () {
            if ($(window).width() > 768) {
                this.overlayScrollbars({
                    className: 'os-theme-light',
                    scrollbars: {
                        autoHide: 'leave',
                        autoHideDelay: 100
                    },
                    overflowBehavior: {
                        x: "hidden",
                        y: "scroll"
                    }
                });
            }
            else {
                this.css('overflow', 'auto');
            }
            return this;
        }
    });

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
                    var resultFormat = {
                        "Success": '<span class="badge badge-pill badge-success badge-task"><i class="fa fa-check-circle"></i><span>成功</span></span>',
                        "Timeout": '<span class="badge badge-pill badge-warning badge-task"><i class="fa fa-exclamation-circle"></i><span>超时</span></span>'
                    };
                    var htmlUserTemplate = '<a class="dropdown-item position-relative" href="{0}"><span class="label label-primary"><i class="fa fa-thumb-tack"></i></span><div class="content">{1}</div><div class="small italic content-task">{2}</div>{3}</a>';
                    var html = result.Tasks.map(function (u) {
                        return $.format(htmlUserTemplate, $.formatUrl('Admin/Tasks'), u.Name, u.LastRuntime, resultFormat[u.LastRunResult]);
                    }).join('');
                    $(html).insertAfter($('#msgHeaderTaskContent'));

                    // new users
                    $('#msgHeaderUser').text(result.NewUsersCount);
                    htmlUserTemplate = '<a class="dropdown-item" href="{4}"><span class="label label-success"><i class="fa fa-plus"></i></span><div title="{2}" class="content">{1}({0})</div><span class="small italic">{3}</span></a>';
                    html = result.Users.map(function (u) {
                        return $.format(htmlUserTemplate, $.safeHtml(u.UserName), $.safeHtml(u.DisplayName), $.safeHtml(u.Description), u.Period, $.formatUrl('Admin/Notifications'));
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
                        return $.format(htmlUserTemplate, u.Id, u.FromIcon, $.safeHtml(u.FromDisplayName), $.safeHtml(u.Title), u.Period, $.safeHtml(u.Content), $.formatUrl('Admin/Messages'));
                    }).join('');
                    $(html).insertAfter($('#msgHeaderMsgContent'));
                }
            });
            return this;
        }
    });

    var formatCategoryName = function (menu) {
        var ret = "";
        if (menu.IsResource === 2) ret = "按钮";
        else if (menu.IsResource === 1) ret = "资源";
        else ret = menu.CategoryName;
        return ret;
    };

    var formatApplicationName = function (app) {
        return app;
    }

    var cascadeMenu = function (menus) {
        var html = "";
        $.each(menus, function (index, menu) {
            if (menu.Menus.length === 0) {
                html += $.format('<li class="dd-item dd3-item" data-id="{0}" data-order="{4}" data-category="{3}" data-resource="{6}"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><div class="checkbox"><label><input type="checkbox" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><div class="radio"><label><input type="radio" name="menu" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><span class="menuType">{5}</span><span class="menuApp">{7}</span><span class="menuOrder">{4}</span></div></li>', menu.Id, menu.Icon, menu.Name, menu.Category, menu.Order, formatCategoryName(menu), menu.IsResource, formatApplicationName(menu.Application));
            }
            else {
                html += $.format('<li class="dd-item dd3-item" data-id="{0}" data-order="{5}" data-category="{3}" data-resource="{7}"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><div class="checkbox"><label><input type="checkbox" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><div class="radio"><label><input type="radio" name="menu" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><span class="menuType">{6}</span><span class="menuApp">{8}</span><span class="menuOrder">{5}</span></div><ol class="dd-list">{4}</ol></li>', menu.Id, menu.Icon, menu.Name, menu.Category, cascadeMenu(menu.Menus), menu.Order, formatCategoryName(menu), menu.IsResource, formatApplicationName(menu.Application));
            }
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
})(jQuery);

$(function () {
    // enable animoation effect
    $('body').removeClass('trans-mute');

    // blazor widget 切换功能
    $('[data-method="blazor"]').on('click', function (e) { 
        e.preventDefault();
        var path = window.location.pathname;
        var search = window.location.search;
        window.location = $.formatUrl("Pages" + path + search);
    });

    // 自动锁屏功能
    var mousePosition = { screenX: 0, screenY: 0 };
    var count = 1;
    var lockScreenPeriod = parseInt($('#lockScreenPeriod').val());
    if (typeof lockScreenPeriod === 'number' && !isNaN(lockScreenPeriod)) {
        var traceMouseOrKey = window.setInterval(function () {
            $(document).off('mousemove').one('mousemove', function (e) {
                if (mousePosition.screenX !== e.screenX || mousePosition.screenY !== e.screenY) {
                    mousePosition.screenX = e.screenX;
                    mousePosition.screenY = e.screenY;

                    // 计数器归零
                    count = 1;
                    return;
                }
            });
        }, 2000);
        var lockHandler = window.setInterval(function () {
            if (count++ * 5 > lockScreenPeriod) {
                window.clearInterval(lockHandler);
                window.clearInterval(traceMouseOrKey);
                this.location.href = $.formatUrl("Account/Lock");
            }
        }, 5000);
    }

    var $sideMenu = $(".sidebar ul");

    // breadcrumb
    var $breadNav = $('#breadNav, .main-header .breadcrumb-item:last');
    var arch = $sideMenu.find('a.active').last();
    $breadNav.removeClass('d-none').text(arch.text() || $('title').text());

    // custom scrollbar
    $sidebar = $('.sidebar').addNiceScroll().autoScrollSidebar({ target: arch.parent(), offsetTop: arch.parent().innerHeight() / 2 });

    // 大于 768 时考虑网站设置 收缩侧边栏
    if ($(window).width() > 768) {
        var $ele = $('aside');
        var collapsed = $ele.hasClass('collapsed');
        if (collapsed) {
            $('body').addClass('sidebar-open');
            $ele.removeClass('collapsed');
        }
    }

    $('.sidebar-toggle-box').on('click', function () {
        $('body').toggleClass('sidebar-open');
    });

    // Apps
    window.App = {
        url: 'api/Apps',
        title: "分配应用"
    };

    // Roles
    window.Role = {
        url: 'api/Roles',
        title: "分配角色"
    };

    // Users
    window.User = {
        url: 'api/Users',
        title: "分配用户"
    };

    // Groups
    window.Group = {
        url: 'api/Groups',
        title: "分配部门"
    };

    // Menus
    window.Menu = {
        url: 'api/Menus',
        iconView: 'Admin/IconView',
        sidebar: 'Admin/Sidebar',
        title: "分配菜单"
    };

    // Exceptions
    window.Exceptions = {
        url: 'api/Exceptions',
        title: "程序异常日志"
    };

    // Dicts
    window.Dicts = {
        url: 'api/Dicts'
    };

    // Profiles
    window.Profiles = {
        url: 'api/Profiles',
        del: 'api/Profiles/Delete'
    };

    // Settings
    window.Settings = {
        url: 'api/Settings'
    };

    // Messages
    window.Messages = {
        url: 'api/Messages'
    };

    // Tasks
    window.Tasks = {
        url: 'api/Tasks'
    };

    // Notifications
    window.Notifications = {
        url: 'api/Notifications'
    };

    window.CheckboxHtmlTemplate = '<div class="form-group col-md-3 col-sm-4 col-6"><label title="{3}" data-toggle="tooltip" role="checkbox" aria-checked="false" class="form-checkbox is-{2}"><span class="checkbox-input"><span class="checkbox-inner"></span><input type="checkbox" value="{0}" {2} /></span><span class="checkbox-label">{1}</span></label></div>';

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