$(function () {
    function iframeResposive() {
        try {
            var fra = $('iframe').get(0);
            fra.height = fra.contentDocument.body.offsetHeight;
        }
        catch (e) {
        }
    }
    $(window).on('load', iframeResposive);
    $(window).on('resize', iframeResposive);

    $('.menu-submenu a').click(function (event) {
        var act = $(this).attr("data-act");
        if (act === "True") return true;
        event.preventDefault();
        $('.menu-submenu a, .menu-submenu p').removeClass('active');
        $(this).addClass('active');
        $(this).parents("ul").first().find('p').addClass('active');
        $('iframe').attr('src', $(this).attr('href'));
    });
});