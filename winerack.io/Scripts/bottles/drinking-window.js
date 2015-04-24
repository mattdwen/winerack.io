function hideDrinkingWindowEditor() {
  $('#drinkingWindow_display').show();
  $('#drinkingWindow_edit').hide();
}

function showDrinkingWindowEditor() {
  $('#drinkingWindow_display').hide();
  $('#drinkingWindow_edit').show();
}

$(function () {
  $('#drinkingWindowEdit').click(function (e) {
    e.preventDefault();
    e.stopPropagation();
    showDrinkingWindowEditor();
  });

  $('#drinkingWindowCancel').click(function (e) {
    e.preventDefault();
    e.stopPropagation();
    hideDrinkingWindowEditor();
  });
});