var $emptyHearingRecord = "";
function backToSourcePage() {
    if (entryPageId == '1') {
        document.location.href = '/Inquiry/MyJcatsAtty';
    }
    else {
        document.location.href = '/Inquiry/MyCalendar?p=' + calnderPersonId;
    }
}

function isValid() {
    for (var indx = 0; indx < $("#allNewHearings .hearing").length; indx++) {
        var $hearing = $("#allNewHearings .hearing").eq(indx);
        if ($hearing.find("#HearingTypeID").IsValueChanged()
                    || $hearing.find("#HearingDate").IsValueChanged()
                    || $hearing.find("#TimeHours").IsValueChanged()
                    || $hearing.find("#Minutes").IsValueChanged()
                    || $hearing.find("#TimeAmPm").IsValueChanged()
                    || $hearing.find("#HearingOfficerID").IsValueChanged()
                    || $hearing.find("#DepartmentID").IsValueChanged()
                    || $hearing.find("#AppearingAttorneyID").IsValueChanged()) {

            if ($hearing.find("#HearingTypeID").val().length == 0) {
                $hearing.find("#HearingTypeID").focus();
                notifyDanger("Hearing type is required.");
                return false;
            }

            if ($hearing.find("#HearingDate").val().length == 0) {
                $hearing.find("#HearingDate").focus();
                notifyDanger("Hearing date is required.");
                return false;
            }

            if ($hearing.find("#TimeHours").val().length == 0) {
                $hearing.find("#TimeHours").focus();
                notifyDanger("Hearing time hour is required.");
                return false;
            }

            if ($hearing.find("#Minutes").val().length == 0) {
                $hearing.find("#Minutes").focus();
                notifyDanger("Hearing time minutes is required.");
                return false;
            }

            if ($hearing.find("#HearingOfficerID").val().length == 0) {
                $hearing.find("#HearingOfficerID").focus();
                notifyDanger("Hearing officer is required.");
                return false;
            }

            if ($hearing.find("#DepartmentID").val().length == 0) {
                $hearing.find("#DepartmentID").focus();
                notifyDanger("Hearing department is required.");
                return false;
            }

            //if ($hearing.find("#DepartmentID").IsValueChanged()) {
            //    confirmBox("Do you want to update the case department to '" + $hearing.find("#DepartmentID option:selected").text() + "'", function (result) {

            //    });
            //}
        }
    }

    if ($(".chkPetition:checked").length == 0) {
        $(".chkPetition:first").focus();
        notifyDanger("Atleast one petition is required");
        return false;
    }

    return true;
}

function getData(buttonID) {
    var data = {
        HearingID: $("#HearingID").val(),
        buttonID: buttonID,
        Hearings: [],
        PetitionList: []
    };

    for (var indx = 0; indx < $("#allNewHearings .hearing").length; indx++) {
        var $hearing = $("#allNewHearings .hearing").eq(indx);
        if ($hearing.find("#HearingTypeID").IsValueChanged()
                    || $hearing.find("#HearingDate").IsValueChanged()
                    || $hearing.find("#TimeHours").IsValueChanged()
                    || $hearing.find("#Minutes").IsValueChanged()
                    || $hearing.find("#TimeAmPm").IsValueChanged()
                    || $hearing.find("#HearingOfficerID").IsValueChanged()
                    || $hearing.find("#DepartmentID").IsValueChanged()
                    || $hearing.find("#AppearingAttorneyID").IsValueChanged()) {

            var hearingInfo = {
                HearingTypeID: $hearing.find("#HearingTypeID").val(),
                HearingDate: $hearing.find("#HearingDate").val(),
                HearingTime: $hearing.find('#TimeHours').val() + ':' + $hearing.find('#Minutes').val() + ' ' + $hearing.find('#TimeAmPm').val(),
                HearingOfficerID: $hearing.find("#HearingOfficerID").val(),
                DepartmentID: $hearing.find("#DepartmentID").val(),
                AppearingAttorneyID: $hearing.find("#AppearingAttorneyID").val()
            }

            data.Hearings.push(hearingInfo);
        }
    }

    $(".chkPetition:checked").each(function (e) {        
        var petitionInfo = {
            PetitionID : $(this).data("id")
        }
        data.PetitionList.push(petitionInfo);
    });

    return data;
}

function saveData(buttonId){

    if (!IsValidFormRequest()) {
        return false;
    }

    if (hasFormChanged("QHEAddHEaring-form")) {
        if (isValid()) {
            var data = getData(buttonId);
            $.ajax({
                type: "POST", url: '/QHE/QHENextHearing', data: data,
                success: function (result) {
                    RequestSubmitted();
                    if (result.isSuccess) {
                        if (buttonId == 1) {
                            window.location.href = "/QHE/QHERecordTime/" + $encryptedHearingId;
                        }
                        else if (buttonId == 2) {
                            backToSourcePage();
                        }
                        else if (buttonId == 3) {
                            if (result.nextHearingId != '') {
                                window.location.href = "/QHE/QHEHearing/" + result.nextHearingId;
                            } else {
                                backToSourcePage();
                            }                            
                        } else if (buttonId == 4) {
                            window.location.href = wizardUrl;
                        }
                        else {
                            notifySuccess("Hearing saved successfully.");
                        }
                    }
                }
            });
        }
    }
    else {
        if (buttonId == 1) {
            window.location.href = "/QHE/QHERecordTime/" + $encryptedHearingId;
        }
        else if (buttonId == 2) {
            backToSourcePage();
        }
        else if (buttonId == 3) {
            $.ajax({
                type: "POST", url: '/QHE/NextQHECase', data: { id: $('#HearingID').val() },
                success: function (result) {
                    if (result.nextHearingId != '') {
                        window.location.href = "/QHE/QHEHearing/" + result.nextHearingId;
                    } else {
                        backToSourcePage();
                    }
                }
            });
        }
        else if (buttonId == 4) {
            document.location.href = wizardUrl;
        }
        else {
            notifySuccess("Nothing has been changed.");
        }
    }
}
var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
    e.preventDefault();
    wizardUrl = $(this).attr('href');

    saveData(4);

});
$("#btnSaveAndContinue").on("click", function () {
    saveData(1);
});

$("#btnSaveAndExit").on("click", function () {
    saveData(2);
});

$("#btnSaveAndNextQHECase").on("click", function () {
    saveData(3);
});
$('#chkPetitionAll').on('change', function () {

    $('.chkPetition').not(':disabled').prop('checked', $(this).is(':checked'));

});
$("#btnAddNewHearing").on("click", function () {
    $("#allNewHearings").append('<div class="hearing" id="hearing-' + $(".hearing").length + '">' + $emptyHearingRecord + '</div>');
    $(".hearing:last  #HearingTypeID").focus();
    if ($(".hearing").length > 1) {
        $("#btnRemoveHearing").prop("disabled", false);
    }
});

$("#btnRemoveHearing").on("click", function () {
    var lastrecordId = $(".hearing").length - 1;
    $("#hearing-" + lastrecordId).remove();
    $(".hearing:last  #HearingTypeID").focus();
    if (lastrecordId <= 1) {
        $(this).prop("disabled", true);
    }
});

$(document).ready(function () {
    setInitialFormValues("QHEAddHEaring-form");

    $emptyHearingRecord = $("#hearing-0").html();
});