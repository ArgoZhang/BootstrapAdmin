/*---LEFT BAR ACCORDION----*/
$(function () {
    $('#nav-accordion').dcAccordion({
        eventType: 'click',
        autoClose: true,
        saveState: true,
        disableLink: true,
        speed: 'slow',
        showCount: false,
        autoExpand: true,
        //        cookie: 'dcjq-accordion-1',
        classExpand: 'dcjq-current-parent'
    });
});

(function ($) {
    //    sidebar dropdown menu auto scrolling
    $('#sidebar .sub-menu > a').click(function () {
        var o = ($(this).offset());
        diff = 300 - o.top;
        if (diff > 0)
            $("#sidebar").scrollTo("-=" + Math.abs(diff), 500);
        else
            $("#sidebar").scrollTo("+=" + Math.abs(diff), 500);
    });

    $('.sidebar-toggle-box').click(function () {
        if ($('#sidebar').is(":visible") === true) {
            $(".sidebar").addClass("sidebar-closed");
        } else {
            $(".sidebar").removeClass("sidebar-closed");
            $("#sidebar").show();
        }
    });

    // custom scrollbar
    $("#sidebar").niceScroll({ styler: "fb", cursorcolor: "#e8403f", cursorwidth: '3', cursorborderradius: '10px', background: '#404040', spacebarenabled: false, cursorborder: '', scrollspeed: 60 });

    //$("html").niceScroll({styler:"fb",cursorcolor:"#e8403f", cursorwidth: '6', cursorborderradius: '10px', background: '#404040', spacebarenabled:false,  cursorborder: '', zindex: '1000', scrollspeed: 100, mousescrollstep: 60});

    //$(".table-responsive").niceScroll({ styler: "fb", cursorcolor: "#e8403f", cursorwidth: '6', cursorborderradius: '10px', background: '#404040', spacebarenabled: false, cursorborder: '', zindex: '1000', horizrailenabled: true });
})(jQuery);