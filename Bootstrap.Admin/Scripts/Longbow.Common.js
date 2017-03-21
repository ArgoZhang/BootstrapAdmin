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
            if (this == null) return "";
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
                "h+": this.getHours() % 12 == 0 ? 12 : this.getHours() % 12,
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
                format = format.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "星期" : "周") : "") + week[this.getDay()]);

            for (var k in o)
                if (new RegExp("(" + k + ")").test(format))
                    format = format.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            return format;
        };
    }

    // 扩展format
    $.extend({
        "format": function (source, params) {
            if (params === undefined) {
                return source;
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
        }
    });

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
                gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') == -1, //火狐内核
                mobile: !!u.match(/AppleWebKit.*Mobile.*/), //是否为移动终端
                ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios终端
                android: u.indexOf('Android') > -1 || u.indexOf('Linux') > -1, //android终端或uc浏览器
                iPhone: u.indexOf('iPhone') > -1, //是否为iPhone或者QQHD浏览器
                iPod: u.indexOf('iPod') > -1, //是否为iPod或者QQHD浏览器
                iPad: u.indexOf('iPad') > -1, //是否iPad
                webApp: u.indexOf('Safari') == -1 //是否web应该程序，没有头部与底部
            };
        }(),
        language: (navigator.browserLanguage || navigator.language).toLowerCase()
    }

    $.extend({
        bc: function (options, callback) {
            var data = $.extend({
                remote: true,
                Id: "",
                url: this.url,
                data: {},
                method: "POST",
                htmlTemplate: '<div class="form-group checkbox col-md-3 col-sm-4 col-xs-6"><label role="tooltip" title="{3}"><input type="checkbox" value="{0}" {2}/>{1}</label></div>',
                title: this.title,
                swal: true,
                modal: null,
                callback: null
            }, options);

            if (!data.url || data.url == "") {
                swal('参数错误', '未设置请求地址Url', 'error');
                return;
            }

            if (data.remote && data.url) {
                $.ajax({
                    url: data.url + data.Id,
                    data: data.data,
                    type: data.method,
                    async: true,
                    success: function (result) {
                        success(result);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        if ($.isFunction(data.callback)) data.callback(false);
                    }
                });
            }
            function success(result) {
                if ($.isFunction(data.callback)) {
                    data.callback(result);
                }
                if (data.modal !== null) {
                    $("#" + data.modal).modal('hide');
                }
                if (data.swal) {
                    if (result) { swal("成功", data.title, "success"); }
                    else { swal("失败", data.title, "error"); }
                }
            }
        }
    });

    // Roles
    Role = {
        url: '../api/Roles/',
        title: "授权角色"
    };

    // Users
    User = {
        url: '../api/Users/',
        title: "授权用户"
    };

    // Groups
    Group = {
        url: '../api/Groups/',
        title: "授权部门"
    };

    // Menus
    Menu = {
        url: '../api/Menus/',
        iconView: '../Admin/IconView',
        title: "授权菜单"
    };

    // Exceptions
    Exceptions = {
        url: '../api/Exceptions/',
        title: "程序异常日志"
    };

    // Dicts
    Dicts = {
        url: '../api/Dicts/'
    };

    // Infos
    Infos = {
        url: '../api/Infos/'
    }

    // Profiles
    Profiles = {
        url: '../api/Profiles/',
        title: '网站设置'
    }

    // Messages
    Messages = {
        url: '../api/Messages/'
    }

    // Tasks
    Tasks = {
        url: '../api/Tasks/'
    }

    // Notifications
    Notifications = {
        url: '../api/Notifications/'
    }

    $.fn.extend({
        adjustDialog: function () {
            var $modal_dialog = this;
            var m_top = Math.max(0, ($(window).height() - $modal_dialog.height()) / 2);
            $modal_dialog.css({ 'margin': m_top + 'px auto' });
            return this;
        },
        autoCenter: function () {
            var that = this;
            var getHeight = function () {
                return ($(window).height() - $(that).outerHeight()) / 2 + $(document).scrollTop();
            }
            $(window).resize(function () {
                $(that).css({
                    marginTop: getHeight()
                });
            });
            that.animate({ marginTop: "+=" + getHeight() });
        },
        lgbTooltip: function (option) {
            if (option == undefined) option = { container: 'body', delay: { "show": 500, "hide": 100 } };
            else if (typeof option == "object") option = $.extend({ container: 'body', delay: { "show": 500, "hide": 100 } }, option);
            $(this).tooltip(option);
            if (option == 'destroy') $(this).removeAttr('data-original-title');
            return this;
        },
        autoValidate: function (rules, messages, handler) {
            var parent = 'body';
            var $wrapper = $('#dialogNew');
            if ($wrapper.length == 1) parent = '#dialogNew';
            // validate
            var $this = $(this);
            if (messages && $.isArray(messages.button)) {
                handler = messages;
                messages = {};
            }
            else {
                messages = $.extend({}, messages);
            }
            $this.validate({
                validClass: "has-success",
                errorClass: "has-error",
                ignore: ".ignore",
                rules: $.extend({}, rules),
                messages: $.extend({}, messages),
                highlight: function (element, errorClass, validClass) {
                    $(element).parents('.form-group').addClass(errorClass).removeClass(validClass);
                },
                unhighlight: function (element, errorClass, validClass) {
                    $(element).lgbTooltip('destroy').parents('.form-group').removeClass(errorClass).addClass(validClass);
                },
                errorPlacement: function (label, element) {
                    var $ele = $(element);
                    if (!$ele.attr('data-original-title')) $ele.lgbTooltip({ container: parent });
                    $ele.attr('data-original-title', $(label).text());
                    $ele.lgbTooltip('show');
                    $('#' + $ele.attr('aria-describedby')).addClass(this.settings.errorClass);
                }
            });
            if (handler && $.isArray(handler.button)) {
                $.each(handler.button, function (index, btn) {
                    $('#' + btn).on('click', function () {
                        $(this).attr('data-valid', $this.valid());
                    });
                });
            }
        },
        smartTable: function (options) {
            var settings = $.extend({
                method: 'get',                      //请求方式（*）
                toolbar: '#toolbar',                //工具按钮用哪个容器
                striped: true,                      //是否显示行间隔色
                cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
                pagination: true,                   //是否显示分页（*）
                sortable: true,                     //是否启用排序
                sortOrder: "desc",                   //排序方式
                sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
                pageNumber: 1,                      //初始化加载第一页，默认第一页
                pageSize: 10,                       //每页的记录行数（*）
                pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
                search: false,                      //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
                strictSearch: false,
                showColumns: true,                  //是否显示所有的列
                showRefresh: true,                  //是否显示刷新按钮
                minimumCountColumns: 2,             //最少允许的列数
                clickToSelect: false,               //是否启用点击选中行
                //height: 500,                      //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
                idField: "Id",
                uniqueId: "Id",                     //每一行的唯一标识，一般为主键列
                showToggle: true,                   //是否显示详细视图和列表视图的切换按钮
                cardView: false,                    //是否显示详细视图
                detailView: false,                  //是否显示父子表
                clickToSelect: false
            }, options);
            $(this).bootstrapTable(settings);
            $('div.toolbar').on('click', 'a', function (e) {
                e.preventDefault();
                var ctl = $('#' + $(this).attr('id').replace('tb_', 'btn_'));
                ctl.trigger("click");
            }).insertBefore($('div.bootstrap-table > div.fixed-table-toolbar > div.bs-bars'));
            $(settings.toolbar).removeClass('hidden');
        },
        lgbDropdown: function (options) {
            var $this = $(this);
            var op = typeof options == 'object' && options;
            if (/val/.test(options)) {
                if (arguments.length == 1)
                    return $this.first().children('a').val();
                else {
                    $this.first().children(':first').children(':first').text($this.find('[data-val="' + arguments[1] + '"]').text())
                }
            }
            else {
                $this.each(function () {
                    $(this).on('click', '.dropdown-menu a', { $parent: $(this) }, function (event) {
                        event.preventDefault();
                        var $op = $(this);
                        event.data.$parent.children('a').val($op.attr('data-val')).children(':first').text($op.text());
                    });
                });
            }
        }
    });

    //fix bug
    $.fn.modal.Constructor.prototype.adjustDialog = function () {
        var modalIsOverflowing = this.$element[0].scrollHeight > document.documentElement.clientHeight

        this.$element.css({
            paddingLeft: !this.bodyIsOverflowing && modalIsOverflowing ? this.scrollbarWidth : '',
            paddingRight: this.bodyIsOverflowing && !modalIsOverflowing ? this.scrollbarWidth : ''
        })

        // added by Argo
        var $modal_dialog = $(this.$element[0]).find('.modal-dialog');
        $modal_dialog.adjustDialog();
    }
})(jQuery);

$(function () {
    // loading customer css
    $.bc({
        Id: 1, url: Dicts.url, data: { type: 'activeCss' }, swal: false,
        callback: function (result) {
            if (result.length > 0)
                $('head').append($.format('<link href="../Content/{0}" rel="stylesheet" type="text/css" />', result[0].Code));
        }
    });
});