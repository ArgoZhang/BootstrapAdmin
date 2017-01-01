(function () {
    (function ($) {
        return $.fn.ajaxLoadingBar = function (options) {
            var settings;
            settings = $.extend({
                turbolinks: true,
                ajax: true
            }, options);
            if (settings.turbolinks) {
                $(document).on('page:fetch', function () {
                    return window.pageLoader.startLoader();
                });
                $(document).on('page:receive', function () {
                    return window.pageLoader.sliderWidth = $('#pageLoader').width();
                });
                $(document).on('page:load', function () {
                    return window.pageLoader.restoreLoader();
                });
                $(document).on('page:restore', function () {
                    $('#pageLoader').remove();
                    return window.pageLoader.restoreLoader();
                });
            }
            if (settings.ajax) {
                $(document).ajaxComplete(function (e) {
                    $('#pageLoader').remove();
                    return window.pageLoader.restoreLoader();
                });
                $(document).ajaxStart(function () {
                    return window.pageLoader.startLoader();
                });
            }
            return window.pageLoader = {
                sliderWidth: 0,
                startLoader: function () {
                    $('#pageLoader').remove();
                    return $('<div/>', {
                        id: 'pageLoader'
                    }).appendTo('body').animate({
                        width: $(document).width() * .4
                    }, 2000).animate({
                        width: $(document).width() * .6
                    }, 6000).animate({
                        width: $(document).width() * .90
                    }, 10000).animate({
                        width: $(document).width() * .99
                    }, 20000);
                },
                restoreLoader: function () {
                    return $('<div/>', {
                        id: 'pageLoader'
                    }).css({
                        width: window.pageLoader.sliderWidth
                    }).appendTo('body').animate({
                        width: $(document).width()
                    }, 500).fadeOut(function () {
                        return $(this).remove();
                    });
                }
            };
        };
    })(jQuery);
    $(window).ajaxLoadingBar();
}).call(this);