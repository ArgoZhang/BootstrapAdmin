$(function () {
    $('a[data-admin="False"]').hide();

    var bsa = new BootstrapAdmin({
        url: Settings.url,
        bootstrapTable: null,
        validateForm: null,
        modal: null,
        dataEntity: new DataEntity({
            map: {
                Title: "sysName",
                Footer: "sysFoot"
            }
        })
    });

    $('button[data-method]').on('click', function (e) {
        var $this = $(this);
        switch ($this.attr('data-method')) {
            case 'footer':
                var data = bsa.dataEntity.get();
                $.bc({
                    url: Settings.url, data: { name: '网站页脚', code: data.Footer, category: Settings.title }, title: Settings.title,
                    callback: function (result) {
                        if (result) $('#websiteFooter').text(data.Footer);
                    }
                });
                break;
            case 'title':
                var data = bsa.dataEntity.get();
                $.bc({
                    url: Settings.url, data: { name: '网站标题', code: data.Title, category: Settings.title }, title: Settings.title,
                    callback: function (result) {
                        if (result) $('#websiteTitle').text(data.Title);
                    }
                });
                break;
            case 'css':
                var cssDefine = $css.val();
                $.bc({
                    url: Settings.url, data: { name: '使用样式', code: cssDefine, category: '当前样式' }, title: '网站样式',
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
    var listCacheUrl = function (options) {
        options = $.extend({ clear: false }, options);
        $sortable.html('');
        $.bc({
            url: Settings.url,
            method: 'GET',
            swal: false,
            callback: function (result) {
                if (result && $.isArray(result)) {
                    $.each(result, function (index, item) {
                        if (options.clear) options.url = item.Url + "?clear=clear";
                        else options.url = item.Url;
                        $.bc({
                            url: options.url,
                            xhrFields: {
                                withCredentials: true
                            },
                            swal: false,
                            callback: function (result) {
                                if ($.isArray(result)) {
                                    var html = '<div class="cache-item"><i class="fa fa-ellipsis-v"></i><div><span data-toggle="tooltip" title="{2}">{2}</span><span class="badge badge-pill badge-success">{0}</span></div><span title="{3}">{3}</span><div><span>{7}</span><button class="btn btn-danger btn-xs" title="{1}" data-url="{5}?cacheKey={1}" data-toggle="tooltip" data-self="{6}" data-placement="left"><i class="fa fa-trash-o"></i></button></div></div>';
                                    var content = result.sort(function (x, y) {
                                        return x.Key > y.Key ? 1 : -1;
                                    }).map(function (ele) {
                                        return $.format(html, ele.Interval, ele.Key, ele.Desc, ele.Value, '', $.format(item.Url, ele.Key), item.Self, Math.max(0, ele.Interval - Math.round((new Date() - new Date(ele.CreateTime.replace(/-/g, '/'))) / 1000)));
                                    }).join('');
                                    $sortable.append($.format('<h6 class="cache-title">{0}</h6>', item.Desc));
                                    $sortable.append(content);
                                    $sortable.find('[data-toggle="tooltip"]').tooltip();
                                }
                            }
                        });
                    });
                }
            }
        });
    }

    var listCache = function (options) {
        $.bc({
            url: options.url,
            xhrFields: {
                withCredentials: true
            },
            swal: false
        });
    }
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
        $(this).tooltip('dispose');
        listCache({ url: $(this).attr('data-url') });
        listCacheUrl();
    });

    var $css = $('#dictCssDefine');
    $.bc({
        url: Dicts.css, swal: false,
        callback: function (result) {
            var html = result.map(function (ele, index) { return $.format('<li><a href="#" data-val="{1}">{0}</a></li>', ele.Name, ele.Code); }).join('');
            $('#cssContainer').append(html);
            $.bc({
                url: Dicts.css, swal: false, method: 'get',
                callback: function (result) {
                    if (result.length > 0)
                        $css.val(result).dropdown('val');
                }
            });
        }
    });
})