(function ($) {
    // 增加Array扩展
    if (!$.isFunction(Array.prototype.filter)) {
        Array.prototype.filter = function (callback, thisObject) {
            if ($.isFunction(callback)) {
                var res = new Array();
                for (var i = 0; i < this.length; i++) {
                    callback.call(thisObject, this[i], i, this) && res.push(this[i]);
                }
                return res;
            }
        };
    }

    // 增加String扩展
    if (!$.isFunction(String.prototype.trim)) {
        String.prototype.trim = function () {
            if (this === null) return "";
            var trimLeft = /^\s+/, trimRight = /\s+$/;
            return this.replace(trimLeft, "").replace(trimRight, "");
        };
    }

    // 扩展Date
    if (!$.isFunction(Date.prototype.format)) {
        Date.prototype.format = function (format) {
            var o = {
                "M+": this.getMonth() + 1,
                "d+": this.getDate(),
                "h+": this.getHours() % 12 === 0 ? 12 : this.getHours() % 12,
                "H+": this.getHours(),
                "m+": this.getMinutes(),
                "s+": this.getSeconds(),
                "q+": Math.floor((this.getMonth() + 3) / 3),
                "S": this.getMilliseconds()
            };
            var week = {
                0: "日",
                1: "一",
                2: "二",
                3: "三",
                4: "四",
                5: "五",
                6: "六"
            };

            if (/(y+)/.test(format))
                format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));

            if (/(E+)/.test(format))
                format = format.replace(RegExp.$1, (RegExp.$1.length > 1 ? RegExp.$1.length > 2 ? "星期" : "周" : "") + week[this.getDay()]);

            for (var k in o)
                if (new RegExp("(" + k + ")").test(format))
                    format = format.replace(RegExp.$1, RegExp.$1.length === 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
            return format;
        };
    }

    // enhance window.console.log
    if (!window.console) {
        window.console = {
            log: function () {

            }
        };
    }
    window.console = window.console || {};
    console.log || (console.log = opera.postError);

    // client
    jQuery.browser = {
        versions: function () {
            var u = navigator.userAgent;
            return {         //移动终端浏览器版本信息
                trident: u.indexOf('Trident') > -1, //IE内核
                presto: u.indexOf('Presto') > -1, //opera内核
                webKit: u.indexOf('AppleWebKit') > -1, //苹果、谷歌内核
                gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') === -1, //火狐内核
                mobile: !!u.match(/AppleWebKit.*Mobile.*/), //是否为移动终端
                ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios终端
                android: u.indexOf('Android') > -1 || u.indexOf('Linux') > -1, //android终端或uc浏览器
                iPhone: u.indexOf('iPhone') > -1, //是否为iPhone或者QQHD浏览器
                iPod: u.indexOf('iPod') > -1, //是否为iPod或者QQHD浏览器
                iPad: u.indexOf('iPad') > -1, //是否iPad
                webApp: u.indexOf('Safari') === -1 //是否web应该程序，没有头部与底部
            };
        }(),
        language: (navigator.browserLanguage || navigator.language).toLowerCase()
    };

    $.extend({
        "format": function (source, params) {
            if (params === undefined || params === null) {
                return null;
            }
            if (arguments.length > 2 && params.constructor !== Array) {
                params = $.makeArray(arguments).slice(1);
            }
            if (params.constructor !== Array) {
                params = [params];
            }
            $.each(params, function (i, n) {
                source = source.replace(new RegExp("\\{" + i + "\\}", "g"), function () {
                    return n;
                });
            });
            return source;
        },
        copyText: function (ele) {
            if (typeof ele !== "string") return false;
            var input = document.createElement('input');
            input.setAttribute('type', 'text');
            input.setAttribute('value', ele);
            document.body.appendChild(input);
            input.select();
            var ret = document.execCommand('copy');
            document.body.removeChild(input);
            return ret;
        },
        fullScreenStatus: function fullScreenStatus(value) {
            if (value !== undefined) window.fullscreen = value;
            return document.fullscreen ||
                document.mozFullScreen ||
                document.webkitIsFullScreen || window.fullscreen ||
                false;
        },
        bc: function (options) {
            options = $.extend({
                id: "",
                url: "",
                data: {},
                title: "",
                modal: false,
                loading: false,
                loadingTimeout: 10000,
                callback: false,
                cors: false,
                contentType: 'application/json',
                dataType: 'json',
                method: 'get',
                autoFooter: false
            }, options);

            if (!options.url || options.url === "") {
                toastr.error('未设置请求地址Url', '参数错误');
                return;
            }

            var loadFlag = "loading";
            var loadingHandler = null;
            if (options.loading && options.modal) {
                var $modal = $(options.modal);
                $modal.on('shown.bs.modal', function () {
                    var $this = $(this);
                    if (loadingHandler !== null) {
                        window.clearTimeout(loadingHandler);
                        loadingHandler = null;
                    }
                    if ($this.hasClass(loadFlag)) return;
                    $this.modal('hide');
                });
                loadingHandler = window.setTimeout(function () { $(options.modal).addClass(loadFlag).modal('show'); }, 300);
                var loadTimeoutHandler = setTimeout(function () {
                    $(options.modal).find('.close').removeClass('d-none');
                    clearTimeout(loadTimeoutHandler);
                }, options.loadingTimeout);
            }

            var data = options.method === 'get' ? options.data : JSON.stringify(options.data);
            var url = options.id !== '' ? options.url + '/' + options.id : options.url;
            if (options.query) {
                var qs = [];
                for (var key in options.query) {
                    qs.push($.format("{0}={1}", key, options.query[key]));
                }
                url = url + "?" + qs.join('&');
            }

            function success(result) {
                if (options.modal && (result || options.loading)) {
                    if (loadingHandler !== null) {
                        // cancel show modal event
                        window.clearTimeout(loadingHandler);
                        loadingHandler = null;
                    }
                    else $(options.modal).removeClass(loadFlag).modal('hide');
                }
                if (options.title) toastr[result ? 'success' : 'error'](options.title + (result ? "成功" : "失败"));
                if ($.isFunction(options.callback)) {
                    options.callback.call(options, result);
                }
                if (options.autoFooter === true) {
                    $.footer();
                }
            }

            var ajaxSettings = {
                url: $.formatUrl(url),
                data: data,
                method: options.method,
                contentType: options.contentType,
                dataType: options.dataType,
                crossDomain: false,
                success: function (result) {
                    success(result);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (window.toastr) {
                        if (errorThrown === '') errorThrown = url;
                        toastr.error(XMLHttpRequest.status === 500 ? '后台应用程序错误' : errorThrown, '程序错误');
                    }
                    success(false);
                }
            };
            if (options.cors) $.extend(ajaxSettings, {
                xhrFields: { withCredentials: true },
                crossDomain: true
            });
            if ($.isArray($.logData) && !$.isEmptyObject(options.data)) $.logData.push({ url: url, data: options.method === 'delete' ? options.logData : options.data });
            if (options.method === 'delete' && $.logData && $.isFunction($.logData.log)) $.logData.log();
            $.ajax(ajaxSettings);
        },
        lgbSwal: function (options) {
            if ($.isFunction(swal)) {
                swal($.extend({ showConfirmButton: false, showCancelButton: false, timer: 1000, title: '未设置', type: "success" }, options));
            }
            else {
                window.log('缺少 swal 脚本引用');
            }
        },
        getUID: function (prefix) {
            if (!prefix) prefix = 'lgb';
            do prefix += ~~(Math.random() * 1000000);
            while (document.getElementById(prefix));
            return prefix;
        },
        footer: function (options) {
            var op = $.extend({ header: "header", content: "body > section:first", ele: 'footer' }, options);
            var $ele = $(op.ele);

            // 增加 1px 修复 IE11 下由于小数点导致页脚消失bug
            return $(op.header).outerHeight() + $(op.content).outerHeight() + $ele.outerHeight() > $(window).height() + 1 ? $ele.removeClass('position-fixed') : $ele.addClass('position-fixed');
        },
        formatUrl: function (url) {
            if (!url) return url;
            if (url.substr(0, 4) === "http") return url;
            var base = $('#pathBase').attr('href');
            return base + url;
        },
        safeHtml: function (text) {
            return (text && typeof text === "string") ? $('<div>').text(text).html() : text;
        },
        syntaxHighlight: function (json) {
            if (typeof (json) === 'string') {
                json = JSON.parse(json);
            }
            json = JSON.stringify(json, undefined, 2);
            json = json.replace(/&/g, '&').replace(/</g, '<').replace(/>/g, '>');
            return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g,
                function (match) {
                    var cls = 'number';
                    if (/^"/.test(match)) {
                        if (/:$/.test(match)) {
                            cls = 'key';
                        } else {
                            cls = 'string';
                        }
                    } else if (/true|false/.test(match)) {
                        cls = 'boolean';
                    } else if (/null/.test(match)) {
                        cls = 'null';
                    }
                    return '<span class="' + cls + '">' + match + '</span>';
                }
            );
        }
    });

    window.lgbSwal = $.lgbSwal;

    $.fn.extend({
        autoCenter: function (options) {
            options = $.extend({ top: 0 }, options);
            var that = this;
            var defaultVal = parseFloat(that.css('marginTop').replace('px', ''));
            var getHeight = function () {
                return Math.max(defaultVal, ($(window).height() - options.top - that.outerHeight()) / 2 + $(document).scrollTop());
            };
            $(window).resize(function () {
                that.css({ marginTop: getHeight() });
            });
            that.css({ marginTop: getHeight(), transition: "all .5s linear" });
            return this;
        },
        lgbTable: function (options) {
            var bsa = new DataTable($.extend(options.dataBinder, { url: options.url }));

            var settings = $.extend(true, {
                url: options.url,
                checkbox: true,
                editButtons: {
                    id: "#tableButtons",
                    events: {},
                    formatter: false
                },
                editTitle: "操作",
                editField: "Id",
                queryButton: false
            }, options.smartTable);

            var $editButtons = $(settings.editButtons.id);
            if ($editButtons.find('button').length > 0) settings.columns.push({
                title: settings.editTitle,
                field: settings.editField,
                events: $.extend({}, bsa.idEvents(), settings.editButtons.events),
                formatter: function (value, row, index) {
                    if ($.isFunction(settings.editButtons.formatter)) {
                        return settings.editButtons.formatter.call($editButtons, value, row, index);
                    }
                    return $editButtons.html();
                }
            });
            if (settings.checkbox) settings.columns.unshift({ checkbox: true });
            return this.smartTable(settings);
        },
        smartTable: function (options) {
            var settings = $.extend({
                toolbar: '#toolbar',                //工具按钮用哪个容器
                cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
                pagination: true,                   //是否显示分页（*）
                sortOrder: "asc",                   //排序方式
                sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
                pageNumber: 1,                      //初始化加载第一页，默认第一页
                pageSize: 20,                       //每页的记录行数（*）
                pageList: [20, 40, 80, 120],        //可供选择的每页的行数（*）
                showExport: true,
                exportTypes: ['csv', 'txt', 'excel'],
                showColumns: true,                  //是否显示所有的列
                showRefresh: true,                  //是否显示刷新按钮
                showToggle: true,                   //是否显示详细视图和列表视图的切换按钮
                cardView: $(window).width() < 768,                    //是否显示详细视图
                queryButton: '#btn_query',
                onLoadSuccess: function (data) {
                    $.footer();
                    if (data.IsSuccess === false) {
                        toastr.error(data.HttpResult.Message, data.HttpResult.Name);
                    }
                },
                onToggle: function () {
                    $.footer();
                }
            }, options);
            settings.url = $.formatUrl(settings.url);
            $.each(settings.columns, function (index, value) {
                if (value.checkbox) return;
                if (!$.isFunction(value.formatter)) {
                    value.formatter = function (value, row, index, field) {
                        return $.safeHtml(value);
                    }
                }
                else {
                    var formatter = value.formatter;
                    value.formatter = function (value, row, index, field) {
                        return formatter.call(this, $.safeHtml(value), row, index, field);
                    }
                }
            });
            this.bootstrapTable(settings);
            $('.bootstrap-table .fixed-table-toolbar .columns .export .dropdown-menu').addClass("dropdown-menu-right");
            var $gear = $(settings.toolbar).removeClass('d-none').find('.gear');
            if ($gear.find('.dropdown-menu > a').length === 0) $gear.addClass('d-none');
            $gear.on('click', 'a', function (e) {
                e.preventDefault();
                $('#' + $(this).attr('id').replace('tb_', 'btn_')).trigger("click");
            });
            if (settings.queryButton) {
                $(settings.queryButton).on('click', this, function (e) {
                    e.data.bootstrapTable('refresh');
                });
            }
            return this;
        },
        lgbPopover: function (options) {
            this.each(function (index, ele) {
                var $ele = $(ele);
                var data = $ele.data($.fn.popover.Constructor.DATA_KEY);
                if (data) {
                    $.extend(data.config, options);
                }
                else {
                    $ele.popover(options);
                }
            });
            return this;
        },
        lgbTooltip: function (options) {
            this.each(function (index, ele) {
                var $ele = $(ele);
                var data = $ele.data($.fn.tooltip.Constructor.DATA_KEY);
                if (data) {
                    $.extend(data.config, options);
                }
                else {
                    $ele.tooltip(options);
                }
            });
            return this;
        },
        lgbDatePicker: function (options) {
            if (!$.isFunction(this.datetimepicker)) return this;
            var option = $.extend({
                language: 'zh-CN',
                weekStart: 1,
                todayBtn: 1,
                autoclose: 1,
                todayHighlight: 1,
                startView: 2,
                minView: 2,
                forceParse: 0,
                format: 'yyyy-mm-dd',
                pickerPosition: 'bottom-left',
                fontAwesome: true
            }, options);
            this.datetimepicker(option);
            return this;
        },
        getTextByValue: function (value) {
            // 通过value获取select控件的text属性
            var text = "";
            if (typeof value !== "string") value = value.toString();
            if (this.attr('data-toggle') === 'lgbSelect') {
                if (value === this.val()) text = this.attr('data-text');
                else {
                    var data = [];
                    this.lgbSelect('get', function (source) { data = source; });
                    var find = data.filter(function (item, index) { return item.value === value; });
                    if (find.length === 1) text = find[0].text;
                }
            }
            else {
                text = this.children().filter(function () { return $(this).val() === value; }).text();
                if (text === "") text = value;
            }
            return text;
        },
        lgbInfo: function (option) {
            this.each(function () {
                var $element = $(this);
                $element.append($.format('<a href="#" tabindex="-1" role="button" data-toggle="popover"><i class="fa fa-question-circle"></i></a>'));
            });
            var container = this.attr('data-container') || '#dialogNew';
            this.find('[data-toggle="popover"]').popover($.extend({
                title: function () {
                    return $(this).parent().text();
                }, content: function () {
                    return $(this).parent().attr('data-content');
                }, trigger: 'focus', html: true, container: container, placement: function () {
                    return $(this.element).parent().attr('data-placement') || 'auto';
                }
            }, option));
            return this;
        },
        notifi: function (options) {
            var op = $.extend({ url: '', method: 'rev', invoke: false, callback: false }, options);
            var connection = new signalR.HubConnectionBuilder().withUrl($.formatUrl(op.url)).build();
            var that = this;
            connection.on(op.method, function () {
                if ($.isFunction(op.callback)) op.callback.apply(that, arguments);
            });
            connection.start().catch(function (err) {
                if ($.isFunction(op.callback)) op.callback.apply(that, arguments);
                return console.error(err.toString());
            }).then(function () {
                // 连接成功
                // invoke 为 调用服务端方法
                // invoke: function (connection) { return connection.invoke('RetrieveDashboard'); }
                if (!op.invoke) return;
                var executor = op.invoke(connection);
                if (typeof executor === "object" && $.isFunction(executor.then)) executor.then(function (result) { console.log(result); }).catch(function (err) { console.error(err.toString()); });
            });
            this.hub = connection;
            return this;
        }
    });

    //extend dropdown method
    $.extend($.fn.dropdown.Constructor.prototype, {
        val: function () {
            var $element = $(this._element);
            var $op = $(this._menu).find('[data-val="' + $element.val() + '"]:first');
            $element.text($op.text());
        },
        select: function () {
            var $element = $(this._element);
            $(this._menu).on('click', 'a', function (event) {
                event.preventDefault();
                var $op = $(this);
                $element.text($op.text()).val($op.attr('data-val'));
            });
        }
    });

    $(function () {
        // fix bug bootstrap-table 1.14.2 showToggle
        if ($.fn.bootstrapTable) {
            $.extend($.fn.bootstrapTable.defaults.icons, {
                refresh: 'fa-refresh'
            });
        }

        // extend bootstrap-toggle
        if ($.fn.bootstrapToggle) {
            var toggle = $.fn.bootstrapToggle.Constructor;
            var oldFunc = toggle.prototype.render;
            toggle.prototype.render = function () {
                var defaultVal = this.$element.attr('data-default-val') || '';
                if (defaultVal === '') this.$element.prop('checked', true);
                oldFunc.call(this);
            }
        }

        if (window.NProgress) {
            $(document).ajaxStart(function () {
                return NProgress.start();
            });

            $(document).ajaxComplete(function (e) {
                return NProgress.done();
            });
        }

        if (window.toastr) toastr.options = {
            "closeButton": true,
            "debug": false,
            "progressBar": true,
            "positionClass": "toast-bottom-right",
            "onclick": null,
            "showDuration": "600",
            "hideDuration": "2000",
            "timeOut": "4000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

        $('body > section .collapse').on('shown.bs.collapse', function () {
            $.footer().removeClass('d-none');
        }).on('body > section hidden.bs.collapse', function () {
            $.footer().removeClass('d-none');
        }).on('body > section hide.bs.collapse', function () {
            $('footer').addClass('d-none');
        }).on('body > section show.bs.collapse', function () {
            $('footer').addClass('d-none');
        });

        $(window).on('resize', function () {
            $.footer();
        });

        $("#gotoTop").on('click', function (e) {
            e.preventDefault();
            $('html, body, body > section:first').animate({
                scrollTop: 0
            }, 200);
        });

        $('[data-toggle="dropdown"].dropdown-select').dropdown('select');
        $('[data-toggle="tooltip"]').tooltip();
        $('[data-toggle="popover"]').popover();
        $('[data-toggle="lgbinfo"]').lgbInfo();
        $('.date').lgbDatePicker().on('show hide', function (e) {
            e.stopPropagation();
        });

        // 移动设备支持 bootstrap-toggle 扩展
        $('[data-toggle="toggle"]').on('touchend', function (e) {
            $(this).tigger('click.bs.toggle');
            e.preventDefault();
        });
    });
})(jQuery);