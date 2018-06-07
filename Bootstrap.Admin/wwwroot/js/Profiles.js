$(function () {
    var $headerIcon = $('#headerIcon');
    var preIcon = $headerIcon.attr('src');
    $('#fileIcon').fileinput({
        uploadUrl: Profiles.url,
        language: 'zh',
        maxFileSize: 5000,
        allowedFileExtensions: ['jpg', 'png', 'bmp', 'gif', 'jpeg'],
        initialPreview: [
            preIcon
        ],
        initialPreviewConfig: [
            { caption: "现在头像", size: $('#fileIcon').attr('data-init'), showZoom: false },
        ],
        initialPreviewAsData: true,
        overwriteInitial: true,
        dropZoneTitle: "请选择头像"
    }).on('fileuploaded', function (event, data, previewId, index) {
        var url = data.response;
        if (!!url) $headerIcon.attr('src', url);
    });

    var bsa = new BootstrapAdmin({
        url: Profiles.url,
        bootstrapTable: null,
        dataEntity: new DataEntity({
            map: {
                Password: "currentPassword",
                NewPassword: "newPassword",
                DisplayName: "displayName",
                UserName: "userName",
                Css: "css"
            }
        })
    });

    $('button[data-method]').on('click', function (e) {
        var $this = $(this);
        var data = bsa.dataEntity.get();
        switch ($this.attr('data-method')) {
            case 'password':
                data.UserStatus = 2;
                $.bc({ url: User.url, method: "PUT", data: data, title: "更改密码" });
                break;
            case 'user':
                data.UserStatus = 1;
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
                data.UserStatus = 3;
                $.bc({ url: User.url, method: "PUT", data: data, title: "保存样式" });
                break;
        }
    });
    $('button[data-admin="False"]').removeAttr('disabled');
    $('#kvFileinputModal').appendTo('body');
});