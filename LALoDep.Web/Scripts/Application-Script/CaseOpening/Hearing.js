var IsResultContinuance = false;
$(function () {


    setInitialFormValues('case-hearing-form', true);
});

$('#saveAndContinue').on('click', function () {

    Save(1);

});
$('#saveAndAdd').on('click', function () {

    Save(2);

});

$('#SaveAddAR').on('click', function () {

    Save(3);

});

$('#SaveAndRecordTime').on('click', function () {

    Save(4);

});

$('#saveAndMain').on('click', function () {

    Save(5);

}); $('#chkPetitionAll').on('change', function () {

    $('.chkPetition').not(':disabled').prop('checked', $(this).is(':checked'));

});

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
$('#HearingTypeID').on('blur', function () {
    var hearingType = $(this).val();
    if (HearingTypeContestedIds.indexOf(hearingType) >= 0) {
        $("#TimeHours").val(contestedDefaultHours);
        $("#Minutes").val(contestedDefaultMinutes);
        $("#TimeAmPm").val(contestedDefaultAm);
    }
    else {
        $("#TimeHours").val(defaultHours);
        $("#Minutes").val(defaultMinutes);
        $("#TimeAmPm").val(defaultAm);
    }
});
var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
    e.preventDefault();
    wizardUrl = $(this).attr('href');
    Save(6);

});
$('body').on('click', '.deleteHearing', function () {
    $this = $(this);

    confirmBox("Are you sure you want to delete?", function (result) {

        if (result) {
            $.ajax({
                type: "POST", url: '/CaseOpening/HearingDelete/' + $this.attr('data-id'),
                success: function (result) {

                    if (result.Status == "Done") {
                        notifyDanger('Hearing Deleted Successfully!.', 'bottom-right', '3000', 'success', 'fa-smile-o', true);
                        if (getParameterByName('dataentry').length > 0)
                            if (Contains(window.location.href, "dataentry=")) {
                                window.location.href = "/CaseOpening/Hearing?dataentry=" + getParameterByName("dataentry");
                            } else {
                                document.location.href = '/CaseOpening/Hearing';
                            }


                    } else {
                        document.location.href = result.URL;

                    }
                },
                dataType: 'json'
            });
        }

    });

});


function Validation(buttonId) {
    var isValid = true;

    if (!hasFormChanged('case-hearing-form')) {
        //if (buttonId == 2) {
        //    document.location.href = '/CaseOpening/Hearing';
        //    return false;
        //} else
        if (buttonId == 3) {
            document.location.href = '/CaseOpening/ActionRequestAdd?dataentry=true';
            return false;
        } else if (buttonId == 4) {
            document.location.href = '/Case/RecordTime';
            return false;
        } else if (buttonId == 5) {
            document.location.href = '/Case/Main?fromHearing=true';
            return false;
        } else if (buttonId == 6) {
            document.location.href = wizardUrl;
            return false;
        } else if (buttonId == 1) {
            document.location.href = '/CaseOpening/ActionRequest';
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
        }
        if ($('#Hours').val() == '' && GetHoursRequiredFlag($('#DataValidation_RequireHearingHoursFlag').val()) == '1') {
            isValid = false;
            $('#Hours').focus();
            notifyDanger('Hours is required.');
            return false;
        }
        
        if ((GetHoursRequiredFlag($('#DataValidation_RequireHearingHoursFlag').val()) == '1' && $('#Hours').val() != '') && ($('#HourTypeID').val() == '' || $('#HourTypeID').val() == null)) {
            isValid = false;
            $('#HourTypeID').focus();
            notifyDanger('Hour Type is required.');
            return false;
        }
        if ($('#PhaseID').val() == '' || $('#PhaseID').val() == null) {
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
    var attendence = $('#attendanceList > tbody > tr')
    for (var index = 0; index < attendence.length; index++) {
        var tr = attendence.eq(index);
        if (tr.data('requiredflag') == 1) {
            var chkBoxYes = tr.find('.rbtnYes');
            var chkBoxNo = tr.find('.rbtnNo');
            if (!chkBoxYes.is(':checked') && !chkBoxNo.is(':checked')) {
                isValid = false;
                $(chkBoxYes).focus();

                notifyDanger('Attended Present is required');
                return false;
            }
        }


    }
    if (!isValid)
        return isValid;

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

        //isValid = false;
        //$('#ContinuanceRequestedByID').focus();
        //notifyDanger('Requested by is a required field if a continuance result is selected.');
        //return false;
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
    if (($('#Hours').val() == '') && GetHoursRequiredFlag($('#WorkHoursRequiredFlag').val()) == '1' && resultSelected) {
        isValid = false;
        $('#Hours').focus();
        notifyDanger('Hours is required.');
        return false;
    }
    if ($('#Hours').val() <= 0 && GetHoursRequiredFlag($('#WorkHoursRequiredFlag').val()) == '1' && resultSelected) {

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
        var count = 0; var countChecked = 0;
        $('.PetitionResultID').each(function () {
            if ($(this).val() > 0) {
                if (!Contains($('#HearingResultFutureHearingsList').val(), $(this).val())) {
                    count++;
                    if ($(this).parents('tr').find('.chkPetition:checked').length > 0) {
                        countChecked++;
                    }
                }

            }
        })
        if ($("#GlobalResultID").val() == '' && count > 0 && count != countChecked) {

            isValid = false;
            $('.chkPetition:first').focus();
            notifyDanger('All children must be resulted if one child is resulted.');
            return false;
        }

    }

    isValid = HearingContinuanceReasonAndRequestValidation(IsResultContinuance);

    return isValid;


}
function GetHoursRequiredFlag(flagElmValue) {
    var newFlag = flagElmValue;
    if (flagElmValue == '1') {
        if (($('#PhaseID').hasValue() || $('#HourTypeID').hasValue())) {
            newFlag = 1;
        } else if ($('#HearingTypeHoursNotRequired').val().length > 0 && Contains($('#HearingTypeHoursNotRequired').val(), $('#HearingTypeID').val())) {
            newFlag = 0;
        }
        else if (hoursNotRequiredBeforeHearingDate !== '' && moment($('#HearingDate').val())< moment(hoursNotRequiredBeforeHearingDate)) {
            newFlag = 0;
        }
      
    }
    return newFlag;
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

    $('.chkPetition').each(function () {
        if ($(this).is(':checked')) {
            var orderedToAppear = ($(this).closest('tr').find('.chkOrderedToAppear').length > 0 && $(this).closest('tr').find('.chkOrderedToAppear').is(':checked')) ? 1 : 0;

            if (petitionsSelectedIds != "")
                petitionsSelectedIds += ',';
            petitionsSelectedIds += $(this).attr('value') + '|' + $(this).parent().parent().find('#ResultID').val() + '|' + orderedToAppear;


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
        'ContinuanceRequestedByID': $('#ContinuanceRequestedByID').val()

    }
    var attendence = $('#attendanceList > tbody > tr')
    for (var index = 0; index < attendence.length; index++) {

        var tr = attendence.eq(index);
        if (tr.find('.rbtnYes').IsRadioChanged() || tr.find('.rbtnNo').IsRadioChanged()) {

            var chkBox = tr.find('.rbtnYes');
            var attendanceList = {
                'RoleID': tr.data('roleid'),
                'AttendanceID': tr.data('attendanceid'),
                'IsSelected': chkBox.is(":checked")
            };
            model.HearingAttendance.push(attendanceList);
        }
    }
    

    var params = HearingContinuanceReasonAndRequestModelData(model);
    $.ajax({
        type: "POST",
        url: '/CaseOpening/HearingSave',
        data: { model: params },
        success: function (result) {

            if (result.Status == "Done") {

                notifySuccess('Data Saved Successfully!.');
                RequestSubmitted();


                if (buttonId == 2) {
                    document.location.href = '/CaseOpening/Hearing' + dataEntryQueryString;
                } else if (buttonId == 3) {
                    document.location.href = '/CaseOpening/ActionRequestAdd?dataentry=true';
                } else if (buttonId == 4) {
                    document.location.href = '/Case/RecordTime';
                } else if (buttonId == 5) {
                    document.location.href = '/Case/Main?fromHearing=true';
                } else if (buttonId == 6) {
                    document.location.href = wizardUrl;
                    return false;
                } else {
                    document.location.href = '/CaseOpening/ActionRequest';

                }
            } else {
                document.location.href = result.URL;

            }
        },
        dataType: 'json'
    });

}

