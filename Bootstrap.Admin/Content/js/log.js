var saveLog = function (options,otype) {
    $.ajax({
        url: "../api/Logs",
        type: "POST",
        data: CreateOperationData(options,otype),
        success: function (result) {
            if (result) {
            }
        }
    });
}

function CreateOperationData(options,otype) {
    var nameStr = $.cookie("lgb____bd____cp");
    nameStr = nameStr.substring("realUserName=".length, nameStr.indexOf("&"));

    var operationType;
    if (otype == "POST") {
        //0表示新增，1表示修改
        operationType = options.data.ID == 0 ? 0 : 1;
    } else if (otype == "DELETE") {
        //2表示删除
        operationType = 2;
    }
    //operationModule表示修改的模块
    var operationModule = options.url.substring(options.url.lastIndexOf("/") + 1, options.url.length);

    var postdata = { OperationType: operationType, UserName: nameStr, Remark: "", OperationModule: operationModule };
    return postdata;
}