$(function () {
    var html = '<tr><td class="inbox-small-cells"><input type="checkbox" class="mail-checkbox"></td><td class="inbox-small-cells"><i class="fa fa-star"></i></td><td class="view-message  dont-show">{0}</td><td class="view-message  dont-show">{1}</td><td class="view-message ">{2}</td><td class="view-message text-right">{3}</td></tr>';

    function listData() {
        $.bc({
            Id: 'inbox', url: Messages.url, method: 'GET', swal: false,
            callback: function (result) {
                if (result) {
                    var content = result.map(function (mail) {
                        return $.format(html, mail.FromDisplayName, mail.Title, mail.Content, mail.SendTime);
                    }).join('');
                    $('#tbMsg').html(content);
                }
            }
        });
    }
    listData();
});