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
        url: Settings.url,
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
                        $.bc({
                            url: Settings.url, data: { name: '网站标题', code: data.Title, category: Settings.title }, title: Settings.title,
                            callback: function (result) {
                                if (result) $('#websiteTitle').text(data.Title);
                            }
                        });
                    }
                }
            }, {
                id: 'footSave',
                click: function (row, data) {
                    if ($(this).attr('data-valid') == "true") {
                        $.bc({
                            url: Settings.url, data: { name: '网站页脚', code: data.Footer, category: Settings.title }, title: Settings.title,
                            callback: function (result) {
                                if (result) $('#websiteFooter').text(data.Footer);
                            }
                        });
                    }
                }
            }, {
                id: 'cssSave',
                click: function (row, data) {
                    var cssDefine = $('#dictCssDefine').val();
                    $.bc({
                        url: Settings.url, data: { name: '使用样式', code: cssDefine, category: '当前样式' }, title: '网站样式',
                        callback: function (result) {
                            if (result) {
                                window.setTimeout(function () { window.location.reload(true); }, 1000);
                            }
                        }
                    });
                }
            }]
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
                        listCache($.extend({ item: item, url: item.Url }, options));
                    });
                }
            }
        });
    }

    var listCache = function (options) {
        options = $.extend({ clear: false, key: '' }, options);
        if (options.key != '') {
            options.url = $.format(options.url, options.key);
        }
        if (options.clear) {
            options.url += '&clear=clear';
        }
        $.bc({
            url: options.url,
            swal: false,
            callback: function (result) {
                if (result && options.key == '') {
                    result = $.parseJSON(result);
                    if ($.isArray(result)) {
                        var html = '<li class="{4}"><i class="fa fa-ellipsis-v"></i><div class="task-title"><span class="task-title-sp" role="tooltip" title="{1}">{2}</span><span class="badge badge-sm label-success">{0}</span><span class="task-value" title="{3}">{3}</span><div class="pull-right hidden-phone"><span>{6}</span><button class="btn btn-danger btn-xs fa fa-trash-o" title="{1}" data-key="{1}" data-url="{5}" role="tooltip" data-placement="left"></button></div></div></li>';
                        var content = result.sort(function (x, y) {
                            return x.Key > y.Key ? 1 : -1;
                        }).map(function (ele) {
                            var key = ele.Key.split('-')[0];
                            var css = 'list-default';
                            switch (key) {
                                case "MenuHelper":
                                    css = 'list-primary';
                                    break;
                                case "UserHelper":
                                    css = 'list-success';
                                    break;
                                case "RoleHelper":
                                    css = 'list-danger';
                                    break;
                                case "GroupHelper":
                                    css = 'list-warning';
                                    break;
                                case "LogHelper":
                                    css = 'list-info';
                                    break;
                                case "DictHelper":
                                    css = 'list-inverse';
                                    break;
                                case "ExceptionHelper":
                                    css = 'list-Exception';
                                    break;
                                case "MessageHelper":
                                    css = 'list-Message';
                                    break;
                                case "TaskHelper":
                                    css = 'list-Task';
                                    break;
                                case "NotificationHelper":
                                    css = 'list-Notification';
                                    break;
                                default:
                                    break;
                            }
                            return $.format(html, ele.Interval, ele.Key, ele.Desc, ele.Value, css, options.url, Math.max(0, ele.Interval - Math.round((new Date() - new Date(ele.CreateTime.replace(/-/g, '/'))) / 1000)));
                        }).join('');
                        $sortable.append($.format('<li class="title">{0}-{1}</li>', options.item.Desc, options.item.Key));
                        $sortable.append(content);
                        $sortable.find('[role="tooltip"]').lgbTooltip();
                    }
                }
            }
        });
    }
    $('#refreshCache').click(function () { listCacheUrl(); }).trigger('click');
    $('#clearCache').click(function () { listCacheUrl({ clear: true }); });
    $sortable.on('click', '.btn', function () {
        $(this).lgbTooltip('destroy');
        listCache({ key: $(this).attr('data-key'), url: $(this).attr('data-url') });
        listCacheUrl();
    });

    $.bc({
        url: Dicts.css, swal: false,
        callback: function (result) {
            var html = result.map(function (ele, index) { return $.format('<li><a href="#" data-val="{1}">{0}</a></li>', ele.Name, ele.Code); }).join('');
            $('#cssContainer').append(html);
            $.bc({
                url: Dicts.css, swal: false, method: 'get',
                callback: function (result) {
                    if (result.length > 0)
                        $('.lgbDropdown').lgbDropdown('val', result);
                }
            });
        }
    });
})