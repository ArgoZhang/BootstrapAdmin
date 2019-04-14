$(function () {
    var $sideMenu = $(".sidebar");
    $sideMenu.dcAccordion({
        autoExpand: true,
        saveState: false
    });
    var $breadNav = $('#breadNav');
    var arch = $sideMenu.find('a.active').last();
    $breadNav.removeClass('d-none').text(arch.text() || $('title').text());


    $('.sidebar-toggle-box').on('click', function (e) {
        if ($(window).width() >= 768) {
            e.preventDefault();
            return false;
        }
        $('body').toggleClass('sidebar-open');
    });
});