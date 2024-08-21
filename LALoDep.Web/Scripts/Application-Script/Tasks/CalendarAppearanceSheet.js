var validateTabClose = true;
var IsResultContinuance = false;
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
$(function () {
    $('select').each(function () {
        if ($(this).data('selected') !== undefined) {
            $(this).val($(this).data('selected'));
        }
    });
    $('#chkAllOrderBack').change(function () {
        $('.chkOrderBack').prop('checked', $(this).is(':checked'));
    });
    //$('#chkAllAsfa').change(function () {
    //    $('.chkAsfa').prop('checked', $(this).is(':checked'));
    //});
    $('.chkNoChangeToStatusAll').change(function () {
        $('.chkNoChangeToStatus').prop('checked', $(this).is(':checked'));
    });
   
    setInitialFormValues('AppearanceSheet', true);

    AutoSaveNote('HearingPrepNoteEntry', 'HearingPrepNoteEntry-' + $('#HearingID').val(), 1, 'Hearing Preparation Note', function () {

        AutoSaveNote('HearingNoteGetMain_NoteEntry', 'HearingNoteGetMain_NoteEntry-' + $('#HearingID').val(), 1, 'General Note', function () {

        });
    });
    

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
$('#NoteTypeCodeID').change(function () {
    if ($('#NoteTypeCodeID').val() == '') {
        $('#btnAddNote').focus();
        notifyDanger('Note Type is required.');
        return false;
    }


    SaveData(3, '/Task/CalendarAppearanceSheetNotes/?hearingId=' + $(this).attr('data-id') + '&codeId=' + $('#NoteTypeCodeID').val(), 'Add Note');
});

$('#btnSaveBackToCalendar').click(function () {

    // window.location.href = '/Task/QuickCalMyCalendar?HearingDate=' + $('.hearingDate').data('date');
    SaveData(2);
});
$redirect_toclientedit = '';

$(".btnRaceNeeded").on("click", function () {
    $redirect_toclientedit = $(this).data('href');
   

    SaveData(8);

});


$(".lnkEditCurrentErh").on("click", function () {
    SaveData(6);

});

$("#btnAdditionalAttendees").on("click", function () {
    SaveData(7);

});
$("#btnSaveAndNextHearing").on("click", function () {
    SaveData(1);

});
$("#btnSaveAndReturn").on("click", function () {
    SaveData(4);

});
$("#btnHearingPrepNoteTotalPrepHoursLink").on("click", function () {
    SaveData(5);

});
function SaveData(buttonId, noteUrl, title) {


    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return;
    }
    var isvalid = Validation(buttonId, noteUrl, title);


    if (isvalid) {
        if ($('#HearingPrepNoteHours').val() != '' && $('#HearingPrepNoteHours').val() > 2) {
            confirmBox('Hearing Preparation Hours is greater than 2 hours.  Is this Correct?', function (result) {
                if (result) {
                    HearingPreparationHoursConfirmationSave(buttonId, noteUrl, title);
                } else {
                    $('#HearingPrepNoteHours').focus();
                }
            });
        }
        else {
            HearingPreparationHoursConfirmationSave(buttonId, noteUrl, title);
        }

    }
}

function HearingPreparationHoursConfirmationSave(buttonId, noteUrl, title) {
    var params = GetData();
    var countDeleteClientStatus = 0;
    $('.trHearingAttendanceClients').each(function () {
        if ($(this).find('.CS_CodeID').IsValueChanged() && $(this).find('.CS_CodeID').val() == '') {
            countDeleteClientStatus++;
        }

    });
    if (countDeleteClientStatus > 0) {
        var msg = countDeleteClientStatus > 1 ? "Are you sure you want to remove this Current Status for these Current Status Dates?" : "Are you sure you want to remove this Current Status for this Current Status Date?";
        confirmBox(msg, function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: '/Task/CalendarAppearanceSheet',
                    data: { model: params },
                    success: function (result) {
						RequestSubmitted();
						validateTabClose = false;
						if (result.Status == "Done") {
                            AutoSaveNote('HearingPrepNoteEntry', 'HearingPrepNoteEntry-' + $('#HearingID').val(), 2);
                            AutoSaveNote('HearingNoteGetMain_NoteEntry', 'HearingNoteGetMain_NoteEntry-' + $('#HearingID').val(), 2);

                            if (result.AlertMessage.length > 0) {
                                AlertMessage(result.AlertMessage, buttonId);

                            } else {
                                if (buttonId == 2) {
                                    BackToCalendar();
                                }
                                else if (buttonId == 3) {
                                    OpenPopup(noteUrl, title);
                                } else if (buttonId == 4) {
                                    document.location.href = document.location.href;
                                } else if (buttonId == 5) {

                                    OpenCustomPopup('/Task/HearingPreparationHours/' + $('#btnSaveAndNextHearing').attr('data-hearingid'), 500, 300, 'Hearing Preparation Hours History')

                                }
                                else if (buttonId == 6) {
                                    OpenPopup('/Task/QuickCalCurrentERH/' + $('#btnSaveAndNextHearing').attr('data-hearingid'), 'Current Ed Rights Holder')
                                } else if (buttonId == 7) {
                                    OpenCustomPopup('/Task/AdditionalAttendees/' + $('#btnSaveAndNextHearing').attr('data-hearingid'), 500, 300, 'Additional Attendees')
                                } else if (buttonId == 8) {
                                    document.location.href = $redirect_toclientedit;
                                }
                                else {
                                    OpenPopup('/Task/QuickCalNextHearing/' + $('#btnSaveAndNextHearing').attr('data-hearingid'), 'Next Hearing')
                                }
                            }

                        } else {
                            document.location.href = result.URL;

                        }
                    },
                    dataType: 'json'
                });
            }
        });
    } else {
        $.ajax({
            type: "POST",
            url: '/Task/CalendarAppearanceSheet',
            data: { model: params },
            success: function (result) {
				RequestSubmitted();
				validateTabClose = false;
                if (result.Status == "Done") {
                    AutoSaveNote('HearingPrepNoteEntry', 'HearingPrepNoteEntry-' + $('#HearingID').val(), 2);
                    AutoSaveNote('HearingNoteGetMain_NoteEntry', 'HearingNoteGetMain_NoteEntry-' + $('#HearingID').val(), 2);

                    if (result.AlertMessage.length > 0) {
                        AlertMessage(result.AlertMessage, buttonId);

                    } else {
                        if (buttonId == 2) {
                            BackToCalendar();
                        }
                        else if (buttonId == 3) {
                            OpenPopup(noteUrl, title);
                        } else if (buttonId == 4) {
                            document.location.href = document.location.href;
                        } else if (buttonId == 5) {

                            OpenCustomPopup('/Task/HearingPreparationHours/' + $('#btnSaveAndNextHearing').attr('data-hearingid'), 500, 300, 'Hearing Preparation Hours History')

                        } else if (buttonId == 6) {
                            OpenPopup('/Task/QuickCalCurrentERH/' + $('#btnSaveAndNextHearing').attr('data-hearingid'), 'Current Ed Rights Holder')
                        } else if (buttonId == 7) {
                            OpenCustomPopup('/Task/AdditionalAttendees/' + $('#btnSaveAndNextHearing').attr('data-hearingid'), 500, 300, 'Additional Attendees')
                        } else if (buttonId == 8) {
                            document.location.href = $redirect_toclientedit;
                        }
                        else {
                            OpenPopup('/Task/QuickCalNextHearing/' + $('#btnSaveAndNextHearing').attr('data-hearingid'), 'Next Hearing')
                        }
                    }
                } else {
                    document.location.href = result.URL;

                }
            },
            dataType: 'json'
        });
    }
}
function BackToCalendar() {
    if ($('#RedirectToPage').val() !== '') {
        window.location.href = $('#RedirectToPage').val();

    } else {
        window.location.href = '/Task/QuickCalMyCalendar?HearingDate=' + $('.hearingDate').data('date');
    }

}
function Validation(buttonId, noteUrl, title) {
    var isValid = true;
    associations = [];

    if (!hasFormChanged('AppearanceSheet')) {

        if (buttonId == 4) {
            notifyDanger('Nothing was changed.');
            isValid = false;
            return false;
		} else if (buttonId == 2) {
			 
            BackToCalendar();
            isValid = false;
            return false;
        }
        else if (buttonId == 3) {
            OpenPopup(noteUrl, title);
            isValid = false;
            return false;
        } else if (buttonId == 5) {

            OpenCustomPopup('/Task/HearingPreparationHours/' + $('#btnSaveAndNextHearing').attr('data-hearingid'), 500, 300, 'Hearing Preparation Hours History')
            isValid = false;
            return false;
        } else if (buttonId == 6) {
            OpenPopup('/Task/QuickCalCurrentERH/' + $('#btnSaveAndNextHearing').attr('data-hearingid'), 'Current Ed Rights Holder')
            isValid = false;
            return false;
        } else if (buttonId == 7) {
            OpenCustomPopup('/Task/AdditionalAttendees/' + $('#btnSaveAndNextHearing').attr('data-hearingid'), 500, 300, 'Additional Attendees')
            return false;
        } else if (buttonId == 8) {
            document.location.href = $redirect_toclientedit;
        }
        else {
            //  notifyDanger('Nothing was changed.');
            OpenPopup('/Task/QuickCalNextHearing/' + $('#btnSaveAndNextHearing').attr('data-hearingid'), 'Next Hearing');
            isValid = false;
            return false;
        }
    }
    if ($('#Hours').val() != '' || ($('#PhaseID').val() != '' && $('#PhaseID').val() != null) || ($('#HourTypeID').val() != '' && $('#HourTypeID').val() != null)) {
        if ($('#AppearingAttorneyID').val() == '') {
            isValid = false;
            $('#AppearingAttorneyID').focus();
            notifyDanger('Appearing Attorney is required.');
            return false;
        }
        if ($('#Hours').is(':visible') && $('#Hours').val() == '' && GetHoursRequiredFlag($('#DataValidation_RequireHearingHoursFlag').val()) == '1') {
            isValid = false;
            $('#Hours').focus();
            notifyDanger('Hours is required.');
            return false;
        }
        if ($('#Hours').is(':visible') && ($('#HourTypeID').val() == '' || $('#HourTypeID').val() == null) && GetHoursRequiredFlag($('#DataValidation_RequireHearingHoursFlag').val()) == '1') {
            isValid = false;
            $('#HourTypeID').focus();
            notifyDanger('Hour Type is required.');
            return false;
        }
        if (($('#PhaseID').val() == '' || $('#PhaseID').val() == null) && $('#PhaseID').is(':visible')) {
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


    $('.trDCC').each(function () {
        var roleId = $(this).find('.AttendedFlagCell').attr('data-roleid');

        var presentYesOrNo = $('#AttendedFlagYes' + roleId).is(':checked') ? 1 : $('#AttendedFlag' + roleId).is(':checked') ? 0 : -1;
        if (presentYesOrNo == 1) {

            if ($(this).find('.CounselPersonID').val() == null && $(this).find('.FillinCounselPersonID').val() == null) {

                //  isValid = false;
                // $(this).find('.CounselPersonID').focus();
                // notifyDanger('Role Call DCC or Fill-in DCC is required if Present.');
                //  return false;
            }

        }

    });
    var presentCSWYesOrNo = $('#CSWFlagYes' + $('#HearingID').val()).is(':checked') ? 1 : $('#CSWFlag' + $('#HearingID').val()).is(':checked') ? 0 : -1;


    if ($('#CSW').val().length > 0 && presentCSWYesOrNo == -1) {
        isValid = false;
        $('#CSWFlagYes' + $('#HearingID').val()).focus();
        notifyDanger('CSW Present Y/N is required if CSW is entered.');
        return false;
    }

    if (!isValid)
        return false;
    if ($('#HearingPrepNoteHours').val() != '' && !($('#HearingPrepNoteHours').val() > 0 && $('#HearingPrepNoteHours').val() <= 8)) {
        isValid = false;
        $('#HearingPrepNoteHours').focus();
        notifyDanger('Hearing Preparation Hours must be between .1 and 8 hours');
        return false;
    }



    if ($('.chkPetition:checked').closest('tr').find('.PetitionResultID option:selected[value!=""]').length > 0 || ($('.chkPetition:checked').length > 0 && $('#GlobalResultID').val() > 0)) {

        var flagClientStatusChange = false;

        if ($('.PetitionResultID[data-selected="0"]').length > 0) {
            if ($('.PetitionResultID[data-selected="0"]  option:selected[value!=""]').length > 0 || $('#GlobalResultID').val() > 0) {
                flagClientStatusChange = true;
            }
        }

        $('.trHearingAttendanceClients').each(function () {
            var roleId = $(this).find('.AttendedFlagCell').attr('data-roleid');
            if (!isValid)
                return false;

            if (flagClientStatusChange) {
                if ($(this).find('.CS_CodeID').length > 0) {
                    if (($(this).find('.CS_CodeID').val() == $(this).find('.CS_CodeID').attr('data-selected') || ($(this).find('.CS_CodeID').val() == '' || $(this).find('.CS_CodeID').val() == null)) && !$(this).find('.chkNoChangeToStatus').is(':checked')) {
                        isValid = false;
                        $(this).find('.CS_CodeID').focus();
                        notifyDanger('When the hearing is resulted then require either a change to the Current Status dopdown value or the "No Change to Status" checkbox to be checked.');
                        return false;
                    }
                }

            }
            var presentYesOrNo = $('#AttendedFlagYes' + roleId).is(':checked') ? 1 : $('#AttendedFlag' + roleId).is(':checked') ? 0 : -1;
            if (presentYesOrNo == -1) {
                isValid = false;
                $('#AttendedFlagYes' + roleId).focus();
                notifyDanger('Present Yes or No is required.');
                return false;
            }
        });
        if (!isValid)
            return false;
        $('.trHearingAttendanceNonClients').each(function () {
            var roleId = $(this).find('.AttendedFlagCell').attr('data-roleid');

            var presentYesOrNo = $('#AttendedFlagYes' + roleId).is(':checked') ? 1 : $('#AttendedFlag' + roleId).is(':checked') ? 0 : -1;
            if (presentYesOrNo == -1) {
                isValid = false;
                $('#AttendedFlagYes' + roleId).focus();
                notifyDanger('Present Yes or No is required.');
                return false;
            }

        });



        if (!isValid)
            return false;
    }






    //SET @602PetitionChecked=TRUE if any petitions checked has 2.PetitionTypeCodeID in 6.CodeID list			
    //SET @HearingTypeIsANonHearingEvent=TRUE if selected HearingTypeCodeID in 5.CodeID			
    //SET @Has602PetitionChecked=TRUE if any checked 2.PetitionTypeCodeID IN 6.CodeID records			


    var PetitionChecked602 = false;
    var HearingTypeIsANonHearingEvent = false;
    var Has602PetitionChecked = false;


    $('.chkPetition:checked').each(function () {
        if (Contains($('#PetitionType602List').val(), $(this).attr('data-petitiontypecodeid'))) {
            PetitionChecked602 = true;
            Has602PetitionChecked = true;
        }
    })
    if (Contains($('#HearingTypeNonHearingEventList').val(), $('#HearingTypeID').val())) {
        HearingTypeIsANonHearingEvent = true;
    }
    //if (!HearingTypeIsANonHearingEvent) {
    //    if (DataValidation_RequireJudgeFlag == '1' && !Has602PetitionChecked) {
    //        if ($('#HearingOfficerID').val() == '') {
    //            isValid = false;
    //            $('#HearingOfficerID').focus();
    //            notifyDanger('Hearing Officer is required.');
    //            return false;
    //        }
    //    }

    //}


    //**if the Global Result or a Result selected has CodeID in any records of 15.CodeID, then the Continuance Request By is required
    $('.PetitionResultID').each(function () {
        if (Contains($('#HearingResultContinuanceList').val(), $(this).val()) && $(this).hasValue()) {

            IsResultContinuance = true;
        }
    })
    if (Contains($('#HearingResultContinuanceList').val(), $('#GlobalResultID').val()) && $('#GlobalResultID').val() != '') {
        IsResultContinuance = true;
    }
    if (IsResultContinuance && $('#ContinuanceRequestedByID').val() == '') {

        //isValid = false;
        //$('#ContinuanceRequestedByID').focus();
        //notifyDanger('Requested by is a required field if a continuance result is selected.');
        //return false;
    }
    if (!isValid)
        return false;



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
            if (!Contains($('#HearingResult602PetitionList').val(), $(this).val()) && $(this).hasValue()) {
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
    if ($('#Hours').is(':visible') && ($('#Hours').val() == '') && GetHoursRequiredFlag($('#WorkHoursRequiredFlag').val()) == '1' && resultSelected) {
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
        //  debugger
        var count = 0; var countChecked = 0;
        $('.PetitionResultID').each(function () {
            if ($(this).val() > 0) {
                if (!Contains($('#HearingResultFutureHearingsList').val(), $(this).val())) {
                    count++;

                }
            } else {
                if ($(this).parents('tr').find('.chkPetition:checked').length > 0) {
                    $(this).focus();
                    countChecked++;
                }
            }

        })
        if ($("#GlobalResultID").val() == '' && count > 0 && countChecked > 0) {

            isValid = false;
            //  $('.chkPetition:first').focus();
            notifyDanger('All petitions must be resulted if one is resulted.');
            return false;
        }

    }
    isValid = HearingContinuanceReasonAndRequestValidation(IsResultContinuance);


    return isValid;
}
function GetData() {
    var updateHearing = 0;
    if ($('#CourtOfficer').IsValueChanged() || $('#CourtReporter').IsValueChanged() || $('#NoticeProperFlag').IsCheckboxChanged() || $('#ReasonableEffortFlag').IsCheckboxChanged() || $('#MediaPresentFlag').IsCheckboxChanged() || $('#CourtOfficer').IsValueChanged() || $('#CSW').IsValueChanged() || $('#CSWFlagYes' + $('#HearingID').val()).IsRadioChanged() || $('#CSWFlag' + $('#HearingID').val()).IsRadioChanged()) {
        updateHearing = 1;
    }
    var petitionsSelectedIds = '';


    $('.chkPetition').each(function () {
        var $tr = $(this).parent().parent();
        var resultId = $tr.find('.PetitionResultID').val() == null ? '' : $tr.find('.PetitionResultID').val();
        if ($(this).IsCheckboxChanged() || $tr.find('.PetitionResultID').IsValueChanged() || (($tr.find('.PetitionResultID').val() == '' || $tr.find('.PetitionResultID').val() == null) && $('#GlobalResultID').IsValueChanged())) {

            if ($(this).is(':checked') || $(this).data('selected') == 1) {
                if ($(this).is(':checked')) {
                    if (petitionsSelectedIds != "")
                        petitionsSelectedIds += ',';
                    //petitionsSelectedIds += $(this).attr('value') + '|' + resultId + '|' + $(this).data('petitionclosedate') + '|' + $(this).data('hearingpetitionkey') + '|1|' + (($(this).parent().parent().find('.PetitionResultID').IsValueChanged() || $(this).parent().parent().find('.chkAsfa').IsValueChanged() || $(this).parent().parent().find('.chkOrderBack').IsValueChanged()) ? 1 : 0) + '|' + $(this).data('selected') + '|' + ($(this).parent().parent().find('.chkOrderBack').is(':checked') ? 1 : 0) + '|' + ($(this).parent().parent().find('.chkAsfa').is(':checked') ? 1 : 0);
                    petitionsSelectedIds += $(this).attr('value') + '|' + resultId + '|' + $(this).data('petitionclosedate') + '|' + $(this).data('hearingpetitionkey') + '|1|' + (($(this).parent().parent().find('.PetitionResultID').IsValueChanged() || $(this).parent().parent().find('.chkOrderBack').IsValueChanged()) ? 1 : 0) + '|' + $(this).data('selected') + '|' + ($(this).parent().parent().find('.chkOrderBack').is(':checked') ? 1 : 0) + '|' + $(this).data("asfaflag");


                } else if ($(this).data('hearingpetitionkey') > 0) {
                    if (petitionsSelectedIds != "")
                        petitionsSelectedIds += ',';
                    //petitionsSelectedIds += $(this).attr('value') + '|' + resultId + '|' + $(this).data('petitionclosedate') + '|' + $(this).data('hearingpetitionkey') + '|' + ($(this).is(':checked') ? 1 : 0) + '|' + (($(this).parent().parent().find('.PetitionResultID').IsValueChanged() || $(this).parent().parent().find('.chkAsfa').IsValueChanged() || $(this).parent().parent().find('.chkOrderBack').IsValueChanged()) ? 1 : 0) + '|' + $(this).data('selected') + '|' + ($(this).parent().parent().find('.chkOrderBack').is(':checked') ? 1 : 0) + '|' + ($(this).parent().parent().find('.chkAsfa').is(':checked') ? 1 : 0);
                    petitionsSelectedIds += $(this).attr('value') + '|' + resultId + '|' + $(this).data('petitionclosedate') + '|' + $(this).data('hearingpetitionkey') + '|' + ($(this).is(':checked') ? 1 : 0) + '|' + (($(this).parent().parent().find('.PetitionResultID').IsValueChanged() || $(this).parent().parent().find('.chkOrderBack').IsValueChanged()) ? 1 : 0) + '|' + $(this).data('selected') + '|' + ($(this).parent().parent().find('.chkOrderBack').is(':checked') ? 1 : 0) + '|' + $(this).data("asfaflag");

                }
            }
        }
    });
    var cSWPresentFlag = $('#CSWFlagYes' + $('#HearingID').val()).is(':checked') ? 1 : $('#CSWFlag' + $('#HearingID').val()).is(':checked') ? 0 : null;

    var model = {
        HearingID: $('#HearingID').val(),
        OfficerPersonID: $('#OfficerPersonID').val(),
        CourtReporter: $('#CourtReporter').val(),
        CourtOfficer: $('#CourtOfficer').val(),
        NoticeProperFlag: $('#NoticeProperFlag').is(':checked'),
        ReasonableEffortFlag: $('#ReasonableEffortFlag').is(':checked'),

        MediaPresentFlag: $('#MediaPresentFlag').is(':checked'),
        'HearingAttendanceID': hearingAttendanceID,
        AppearingAttorneyID: $('#AppearingAttorneyID').val(),
        UpdateHearing: updateHearing,

        HearingAttendance: [],
        'PetitionsSelectedIds': petitionsSelectedIds,
        'GlobalResultID': $('#GlobalResultID').val(),
        'ContinuanceRequestedByID': $('#ContinuanceRequestedByID').val(),
        CurrentDCCList: [],
        HearingNoteGetMain: null,
        'Hours': $('#Hours').val(),
        'HourTypeID': $('#HourTypeID').val(),
        'PhaseID': $('#PhaseID').val(),
        'IsHoursChanged': ($('#PhaseID').IsValueChanged() || $('#HourTypeID').IsValueChanged() || $('#Hours').IsValueChanged()),
        'HearingPrepNoteEntry': $('#HearingPrepNoteEntry').GetHtml(),
        'HearingPrepNoteEntryChanged': $('#HearingPrepNoteEntry').data("old-value-on-pageload") !== $('#HearingPrepNoteEntry').GetHtml(),
        'HearingPrepNoteHours': $('#HearingPrepNoteHours').val(),
        'HearingPrepNoteID': $('#HearingPrepNoteID').val(),
        'CSW': $('#CSW').val(),
        'CSWPresentFlag': cSWPresentFlag,
    };

    if ($('#HearingNoteGetMain_NoteEntry').data("old-value-on-pageload") !== $('#HearingNoteGetMain_NoteEntry').GetHtml()) {
        model.HearingNoteGetMain = {
            NoteEntry: $('#HearingNoteGetMain_NoteEntry').GetHtml(),
            NoteID: $('#HearingNoteGetMain_NoteID').val(),
            NoteTypeCodeID: $('#HearingNoteGetMain_NoteTypeCodeID').val(),
        };
    }

    $('.trHearingAttendanceClients').each(function () {
        var roleId = $(this).find('.AttendedFlagCell').attr('data-roleid');
        model.HearingAttendance.push({
            AppearanceRequiredFlag: $(this).find('.AttendedFlagCell').attr('data-appearancerequiredflag'),
            ClientFlag: 1,
            RoleID: roleId,
            HearingAttendanceID: $(this).find('.AttendedFlagCell').attr('data-id'),
            AttendedFlag: $('#AttendedFlagYes' + roleId).is(':checked') ? 1 : $('#AttendedFlag' + roleId).is(':checked') ? 0 : -1,
            CounselPersonID: '',
            FillinCounselPersonID: '',
            PersonID: $(this).find('.AttendedFlagCell').attr('data-personid'),
            ICWAFlag: $(this).find('.chkICWA').is(':checked') ? 1 : 0,
            Placement: $(this).find('.txtPlacement').val(),
            CS_StartDate: $(this).find('.CS_StartDate').attr('data-value'),
            CS_CodeID: $(this).find('.CS_CodeID').val(),
            CS_ID: $(this).find('.chkNoChangeToStatus').attr('data-cs-id'),
            CS_PersonID: $(this).find('.chkNoChangeToStatus').attr('data-cs-personid'),
            UpdateHearingAttendenceRecord: $(this).find('.chkICWA').IsCheckboxChanged() || $(this).find('.txtPlacement').IsValueChanged() || $('#AttendedFlagYes' + roleId).IsRadioChanged() || $('#AttendedFlag' + roleId).IsRadioChanged(),
            UpdateCaseStatusRecord: $(this).find('.chkNoChangeToStatus').IsCheckboxChanged() ? false : $(this).find('.CS_CodeID').IsValueChanged(),

        });


    });

    $('.trHearingAttendanceNonClients').each(function () {
        var roleId = $(this).find('.AttendedFlagCell').attr('data-roleid');
        model.HearingAttendance.push({
            AppearanceRequiredFlag: $(this).find('.AttendedFlagCell').attr('data-appearancerequiredflag'),
            ClientFlag: 0,
            RoleID: roleId,
            HearingAttendanceID: $(this).find('.AttendedFlagCell').attr('data-id'),
            AttendedFlag: $('#AttendedFlagYes' + roleId).is(':checked') ? 1 : $('#AttendedFlag' + roleId).is(':checked') ? 0 : -1,
            CounselPersonID: $(this).find('#item_CounselPersonID').val(),
            FillinCounselPersonID: $(this).find('#item_FillinCounselPersonID').val(),
            PersonID: $(this).find('.AttendedFlagCell').attr('data-personid'),
            ICWAFlag: 0,
            Placement: '',

            UpdateHearingAttendenceRecord: $('#AttendedFlagYes' + roleId).IsRadioChanged() || $('#AttendedFlag' + roleId).IsRadioChanged() || $(this).find('#item_CounselPersonID').IsValueChanged()
                || $(this).find('#item_FillinCounselPersonID').IsValueChanged()
        });

    });

    $('.trDCC').each(function () {
        var roleId = $(this).find('.AttendedFlagCell').attr('data-roleid');
        if ($('#AttendedFlagYes' + roleId).IsRadioChanged() || $(this).find('#item_CounselPersonID').IsValueChanged() || $(this).find('#item_FillinCounselPersonID').IsValueChanged()) {
            model.CurrentDCCList.push({
                CurrentDCCRoleID: roleId,
                HearingAttendanceID: $(this).find('.AttendedFlagCell').attr('data-id'),
                AttendedFlag: $('#AttendedFlagYes' + roleId).is(':checked') ? 1 : $('#AttendedFlag' + roleId).is(':checked') ? 0 : -1,
                CounselPersonID: $(this).find('#item_CounselPersonID').val(),
                FillinCounselPersonID: $(this).find('#item_FillinCounselPersonID').val(),
                CurrentDCCPersonID: $(this).find('.AttendedFlagCell').attr('data-personid')
            });
        }
    });
    model = HearingContinuanceReasonAndRequestModelData(model);
    return model;
}



$("#lnkAddRoleCallDCC").on("click", function () {
    OpenCustomPopup('/Task/QuickCalDCCAndNewPrivateCounsel/' + $(this).data('id') + '?AddMode=DCC', 500, 300, 'Add New DCC Attorney to the Dropdown');
});

$("#lnkAddNewCounsel").on("click", function () {
    OpenCustomPopup('/Task/QuickCalDCCAndNewPrivateCounsel/' + $(this).data('id') + '?AddMode=PrivateCounsel', 500, 300, 'Add New Private Council to the Dropdown');
});

function DCCDropdownRefresh(selectedId) {
    ClosePopup();
    $.ajax({
        type: "POST",
        url: '/Task/QuickCalDCCAndNewPrivateCounselPopulateDropdown?hearingId=' + $('#HearingID').val() + '&mode=DCC',

        success: function (result) {

            if (result.Status == "Done") {
                ResetDropDownList('.trDCC .CounselPersonID', result.SelectList);
                $('.trDCC .CounselPersonID').each(function () {
                    if ($(this).data('selected') !== undefined) {
                        $(this).val($(this).data('selected'));
                    }
                });
                $('.CounselPersonID:first').val(selectedId);


            }
        },
        dataType: 'json'
    });
}
function PrivateCounselDropdownRefresh(selectedId) {
    ClosePopup();
    $.ajax({
        type: "POST",
        url: '/Task/QuickCalDCCAndNewPrivateCounselPopulateDropdown?hearingId=' + $('#HearingID').val() + '&mode=PrivateCounsel',

        success: function (result) {

            if (result.Status == "Done") {

                ResetDropDownList('.trHearingAttendanceNonClients .CounselPersonID', result.SelectList);
                ResetDropDownList('.trHearingAttendanceNonClients .FillinCounselPersonID', result.SelectList);
                if ($('.trHearingAttendanceNonClients').length == 1) {
                    $('.trHearingAttendanceNonClients .CounselPersonID').val(selectedId);
                    $('.trHearingAttendanceNonClients .FillinCounselPersonID').val(selectedId);
                }
                else {

                    $('.trHearingAttendanceNonClients .FillinCounselPersonID').each(function () {
                        if ($(this).data('selected') !== undefined) {
                            $(this).val($(this).data('selected'));
                        }
                    }); $('.trHearingAttendanceNonClients .CounselPersonID').each(function () {
                        if ($(this).data('selected') !== undefined) {
                            $(this).val($(this).data('selected'));
                        }
                    });
                }

            }
        },
        dataType: 'json'
    });

}

function ResetDropDownList(dropdownClass, selectList) {

    $(dropdownClass).each(function () {
        $obj = $(this);
        $obj.find('option').remove();
        var htmlEmpty = $('<option>').val('').text('');
        $obj.append(htmlEmpty);
        $.each(selectList, function () {
            var html = $('<option>').val(this.Value).text(this.Text);
            $obj.append(html);
        });
    });

}


setTimeout(function () {

    if ($('#selectParentPhase option[data-hearingtypeid="' + $('#HearingTypeID').val() + '"]').length > 0) {
        $('#PhaseID').html('  <option value=""></option>');
        $('#selectParentPhase option[data-hearingtypeid="' + $('#HearingTypeID').val() + '"]').each(function () {
            $('#PhaseID').append($(this).clone())
        });

    }
    else {

        $('#PhaseID').html('');
        $('#selectPhase option').each(function () {
            $('#PhaseID').append($(this).clone())
        });
    }
    if ($('#selectParentHourType option[data-hearingtypeid="' + $('#HearingTypeID').val() + '"]').length > 0) {
        $('#HourTypeID').html('  <option value=""></option>');
        $('#selectParentHourType option[data-hearingtypeid="' + $('#HearingTypeID').val() + '"]').each(function () {
            $('#HourTypeID').append($(this).clone())
        });

    }
    else {
        $('#HourTypeID').html('');
        $('#selectHourType option').each(function () {
            $('#HourTypeID').append($(this).clone())
        });
    }
    $('#HourTypeID').val($('#HourTypeID').data('selected'));
    $('#PhaseID').val($('#PhaseID').data('selected'));
}, 500);
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
                    document.location.href = $('a[title="Advisement of Rights"]').attr('href');// '/Case/Advisements';
                }
            }, no: {
                label: "Continue",
                className: "btn-secondary",
                callback: function () {
                    if (buttonId == 2) {
                        BackToCalendar();
                    }
                    else if (buttonId == 3) {
                        OpenPopup(noteUrl, title);
                    } else if (buttonId == 4) {
                        document.location.href = document.location.href;
                    } else if (buttonId == 5) {

                        OpenCustomPopup('/Task/HearingPreparationHours/' + $('#btnSaveAndNextHearing').attr('data-hearingid'), 500, 300, 'Hearing Preparation Hours History')

                    }
                    else if (buttonId == 6) {
                        OpenPopup('/Task/QuickCalCurrentERH/' + $('#btnSaveAndNextHearing').attr('data-hearingid'), 'Current Ed Rights Holder')
                    } else if (buttonId == 7) {
                        OpenCustomPopup('/Task/AdditionalAttendees/' + $('#btnSaveAndNextHearing').attr('data-hearingid'), 500, 300, 'Additional Attendees')
                    }
                    else {
                        OpenPopup('/Task/QuickCalNextHearing/' + $('#btnSaveAndNextHearing').attr('data-hearingid'), 'Next Hearing')
                    }
                }


            }
        }
    });
}


window.onbeforeunload = function (e) {

	if (hasFormChanged('AppearanceSheet') && validateTabClose) {
		return 'There is unsaved data.';
	}
	return undefined;
}



setInterval(function () {
    AutoSaveNote('HearingPrepNoteEntry', 'HearingPrepNoteEntry-' + $('#HearingID').val(), 0);
    AutoSaveNote('HearingNoteGetMain_NoteEntry', 'HearingNoteGetMain_NoteEntry-' + $('#HearingID').val(), 0);

    
}, 30000);
//30000
