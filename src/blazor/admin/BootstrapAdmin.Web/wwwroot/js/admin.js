(function ($) {
    $.extend({
        formatUrl: function (url) {
            if (!url) return url;
            if (url.substr(0, 4) === "http") return url;
            var base = $('head > base').attr('href');
            return base + url;
        },
        login: function (form, url) {
            var $form = $(form);
            var $login = $('.login-wrap');

            $('[data-bs-toggle="tooltip"]').tooltip();

            // handle input event
            $('input').on('change', function () {
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
                var mobile = $form.attr('data-mobile') === '';
                if (mobile) {
                    $form.submit();
                }
                else {
                    var $userName = $form.find('[name="userName"]');
                    var $password = $form.find('[name="password"]');
                    var userName = $userName.val();
                    var password = $password.val();
                    if (userName !== '' && password !== '') {
                        var postData = JSON.stringify({ userName, password });
                        // call webapi authenticate
                        $.ajax({
                            url: $.formatUrl(url),
                            data: postData,
                            method: 'POST',
                            contentType: 'application/json',
                            dataType: 'json',
                            crossDomain: false,
                            success: function (result) {
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
                }
            });
        }
    });
})(jQuery);
