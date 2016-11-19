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
        url: Profiles.url,
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
                            url: Profiles.url, data: { name: '网站标题', code: data.Title, category: Profiles.title }, title: Profiles.title,
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
                            url: Profiles.url, data: { name: '网站页脚', code: data.Footer, category: Profiles.title }, title: Profiles.title,
                            callback: function (result) {
                                if (result) $('#websiteFooter').text(data.Footer);
                            }
                        });
                    }
                }
            }]
        }
    });

    function listCache(options) {
        options = $.extend({ url: '../../CacheList.axd' }, options);
        $.bc({
            url: options.url,
            swal: false,
            callback: function (result) {
                if (result) {
                    result = $.parseJSON(result);
                    if ($.isArray(result)) {
                        var html = '<li class="{4}"><i class="fa fa-ellipsis-v"></i><div class="task-title"><span class="task-title-sp tooltips" data-placement="right" title="{1}">{2}</span><span class="badge badge-sm label-success">{0}</span><span class="task-value tooltips" data-placement="top" data-original-title="{3}">{3}</span><div class="pull-right hidden-phone"><button class="btn btn-danger btn-xs fa fa-trash-o" data-key="{1}"></button></div></div></li>';
                        var content = result.map(function (ele) {
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
                            return $.format(html, ele.Interval, ele.Key, ele.Desc, ele.Value, css);
                        }).join('');
                        $('#sortable').html(content);
                        $('.tooltips').tooltip();
                        $('#sortable .btn').click(function () {
                            listCache({ url: $.format('../../CacheList.axd?cacheKey={0}', $(this).attr('data-key')) });
                        });
                    }
                }
            }
        });
    }
    listCache();
    $('#refreshCache').click(function () { listCache(); });
    $('#clearCache').click(function () { listCache({ url: '../../CacheList.axd?clear=clear' }); });
})