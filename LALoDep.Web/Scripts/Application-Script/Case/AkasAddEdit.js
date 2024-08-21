var $BaseURL = '/';
setInitialFormValues('AKAsAddEdit-form', true);

function saveData() {
    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return false;
    }

    if (Validation()) {
        if (!hasFormChanged('AKAsAddEdit-form') && $('#PersonID').val() > 0) {
            notifyDanger('Nothing was changed.');
            return false;
        }
        var data = {
            'PersonID': $('#PersonID').val(),
            'AgencyID': $('#AgencyID').val(),
            'PersonNameID': $('#PersonNameID').val(),
            'PersonNameFirst': $('#PersonNameFirst').val(),
            'PersonNameLast': $('#PersonNameLast').val(),
            'PersonNameTypeCodeID': $('#PersonNameTypeCodeID').val(),
            'RecordStateID': $('#RecordStateID').val(),
        };
        $.ajax({
            type: "POST",
            url: '/Case/AKAsAddEditSave',
            data: data,
            success: function (result) {
                if (result.isSuccess) {
                    RequestSubmitted();
                    window.location.href = $BaseURL + 'Case/AKAs';
                }
            },
            dataType: 'json'
        });
    } else {
        return false;
    }
}

function Validation() {

    var flag = true;
    var message;
    $('.required').each(function () {

        if ($(this).val() == '') {
            if ($(this).parent().find('.control-label').text() == '')
                message = $(this).parent().parent().find('.control-label').text();
            else
                message = $(this).parent().find('.control-label').text();
            notifyDanger(message + ' is required');
            $(this).focus();
            flag = false;
            return false;
        }

    });
    return flag;
}

$('#btnSave').on("click", function (e) {

    saveData();
});

$('#btnCancel').on("click", function (e) {
    window.location.href = $BaseURL + 'Case/AKAs';
});