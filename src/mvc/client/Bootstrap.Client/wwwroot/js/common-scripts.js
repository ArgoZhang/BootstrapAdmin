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

})(jQuery);

$(function () {
    // enable animoation effect
    $('body').removeClass('trans-mute');

    var $sideMenu = $(".sidebar ul");

    // breadcrumb
    var $breadNav = $('#breadNav, .main-header .breadcrumb-item:last');
    var arch = $sideMenu.find('a.active').last();
    $breadNav.removeClass('d-none').text(arch.text() || $('title').text());

    // custom scrollbar
    var $sidebar = $('.sidebar').addNiceScroll().autoScrollSidebar({ target: arch.parent(), offsetTop: arch.parent().innerHeight() / 2 });
    $(window).on('resize', function () {
        $sidebar.addNiceScroll();
    });

    $('.sidebar-toggle-box').on('click', function (e) {
        // 判断是否为 LTE 模式
        if ($(window).width() >= 768 && $('aside').is(':hidden')) {
            e.preventDefault();
            return false;
        }
        $('body').toggleClass('sidebar-open');
    });

    $('.cloud, .bird').css({
        "transition": "all linear 3s"
    });

    $('.cloud').on('mouseenter mouseleave', function () {
        $(this).toggleClass('move');
    });
});