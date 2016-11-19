$(function () {
    if ($.browser.versions.ios) $('#content').css({ 'overflow': 'auto', '-webkit-overflow-scrolling': 'touch' });
    function setActive(ele) {
        ele.addClass('active');
        ele.parents("ul").first().find('p').addClass('active');
        $breadNav.text(ele.text()).show();
    }
    $('#navbar').attr('data-toggle', "dropdown").addClass('dropdown-toggle');
    var $subMenu = $('#submenu');
    var $breadNav = $('#breadNav');
    $subMenu.on('click', 'a', function (event) {
        var $this = $(this);
        var act = $this.attr("data-act");
        if (act === "True") return true;
        event.preventDefault();
        $subMenu.find('p, a').removeClass('active');
        setActive($this);
        $('iframe').attr('src', $this.attr('href'));
    });
    $breadNav.hide();
    $subMenu.find('a').each(function (index, ele) {
        var $this = $(this);
        if ($this.attr('href') == window.location.pathname) setActive($this);
    });
});