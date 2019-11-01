$(function () {
    var $imgUrl = $('#imgUrl');
    $('[data-oauth="False"]').attr("data-original-title", "点击登录系统");
    $(".container").autoCenter();

    $("a[data-method]").on('click', function () {
        var $this = $(this);
        switch ($this.attr("data-method")) {
            case "register":
                $("#dialogNew").modal('show');
                break;
            case "forgot":
                $("#dialogForgot").modal('show');
                break;
        }
    });

    $.extend({
        captchaCheck: function (captcha, success) {
            $.bc({
                url: "api/OnlineUsers",
                method: "put",
                callback: function (result) {
                    // 返回真时表示三次验证需要滑块验证码
                    if (result) captcha.addClass('d-block');
                    else success();
                }
            });
        },
        capWidth: function () {
            return $(window).width() < 768 ? 256 : 272;
        },
        capHeight: function () {
            // 如果关闭 oAuth 认证 高度要缩小
            var height = 150;
            if ($(window).width() < 768) height = $('.slidercaptcha:first').hasClass('oauth') ? 96 : 150;
            return height;
        },
        capRegSuccess: function () {
            $.bc({
                url: "api/Register",
                data: { UserName: $('#userName').val(), Password: $('#password').val(), DisplayName: $('#displayName').val(), Description: $('#description').val() },
                modal: '#dialogNew',
                method: "post",
                callback: function (result) {
                    var title = result ? "提交成功<br/>等待管理员审批" : "提交失败";
                    lgbSwal({ timer: 1500, title: title, type: result ? "success" : "error" });
                }
            });
        },
        capForgotSuccess: function () {
            $.bc({
                url: "api/Register",
                data: { UserName: $('#f_userName').val(), DisplayName: $('#f_displayName').val(), Reason: $('#f_desc').val() },
                modal: '#dialogForgot',
                method: "put",
                callback: function (result) {
                    var title = result ? "提交成功<br/>等待管理员重置密码" : "提交失败";
                    lgbSwal({ timer: 1500, title: title, type: result ? "success" : "error" });
                }
            });
        }
    });

    $('#btnSubmit').on('click', function () {
        $.captchaCheck($('#dialogNew .slidercaptcha'), $.capRegSuccess);
        return false;
    });

    $('#btnForgot').on('click', function () {
        $.captchaCheck($('#dialogForgot .slidercaptcha'), $.capForgotSuccess);
        return false;
    });

    $('.rememberPwd').on('click', function () {
        var $this = $(this);
        var $check = $this.find('i');
        var $rem = $('#remember');
        if ($check.hasClass('fa-square-o')) {
            $check.addClass('fa-check-square-o').removeClass('fa-square-o');
            $rem.val('true');
        } else {
            $check.addClass('fa-square-o').removeClass('fa-check-square-o');
            $rem.val('false');
        }
    });

    $('.slidercaptcha .close').on('click', function (e) {
        $(this).parents('.slidercaptcha').removeClass('d-block');
        return false;
    });

    var $loginButton = $('button[type="submit"]').on('click', function (e) {
        $.captchaCheck($('#login .slidercaptcha'), function () {
            $('form').submit();
        });
        return false;
    });

    $('#captcha, #regcap, #forgotcap').sliderCaptcha({
        width: $.capWidth(),
        height: $.capHeight(),
        localImages: function () {
            var base = $('#pathBase').attr('href');
            return base + 'lib/captcha/images/Pic' + Math.round(Math.random() * 4) + '.jpg';
        },
        setSrc: function () {
            return $imgUrl.val() + 'Pic' + Math.round(Math.random() * 136) + '.jpg';
        },
        onSuccess: function () {
            var parent = this.parents('.slidercaptcha').removeClass('d-block');
            this.sliderCaptcha('reset');
            if (parent.hasClass('reg')) {
                $.capRegSuccess();
            }
            else if (parent.hasClass('forgot')) {
                $.capForgotSuccess();
            }
            else {
                $('form').submit();
            }
        }
    });

    // use Gitee authentication when SystemDemoModel
    var $login = $('#login');
    var $username = $('[name="userName"]');
    var $password = $('[name="password"]');
    var $loginUser = $('#loginUser');
    var $loginMobile = $('#loginMobile');
    var $loginPwd = $('#loginPwd');
    var $loginSMS = $('#loginSMS');
    if ($login.attr('data-demo') === 'True') {
        $login.find('[name="userName"], [name="password"]').attr('data-valid', 'false');
    }

    var $loginType = $('#loginType');
    if ($loginType.length === 1) {
        $login.on('submit', function (e) {
            var model = $loginType.attr('data-value');
            if (model === 'username') {
                if ($username.val() === '' && $password.val() === '') {
                    e.preventDefault();
                    location.href = "Gitee";
                }
            }
            else {
                // sms
                var url = $.format('Account/Mobile{0}', location.search);
                $login.attr('action', $.formatUrl(url));
                return true;
            }
        });

        // login type
        $loginType.on('click', function (e) {
            e.preventDefault();
            var $this = $(this);
            $login.find('[data-toggle="tooltip"]').tooltip('hide');
            var model = $this.attr('data-value');
            if (model === 'username') {
                $loginUser.addClass('d-none');
                $loginPwd.addClass('d-none');
                $loginSMS.removeClass('d-none');
                $loginMobile.removeClass('d-none');

                $this.attr('data-value', 'sms').text('用户名密码登陆');
                $loginButton.attr('data-original-title', '请输入手机号码并点击发送验证码');
            }
            else {
                // sms model
                $loginUser.removeClass('d-none');
                $loginPwd.removeClass('d-none');
                $loginSMS.addClass('d-none');
                $loginMobile.addClass('d-none');

                $this.attr('data-value', 'username').text('短信验证登陆');
                $loginButton.attr('data-original-title', '不填写密码默认使用 Gitee 认证');
            }
        });
    }

    var timeHanlder = null;
    $('#btnSendCode').on('click', function () {
        // validate mobile phone
        var $phone = $('#phone');
        var validator = $login.find('[data-toggle="LgbValidate"]').lgbValidator();
        if (!validator.validElement($phone.get(0))) {
            $phone.tooltip('show');
            return;
        }

        var phone = $phone.val();
        var apiUrl = "api/Login?phone=" + phone;
        var $this = $(this);
        $.bc({
            url: apiUrl,
            method: 'PUT',
            callback: function (result) {
                $this.attr('data-original-title', result.Result ? "发送成功" : "短信登录体验活动结束").tooltip('show');
                var handler = setTimeout(function () {
                    clearTimeout(handler);
                    $this.tooltip('hide').attr('data-original-title', "点击发送验证码");
                }, 2000);

                if (result.Result) {
                    // send success
                    $this.text('已发送').attr('disabled', true);
                    $('#code').removeAttr('disabled');
                    if (result.Data === null) $loginButton.attr('data-original-title', '请输入验证码');
                    else {
                        $('#code').val(result.Data);
                        $loginButton.attr('data-original-title', '点击登录系统');
                    }

                    timeHanlder = setTimeout(function () {
                        clearTimeout(timeHanlder);
                        var count = 299;
                        timeHanlder = setInterval(function () {
                            if (count === 0) {
                                clearInterval(timeHanlder);
                                $this.text('发送验证码').removeAttr('disabled');
                                return;
                            }
                            $this.text(count-- + ' 秒后可重发');
                        }, 1000);
                    }, 1000);
                }
            }
        });
    });
});
