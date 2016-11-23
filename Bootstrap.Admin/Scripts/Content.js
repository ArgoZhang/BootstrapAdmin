$(function () {
    $('#navbar').attr('data-toggle', "dropdown").addClass('dropdown-toggle');
    var $subMenu = $('#submenu');
    var $breadNav = $('#breadNav');
    $breadNav.hide();
    $subMenu.on('click', 'a', function () {
        var $this = $(this);
        var href = $this.attr('href');
        var tail = new Date().format("HHmmss");
        if (href.indexOf('?') > -1) href += '&ba=' + tail;
        else href += '?ba=' + tail;
        window.location.href = href;
        return false;
    });
});