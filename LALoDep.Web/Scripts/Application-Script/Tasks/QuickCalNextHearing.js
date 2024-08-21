
$(function () {

    $('#HearingTypeID').focus();
    setInitialFormValues('case-hearing-form', true);
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
$('#saveAndContinue').on('click', function () {

    Save(1);

});
$('#saveAndAddMore').on('click', function () {

    Save(2);

});
$('#btnCancel').on('click', function () {
    if (backUrl !== '') {
        document.location.href = backUrl;
        return;
    }
    self.parent.document.location.href =
    self.parent.document.location.href;

});
    
$('#chkPetitionAll').on('change', function () {

    $('.chkPetition').not(':disabled').prop('checked', $(this).is(':checked'));

});
function Validation(buttonId) {
    var isValid = true;

    if (!hasFormChanged('case-hearing-form')) {
        
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

        

    }


    
    if ($('.chkPetition:checked').length == 0) {
        isValid = false;
        $('.chkPetition:first').focus();
        notifyDanger('At least one petition is required per hearing.');
        return false;
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

    $('.chkPetition').each(function () {
        
        var orderedToAppear = ($(this).closest('tr').find('.chkOrderedToAppear').length > 0 && $(this).closest('tr').find('.chkOrderedToAppear').is(':checked')) ? 1 : 0;
        if ($(this).is(':checked')) {
            if (petitionsSelectedIds != "")
                petitionsSelectedIds += ',';
            petitionsSelectedIds += $(this).attr('value') + '|' + orderedToAppear;


        }
    });
    

    var model = {
        'HearingID':0,
        'HearingTypeID': $('#HearingTypeID').val(),
        'HearingDate': $('#HearingDate').val(),
        'HearingTime': $('#TimeHours').val() + ':' + $('#Minutes').val() + ' ' + $('#TimeAmPm').val(),
        'HearingOfficerID': $('#HearingOfficerID').val(),
        'DepartmentID': $('#DepartmentID').val(),
        
        'Note': $('#Note').val(),
        'PetitionsSelectedIds': petitionsSelectedIds,
        'UpdateDepartment': updateDepartment
     

    }
    var params = model;
    $.ajax({
        type: "POST",
        url: '/Task/QuickCalNextHearingSave',
        data: { model: params },
        success: function (result) {

            if (result.Status == "Done") {

                notifySuccess('Data Saved Successfully!.');
                RequestSubmitted();
                setTimeout(function () {
                    if (buttonId == 1) {
                        self.parent.document.location.href =
                   self.parent.document.location.href;
                    } else {
                         document.location.href =
                  document.location.href;
                    }
                   
                }, 2000)
              
 
            } else {
                document.location.href = result.URL;

            }
        },
        dataType: 'json'
    });

}

