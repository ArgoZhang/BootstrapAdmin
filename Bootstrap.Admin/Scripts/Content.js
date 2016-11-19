$(function () {
    $('#navbar').attr('data-toggle', "dropdown").addClass('dropdown-toggle');
    function iframeResposive() {
        try {
            var fra = $('iframe').get(0);
            fra.height = fra.contentDocument.body.offsetHeight;
        }
        catch (e) {
        }
    }
    $(window).on('load', iframeResposive);
    $(window).on('resize', iframeResposive);

    var $subMenu = $('#submenu');
    $subMenu.on('click', 'a', function (event) {
        var $this = $(this);
        var act = $this.attr("data-act");
        if (act === "True") return true;
        event.preventDefault();
        $subMenu.find('p, a').removeClass('active');
        setActive($this);
        $('iframe').attr('src', $this.attr('href'));
    });
    $subMenu.find('a').each(function (index, ele) {
        var $this = $(this);
        if ($this.attr('href') == window.location.pathname) setActive($this);
    });
    function setActive(ele) {
        ele.addClass('active');
        ele.parents("ul").first().find('p').addClass('active');
    }
});