(function ($) {
    // sparkline component
    'use strict';

    var lgbSparkline = function (element, options) {
        this.$element = $(element);
        this.options = $.extend({}, lgbSparkline.DEFAULTS, options);
        this.init();
    };

    lgbSparkline.VERSION = '1.0';
    lgbSparkline.Author = 'argo@163.com';
    lgbSparkline.DataKey = "lgb.sparkline";
    lgbSparkline.Template = '<span class="d-inline-block tooltipped tooltipped-s" aria-label="Past year of activity">';
    lgbSparkline.Template += '<svg width="155" height="30">';
    lgbSparkline.Template += '    <defs>';
    lgbSparkline.Template += '        <linearGradient id="{0}" x1="0" x2="0" y1="1" y2="0">';
    lgbSparkline.Template += '            <stop offset="10%" stop-color="#c6e48b"></stop>';
    lgbSparkline.Template += '            <stop offset="33%" stop-color="#7bc96f"></stop>';
    lgbSparkline.Template += '            <stop offset="66%" stop-color="#239a3b"></stop>';
    lgbSparkline.Template += '            <stop offset="90%" stop-color="#196127"></stop>';
    lgbSparkline.Template += '        </linearGradient>';
    lgbSparkline.Template += '        <mask id="{1}" x="0" y="0" width="155" height="28">';
    lgbSparkline.Template += '            <polyline transform="translate(0, 28) scale(1,-1)" points="" fill="transparent" stroke="#8cc665" stroke-width="2">';
    lgbSparkline.Template += '            </polyline>';
    lgbSparkline.Template += '        </mask>';
    lgbSparkline.Template += '    </defs>';
    lgbSparkline.Template += '    <g transform="translate(0, 2.0)">';
    lgbSparkline.Template += '        <rect x="0" y="-2" width="155" height="30" style="stroke: none; fill: url(#{0}); mask: url(#{1})"></rect>';
    lgbSparkline.Template += '    </g>';
    lgbSparkline.Template += '</svg>';
    lgbSparkline.Template += '</span>';

    lgbSparkline.AllowMethods = /val/;

    function Plugin(option) {
        var params = $.makeArray(arguments).slice(1);
        return this.each(function () {
            var $this = $(this);
            var data = $this.data(lgbSparkline.DataKey);
            var options = typeof option === 'object' && option;

            if (!data) $this.data(lgbSparkline.DataKey, data = new lgbSparkline(this, options));
            if (!lgbSparkline.AllowMethods.test(option)) return;
            if (typeof option === 'string') {
                data[option].apply(data, params);
            }
        });
    }

    $.fn.lgbSparkline = Plugin;
    $.fn.lgbSparkline.Constructor = lgbSparkline;

    var _proto = lgbSparkline.prototype;
    _proto.init = function () {
        var getUID = function (prefix) {
            if (!prefix) prefix = 'lgb';
            do prefix += ~~(Math.random() * 1000000);
            while (document.getElementById(prefix));
            return prefix;
        };

        var that = this;
        var element = $.format(lgbSparkline.Template, getUID('gradient'), getUID('spark'))
        this.$ctl = $(element).appendTo(this.$element);
    };
    _proto.val = function (val) {
        this.$ctl.find('polyline').attr('points', val);
    };
})(jQuery);

$(function () {
    $.bc({
        url: "api/Analyse?logType=LoginUsers",
        callback: function (result) {
            $('#login').lgbSparkline('val', result.Polylines.join(' '));
            var op = $.extend(true, {}, option, {
                legend: { data: ["登录数量"] },
                series: [
                    {
                        name: "登录数量",
                        data: result.Datas.map(function (val, index) {
                            return val.Value;
                        })
                    }
                ],
                xAxis: [
                    {
                        data: result.Datas.map(function (val, index) {
                            return val.Key;
                        })
                    }
                ],
                dataZoom: {
                    start: 50
                }
            });
            var loginChart = echarts.init(document.getElementById('loginChart'));
            $('#loginLoading').addClass('d-none');
            loginChart.setOption(op);
        }
    });
    $.bc({
        url: "api/Analyse?logType=log",
        callback: function (result) {
            $('#log').lgbSparkline('val', result.Polylines.join(' '));
            var op = $.extend(true, {}, option, {
                legend: { data: ["操作数量"] },
                series: [
                    {
                        name: "操作数量",
                        data: result.Datas.map(function (val, index) {
                            return val.Value;
                        })
                    }
                ],
                xAxis: [
                    {
                        data: result.Datas.map(function (val, index) {
                            return val.Key;
                        })
                    }
                ]
            });
            var logChart = echarts.init(document.getElementById('logChart'));
            $('#logLoading').addClass('d-none');
            logChart.setOption(op);
        }
    });
    $.bc({
        url: "api/Analyse?logType=trace",
        callback: function (result) {
            $('#trace').lgbSparkline('val', result.Polylines.join(' '));
            var op = $.extend(true, {}, option, {
                legend: { data: ["访问数量"] },
                series: [
                    {
                        name: "访问数量",
                        data: result.Datas.map(function (val, index) {
                            return val.Value;
                        })
                    }
                ],
                xAxis: [
                    {
                        data: result.Datas.map(function (val, index) {
                            return val.Key;
                        })
                    }
                ]
            });
            var traceChart = echarts.init(document.getElementById('traceChart'));
            $('#traceLoading').addClass('d-none');
            traceChart.setOption(op);
        }
    });

    var option = {
        title: {
            text: '',
            subtext: ''
        },
        tooltip: {
            trigger: 'axis'
        },
        legend: {
            data: ['日访问量']
        },
        toolbox: {
            show: true,
            feature: {
                mark: { show: true },
                dataZoom: { show: true },
                dataView: { show: false },
                magicType: { show: true, type: ['line', 'bar'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        calculable: true,
        dataZoom: {
            show: true,
            realtime: true,
            start: 0,
            end: 100
        },
        xAxis: [
            {
                type: 'category',
                boundaryGap: false,
                data: function () {
                    var list = [];
                    for (var i = 1; i <= 30; i++) {
                        list.push('2013-03-' + i);
                    }
                    return list;
                }()
            }
        ],
        yAxis: [
            {
                type: 'value'
            }
        ],
        series: [
            {
                name: '日访问量',
                type: 'line',
                data: function () {
                    var list = [];
                    for (var i = 1; i <= 30; i++) {
                        list.push(Math.round(Math.random() * 30));
                    }
                    return list;
                }()
            }
        ]
    };
});