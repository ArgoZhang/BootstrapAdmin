$(function () {
    $.bc({
        url: Menu.iconView,
        contentType: 'text/html',
        dataType: 'html',
        callback: function (result) {
            if (result) {
                var $html = $('#main-content').html(result);
                var $iconList = $('div.fontawesome-icon-list').on('click', 'a', function () {
                    window.console.log($(this).children('i').attr('class'));
                    return false;
                });

                $iconList.find('ul li').addClass('col-xl-2 col-md-3 col-sm-4 col-6');
                $iconList.find('div').addClass('col-xl-2 col-6');

                $('#main-content').scrollspy({ offset: 150, target: '.fa-nav' });

                if (!$.browser.versions.ios) $html.find('.fa-nav .nav').niceScroll({ cursorcolor: "#e8403f", cursorwidth: '3px', spacebarenabled: false, cursorborder: '' });
            }
        }
    });
});