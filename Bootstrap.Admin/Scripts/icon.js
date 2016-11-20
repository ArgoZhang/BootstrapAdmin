$(function () {
    var $iconList = $('#iconTab').find('div.fontawesome-icon-list');
    var $main = $('#main-content');
    var $tl = $('#timeline');

    $iconList.on('click', 'a', function () {
        window.console.log($(this).children('i').attr('class'));
        return false;
    });

    $iconList.on('click', 'ul li', function () {
        window.console.log($(this).children('span:first').attr('class'));
    });
    $iconList.find('ul li').addClass('col-md-3 col-sm-4 col-xs-6');
    $iconList.find('div').addClass('col-xs-6');

    $main.scrollspy({ offset: 150, target: '#timeline' });
    $('a[data-toggle="tab"]').on('show.bs.tab', function (e) {
        if (e.target.href.indexOf("#Glyphicons") > -1) $tl.attr('style', 'display: none;');
        else $tl.removeAttr('style');
    })
});