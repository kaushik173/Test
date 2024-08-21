var oldModelData; 

var validateTabClose = true;


$(".ddlTime").on("change", function () {

    CalCulateHours();
});
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


    if ($('#WorkDescriptionCodeID').find('option:selected').attr('data-useworktimeflag') == 1) {
        $('.useWorkTime').show();
        $('.WorkHours').removeClass('col-lg-3').removeClass('col-md-4').addClass('pull-left');
        $('.WorkHours label').text('Hours')
        $('#WorkHours').prop('disabled', true);
        if (!onload) {
            $("#WorkPhaseCodeID").val('');
        }

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

    onload = false;
}
function CalCulateHours() {
    $("#WorkStartTime").val($("#WorkStartDate").val() + ' ' + ($("#WorkPhaseStartTimeHours").val() + ':' + $("#WorkPhaseStartTimeMinutes").val() + ' ' + $("#WorkPhaseStartTimeAmPm").val()))
    $("#WorkEndTime").val($("#WorkStartDate").val() + ' ' + ($("#WorkPhaseEndTimeHours").val() + ':' + $("#WorkPhaseEndTimeMinutes").val() + ' ' + $("#WorkPhaseEndTimeAmPm").val()))

    var startTime = moment(moment(new Date()).format("MM/DD/YYYY") + ' ' + ($("#WorkPhaseStartTimeHours").val() + ':' + $("#WorkPhaseStartTimeMinutes").val() + ' ' + $("#WorkPhaseStartTimeAmPm").val()))
    var endTime = moment(moment(new Date()).format("MM/DD/YYYY") + ' ' + ($("#WorkPhaseEndTimeHours").val() + ':' + $("#WorkPhaseEndTimeMinutes").val() + ' ' + $("#WorkPhaseEndTimeAmPm").val()))


    var ms = endTime.diff(startTime);
    var d = moment.duration(ms);

    $("#WorkHours").val((RoundHours(d.asMinutes(), 3, 3) / 60));
}
$(".ddlStaff").on("change", function () {
    WorkIVeEligibleCodeDisplay(false);
});
WorkIVeEligibleCodeDisplay(true);
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
    if ($('#WorkDescriptionCodeID').find('option:selected').attr('data-zipcoderequiredflag') == 0) {
        $('#FromZipCode').val('');
        $('#ToZipCode').val('');
    }
    if ($('#WorkDescriptionCodeID').find('option:selected').attr('data-useworktimeflag') == 1) {
        $("#WorkStartTime").val($("#WorkStartDate").val() + ' ' + ($("#WorkPhaseStartTimeHours").val() + ':' + $("#WorkPhaseStartTimeMinutes").val() + ' ' + $("#WorkPhaseStartTimeAmPm").val()))
        $("#WorkEndTime").val($("#WorkStartDate").val() + ' ' + ($("#WorkPhaseEndTimeHours").val() + ':' + $("#WorkPhaseEndTimeMinutes").val() + ' ' + $("#WorkPhaseEndTimeAmPm").val()))

    }
    var data = {
        WorkTimeID: $("#WorkTimeID").val(),
        WorkID: $("#WorkID").val(),
        AgencyID: $("#AgencyID").val(),
        HearingID: $("#HearingID").val(),
        QHEHearingID: $("#QHEHearingID").val(),
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
        "WorkIVeEligibleCodeID": $('#WorkIVeEligibleCodeID').val(),
        WorkForList: [],
        DeleteWorkForList: [],
        "WorkStartTime": $('#WorkStartTime').val(),
        "WorkEndTime": $('#WorkEndTime').val(),
        "FromZipCode": $('#FromZipCode').val(),
        "ToZipCode": $('#ToZipCode').val(),
        "WorkZipCodeID": $('#WorkZipCodeID').val(),
    }

    var workForTr = $("#workedForList tbody tr");
    for (var indx = 0; indx < workForTr.length; indx++) {
        $tr = workForTr.eq(indx);
        $personchk = $tr.find(".chkWork");
        if ($personchk.is(":checked")) {
            var workList = {
                "RoleID": $tr.data("roleid"),
                "WorkRoleID": $personchk.val(),
            };

            data.WorkForList.push(workList);
        }
        else if ($personchk.val() > 0) {
            var workList = {

                "RoleID": $tr.data("roleid"),
                "WorkRoleID": $personchk.val(),
            };

            data.DeleteWorkForList.push(workList);
        }


    }

    return data;
}

function SaveData(buttonId) {
    if ($('#UseWorkHoursForActivityLog').val() == '1') {
        $('#WorkEndDate').val($('#WorkStartDate').val())
    }
    IPadKeyboardFix();
    if (!IsValidFormRequest()) {
        return false;
    }

    if (!hasFormChanged('recordTimeEdit-form')) {
        if (buttonId == 3) {
            document.location.href = wizardUrl;
            return false;
        } else if ($('#RedirectToPage').val() !== '') {
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
    } else if ($("#WorkPhaseCodeID").is(':visible') && $("#WorkPhaseCodeID").val() == "" && $("#WorkPhaseRequiredFlag").val() == '1') {
        notifyDanger('Phase is required.');
        $("#WorkPhaseCodeID").focus();
        return false;
    } else if ($('#WorkDescriptionCodeID').find('option:selected').attr('data-useworktimeflag') == 1 && $("#WorkPhaseCodeID").is(':visible') && $("#WorkPhaseCodeID").val() == "") {
        notifyDanger('Phase is required.');
        $("#WorkPhaseCodeID").focus();
        return false;
    }
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
    else if ($("#WorkStartDate").val() == "") {
        notifyDanger('Start Date is required.');
        $("#WorkStartDate").focus();
        return false;
    } else if (moment($("#WorkStartDate").val()) > moment()) {
        notifyDanger('Start Date cannot be in the future.');
        $("#WorkStartDate").focus();

        return false;
    } else if ($("#WorkEndDate").val() == "") {
        notifyDanger('End Date is required.');
        $("#WorkEndDate").focus();
        return false;
    }
    else if ($(".chkWork:checked").length == 0) {
        notifyDanger('At least one Worked For Person is required.');
        return false;
    }
    if (moment($("#WorkStartDate").val()) > moment($("#WorkEndDate").val())) {
        notifyDanger('End Date can not be before Start Date.');
        $("#WorkEndDate").focus();
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
    var data = getData();
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
    var ajaxUrl = '/QHE/QHERecordTimeEditSave';
    //$.ajax({
    //    type: "POST", dataType: 'json', url: ajaxUrl, data: JSON.stringify({ 'viewModel': data, 'oldViewModel': oldModelData }), contentType: "application/json",
    //    success: function (result) {
    //        RequestSubmitted();
    //        if (result.isSuccess) {
    //            if (buttonId == 2) {
    //                document.location.href = wizardUrl;
    //            } else if (result.URL != '') {
    //                window.location.href = result.URL;
    //            }
    //        }
    //        else {
    //            notifyDanger('There is something wrong while processing request.');
    //        }
    //    }
    //});

    //var ajaxUrl = '/Case/RecordTimeEditSave';
    

    $.ajax({
        type: "POST", dataType: 'json', url: ajaxUrl, data: JSON.stringify({ 'viewModel': data, 'oldViewModel': oldModelData }), contentType: "application/json",
		success: function (result) {
			validateTabClose = false;
            RequestSubmitted();
            if (result.isSuccess) {
                if (buttonId == 3) {
                    document.location.href = wizardUrl;
                    return false;
                }
                if ($('#RedirectToPage').val() !== '') {
                    window.location.href = $('#RedirectToPage').val();

                }
                else if (result.URL != '') {
                    window.location.href = result.URL;
                }
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
var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
    e.preventDefault();
    wizardUrl = $(this).attr('href');

    SaveData(2);

});
$("#btnSave").on("click", function () {
    SaveData(1);
});

$("#btnCancel").on("click", function () {
	validateTabClose = false;
    window.location.href = "/QHE/QHERecordTime/" + $hearingId
});

$(function () {
    setInitialFormValues('recordTimeEdit-form', true);
    if ($('#UseWorkHoursForActivityLog').val() == '1') {
        $('#WorkEndDate').prop('disabled', true);
    }
    oldModelData = getData();

});


window.onbeforeunload = function (e) {

	if (hasFormChanged('recordTimeEdit-form') && validateTabClose) {
		return 'There is unsaved data.';
	}
	return undefined;
}