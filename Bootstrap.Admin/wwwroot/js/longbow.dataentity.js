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
                    var val = value[name];
                    if ((typeof val == "string" && val == "") || val == undefined) val = ctl.attr('data-default-val');
                    ctl.children(':first').text(ctl.next().find('[data-val="' + val + '"]').text());
                }
                else if (ctl.attr('data-toggle') == 'toggle') {
                    ctl.bootstrapToggle(value[name] ? 'on' : 'off');
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
                else if (ctl.attr('data-toggle') == 'toggle') {
                    ctl.bootstrapToggle(dv == "true" ? 'on' : 'off');
                }
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