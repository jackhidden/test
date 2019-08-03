$(document).ready(function () {

    //var url = window.location;
    //// Will only work if string in href matches with location
    //$('ul.sidebar-menu a[href="' + url + '"]').parent().addClass('active');

    //// Will also work for relative and absolute hrefs
    //$('ul.sidebar-menu a').filter(function () {
    //    return this.href == url;
    //}).parent().addClass('active');

    //// Will also work for relative and absolute hrefs
    //$('ul.sidebar-menu a').filter(function () {
    //    var regex = new RegExp('\\b' + this.href + '\\b');
    //    return url.href.search(regex) !== -1;
    //}).parent().addClass('active');

    var url = window.location.href;
    // for sidebar menu entirely but not cover treeview
    $('ul.sidebar-menu a').filter(function () {
        return this.href == url;
    }).parent().addClass('active');
    // for treeview
    $('ul.treeview-menu a').filter(function () {
        return this.href == url;
    }).closest('.treeview').addClass('active');

});