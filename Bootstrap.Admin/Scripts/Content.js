$(function () {
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
});