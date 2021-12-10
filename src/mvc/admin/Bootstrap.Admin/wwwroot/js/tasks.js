$(function () {
    var $taskMsg = $('#taskMsg');
    var $taskLogModelTitle = $('#taskModalLabel');
    var stateFormatter = function (value) {
        var template = "<button class='btn btn-sm btn-{0}'><i class='fa fa-{1}'></i><span>{2}<span></button>";
        var content = "";
        if (value === "Ready") {
            content = $.format(template, 'info', 'fa', '未开始');
        }
        else if (value === "Running") {
            content = $.format(template, 'success', 'play-circle', '运行中');
        }
        else if (value === "Disabled") {
            content = $.format(template, 'danger', 'times-circle', '已停止');
        }
        return content;
    };
    var resultFormatter = function (value) {
        var template = "<button class='btn btn-sm btn-{0}'><span>{1}<span></button>";
        var content = "";
        if (value === "Success") {
            content = $.format(template, 'success', '成功');
        }
        else if (value === "Error") {
            content = $.format(template, 'danger', '故障');
        }
        else if (value === "Cancelled") {
            content = $.format(template, 'info', '取消');
        }
        else if (value === "Timeout") {
            content = $.format(template, 'warning', '超时');
        }
        return content;
    };

    var $task = $('#table').lgbTable({
        url: Tasks.url,
        dataBinder: {
            map: {
                CronExpression: "#taskCron",
                Name: "#taskName",
                TaskExecutorName: "#taskExecutor"
            }
        },
        smartTable: {
            idField: 'Name',
            sidePagination: "client",
            sortName: 'Name',
            sortOrder: 'asc',
            showAdvancedSearchButton: false,
            queryParams: function (params) { return $.extend(params, { operateType: $("#txt_operate_type").val(), OperateTimeStart: $("#txt_operate_start").val(), OperateTimeEnd: $("#txt_operate_end").val() }); },
            columns: [
                { title: "名称", field: "Name", sortable: true },
                { title: "创建时间", field: "CreatedTime", sortable: true },
                { title: "上次执行时间", field: "LastRuntime", sortable: true },
                { title: "下次执行时间", field: "NextRuntime", sortable: true },
                { title: "触发条件", field: "TriggerExpression", sortable: false },
                { title: "执行结果", field: "LastRunResult", sortable: false, align: 'center', formatter: resultFormatter },
                { title: "状态", field: "Status", sortable: true, align: 'center', formatter: stateFormatter }
            ],
            editButtons: {
                events: {
                    'click .info': function (e, value, row, index) {
                        $taskLogModelTitle.html(row.Name + ' - 任务日志窗口(最新50条)');
                        $('#dialogLog').modal('show').on('hide.bs.modal', function () {
                            // close hub
                            if ($taskMsg.hub) $taskMsg.hub.stop();
                            $taskMsg.html('<div></div>');
                        });

                        // open hub
                        $taskMsg.notifi({
                            url: 'TaskLogHub',
                            callback: function (result) {
                                var content = this.children();
                                while (content.children().length > 50) {
                                    content.children().first().remove();
                                }
                                var data = JSON.parse(result);
                                if (data.name !== row.Name) return;

                                result = data.msg;
                                result = result.replace("Run(Cancelled)", "<span class='text-info'>Run(Cancelled)</span>");
                                result = result.replace("Run(Success)", "<span class='text-success'>Run(Success)</span>");
                                result = result.replace("Run(Error)", "<span class='text-danger'>Run(Error)</span>");
                                result = result.replace("Run(Timeout)", "<span class='text-warning'>Run(Timeout)</span>");
                                content.append('<div>' + result + '</div>');

                                // auto scroll
                                if ($autoScroll.find('i').hasClass(check[0])) this.scrollTop(content.height());
                            },
                            invoke: function () {
                                $.bc({
                                    url: 'api/TasksLog?name=' + encodeURI(row.Name)
                                });
                            },
                            onclose: function (error) {
                                console.log(error);
                            }
                        });
                    },
                    'click .pause': function (e, value, row, index) {
                        $.bc({
                            url: 'api/Tasks/' + row.Name + "?operType=" + "pause", method: 'put',
                            callback: function (result) {
                                if (result) $task.bootstrapTable('refresh');
                            }
                        });
                    },
                    'click .run': function (e, value, row, index) {
                        $.bc({
                            url: 'api/Tasks/' + row.Name + "?operType=" + "run", method: 'put',
                            callback: function (result) {
                                if (result) $task.bootstrapTable('refresh');
                            }
                        });
                    }
                }
            }
        }
    });

    var $autoScroll = $('#dialogLog').find('.modal-footer > a.btn');
    var check = ["fa-check-square-o", "fa-square-o"];
    $autoScroll.on('click', function () {
        var $this = $(this).find('i');

        if ($this.hasClass(check[0])) $this.addClass(check[1]).removeClass(check[0]);
        else $this.addClass(check[0]).removeClass(check[1]);
    });
});