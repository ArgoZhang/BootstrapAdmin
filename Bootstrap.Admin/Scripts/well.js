$(function () {
    $('#main-content').css('backgroundImage', 'url("../../content/images/bg.jpg")');
    function resposive() {
        $('.well-bg').height($(window).height() - 270);
    }
    $(window).on('load', resposive);
    $(window).on('resize', resposive);
});