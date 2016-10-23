$(function () {
    $.extend({
        "format": function (source, params) {
            if (params === undefined) {
                return source;
            }
            if (arguments.length > 2 && params.constructor !== Array) {
                params = $.makeArray(arguments).slice(1);
            }
            if (params.constructor !== Array) {
                params = [params];
            }
            $.each(params, function (i, n) {
                source = source.replace(new RegExp("\\{" + i + "\\}", "g"), function () {
                    return n;
                });
            });
            return source;
        }
    });

    var ajax = function (options) {
        options = $.extend({
            test: false,
            div: undefined,
            anonymous: false,
            title: '未录入',
            url: '',
            method: 'get',
            data: '',
            headers: {},
            success: function (result) {
            },
            error: function (textStatus, errorThrown) {
            }
        }, options);
        if (options.url == '') {
            document.writeln("请求地址丢失！");
            return;
        }
        $.ajax({
            url: 'http://localhost:53233/' + options.url,
            data: options.data,
            type: options.method,
            headers: options.headers,
            success: function (result) {
                if (!options.test) {
                    var $body = $('#main-content');
                    $body.append('<section></section>');
                    var $section = $body.find('section').last();
                    $section.append($.format('<h3>{0}</h3>', options.title));
                    $section.append($.format('<div><label class="control-label">请求地址</label><div>{0}</div></div>', options.url));
                    $section.append($.format('<div><label class="control-label">允许匿名</label><div>{0}</div></div>', options.anonymous ? "允许" : "不允许"));
                    $section.append($.format('<div><label class="control-label">Headers</label><div>{0}</div></div>', JSON.stringify(options.headers)));
                    $section.append($.format('<div><label class="control-label">HTTP方法</label><div>{0}</div></div>', options.method));
                    $section.append($.format('<div><label class="control-label">传递参数</label><div>{0}</div></div>', JSON.stringify(options.data)));
                    $section.append($.format('<div><label class="control-label">返回值</label><div>{0}</div></div>', JSON.stringify(result)));
                    $section.append('<div><label class="control-label">试一试</label></div>');
                    $section.append($.format('<div class="input-group"><input type="text" class="form-control" value=\'{1}\' data-val=\'{0}\' /><span class="input-group-btn"><button class="btn btn-default btn-success" type="button">试一试</button></span></div>', JSON.stringify(options), JSON.stringify(options.data)));
                    $section.append('<div class="test"></div>')
                    options.success(result);
                }
                else {
                    if (options.div) {
                        options.div.html($.format('<label class="control-label">测试结果：</label><div>{0}</div>', JSON.stringify(result))).show();
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                var $body = $('body');
                var $section = $body.find('section').last();
                $section.append($.format('<div><label class="control-label">错误参数</label><div>textStatus:{0}</div><div>', textStatus));
                $section.append($.format('<div><label class="control-label">错误参数</label><div>errorThrown:{0}</div><div>', errorThrown));
                options.error(textStatus, errorThrown);
            }
        });
    };

    $('body').on('click', 'button', function () {
        var $input = $(this).parent().prev();
        var options = $.extend(JSON.parse($input.attr('data-val')), { data: JSON.parse($input.val()) }, { test: true, div: $(this).parent().parent().next() });
        ajax(options);
    });

    // 接口调用
    ajax({
        title: '用户登陆调用接口',
        anonymous: true,
        url: 'api/login', method: 'POST', data: { userName: 'Test', password: '1' }, success: function (result) {
            var token = result.Token;
            ajax({
                title: '用户登陆信息接口', url: 'api/login', headers: { "Token": token },
                method: 'GET'
            });
            ajax({
                title: '指定用户信息接口', url: 'api/Users', headers: { Token: token },
                method: 'GET', data: { userName: "Test" }
            });
            ajax({
                title: '检查当前用户名是否可用接口', url: 'api/Users', headers: { Token: token },
                method: 'PUT', data: { UserName: "101", UserStatus: 9 }
            });
            ajax({
                title: '更改用户显示名称接口', url: 'api/Users', headers: { Token: token },
                method: 'PUT', data: { UserName: "101", UserStatus: 1, DisplayName: "1010" }
            });
            ajax({
                title: '更改用户密码接口', url: 'api/Users', headers: { Token: token },
                method: 'PUT', data: { UserName: "101", UserStatus: 2, Password: "1", NewPassword: "2" }
            });
            ajax({
                title: '新建用户接口', url: 'api/Users', headers: { Token: token },
                method: 'POST', data: { ID: 0, UserName: "102", Password: "1", DisplayName: "102" }
            });
            ajax({
                title: '更新用户接口', url: 'api/Users', headers: { Token: token },
                method: 'POST', data: { ID: 21, UserName: "102", Password: "1", DisplayName: "102" }
            });
            ajax({
                title: '删除用户接口', url: 'api/Users', headers: { Token: token },
                method: 'DELETE', data: { "": "50,51" }
            });
        }
    });
});