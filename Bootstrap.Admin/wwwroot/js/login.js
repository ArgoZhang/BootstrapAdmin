$(function () {
    $(".container").autoCenter();

    $("a[data-method]").on('click', function () {
        var $this = $(this);
        switch ($this.attr("data-method")) {
            case "register":
                $("#dialogNew").modal('show');
                break;
            case "forgot":
                break;
        }
    });

    $('#btnSubmit').on('click', function () {
        $.bc({
            url: "api/New",
            data: { UserName: $('#userName').val(), Password: $('#password').val(), DisplayName: $('#displayName').val(), Description: $('#description').val() },
            modal: '#dialogNew',
            method: "post",
            callback: function (result) {
                var title = result ? "提交成功<br/>等待管理员审批" : "提交失败";
                swal({ html: true, showConfirmButton: false, showCancelButton: false, timer: 1500, title: title, type: result ? "success" : "error" });
            }
        });
    });

    $('.rememberPwd').on('click', function () {
        var $this = $(this);
        var $check = $this.find('i');
        var $rem = $('#remember');
        if ($check.hasClass('fa-square-o')) {
            $check.addClass('fa-check-square-o').removeClass('fa-square-o');
            $rem.val('true');
        } else {
            $check.addClass('fa-square-o').removeClass('fa-check-square-o');
            $rem.val('false');
        }
    });
});