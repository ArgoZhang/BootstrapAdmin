$(function () {
    var bsa = new BootstrapAdmin({
        url: '../api/Users',
        dataEntity: new DataEntity({
            map: {
                ID: "userID",
                UserName: "userName",
                Password: "password",
                DisplayName: "displayName"
            }
        }),
        click: {
            assign: [{
                id: 'btn_assignRole',
                click: function (row) {
                    Role.getRolesByUserId(row.ID, function (data) {
                        $("#dialogRole .modal-title").text($.format('{0}-角色授权窗口', row.DisplayName));
                        $('#dialogRole form').html(data);
                        $('#dialogRole').modal('show');
                    });
                }
            }, {
                id: 'btn_assignGroup',
                click: function (row) {
                    var userId = row.ID;
                }
            }, {
                id: 'btnSubmitUserRole',
                click: function (row) {
                    var userId = row.ID;
                    var roleIds = $('#dialogRole :checked').map(function (index, element) {
                        return $(element).val();
                    }).toArray().join(',');
                    Role.saveRolesByUserId(userId, roleIds, { modal: 'dialogRole' });
                }
            }]
        },
        success: function (src, data) {
            if (src === 'save' && data.ID === $('#userId').val()) {
                $('.username').text(data.DisplayName);
            }
        }
    });

    $('table').smartTable({
        url: '../api/Users',            //请求后台的URL（*）
        sortName: 'UserName',
        queryParams: function (params) { return $.extend(params, { name: $("#txt_search_name").val(), displayName: $('#txt_display_name').val() }); },           //传递参数（*）
        columns: [{ checkbox: true },
            { title: "Id", field: "ID", events: bsa.idEvents(), formatter: BootstrapAdmin.idFormatter },
            { title: "登陆名称", field: "UserName", sortable: true },
            { title: "显示名称", field: "DisplayName", sortable: false }
        ]
    });

    // validate
    $('#dataForm').autoValidate({
        userName: {
            required: true,
            maxlength: 50
        },
        password: {
            required: true,
            maxlength: 50
        },
        confirm: {
            required: true,
            equalTo: "#password"
        },
        displayName: {
            required: true,
            maxlength: 50
        }
    });
});