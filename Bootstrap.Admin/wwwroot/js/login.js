$(function () {
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
                    if (result) captcha.addClass('d-block');
                    else success();
                }
            });
        },
        capWidth: function () {
            return $(window).width() < 768 ? 216 : 280;
        },
        capHeight: function () {
            return $(window).width() < 768 ? 110 : 150;
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

    $('button[type="submit"]').on('click', function (e) {
        $.captchaCheck($('#login .slidercaptcha'), function () {
            $('form').submit();
        });
        return false;
    });

    $('#captcha, #regcap, #forgotcap').sliderCaptcha({
        width: $.capWidth(),
        height: $.capHeight(),
        setSrc: function () {
            return 'http://pocoafrro.bkt.clouddn.com/Pic' + Math.round(Math.random() * 136) + '.jpg';
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
});