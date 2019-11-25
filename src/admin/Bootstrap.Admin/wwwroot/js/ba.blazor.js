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

    $.extend({
        format: function (source, params) {
            if (params === undefined || params === null) {
                return null;
            }
            if (arguments.length > 2 && params.constructor !== Array) {
                params = $.makeArray(arguments).slice(1);
            }
            if (params.constructor !== Array) {
                params = [params];
            }
            $.each(params, function (i, n) {
                source = source.replace(new RegExp("\\{" + i + "\\}", "g"), function () {
                    return n;
                });
            });
            return source;
        },
        activePage: function (id) {
            $('.main-content .content').addClass('d-none');
            $('#page_' + id).removeClass('d-none');
        },
        activeMenu: function (id) {
            var $menu = $('#menus_' + id);
            $('.sidebar .active').removeClass('active');
            $menu.addClass('active');
            // set website title
            $('head title').text($menu.text());
            this.moveTab(id);
        },
        removeTab: function (tabId) {
            var nextId = $('#navBar').find('.active').first().attr('id');
            var $curTab = $('#' + tabId);
            if ($curTab.hasClass('active')) {
                var $nextTab = $curTab.parent().next().find('.nav-link');
                var $prevTab = $curTab.parent().prev().find('.nav-link');
                if ($nextTab.length === 1) nextId = $nextTab.attr('id');
                else if ($prevTab.length === 1) nextId = $prevTab.attr('id');
                else nextId = "";
                if (nextId !== "") {
                    this.activeMenu(nextId);
                    this.activePage(nextId);
                }
            }
            return nextId;
        },
        moveTab: function (tabId) {
            var $tab = $('#' + tabId);
            if ($tab.length === 0) return;
            var $navBar = $('#navBar');
            $navBar.parent().addClass('d-flex').removeClass('d-none');

            // reset active
            $navBar.find('.active').removeClass('active');
            $tab.addClass('active');

            var $first = $navBar.children().first();
            var marginLeft = $tab.position().left - 2 - $first.position().left;
            var scrollLeft = $navBar.scrollLeft();
            if (marginLeft < scrollLeft) {
                // overflow left
                $navBar.scrollLeft(marginLeft);
                return;
            }
            var marginRight = $tab.position().left + $tab.outerWidth() - $navBar.outerWidth();
            if (marginRight < 0) return;
            $navBar.scrollLeft(marginRight - $first.position().left);
        },
        movePrevTab: function () {
            var $navBar = $('#navBar');
            var $curTab = $navBar.find('.active').first();
            return $curTab.parent().prev().find('.nav-link').first().attr('url');
        },
        moveNextTab: function () {
            var $navBar = $('#navBar');
            var $curTab = $navBar.find('.active').first();
            return $curTab.parent().next().find('.nav-link').first().attr('url');
        },
        enableAnimation: function () {
            $('body').removeClass('trans-mute');
        },
        initSidebar: function () {
            $('.sidebar').addNiceScroll().autoScrollSidebar();
        },
        enableBackground: function (val) {
            if (val) $('.main-content').addClass('welcome-bg').find('nav').addClass('d-none').removeClass('d-flex');
            else $('.main-content').removeClass('welcome-bg').find('nav').addClass('d-flex').removeClass('d-none');
        }
    });

    $(function () {
        $(document)
            .on('click', '.nav-link-bar.dropdown', function (e) {

            });
    });
})(jQuery);
