$(function () {
    $('a[data-admin="False"]').hide();

    var dataBinder = new DataEntity({
        Title: "#sysName",
        Footer: "#sysFoot"
    });

    $('button[data-method]').on('click', function (e) {
        var $this = $(this);
        var data = {};
        switch ($this.attr('data-method')) {
            case 'footer':
                data = dataBinder.get();
                $.bc({
                    url: Settings.url, data: { name: '网站页脚', code: data.Footer, category: '网站设置' }, title: '保存网站页脚', method: "post",
                    callback: function (result) {
                        if (result) $('#websiteFooter').text(data.Footer);
                    }
                });
                break;
            case 'title':
                data = dataBinder.get();
                $.bc({
                    url: Settings.url, data: { name: '网站标题', code: data.Title, category: '网站设置' }, title: '保存网站标题', method: "post",
                    callback: function (result) {
                        if (result) $('#websiteTitle').text(data.Title);
                    }
                });
                break;
            case 'css':
                var cssDefine = $css.val();
                $.bc({
                    url: Settings.url, data: { name: '使用样式', code: cssDefine, category: '当前样式' }, title: '保存网站样式', method: "post",
                    callback: function (result) {
                        if (result) {
                            window.setTimeout(function () { window.location.reload(true); }, 1000);
                        }
                    }
                });
                break;
        }
    });

    var $sortable = $('#sortable');
    var $refresh = $('a[data-method="refresh"]');
    var listCacheUrl = function (options) {
        $refresh.addClass('fa-spin');
        options = $.extend({ clear: false }, options);
        $sortable.html('');
        $.bc({
            url: Settings.url,
            callback: function (urls) {
                if (urls && $.isArray(urls)) {
                    $.each(urls, function (index, item) {
                        if (options.clear) options.url = item.Url + "?clear=clear";
                        else options.url = item.Url;
                        $.bc({
                            url: options.url,
                            cors: !item.Self,
                            callback: function (result) {
                                if ($.isArray(result)) {
                                    var html = '<div class="cache-item"><i class="fa fa-ellipsis-v"></i><div><span data-toggle="tooltip" title="{2}">{2}</span><span class="badge badge-pill badge-success">{0}</span></div><span title="{3}">{3}</span><div><span>{6}</span><button class="btn btn-danger" title="{1}" data-url="{4}?cacheKey={1}" data-toggle="tooltip" data-self="{5}" data-placement="left"><i class="fa fa-trash-o"></i></button></div></div>';
                                    var content = result.sort(function (x, y) {
                                        return x.Key > y.Key ? 1 : -1;
                                    }).map(function (ele) {
                                        return $.format(html, ele.Interval, ele.Key, ele.Desc, ele.Value, $.format(item.Url, ele.Key), item.Self, ele.ElapsedSeconds);
                                    }).join('');

                                    var cache = $('<div class="card-cache"></div>');
                                    cache.append($.format('<h6>{0}</h6>', item.Desc));
                                    cache.append(content);
                                    $sortable.append(cache);
                                    $sortable.find('[data-toggle="tooltip"]').tooltip();
                                }
                                if (index === urls.length - 1) $refresh.removeClass('fa-spin');
                                $('.site-footer').footer();
                            }
                        });
                    });
                }
                else $refresh.removeClass('fa-spin');
                $('.site-footer').footer();
            }
        });
    };

    $('a[data-method]').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
        var $this = $(this).tooltip('hide');
        var options = {};
        switch ($this.attr('data-method')) {
            case 'clear':
                options.clear = true;
                break;
            case 'refresh':
                break;
        }
        listCacheUrl(options);
    }).last().trigger('click');
    $sortable.on('click', '.btn', function () {
        var $this = $(this).tooltip('dispose');
        $.bc({ url: $this.attr('data-url'), cors: $this.attr('data-self') === 'false' });
        listCacheUrl();
    });

    var $css = $('#dictCssDefine').dropdown('val');
});