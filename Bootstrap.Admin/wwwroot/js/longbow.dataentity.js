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
                if (ctl.attr('data-toggle') == "dropdown") {
                    ctl.val(value[name]).dropdown('val');
                }
                else if (ctl.attr('data-toggle') == 'toggle') {
                    ctl.bootstrapToggle(value[name] ? 'on' : 'off');
                }
                else ctl.val(value[name]);
            }
        },
        reset: function () {
            for (name in this.options.map) {
                var ctl = $("#" + this.options.map[name]);
                var dv = ctl.attr("data-default-val");
                if (dv === undefined) dv = "";
                if (ctl.attr('data-toggle') == "dropdown") {
                    ctl.val(dv).dropdown('val');
                }
                else if (ctl.attr('data-toggle') == 'toggle') {
                    ctl.bootstrapToggle(dv == "true" ? 'on' : 'off');
                }
                else ctl.val(dv);
            }
        },
        get: function () {
            var target = {};
            for (name in this.options.map) {
                var ctl = $("#" + this.options.map[name]);
                var dv = ctl.attr('data-default-val');
                if (ctl.attr('data-toggle') == 'toggle') {
                    target[name] = ctl.prop('checked');
                    continue;
                }
                else if (dv != undefined && ctl.val() == "") target[name] = dv;
                else target[name] = ctl.val();
                if (target[name] == "true" || target[name] == "True") target[name] = true;
                if (target[name] == "false" || target[name] == "False") target[name] = false;
            }
            return target;
        }
    }
}(jQuery));