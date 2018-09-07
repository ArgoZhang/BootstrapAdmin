$(function () {
    var $headerIcon = $('#headerIcon');
    var preIcon = $headerIcon.attr('src');
    $('#fileIcon').fileinput({
        uploadUrl: $.formatUrl(Profiles.url),
        browseOnZoneClick: true,
        theme: 'fa',
        language: 'zh',
        maxFileSize: 5000,
        allowedFileExtensions: ['jpg', 'png', 'bmp', 'gif', 'jpeg'],
        initialPreview: [
            preIcon
        ],
        initialPreviewConfig: [
            { caption: "现在头像", size: $('#fileIcon').attr('data-init'), showZoom: true },
        ],
        initialPreviewAsData: true,
        overwriteInitial: true,
        dropZoneTitle: "请选择头像"
    }).on('fileuploaded', function (event, data, previewId, index) {
        var url = data.response.initialPreview[0];
        if (!!url === true) $headerIcon.attr('src', url);
    });

    var dataBinder = new DataEntity({
        Password: "#currentPassword",
        NewPassword: "#newPassword",
        DisplayName: "#displayName",
        UserName: "#userName",
        Css: "#css"
    });

    $('button[data-method]').on('click', function (e) {
        var $this = $(this);
        if ($this.parent().attr("data-admin") === "True") return false;
        var data = dataBinder.get();
        switch ($this.attr('data-method')) {
            case 'password':
                data.UserStatus = 'ChangePassword';
                $.bc({ url: User.url, method: "PUT", data: data, title: "更改密码" });
                break;
            case 'user':
                data.UserStatus = 'ChangeDisplayName';
                $.bc({
                    url: User.url, method: "PUT", data: data, title: "修改用户显示名称",
                    callback: function (result) {
                        if (result) {
                            $('#userDisplayName').text(data.DisplayName);
                        }
                    }
                });
                break;
            case 'css':
                data.UserStatus = 'ChangeTheme';
                $.bc({ url: User.url, method: "PUT", data: data, title: "保存样式" });
                break;
        }
    });
    $('[data-admin="False"]').removeClass('d-none');
    if ($('[enctype="multipart/form-data"]').is(":hidden")) {
        $('.card-img').removeClass('d-none');
    }
    $('#css').dropdown('val');
});