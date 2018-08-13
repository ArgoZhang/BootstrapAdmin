$(function () {
    var $iconList = $('div.fontawesome-icon-list').on('click', 'a', function () {
        window.console.log($(this).children('i').attr('class'));
        return false;
    });

    $iconList.find('ul li').addClass('col-xl-2 col-md-3 col-sm-4 col-6');
    $iconList.find('div').addClass('col-xl-2 col-6');

    $('#main-content').scrollspy({ offset: 150, target: '.fa-nav' });
});