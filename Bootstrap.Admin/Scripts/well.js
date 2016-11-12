$(function () {
    $('#main-content').css('backgroundImage', 'url("../../content/images/bg.jpg")');
    function resposive() {
        var height = $(window).height();
        if (height > 672)
            $('.well-bg').height(height - 270);
        else
            $('.well-bg').height(height - 158);
    }
    $(window).on('load', resposive);
    $(window).on('resize', resposive);
});