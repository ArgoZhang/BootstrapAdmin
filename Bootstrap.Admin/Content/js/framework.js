(function ($) {
    var handlerCallback = function (callback, e, data) {
        if ($.isFunction(callback)) callback.call(e, data);
        if ($.isFunction(this.options.callback)) this.options.callback.call(e, data);
    }

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
                        $.ajax({
                            url: options.url,
                            data: { "": iDs },
                            type: 'DELETE',
                            success: function (result) {
                                if (result) {
                                    if ($.isPlainObject(result)) {
                                        var info = result.result ? "success" : "error";
                                        var msg = result.msg
                                        swal(msg, "删除数据", info);
                                        result = result.result;
                                    }
                                    else swal("成功", "删除数据", "success");
                                    if (result) $(options.bootstrapTable).bootstrapTable('refresh');
                                }
                                else swal("失败", "删除数据", "error");
                                handlerCallback.call(that, callback, e, { oper: 'del', success: !!result, data: iDs });
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                swal("失败", "删除数据", "error");
                                handlerCallback.call(that, callback, e, { oper: 'del', success: false });
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
            $.ajax({
                url: options.url,
                data: options.data,
                type: 'POST',
                success: function (result) {
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
                        if (options.modal.constructor === String) $('#' + options.modal).modal("hide");
                        swal("成功", "保存数据", "success");
                    }
                    else {
                        swal("失败", "保存数据", "error");
                    }
                    handlerCallback.call(that, callback, e, { oper: 'save', success: !!result, index: index, data: finalData });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    swal("失败", "保存数据失败", "error");
                    handlerCallback.call(that, callback, e, { oper: 'save', success: false });
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

    var htmlTemplate = '<div class="form-group checkbox col-lg-3 col-xs-4"><label class="tooltips" data-placement="top" data-original-title="{3}" title="{3}"><input type="checkbox" value="{0}" {2}/>{1}</label></div>';

    var processData = function (options) {
        var data = $.extend({ data: {}, remote: true, method: "POST", Id: "", url: this.url, title: this.title, html: this.html, swal: true }, options);

        if (data.remote) {
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
        else success()

        function success(result) {
            if ($.isFunction(data.callback)) {
                var formatData = result;
                if ($.isArray(result)) {
                    if ($.isFunction(data.html)) formatData = data.html(result);
                }
                data.callback(formatData);
            }
            else if ($.isPlainObject(data.callback) && data.callback.modal !== undefined) {
                $("#" + data.callback.modal).modal('hide');
            }
            if (data.swal) {
                if (result) { swal("成功", data.title, "success"); }
                else { swal("失败", data.title, "error"); }
            }
        }
    };

    window.bd = processData;

    // Roles
    Role = {
        url: '../api/Roles/',
        title: "授权角色",
        html: function (result) {
            return $.map(result, function (element, index) {
                return $.format(htmlTemplate, element.ID, element.RoleName, element.Checked, element.Description);
            }).join('');
        }
    };
    Role.getRolesByUserId = function (userId, callback) {
        processData.call(this, { Id: userId, callback: callback, data: { type: "user" }, swal: false });
    };
    Role.getRolesByGroupId = function (groupId, callback) {
        processData.call(this, { Id: groupId, callback: callback, data: { type: "group" }, swal: false });
    };
    Role.getRolesByMenuId = function (menuId, callback) {
        processData.call(this, { Id: menuId, callback: callback, data: { type: "menu" }, swal: false });
    };
    Role.saveRolesByUserId = function (userId, roleIds, callback) {
        processData.call(this, { Id: userId, callback: callback, method: "PUT", data: { type: "user", roleIds: roleIds } });
    };
    Role.saveRolesByGroupId = function (groupId, roleIds, callback) {
        processData.call(this, { Id: groupId, callback: callback, method: "PUT", data: { type: "group", roleIds: roleIds } });
    };
    Role.saveRolesByMenuId = function (menuId, roleIds, callback) {
        processData.call(this, { Id: menuId, callback: callback, method: "PUT", data: { type: "menu", roleIds: roleIds } });
    };
    // Users
    User = {
        url: '../api/Users/',
        title: "授权用户",
        html: function (result) {
            return $.map(result, function (element, index) {
                return $.format(htmlTemplate, element.ID, element.DisplayName, element.Checked, element.UserName);
            }).join('');
        }
    };
    User.getUsersByRoleId = function (roleId, callback) {
        processData.call(this, { Id: roleId, callback: callback, data: { type: "role" }, swal: false });
    };
    User.saveUsersByRoleId = function (roleId, userIds, callback) {
        processData.call(this, { Id: roleId, callback: callback, method: "PUT", data: { type: "role", userIds: userIds } });
    };
    User.getUsersByGroupeId = function (groupId, callback) {
        processData.call(this, { Id: groupId, callback: callback, data: { type: "group" }, swal: false });
    };
    User.saveUsersByGroupId = function (groupId, userIds, callback) {
        processData.call(this, { Id: groupId, callback: callback, method: "PUT", data: { type: "group", userIds: userIds } });
    };
    User.saveUserDisplayName = function (user, callback) {
        processData.call(this, { Id: '', callback: callback, method: "PUT", data: user, title: "修改用户显示名称" });
    };
    User.changePassword = function (user) {
        processData.call(this, { Id: '', method: "PUT", data: user });
    };
    User.processUser = function (id, result, callback) {
        processData.call(this, { Id: id, callback: callback, method: "PUT", data: { type: "user", userIds: result }, title: result == "1" ? "授权用户" : "拒绝用户" });
    };
    // Groups
    Group = {
        url: '../api/Groups/',
        title: "授权部门",
        html: function (result) {
            return $.map(result, function (element, index) {
                return $.format(htmlTemplate, element.ID, element.GroupName, element.Checked, element.Description);
            }).join('');
        }
    };
    Group.getGroupsByUserId = function (userId, callback) {
        processData.call(this, { Id: userId, callback: callback, data: { type: "user" }, swal: false });
    };
    Group.saveGroupsByUserId = function (userId, groupIds, callback) {
        processData.call(this, { Id: userId, callback: callback, method: "PUT", data: { type: "user", groupIds: groupIds } });
    };
    Group.getGroupsByRoleId = function (roleId, callback) {
        processData.call(this, { Id: roleId, callback: callback, data: { type: "role" }, swal: false });
    };
    Group.saveGroupsByRoleId = function (roleId, groupIds, callback) {
        processData.call(this, { Id: roleId, callback: callback, method: "PUT", data: { type: "role", groupIds: groupIds } });
    };

    // Menus
    Menu = {
        url: '../api/Menus/',
        title: "授权菜单",
        html: function (result) {
            var htmlString = "";
            if ($.isArray(result)) {
                htmlString = Menu.cascadeMenu(result)
            }
            return htmlString;
        }
    };
    Menu.cascadeMenu = function (menus) {
        var html = "";
        $.each(menus, function (index, menu) {
            if (menu.Menus.length == 0) {
                html += $.format('<li class="dd-item dd3-item" data-id="{0}" data-category="{3}"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><label><input type="checkbox" value="{0}"><span><i class="{1}"></i>{2}</span></label><label><input type="radio" name="menu" value="{0}"><span><i class="{1}"></i>{2}</span></label></div></li>', menu.ID, menu.Icon, menu.Name, menu.Category);
            }
            else {
                html = $.format('<li class="dd-item dd3-item" data-id="{0}" data-category="{3}"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><label><input type="checkbox" value="{0}"><span><i class="{1}"></i>{2}</span></label><label><input type="radio" name="menu" value="{0}"><span><i class="{1}"></i>{2}</span></label></div></li><ol class="dd-list">{4}</ol>', menu.ID, menu.Icon, menu.Name, menu.Category, Menu.cascadeMenu(menu.Menus));
            }
        });
        return html;
    };
    Menu.getMenus = function (callback) {
        processData.call(this, { Id: 0, callback: callback, data: { type: "user" }, swal: false });
    };
    Menu.getMenusByRoleId = function (roleId, callback) {
        processData.call(this, { Id: roleId, callback: callback, data: { type: "role" }, swal: false, html: null });
    };
    Menu.saveMenusByRoleId = function (roleId, menuIds, callback) {
        processData.call(this, { Id: roleId, callback: callback, method: "PUT", data: { type: "role", menuIds: menuIds } });
    };

    // Exceptions
    Exceptions = {
        url: '../api/Exceptions/',
        title: "程序异常日志",
        html: function (result) {
            return result.map(function (ele) {
                return $.format('<div class="form-group col-lg-3 col-md-3 col-sm-4 col-xs-6"><a class="logfile" href="#"><i class="fa fa-file-text-o"></i><span>{0}</span></a></div>', ele);
            }).join('');
        }
    };
    Exceptions.getFiles = function (callback) {
        processData.call(this, { Id: "", callback: callback, swal: false });
    }
    Exceptions.getFileByName = function (fileName, callback) {
        processData.call(this, { Id: "", callback: callback, method: "PUT", swal: false, data: { "": fileName } });
    };

    // Dicts
    Dicts = {
        url: '../api/Dicts/'
    };
    Dicts.retrieveCategories = function (callback) {
        processData.call(this, { Id: 1, callback: callback, swal: false, data: { type: 'category' } });
    };

    // Notifi
    Notifications = {
        url: '../api/Notifications/'
    };
    Notifications.retrieveNotifies = function (category, callback) {
        processData.call(this, { Id: category, callback: callback, method: "GET", swal: false });
    };
    Notifications.retrieveAllNotifies = function (callback) {
        processData.call(this, { Id: "", callback: callback, method: "GET", swal: false });
    }
})(jQuery);