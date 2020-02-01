$(function () {
    var apiUrl = "api/OnlineUsers";
    var $table = $('table').smartTable({
        url: apiUrl,
        method: "get",
        sidePagination: "client",
        toolbar: false,
        search: false,
        showToggle: false,
        showRefresh: false,
        showColumns: false,
        showAdvancedSearchButton: false,
        columns: [
            {
                title: "序号", formatter: function (value, row, index) {
                    var options = $table.bootstrapTable('getOptions');
                    return options.pageSize * (options.pageNumber - 1) + index + 1;
                }
            },
            {
                title: "会话Id", field: "ConnectionId"
            },
            { title: "登录名称", field: "UserName" },
            { title: "显示名称", field: "DisplayName" },
            { title: "登录时间", field: "FirstAccessTime" },
            { title: "访问时间", field: "LastAccessTime" },
            { title: "请求方式", field: "Method" },
            { title: "主机", field: "Ip" },
            { title: "登录地点", field: "Location" },
            { title: "浏览器", field: "Browser" },
            { title: "操作系统", field: "OS" },
            { title: "访问地址", field: "RequestUrl" },
            {
                title: "历史地址", field: "ConnectionId", formatter: function (value, row, index, field) {
                    return $.format('<button type="button" class="btn btn-info" data-id="{0}" data-toggle="popover" data-trigger="focus" data-html="true" data-title="访问记录"><i class="fa fa-info"></i><span>明细</span></button>', value);
                }
            }
        ]
    }).on('click', 'button[data-id]', function () {
        var $this = $(this);
        if (!$this.data($.fn.popover.Constructor.DATA_KEY)) {
            var id = $this.attr('data-id');
            var data = $table.bootstrapTable('getData');
            var row = data.filter(function (v) {
                return v.ConnectionId === id;
            });
            var content = row[0].RequestUrls.map(function (item) {
                return $.format("<tr><td>{0}</td><td>{1}</td></tr>", item.Key, item.Value);
            }).join('');
            content = content === '' ?
                '已断开' :
                $.format("<div class='bootstrap-table' style='margin: 4px 0;'><div class='fixed-table-container'><div class='fixed-table-body'><table class='table table-bordered table-hover'><thead><tr><th class='p-1'><b>访问时间</b></th><th class='p-1'>访问地址</th></tr></thead><tbody>{0}</tbody></table></div></div></div>", content);
            $this.popover({ content: content, sanitize: false, placement: $(window).width() < 768 ? 'top' : 'left' });
            $this.popover('show');
        }
    }).on('mouseup', 'button[data-id]', function () {
        $(this).focus();
    });

    $('#refreshUsers').tooltip().on('click', function () {
        $table.bootstrapTable('refresh');
    });
});