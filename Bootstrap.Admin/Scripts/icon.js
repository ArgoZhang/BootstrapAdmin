$(function () {
    $('.fontawesome-icon-list a').click(function () {
        window.console.log($(this).children('i').attr('class'));
        return false;
    });

    $('.fontawesome-icon-list ul li').click(function () {
        window.console.log($(this).children('span:first').attr('class'));
    });

    $('.fontawesome-icon-list ul li').addClass('col-md-3 col-sm-4');
});