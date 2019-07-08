$(function () {
    var $taskMsg = $('#taskMsg');
    var $taskLogModelTitle = $('#taskModalLabel');
    var stateFormatter = function (value) {
        var template = "<button class='btn btn-sm btn-{0}'><i class='fa fa-{1}'></i><span>{2}<span></button>";
        var content = "";
        if (value === "0") {
            content = $.format(template, 'info', 'fa', '未开始');
        }
        else if (value === "1") {
            content = $.format(template, 'success', 'play-circle', '运行中');
        }
        else if (value === "2") {
            content = $.format(template, 'primary', 'stop-circle', '已停止');
        }
        else if (value === "3") {
            content = $.format(template, 'danger', 'times-circle', '已禁用');
        }
        return content;
    };
    var enabledFormatter = function (value) {
        var template = "<i class='fa fa-toggle-{0}'></i>";
        return $.format(template, value ? 'on' : 'off');
    };
    $('.card-body table').lgbTable({
        url: Tasks.url,
        dataBinder: {
            map: {
                Id: "#taskID",
                Name: "#taskName"
            }
        },
        smartTable: {
            sortName: 'CreateTime',
            sortOrder: 'desc',
            queryParams: function (params) { return $.extend(params, { operateType: $("#txt_operate_type").val(), OperateTimeStart: $("#txt_operate_start").val(), OperateTimeEnd: $("#txt_operate_end").val() }); },
            columns: [
                { title: "名称", field: "Name", sortable: true },
                { title: "创建时间", field: "CreatedTime", sortable: true },
                { title: "上次执行时间", field: "LastRuntime", sortable: true },
                { title: "下次执行时间", field: "NextRuntime", sortable: true },
                { title: "触发条件", field: "TriggerExpression", sortable: false },
                { title: "是否启用", field: "Enabled", sortable: true, formatter: enabledFormatter },
                { title: "状态", field: "Status", sortable: true, align: 'center', width: 106, formatter: stateFormatter }
            ],
            editButtons: {
                events: {
                    'click .info': function (e, value, row, index) {
                        $taskLogModelTitle.html(row.Name + ' - 任务日志窗口(最新50条)');
                        $.bc({
                            url: 'api/Tasks?name=' + row.Name,
                            method: 'put'
                        });
                        $('#dialogLog').modal('show').on('hide.bs.modal', function () {
                            // close hub
                            if ($taskMsg.hub) $taskMsg.hub.stop();
                            $taskMsg.html('');
                        });

                        var lastMsg = "";
                        // open hub
                        $taskMsg.notifi({
                            url: 'NotiHub',
                            method: 'taskRev',
                            callback: function (result) {
                                if (lastMsg === result) return;
                                lastMsg = result;
                                while (this.children().length > 50) {
                                    this.children().first().remove();
                                }
                                this.append('<div>' + result + '</div>');
                            },
                            onclose: function (error) {
                                console.log(error);
                            }
                        });
                    }
                }
            }
        }
    });
});