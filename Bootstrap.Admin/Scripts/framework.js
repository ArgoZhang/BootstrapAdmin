(function ($) {
    BootstrapAdmin = function (options) {
        var that = this;
        options = options || {};
        options.click = $.extend({}, BootstrapAdmin.settings.click, options.click);
        this.options = $.extend({}, BootstrapAdmin.settings, options);
        this.dataEntity = options.dataEntity;

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
                    v.validate().resetForm();
                    v.find('[data-original-title]').lgbTooltip('destroy');
                    v.find('.has-error, .has-success').removeClass("has-error has-success");
                }
            });
        }

        function handler(cid, event) {
            var source = $("#" + cId);
            source.data('click', name);
            if (event !== null) source.data('event', event);
            source.click(function (e) {
                e.preventDefault();
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
                modal: this.options.modal,
                src: this
            };
            return {
                'click .edit': function (e, value, row, index) {
                    op.dataEntity.load(row);
                    $(op.table).bootstrapTable('uncheckAll');
                    $(op.table).bootstrapTable('check', index);
                    handlerCallback.call(op.src, null, e, { oper: 'edit', data: row });
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
                    lgbSwal({ title: '请选择要编辑的数据', type: "warning" });
                    return;
                }
                else if (arrselections.length > 1) {
                    lgbSwal({ title: '请选择一个要编辑的数据', type: "warning" });
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
                    lgbSwal({ title: '请选择要删除的数据', type: "warning" });
                    return;
                }
                else {
                    swal({
                        title: "您确定要删除吗？",
                        type: "warning",
                        showCancelButton: true,
                        closeOnConfirm: true,
                        confirmButtonText: "是的，我要删除",
                        confirmButtonColor: "#d9534f",
                        cancelButtonText: "取消"
                    }, function () {
                        setTimeout(function () {
                            var iDs = arrselections.map(function (element, index) { return element.Id }).join(",");
                            options.IDs = iDs;
                            $.bc({
                                url: options.url, data: { "": iDs }, method: 'DELETE', title: '删除数据',
                                callback: function (result) {
                                    if ($.isPlainObject(result)) {
                                        lgbSwal({ title: result.msg, type: result.result ? "success" : "error" });
                                        result = result.result;
                                        this.swal = false;
                                    }
                                    if (result) $(options.bootstrapTable).bootstrapTable('refresh');
                                    handlerCallback.call(that, callback, e, { oper: 'del', success: result });
                                }
                            });
                        }, 100);
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
                        if (options.bootstrapTable.constructor === String && options.data.Id.constructor === String) {
                            // 更新表格
                            if (options.data.Id > 0) {
                                var allTableData = $(options.bootstrapTable).bootstrapTable('getData');
                                for (index = 0; index < allTableData.length; index++) {
                                    finalData = allTableData[index];
                                    if (finalData.Id == options.data.Id) {
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
                    lgbSwal({ title: '请选择要编辑的数据', type: "warning" });
                    return;
                }
                else if (arrselections.length > 1) {
                    lgbSwal({ title: '请选择一个要编辑的数据', type: "warning" });
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
})(jQuery);