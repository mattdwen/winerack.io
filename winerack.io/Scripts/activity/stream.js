$(function () {
  var $stream = $('.activity-stream');
  $stream.imagesLoaded(function () {
    $stream.masonry({
      itemSelector: '.activity-event'
    });
  });
});