$(function () {
    var url = '../api/Exceptions/';
    var $dialog = $('#dialogNew');
    var $dataForm = $('#dataForm');
    var $dataFormDetail = $('#dataFormDetail');
    var $errorList = $('#errorList');
    var $errorDetail = $('#errorDetail');
    var $errorDetailTitle = $('#myDetailModalLabel');

    var bsa = new BootstrapAdmin({
        url: url,
        bootstrapTable: null,
        validateForm: null
    });

    $('table').smartTable({
        url: url,
        sortName: 'LogTime',
        queryParams: function (params) { return $.extend(params, { StartTime: $("#txt_operate_start").val(), EndTime: $("#txt_operate_end").val() }); },
        columns: [{ checkbox: true },
            { title: "请求网址", field: "ErrorPage", sortable: true },
            { title: "用户名", field: "UserID", sortable: true },
            { title: "IP", field: "UserIp", sortable: true },
            { title: "错误", field: "Message", sortable: false },
            { title: "记录时间", field: "LogTime", sortable: true }
        ]
    });

    $('input[type="datetime"]').parent().datetimepicker({
        locale: "zh-cn",
        format: "YYYY-MM-DD"
    });

    $('#btn_view').on('click', function (row) {
        $.bc({
            Id: "", url: Exceptions.url, swal: false,
            callback: function (result) {
                var html = result.map(function (ele) {
                    return $.format('<div class="form-group col-lg-3 col-md-3 col-sm-4 col-xs-6"><a class="logfile" href="#"><i class="fa fa-file-text-o"></i><span>{0}</span></a></div>', ele);
                }).join('');
                $dataForm.children('div').html(html);
            }
        });
        $dialog.modal('show');
    });

    $dialog.on('click', 'a', function () {
        var fileName = $(this).find('span').text();
        $errorDetailTitle.text(fileName);
        $errorList.hide();
        $errorDetail.show();
        $dataFormDetail.html('<div class="text-center"><i class="fa fa-spinner fa-pulse fa-3x fa-fw"></i></div>');
        $.bc({
            Id: "", url: Exceptions.url, method: "PUT", swal: false, data: { "": fileName },
            callback: function (result) {
                $dataFormDetail.html(result);
            }
        });
    });

    $errorDetail.on('click', 'button', function () {
        $errorDetail.hide();
        $errorList.show();
    });
});