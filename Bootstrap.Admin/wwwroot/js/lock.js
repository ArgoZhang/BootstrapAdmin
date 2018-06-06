$(function () {
    $('body').addClass('lock-screen');
    $('#time').text((new Date()).format('HH:mm:ss'));

    setInterval(function () {
        $('#time').text((new Date()).format('HH:mm:ss'));
    }, 500);

    $.extend($.validator.messages, { required: "请输入密码" });
    // validate
    $('form').autoValidate({
        password: {
            required: true,
            maxlength: 50
        }
    });
});