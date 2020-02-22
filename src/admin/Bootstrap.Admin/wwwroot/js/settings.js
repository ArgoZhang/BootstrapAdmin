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
                    url: Settings.url, data: [
                        { name: 'SaveWebFooter', code: data.Footer }
                    ], title: '保存网站页脚', method: "post",
                    callback: function (result) {
                        if (result) $('#websiteFooter').text(data.Footer);
                    }
                });
                break;
            case 'title':
                data = dataBinder.get();
                $.bc({
                    url: Settings.url, data: [
                        { name: 'SaveWebTitle', code: data.Title }
                    ], title: '保存网站标题', method: "post",
                    callback: function (result) {
                        if (result) $('#websiteTitle, aside .nav-brand a span').text(data.Title);
                    }
                });
                break;
            case 'css':
                var cssDefine = $css.val();
                $.bc({
                    url: Settings.url, data: [
                        { name: 'SaveTheme', code: cssDefine }
                    ], title: '保存网站样式', method: "post",
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
                        { name: 'ShowCardTitle', code: cardTitle },
                        { name: 'ShowSideBar', code: uiSettings },
                        { name: 'FixedTableHeader', code: fixedTableHeader }
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
                    url: Settings.url, data: [
                        { name: 'OAuth', code: oauth },
                        { name: 'SMS', code: mobile }
                    ], title: '登录设置', method: "post"
                });
                break;
            case 'saveAutoLock':
                var autoLock = $('#lockScreen').prop('checked') ? "1" : "0";
                $.bc({
                    url: Settings.url, data: [
                        { name: 'AutoLock', code: autoLock },
                        { name: 'AutoLockPeriod', code: $('#lockPeriod').val() }
                    ], title: '保存自动锁屏设置', method: "post",
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
                    url: Settings.url, data: [{ name: 'DefaultApp', code: defaultApp }], title: '保存默认应用程序设置', method: "post"
                });
                break;
            case 'saveBlazor':
                var blazor = $('#blazor').prop('checked') ? "1" : "0";
                $.bc({
                    url: Settings.url, data: [{ name: 'Blazor', code: blazor }], title: 'Blazor 设置', method: "post",
                    callback: function (result) {
                        if (result) {
                            // 通过值设置是否显示 Blazor 挂件
                            var $blazor = $('header .nav .dropdown-blazor').parent();
                            if (blazor === "1") $blazor.removeClass('d-none');
                            else $blazor.addClass('d-none');
                        }
                    }
                });
                break;
            case 'saveIpLocator':
                var iplocator = $iplocator.val();
                $.bc({
                    url: Settings.url, data: [{ name: 'IPLocator', code: iplocator }], title: '保存地理位置服务设置', method: "post"
                });
                break;
            case 'saveLogPeriod':
                var errLog = $('#appErrorLog').val();
                var opLog = $('#opLog').val();
                var logLog = $('#logLog').val();
                var traceLog = $('#traceLog').val();
                var cookiePeriod = $('#cookiePeriod').val();
                var ipCachePeriod = $('#ipCachePeriod').val();
                $.bc({
                    url: Settings.url, data: [
                        { name: 'ErrLog', code: errLog },
                        { name: 'OpLog', code: opLog },
                        { name: 'LogLog', code: logLog },
                        { name: 'TraceLog', code: traceLog },
                        { name: 'CookiePeriod', code: cookiePeriod },
                        { name: 'IPCachePeriod', code: ipCachePeriod }
                    ], title: '保存日志缓存设置', method: "post"
                });
                break;
            case 'saveDemo':
                var demo = $('#demo').prop('checked') ? "1" : "0";
                var authKey = $('#authKey').val();
                $.bc({
                    url: Settings.url + '/Demo', data: { name: authKey, code: demo }, title: '演示系统设置', method: "put",
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
    var $iplocator = $('#iplocator').dropdown('val');
});