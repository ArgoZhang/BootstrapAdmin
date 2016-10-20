(function ($) {
    ExtenderChecker = function (options) {
        var that = this;
        this.options = $.extend({}, ExtenderChecker.settings, options);

        this.dataEntity = options.dataEntity;
        if (!(this.dataEntity instanceof DataEntity) && window.console) {
            window.console.log('初始化参数中没有DataEntity实例');
        }

        // handler click event
        for (name in this.options.click) {
            var source = $("#" + this.options.click[name]);
            source.data('click', name);
            $("#" + this.options.click[name]).click(function () {
                var method = $(this).data('click');
                ExtenderChecker.prototype[method].apply(that);
            });
        }

        // handler modal window show event
        if (this.options.modal.constructor === String) {
            $('#' + this.options.modal).on('show.bs.modal', function (e) {
                if (that.options.validateForm.constructor === String) {
                    var v = $('#' + that.options.validateForm).validate();
                    v.currentElements.each(function () { $(this).popover('destroy'); })
                    v.resetForm();
                }
            });
        }
    };

    ExtenderChecker.VERSION = "1.0";
    ExtenderChecker.Author = "Argo Zhang";
    ExtenderChecker.Email = "argo@163.com";

    ExtenderChecker.settings = {
        url: undefined,
        bootstrapTable: 'table',
        validateForm: 'dataForm',
        modal: 'dialogNew',
        click: {}
    }

    ExtenderChecker.idFormatter = function (value, row, index) {
        return "<a class='edit' href='javascript:void(0)'>" + value + "</a>";
    };

    ExtenderChecker.prototype = {
        constructor: ExtenderChecker,

        query: function () {
            if (this.options.bootstrapTable.constructor === String) $(this.options.bootstrapTable).bootstrapTable('refresh');
        },

        create: function () {
            this.dataEntity.reset();
            if (this.options.modal.constructor === String) $('#' + this.options.modal).modal("show");
            if (this.options.bootstrapTable.constructor === String) $(this.options.bootstrapTable).bootstrapTable('uncheckAll');
        },

        edit: function () {
            options = this.options;
            if (options.bootstrapTable.constructor !== String) return;
            var arrselections = $(options.bootstrapTable).bootstrapTable('getSelections');
            if (arrselections.length == 0) {
                swal('请选择要编辑的条目', "编辑操作", "warning");
            }
            else if (arrselections.length > 1) {
                swal('请选择一个要编辑的条目', "编辑操作", "warning");
            }
            else {
                this.dataEntity.load(arrselections[0]);
                if (options.modal.constructor === String) $('#' + options.modal).modal("show");
            }
        },

        del: function () {
            var options = this.options;
            if (options.bootstrapTable.constructor !== String) return;
            var arrselections = $(options.bootstrapTable).bootstrapTable('getSelections');
            if (arrselections.length == 0) {
                swal('请选择要删除的条目', "删除操作", "warning");
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
                    $.ajax({
                        url: options.url,
                        data: { "": iDs },
                        type: 'DELETE',
                        success: function (result) {
                            if (result) setTimeout(function () { swal("成功！", "删除数据", "success"); $(options.bootstrapTable).bootstrapTable('refresh'); }, 100);
                            else setTimeout(function () { swal("失败", "删除数据", "error"); }, 200);
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            swal("失败", "删除数据", "error");
                        }
                    });
                });
            }
        },

        save: function () {
            var options = $.extend({}, this.options, { data: this.dataEntity.get() });
            if (options.validateForm.constructor === String && !$("#" + options.validateForm).valid()) return;
            $.ajax({
                url: options.url,
                data: options.data,
                type: 'POST',
                success: function (result) {
                    if (result) {
                        if ($.isFunction(options.success)) options.success(options.data);
                        if (options.bootstrapTable.constructor === String && options.data.ID.constructor === String) {
                            // 更新表格
                            if (options.data.ID > 0) {
                                var allTableData = $(options.bootstrapTable).bootstrapTable('getData');
                                for (index = 0; index < allTableData.length; index++) {
                                    var temp = allTableData[index];
                                    if (temp.ID == options.data.ID) {
                                        $(options.bootstrapTable).bootstrapTable('updateRow', { index: index, row: $.extend(temp, options.data) });
                                        break;
                                    }
                                }
                            }
                            else {
                                $(options.bootstrapTable).bootstrapTable('refresh');
                            }
                        }
                        if (options.modal.constructor === String) $('#' + options.modal).modal("hide");
                        swal("成功", "保存数据", "success");
                    }
                    else {
                        swal("失败", "保存数据", "error");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    swal("失败", "保存数据失败", "error");
                }
            });
        }
    };
})(jQuery);