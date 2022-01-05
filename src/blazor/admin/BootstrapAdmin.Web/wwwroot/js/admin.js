(function ($) {
    $.extend({
        formatUrl: function (url) {
            if (!url) return url;
            if (url.substr(0, 4) === "http") return url;
            var base = $('head > base').attr('href');
            return base + url;
        },
        login: function (form, mobile, url) {
            var $form = $(form);
            if (mobile) {
                $form.submit();
            }
            else {
                var userName = $form.find('[name="userName"]').val();
                var password = $form.find('[name="password"]').val();
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
                            $('.error-msg').addClass("show");
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        console.log(errorThrown);
                    }
                });
            }
        }
    });
})(jQuery);
