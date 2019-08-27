$(function () {
    var $captcha = $('#captcha');

    $captcha.sliderCaptcha({
        localImages: function () {
            return '../../lib/captcha/images/Pic' + Math.round(Math.random() * 4) + '.jpg';
        },
        setSrc: function () {
            return $captcha.attr('data-imageLibUrl') + 'Pic' + Math.round(Math.random() * 136) + '.jpg';
        },
        onSuccess: function () {
            var that = this;
            setTimeout(function () {
                that.parent().removeClass('d-inline-block');
                that.sliderCaptcha('reset');
                $('.userinfo .dropdown-menu a:first')[0].click();
            }, 1000);
        },
        remoteUrl: $.formatUrl('api/Captcha')
    });

    $('#btnCaptcha').on('click', function () {
        $('#captcha').parent().addClass('d-inline-block');
        $.footer();
    });

    $.footer();
});