
        function Validation() {

            var flag = true;
            var message;
            $('.required').each(function () {

                if ($(this).val() == '') {
                    if ($(this).parent().find('.control-label').text() == '')
                        message = $(this).parent().parent().find('.control-label').text();
                    else
                        message = $(this).parent().find('.control-label').text();
                    Notify(message + ' is required', 'bottom-right', '4000', 'danger', 'fa-warning', true);
                    $(this).focus();
                    flag = false;
                    return false;
                }

            });
            return flag;
        }
function SaveData() {
    //var data = $("#appealDecisionAddEdit-form").serialize();.
    debugger;
    var data = {
        'DecisionID': $('#DecisionID').val(),
        'EncryptedAppleaID': $('#EncryptedAppleaID').val(),
        'DecisionID': $('#DecisionID').val(),
        'AgencyID': $('#AgencyID').val(),
        'RecordStateID': $('#RecordStateID').val(),
        'DecisionCodeID': $('#DecisionCodeID').val(),
        'DecisionDate': $('#DecisionDate').val(),
    };
    
            
    $.ajax({
        type: "POST", dataType: 'json', url: '/WritsAppeals/AppealsDecisionAddEditSave', data: data,
        success: function (data) {
            if (data.IsSuccess) {
                window.location.href = "/WritsAppeals/List";
            } else {

            }
        }
    });
}

$('#btnSave').on('click', function () {
    IPadKeyboardFix();
    if (Validation())
        SaveData();
    else
        return false;
});
$('#btnCancel').on('click', function () {
    window.location.href = "/WritsAppeals/List";
});
     
$('body').on('click', '.delete', function () {
    var id = $(this).attr('data-id');
    var tr = $(this).parent().parent();
    confirmBox("Are you sure you want to remove selected records?", function (result) {
        if (result) {
            $.ajax({
                type: "POST", url: '/WritsAppeals/DecisionDelete/' + id,
                dataType: "json",
                success: function (data) {
                    window.location.href = "/WritsAppeals/List";
                   // tr.remove();
                    //Notify('Selected record delete successfully.', 'bottom-right', '5000', 'success', 'fa-check', true);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
            });
        }
    });
});
        

