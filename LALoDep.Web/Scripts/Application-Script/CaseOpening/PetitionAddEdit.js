

$('#saveAndContinue').on('click', function () {

    Save(1);

});
$('#saveAndAdd').on('click', function () {

    Save(2);

});
$('#saveAndMain').on('click', function () {

    Save(6);

});

$('#btnBackToList').on('click', function () {
    history.back();
    //   document.location.href = '/CaseOpening/PetitionList' + dataEntryQueryString;

});

$('.btnAddAllegation').on('click', function () {
    var $tr = $('#tblAllegation thead tr.template').clone();

    $('#tblAllegation tbody').append($tr);
    $('#tblAllegation tbody tr').removeClass('hidden');
});

//$('body').on('click', '.deleteAllegation', function () {
//    $this = $(this);
//    confirmBox("Are you sure you want to delete?", function (result) {
//        if (result) {
//            $this.parents('tr').find('#IsDelete').val('1');
//            $this.parents('tr').remove();
//        }
//    });
//});

$('#btnDelete').on('click', function () {

    confirmBox("Are you sure you want to delete?", function (result) {

        if (result) {
            $.ajax({
                type: "POST",
                url: '/CaseOpening/PetitionDelete/' + $('#PetitionID').val(),

                success: function (data) {
                    if (data.Status == 'Done') {

                        document.location.href = '/CaseOpening/PetitionList' + dataEntryQueryString;
                    }

                },
                dataType: 'json'
            });
        }

    });

});
var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
    e.preventDefault();
    wizardUrl = $(this).attr('href');
    Save(5);

});

$(function () {

    $('.allegationTypeDll').each(function () {

        $(this).val($(this).attr('data-val'))
    })
    setTimeout(function () {
        setInitialFormValues('petitionaddedit', true);
    }, 1000)
});
var oldModel = {
    'PetitionID': $('#PetitionID').val(),
    'PetitionTypeID': $('#PetitionTypeID').val(),
    'FileDate': $('#FileDate').val(),
    'CaseNumber': $('#CaseNumber').val(),
    'PhysicalFileName': $('#PhysicalFileName').val(),
    'CloseDate': $('#CloseDate').val(),
    'CaseAttributeID': $('#CaseAttributeID').val(),
    'AttorneyID': $('#AttorneyID').length > 0 ? $('#AttorneyID').val() : 0
};
var data = [];
var allegations = [];
function GetAllegations() {

    allegations = [];
    var isValid = true;
    $('#tblAllegation tbody tr').each(function () {
        if ($('#PetitionID').val() == '0') {
            if ($(this).find('#AllegationTypeID').val() == '') {


                if ($(this).find('#AllegationCount').val() != '' || $(this).find('#AllegationNote').val() != '' || $(this).find('#AllegationFindingID').val() != '') {

                    isValid = false;
                    Notify('Allegation is required.');
                    $(this).find('#AllegationTypeID').focus();

                    return false;

                }
            }
            if ($(this).find('#AllegationTypeID').val() != '') {
                allegations.push({
                    'AllegationIdentifier': $(this).find('#AllegationCount').val(),
                    'NoteEntry': $(this).find('#AllegationNote').val(),
                    'AllegationFindingCodeID': $(this).find('#AllegationFindingID').val(),
                    'AllegationTypeCodeID': $(this).find('#AllegationTypeID').val(),
                    'NoteID': $(this).find('#NoteID').val(),
                    'AllegationID': $(this).find('#AllegationID').val(),
                    'RecordStateID': $(this).find('#IsDelete').is(":checked") ? 10 : 1

                });
            }
        } else {
            if ($(this).find('#AllegationTypeID').val() == '') {


                if ($(this).find('#AllegationCount').val() != '' || $(this).find('#AllegationNote').val() != '' || $(this).find('#AllegationFindingID').val() != '' || $(this).find('#AllegationID').val() > 0) {

                    isValid = false;
                    Notify('Allegation is required.');
                    $(this).find('#AllegationTypeID').focus();

                    return false;

                }
            }
            if ($(this).find('#AllegationTypeID').IsValueChanged() || $(this).find('#AllegationCount').IsValueChanged() || $(this).find('#AllegationNote').IsValueChanged() || $(this).find('#AllegationFindingID').IsValueChanged() || ($(this).find('input#IsDelete:checked').length > 0 && $(this).find('#AllegationID').val() > 0)) {
                allegations.push({
                    'AllegationIdentifier': $(this).find('#AllegationCount').val(),
                    'NoteEntry': $(this).find('#AllegationNote').val(),
                    'AllegationFindingCodeID': $(this).find('#AllegationFindingID').val(),
                    'AllegationTypeCodeID': $(this).find('#AllegationTypeID').val(),
                    'NoteID': $(this).find('#NoteID').val(),
                    'AllegationID': $(this).find('#AllegationID').val(),
                    'RecordStateID': $(this).find('input#IsDelete:checked').length > 0 ? 10 : 1

                });
            }
        }


    });
    return allegations;
}

function AllegationsValidation() {


    var isValid = true;
    $('#tblAllegation tbody tr').each(function () {
        if ($('#PetitionID').val() == '0') {
            if ($(this).find('#AllegationTypeID').val() == '') {


                if ($(this).find('#AllegationCount').val() != '' || $(this).find('#AllegationNote').val() != '' || $(this).find('#AllegationFindingID').val() != '') {

                    isValid = false;
                    notifyDanger('Allegation is required.');
                    $(this).find('#AllegationTypeID').focus();

                    return false;

                }
            }

        } else {
            if ($(this).find('#AllegationTypeID').val() == '') {


                if ($(this).find('#AllegationCount').val() != '' || $(this).find('#AllegationNote').val() != '' || $(this).find('#AllegationFindingID').val() != '' || $(this).find('#AllegationID').val() > 0) {

                    isValid = false;
                    notifyDanger('Allegation is required.');
                    $(this).find('#AllegationTypeID').focus();

                    return false;

                }
            }

        }


    });
    return isValid;
}

function Validation() {
    var isValid = true;
    if ($('#PetitionTypeID').val() == '') {
        isValid = false;
        $('#PetitionTypeID').focus();
        notifyDanger('Petition Type  is required.');
        return false;
    }
    if ($('#FileDate').val() == '') {
        isValid = false;
        $('#FileDate').focus();
        notifyDanger('File Date is required.');
        return false;
    }
    if (moment($('#FileDate').val()) > moment()) {
        isValid = false;
        $('#FileDate').focus();
        notifyDanger('Petition File Date can not be in the future.');
        return false;
    }

    if ($('#CaseNumber').val() == '') {
        isValid = false;
        $('#CaseNumber').focus();
        notifyDanger('Case Number is required.');
        return false;
    }
    if ($('#AttorneyID').val() == '' && $caseClosedDate == 1) {

        isValid = false;
        $('#AttorneyID').focus();
        notifyDanger('Select Current Attorney or Reassign Case To is required.');
        return false;


    }




    data = [];


    var selectedChildCounter = 0;
    var isAgencyAttorneySelected = false;

    var isRespondentSelected = false;
    if ($('#AttorneyID').val() != '' && $caseClosedDate == 1) {
        isAgencyAttorneySelected = true;
    }

    $('.PetitionRoleStartDate').each(function () {

        if ($(this).val() !== '' && $(this).closest('tr').find('.PetitionRoleEndDate').val() == '') {
            var vdate = $(this).val();
            var PetitionRoleID = $(this).closest('tr').find('#PetitionRoleID').val();
            console.log(PetitionRoleID)
            $('.PetitionRoleEndDate').each(function () {
                var PetitionRoleID_old = $(this).closest('tr').find('#PetitionRoleID').val();

                //     console.log(PetitionRoleID_old)

                if ($(this).val() == '' && PetitionRoleID_old !== PetitionRoleID && moment($(this).closest('tr').find('.PetitionRoleStartDate').val()) < moment(vdate)) {
                    $(this).val(vdate)
                }
            });
        }
    });
    $('#tblRoles tbody tr').each(function () {


        $tr = $(this);
        if ($(this).find('.PetitionRoleStartDate').length > 0 && $(this).find('.PetitionRoleStartDate').val() !== '' && $(this).find('.PetitionRoleID').val() == '0') {
            $(this).find('#chkItem').prop('checked', true)
        }

        if ($(this).find('#chkItem').is(':checked')) {

            if ($(this).find('#ChildFlag').val() == '1') {
                selectedChildCounter++;
            } else if ($(this).find('#RespondentFlag').val() == '1') {
                isRespondentSelected = true;
            } else if ($(this).find('#RoleTypeCodeID').val() == $('#AttorneyAgencyRoleTypeCodeID').val()) {
                isAgencyAttorneySelected = true;
            }


            if ($(this).find('.PetitionRoleStartDate').val() == '') {
                notifyDanger('On Petition Date is required');
                isValid = false;
                $(this).find('.PetitionRoleStartDate').focus();

                return false;
            }


            if ($(this).find('.PetitionRoleEndDate').val() !== '' && moment($(this).find('.PetitionRoleStartDate').val()) > moment($(this).find('.PetitionRoleEndDate').val())) {
                notifyDanger('Off Petition Date can not be earlier than On Petition Date.');
                isValid = false;
                $(this).find('.PetitionRoleEndDate').focus();
                return false;
            }
            var sDate = $(this).find('.PetitionRoleStartDate').val();
            var countMatch = 0;
            $('.PetitionRoleStartDate').each(function () {
                if ($(this).val() == sDate) {
                    //  countMatch++
                }
            });
            if (countMatch > 1) {
                //      notifyDanger('On Petition Date range can not overlap another record.');
                //  $(this).find('.PetitionRoleStartDate').focus();
                console.log('countMatch')
                // isValid = false;
                // return false;
            }
            var countMatchRange = 0;
            $('.PetitionRoleStartDate').each(function () {

                if ($(this).val() !== '' && $(this).closest('tr').find('.PetitionRoleEndDate').val() == '') {
                    if ($('#MaxPetitionRoleEndDate').val() !== '') {
                        var vdate = $(this).val();
                        
                        if (moment(vdate) < moment($('#MaxPetitionRoleEndDate').val())) {
                            countMatchRange++
                            //   console.log(compareDate.isBetween(startDate, endDate))
                        }
                    }
                    //if ($('#MinPetitionRoleStartDate').val() !== '' && $('#MaxPetitionRoleEndDate').val()!=='') {
                    //    var vdate = $(this).val();
                    //    var compareDate = moment(vdate, "MM/DD/YYYY");
                    //    var startDate = moment($('#MinPetitionRoleStartDate').val(), "MM/DD/YYYY");
                    //    var endDate = moment($('#MaxPetitionRoleEndDate').val(), "MM/DD/YYYY");
                    //    if (compareDate.isBetween(startDate, endDate)) {
                    //        countMatchRange++
                    //        //   console.log(compareDate.isBetween(startDate, endDate))
                    //    }
                    //}
                
                    //$('.PetitionRoleEndDate').each(function () {
                    //    if ($(this).val() !== '') {

                    //        var compareDate = moment(vdate, "MM/DD/YYYY");
                    //        var startDate = moment($(this).closest('tr').find('.PetitionRoleStartDate').val(), "MM/DD/YYYY");
                    //        var endDate = moment($(this).val(), "MM/DD/YYYY");

                    //        if (compareDate.isBetween(startDate, endDate)) {
                    //         //   countMatchRange++
                    //         //   console.log(compareDate.isBetween(startDate, endDate))
                    //        }

                    //        if (moment(vdate) >= moment($(this).closest('tr').find('.PetitionRoleStartDate').val()) && moment(vdate) < moment($(this).val())) {
                    //            if (!(vdate == $(this).closest('tr').find('.PetitionRoleStartDate').val() && vdate == $(this).val()))
                    //            {
                    //                countMatchRange++
                    //        	} 
                    //        }

                    //    }

                    //});
                }
            });
            if (countMatchRange > 0) {
                notifyDanger('On Petition Date range can not overlap another record.');
                $(this).find('.PetitionRoleStartDate').focus();
                console.log('countMatchRange')
                isValid = false;
                return false;
            }
        }

        if (isValid == false)
            return false;

        if ($(this).find('#chkItem').is(':checked') && $(this).find('#PetitionRoleID').val() == '0') {
            data.push({
                'RoleID': $(this).find('#RoleID').val(),
                'PetitionRoleID': $(this).find('#PetitionRoleID').val(),
                'PetitionRoleStartDate': $(this).find('#item_PetitionRoleStartDate').val(),
                'PetitionRoleEndDate': $(this).find('#item_PetitionRoleEndDate').val(),
                'Selected': 1

            });
        } else if (!$(this).find('#chkItem').is(':checked') && $(this).find('#PetitionRoleID').val() > 0) {
            data.push({
                'RoleID': $(this).find('#RoleID').val(),
                'PetitionRoleID': $(this).find('#PetitionRoleID').val(),
                'PetitionRoleStartDate': $(this).find('#item_PetitionRoleStartDate').val(),
                'PetitionRoleEndDate': $(this).find('#item_PetitionRoleEndDate').val(),
                'Selected': 0
            });
        } else if ($(this).find('#chkItem').is(':checked') && $tr.find('.PetitionRoleStartDate').length > 0 && ($tr.find('.PetitionRoleStartDate').IsValueChanged() || $tr.find('.PetitionRoleEndDate').IsValueChanged())) {

            data.push({
                'RoleID': $(this).find('#RoleID').val(),
                'PetitionRoleID': $(this).find('#PetitionRoleID').val(),
                'PetitionRoleStartDate': $tr.find('#item_PetitionRoleStartDate').val(),
                'PetitionRoleEndDate': $tr.find('#item_PetitionRoleEndDate').val(),
                'Selected': 1
            });
        }
    });
    if (isValid == false)
        return false;
    if (selectedChildCounter != 1) {
        notifyDanger('One and only one child must be selected per petition');
        isValid = false;
        return false;

    }

    if (!isRespondentSelected) {
        notifyDanger('At least one respondent must be selected per petition');

        isValid = false;
        return false;

    }
    if (!isAgencyAttorneySelected) {
        notifyDanger('At least one agency attorney must be selected per petition');

        isValid = false;
        return false;

    }
    if (!AllegationsValidation()) {
        return false;
    }

    GetAllegations();





    return isValid;
}
function Save(buttonId) {



    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return;
    }

    if (!hasFormChanged('petitionaddedit')) {
        if ($('#PetitionID').val() > 0) {
            if (buttonId == 1) {
                document.location.href = '/CaseOpening/PetitionList' + dataEntryQueryString;
                return;
            } else if (buttonId == 5) {
                document.location.href = wizardUrl;
                return false;
            } else if (buttonId == 6) {

                document.location.href = '/Case/Main';

                return false;
            }
            notifyDanger('Nothing was changed.');

            return;
        } else if (buttonId == 5) {
            document.location.href = wizardUrl;
            return false;
        }

    }


    var isvalid = Validation();
    var uncheckedCheckboxes = false;
    if ($('#PetitionID').val() !== '0') {
        $('#tblRoles tr').each(function () {
            if ($(this).find('#chkItem').IsCheckboxChanged() && $(this).find('#chkItem').attr('data-old-value-on-pageload') == 'true') {
                uncheckedCheckboxes = true;
            }
        });
    }



    if (isvalid) {
        if (uncheckedCheckboxes) {
            confirmBox("You have unchecked a role on this petition.  This will erase the history of this role being on this petition.  Do you want to continue?", function (result) {

                if (result) {
                    if ($("#tblAllegation input:checkbox:checked").length > 0) {
                        confirmBox("Are you sure you want to delete selected allegation(s)?", function (result) {
                            if (result) {
                                saveData(buttonId);
                            }
                        });
                    }
                    else {
                        saveData(buttonId)
                    }
                }

            });
        } else {

            if ($("#tblAllegation input:checkbox:checked").length > 0) {
                confirmBox("Are you sure you want to delete selected allegation(s)?", function (result) {
                    if (result) {
                        saveData(buttonId);
                    }
                });
            }
            else {
                saveData(buttonId)
            }
        }
    }

}

function saveData(buttonId) {
    var model = {
        'PetitionID': $('#PetitionID').val(),
        'PetitionTypeID': $('#PetitionTypeID').val(),
        'FileDate': $('#FileDate').val(),
        'CaseNumber': $('#CaseNumber').val(),
        'PhysicalFileName': $('#PhysicalFileName').val(),
        'CloseDate': $('#CloseDate').val(),
        'CaseAttributeID': $('#CaseAttributeID').val(),
        'AttorneyID': $('#AttorneyID').length > 0 ? $('#AttorneyID').val() : '',
        RoleList: data,
        AllegationList: allegations
    }
    var params = model;
    $.ajax({
        type: "POST", url: '/CaseOpening/PetitionSave', data: { model: params, oldModel: oldModel },
        success: function (result) {

            if (result.Status == "Done") {
                RequestSubmitted();
                if (buttonId == 2) {
                    document.location.href = result.ReturnUrl + dataEntryQueryString;
                } else if (buttonId == 5) {
                    document.location.href = wizardUrl;
                    return false;
                } else if (buttonId == 6) {
                    document.location.href = '/Case/Main';

                    return false;
                } else {
                    document.location.href = '/CaseOpening/PetitionList' + dataEntryQueryString;

                }
            } else {
                document.location.href = result.URL;

            }
        },
        dataType: 'json'
    });
}