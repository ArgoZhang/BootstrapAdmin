$(function () {
    $('#captcha').sliderCaptcha({
        setSrc: function () {
            return 'http://pocoafrro.bkt.clouddn.com/Pic' + Math.round(Math.random() * 136) + '.jpg';
        },
        onSuccess: function () {
            var that = this;
            setTimeout(() => {
                that.parent().removeClass('d-inline-block');
                that.sliderCaptcha('reset');
            }, 1000);
        }
    });

    $('#btnCaptcha').on('click', function () {
        $('#captcha').parent().addClass('d-inline-block');
    });

    $.footer();
});