function ConfirmDelete(uniqueId, isDeleteClicked) {
    var deletespan = 'DeleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'ConfirmDeleteSpan_' + uniqueId;

    if (isDeleteClicked) {
        $("#" + deletespan).hide();
        $("#" + confirmDeleteSpan).show();
    } else {
        $("#" + deletespan).show();
        $("#" + confirmDeleteSpan).hide();
    }
}