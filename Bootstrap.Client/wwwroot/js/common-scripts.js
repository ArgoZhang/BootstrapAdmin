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
    var $breadNav = $('#breadNav');
    var arch = $sideMenu.find('a.active').last();
    $breadNav.removeClass('d-none').text(arch.text() || $('title').text());

    $(window).on('resize', function () {
        $('footer').footer();
    });

    $("#gotoTop").on('click', function (e) {
        e.preventDefault();
        $('html, body').animate({
            scrollTop: 0
        }, 200);
    });

    $('.sidebar-toggle-box').on('click', function (e) {
        if ($(window).width() >= 768) {
            e.preventDefault();
            return false;
        }
        $('body').toggleClass('sidebar-open');
    });

    $('select[data-valid="true"]').on('input', function (e) {
        e.stopPropagation();
    }).on('change', function () {
        $(this).trigger('input.lgb.validate');
    });

});