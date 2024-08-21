
$(function () {


    setInitialFormValues('AREdit');
});

$('#btnSave').on('click', function () {

    Save(1);

});
$('#btnSaveAndPrint').on('click', function () {

    Save(3);

});
$('#btnSaveExit').on('click', function () {
    Save(2);


});


var associations = [];

function Validation(buttonId) {
    var isValid = true;
    associations = [];

    if (!hasFormChanged('AREdit')) {
        if (buttonId == 1) {
            document.location.href = '/CaseOpening/CaseAddresses?dataentry=true&AR=true';
            isValid = false;
            return false;
        } else if (buttonId == 2) {
            document.location.href = '/CaseOpening/ActionRequest?dataentry=true';
            isValid = false;
            return false;
        } else if (buttonId == 3) {
            var data = {
                'id': $('#HearingReportFilingDueID').val(),
            }
           $.download($('#hdnCurrentSessionGuidPath').val()+'/CaseOpening/ActionRequestPrint/' + $('#HearingReportFilingDueID').val(), data, "POST");
            isValid = false;
            return false;


        }
        Notify('Nothing was changed.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        isValid = false;
        return false;
    }



    if ($('#RequestForID').val() == '') {
        isValid = false;
        $('#RequestForID').focus();
        Notify('Request For is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        return false;
    }
    if ($('#DueDate').val() == '') {
        isValid = false;
        $('#DueDate').focus();
        Notify('Due Date is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        return false;
    }






    return isValid;
}
function Save(buttonId) {


    IPadKeyboardFix();
    var isvalid = Validation(buttonId);

    if (isvalid) {


        var params = $('#AREdit').serialize();
        $.ajax({
            type: "POST", url: '/Case/EditAR', data: params,
            success: function (result) {

                if (result.Status == "Done") {

                    Notify('Data Saved Successfully!.', 'bottom-right', '3000', 'success', 'fa-smile-o', true);


                    if (buttonId == 1) {
                        document.location.href = '/CaseOpening/CaseAddresses?dataentry=true&AR=true' ;
                    } else if (buttonId == 2) {
                        document.location.href = '/CaseOpening/ActionRequest?dataentry=true';

                    }
                    else {
                        var data = {
                            'id': result.HearingReportFilingDueID,
                        }
                       $.download($('#hdnCurrentSessionGuidPath').val()+'/CaseOpening/ActionRequestPrint/' + result.HearingReportFilingDueID, data, "POST");

                    }
                } else {
                    document.location.href = result.URL;

                }
            },
            dataType: 'json'
        });
    }

}
