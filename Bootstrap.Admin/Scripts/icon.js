$(function () {
    var $iconList = $('#iconTab').find('div.fontawesome-icon-list');
    $iconList.on('click', 'a', function () {
        window.console.log($(this).children('i').attr('class'));
        return false;
    });

    $iconList.on('click', 'ul li', function () {
        window.console.log($(this).children('span:first').attr('class'));
    });
    $iconList.find('ul li').addClass('col-md-3 col-sm-4 col-xs-6');
    $iconList.find('div').addClass('col-xs-6');
});