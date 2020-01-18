$(function () {
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
                    url: Settings.url, data: [{ name: '网站页脚', code: data.Footer, category: '网站设置' }], title: '保存网站页脚', method: "post",
                    callback: function (result) {
                        if (result) $('#websiteFooter').text(data.Footer);
                    }
                });
                break;
            case 'title':
                data = dataBinder.get();
                $.bc({
                    url: Settings.url, data: [{ name: '网站标题', code: data.Title, category: '网站设置' }], title: '保存网站标题', method: "post",
                    callback: function (result) {
                        if (result) $('#websiteTitle').text(data.Title);
                    }
                });
                break;
            case 'css':
                var cssDefine = $css.val();
                $.bc({
                    url: Settings.url, data: [{ name: '使用样式', code: cssDefine, category: '当前样式' }], title: '保存网站样式', method: "post",
                    callback: function (result) {
                        if (result) {
                            window.setTimeout(function () { window.location.reload(true); }, 1000);
                        }
                    }
                });
                break;
            case 'UISettings':
                var uiSettings = $('#sider').prop('checked') ? "1" : "0";
                var cardTitle = $('#cardTitle').prop('checked') ? "1" : "0";
                var fixedTableHeader = $('#tableHeader').prop('checked') ? "1" : "0";
                $.bc({
                    url: Settings.url, data: [
                        { name: '卡片标题状态', code: cardTitle, category: '网站设置' },
                        { name: '侧边栏状态', code: uiSettings, category: '网站设置' },
                        { name: '固定表头', code: fixedTableHeader, category: '网站设置' }
                    ], title: '保存网站设置', method: "post",
                    callback: function (result) {
                        if (result) {
                            window.setTimeout(function () { window.location.reload(true); }, 1000);
                        }
                    }
                });
                break;
            case 'loginSettings':
                var mobile = $('#mobile').prop('checked') ? "1" : "0";
                var oauth = $('#oauth').prop('checked') ? "1" : "0";
                $.bc({
                    url: Settings.url, data: [{ name: 'OAuth 认证登录', code: oauth, category: '网站设置' }, { name: '短信验证码登录', code: mobile, category: '网站设置' }], title: '登录设置', method: "post"
                });
                break;
            case 'saveAutoLock':
                var autoLock = $('#lockScreen').prop('checked') ? "1" : "0";
                $.bc({
                    url: Settings.url, data: [{ name: '自动锁屏', code: autoLock, category: '网站设置' }, { name: '自动锁屏时长', code: $('#lockPeriod').val(), category: '网站设置' }], title: '保存自动锁屏设置', method: "post",
                    callback: function (result) {
                        if (result) {
                            window.setTimeout(function () { window.location.reload(true); }, 1000);
                        }
                    }
                });
                break;
            case 'saveDefaultApp':
                var defaultApp = $('#defaultApp').prop('checked') ? "1" : "0";
                $.bc({
                    url: Settings.url, data: [{ name: '默认应用程序', code: defaultApp, category: '网站设置' }], title: '保存默认应用程序设置', method: "post"
                });
                break;
            case 'saveBlazor':
                var blazor = $('#blazor').prop('checked') ? "1" : "0";
                $.bc({
                    url: Settings.url, data: [{ name: 'Blazor', code: blazor, category: '网站设置' }], title: 'Blazor 设置', method: "post",
                    callback: function (result) {
                        if (result) {
                            // 导航到 Blazor 页面
                            window.location.href = $.formatUrl("Pages/Admin/Settings");
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
            autoFooter: true,
            callback: function (urls) {
                if (urls && $.isArray(urls)) {
                    $.each(urls, function (index, item) {
                        if (options.clear) options.url = item.Url + "?cacheKey=*";
                        else options.url = item.Url;
                        $.bc({
                            url: options.url,
                            cors: !item.Self,
                            autoFooter: true,
                            callback: function (result) {
                                if ($.isArray(result)) {
                                    var html = '<div class="cache-item"><i class="fa fa-ellipsis-v"></i><span data-toggle="tooltip" title="{2}">{2}</span><span class="badge badge-pill badge-success">{0}</span><span title="{3}">{3}</span><div><span>{6}</span><button class="btn btn-danger" title="{1}" data-url="{4}?cacheKey={1}" data-toggle="tooltip" data-self="{5}" data-placement="left"><i class="fa fa-trash-o"></i></button></div></div>';
                                    var content = result.sort(function (x, y) {
                                        return x.Key > y.Key ? 1 : -1;
                                    }).map(function (ele) {
                                        return $.format(html, ele.Interval / 1000, ele.Key, ele.Desc, ele.Value, $.format(item.Url, ele.Key), item.Self, ele.ElapsedSeconds);
                                    }).join('');

                                    var cache = $('<div class="card-cache"></div>');
                                    cache.append($.format('<h6>{0}</h6>', item.Desc));
                                    cache.append(content);
                                    $sortable.append(cache);
                                    $sortable.find('[data-toggle="tooltip"]').tooltip();
                                }
                            }
                        });
                    });
                }
                $refresh.removeClass('fa-spin');
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