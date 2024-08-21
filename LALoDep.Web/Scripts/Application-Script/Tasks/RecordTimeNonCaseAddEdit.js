
//$("#WorkDescriptionCodeID").on("change", function () {
//    //if ($("select#WorkIVeEligibleCodeID").length > 0) {
//    //    $("select#WorkIVeEligibleCodeID").val($(this).find('option:selected').attr('data-default-ive'))
//    //}
//    if ($("select#WorkIVeEligibleCodeID").length > 0) {
//        $("select#WorkIVeEligibleCodeID").val($(this).find('option:selected').attr('data-default-ive'))
//        if ($(this).find('option:selected').attr('data-default-ive') > 0 && $(this).find('option:selected').attr('data-default-ive-can-change')==0) {

//            $("select#WorkIVeEligibleCodeID").prop('disabled', true)
//        } else {
//            $("select#WorkIVeEligibleCodeID").prop('disabled', false)
//        }

//    } else {
//        $("select#WorkIVeEligibleCodeID").prop('disabled', false)
//    }
//});

//if ($("select#WorkIVeEligibleCodeID").length > 0) {
//    if ($("#WorkDescriptionCodeID").find('option:selected').attr('data-default-ive') > 0 && $("#WorkDescriptionCodeID").find('option:selected').attr('data-default-ive-can-change') == 0) {
//        $("select#WorkIVeEligibleCodeID").prop('disabled', true)
//    } else {
//        $("select#WorkIVeEligibleCodeID").prop('disabled', false)
//    }
//}
$("#WorkDescriptionCodeID").on("change", function () {

    WorkIVeEligibleCodeDisplay(false);
});
function WorkIVeEligibleCodeDisplay(onload) {
    var isattorney = 0;
    isattorney = $('#PersonID').find('option:selected').attr('data-isattorneyflag');
    var issupervisor = 0;
    issupervisor = $('#PersonID').find('option:selected').attr('data-issupervisorflag');

    if ($("select#WorkIVeEligibleCodeID").length > 0 && issupervisor == '1') {
        if (!onload)
            $("select#WorkIVeEligibleCodeID").val($('#WorkDescriptionCodeID').find('option:selected').attr('data-supervisor-default-ive'))
        if ($('#WorkDescriptionCodeID').find('option:selected').attr('data-supervisor-default-ive') > 0 && $('#WorkDescriptionCodeID').find('option:selected').attr('data-supervisor-default-ive-can-change') == 0) {
            $("select#WorkIVeEligibleCodeID").prop('disabled', true)
        } else {
            $("select#WorkIVeEligibleCodeID").prop('disabled', false)
        }
    } else if ($("select#WorkIVeEligibleCodeID").length > 0 && isattorney == '1') {
        if (!onload)
            $("select#WorkIVeEligibleCodeID").val($('#WorkDescriptionCodeID').find('option:selected').attr('data-attorny-default-ive'))
        if ($('#WorkDescriptionCodeID').find('option:selected').attr('data-attorny-default-ive') > 0 && $('#WorkDescriptionCodeID').find('option:selected').attr('data-attorny-default-ive-can-change') == 0) {
            $("select#WorkIVeEligibleCodeID").prop('disabled', true)
        } else {
            $("select#WorkIVeEligibleCodeID").prop('disabled', false)
        }
    } else if ($("select#WorkIVeEligibleCodeID").length > 0) {
        if (!onload)
            $("select#WorkIVeEligibleCodeID").val($('#WorkDescriptionCodeID').find('option:selected').attr('data-default-ive'))
        if ($('#WorkDescriptionCodeID').find('option:selected').attr('data-default-ive') > 0 && $('#WorkDescriptionCodeID').find('option:selected').attr('data-default-ive-can-change') == 0) {
            $("select#WorkIVeEligibleCodeID").prop('disabled', true)
        } else {
            $("select#WorkIVeEligibleCodeID").prop('disabled', false)
        }
    } else {
        $("select#WorkIVeEligibleCodeID").prop('disabled', false)
    }
}
$(".ddlStaff").on("change", function () {
    WorkIVeEligibleCodeDisplay(false);
});
WorkIVeEligibleCodeDisplay(true);
var oldModelData;
function validation() {
    var closedflag = true;
    var workForTr = $("#workedForList tbody tr");
    for (var indx = 0; indx < workForTr.length; indx++) {
        $tr = workForTr.eq(indx);

        if ($tr.find("input.chkWork").is(":checked") && $tr.data("allmainpetitionsclosedflag") == 1) {
            closedflag = false;
            break;
        }
    }
    return closedflag;
}

function getData() {
    var data = {
        WorkID: $("#WorkID").val(),
        AgencyID: $("#AgencyID").val(),
        HearingID: $("#HearingID").val(),
        WorkHoursOverTime: $("#WorkHoursOverTime").val(),
        RecordStateID: $("#RecordStateID").val(),

        PersonID: $("#PersonID").val(),
        WorkDescriptionCodeID: $("#WorkDescriptionCodeID").val(),
        WorkPhaseCodeID: $("#WorkPhaseCodeID").val(),

        WorkHours: $("#WorkHours").val(),
        WorkMileage: $("#WorkMileage").val(),
        WorkStartDate: $("#WorkStartDate").val(),
        WorkEndDate: $("#WorkEndDate").val(),

        NoteEntry: $("#NoteEntry").GetHtml(),
        NoteID: $("#NoteID").val(),
        NoteAgencyID: $("#NoteAgencyID").val(),
        NoteEntityCodeID: $("#NoteEntityCodeID").val(),
        NoteEntityTypeCodeID: $("#NoteEntityTypeCodeID").val(),
        NoteEntityPrimaryKeyID: $("#NoteEntityPrimaryKeyID").val(),
        NoteTypeCodeID: $("#NoteTypeCodeID").val(),
        NoteSubject: $("#NoteSubject").val(),
        NotePetitionID: $("#NotePetitionID").val(),
        NoteHearingID: $("#NoteHearingID").val(),
        NoteRecordStateID: $("#NoteRecordStateID").val(),
        WorkIVeEligibleCodeID: $('#WorkIVeEligibleCodeID').val(),
        CountyID: $('#CountyID').val(),


    }




    return data;
}

function ajaxREquestForSave(data, buttonId) {
    var ajaxUrl = '/Task/RecordTimeNonCaseAddEditSave';



    $.ajax({
        type: "POST", dataType: 'json', url: ajaxUrl, data: JSON.stringify({ 'viewModel': data, 'oldViewModel': oldModelData }), contentType: "application/json",
        success: function (result) {
            RequestSubmitted();
            if (result.isSuccess) {

                if ($('#RedirectToPage').val() !== '') {
                    window.location.href = $('#RedirectToPage').val();

                }
                else if (result.URL != '') {
                    window.location.href = result.URL;
                }
            }
            else {
                notifyDanger('There is something wrong while processing request.');
            }
        }
    });
}

function SaveData(buttonId) {
    IPadKeyboardFix();
    if (!IsValidFormRequest()) {
        return false;
    }

    if (!hasFormChanged('recordTimeEdit-form')) {
        if (buttonId == 3) {
            document.location.href = wizardUrl;
            return false;
        }
        else if ($('#RedirectToPage').val() !== '') {
            window.location.href = $('#RedirectToPage').val();
            return false;
        }
        notifyDanger('Nothing was changed.');

        return false;
    }



    if ($('#PersonID').val() == "") {
        notifyDanger('Worker is required.');
        return false;
    }
    else if ($("#WorkDescriptionCodeID").val() == "") {
        notifyDanger('Description is required.');
        $("#WorkDescriptionCodeID").focus();
        return false;
    }

    else if ($("#WorkHours").val() == "") {
        notifyDanger('Hours is required.');
        $("#WorkHours").focus();
        return false;
    } else if (!($("#WorkHours").val() >= 0.1 && $("#WorkHours").val() <= 8) && !$('#WorkHours').is(':disabled')) {
        notifyDanger('Hours must be between .1 and 8.');
        $("#WorkHours").focus();
        return false;
    } else if ($("select#WorkIVeEligibleCodeID").val() == "" && $("select#WorkIVeEligibleCodeID").length > 0) {
        notifyDanger('IV-E Eligible is required.');
        $("select#WorkIVeEligibleCodeID").focus();
        return false;
    }
    else if ($("#WorkStartDate").val() == "") {
        notifyDanger('Date is required.');
        $("#WorkStartDate").focus();
        return false;
    }
    else if (moment($("#WorkStartDate").val()) > moment()) {
        notifyDanger('Date cannot be in the future.');
        $("#WorkEndDate").focus();
        return false;
    }
    if ($("#NoteSubject").is(':visible') && $("#NoteSubject").val() !== "" && $('#NoteEntry').GetText() == '') {
        notifyDanger('Note is required when Note Subject is entered.');
        $("#NoteEntry").focus();

        return false;
    }

    if ($('#CountyID').val() == "") {
        notifyDanger('County is required.');
        $("#CountyID").focus();
        return false;
    }


    var data = getData();
    ajaxREquestForSave(data, buttonId);
}

$("#btnSave").on("click", function () {
    SaveData(1);
});

$("#btnCancel").on("click", function () {



    if ($('#RedirectToPage').val() !== '') {
        window.location.href = $('#RedirectToPage').val();

    } else {

        window.location.href = "/Task/RecordTimeNonCase";
    }

});
$(function () {
    if ($('#WorkHours').is(':disabled')) {
        if ($('#WorkHours').val() == '')
            $('#WorkHours').val('0')
    }
    setInitialFormValues('recordTimeEdit-form', true);
    oldModelData = getData();

    $('body').on('click', '#btnDelete', function () {
        var id = $(this).attr('data-id');
        var tr = $(this).parent().parent();
        confirmBox("Are you sure you want to delete?", function (result) {
            if (result) {
                $.ajax({
                    type: "POST", url: '/Task/RecordTimeNonCaseDelete/' + id,
                    dataType: "json",
                    success: function (data) {

                        Notify('Record delete successfully.', 'bottom-right', '5000', 'success', 'fa-check', true);
                        window.location.href = "/Task/RecordTimeNonCase";
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    }
                });
            }
        });
    });

});

