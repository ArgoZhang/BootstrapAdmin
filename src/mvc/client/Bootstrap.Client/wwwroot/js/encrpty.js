$(function () {
    $(document).on('click', '[data-method]', function (e) {
        var method = $(this).attr('data-method');
        switch (method) {
            case 'salt':
                $.bc({
                    url: 'api/Encrpty/Salt', method: 'get', callback: function (result) {
                        $('#salt').val(result);
                    }
                });
                break;
            case 'hash':
                $.bc({
                    url: 'api/Encrpty/Hash', method: 'post', data: { password: $('#password').val(), salt: $('#salt').val() }, callback: function (result) {
                        $('#hash').val(result);
                    }
                });
                break;
            default:
                break;
        }
    });

    $.footer();
});