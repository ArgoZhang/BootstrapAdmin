(function ($) {
    $.extend({
        formatUrl: function (url) {
            if (!url) return url;
            if (url.substr(0, 4) === "http") return url;
            var base = $('head > base').attr('href');
            return base + url;
        },
        login: function (form, obj, url, method) {
            var $form = $(form);
            var $login = $('.login-wrap');

            $('[data-bs-toggle="tooltip"]').tooltip();

            // handle input event
            $form.on('change', 'input', function () {
                // dispose login wrap error
                $login.tooltip('dispose');

                var $this = $(this);
                var val = $this.val();
                if (val.length > 0) {
                    $this.removeClass('is-invalid');
                    $this.tooltip('dispose');
                }
                else {
                    $this.addClass('is-invalid');
                    var handler = window.setTimeout(function () {
                        if (handler) {
                            window.clearTimeout(handler);
                        }
                        $this.tooltip('show');
                    }, 100);
                }
            });

            $('.btn-login').on('click', function (e) {
                e.preventDefault();
                var mobile = $form.find('.is-mobile').length === 1;
                var userName = "";
                var password = "";
                if (mobile) {
                    var $phone = $form.find('[name="phone"]');
                    var $code = $form.find('[name="code"]');
                    userName = $phone.val();
                    password = $code.val();

                    if (userName === '') {
                        $phone.trigger("change");
                        return;
                    }
                    if (password === '') {
                        $code.trigger("change");
                        return;
                    }
                }
                else {
                    var $userName = $form.find('[name="userName"]');
                    var $password = $form.find('[name="password"]');
                    userName = $userName.val();
                    password = $password.val();

                    if (userName === '') {
                        $userName.trigger("change");
                        return;
                    }
                    if (password === '') {
                        $password.trigger("change");
                        return;
                    }
                }

                var authenticate = function (userName, postData, mobile) {
                    var postUrl = url + '?mobile=' + mobile;
                    $.ajax({
                        url: $.formatUrl(postUrl),
                        data: postData,
                        method: 'POST',
                        contentType: 'application/json',
                        dataType: 'json',
                        crossDomain: false,
                        success: function (result) {
                            obj.invokeMethodAsync(method, userName, result.authenticated);
                            if (result.authenticated) {
                                $form.submit();
                            }
                            else {
                                console.log(result.error);
                                var $login = $('.login-wrap').addClass('is-invalid').tooltip({
                                    title: result.error
                                });
                                var handler = window.setTimeout(function () {
                                    if (handler) {
                                        window.clearTimeout(handler);
                                    }
                                    $login.tooltip('show');
                                }, 100);
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            console.log(errorThrown);
                        }
                    });
                }

                var postData = JSON.stringify({ userName, password });
                // call webapi authenticate
                authenticate(userName, postData, mobile);
            });
        }
    });
})(jQuery);
