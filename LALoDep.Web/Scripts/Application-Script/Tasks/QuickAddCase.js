
$(function () {

    $('#OtherHearingTypeID').val('')
    $('#OtherHearingDepartmentID').val('')
    $('#OtherHearingOfficerID').val('')
    setInitialFormValues('QuickAddCase-form', true);

    $('select').each(function () {
        $(this).val($(this).data('selected'));
    });
});

$('#btnSave').on('click', function () {
    GetData();
    Save(1);

});
$('#btnSaveAndAdd').on('click', function () {

    Save(2);

});

var associations = [];

function Validation(buttonId) {
    var isValid = true;
    associations = [];

    if (!hasFormChanged('QuickAddCase-form')) {

        notifyDanger('Nothing was changed.');
        isValid = false;
        return false;
    }



    if ($('input[id*="IsClient"]:checked').length == 0) {
        isValid = false;
        $('input[id*="IsClient"]:eq(0)').focus();
        notifyDanger('One client is required.');
        return false;
    }



    var childAdded = false;
    var parentAdded = false;

    if ($('#CasePersonList_1__CaseNumber').val() == '') {
        notifyDanger('Child 1 Case # is required.');
        $('#CasePersonList_1__CaseNumber').focus();
        return false;
    }
    for (var i = 1; i <= 2; i++) {
        if ($('#CasePersonList_' + i + '__IsClient').is(':checked') != '' || $('#CasePersonList_' + i + '__LastName').val() != '' || $('#CasePersonList_' + i + '__FirstName').val() != '' || $('#CasePersonList_' + i + '__HasAddress').is(':checked')) {
            childAdded = true;
            if ($('#CasePersonList_' + i + '__LastName').val() == '') {
                notifyDanger('Child ' + i + ' Last Name is required.');
                $('#CasePersonList_' + i + '__LastName').focus();
                return false;
            }
            if ($('#CasePersonList_' + i + '__FirstName').val() == '') {
                notifyDanger('Child ' + i + ' First Name is required.');
                $('#CasePersonList_' + i + '__FirstName').focus();
                return false;
            } if ($('#CasePersonList_' + i + '__RoleID').val() == '3' && $('#CasePersonList_' + i + '__DOB').val() == '' && $("#DOBRequiredForChildren").val() == "1") {
                notifyDanger('Child ' + i + ' DOB is required.');
                $('#CasePersonList_' + i + '__DOB').focus();
                return false;
            }
            if ($('#CasePersonList_' + i + '__SexTypeCodeID').val() == '') {
                notifyDanger('Child ' + i + ' Gender is required.');
                $('#CasePersonList_' + i + '__SexTypeCodeID').focus();
                return false;
            }
            if ($('#CasePersonList_' + i + '__RoleID').val() == '') {
                notifyDanger('Child ' + i + ' Role is required.');
                $('#CasePersonList_' + i + '__RoleID').focus();
                return false;
            }
         
        }
    }
    for (var i = 0; i <= 5; i++) {

        if ($('#CasePersonList_' + i + '__LastName').val() != '' || $('#CasePersonList_' + i + '__FirstName').val() != '' || $('#CasePersonList_' + i + '__HasAddress').is(':checked')) {
            parentAdded = true;
            var number = 1;
            if (i != 0) {
                number = i - 2;

            }
            if ($('#CasePersonList_' + i + '__LastName').val() == '') {
                notifyDanger('Parent ' + number + ' Last Name is required.');
                $('#CasePersonList_' + i + '__LastName').focus();
                return false;
            }
            if ($('#CasePersonList_' + i + '__FirstName').val() == '') {
                notifyDanger('Parent ' + number + ' First Name is required.');
                $('#CasePersonList_' + i + '__FirstName').focus();
                return false;
            }
            if ($('#CasePersonList_' + i + '__SexTypeCodeID').val() == '') {
                notifyDanger('Parent ' + number + ' Gender is required.');
                $('#CasePersonList_' + i + '__SexTypeCodeID').focus();
                return false;
            } if ($('#CasePersonList_' + i + '__ChildrenAssociationTypeCodeID').val() == '') {
                notifyDanger('Parent ' + number + ' Association to Children is required.');
                $('#CasePersonList_' + i + '__ChildrenAssociationTypeCodeID').focus();
                return false;
            }
            if ($('#CasePersonList_' + i + '__RoleID').val() == '') {
                notifyDanger('Parent ' + number + ' Role is required.');
                $('#CasePersonList_' + i + '__RoleID').focus();
                return false;
            }
        }
        if (i == 0) {
            i = 3;
        }
    }
    if (!childAdded) {
        isValid = false;
        $('#CasePersonList_1__LastName').focus();
        notifyDanger('At least one Child is required.');
        return false;
    } if (!parentAdded) {
        isValid = false;
        $('#CasePersonList_0__LastName').focus();
        notifyDanger('At least one Parent is required.');
        return false;
    }
    if ($('#AppointmentDate').val() == '') {
        notifyDanger('Appointment Date is required.');
        $('#AppointmentDate').focus();
        return false;
    }

    if ($('#CaseOfficerPersonID').val() == '') {
        notifyDanger('Case Judge is required.');
        $('#CaseOfficerPersonID').focus();
        return false;
    }
    if ($('#CaseDepartmentID').val() == '') {
        notifyDanger('Case Department is required.');
        $('#CaseDepartmentID').focus();
        return false;
    }

    if ($('#AttorneyPersonID').val() == '') {
        notifyDanger('Case Attorney is required.');
        $('#AttorneyPersonID').focus();
        return false;
    }

    if ($('#PetitionFileDate').val() == '') {
        notifyDanger('Petition File Date is required.');
        $('#PetitionFileDate').focus();
        return false;
    }
    if ($("#PetitionTypeCodeID").val() == '') {
        isValid = false;
        $("#PetitionTypeCodeID").focus();
        notifyDanger('Petition Type is required.');
        return false;
    }
    if ($('#HearingTypeID').val() != '' || $('#HearingDate').val() != '' || $('#HearingDepartmentID').val() != '' || $('#HearingOfficerID').val() != '') {
        if ($('#HearingTypeID').val() == '') {
            notifyDanger('Hearing Type is required.');
            $('#HearingTypeID').focus();
            return false;
        }
        if ($('#HearingDate').val() == '') {
            notifyDanger('Hearing Date is required.');
            $('#HearingDate').focus();
            return false;
        }
        if ($('#HearingTime').val() == '') {
            notifyDanger('Hearing Time is required.');
            $('#HearingTime').focus();
            return false;
        }
        if ($('#HearingDepartmentID').val() == '' && $('#CaseDepartmentID').val() == '') {
            notifyDanger('Hearing Department is required.');
            $('#HearingDepartmentID').focus();
            return false;
        }
        if ($('#HearingOfficerID').val() == '' && $('#CaseOfficerPersonID').val() == '') {
            notifyDanger('Hearing Officer is required.');
            $('#HearingOfficerID').focus();
            return false;
        }



    }
    if ($('#OtherHearingTypeID').val() != '' || $('#OtherHearingDate').val() != '' || $('#OtherHearingDepartmentID').val() != '' || $('#OtherHearingOfficerID').val() != '') {
        if ($('#OtherHearingTypeID').val() == '') {
            notifyDanger('Hearing Type is required.');
            $('#OtherHearingTypeID').focus();
            return false;
        }
        if ($('#OtherHearingDate').val() == '') {
            notifyDanger('Hearing Date is required.');
            $('#OtherHearingDate').focus();
            return false;
        }
        if ($('#OtherHearingTime').val() == '') {
            notifyDanger('Hearing Time is required.');
            $('#OtherHearingTime').focus();
            return false;
        }
        if ($('#OtherHearingDepartmentID').val() == '' && $('#CaseDepartmentID').val() == '') {
            notifyDanger('Hearing Department is required.');
            $('#OtherHearingDepartmentID').focus();
            return false;
        }
        if ($('#OtherHearingOfficerID').val() == '' && $('#CaseOfficerPersonID').val() == '') {
            notifyDanger('Hearing Officer is required.');
            $('#OtherHearingOfficerID').focus();
            return false;
        }



    }
    if ($('#NoteTypeID').val() != '' || $('#Note').val() != '' || $('#NoteSubject').val() != '') {
        if ($('#NoteTypeID').val() == '') {
            notifyDanger('Note Type is required.');
            $('#NoteTypeID').focus();
            return false;
        }
        if ($('#Note').val() == '') {
            notifyDanger('Note is required.');
            $('#HearingDate').focus();
            return false;
        }

    }
    if ($('#PlacementAddressID').val() != '' || $('#Street').val() != '' || $('#City').val() != '' || $('#ZipCode').val() != '' || $('#AddressPhone').val() != '') {


        if ($('.chkAddress:checked').length == 0) {
            notifyDanger('At least one Address must be checked when Placement Address is selected or Address information is entered.');

            $('input[id*="IsClient"]:checked').parent().parent().parent().parent().find('.chkAddress').focus();
            return false;
        }


    }
    if ($('#PlacementAddressID').val() == '' && $('.chkAddress:checked').length > 0) {
        if ($('#Street').val() == '') {
            notifyDanger('Address Street is required when an Address is checked and Placement address is not selected.');
            $('#Street').focus();
            return false;
        }
        if ($('#City').val() == '') {
            notifyDanger('City is required when an Address is checked and Placement address is not selected.');
            $('#City').focus();
            return false;
        }
        if ($('#StateID').val() == '') {
            notifyDanger('State is required when an Address is checked and Placement address is not selected.');
            $('#StateID').focus();
            return false;
        }
        if ($('#ZipCode').val() == '') {
            notifyDanger('Zip Code is required when an Address is checked and Placement address is not selected.');
            $('#ZipCode').focus();
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

        var params = GetData();

        $.ajax({
            type: "POST", url: '/Task/QuickAddCase', data: { model: params },
            success: function (result) {

                if (result.Status == "Done") {
                    RequestSubmitted();
                    notifySuccess('Data Saved Successfully!.');

                    if (buttonId == 1) {
                        document.location.href = result.MainUrl;
                    } else if (buttonId == 2) {
                        document.location.href = '/Task/QuickAddCaseSearch';

                    }


                } else {
                    document.location.href = result.URL;

                }
            },
            dataType: 'json'
        });
    }

}
function GetData() {

    var childList = [];
    var parentList = [];
    for (var i = 1; i <= 3; i++) {
        if ($('#CasePersonList_' + i + '__LastName').val() != '' || $('#CasePersonList_' + i + '__FirstName').val() != '' || $('#CasePersonList_' + i + '__HasAddress').is(':checked')) {
            childList.push({
                CheckBoxLabel: i.toString(),
                IsParent: false,
                IsClient: $('#CasePersonList_' + i + '__IsClient').is(':checked'),
                CaseNumber: $('#CasePersonList_' + i + '__CaseNumber').val(),
                RoleID: $('#CasePersonList_' + i + '__RoleID').val(),
                LastName: $('#CasePersonList_' + i + '__LastName').val(),
                FirstName: $('#CasePersonList_' + i + '__FirstName').val(),
                DOB: $('#CasePersonList_' + i + '__DOB').val(),

                SexTypeCodeID: $('#CasePersonList_' + i + '__SexTypeCodeID').val(),
                //    ChildrenAssociationTypeCodeID: $('#CasePersonList_' + i + '__ChildrenAssociationTypeCodeID').val(),
                IsSS: $('#CasePersonList_' + i + '__IsSS').is(':checked'),
                HasAddress: $('#CasePersonList_' + i + '__HasAddress').is(':checked')
            });
        }
    }
    for (var i = 0; i <= 5; i++) {

        if ($('#CasePersonList_' + i + '__LastName').val() != '' || $('#CasePersonList_' + i + '__FirstName').val() != '' || $('#CasePersonList_' + i + '__HasAddress').is(':checked')) {
            var number = 1;
            if (i != 0) {
                number = i - 2;

            }
            parentList.push({
                CheckBoxLabel: number.toString(),
                IsParent: true,
                IsClient: $('#CasePersonList_' + i + '__IsClient').is(':checked'),
                // CaseNumber: $('#CasePersonList_' + i + '__CaseNumber').val(),
                RoleID: $('#CasePersonList_' + i + '__RoleID').val(),
                LastName: $('#CasePersonList_' + i + '__LastName').val(),
                FirstName: $('#CasePersonList_' + i + '__FirstName').val(),
                DOB: $('#CasePersonList_' + i + '__DOB').val(),

                SexTypeCodeID: $('#CasePersonList_' + i + '__SexTypeCodeID').val(),
                ChildrenAssociationTypeCodeID: $('#CasePersonList_' + i + '__ChildrenAssociationTypeCodeID').val(),
                IsSS: $('#CasePersonList_' + i + '__IsSS').is(':checked'),
                HasAddress: $('#CasePersonList_' + i + '__HasAddress').is(':checked')
            });
        }
        if (i == 0) {
            i = 3;
        }
    }
    $('#HearingTime').val($('#Hours').val() + ':' + $('#Minutes').val() + " " + $('#TimeAmPm').val());
    $('#OtherHearingTime').val($('#Hours2').val() + ':' + $('#Minutes2').val() + " " + $('#TimeAmPm2').val());
    var parames = {
        AppointmentDate: $('#AppointmentDate').val(),
        IsPanel: $('#IsPanel').is(':checked'),
        CaseOfficerPersonID: $('#CaseOfficerPersonID').val(),
        CaseDepartmentID: $('#CaseDepartmentID').val(),
        CaseRefrelSourceID: $('#CaseRefrelSourceID').val(),
        AttorneyPersonID: $('#AttorneyPersonID').val().split('|')[0],
        AttorneyRoleTypeID: $('#AttorneyPersonID').val().split('|')[1],
        DesignatedDayCodeID: $('#DesignatedDayCodeID').val(),
        PetitionFileDate: $('#PetitionFileDate').val(),
        PetitionTypeCodeID: $('#PetitionTypeCodeID').val(),
        Allegation1ID: $('#Allegation1ID').val(),
        Allegation2ID: $('#Allegation2ID').val(),
        Allegation3ID: $('#Allegation3ID').val(),
        Allegation4ID: $('#Allegation4ID').val(),
        Allegation5ID: $('#Allegation5ID').val(),
        Allegation6ID: $('#Allegation6ID').val(),
        PhysicalFileName: $('#PhysicalFileName').val(),
        HearingTypeID: $('#HearingTypeID').val(),
        HearingDate: $('#HearingDate').val(),
        HearingTime: $('#HearingTime').val(),
        HearingDepartmentID: $('#HearingDepartmentID').val() == '' ? $('#CaseDepartmentID').val() : $('#HearingDepartmentID').val(),
        HearingOfficerID: $('#HearingOfficerID').val() == '' ? $('#CaseOfficerPersonID').val() : $('#HearingOfficerID').val(),
        AppearingAttorneyID: $('#AppearingAttorneyID').val().split('|')[0],
        OtherHearingTypeID: $('#OtherHearingTypeID').val(),
        OtherHearingDate: $('#OtherHearingDate').val(),
        OtherHearingTime: $('#OtherHearingTime').val(),
        OtherHearingDepartmentID: $('#OtherHearingDepartmentID').val() == '' ? $('#CaseDepartmentID').val() : $('#OtherHearingDepartmentID').val(),
        OtherHearingOfficerID: $('#OtherHearingOfficerID').val() == '' ? $('#CaseOfficerPersonID').val() : $('#OtherHearingOfficerID').val(),
        OtherAppearingAttorneyID: $('#OtherAppearingAttorneyID').val().split('|')[0],
        NoteTypeID: $('#NoteTypeID').val(),
        NoteSubject: $('#NoteSubject').val(),
        Note: $('#Note').val(),
        PlacementAddressID: $('#PlacementAddressID').val(),
        Street: $('#Street').val(),
        City: $('#City').val(),
        StateID: $('#StateID').val(),
        ZipCode: $('#ZipCode').val(),
        AddressPhone: $('#AddressPhone').val(),
        AgencyID: $('#AgencyID').val(),
        ChildList: childList,
        ParentList: parentList
    }
    return parames;
}


$('input[id*="IsClient"]').change(function () {
    if ($(this).is(':checked')) {
        if ($(this).parent().find('span:contains("Parent")').length > 0) {
            $('input[id*="IsClient"]').not($(this)).prop('checked', false);

        } else {
            $('input[id*="IsClient"]').not($(this)).each(function () {

                if ($(this).parent().find('span:contains("Parent")').length > 0) {
                    $(this).prop('checked', false);
                }
            });

        }
    }
});
