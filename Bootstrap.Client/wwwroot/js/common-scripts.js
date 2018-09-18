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

    var $sidebar = $('aside');
    var $sideMenu = $(".sidebar");
    var $breadNav = $('#breadNav');

    $sideMenu.dcAccordion({
        autoExpand: true
    });

    // custom scrollbar
    if (!$.browser.versions.ios) $sidebar.niceScroll({ cursorcolor: "#e8403f", cursorwidth: '3px', background: '#2a3542', spacebarenabled: false, cursorborder: '' });

    $("#gotoTop").on('click', function (e) {
        e.preventDefault();
        $('body').animate({
            scrollTop: 0
        }, 200);
    });

    // breadcrumb
    var arch = $sideMenu.find('a.active').last();
    if (arch.text() !== "") $breadNav.removeClass('d-none').text(arch.text());

    // sidebar scroll animate
    var top = (arch.offset() || { top: 0 }).top;
    if (top > 0) {
        var middle = $('header').outerHeight() + $sidebar.outerHeight() / 2;
        if (top > middle) $sidebar.animate({ scrollTop: top + arch.outerHeight() / 2 - middle }, 500);
    }

    $sideMenu.on('click', 'a.dcjq-parent', function () {
        var o = $(this).offset();
        diff = 110 - o.top;
        if (diff > 0)
            $sidebar.scrollTo("-=" + Math.abs(diff), 500);
        else
            $sidebar.scrollTo("+=" + Math.abs(diff), 500);

        // resize nicscroll
        $sidebar.getNiceScroll().resize();
    });

    $('.sidebar-toggle-box').on('click', function () {
        $('body').toggleClass('sidebar-open');
    });

    $('[data-toggle="dropdown"].dropdown-select').dropdown('select');

    // tooltip
    $('[data-toggle="tooltip"]').tooltip();
});