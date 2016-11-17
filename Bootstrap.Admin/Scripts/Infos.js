$(function () {
    var $headerIcon = $('#headerIcon');
    var preIcon = $headerIcon.attr('src');
    $('#fileIcon').fileinput({
        uploadUrl: '../api/Infos',
        language: 'zh',
        allowedFileExtensions: ['jpg'],
        initialPreview: [
            preIcon
        ],
        initialPreviewConfig: [
            { caption: "现在头像", size: 730, showZoom: false },
        ],
        initialPreviewAsData: true,
        overwriteInitial: true,
        dropZoneTitle: "请选择头像"
    }).on('fileuploaded', function (event, data, previewId, index) {
        var url = data.response;
        if (!!url) $headerIcon.attr('src', url);
    });

    $('#infoDataForm').autoValidate({
        displayName: {
            required: true,
            maxlength: 50
        }
    }, {
        button: ['btnSaveDisplayName']
    });
    $('#passwordDataForm').autoValidate({
        currentPassword: {
            required: true,
            maxlength: 50
        },
        newPassword: {
            required: true,
            maxlength: 50
        },
        confirmPassword: {
            required: true,
            equalTo: "#newPassword",
            maxlength: 50
        }
    }, {
        button: ['btnSavePassword']
    });

    var bsa = new BootstrapAdmin({
        url: '../api/Infos',
        bootstrapTable: null,
        dataEntity: new DataEntity({
            map: {
                Password: "currentPassword",
                NewPassword: "newPassword",
                DisplayName: "displayName",
                UserName: "userName"
            }
        }),
        click: {
            assign: [{
                id: 'btnSavePassword',
                click: function (row, data) {
                    if ($(this).attr('data-valid') == "true") {
                        data.UserStatus = 2;
                        User.changePassword(data);
                    }
                }
            }, {
                id: 'btnSaveDisplayName',
                click: function (row, data) {
                    if ($(this).attr('data-valid') == "true") {
                        data.UserStatus = 1;
                        User.saveUserDisplayName(data, function (result) {
                            if (result) {
                                $('#userDisplayName').text(data.DisplayName);
                            }
                        });
                    }
                }
            }]
        }
    });

    $('button[data-admin="True"]').attr('disabled', 'disabled');
    $('#kvFileinputModal').appendTo('body');
});