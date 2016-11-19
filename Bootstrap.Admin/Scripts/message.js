$(function () {
    var html = '<tr {0}><td class="inbox-small-cells"><input type="checkbox" class="mail-checkbox"></td><td class="inbox-small-cells"><i class="fa fa-star {1}"></i></td><td class="view-message  dont-show">{2}</td><td class="view-message  dont-show">{3}</td><td class="view-message ">{4}</td><td class="view-message text-right">{5}</td></tr>';

    function listData(options) {

        $.bc({
            Id: options.Id, url: Messages.url, method: 'GET', swal: false,
            callback: function (result) {
                if (result) {
                    var content = result.map(function (mail) {
                        if (mail.Status == '0')
                            mailStatus = 'class="unread"';
                        else
                            mailStatus = " "
                        if (mail.Mark == '1')
                            mailMark = "inbox-started";
                        else
                            mailMark = " ";
                        return $.format(html, mailStatus, mailMark, mail.FromDisplayName, mail.Title, mail.Content, mail.SendTime);
                    }).join('');
                    $('#tbMsg').html(content);
                }
            }
        });
    }

    listData({ Id: 'inbox' });

    $('#mailBox').on('click', 'a', function () {
        listData({ Id: $(this).attr('data-id') });
    })
});