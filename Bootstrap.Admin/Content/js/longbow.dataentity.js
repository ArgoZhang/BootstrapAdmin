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
                var ctl = $("#" + this.options.map[name]);
                ctl.val(value[name]);
                if (ctl.attr('data-toggle') == "dropdown") {
                    ctl.children(':first').text(ctl.next().find('[data-val="' + value[name] + '"]').text());
                }
            }
        },
        reset: function () {
            for (name in this.options.map) {
                var ctl = $("#" + this.options.map[name]);
                var dv = ctl.attr("data-default-val");
                if (dv === undefined) dv = "";
                ctl.val(dv);
                if (ctl.attr('data-toggle') == "dropdown") {
                    ctl.children(':first').text(ctl.next().find('[data-val="' + dv + '"]').text());
                }
            }
        },
        get: function () {
            var target = {};
            for (name in this.options.map) {
                var ctl = $("#" + this.options.map[name]);
                var dv = ctl.attr('data-default-val');
                if (dv !== undefined && ctl.val().trim() === "") target[name] = dv;
                else target[name] = ctl.val();
            }
            return target;
        }
    }
}(jQuery));