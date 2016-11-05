(function ($) {
    LogPlugin = function (options) {
        var that = this;
        this.options = $.extend({}, LogPlugin.settings, options);

        // handler click event
        for (name in this.options.click) {
            var cId = this.options.click[name];
            var source = $("#" + cId);
            source.data('click', name);
            source.click(function () {
                var method = $(this).data('click');
                LogPlugin.prototype[method].call(that, this);
            });
        }

    }

    LogPlugin.settings = {
        url: '../api/Logs',
        click: {
            query: 'btn_query',
            del: 'btn_delete',
            save: 'btnSubmit'
        }
    }

    LogPlugin.prototype = {
        constructor: LogPlugin,
        query: function (element) {
            log(this.options.url, { crud: 'Query' });
        }
    }

    var log = function (url, data) {
        $.post(url, data);
    }
})(jQuery);