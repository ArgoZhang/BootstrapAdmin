$(function () {
    var swalDeleteOptions = {
        title: "删除前台站点配置",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#dc3545',
        cancelButtonColor: '#6c757d',
        confirmButtonText: "我要删除",
        cancelButtonText: "取消"
    };

    var dataBinder = new DataEntity({
        Title: "#sysName",
        Footer: "#sysFoot"
    });

    var $dialog = $('#dialogNew');

    $(document).on('click', 'button[data-method]', function (e) {
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
                var enableHealth = $('#health').prop('checked') ? "1" : "0";
                $.bc({
                    url: Settings.url, data: [
                        { name: 'ShowCardTitle', code: cardTitle },
                        { name: 'ShowSideBar', code: uiSettings },
                        { name: 'FixedTableHeader', code: fixedTableHeader },
                        { name: 'EnableHealth', code: enableHealth }
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
                    url: Settings.url + '/Demo', data: { name: authKey, code: demo }, title: '演示系统设置', method: "post",
                    callback: function (result) {
                        if (result) {
                            window.setTimeout(function () { window.location.reload(true); }, 1000);
                        }
                    }
                });
                break;
            case 'appPath':
                var appPath = $('#sysAppPath').val();
                $.bc({
                    url: Settings.url, data: [{ name: 'AppPath', code: appPath }], title: '后台管理地址设置', method: "post"
                });
                break;
            case 'addApp':
                $('#appKey').val('').removeAttr('readonly');
                $('#appName').val('').removeAttr('readonly');
                $('#appUrl').val('');
                $('#appTitle').val('');
                $('#appFooter').val('');
                $('#appIcon').val('');
                $('#appFavicon').val('');
                $('#appId').val('new');
                $dialog.modal('show');
                break;
            case 'saveNewApp':
                var appPath = $('#appUrl').val();
                var appKey = $('#appKey').val();
                var appName = $('#appName').val();
                var appTitle = $('#appTitle').val();
                var appFooter = $('#appFooter').val();
                var appId = $('#appId').val();
                var appIcon = $('#appIcon').val();
                var appFavicon = $('#appFavicon').val();
                $.bc({
                    url: Settings.url, data: { AppIcon: appIcon, AppFavicon: appFavicon, AppName: appName, AppCode: appKey, AppUrl: appPath, AppTitle: appTitle, AppFooter: appFooter, AppId: appId }, title: "保存" + appName, method: "put",
                    callback: function (result) {
                        if (result) {
                            $dialog.modal('hide');

                            if (appId === 'new') {
                                // 保存成功创建新 dom
                                var segment = $.format('<div class="form-group col-12 app" data-key="{0}"><label class="control-label" for="{0}">{1}</label><div class="input-group flex-fill"><input id="{0}" class="form-control" placeholder="请输入应用首页，2000字以内" value="{2}" maxlength="2000" data-valid="true" /><div class="input-group-append"><button class="btn btn-danger" type="button" data-key="{0}" data-method="delApp"><i class="fa fa-trash-o"></i><span>删除</span></button><button class="btn btn-primary" type="button" data-key="{0}" data-method="editApp"><i class="fa fa fa-pencil"></i><span>编辑</span></button></div></div></div>', appKey, appName, appPath);

                                // append dom
                                $('#appList').append($(segment));
                            }
                            else {
                                // update
                                $('#' + appKey).val(appPath);
                            }
                        }
                    }
                });
                break;
            case 'editApp':
                $('#appId').val('edit');
                $('#appKey').attr('readonly', true);
                $('#appName').attr('readonly', true);
                var appKey = $(this).parents('.app').attr('data-key');
                $.bc({
                    url: Settings.url, id: appKey, method: 'get',
                    callback: function (result) {
                        if (result) {
                            $('#appUrl').val(result.AppUrl);
                            $('#appKey').val(result.AppCode);
                            $('#appName').val(result.AppName);
                            $('#appTitle').val(result.AppTitle);
                            $('#appFooter').val(result.AppFooter);
                            $('#appIcon').val(result.AppIcon);
                            $('#appFavicon').val(result.AppFavicon);
                            $dialog.modal('show');
                        }
                    }
                });
                break;
            case 'delApp':
                var appKey = $(this).attr('data-key');
                var appName = $(this).parents('.input-group').prev().text();
                var $this = $(this);
                swal($.extend({}, swalDeleteOptions, { html: "您确定要删除" + appName + "前台站点配置吗" })).then(function (result) {
                    if (result.value) {
                        $.bc({
                            url: Settings.url + '/AppPath', data: { name: appName, code: appKey }, title: "删除" + appName, method: "delete",
                            callback: function (result) {
                                // remove dom
                                $this.parents('.form-group').remove();
                            }
                        });
                    }
                });
                break;
            case 'saveLoginView':
                var logView = $('#loginView').val();
                $.bc({
                    url: Settings.url, data: [{ name: 'Login', code: logView }], title: '保存登录界面设置', method: "post"
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