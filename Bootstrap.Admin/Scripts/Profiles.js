$(function () {
    var html = '<li class="list-primary"><i class="fa fa-ellipsis-v"></i><div class="task-title"><span class="task-title-sp">{2}</span><span class="badge badge-sm label-success">{0}</span><span class="task-value">{3}</span><div class="pull-right hidden-phone"><button class="btn btn-danger btn-xs fa fa-trash-o" data-val="{1}"></button></div></div></li>';
    $.ajax({
        url: '../../CacheList.axd',
        type: 'GET',
        success: function (result) {
            if (result) {
                result = $.parseJSON(result);
                if ($.isArray(result)) {
                    var content = result.map(function (ele) {
                        return $.format(html, ele.Interval, ele.Key, ele.Desc, ele.Value);
                    }).join('');
                    $('#sortable').append(content);
                }
            }
            else {
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        }
    });
});