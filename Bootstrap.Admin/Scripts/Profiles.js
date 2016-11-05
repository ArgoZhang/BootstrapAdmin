$(function () {
    var html = '<li class="list-primary"><i class="fa fa-ellipsis-v"></i><div class="task-title"><span class="task-title-sp">{2}</span><span class="badge badge-sm label-success">{0}</span><div class="task-wrapper"><span class="task-value tooltips" data-placement="top" data-original-title="{3}">{3}</span></div><div class="pull-right hidden-phone"><button class="btn btn-danger btn-xs fa fa-trash-o tooltips" data-key="{1}" data-placement="left" data-original-title="{1}"></button></div></div></li>';

    function listCache(options) {
        $.ajax({
            url: options.url,
            type: 'GET',
            success: function (result) {
                if (result) {
                    result = $.parseJSON(result);
                    if ($.isArray(result)) {
                        var content = result.map(function (ele) {
                            return $.format(html, ele.Interval, ele.Key, ele.Desc, ele.Value);
                        }).join('');
                        $('#sortable').html(content);
                        $('.tooltips').tooltip();
                        $('#sortable .btn').click(function () {
                            var key = $(this).attr('data-key');
                            listCache({ url: $.format('../../CacheList.axd?cacheKey={0}', key) });
                        });
                    }
                }
                else {
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    }

    listCache({ url: '../../CacheList.axd' });

    $('a.fa-refresh').click(function () { listCache({ url: '../../CacheList.axd' }); });
});