var oldModelData;
$(function () {
    if ($('#Hours').is(':disabled')) {
        $('#Hours').val('0')
    }


    setInitialFormValues('quick-ar-form', true);
    $('input:checkbox').removeAttr('value')
    
});

$("input#CompletedDate").on("blur", function () {
    if ($(this).val() == "") {
        $("#Completed").val(false);
    }
});

$('body').on('change', 'input#CompletedDate', function (ev) {
    if ($(this).val() == "") {
     
        $("#Completed").val('false');
    }
    else {
     
        $("#Completed").val('true');
    }
});

$("#btnRequestNote").on("click", function () {
    var note = '<div class="row padding-bottom-10">' + $(this).data("note") + '</div>';
    OpenModal(note, "Request Note");
});

$('#btnSave').on('click', function () {

    Save(1);

});
$('#btnPrint').on('click', function () {

    Save(2);

});
$('#btnExit').on('click', function () {
    Save(3);


});


$("#RecordTimeTypeID").on("change", function () {
    if ($("select#WorkIVeEligibleCodeID").length > 0) {
        $("select#WorkIVeEligibleCodeID").val($(this).find('option:selected').attr('data-default-ive'))
        if ($(this).find('option:selected').attr('data-default-ive') > 0 && $(this).find('option:selected').attr('data-default-ive-can-change') == 0) {
            $("select#WorkIVeEligibleCodeID").prop('disabled', true)
        } else {
            $("select#WorkIVeEligibleCodeID").prop('disabled', false)
        }
    }
});
$('.chk300').attr('name', 'chk300');
$('.chkRT').attr('name', 'chkRT');
$('.chkDual').attr('name', 'chkDual');
$('.chk300').on('click', function () {
    $('input[name="' + $(this).attr('id') + '"]:hidden').val($(this).is(':checked'));
});
$('.chkRT').on('click', function () {
    $('input[name="' + $(this).attr('id') + '"]:hidden').val($(this).is(':checked'));
});
$('.chkDual').on('click', function () {
    $('input[name="' + $(this).attr('id') + '"]:hidden').val($(this).is(':checked'));
});

var associations = [];

function Validation(buttonId) {
    var isValid = true;
    associations = [];

    if (!hasFormChanged('quick-ar-form')) {
         if (buttonId == 3) {
            if ($exitUrl.length > 0) {
                document.location.href = $exitUrl;
                return false;
            }
            isValid = false;
            return false;
        } else if (buttonId == 2) {
            Print();
            isValid = false;
            return false;


        }
        notifyDanger('Nothing was changed.');
        isValid = false;
        return false;
    }
    var isRoleAdded = false;

    if ($('.newrolefield:text').val().length > 0 || $('select.newrolefield').val().length > 0 || $('.newrolefield:checkbox').is(':checked')) {
        if ($('#AssociationTypeCodeID').val() == '') {
            isValid = false;
            $('#AssociationTypeCodeID').focus();
            notifyDanger('Association/Role is required.');
            return false;
        } if ($('#LastName').val() == '') {
            isValid = false;
            $('#LastName').focus();
            notifyDanger('Last Name is required.');
            return false;
        } if ($('#FirstName').val() == '') {
            isValid = false;
            $('#FirstName').focus();
            notifyDanger('First Name is required.');
            return false;
        } if ($('#RoleDate').val() == '') {
            isValid = false;
            $('#RoleDate').focus();
            notifyDanger('Role Date is required.');
            return false;
        }
        isRoleAdded = true;

    }
    if ($('#PlacementAgencyID').val() > 0) {
        if (isRoleAdded) {
            isValid = false;
            $('#RoleDate').focus();
            notifyDanger('Enter Association,role, name and/or address information OR a placement address. Not both.');
            return false;
        }
        if ($('#StartDate').val() == '') {
            isValid = false;
            $('#StartDate').focus();
            notifyDanger('Start Date is required');
            return false;

        }
        isRoleAdded = true;
    }

    
    if (isRoleAdded && $('.chk300').length == 0) {
        isValid = false;

        notifyDanger('All petitions are closed, role cannot be added');
        return false;
    }
    if ($('.recordtimefield').val().length > 0 || $('.chkRT:checked').length > 0) {
        if ($('#RecordTimeTypeID').val() == '') {
            isValid = false;
            $('#RecordTimeTypeID').focus();
            notifyDanger('Record Time Type is required');
            return false;

        } if ($('#PhaseID').val() == '') {
            isValid = false;
            $('#PhaseID').focus();
            notifyDanger('Phase is required');
            return false;

        } if ($('#Hours').val() == '') {
            isValid = false;
            $('#Hours').focus();
            notifyDanger('Hours is required');
            return false;

        } if ($('#RecordDate').val() == '') {
            isValid = false;
            $('#RecordDate').focus();
            notifyDanger('Record Date is required');
            return false;

        } 
          if ($("select#WorkIVeEligibleCodeID").val() == "" && $("select#WorkIVeEligibleCodeID").length > 0) {
            notifyDanger('IV-E Eligible is required.');
            $("select#WorkIVeEligibleCodeID").focus();
            return false;
        }
        if ($('.chkRT:checked').length == 0) {
            isValid = false;
            $('.chkRT:first').focus();
            notifyDanger('At least one client is required when recording time (RT checkbox)');
            return false;

        }


    }

    if (isRoleAdded && $('.chk300:checked').length == 0) {
        isValid = false;
        $('.chkRT:first').focus();
        notifyDanger('At least one petition is required to add this role');
        return false;
    }
   

    if (moment($('#CompletedDate').val()) > moment()) {
        isValid = false;

        notifyDanger('Completion date must be today');
        return false;
    }
    if (moment($('#CompletedDate').val()) < moment($('.RequestDate').text())) {
        isValid = false;

        notifyDanger('Completion date can be before request date');
        return false;
    }
    
    return isValid;
}

function serializeData($form) {
    var $disabledFields = $form.find('[disabled]');
    $disabledFields.prop('disabled', false); // enable fields so they are included


    //  var data = $form.serialize();
    var data = {};
    var unindexed_array = $form.serializeArray();
    $.map(unindexed_array, function (n, i) {

        data[n['name']] = $('#' + n['name']).GetHtmlWithEscape();
    });
    
    $disabledFields.prop('disabled', true); // back to disabled
    return data;
}
function Save(buttonId) {

    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return;
    }


    var isvalid = Validation(buttonId);

    if (isvalid) {
        $('input:checkbox:checked').attr('value','true')


        var params =serializeData( $('#quick-ar-form'));
        $.ajax({
            type: "POST", url: '/Case/QuickAR', data: params,
            success: function (result) {

                if (result.Status == "Done") {

                    notifySuccess('Data Saved Successfully!.');

                    if (buttonId == 1) {
                        RequestSubmitted();
                        document.location.href = document.location.href;
                    } else if (buttonId == 3) {
                        RequestSubmitted();
                        if ($exitUrl.length > 0) {
                            document.location.href = $exitUrl;
                            return false;
                        }

                    }
                    else {
                        Print();

                        RequestSubmitted();
                        setTimeout(function () {
                            document.location.href = document.location.href;
                        }, 2000)

                    }

                } else {
                    document.location.href = result.URL;

                }
            },
            dataType: 'json'
        });
    }

}
function Print() {

    var data = {
        'id': $('#EncryptHearingReportFilingDueID').val(),
    }

   $.download($('#hdnCurrentSessionGuidPath').val()+'/CaseOpening/ActionRequestPrint/' + $('#EncryptHearingReportFilingDueID').val(), data, "POST", 'target="_blank"');

}