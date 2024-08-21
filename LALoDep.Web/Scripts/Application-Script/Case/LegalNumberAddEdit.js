
function saveData(buttonID) {
    IPadKeyboardFix();
    if (!IsValidFormRequest()) {
        return false;
    }

    if (Validation()) {
        if (!hasFormChanged('legalNumberAddEdit-form')) {

            if (buttonID == 1 && $PageID == 1) {
                window.location.href = $BaseURL + 'Users/PersonLegalNumbers/' + $('#hdnPersonID').val();
                return false;
            } else if (buttonID == 2 && $PageID == 1) {
                window.location.href = $BaseURL + 'Case/LegalNumberAdd/' + $('#hdnPersonID').val() + '?PageID=' + $('#hdnPageID').val();
                return false;
            } else if (buttonID == 1 && $PageID == "") {
                window.location.href = $BaseURL + 'Case/LegalNumber';
                return false;
            }
            else if (buttonID == 2 && $PageID == "") {
                window.location.href = $BaseURL + 'Case/LegalNumberAdd/' + $('#hdnPersonID').val();
                return false;
            }

            notifyDanger('Nothing was changed.');

            return false;
        }
        var data = {
            'PersonID': $('#PersonID').val(),
            'AgencyID': $('#AgencyID').val(),
            'LegalNumberID': $('#LegalNumberID').val(),
            'LegalNumberEntry': $('#LegalNumberEntry').val(),
            'LegalNumberTypeCodeID': $('#LegalNumberTypeCodeID').val(),
            'LegalNumberComment': $('#LegalNumberComment').val(),
            'RecordStateID': $('#RecordStateID').val(),

        };
        $.ajax({
            type: "POST", url: '/Case/LegalNumberAddEdit', data: data,
            success: function (result) {
                
                if (result.isSuccess) {
                    RequestSubmitted();
                    if (buttonID == 1 && $PageID == 1)
                        window.location.href = $BaseURL + 'Users/PersonLegalNumbers/' + $('#hdnPersonID').val();
                    else if (buttonID == 2 && $PageID == 1)
                        window.location.href = $BaseURL + 'Case/LegalNumberAdd/' + $('#hdnPersonID').val() + '?PageID=' + $('#hdnPageID').val();
                    else if (buttonID == 1 && $PageID == "")
                        window.location.href = $BaseURL + 'Case/LegalNumber';
                    else if (buttonID == 2 && $PageID == "")
                        window.location.href = $BaseURL + 'Case/LegalNumberAdd/' + $('#hdnPersonID').val();
                } else {
                    notifyDanger(result.ErrorMessage);
                    $('#LegalNumberEntry').focus();
                }
            },
            dataType: 'json'
        });


    }
    else {
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
    saveData(1);
});
$('#btnSaveAndAdd').on("click", function (e) {
    saveData(2);
});
$('#btnCancel').on("click", function (e) {
    if ($PageID == 1)
        window.location.href = $BaseURL + 'Users/PersonLegalNumbers/' + $('#hdnPersonID').val();
    else
        window.location.href = $BaseURL + 'Case/LegalNumber';
});

setInitialFormValues('legalNumberAddEdit-form', true);