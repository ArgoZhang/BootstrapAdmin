(function ($) {
    var logPlugin = function (options) {
        this.options = $.extend({}, logPlugin.settings, options);

        var that = this;
        for (var name in this.options.click) {
            $(name).on('click', { handler: this.options.click[name] }, function (e) {
                e.data.handler.call(that);
            });
        }
    };

    logPlugin.settings = {
        url: 'api/Logs',
        click: {
            '#btn_delete': function () {
                this.log({ crud: '删除' });
            },
            '#btnSubmit': function () {
                this.log({ crud: '保存' });
            },
            '#btnSubmitRole': function () {
                this.log({ crud: '分配角色' });
            },
            '#btnSubmitGroup': function () {
                this.log({ crud: '分配部门' });
            },
            '#btnSubmitUser': function () {
                this.log({ crud: '分配用户' });
            },
            '#btnSubmitMenu': function () {
                this.log({ crud: '分配菜单' });
            }
        }
    };

    logPlugin.prototype = {
        constructor: logPlugin,
        log: function (data) {
            $.extend(data, { requestUrl: window.location.pathname });
            $.post({
                url: $.formatUrl(this.options.url),
                data: JSON.stringify(data),
                contentType: 'application/json',
                dataType: 'json'
            });
        }
    };

    $.extend({ logPlugin: function (options) { return new logPlugin(options); } });
})(jQuery);

$(function () {
    $.logPlugin();
});