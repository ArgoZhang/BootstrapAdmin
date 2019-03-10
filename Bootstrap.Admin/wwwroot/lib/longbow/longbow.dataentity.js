(function ($) {
    DataEntity = function (options) {
        this.options = options;
    };

    DataEntity.prototype = {
        load: function (value) {
            for (name in this.options) {
                var ctl = $(this.options[name]);
                if (ctl.attr('data-toggle') === "dropdown") {
                    ctl.val(value[name]).dropdown('val');
                }
                else if (ctl.attr('data-toggle') === 'toggle') {
                    ctl.bootstrapToggle(value[name] ? 'on' : 'off');
                }
                else ctl.val(value[name]);
            }
        },
        reset: function () {
            for (name in this.options) {
                var ctl = $(this.options[name]);
                var dv = ctl.attr("data-default-val");
                if (dv === undefined) dv = "";
                if (ctl.attr('data-toggle') === "dropdown") {
                    ctl.val(dv).dropdown('val');
                }
                else if (ctl.attr('data-toggle') === 'toggle') {
                    ctl.bootstrapToggle(dv === "true" ? 'on' : 'off');
                }
                else ctl.val(dv);
            }
        },
        get: function () {
            var target = {};
            for (name in this.options) {
                var ctl = $(this.options[name]);
                var dv = ctl.attr('data-default-val');
                if (ctl.attr('data-toggle') === 'toggle') {
                    target[name] = ctl.prop('checked');
                    continue;
                }
                else if (dv !== undefined && ctl.val() === "") target[name] = dv;
                else target[name] = ctl.val();
                if (target[name] === "true" || target[name] === "True") target[name] = true;
                if (target[name] === "false" || target[name] === "False") target[name] = false;
            }
            return target;
        }
    };

    DataTable = function (options) {
        var that = this;
        this.options = $.extend(true, {}, DataTable.settings, options);
        this.dataEntity = new DataEntity(options.map);

        // handler click event
        for (var name in this.options.click) {
            $(name).on('click', { handler: this.options.click[name] }, function (e) {
                e.preventDefault();
                e.data.handler.call(that, this);
            });
        }

        // handler extra click event
        for (var cId in this.options.events) {
            $(cId).on('click', { handler: this.options.events[cId] }, function (e) {
                var options = that.options;
                var row = {};
                if (options.bootstrapTable && options.bootstrapTable.constructor === String) {
                    var arrselections = $(options.bootstrapTable).bootstrapTable('getSelections');
                    if (arrselections.length === 0) {
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
                e.data.handler.call(this, row);
            });
        }
    };

    DataTable.settings = {
        url: undefined,
        bootstrapTable: 'table',
        modal: '#dialogNew',
        click: {
            '#btn_query': function (element) {
                if (this.options.bootstrapTable.constructor === String) $(this.options.bootstrapTable).bootstrapTable('refresh');
                handlerCallback.call(this, null, element, { oper: 'query' });
            },
            '#btn_add': function (element) {
                this.dataEntity.reset();
                if (this.options.modal.constructor === String) $(this.options.modal).modal("show");
                if (this.options.bootstrapTable.constructor === String) $(this.options.bootstrapTable).bootstrapTable('uncheckAll');
                handlerCallback.call(this, null, element, { oper: 'create' });
            },
            '#btn_edit': function (element) {
                var options = this.options;
                var data = {};
                if (options.bootstrapTable.constructor === String) {
                    var arrselections = $(options.bootstrapTable).bootstrapTable('getSelections');
                    if (arrselections.length === 0) {
                        lgbSwal({ title: '请选择要编辑的数据', type: "warning" });
                        return;
                    }
                    else if (arrselections.length > 1) {
                        lgbSwal({ title: '请选择一个要编辑的数据', type: "warning" });
                        return;
                    }
                    else {
                        data = arrselections[0];
                        this.dataEntity.load(data);
                        if (options.modal.constructor === String) $(options.modal).modal("show");
                    }
                }
                handlerCallback.call(this, null, element, { oper: 'edit', data: data });
            },
            '#btn_delete': function (element) {
                var that = this;
                var options = this.options;
                if (options.bootstrapTable.constructor === String) {
                    var arrselections = $(options.bootstrapTable).bootstrapTable('getSelections');
                    if (arrselections.length === 0) {
                        lgbSwal({ title: '请选择要删除的数据', type: "warning" });
                        return;
                    }
                    else {
                        swal({
                            title: "删除数据",
                            text: "您确定要删除选中的所有数据吗",
                            type: "warning",
                            showCancelButton: true,
                            cancelButtonClass: 'btn-secondary',
                            confirmButtonText: "我要删除",
                            confirmButtonClass: "btn-danger ml-2",
                            cancelButtonText: "取消"
                        }, function () {
                            $.logData.push({ url: options.url, data: arrselections });
                            setTimeout(function () {
                                var iDs = arrselections.map(function (element, index) { return element.Id; });
                                $.bc({
                                    url: options.url, data: iDs, method: 'delete', title: '删除数据',
                                    callback: function (result) {
                                        if (result) $(options.bootstrapTable).bootstrapTable('refresh');
                                        handlerCallback.call(that, null, element, { oper: 'del', success: result });
                                    }
                                });
                            }, 100);
                        });
                    }
                }
            },
            '#btnSubmit': function (element) {
                var that = this;
                var options = $.extend(true, {}, this.options, { data: this.dataEntity.get() });
                $.bc({
                    url: options.url, data: options.data, title: "保存数据", modal: options.modal, method: "post",
                    callback: function (result) {
                        if (result) {
                            $(options.bootstrapTable).bootstrapTable('refresh');
                            handlerCallback.call(that, null, element, { oper: 'save', success: result, data: options.data });
                        }
                    }
                });
            }
        }
    };

    DataTable.prototype = {
        constructor: DataTable,
        idEvents: function () {
            var op = {
                dataEntity: this.dataEntity,
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
                    $(op.modal).modal("show");
                }
            };
        }
    };

    function handlerCallback(callback, element, data) {
        if ($.isFunction(callback)) callback.call(e, data);
        if ($.isFunction(this.options.callback)) this.options.callback.call(element, data);
    }
}(jQuery));