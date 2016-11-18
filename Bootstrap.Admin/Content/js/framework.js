(function ($) {
    BootstrapAdmin = function (options) {
        var that = this;
        options = options || {};
        options.click = $.extend({}, BootstrapAdmin.settings.click, options.click);
        this.options = $.extend({}, BootstrapAdmin.settings, options);

        this.dataEntity = options.dataEntity;
        if (!(this.dataEntity instanceof DataEntity) && window.console) {
            window.console.log('初始化参数中没有DataEntity实例');
        }

        // handler click event
        for (name in this.options.click) {
            var ele = this.options.click[name];
            var cId = ele;
            var event = null;
            if ($.isArray(ele)) {
                for (index in ele) {
                    if (ele[index].id === undefined) {
                        window.console.log('options.click.assign[{0}].{1}.id 未设置控件id', ele[index].id, name);
                        continue;
                    }
                    cId = ele[index]['id'];
                    event = ele[index]['click'];
                    handler(cId, event);
                }
            }
            else handler(cId, event);
        }

        // handler modal window show event
        if (this.options.modal && this.options.modal.constructor === String) {
            $('#' + this.options.modal).on('show.bs.modal', function (e) {
                if (that.options.validateForm && that.options.validateForm.constructor === String) {
                    var v = $('#' + that.options.validateForm);
                    var vf = v.validate();
                    vf.currentElements.each(function () { $(this).popover('destroy'); })
                    vf.resetForm();
                    v.find('div.form-group').removeClass("has-error has-success");
                }
            });
        }

        function handler(cid, event) {
            var source = $("#" + cId);
            source.data('click', name);
            if (event !== null) source.data('event', event);
            source.click(function () {
                var method = source.data('click');
                BootstrapAdmin.prototype[method].call(that, this, source.data('event'));
            });
        }
    };

    BootstrapAdmin.VERSION = "1.0";
    BootstrapAdmin.Author = "Argo Zhang";
    BootstrapAdmin.Email = "argo@163.com";

    BootstrapAdmin.settings = {
        url: undefined,
        bootstrapTable: 'table',
        validateForm: 'dataForm',
        modal: 'dialogNew',
        click: {
            query: 'btn_query',
            create: 'btn_add',
            edit: 'btn_edit',
            del: 'btn_delete',
            save: 'btnSubmit',
            assign: []
        }
    };

    BootstrapAdmin.idFormatter = function (value, row, index) {
        return "<a class='edit' href='javascript:void(0)'>" + value + "</a>";
    };

    BootstrapAdmin.prototype = {
        constructor: BootstrapAdmin,
        idEvents: function () {
            var op = {
                dataEntity: $.extend({}, this.options.dataEntity),
                table: this.options.bootstrapTable,
                modal: this.options.modal
            };
            return {
                'click .edit': function (e, value, row, index) {
                    op.dataEntity.load(row);
                    $(op.table).bootstrapTable('uncheckAll');
                    $(op.table).bootstrapTable('check', index);
                    $('#' + op.modal).modal("show");
                }
            }
        },

        query: function (e, callback) {
            if (this.options.bootstrapTable.constructor === String) $(this.options.bootstrapTable).bootstrapTable('refresh');
            handlerCallback.call(this, callback, e, { oper: 'query' });
        },

        create: function (e, callback) {
            if (this.dataEntity instanceof DataEntity) this.dataEntity.reset();
            if (this.options.modal.constructor === String) $('#' + this.options.modal).modal("show");
            if (this.options.bootstrapTable.constructor === String) $(this.options.bootstrapTable).bootstrapTable('uncheckAll');
            handlerCallback.call(this, callback, e, { oper: 'create' });
        },

        edit: function (e, callback) {
            var options = this.options;
            var data = {};
            if (options.bootstrapTable.constructor === String) {
                var arrselections = $(options.bootstrapTable).bootstrapTable('getSelections');
                if (arrselections.length == 0) {
                    swal('请选择要编辑的数据', "编辑操作", "warning");
                    return;
                }
                else if (arrselections.length > 1) {
                    swal('请选择一个要编辑的数据', "编辑操作", "warning");
                    return;
                }
                else {
                    data = arrselections[0];
                    if (this.dataEntity instanceof DataEntity) this.dataEntity.load(data);
                    if (options.modal.constructor === String) $('#' + options.modal).modal("show");
                }
            }
            handlerCallback.call(this, callback, e, { oper: 'edit', data: data });
        },

        del: function (e, callback) {
            var that = this;
            var options = this.options;
            if (options.bootstrapTable.constructor === String) {
                var arrselections = $(options.bootstrapTable).bootstrapTable('getSelections');
                if (arrselections.length == 0) {
                    swal('请选择要删除的数据', "删除操作", "warning");
                    return;
                }
                else {
                    swal({
                        title: "您确定要删除吗？",
                        text: "删除操作",
                        type: "warning",
                        showCancelButton: true,
                        closeOnConfirm: true,
                        confirmButtonText: "是的，我要删除",
                        confirmButtonColor: "#d9534f",
                        cancelButtonText: "取消"
                    }, function () {
                        var iDs = arrselections.map(function (element, index) { return element.ID }).join(",");
                        options.IDs = iDs;
                        $.bc({
                            url: options.url, data: { "": iDs }, method: 'DELETE', title: '删除数据',
                            callback: function (result) {
                                if ($.isPlainObject(result)) {
                                    var info = result.result ? "success" : "error";
                                    var msg = result.msg
                                    swal(msg, "删除数据", info);
                                    result = result.result;
                                    this.swal = false;
                                }
                                if (result) $(options.bootstrapTable).bootstrapTable('refresh');
                                handlerCallback.call(that, callback, e, { oper: 'del', success: result });
                            }
                        });
                    });
                }
            }
        },

        save: function (e, callback) {
            var that = this;
            var options = $.extend({ data: {} }, this.options);
            if (this.dataEntity instanceof DataEntity) options = $.extend(options, { data: this.dataEntity.get() });
            if (options.validateForm.constructor === String && !$("#" + options.validateForm).valid()) return;
            $.bc({
                url: options.url, data: options.data, title: "保存数据", modal: options.modal,
                callback: function (result) {
                    var finalData = null;
                    var index = 0;
                    if (result) {
                        if (options.bootstrapTable.constructor === String && options.data.ID.constructor === String) {
                            // 更新表格
                            if (options.data.ID > 0) {
                                var allTableData = $(options.bootstrapTable).bootstrapTable('getData');
                                for (index = 0; index < allTableData.length; index++) {
                                    finalData = allTableData[index];
                                    if (finalData.ID == options.data.ID) {
                                        $(options.bootstrapTable).bootstrapTable('updateRow', { index: index, row: $.extend(finalData, options.data) });
                                        break;
                                    }
                                }
                            }
                            else {
                                $(options.bootstrapTable).bootstrapTable('refresh');
                                finalData = options.data;
                            }
                        }
                    }
                    handlerCallback.call(that, callback, e, { oper: 'save', success: result, index: index, data: finalData });
                }
            });
        },

        assign: function (e, callback) {
            var options = this.options;
            var row = {};
            if (options.bootstrapTable && options.bootstrapTable.constructor === String) {
                var arrselections = $(options.bootstrapTable).bootstrapTable('getSelections');
                if (arrselections.length == 0) {
                    swal('请选择要编辑的数据', "编辑操作", "warning");
                    return;
                }
                else if (arrselections.length > 1) {
                    swal('请选择一个要编辑的数据', "编辑操作", "warning");
                    return;
                }
                else {
                    row = arrselections[0];
                }
            }
            var data = options.dataEntity;
            if (data instanceof DataEntity) data = data.get();
            if ($.isFunction(callback)) callback.call(e, row, $.extend({}, data));
            if ($.isFunction(this.options.callback)) this.options.callback.call(e, { oper: 'assign', row: row, data: data });
        }
    };

    var handlerCallback = function (callback, e, data) {
        if ($.isFunction(callback)) callback.call(e, data);
        if ($.isFunction(this.options.callback)) this.options.callback.call(e, data);
    }


    $.extend({
        bc: function (options, callback) {
            var data = $.extend({
                remote: true,
                Id: "",
                url: this.url,
                data: {},
                method: "POST",
                htmlTemplate: '<div class="form-group checkbox col-lg-3 col-xs-4"><label class="tooltips" data-placement="top" data-original-title="{3}" title="{3}"><input type="checkbox" value="{0}" {2}/>{1}</label></div>',
                title: this.title,
                swal: true,
                modal: null,
                callback: null
            }, options);

            if (!data.url || data.url == "") {
                swal('参数错误', '未设置请求地址Url', 'error');
                return;
            }

            if (data.remote && data.url) {
                $.ajax({
                    url: data.url + data.Id,
                    data: data.data,
                    type: data.method,
                    success: function (result) {
                        success(result);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        if ($.isFunction(data.callback)) data.callback(false);
                    }
                });
            }
            function success(result) {
                if ($.isFunction(data.callback)) {
                    data.callback(result);
                }
                if (data.modal !== null) {
                    $("#" + data.modal).modal('hide');
                }
                if (data.swal) {
                    setTimeout(function () {
                        if (result) { swal("成功", data.title, "success"); }
                        else { swal("失败", data.title, "error"); }
                    }, 100);
                }
            }
        }
    });

    // Roles
    Role = {
        url: '../api/Roles/',
        title: "授权角色"
    };

    // Users
    User = {
        url: '../api/Users/',
        title: "授权用户"
    };

    // Groups
    Group = {
        url: '../api/Groups/',
        title: "授权部门"
    };

    // Menus
    Menu = {
        url: '../api/Menus/',
        title: "授权菜单"
    };

    // Exceptions
    Exceptions = {
        url: '../api/Exceptions/',
        title: "程序异常日志"
    };

    // Dicts
    Dicts = {
        url: '../api/Dicts/'
    };

    // Infos
    Infos = {
        url: '../api/Infos/'
    }

    // Profiles
    Profiles = {
        url: '../api/Profiles/',
        title: '网站设置'
    }

    // Messages
    Messages = {
        url: '../api/Messages/'
    }

    // Tasks
    Tasks = {
        url: '../api/Tasks/'
    }

    // Notifications
    Notifications = {
        url: '../api/Notifications/'
    }
})(jQuery);