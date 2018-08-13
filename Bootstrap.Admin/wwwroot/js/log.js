(function ($) {
    logPlugin = function (options) {
        this.options = $.extend({}, logPlugin.settings, options);
    }

    logPlugin.settings = {
        url: '../api/Logs',
        click: {
            query: 'btn_query',
            del: 'btn_delete',
            save: 'btnSubmit'
        }
    }

    logPlugin.prototype = {
        constructor: logPlugin,
        init: function () {
            var that = this;
            // handler click event
            for (name in this.options.click) {
                var cId = this.options.click[name];
                var source = $("#" + cId);
                source.data('click', name);
                source.click(function () {
                    var method = $(this).data('click');
                    logPlugin.prototype[method].call(that, this);
                });
            }
        },
        query: function (element) {
            //log(this.options.url, { crud: '查询' });
        },
        save: function () {
            log(this.options.url, { crud: '保存' });
        },
        del: function () {
            log(this.options.url, { crud: '删除' });
        }
    }

    logPlugin.init = function (options) {
        var log = new logPlugin(options);
        log.init();
    }

    var log = function (url, data) {
        $.extend(data, { requestUrl: window.location.pathname });
        $.post({
            url: url,
            data: JSON.stringify(data),
            contentType: 'application/json',
            dataType: 'json'
        });
    }
})(jQuery);

$(function () {
    logPlugin.init();
});