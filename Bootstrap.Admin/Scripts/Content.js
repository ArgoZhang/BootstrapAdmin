$(function () {
    var fra = $('iframe').get(0);
    fra.height = fra.contentDocument.body.offsetHeight;
});