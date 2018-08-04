$(function () {
    $('#btnAccount').click(function () {
        var valid = $('form').valid();
        if (valid) {
            $('.setup-main').hide();
            $('.steps li').toggleClass('current');
            $('#loginID').text($('#userName').val());
            $('#loginName').text($('#displayName').val());
            $('#loginDesc').text($('#description').val());
            $('.setup-confirm').show();
        }
    });

    $('#btnPrev').click(function () {
        $('.steps li').toggleClass('current');
        $('.setup-main').show();
        $('.setup-confirm').hide();
    });
});