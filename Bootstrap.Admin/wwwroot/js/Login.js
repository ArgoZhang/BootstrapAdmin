$(function () {
    $(".container").autoCenter();

    // validate
    $('#login').autoValidate({
        userName: {
            required: true,
            maxlength: 50
        },
        password: {
            required: true,
            maxlength: 50
        }
    });
})