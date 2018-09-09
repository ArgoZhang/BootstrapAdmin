$(function () {

    function loadData() {
        $.bc({
            url: Messages.url,
            callback: function (result) {
                if (result) {
                    $('#s_inbox').text(result.inboxCount);
                    $('#s_sendmail').text(result.sendmailCount);
                    $('#s_mark').text(result.markCount);
                    $('#s_trash').text(result.trashCount);
                }
            }
        });
    }

    var html = '<tr {0}><td class="inbox-small-cells"><input type="checkbox" class="mail-checkbox"></td><td class="inbox-small-cells"><i class="fa fa-star {1}"></i></td><td class="view-message  dont-show">{2}<span class="label {3} pull-right">{4}</span></td><td class="view-message  dont-show">{5}</td><td class="view-message ">{6}</td><td class="view-message text-right">{7}</td></tr>';

    function listData(options) {
        $.bc({
            id: options.id, url: Messages.url,
            callback: function (result) {
                if (result) {
                    var content = result.map(function (mail) {
                        if (mail.Status === '0')
                            mailStatus = 'class="unread"';
                        else
                            mailStatus = " ";
                        if (mail.Mark === '1')
                            mailMark = "inbox-started";
                        else
                            mailMark = " ";
                        if (mail.Label === '0')
                            mailLabel = 'label-success';
                        else
                            mailLabel = 'label-warning';
                        return $.format(html, mailStatus, mailMark, mail.FromDisplayName, mailLabel, mail.LabelName, mail.Title, mail.Content, mail.SendTime);
                    }).join('');
                    $('#tbMsg').html(content);
                }
            }
        });
    }

    listData({ id: 'inbox' });
    loadData();

    $('#mailBox').on('click', 'a', function () {
        listData({ Id: $(this).attr('data-id') });
    });
});