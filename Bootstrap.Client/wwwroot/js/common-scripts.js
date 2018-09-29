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
        $('body').animate({
            scrollTop: 0
        }, 200);
    });

    // breadcrumb
    var $breadNav = $('#breadNav');
    var arch = $sideMenu.find('a.active').last();
    $breadNav.removeClass('d-none').text(arch.text() || $('title').text());

    $('.sidebar-toggle-box').on('click', function () {
        if ($(window).width() >= 768) return;
        $('body').toggleClass('sidebar-open');
    });

    $('[data-toggle="dropdown"].dropdown-select').dropdown('select');

    // tooltip
    $('[data-toggle="tooltip"]').tooltip();
});