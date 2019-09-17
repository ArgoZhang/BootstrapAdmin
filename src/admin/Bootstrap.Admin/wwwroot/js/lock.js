$(function () {
    $('#time').text((new Date()).format('HH:mm:ss'));

    setInterval(function () {
        $('#time').text((new Date()).format('HH:mm:ss'));
    }, 500);

    $(".lock-wrapper").autoCenter();

    var timeHanlder = null;
    $('#btnSendCode').on('click', function () {
        var $this = $(this);
        var method = $this.attr('data-method');
        var phone = $('input[name="username"]').val();
        var $password = $('input[name="password"]');
        var $code = $('#smscode');
        var code = $code.val();
        if (method === 'submit') {
            if ($code.val() === '') {
                $code.tooltip('show');
                var handler = setTimeout(function () {
                    clearTimeout(handler);
                    $code.tooltip('hide');
                }, 1000);
                return true;
            }

            // 提交
            $password.val(code);
            $('form').submit();
            return true;
        }
        // validate mobile phone
        var apiUrl = "api/Login?phone=" + phone;
        $.bc({
            url: apiUrl,
            method: 'PUT',
            callback: function (result) {
                $this.attr('data-original-title', result ? "发送成功" : "发送失败").tooltip('show');
                var handler = setTimeout(function () {
                    clearTimeout(handler);
                    $this.tooltip('hide').tooltip('disable');
                }, 1000);

                if (result) {
                    // send success
                    $('#smscode').removeAttr('disabled');
                    $this.text('已发送').attr('data-method', 'submit');
                    timeHanlder = setTimeout(function () {
                        clearTimeout(timeHanlder);
                        var count = 299;
                        timeHanlder = setInterval(function () {
                            if (count === 0) {
                                clearInterval(timeHanlder);
                                $this.text('发送验证码').attr('data-method', 'send').attr('data-original-title', "点击发送验证码").tooltip('enable');
                                return;
                            }
                            $this.text('登录 (' + count-- + 's)');
                        }, 1000);
                    }, 1000);
                }
            }
        });
    });
});
