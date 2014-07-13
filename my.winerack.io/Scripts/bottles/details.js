$(function () {
    var hash = window.location.hash;
    hash && $('ul.nav a[href="' + hash + '"]').tab('show');
});

$('#cellarSlider').editRangeSlider({
    arrows: false,
    bounds: { min: 0, max: 20 },
    defaultValues: { min: $('#cellarMin').val(), max: $('#cellarMax').val() },
    step: 1
}).bind("valuesChanged", function (e, data) {
    $('#cellarMin').val(data.values.min);
    $('#cellarMax').val(data.values.max);
});

$('#cellarUpdate').click(function () {
    $.post('/api/bottles/cellar/' + $('#Bottle_ID').val(), {
        Min: $('#cellarMin').val(),
        Max: $('#cellarMax').val()
    });
});