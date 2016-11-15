$(function () {
    bindImageSelectListener();

    $('#btn_img').click(function () {
        $.ajaxFileUpload({
            url: '../api/Infos/',        
            secureuri: false,
            fileElementId: 'uploadImg',  //上传文件的Name属性
            dataType: 'json',
            type: 'POST',
            success: function (data, status) {
                if (data.Result === true) {    
                    swal("成功", "修改头像", "success");
                    //刷新头像
                    changeHeaderIcon(data.img_str);
                } else if (data.Result === false) {
                   swal("失败", "网站管理员不允许修改头像", "error");
                }
                bindImageSelectListener();
            },
            error: function (data, status, e) {
                swal("失败", "修改头像", "error");
                bindImageSelectListener();
            }
        });
       
        return false;
    })
})

function changeHeaderIcon(img_str) {
    $("#header_img").attr("src", img_str);
}

function bindImageSelectListener() {
    $('#uploadImg').on("change", function () {
        var file = this.files[0];
        if (this.files && file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#headImg').attr("src", e.target.result);
            }
            reader.readAsDataURL(file);
        }
    })
}