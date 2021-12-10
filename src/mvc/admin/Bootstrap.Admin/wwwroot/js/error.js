$(function () {
    $.fn.extend({
        autoCenter: function () {
            var that = this;
            var getHeight = function () {
                return Math.max(0, ($(window).height() - that.outerHeight()) / 2 + $(document).scrollTop());
            };
            $(window).resize(function () {
                that.css({ marginTop: getHeight() });
            });
            that.css({ marginTop: getHeight(), transition: "all .5s linear" });
        }
    });
    $('.error-wrapper').autoCenter();
});