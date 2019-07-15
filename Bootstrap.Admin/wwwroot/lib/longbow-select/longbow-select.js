(function ($) {
    'use strict';

    var lgbSelect = function (element, options) {
        this.$element = $(element);
        this.options = $.extend({}, lgbSelect.DEFAULTS, options);
        this.init();
    };

    lgbSelect.VERSION = '1.0';
    lgbSelect.Author = 'argo@163.com';
    lgbSelect.DataKey = "lgb.select";
    lgbSelect.Template = '<div class="form-select">';
    lgbSelect.Template += '<input type="text" readonly="readonly" class="form-control form-select-input" />';
    lgbSelect.Template += '<span class="form-select-append">';
    lgbSelect.Template += '    <i class="fa fa-angle-up"></i>';
    lgbSelect.Template += '</span>';
    lgbSelect.Template += '<div class="dropdown-menu-arrow"></div>';
    lgbSelect.Template += '<div class="dropdown-menu"></div>';
    lgbSelect.Template += '</div>';
    lgbSelect.DEFAULTS = {
        placeholder: "请选择 ...",
        borderClass: null
    };
    lgbSelect.AllowMethods = /disabled|enable|val|reset|get/;

    function Plugin(option) {
        var params = $.makeArray(arguments).slice(1);
        return this.each(function () {
            var $this = $(this);
            var data = $this.data(lgbSelect.DataKey);
            var options = typeof option === 'object' && option;

            if (!data) $this.data(lgbSelect.DataKey, data = new lgbSelect(this, options));
            if (!lgbSelect.AllowMethods.test(option)) return;
            if (typeof option === 'string') {
                data[option].apply(data, params);
            }
        });
    }

    $.fn.lgbSelect = Plugin;
    $.fn.lgbSelect.Constructor = lgbSelect;

    var _proto = lgbSelect.prototype;
    _proto.init = function () {
        var getUID = function (prefix) {
            if (!prefix) prefix = 'lgb';
            do prefix += ~~(Math.random() * 1000000);
            while (document.getElementById(prefix));
            return prefix;
        };

        var that = this;

        // 原有控件
        this.$element.addClass('d-none');

        // 新控件 <div class="form-select">
        this.$ctl = $(lgbSelect.Template).insertBefore(this.$element);

        // 下拉组合框
        this.$input = this.$ctl.find('.form-select-input');
        this.$menus = this.$ctl.find('.dropdown-menu');

        // init for
        var $for = this.$element.parent().find('[for="' + this.$element.attr('id') + '"]');
        if ($for.length > 0) {
            var id = getUID();
            this.$input.attr('id', id);
            $for.attr('for', id);
        }

        if (this.options.borderClass) {
            this.$input.addClass(this.options.borderClass);
        }
        this.$input.attr('placeholder', this.options.placeholder);

        // init dropdown-menu data
        var data = this.$element.find('option').map(function () {
            return { value: this.value, text: this.text, selected: this.selected }
        });

        // bind attribute
        ["data-valid", "data-required-msg"].forEach(function (v, index) {
            if (that.$element.attr(v) !== undefined) {
                that.$input.attr(v, that.$element.attr(v));
            }
        });

        // save ori attrs
        var attrs = [];
        ["id", "data-default-val"].forEach(function (v, index) {
            attrs.push({ name: v, value: that.$element.attr(v) });
        });

        // replace element select -> input hidden
        this.$element.remove();
        this.$element = $('<input type="hidden" data-toggle="lgbSelect" />').val(that.val()).insertAfter(this.$ctl);
        this.$element.data(lgbSelect.DataKey, this);

        // bind ori atts
        attrs.forEach(function (v) {
            that.$element.attr(v.name, v.value);
        });

        //  bind event
        this.$ctl.on('click', '.form-select-input', function (e) {
            e.preventDefault();

            that.$ctl.toggleClass('open');
            // calc width
            that.$ctl.find('.dropdown-menu').outerWidth(that.$ctl.outerWidth());
        });

        this.$ctl.on('click', 'a.dropdown-item', function (e) {
            e.preventDefault();

            var $this = $(this);
            $this.parent().children().removeClass('active');
            that.val($this.attr('data-val'), true);
        });

        $(document).on('click', function (e) {
            if (that.$input[0] !== e.target)
                that.closeMenu();
        });

        // init dropdown-menu
        this.reset(data);
    };

    _proto.closeMenu = function () {
        this.$ctl.removeClass('open');
    };

    _proto.disabled = function () {
        this.$ctl.addClass('is-disabled');
        this.$input.attr('disabled', 'disabled');
    };

    _proto.enable = function () {
        this.$ctl.removeClass('is-disabled');
        this.$input.removeAttr('disabled');
    };

    _proto.reset = function (value) {
        var that = this;

        // keep old value
        var oldValue = this.$input.val();

        // warning: must use attr('value') method instead of val(). otherwise the others input html element will filled by first element value.
        // see https://gitee.com/LongbowEnterprise/longbow-select/issues/IZ3BR?from=project-issue
        this.$input.attr('value', '');
        this.$menus.html('');
        $.each(value, function (index) {
            var $item = $('<a class="dropdown-item" href="#" data-val="' + this.value + '">' + this.text + '</a>');
            that.$menus.append($item);
            if (this.selected === true || this.value === oldValue || index === 0 || this.value === that.$element.attr('data-default-val')) {
                that.$input.attr('value', this.text);
                that.$element.val(this.value).attr('data-text', this.text);
                $item.addClass('active');
            }
        });

        this.source = value;
    };

    _proto.get = function (callback) {
        if ($.isFunction(callback)) {
            callback.call(this.$element, this.source);
        }
    };

    _proto.val = function (value, valid) {
        if (value !== undefined) {
            var text = this.$menus.find('a.dropdown-item[data-val="' + value + '"]').text();
            this.$input.val(text);
            this.$element.val(value).attr('data-text', text);
            this.$menus.find('.dropdown-item').removeClass('active');
            this.$menus.find('.dropdown-item[data-val="' + value + '"]').addClass('active');

            // trigger changed.lgbselect
            this.$element.trigger('changed.lgbSelect');

            // trigger lgbValidate
            if (valid && this.$input.attr('data-valid') === 'true') this.$input.trigger('input.lgb.validate');
        }
        else {
            return this.$element.val();
        }
    };

    $(function () {
        $('select[data-toggle="lgbSelect"]').lgbSelect();
    });
})(jQuery);
