function GetHoursRequiredFlag(flagElmValue) {
    var newFlag = flagElmValue;
    if (flagElmValue == '1') {
        if (($('#PhaseID').hasValue() || $('#HourTypeID').hasValue())) {
            newFlag = 1;
        } else if ($('#HearingTypeHoursNotRequired').val().length > 0 && Contains($('#HearingTypeHoursNotRequired').val(), $('#HearingTypeID').val())) {
            newFlag = 0;
        } else if (hoursNotRequiredBeforeHearingDate !== '' && moment($('#HearingDate').val()) < moment(hoursNotRequiredBeforeHearingDate)) {
            newFlag = 0;
        }

    }
    return newFlag;
}

$('#HearingTypeID').on('change', function () {

    if ($('#selectParentPhase option[data-hearingtypeid="' + $(this).val() + '"]').length > 0) {
        $('#PhaseID').html('  <option value=""></option>');
        $('#selectParentPhase option[data-hearingtypeid="' + $(this).val() + '"]').each(function () {
            $('#PhaseID').append($(this).clone())
        });

    } else {
        $('#PhaseID').html('');
        $('#selectPhase option').each(function () {
            $('#PhaseID').append($(this).clone())
        });
    }
    if ($('#selectParentHourType option[data-hearingtypeid="' + $(this).val() + '"]').length > 0) {
        $('#HourTypeID').html('  <option value=""></option>');
        $('#selectParentHourType option[data-hearingtypeid="' + $(this).val() + '"]').each(function () {
            $('#HourTypeID').append($(this).clone())
        });

    } else {
        $('#HourTypeID').html('');
        $('#selectHourType option').each(function () {
            $('#HourTypeID').append($(this).clone())
        });
    }
    $('#HourTypeID').val($('#HourTypeID').data('selected'));
    $('#PhaseID').val($('#PhaseID').data('selected'));
    if ($('#Hours').val() !== '') {
        if ($('#HourTypeID option').length == 2) {
            $('#HourTypeID').val($('#HourTypeID option:last').val())
        }
        if ($('#PhaseID option').length == 2) {
            $('#PhaseID').val($('#PhaseID option:last').val())
        }
    }

});
$('#Hours').on('change', function () {
    if ($('#Hours').val() !== '') {
        if ($('#HourTypeID option').length == 2) {
            $('#HourTypeID').val($('#HourTypeID option:last').val())
        }
        if ($('#PhaseID option').length == 2) {
            $('#PhaseID').val($('#PhaseID option:last').val())
        }
    }


});
var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
    e.preventDefault();
    wizardUrl = $(this).attr('href');

    Save(5)

});
$('#chkPetitionAll').on('change', function () {

    $('.chkPetition').not(':disabled').prop('checked', $(this).is(':checked'));

});
$(function () {
    $('select').each(function () {
        if ($(this).data('selected') !== undefined) {
            $(this).val($(this).data('selected'));
        }
    });
    $('#HearingTypeID').change();
    setInitialFormValues('case-hearing-form', true);
});


function Validation(buttonId) {
    var isValid = true;

    if (!hasFormChanged('case-hearing-form')) {

        if (buttonId == 1) {
            document.location.href = '/QHE/QHEAttendance/' + $encryptedHearingId;
            return false;
        } else if (buttonId == 2) {
            document.location.href = '/QHE/QHENextHearing/' + $encryptedHearingId;
            return false;
        } else if (buttonId == 3) {
            backToSourcePage();
            return false;
        } else if (buttonId == 4) {
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
            return false;
        } else if (buttonId == 5) {
            document.location.href = wizardUrl;
            return false;
        }
        notifyDanger('Nothing was changed.');
        isValid = false;
        return false;
    }
    if ($('#HearingTypeID').val() == '') {
        isValid = false;
        $('#HearingTypeID').focus();
        notifyDanger('Hearing Type is required.');
        return false;
    }
    if ($('#HearingDate').val() == '') {
        isValid = false;
        $('#HearingDate').focus();
        notifyDanger('Hearing Date is required.');
        return false;
    }
    if ($('#Hours').val() != '' || ($('#PhaseID').val() != '' && $('#PhaseID').val() != null) || ($('#HourTypeID').val() != '' && $('#HourTypeID').val() != null)) {
        if ($('#AppearingAttorneyID').val() == '') {
            isValid = false;
            $('#AppearingAttorneyID').focus();
            notifyDanger('Appearing Attorney is required.');
            return false;
        } if ($('#Hours').val() == '' && GetHoursRequiredFlag($('#DataValidation_RequireHearingHoursFlag').val()) == '1') {
            isValid = false;
            $('#HourTypeID').focus();
            notifyDanger('Hours is required.');
            return false;
        }
        if (($('#HourTypeID').val() == '' || $('#HourTypeID').val() == null) && GetHoursRequiredFlag($('#DataValidation_RequireHearingHoursFlag').val()) == '1') {
            isValid = false;
            $('#HourTypeID').focus();
            notifyDanger('Hour Type is required.');
            return false;
        } if ($('#PhaseID').val() == '' || $('#PhaseID').val() == null) {
            isValid = false;
            $('#PhaseID').focus();
            notifyDanger('Phase is required.');
            return false;
        }
        if ($('#Hours').val() <= 0 && GetHoursRequiredFlag($('#DataValidation_RequireHearingHoursFlag').val()) == '1') {
            isValid = false;
            $('#Hours').focus();
            notifyDanger('Hours worked must be a positive numeric value.');
            return false;
        }
        if (moment($('#HearingDate').val()) > moment() && $('#Hours').val() !== '' && $('#Hours') > 0) {
            isValid = false;
            $('#Hours').focus();
            notifyDanger('Hours worked may not be entered on a future hearing.');
            return false;
        }
    }


    //SET @602PetitionChecked=TRUE if any petitions checked has 2.PetitionTypeCodeID in 6.CodeID list			
    //SET @HearingTypeIsANonHearingEvent=TRUE if selected HearingTypeCodeID in 5.CodeID			
    //SET @Has602PetitionChecked=TRUE if any checked 2.PetitionTypeCodeID IN 6.CodeID records			


    var PetitionChecked602 = false;
    var HearingTypeIsANonHearingEvent = false;
    var Has602PetitionChecked = false;
    var IsResultContinuance = false;

    $('.chkPetition:checked').each(function () {
        if (Contains($('#PetitionType602List').val(), $(this).attr('data-petitiontypecodeid'))) {
            PetitionChecked602 = true;
            Has602PetitionChecked = true;
        }
    })
    if (Contains($('#HearingTypeNonHearingEventList').val(), $('#HearingTypeID').val())) {
        HearingTypeIsANonHearingEvent = true;
    }
    if (!HearingTypeIsANonHearingEvent) {
        if (DataValidation_RequireJudgeFlag == '1' && !Has602PetitionChecked) {
            if ($('#HearingOfficerID').val() == '') {
                isValid = false;
                $('#HearingOfficerID').focus();
                notifyDanger('Hearing Officer is required.');
                return false;
            }
        }
        if ($('#DepartmentID').val() == '') {
            isValid = false;
            $('#DepartmentID').focus();
            notifyDanger('Department is required.');
            return false;
        }
    }

    //**if the Global Result or a Result selected has CodeID in any records of 15.CodeID, then the Continuance Request By is required
    $('.PetitionResultID').each(function () {
        if (Contains($('#HearingResultContinuanceList').val(), $(this).val()) && $(this).val() != '') {

            IsResultContinuance = true;
        }
    })
    if (Contains($('#HearingResultContinuanceList').val(), $('#GlobalResultID').val()) && $('#GlobalResultID').val() != '') {
        IsResultContinuance = true;
    }
    if (IsResultContinuance && $('#ContinuanceRequestedByID').val() == '') {

        isValid = false;
        $('#ContinuanceRequestedByID').focus();
        notifyDanger('Requested by is a required field if a continuance result is selected.');
        return false;
    }




    if (Has602PetitionChecked) {

        //        10a. The selected 1.HearingTypeCodeID must be in 7.CodeID records			
        //        Display Error Message:	602 petition is on this hearing therefore the hearing type must be one of the following: 		
        //            **list out all the use the 7.CodeValue records comma delimited		

        if (!Contains($('#HearingType602PetitionList').val(), $('#HearingTypeID').val())) {
            isValid = false;



            AlertBoxWithCallback("602 petition is on this hearing therefore the hearing type must be one of the following:<br/>" + $('#HearingType602PetitionListText').val(), function () {

                $('#HearingTypeID').focus();

            });
            return false;
        }


        //        10b. The selected 1.HearingCourtDepartmentCodeID must be in 9.CodeID records			
        //        Display Error Message:	602 petition is on this hearing therefore the court department must be one of the following:		
        //            **list out all the use the 9.CodeValue records comma delimited		
        if (!Contains($('#CourtDepartment602PetitionList').val(), $('#DepartmentID').val())) {
            isValid = false;



            AlertBoxWithCallback("602 petition is on this hearing therefore the court department must be one of the following:<br/>" + $('#CourtDepartment602PetitionListText').val(), function () {

                $('#DepartmentID').focus();

            });
            return false;
        }

        //        10c. The selected 2.PetitionResultID (and Global Result if selected) must be in 8.CodeID records			
        //Display Error Message:	602 petition is on this hearing therefore the hearing result must be one of the following: 		
        //            **list out all the use the 8.CodeValue records comma delimited		
        var NotInHearingResult602PetitionList = false;
        var elm;
        $('.PetitionResultID').each(function () {
            if (!Contains($('#HearingResult602PetitionList').val(), $(this).val()) && $(this).val() !== null) {
                NotInHearingResult602PetitionList = true;
                elm = $(this);
            }
        })
        if (!Contains($('#HearingResult602PetitionList').val(), $('#GlobalResultID').val()) && $('#GlobalResultID').val() != '') {
            NotInHearingResult602PetitionList = true;
            elm = $('#GlobalResultID');
        }
        if (NotInHearingResult602PetitionList) {
            isValid = false;



            AlertBoxWithCallback("602 petition is on this hearing therefore the hearing result must be one of the following:<br/>" + $('#HearingResult602PetitionListText').val(), function () {
                $(elm).focus();

            });
            return false;
        }

    }


    if ($('.PetitionResultID option:selected').length > 0) {

        $('.PetitionResultID').each(function () {
            if ($(this).val() > 0 && !$(this).parent().parent().find('.chkPetition').is(':checked')) {
                isValid = false;

                $(this).parent().parent().find('.chkPetition').focus();
                notifyDanger('If a result is entered for a petition, the ON checkbox must be checked.');
                return false;

            }
        })

    }
    if (!isValid)
        return false;


    //12	If the Hearing Date is in the future, any results (Global or individual result) selected must be in 4.CodeID records			
    //	Can not update future hearing with this result.		
    if (moment($('#HearingDate').val()) > moment()) {
        var IsInHearingResultFutureHearingsList = true;
        var elm;

        //Check Global result is available in the 4.CodeID
        if ($('#GlobalResultID').val() > 0) {
            if (Contains($('#HearingResultFutureHearingsList').val(), $('#GlobalResultID').val())) {
                IsInHearingResultFutureHearingsList = true;
            }
            else {
                IsInHearingResultFutureHearingsList = false;
            }
            elm = $('#GlobalResultID');
        }

        //Check Individual result is available in the 4.CodeID
        $('.PetitionResultID').each(function () {
            if (IsInHearingResultFutureHearingsList) {
                if (Contains($('#HearingResultFutureHearingsList').val(), $(this).val()) && $(this).val() > 0) {
                    IsInHearingResultFutureHearingsList = true;
                    elm = $(this);
                } else if ($(this).val() > 0) {
                    IsInHearingResultFutureHearingsList = false;
                    elm = $(this);
                }
            }
        });

        //if (Contains($('#HearingResultFutureHearingsList').val(), $('#GlobalResultID').val()) && $('#GlobalResultID').val() > 0) {
        //    IsInHearingResultFutureHearingsList = true;
        //    elm = $('#GlobalResultID');
        //}


        if (!IsInHearingResultFutureHearingsList) {
            isValid = false;
            $(elm).focus();
            notifyDanger('Can not update future hearing with this result.');
            return false;
        }
    }
    /*
    if (moment($('#HearingDate').val()) > moment()) {
        var IsInHearingResultFutureHearingsList = false;
        var elm;
        $('.PetitionResultID').each(function () {
            if (Contains($('#HearingResultFutureHearingsList').val(), $(this).val()) && $(this).val() != '') {
                IsInHearingResultFutureHearingsList = true;
                elm = $(this);
            }
        })
        if (Contains($('#HearingResultFutureHearingsList').val(), $('#GlobalResultID').val()) && $('#GlobalResultID').val() != '') {
            IsInHearingResultFutureHearingsList = true;
            elm = $('#GlobalResultID');
        }

        if (IsInHearingResultFutureHearingsList) {
            isValid = false;
            $(elm).focus();
            notifyDanger('Can not update future hearing with this result.');
            return false;
        }
    }
    */
    var resultSelected = false;
    $('.PetitionResultID').each(function () {

        if ($(this).val() > 0 && !Contains($('#HearingResultListForHoursNotRequiredValidation').val(), $(this).val())) {
            resultSelected = true;
        }
    })
    if (!resultSelected) {
        if ($('#GlobalResultID').val() > 0 && !Contains($('#HearingResultListForHoursNotRequiredValidation').val(), $('#GlobalResultID').val())) {
            resultSelected = true;
        }
    }
    if ($('#Hours').is(':visible') &&  ($('#Hours').val() == '') && GetHoursRequiredFlag($('#WorkHoursRequiredFlag').val()) == '1' && resultSelected) {
        isValid = false;
        $('#Hours').focus();
        notifyDanger('Hours is required.');
        return false;
    }
    if ($('#Hours').is(':visible') && $('#Hours').val() <= 0 && GetHoursRequiredFlag($('#WorkHoursRequiredFlag').val()) == '1' && resultSelected) {

        isValid = false;
        $('#Hours').focus();
        notifyDanger('Hours worked must be a positive numeric value.');
        return false;
    }
    if ($('.chkPetition:checked').length == 0) {
        isValid = false;
        $('.chkPetition:first').focus();
        notifyDanger('At least one petition is required per hearing.');
        return false;
    } else {
        var count = 0;
        $('.PetitionResultID').each(function () {
            if ($(this).val() > 0) {

                count++;
            }
        })
        if (count > 0 && count != $('.chkPetition:checked').length) {

            isValid = false;
            $('.chkPetition:first').focus();
            notifyDanger('All petitions must be resulted if one petition is resulted.');
            return false;
        }

    }


    return isValid;
}



function Save(buttonId) {

    IPadKeyboardFix();
    if (!IsValidFormRequest()) {
        return;
    }
    var isvalid = Validation(buttonId);

    if (isvalid) {

        if ($('#hidDepartmentID').val() != $('#DepartmentID').val()) {
            confirmBox("Do you want to update the case department to be this hearing department?", function (result) {
                if (result) {
                    SaveData(true, buttonId);
                } else {
                    SaveData(false, buttonId);
                }
            });


        } else {
            SaveData(false, buttonId);
        }
    }
}

function SaveData(updateDepartment, buttonId) {
    var petitionsSelectedIds = '';
    var haOrderedToAppear = [];
    $('.chkPetition').each(function () {
        var resultId = $(this).parent().parent().find('.PetitionResultID').val() == null ? '' : $(this).parent().parent().find('.PetitionResultID').val();
        var orderedToAppear = ($(this).closest('tr').find('.chkOrderedToAppear').length > 0 && $(this).closest('tr').find('.chkOrderedToAppear').is(':checked')) ? 1 : 0;

        if ($(this).is(':checked') || $(this).data('selected') == 1) {
            if ($(this).is(':checked')) {
                if (petitionsSelectedIds != "")
                    petitionsSelectedIds += ',';
                petitionsSelectedIds += $(this).attr('value') + '|' + resultId + '|' + $(this).data('petitionclosedate') + '|' + $(this).data('hearingpetitionkey') + '|1|' + ($(this).parent().parent().find('.PetitionResultID').IsValueChanged() ? 1 : 0) + '|' + $(this).data('selected') + '|' + orderedToAppear;


            } else if ($(this).data('hearingpetitionkey') > 0) {
                if (petitionsSelectedIds != "")
                    petitionsSelectedIds += ',';
                petitionsSelectedIds += $(this).attr('value') + '|' + resultId + '|' + $(this).data('petitionclosedate') + '|' + $(this).data('hearingpetitionkey') + '|' + ($(this).is(':checked') ? 1 : 0) + '|' + ($(this).parent().parent().find('.PetitionResultID').IsValueChanged() ? 1 : 0) + '|' + $(this).data('selected') + '|' + orderedToAppear;

            }
            var tdParent = $(this).closest('tr').find('.chkOrderedToAppear').parent();
            if ($(this).closest('tr').find('.chkOrderedToAppear').IsCheckboxChanged()) {

                haOrderedToAppear.push({
                    'HA_ID': tdParent.data('ha-id'),
                    'HA_RoleID': tdParent.data('ha-roleid'),
                    'HA_AttendedFlag': tdParent.data('ha-attendedflag'),
                    'HA_CounselPersonID': tdParent.data('ha-counselpersonid'),
                    'HA_FillinCounselPersonID': tdParent.data('ha-fillincounselpersonid'),
                    'HA_Placement': tdParent.data('ha-placement'),
                    'HA_AppearanceRequiredFlag': orderedToAppear,
                });
            }


        }



    });


    var model = {
        'HearingID': $('#HearingID').val(),
        'HearingTypeID': $('#HearingTypeID').val(),
        'HearingDate': $('#HearingDate').val(),
        'HearingTime': $('#TimeHours').val() + ':' + $('#Minutes').val() + ' ' + $('#TimeAmPm').val(),
        'HearingOfficerID': $('#HearingOfficerID').val(),
        'DepartmentID': $('#DepartmentID').val(),
        'AppearingAttorneyID': $('#AppearingAttorneyID').val(),
        'Hours': $('#Hours').val(),
        'HourTypeID': $('#HourTypeID').val(),
        'PhaseID': $('#PhaseID').val(),
        'MediaPresent': $('#MediaPresent').is(':checked'),
        'Note': $('#Note').GetHtml(),
        'PetitionsSelectedIds': petitionsSelectedIds,
        'UpdateDepartment': updateDepartment,
        'GlobalResultID': $('#GlobalResultID').val(),
        'ContinuanceRequestedByID': $('#ContinuanceRequestedByID').val(),
        'HearingAttendanceID': hearingAttendanceID,
        'NoteID': noteId,
        'WorkID': workId,
        'buttonId': buttonId,
        'OrderedToAppearModelList': haOrderedToAppear
    }
    var params = model;
    console.log(params)
    $.ajax({
        type: "POST",
        url: '/QHE/QHEHearingUpdate',
        data: { model: params },
        success: function (result) {
            if (result.isSuccess) {
                notifySuccess('Data Saved Successfully!.');
                RequestSubmitted();
                if (result.AlertMessage.length > 0) {
                    AlertMessage(result.AlertMessage, buttonId);

                } else {
                    if (buttonId == 2) {
                        document.location.href = '/QHE/QHENextHearing/' + $encryptedHearingId;
                    } else if (buttonId == 3) {
                        backToSourcePage();
                    } else if (buttonId == 4) {
                        if (result.nextHearingId != '') {
                            document.location.href = "/QHE/QHEHearing/" + result.nextHearingId;
                        } else {
                            backToSourcePage()
                        }
                    } else if (buttonId == 5) {
                        document.location.href = wizardUrl;
                        return false;
                    } else {
                        document.location.href = '/QHE/QHEAttendance/' + $encryptedHearingId;
                    }
                }
            } else {
                document.location.href = result.URL;
            }
        },
        dataType: 'json'
    });
}

function backToSourcePage() {
    if (entryPageId == 1) {
        document.location.href = '/Inquiry/MyJcatsAtty';
    }
    else {
        document.location.href = '/Inquiry/MyCalendar?p=' + calnderPersonId;
    }
}
$('#saveAndContinue').on('click', function () {
    Save(1);
});

$('#saveAndAdd').on('click', function () {
    Save(2);
});

$('#saveAndExit').on('click', function () {
    Save(3);
});

$('#SaveAndNextQHECase').on('click', function () {
    Save(4);
});
function AlertMessage(msg, buttonId) {
    bootbox.dialog({
        className: "modal-custom modal-message modal-primary confirm-box ",// "yellow",
        size: 400,
        message: msg,
        title: "Confirmation",
        header: "<i class='fa fa-warning'></i>",
        buttons: {
            yes: {
                label: "Go To Advisements",
                className: "btn-primary",
                callback: function () {
                    document.location.href = '/Case/Advisements';
                }
            }, no: {
                label: "Continue",
                className: "btn-secondary",
                callback: function () {
                    if (buttonId == 2) {
                        document.location.href = '/QHE/QHENextHearing/' + $encryptedHearingId;
                    } else if (buttonId == 3) {
                        backToSourcePage();
                    } else if (buttonId == 4) {
                        if (result.nextHearingId != '') {
                            document.location.href = "/QHE/QHEHearing/" + result.nextHearingId;
                        } else {
                            backToSourcePage()
                        }
                    } else if (buttonId == 5) {
                        document.location.href = wizardUrl;
                        return false;
                    } else {
                        document.location.href = '/QHE/QHEAttendance/' + $encryptedHearingId;
                    }
                }


            }
        }
    });
}