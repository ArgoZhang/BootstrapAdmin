$(function () {
    $('form').autoValidate({
        userName: {
            required: true,
            maxlength: 50
        },
        displayName: {
            required: true,
            maxlength: 50
        },
        password: {
            required: true,
            maxlength: 50
        }
    });

    $('#btnAccount').click(function () {
        var valid = $('form').valid();
        if (valid) {
            $('.setup-main').hide();
            $('.steps li').toggleClass('current');
            $('.setup-confirm span:first').text($('#userName').val());
            $('.setup-confirm span:last').text($('#displayName').val());
            $('.setup-confirm').show();
        }
    });
});