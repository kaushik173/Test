var $BaseURL = "/";
var validateTabClose = true;

$(function () {

    if ($('#WorkHours').is(':disabled')) {
        $('#WorkHours').val('0')
    }

    if ($('#UseWorkHoursForActivityLog').val() == '1') {
        $('#EndDate').prop('disabled', true);
    }

    setInitialFormValues('frmRecordAddEdit', true);


});
var $BaseURL = "/";

var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
    e.preventDefault();
    wizardUrl = $(this).attr('href');
    if (!hasFormChanged('frmRecordAddEdit')) {
        document.location.href = wizardUrl;
        return false;
    }
    SaveData(4);

});
function CalCulateHours() {

    $("#WorkStartTime").val($('#StartDate').val() + ' ' + ($("#WorkPhaseStartTimeHours").val() + ':' + $("#WorkPhaseStartTimeMinutes").val() + ' ' + $("#WorkPhaseStartTimeAmPm").val()))
    $("#WorkEndTime").val($('#StartDate').val() + ' ' + ($("#WorkPhaseEndTimeHours").val() + ':' + $("#WorkPhaseEndTimeMinutes").val() + ' ' + $("#WorkPhaseEndTimeAmPm").val()))


    var startTime = moment(moment(new Date()).format("MM/DD/YYYY") + ' ' + ($("#WorkPhaseStartTimeHours").val() + ':' + $("#WorkPhaseStartTimeMinutes").val() + ' ' + $("#WorkPhaseStartTimeAmPm").val()))
    var endTime = moment(moment(new Date()).format("MM/DD/YYYY") + ' ' + ($("#WorkPhaseEndTimeHours").val() + ':' + $("#WorkPhaseEndTimeMinutes").val() + ' ' + $("#WorkPhaseEndTimeAmPm").val()))


    var ms = endTime.diff(startTime);
    var d = moment.duration(ms);

    $("#WorkHours").val((RoundHours(d.asMinutes(), 3, 3) / 60));
}

$(".ddlTime").on("change", function () {

    CalCulateHours();
});

$("#WorkDescriptionCodeID").on("change", function () {

    WorkIVeEligibleCodeDisplay();
});
function WorkIVeEligibleCodeDisplay() {
    var isattorney = 0;
    isattorney = $('#StaffOnPersonID').find('option:selected').attr('data-isattorneyflag');
    if (isattorney === 0) {
        isattorney = $('#StaffNotOnPersonID').find('option:selected').attr('data-isattorneyflag')
    }
    var issupervisor = 0;
    issupervisor = $('#StaffOnPersonID').find('option:selected').attr('data-issupervisorflag');
    if (issupervisor === 0) {
        issupervisor = $('#StaffNotOnPersonID').find('option:selected').attr('data-issupervisorflag')
    }

    if ($("select#WorkIVeEligibleCodeID").length > 0 && issupervisor == '1') {
        $("select#WorkIVeEligibleCodeID").val($('#WorkDescriptionCodeID').find('option:selected').attr('data-supervisor-default-ive'))
        if ($('#WorkDescriptionCodeID').find('option:selected').attr('data-supervisor-default-ive') > 0 && $('#WorkDescriptionCodeID').find('option:selected').attr('data-supervisor-default-ive-can-change') == 0) {
            $("select#WorkIVeEligibleCodeID").prop('disabled', true)
        } else {
            $("select#WorkIVeEligibleCodeID").prop('disabled', false)
        }
    } else if ($("select#WorkIVeEligibleCodeID").length > 0 && isattorney == '1') {
        $("select#WorkIVeEligibleCodeID").val($('#WorkDescriptionCodeID').find('option:selected').attr('data-attorny-default-ive'))
        if ($('#WorkDescriptionCodeID').find('option:selected').attr('data-attorny-default-ive') > 0 && $('#WorkDescriptionCodeID').find('option:selected').attr('data-attorny-default-ive-can-change') == 0) {
            $("select#WorkIVeEligibleCodeID").prop('disabled', true)
        } else {
            $("select#WorkIVeEligibleCodeID").prop('disabled', false)
        }
    } else if ($("select#WorkIVeEligibleCodeID").length > 0) {
        $("select#WorkIVeEligibleCodeID").val($('#WorkDescriptionCodeID').find('option:selected').attr('data-default-ive'))
        if ($('#WorkDescriptionCodeID').find('option:selected').attr('data-default-ive') > 0 && $('#WorkDescriptionCodeID').find('option:selected').attr('data-default-ive-can-change') == 0) {
            $("select#WorkIVeEligibleCodeID").prop('disabled', true)
        } else {
            $("select#WorkIVeEligibleCodeID").prop('disabled', false)
        }
    }

    if ($('#WorkDescriptionCodeID').find('option:selected').attr('data-useworktimeflag') == 1) {
        $('.useWorkTime').show();
        $('.WorkHours').removeClass('col-lg-3').removeClass('col-md-4').addClass('pull-left');
        $('.WorkHours label').text('Hours')
        $('#WorkHours').prop('disabled', true);
        $("#WorkPhaseCodeID").val('');
    } else {
        $('.useWorkTime').hide();
        $('.WorkHours').addClass('col-lg-3 col-md-4');
        $('.WorkHours label').text($('.WorkHours').data('label'))
        $('#WorkHours').not('.disabled-permanent').prop('disabled', false);
        $("#WorkStartTime").val('');
        $("#WorkEndTime").val('');
    }
    if ($('#WorkDescriptionCodeID').find('option:selected').attr('data-zipcoderequiredflag') == 1) {
        $('.FromAndToZipCode').show();
    } else {
        $('.FromAndToZipCode').hide();
    }
    $("#WorkTimeVisibleFlag").val($('#WorkDescriptionCodeID').find('option:selected').attr('data-useworktimeflag'));
}

$(".ddlStaff").on("change", function () {
    WorkIVeEligibleCodeDisplay();
});

var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
    e.preventDefault();
    wizardUrl = $(this).attr('href');

    SaveData(2);

});


$("#btnSave").on("click", function () {
    SaveData(1)
});

$("#btnCancel").on("click", function () {
	validateTabClose = false;
    window.location.href = '/QHE/QHERecordTime/' + $hearingId;
}); 
function ajaxREquestForSave(data, buttonId) {
    var ajaxUrl = '/QHE/QHERecordTimeAddSave';
    $.ajax({
        type: "POST", dataType: 'json', url: ajaxUrl, data: data, contentType: "application/json",
		success: function (result) {
			validateTabClose = false;
            if (result.isSuccess) {
                RequestSubmitted();
                if (buttonId == 2) {
                    document.location.href = wizardUrl;
                } else
                    window.location.href = '/QHE/QHERecordTime/' + $hearingId;
            }
            else {
                notifyDanger('There is something wrong while processing request.');
            }
        }
    });
}

$(document).ready(function () {
    if ($('#UseWorkHoursForActivityLog').val() == '1') {
        $('#EndDate').prop('disabled', true);
    }

    setInitialFormValues("QHERecordTimeAdd-form")
});

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

function GetData(buttonID) {
    if ($('#WorkDescriptionCodeID').find('option:selected').attr('data-zipcoderequiredflag') == 0) {
        $('#FromZipCode').val('');
        $('#ToZipCode').val('');
    }
    if ($('#WorkDescriptionCodeID').find('option:selected').attr('data-useworktimeflag') == 1) {
         $("#WorkStartTime").val($('#StartDate').val() + ' ' + ($("#WorkPhaseStartTimeHours").val() + ':' + $("#WorkPhaseStartTimeMinutes").val() + ' ' + $("#WorkPhaseStartTimeAmPm").val()))
    $("#WorkEndTime").val($('#EndDate').val() + ' ' + ($("#WorkPhaseEndTimeHours").val() + ':' + $("#WorkPhaseEndTimeMinutes").val() + ' ' + $("#WorkPhaseEndTimeAmPm").val()))

    }
  
    var data = {
        "NextCaseID": $('#NextCaseID').val(),
        "NextCase": $('#NextCase').val(),
        "StaffOnPersonID": $('#StaffOnPersonID').val(),
        "StaffNotOnPersonID": $('#StaffNotOnPersonID option:selected').val(),
        "RoleTypeCodeID": $("#StaffNotOnPersonID option:selected").data("roletypeid"),
        "StartDate": $('#StartDate').val(),
        "EndDate": $('#EndDate').val(),
        "WorkHours": $('#WorkHours').val(),
        "WorkMileage": $('#WorkMileage').val(),
        "WorkDescriptionCodeID": $('#WorkDescriptionCodeID').val(),
        "WorkPhaseCodeID": $('#WorkPhaseCodeID').val(),
        "NoteEntry": $('#NoteEntry').GetHtml(),
        "ButtonID": buttonID,
        "NoteSubject": $('#NoteSubject').val(),
        "WorkIVeEligibleCodeID": $('#WorkIVeEligibleCodeID').val(),
        "WorkForList": [],
        "WorkStartTime": $('#WorkStartTime').val(),
        "WorkEndTime": $('#WorkEndTime').val(),
        "FromZipCode": $('#FromZipCode').val(),
        "ToZipCode": $('#ToZipCode').val(),
        "HearingID": $('#HearingID').val(),
        "WorkTimeVisibleFlag": $("#WorkTimeVisibleFlag").val(),
    };

    var workForTr = $("#workedForList tbody tr");
    for (var indx = 0; indx < workForTr.length; indx++) {
        $tr = workForTr.eq(indx);
        $personchk = $tr.find(".chkWork");
        if ($personchk.is(":checked")) {
            var workList = {
                "RoleID": $tr.data("roleid"),
                "RoleTypeCodeID": $tr.data("roletypecodeid"),
                "PersonID": $personchk.val(),
            };

            data.WorkForList.push(workList);
        }
    }
    return data;
}

function SaveData(buttonId) {
    if ($('#UseWorkHoursForActivityLog').val() == '1') {
        $('#EndDate').val($('#StartDate').val())
    }
    IPadKeyboardFix();
    if (!IsValidFormRequest()) {
        return;
    }
    if (!hasFormChanged("QHERecordTimeAdd-form")) {
        
            if (buttonId == 2) {
                document.location.href = wizardUrl;
            } else
                notifyDanger("Nothing has been changed.");
        
    }
    if ($('#StaffOnPersonID').val() == "" && $('#StaffNotOnPersonID').val() == "") {
        notifyDanger('Staff member is required.');
        $('#StaffOnPersonID').focus();
        return false;
    }
    if ($('#StaffOnPersonID').val() != "" && $('#StaffNotOnPersonID').val() != "") {
        notifyDanger('Only one staff dropdown can be selected, not both.');
        return false;
    } else if ($("#WorkDescriptionCodeID").val() == "") {
        notifyDanger('Description is required.');
        $("#WorkDescriptionCodeID").focus();
        return false;
    } else if ($("#WorkPhaseCodeID").is(':visible') && $("#WorkPhaseCodeID").val() == "" && $("#WorkPhaseRequiredFlag").val() == '1') {
        notifyDanger('Phase is required.');
        $("#WorkPhaseCodeID").focus();
        return false;
    }
    else if ($('#WorkDescriptionCodeID').find('option:selected').attr('data-useworktimeflag') == 1 && $("#WorkPhaseCodeID").is(':visible') && $("#WorkPhaseCodeID").val() == "") {
        notifyDanger('Phase is required.');
        $("#WorkPhaseCodeID").focus();
        return false;
    }
    //else if ($("#WorkPhaseCodeID").val() == "") {
    //    notifyDanger('Phase is required.');
    //    $("#WorkPhaseCodeID").focus();
    //    return false;
    //}

    else if ($("#WorkHours").val() == "" && $("#WorkHoursRequiredFlag").val() == '1') {
        notifyDanger('Hours is required.');
        $("#WorkHours").focus();
        return false;
    }
    else if ($("select#WorkIVeEligibleCodeID").val() == "" && $("select#WorkIVeEligibleCodeID").length > 0) {
        notifyDanger('IV-E Eligible is required.');
        $("select#WorkIVeEligibleCodeID").focus();
        return false;
    }
    else if ($(".chkWork:checked").length == 0) {
        notifyDanger('At least one Worked For Person is required.');
        return false;
    }
    if ($("#StartDate").val() == "") {
        notifyDanger('Start Date is required.');
        $("#StartDate").focus();
        closedflag = false;
        return false;
    }
    if (moment($("#StartDate").val()) > moment()) {
        notifyDanger('Start Date cannot be in the future.');
        $("#StartDate").focus();
        closedflag = false;
        return false;
    }
    if ($("#EndDate").val() != "" && moment($("#StartDate").val()) > moment($("#EndDate").val())) {
        notifyDanger('End Date can not be before Start Date.');
        $("#EndDate").focus();
        closedflag = false;
        return false;
    }
    if ($("#NoteSubject").is(':visible') && $("#NoteSubject").val() !== "" && $('#NoteEntry').GetText() == '') {
        notifyDanger('Note is required when Note Subject is entered.');
        $("#NoteEntry").focus();

        return false;
    }
    if ($("#WorkEndTime").val() != "" && moment($("#WorkStartTime").val()) > moment($("#WorkEndTime").val())) {
        notifyDanger('End Time can not be before Start Time.');
        $("#WorkPhaseEndTimeHours").focus();
        closedflag = false;
        return false;
    }

    if ($('#WorkDescriptionCodeID').find('option:selected').attr('data-zipcoderequiredflag') == 1) {



        if ($("#WorkMileage").val() == "") {
            notifyDanger('Mileage required when Work Description is Travel.');
            $("#WorkMileage").focus();
            closedflag = false;
            return false;
        } if ($("#FromZipCode").val() == "") {
            notifyDanger('From Zip Code is required when Work Description is Travel.');
            $("#FromZipCode").focus();
            closedflag = false;
            return false;
        }
        if ($("#FromZipCode").val().length < 5) {
            notifyDanger('From ZipCode must be exactly five digits');
            $("#FromZipCode").focus();
            closedflag = false;
            return false;
        }
        if ($("#ToZipCode").val() == "") {
            notifyDanger('To Zip Code is required when Work Description is Travel.');
            $("#ToZipCode").focus();
            closedflag = false;
            return false;
        }

        if ($("#ToZipCode").val().length < 5) {
            notifyDanger('To ZipCode must be exactly five digits');
            $("#ToZipCode").focus();
            closedflag = false;
            return false;
        }
        if ($("#ToZipCode").val() == $("#FromZipCode").val()) {
            notifyDanger('From ZipCode cannot equal To ZipCode.');
            $("#ToZipCode").focus();
            closedflag = false;
            return false;
        }
    }
    var isclosed = validation();

    var data = JSON.stringify(GetData(buttonId));
    if (!isclosed) {
        confirmBox("You have selected a client who does not have an open petition. OK to continue?", function (result) {
            if (result) {

                ajaxREquestForSave(data, buttonId);
            }
        });
    }
    else {
        ajaxREquestForSave(data, buttonId);
    }

}

function ajaxREquestForSave(data, buttonId) {
    var ajaxUrl = '/QHE/QHERecordTimeAddSave';
    isBottomBarAccessible(false);


    $.ajax({
        type: "POST", dataType: 'json', url: ajaxUrl, data: data, contentType: "application/json",
        success: function (result) {
			if (result.isSuccess) {
				validateTabClose = false;
                RequestSubmitted();
                notifySuccess('Data Saved Successfully!');

                if (buttonId == 2) {
                    document.location.href = wizardUrl;
                } else
                    window.location.href = '/QHE/QHERecordTime/' + $hearingId;



            } else if (result.Message !== undefined) {
                AlertBox(result.Message, "", function (result) {
                    if (result) {

                        $('#WorkPhaseStartTimeHours').focus()
                    }
                });
            }
            else {
                notifyDanger('There is something wrong while processing request.');
            }
        }
    });
}

window.onbeforeunload = function (e) {

	if (hasFormChanged('frmRecordAddEdit') && validateTabClose) {
		return 'There is unsaved data.';
	}
	return undefined;
}
