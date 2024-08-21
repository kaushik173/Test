
$('#saveAndContinue').on('click', function () {

    Save(1);

});
$('#saveAndAdd').on('click', function () {

    Save(2);

}); $('#saveAndPetition').on('click', function () {

    Save(3);

});
$('#saveAndAssociation').on('click', function () {

    Save(4);

});
var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
    e.preventDefault();
    wizardUrl = $(this).attr('href');
    Save(5);

});


$(".item_RoleID").on("change", function () {
    var $tr = $(this).parents('tr');
    var roleId = $(this).val();
    var sexDropdown = $('.item_SexID', $tr);
    if (roleId == '774') {        
        sexDropdown.val('763');
    } else if (roleId == '782') {
        sexDropdown.val('762');
    } 
});

$('.item_IsCn').on('click', function () {

    if ($(this).is(':checked')) {
        $('.item_IsCn').not($(this)).prop('checked', false);
    }

});

var data = [];
function Validation(buttonId) {
    var isValid = true;
    data = [];
    var isClientNonChildCount = 0;
    $('#tblRespondent tbody tr').each(function () {
        var flag = !$(this).find('#item_IsClient').is(":checked")
            && $(this).find('#item_LastName').val() == ''
            && $(this).find('#item_FirstName').val() == ''
            && $(this).find('#item_DOB').val() == ''
            && $(this).find('#item_RaceID').val() == ''
            && $(this).find('#item_SexID').val() == ''
            && $(this).find('#item_RoleID').val() == ''
            && $(this).find('#item_DesignatedDayID').val() == ''
        && !$(this).find('#item_IsSs').is(":checked");

        if (!flag) {

            if ($(this).find('#item_LastName').val() == '') {
                isValid = false;
                $(this).find('#item_LastName').focus();
                notifyDanger('Last Name is required.');
                return false;
            }

            if ($(this).find('#item_FirstName').val() == '') {

                $(this).find('#item_FirstName').focus();
                notifyDanger('First Name is required.');
                isValid = false;
                return false;
            }

            if ($(this).find('#item_SexID').val() == '') {

                $(this).find('#item_SexID').focus();
                notifyDanger('Gender is required.');

                isValid = false;
                return false;
            }

            if ($(this).find('#item_RoleID').val() == '') {

                $(this).find('#item_RoleID').focus();
                notifyDanger('Role is required.');
                isValid = false;
                return false;
            }
            if ($(this).find('#item_RoleID').val() == '3' && $(this).find('#item_DOB').val() == '' && $("#DOBRequiredForChildren").val() == "1") {
                notifyDanger('DOB is required.');
                $(this).find('#item_DOB').focus();
                isValid = false;
                return false;
            }
            
            if ($(this).find('#item_IsClient').is(":checked") && $('#IsRoleClientAdded').val() == 'True' && $(this).find('#item_RoleID').val() != '3') {

                notifyDanger('One and only one respondent client is allowed on a case.');
                isValid = false;
                return false;
            } if ($(this).find('#item_IsClient').is(":checked") && $(this).find('#item_RoleID').val() != '3') {

                isClientNonChildCount = 1;

            }



        }
        if ($(this).find('#item_LastName').val() != '') {
            data.push({

                'LastName': $(this).find('#item_LastName').val(),
                'IsClient': $(this).find('#item_IsClient').is(':checked') ? true : false,
                'FirstName': $(this).find('#item_FirstName').val(),
                'DOB': $(this).find('#item_DOB').val(),
                'IsCn': $(this).find('#item_IsCn').is(':checked') ? true : false,
                'RaceID': $(this).find('#item_RaceID').val(),
                'SexID': $(this).find('#item_SexID').val(),
                'RoleID': $(this).find('#item_RoleID').val(),
                'DesignatedDayID': $(this).find('#item_DesignatedDayID').val(),
                'StartOrApptDate': $(this).find('#item_StartOrApptDate').val(),
                'IsSs': $(this).find('#item_IsSs').is(':checked') ? true : false

            });
        }




    });


    if (!isValid)
        return false;

    if (data.length == 0) {

        if (buttonId == 3) {
            document.location.href = '/CaseOpening/PetitionList?dataentry=true';
            return false;
        } else if (buttonId == 4) {
            document.location.href = '/CaseOpening/Associations?dataentry=true';
            return false;
        } else if (buttonId == 1) {
            document.location.href = '/CaseOpening/LegalParties';
            return false;
        }
        else if (buttonId == 5) {
            document.location.href = wizardUrl;
            return false;
        }

        notifyDanger('Nothing is changed');

        isValid = false;
        return false;

    }
    var childId = '3';
    var isChildSelected = false;
    $('.item_RoleID').each(function () {

        if ($(this).val() == childId) {
            isChildSelected = true;
        }
    });
    if (!isChildSelected && $('#IsChildAdded').val() == 'False') {

        notifyDanger('At least one child is required on a case.');
        isValid = false;
        return false;
    }
    if ($('.item_IsClient:checked').length == 0 && $('#IsRoleClientAdded').val() == 'False') {

        notifyDanger('At least one client is required');
        isValid = false;
        return false;
    } if ($('.item_IsClient:checked').length > 1 && isClientNonChildCount > 0) {

        notifyDanger('One and only one respondent client is allowed on a case.');
        isValid = false;
        return false;
    }
    if ($('.item_IsCn:checked').length == 0 && $('#IsCnAdded').val() == 'False') {

        notifyDanger('One and only one Case Name(CN) is required.');
        isValid = false;
        return false;
    }

    if (data.length == 1 && $('#tblRespondentData tbody tr').length == 0) {
        notifyDanger('At least one respondent is required on a case.');
        isValid = false;
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
        var params = data;


        $.ajax({
            type: "POST", url: '/CaseOpening/AddCaseRespondents', data: { respondents: params },
            success: function (result) {

                if (result.Status == "Done") {
                    notifySuccess('Data Saved Successfully!.');

                    RequestSubmitted();
                    if (buttonId == 2) {
                        document.location.href = document.location.href;
                    } else if (buttonId == 3) {
                        document.location.href = '/CaseOpening/PetitionList?dataentry=true';
                    } else if (buttonId == 4) {
                        document.location.href = '/CaseOpening/Associations?dataentry=true';
                    } else if (buttonId == 5) {
                        document.location.href = wizardUrl;

                    } else {
                        document.location.href = '/CaseOpening/LegalParties';

                    }
                }
            },
            dataType: 'json'
        });
    }

}
