(function ($) {
    DataEntity = function (options) {
        this.options = $.extend({ map: {} }, options);
    };

    DataEntity.VERSION = "1.0";
    DataEntity.Author = "Argo Zhang";
    DataEntity.Email = "argo@163.com";

    DataEntity.prototype = {
        load: function (value) {
            for (name in this.options.map) {
                $("#" + this.options.map[name]).val(value[name]);
            }
        },
        reset: function () {
            for (name in this.options.map) {
                $("#" + this.options.map[name]).val("");
            }
        },
        get: function () {
            var target = {};
            for (name in this.options.map) {
                target[name] = $("#" + this.options.map[name]).val();
            }
            return target;
        }
    }
}(jQuery));