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
            },
            '#btnReset': function () {
                this.log({ crud: '重置密码' });
            },
            '#btnSaveDisplayName': function () {
                this.log({ crud: '设置显示名称' });
            },
            '#btnSavePassword': function () {
                this.log({ crud: '修改密码' });
            },
            '#btnSaveApp': function () {
                this.log({ crud: '设置默认应用' });
            },
            '#btnSaveCss': function () {
                this.log({ crud: '设置个人样式' });
            },
            'a.btn.fileinput-upload-button': function () {
                this.log({ crud: '设置头像' });
            },
            'button.kv-file-remove': function () {
                this.log({ crud: '删除头像' });
            },
            'button[data-method="title"]': function () {
                this.log({ crud: '保存网站标题' });
            },
            'button[data-method="footer"]': function () {
                this.log({ crud: '保存网站页脚' });
            },
            'button[data-method="css"]': function () {
                this.log({ crud: '设置网站样式' });
            },
            'button[data-method="UISettings"]': function () {
                this.log({ crud: '保存网站设置' });
            },
            'button[data-method="LoginSettings"]': function () {
                this.log({ crud: '保存登录设置' });
            }
        }
    };

    logPlugin.prototype = {
        constructor: logPlugin,
        log: function (data) {
            var bcData = $.logData.shift();
            if (bcData !== undefined) $.extend(data, { requestData: JSON.stringify(bcData) });
            $.extend(data, { requestUrl: window.location.pathname });
            $.post({
                url: $.formatUrl(this.options.url),
                data: JSON.stringify(data),
                contentType: 'application/json',
                dataType: 'json'
            });
        }
    };

    $.extend({
        logPlugin: function (options) {
            if (!window.logPlugin) window.logPlugin = new logPlugin(options);
            return window.logPlugin;
        }
    });
    $.logData = [];
    $.logData.log = function () {
        $.logPlugin().log({ crud: '删除数据' });
    };
})(jQuery);

$(function () {
    $.logPlugin();
});