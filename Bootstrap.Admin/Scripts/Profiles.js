$(function () {
    $('a[data-admin="False"]').hide();
    $('#headerDataForm').autoValidate({
        sysName: {
            required: true,
            maxlength: 50
        }
    }, {
        button: ['sysSave']
    });
    $('#footerDataForm').autoValidate({
        sysFoot: {
            required: true,
            maxlength: 50
        }
    }, {
        button: ['footSave']
    });

    var bsa = new BootstrapAdmin({
        url: '../api/Profiles',
        bootstrapTable: null,
        validateForm: null,
        modal: null,
        dataEntity: new DataEntity({
            map: {
                Title: "sysName",
                Footer: "sysFoot"
            }
        }),
        click: {
            assign: [{
                id: 'sysSave',
                click: function (row, data) {
                    if ($(this).attr('data-valid') == "true") {
                        Profiles.saveWebSite({ name: '网站标题', code: data.Title, category: '网站设置' });
                    }
                }
            }, {
                id: 'footSave',
                click: function (row, data) {
                    if ($(this).attr('data-valid') == "true") {
                        Profiles.saveWebSite({ name: '网站页脚', code: data.Footer, category: '网站设置' });
                    }
                }
            }]
        }
    });

    var html = '<li class="list-primary"><i class="fa fa-ellipsis-v"></i><div class="task-title"><span class="task-title-sp tooltips" data-placement="right" title="{1}">{2}</span><span class="badge badge-sm label-success">{0}</span><span class="task-value tooltips" data-placement="top" data-original-title="{3}">{3}</span><div class="pull-right hidden-phone"><button class="btn btn-danger btn-xs fa fa-trash-o" data-key="{1}"></button></div></div></li>';

    function listCache(options) {
        options = $.extend({ url: '../../CacheList.axd' }, options);
        $.ajax({
            url: options.url,
            type: 'POST',
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

    listCache();
    $('#refreshCache').click(function () { listCache(); });
    $('#clearCache').click(function () { listCache({ url: '../../CacheList.axd?clear=clear' }); });
})