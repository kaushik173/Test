
$(function () {


    setInitialFormValues('CopyCaseTransferForm');
});

$('#btnSave').on('click', function () {

    Save();

});
$('#btnCancel').on('click', function () {

    document.location.href = '/CaseOpening/LegalParties?dataentry=true';


});



function Validation() {
    var isValid = true;





    if ($('#TransferToID').val() == '') {
        isValid = false;
        $('#TransferToID').focus();
        notifyDanger('Transfer To is required.');
        return false;
    }
    if ($('#TransferDate').val() == '') {
        isValid = false;
        $('#TransferDate').focus();
        notifyDanger('Transfer Date is required.');
        return false;
    }

    if (moment($('#TransferDate').val()) > moment()) {
        isValid = false;
        $('#TransferDate').focus();
        notifyDanger('Transfer Date can not be a future date.');
        return false;
    }
    if ($('.chkClient:checked').length == 0) {
        isValid = false;
        $('#TransferDate').focus();
        notifyDanger('At least one client must be checked.');
        return false;
    }
   
 

    return isValid;
}
function Save() {

    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return;
    }

    var isvalid = Validation();

    if (isvalid) {

        confirmBox("Are you sure you want to Copy Case Transfer this/these client(s)?", function (result) {
            if (result) {



                var ClientIds = '';
                $('.chkClient:checked').each(function () {
                    if (ClientIds != '')
                        ClientIds += ',';
                    ClientIds += $(this).attr('data-id');

                });
                var UncheckedClientIds = '';
                $('.chkClient').not(':checked').each(function () {
                    if (UncheckedClientIds != '')
                        UncheckedClientIds += ',';
                    UncheckedClientIds += $(this).attr('data-id');

                });
                var model = {
                    'TransferDate': $('#TransferDate').val(),
                    'TransferToID': $('#TransferToID').val(),

                    'IncludeClientPersonIDList': ClientIds,
                    'ExcludeClientPersonIDList': UncheckedClientIds


                }
                var params = model;
                $.ajax({
                    type: "POST", url: '/Case/CopyCaseTransfer', data: { model: params },
                    success: function (result) {

                        if (result.Status == "Done") {
                            RequestSubmitted();

                            notifySuccess('Data Saved Successfully!.');



                            document.location.href = '/CaseOpening/LegalParties?dataentry=true';


                        } else {
                            notifyDanger(result.Message);

                        }
                    },
                    dataType: 'json'
                });

            }
        });
    }

}
