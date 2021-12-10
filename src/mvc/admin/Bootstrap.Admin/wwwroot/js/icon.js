$(function () {
    $.bc({
        url: Menu.iconView,
        contentType: 'text/html',
        dataType: 'html',
        callback: function (result) {
            if (result) {
                var $html = $('#main-content').html(result);
                var $iconList = $('div.fontawesome-icon-list').on('click', 'a', function () {
                    var text = $(this).children('i').attr('class');
                    window.console.log(text);
                    if ($.copyText(text)) {
                        toastr.success('拷贝成功');
                    }
                    else {
                        toastr.error('拷贝失败');
                    }
                    return false;
                });

                $iconList.find('ul li').addClass('col-xl-2 col-md-3 col-sm-4 col-6');
                $iconList.find('div').addClass('col-xl-2 col-6');

                $('#main-content').scrollspy({ offset: 150, target: '.fa-nav' });

                if (!$.browser.versions.ios) $html.find('.fa-nav .nav').overlayScrollbars({ className: 'os-theme-light', scrollbars: { autoHide: 'leave' } });
            }
        }
    });
});