
$(function () {
    setInitialFormValues('case-legalparties-form', true);
});

$('#saveAndContinue').on('click', function () {
    Save(1);
});
$('#saveAndAdd').on('click', function () {
    Save(2);
}); 
$('#saveAndMain').on('click', function () {
    Save(3);
});

$('#saveAndTransfer').on('click', function () {
    Save(4);
});

$(".transfre-case").on("click", function () {
    var ele = $(this);
    Save(7, ele);
});


$(".chkRelatedTo").on("change", function () {
    if (!$(this).is(':checked')) {
        $(this).parent().parent().find('.chkLiveWith').prop('checked', false);
    }
});

$(".chkLiveWith").on("change", function () {
    if ($(this).is(':checked') && !$(this).parent().parent().find('.chkRelatedTo').is(':checked')) {
        $(this).parent().parent().find('.chkRelatedTo').prop('checked', true);
    }
});
var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
    e.preventDefault();
    wizardUrl = $(this).attr('href');
    Save(5);

});
var legalPartyRoleList = [];

var legalPartiesList = [];
function Validation() {
    var isValid = true;
    legalPartyRoleList = [];
    legalPartiesList = [];



    var associationForNewRoleSelectedIds = '';

    $('#tblAssociationRelatedToList tbody tr').each(function () {
        if ($(this).find('#chkRelatedTo').is(':checked')) {
            if (associationForNewRoleSelectedIds != "")
                associationForNewRoleSelectedIds += ',';
            associationForNewRoleSelectedIds += $(this).find('#PersonID').val() + '|' + ($(this).find('#chkLiveWith').is(':checked') ? '1' : '0');


        }
    });
    if (associationForNewRoleSelectedIds != '' && $('#AssociationForNewRoleAssociationTypeID').val() == '') {
        $('#AssociationForNewRoleAssociationTypeID').focus();
        notifyDanger('Association  is required if people are checked.');
        return false;
    }
    var flag = true;
    $('.newroleinput').each(function () {

        if ($(this).val().length > 0 || associationForNewRoleSelectedIds != '') {
            if ($('#NewRoleID').val() == '') {
                isValid = false;
                $('#NewRoleID').focus();
                notifyDanger('Role is required.');
                flag = false;
                return false;
            }
            if ($('#FirstName').val() == '') {
                isValid = false;
                $('#FirstName').focus();
                notifyDanger('First Name is required.');
                flag = false;
                return false;
            } if ($('#LastName').val() == '') {
                isValid = false;
                $('#LastName').focus();
                notifyDanger('Last Name is required.');
                flag = false;
                return false;
            } if ($('#RoleStartDate').val() == '') {
                isValid = false;
                $('#RoleStartDate').focus();
                notifyDanger('Start Date is required.');
                flag = false;
                return false;
            }

            if ($('#EmailAddress').val().length > 0 && !IsValidData('email', $('#EmailAddress').val())) {

                isValid = false;
                $('#EmailAddress').focus();
                notifyDanger('Invalid Email Address.');
                flag = false;
                return false;
            }
        }
    });
    if (!flag) {
        return false;
	}
    var attorneyAssociate = '';
    $('#chkAttorney:checked').each(function () {
        if (attorneyAssociate != '')
            attorneyAssociate += ',';

        attorneyAssociate += $(this).parent().find('#PersonID').val();
    });

    $('#tblLegalPartyRoleList tbody tr').each(function () {
        if ($(this).find('#item_LegalTypeID').val() != '' && $(this).find('#LegalPartyRoleStartDate').val() == '') {
            isValid = false;
            $(this).find('#LegalPartyRoleStartDate').focus();
            notifyDanger('Start Date is required.');
            return false;

        }

        if (isValid) {

            if ($(this).find('#item_LegalTypeID').val() != '') {
                if ($(this).find('#item_LegalTypeID').attr('data-name') == 'Other Agency Attorney') {
                    legalPartyRoleList.push({
                        'RoleTypeCodeID': $(this).find('#item_LegalTypeID').val().split('|')[1],
                        'PersonID': $(this).find('#item_LegalTypeID').val().split('|')[0],
                        'StartDate': $(this).find('#LegalPartyRoleStartDate').val(),
                        'SelectedAttorneyAssociate': attorneyAssociate

                    });
                } else {
                    legalPartyRoleList.push({
                        'RoleTypeCodeID': $(this).find('#item_LegalTypeID').val().split('|')[1],
                        'PersonID': $(this).find('#item_LegalTypeID').val().split('|')[0],
                        'StartDate': $(this).find('#LegalPartyRoleStartDate').val()


                    });
                }

            }
        }

    });



    $('#tblLegalPartiesList tbody tr').each(function () {
        if ($(this).find('#item_RoleStartDate').val() == '') {
            isValid = false;
            $(this).find('#item_RoleStartDate').focus();
            notifyDanger('Start Date is required.');
            return false;

        }
        if ($(this).find('#item_RoleEndDate').val() != '' && moment($(this).find('#item_RoleStartDate').val()) > moment($(this).find('#item_RoleEndDate').val())) {
            isValid = false;
            $(this).find('#item_RoleEndDate').focus();
            notifyDanger('End Date must be after Start Date.');
            return false;

        }
        if (isValid) {
            if ($(this).find('#item_RoleStartDate').val() != $(this).find('#item_RoleStartDate').attr('data-val') || $(this).find('#item_RoleEndDate').val() != $(this).find('#item_RoleEndDate').attr('data-val')) {
                legalPartiesList.push({
                    'RoleID': $(this).find('#RoleID').val(),
                    'RoleStartDate': $(this).find('#item_RoleStartDate').val(),
                    'RoleEndDate': $(this).find('#item_RoleEndDate').val(),
                    'PersonID': $(this).find('#PersonID').val(),
                    'RoleTypeCodeID': $(this).find('#RoleTypeCodeID').val()



                });
            }
        }

    });


    if (attorneyAssociate != '' && $('select[data-name="Other Agency Attorney"]').val() == '') {
        isValid = false;

        notifyDanger('Other Agency Attorney is required if associations are checked.');
        return false;

    }

    var associationForNewRoleSelectedIds = '';

    $('#tblAssociationRelatedToList tbody tr').each(function () {
        if ($(this).find('#chkRelatedTo').is(':checked')) {
            if (associationForNewRoleSelectedIds != "")
                associationForNewRoleSelectedIds += ',';
            associationForNewRoleSelectedIds += $(this).find('#PersonID').val() + '|' + ($(this).find('#chkLiveWith').is(':checked') ? '1' : '0');

        }
    });
    if ($('#AssociationForNewRoleAssociationTypeID').val() > 0 && associationForNewRoleSelectedIds == '' && $('#NewRoleID').val() > 0) {
        notifyDanger('At least one person must be check if association is selected.');
        return false;
    }
    if ($('#AddressTypeID').val() == '' && ($('#Street').val() !== '' || $('#City').val() !== '' || $('#StateID').val() !== '' || $('#ZipCode').val() !== '' || $('#AddressPhone').val() !== '')) {
        notifyDanger('Address Type is required.');
        $('#AddressTypeID').focus();
        return false;
    }
     

    return isValid;
}
function Save(buttonId, sender) {
    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return false;
    }

    if (!hasFormChanged('case-legalparties-form')) {

        if (buttonId == 3) {
            document.location.href = '/Case/Main';
            isValid = false;
            return false;
        } else if (buttonId == 4) {
            document.location.href = '/Case/CopyCaseTransfer';
            isValid = false;
            return false;
        } else if (buttonId == 1) {
            document.location.href = '/CaseOpening/PetitionList?route=true';

            isValid = false;
            return false;
        }
        else if (buttonId == 5) {
            document.location.href = wizardUrl;
            return false;
        }
        else if (buttonId == 7) {
            if (sender != undefined) {
                var data = sender.data();
                document.location.href = '/Inquiry/TransferCase/' + data.id + '?pId=' + data.pId + '&pageId=1&roleTypeId=' + data.roleTypeId;
            }
            else {
                notifyDanger('Nothing was changed.');                
            }
            isValid = false;
            return false;
        }
        else {
            notifyDanger('Nothing was changed.');
            isValid = false;
            return false;

        }


    }


    var isvalid = Validation();

    if (isvalid) {

        var associationForNewRoleSelectedIds = '';

        $('#tblAssociationRelatedToList tbody tr').each(function () {
            if ($(this).find('#chkRelatedTo').is(':checked')) {
                if (associationForNewRoleSelectedIds != "")
                    associationForNewRoleSelectedIds += ',';
                associationForNewRoleSelectedIds += $(this).find('#PersonID').val() + '|' + ($(this).find('#chkLiveWith').is(':checked') ? '1' : '0');


            }
        });


        var model = {
            'NewRoleID': $('#NewRoleID').val(),
            'FirstName': $('#FirstName').val(),
            'LastName': $('#LastName').val(),
            'RoleStartDate': $('#RoleStartDate').val(),
            'AddressTypeID': $('#AddressTypeID').val(),
            'AddrStartDate': $('#AddrStartDate').val(),
            'Street': $('#Street').val(),
            'City': $('#City').val(),
            'StateID': $('#StateID').val(),
            'ZipCode': $('#ZipCode').val(),
            'AddressPhone': $('#AddressPhone').val(),
            'WorkPhone': $('#WorkPhone').val(),
            'MobilePhone': $('#MobilePhone').val(),
            'EmailAddress': $('#EmailAddress').val(),
            'AssociationForNewRoleAssociationTypeID': $('#AssociationForNewRoleAssociationTypeID').val(),
            'AssociationForNewRoleSelectedIds': associationForNewRoleSelectedIds,

            LegalPartiesSelectedRoleList: legalPartyRoleList,
            LegalPartiesList: legalPartiesList
        }


        if (legalPartiesList.length > 0 || legalPartyRoleList.length > 0 || $('#NewRoleID').val() > 0) {
            var params = model;
            $.ajax({
                type: "POST",
                url: '/CaseOpening/LegalPartiesSave',
                data: { model: params },
                success: function (result) {

                    if (result.Status == "Done") {

                        notifySuccess('Data Saved Successfully!.');
                        RequestSubmitted();

                        if (buttonId == 2) {
                            document.location.href = '/CaseOpening/LegalParties' + dataEntryQueryString;
                        } else if (buttonId == 3) {
                            document.location.href = '/Case/Main';
                        } else if (buttonId == 4) {
                            document.location.href = '#';
                        } else if (buttonId == 5) {
                            document.location.href = wizardUrl;
                            return false;
                        }
                        else if (buttonId == 7) {
                            if (sender != undefined) {
                                var data = sender.data();
                                document.location.href = '/Inquiry/TransferCase/' + data.id + '?pId=' + data.pId + '&roleTypeId=' + data.roleTypeId;
                            }
                            else {
                                document.location.href = '/CaseOpening/PetitionList?route=true';
                            }
                        }
                        else {
                            document.location.href = '/CaseOpening/PetitionList?route=true';

                        }
                    } else {
                        document.location.href = result.URL;

                    }
                },
                dataType: 'json'
            });
        } else {
            notifyDanger('Select/Create New Role is required.');

        }



    }

}
