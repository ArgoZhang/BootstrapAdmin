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
                if (ctl.hasClass('selectpicker')) ctl.selectpicker('val', value[name]);
                else ctl.val(value[name]);
            }
        },
        reset: function () {
            for (name in this.options.map) {
                var ctl = $("#" + this.options.map[name]);
                if (ctl.hasClass('selectpicker')) ctl.selectpicker('val', "");
                else ctl.val("");
            }
        },
        get: function () {
            var target = {};
            for (name in this.options.map) {
                var ctl = $("#" + this.options.map[name]);
                if (ctl.hasClass('selectpicker')) {
                    target[name] = ctl.selectpicker('val');
                    target[name + 'Name'] = ctl.parentsUntil('bootstrap-select').children('button[data-id="' + this.options.map[name] + '"]').attr('title');
                }
                else target[name] = ctl.val();
            }
            return target;
        }
    }
}(jQuery));