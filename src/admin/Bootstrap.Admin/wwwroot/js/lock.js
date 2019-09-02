$(function () {
    $('#time').text((new Date()).format('HH:mm:ss'));

    setInterval(function () {
        $('#time').text((new Date()).format('HH:mm:ss'));
    }, 500);

    $(".lock-wrapper").autoCenter();
});
