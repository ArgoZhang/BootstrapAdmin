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
    }).on('fileuploaded', function (event, data, previewId, index) {
        var url = data.response;
        if (!!url) $headerIcon.attr('src', url);
    });
});