$(function () {

    var url = "../api/Profiles";

    $.ajax({
        url: url,
        type: 'GET',
        success: function (result) {
            if (result) {
                $('#sysNameSet').val(result[0]);
                $('#footSet').val(result[1]);
            }
            else {
                swal("失败", "加载数据失败", "error");
            }
        }
    });

    $('#sysSave').click(function () {
         var dvalue = $('#sysNameSet').val();
        $.ajax({
            url: url,
            data: { "type": "sysName", "dvalue": dvalue },
            type: 'POST',
            success: function (result) {
                if (result) {
                    swal("成功", "设置网站标题成功", "success");
                }
                else {
                    swal("失败", "设置网站标题失败", "error");
                }
            }
        });
    });

    $('#footSave').click(function () {
        var dvalue = $('#footSet').val();
        $.ajax({
            url: url,
            data: { "type": "foot", "dvalue": dvalue },
            type: 'POST',
            success: function (result) {
                if (result) {
                    swal("成功", "设置网站页脚成功", "success");
                }
                else {
                    swal("失败", "设置网站页脚失败", "error");
                }
            }
        });
    });

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

  

  
})