
$('#save').on('click', function () {
    Save(1);
});

$('#saveMore').on('click', function () {
    Save(2);
});

$('#cancel').on('click', function () {
    document.location.href = '/Users';
});

$('.delete').on('click', function () {
    $this = $(this);
    confirmBox("Are you sure you want to delete?", function (result) {
        if (result) {


            $.ajax({
                type: "POST",
                url: '/Users/DeleteUserRole?roleId=' + $this.parents('tr').find('.item_RoleID').val(),
                success: function (result) {
                    $this.parents('tr').remove();
                },
                dataType: 'json'
            });
            //$('#pageChanges').val('1');
        }
    });
});

$('#btnDelete').on('click', function () {
    $this = $(this);
    confirmBox("Are you sure you want to delete?", function (result) {
        if (result) {
            $this.parents('tr').hide();
            $.ajax({
                type: "POST",
                url: '/Users/DeleteUser?userId=' + $jcatsUserID,
                success: function (result) {
                    if (result.Status === "Done") {
                        notifySuccess('User Deleted Successfully.');
                        document.location.href = '/Users';
                    } else {
                        document.location.href = result.URL;
                    }
                },
                dataType: 'json'
            });
        }
    });
});

$('#btnLoginAsUser').on('click', function () {
    $.ajax({
        type: "POST",
        url: '/Users/LoginInAs?username=' + $('#JcatsUserData_JcatsUserLoginName').val(),
        success: function (result) {
            if (result.Status === "Done") {
                document.location.href = '/' + result.URL;
            } else if (result.Status === "Fail") {
                document.location.href = result.URL;
            } else {
                notifyDanger(result.Message);
            }
        },
        dataType: 'json'
    });
});

$("#JcatsUserData_AgencyID").on("change", function () {
    populateSupervisor($(this).val());
});

function Validation() {
    var isValid = true;
    if (!hasFormChanged('UserAddEditForm')) {
        notifyDanger('Nothing was changed.');
        isValid = false;
        return false;
    }
    if ($('#JcatsUserData_PersonNameFirst').val() === '') {
        isValid = false;
        $('#JcatsUserData_PersonNameFirst').focus();
        notifyDanger('First Name is required.');
        return false;
    }
    if ($('#JcatsUserData_PersonNameLast').val() === '') {
        isValid = false;
        $('#JcatsUserData_PersonNameLast').focus();
        notifyDanger('Last Name is required.');
        return false;
    }
    //if ($('#JcatsUserData_RoleTypeCodeID').val() === '') {
    //    isValid = false;
    //    $('#JcatsUserData_RoleTypeCodeID').focus();
    //    notifyDanger('Role is required.');
    //    return false;
    //}
    if ($('#JcatsUserData_JcatsUserTimeOut').val() !== '' && !($('#JcatsUserData_JcatsUserTimeOut').val() >= 1 && $('#JcatsUserData_JcatsUserTimeOut').val() <= 480)) {
        isValid = false;
        $('#JcatsUserData_JcatsUserTimeOut').focus();
        notifyDanger('Timeout should be greater than or equals 1 and less than or equals 480 minutes!');
        return false;
    }
    if ($('#JcatsUserData_JcatsUserLoginName').val() !== '') {
        if (($jcatsUserID === '' || $jcatsUserID === '0') && $('#JcatsUserData_JcatsUserPassword').val() === '') {
            isValid = false;
            $('#JcatsUserData_JcatsUserPassword').focus();
            notifyDanger('Password is required.');
            return false;
        }
        if ($('#JcatsUserData_JcatsGroupID').val() === '') {
            isValid = false;
            $('#JcatsUserData_JcatsGroupID').focus();
            notifyDanger('Security Group is required.');
            return false;
        }
        if ($('#JcatsUserData_JcatsUserStartDate').val() === '') {
            isValid = false;
            $('#JcatsUserData_JcatsUserStartDate').focus();
            notifyDanger('User Start Date is required.');
            return false;
        }
    }
    else if ($('#JcatsUserData_JcatsUserPassword').val() !== '' || $('#JcatsUserData_JcatsGroupID').val() !== '' || $('#JcatsUserData_JcatsUserLevelCodeID').val() !== ''
        || $('#JcatsUserData_JcatsUserStartDate').val() !== '' || $('#JcatsUserData_JcatsUserEndDate').val() !== '') {
        isValid = false;
        $('#JcatsUserData_JcatsUserLoginName').focus();
        notifyDanger('Login Name is required.');
        return false;
    }

    if ($('#JcatsUserData_JcatsUserLoginName').val() !== '' && $('#JcatsUserData_JcatsGroupID').val() !== '') {
        if ($('#JcatsUserData_AgencyID').val() === "") {
            $('#JcatsUserData_AgencyID').focus();
            isValid = false;
            notifyDanger('User Home Agency is required.');
            return false;
        }
    }

    if ($('#StaffInfo_StaffInfoBarNumber').val() !== '') {
        if ($('#StaffInfo_StaffInfoBarAdmittedDate').val() === '') {
            isValid = false;
            $('#StaffInfo_StaffInfoBarAdmittedDate').focus();
            notifyDanger('Bar Admitted Date is required.');
            return false;
        }
        if ($('#StaffInfo_StaffInfoEligibilityEffectiveDate').val() === '') {
            isValid = false;
            $('#StaffInfo_StaffInfoEligibilityEffectiveDate').focus();
            notifyDanger('Eligibility Effective Date is required.');
            return false;
        }
    }
    else if ($('#StaffInfo_StaffInfoBarAdmittedDate').val() !== '' || $('#StaffInfo_StaffInfoEligibilityEffectiveDate').val() !== '' || $('#StaffInfo_StaffInfoEligibilityEndingDate').val() !== '') {
        isValid = false;
        $('#StaffInfo_StaffInfoBarNumber').focus();
        notifyDanger('Bar # is required.');
        return false;
    }

    if ($('#StaffInfo_UsePrimaryEmail').is(':checked') && $('#StaffInfo_EmailPrimary').val() === '') {
        isValid = false;
        $('#StaffInfo_EmailPrimary').focus();
        notifyDanger('Primary Email is required if Use Primary Email is checked.');
        return false;
    }
    if ($('#StaffInfo_UseSecondaryEmail').is(':checked') && $('#StaffInfo_EmailSecondary').val() === '') {
        isValid = false;
        $('#StaffInfo_EmailSecondary').focus();
        notifyDanger('Secondary Email is required if Use Secondary Email is checked.');
        return false;
    }
    if ($('#StaffInfo_EmailToAlternatePersonContactFlag').is(':checked') && $('#StaffInfo_AlternateContactPersonID').val() === '') {
        isValid = false;
        $('#StaffInfo_AlternateContactPersonID').focus();
        notifyDanger('Alternate Contact Email is required if Include Alternative Contact Email is checked.');
        return false;
    }

    if ($('#JcatsUserData_JcatsUserEndDate').val() != '' && $('#JcatsUserData_JcatsUserStartDate').val() != '' && moment($('#JcatsUserData_JcatsUserStartDate').val()) > moment($('#JcatsUserData_JcatsUserEndDate').val())) {
        isValid = false;
        $('#JcatsUserData_JcatsUserEndDate').focus();
        notifyDanger('User End Date must be after User Start Date.');
        return false;

    }

    //if ($('input.HomeAgency:checked').length === 0) {
    //    if ($('.ddlAgencyID:eq(0)').val() === "") {
    //        isValid = false;
    //        $('.ddlAgencyID:eq(0)').focus();
    //        notifyDanger('Agency is required.');
    //        return false;
    //    }
    //}

    if ($('#TitleIVeStaffInfo_PercentBenefitsCAC').val() != '' && !($('#TitleIVeStaffInfo_PercentBenefitsCAC').val() >= 0 && $('#TitleIVeStaffInfo_PercentBenefitsCAC').val() <= 100)) {
        isValid = false;
        $('#TitleIVeStaffInfo_PercentBenefitsCAC').focus();
        notifyDanger('% Benefits paid by CAC should be between 0 and 100.');
        return false;

    }
    if ($('#TitleIVeStaffInfo_PercentBenefitsFFDRP').val() != '' && !($('#TitleIVeStaffInfo_PercentBenefitsFFDRP').val() >= 0 && $('#TitleIVeStaffInfo_PercentBenefitsFFDRP').val() <= 100)) {
        isValid = false;
        $('#TitleIVeStaffInfo_PercentBenefitsFFDRP').focus();
        notifyDanger('% Benefits paid by FFDRP should be between 0 and 100.');
        return false;

    }
    if ($('#TitleIVeStaffInfo_PercentBenefitsFFDRP').val() != '' && $('#TitleIVeStaffInfo_PercentBenefitsCAC').val() != '' && (parseFloat($('#TitleIVeStaffInfo_PercentBenefitsFFDRP').val()) + parseFloat($('#TitleIVeStaffInfo_PercentBenefitsCAC').val())) > 100) {
        isValid = false;
        $('#TitleIVeStaffInfo_PercentBenefitsFFDRP').focus();
        notifyDanger('% Benefits paid by CAC and % Benefits paid by FFDRP cannot exceed 100%.');
        return false;

    }
    var isRoleExists = false;
    $('.agencyInfoRow').each(function () {
        if ($(this).find('.item_RoleTypeCodeID').val() > 0) {
            isRoleExists = true;
        }
        if (($(this).find('.item_AgencyID').val() > 0 || $(this).find('.item_RoleStartDate').val() !== "") && $(this).find(".item_RoleTypeCodeID").val() === '') {
            isValid = false;
            $(this).find('.item_RoleTypeCodeID').focus();
            notifyDanger('Role is required.');
            return false;
        }

        if (($(this).find(".item_RoleTypeCodeID").val() > 0 || $(this).find('.item_AgencyID').val() > 0) && $(this).find('.item_RoleStartDate').val() === '') {
            isValid = false;
            $(this).find('.item_RoleStartDate').focus();
            notifyDanger('Start Date is required.');
            return false;
        }

        if (($(this).find(".item_RoleTypeCodeID").val() > 0 || $(this).find('.item_RoleID').val() > 0) && ($(this).find('.item_AgencyID').val() === null || $(this).find('.item_AgencyID').val() === "")) {
            isValid = false;
            $(this).find('.item_AgencyID').focus();
            notifyDanger('Agency is required.');
            return false;
        }

        if ($(this).find('.item_RoleStartDate').val() != '' && $(this).find('.item_RoleEndDate').val() != '' && moment($(this).find('.item_RoleStartDate').val()) > moment($(this).find('.item_RoleEndDate').val())) {
            isValid = false;
            $(this).find('.item_RoleEndDate').focus();
            notifyDanger('Role End Date must be after Role Start Date.');
            return false;

        }
    });
    if (!isRoleExists) {
        isValid = false;
        $('.item_RoleTypeCodeID:eq(0)').focus();
        notifyDanger('At least one Role is required');
        return false;
    }

    return isValid;
}

function Save(buttonId) {
    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return false;
    }
    $('#TitleIVeStaffInfo_MonthlySalaryAndBenefits').toNumber();
    var isvalid = Validation();
    if (isvalid) {
        SaveData(false, buttonId);
    }
    $('#TitleIVeStaffInfo_MonthlySalaryAndBenefits').formatCurrency();
}

function SaveData(updateDepartment, buttonId) {
    var userInfo = {
        'PersonNameSalutationCodeID': $('#JcatsUserData_PersonNameSalutationCodeID').val(),
        'PersonNameFirst': $('#JcatsUserData_PersonNameFirst').val(),
        'PersonNameLast': $('#JcatsUserData_PersonNameLast').val(),
        'JcatsGroupID': $('#JcatsUserData_JcatsGroupID').val(),
        'JcatsUserLoginName': $('#JcatsUserData_JcatsUserLoginName').val(),
        'JcatsUserStartDate': $('#JcatsUserData_JcatsUserStartDate').val(),
        'JcatsUserEndDate': $('#JcatsUserData_JcatsUserEndDate').val(),
        'PersonNameSuffixCodeID': $('#JcatsUserData_PersonNameSuffixCodeID').val(),
        //'RoleTypeCodeID': $('#JcatsUserData_RoleTypeCodeID').val(),

        'JcatsUserLevelCodeID': $('#JcatsUserData_JcatsUserLevelCodeID').val(),
        'JcatsUserPassword': $('#JcatsUserData_JcatsUserPassword').val(),
        'JcatsUserID': $jcatsUserID,
        'PersonID': $personID,
        'PersonNameID': $('#JcatsUserData_PersonNameID').val(),
        'JcatsUserTimeOut': $('#JcatsUserData_JcatsUserTimeOut').val(),
        'AgencyID': $('#JcatsUserData_AgencyID').val()
    };

    var staffInfo = {
        'StaffInfoID': $('#StaffInfo_StaffInfoID').val(),
        'StaffInfoBarNumber': $('#StaffInfo_StaffInfoBarNumber').val(),
        'EmailPrimary': $('#StaffInfo_EmailPrimary').val(),
        'EmailSecondary': $('#StaffInfo_EmailSecondary').val(),
        'Fax': $('#StaffInfo_Fax').val(),
        'AlternateContactPersonID': $('#StaffInfo_AlternateContactPersonID').val(),
        'MobilePhone': $('#StaffInfo_MobilePhone').val(),
        'WorkPhone': $('#StaffInfo_WorkPhone').val(),
        'StaffInfoBarAdmittedDate': $('#StaffInfo_StaffInfoBarAdmittedDate').val(),
        'StaffInfoEligibilityEffectiveDate': $('#StaffInfo_StaffInfoEligibilityEffectiveDate').val(),
        'StaffInfoEligibilityEndingDate': $('#StaffInfo_StaffInfoEligibilityEndingDate').val(),

        'EmailToAlternatePersonContactFlag': $('#StaffInfo_EmailToAlternatePersonContactFlag').is(':checked'),
        'StaffInfoEmployeeStatusCodeID': $('#StaffInfo_StaffInfoEmployeeStatusCodeID').val(),
        'EmailToPrimaryPersonContactID': $('#StaffInfo_EmailToPrimaryPersonContactID').val(),
        'EmailToSecondaryPersonContactID': $('#StaffInfo_EmailToSecondaryPersonContactID').val(),
        'MobilePersonContactID': $('#StaffInfo_MobilePersonContactID').val(),
        'WorkPersonContactID': $('#StaffInfo_WorkPersonContactID').val(),
        'FaxPersonContactID': $('#StaffInfo_FaxPersonContactID').val(),
        'EmailPrimaryPersonContactID': $('#StaffInfo_EmailPrimaryPersonContactID').val(),
        'EmailSecondaryPersonContactID': $('#StaffInfo_EmailSecondaryPersonContactID').val(),
        'UsePrimaryEmail': $('#StaffInfo_UsePrimaryEmail').is(':checked'),
        'UseSecondaryEmail': $('#StaffInfo_UseSecondaryEmail').is(':checked'),
        'StaffInfoEmployeeID': $('#StaffInfo_StaffInfoEmployeeID').val(),

    };


    var titleIVeStaffInfo = {
        'StaffTitle': $('#TitleIVeStaffInfo_StaffTitle').val(),
        'PercentBenefitsCAC': $('#TitleIVeStaffInfo_PercentBenefitsCAC').val(),
        'PercentBenefitsFFDRP': $('#TitleIVeStaffInfo_PercentBenefitsFFDRP').val(),
        'MonthlySalaryAndBenefits': $('#TitleIVeStaffInfo_MonthlySalaryAndBenefits').val(),
        'FullTime': $('#TitleIVeStaffInfo_FullTime').is(':checked'),
        'IsEmployee': $('#TitleIVeStaffInfo_IsEmployee').is(':checked')?1:0,
        'TotalContractPayments': $('#TitleIVeStaffInfo_TotalContractPayments').val(),
        'AlternateWorkSchedule': $('#TitleIVeStaffInfo_AlternateWorkSchedule').val(),
        'NormalWorkHours': $('#Titl9eIVeStaffInfo_NormalWorkHours').val(),
        'NeedsActivityLog': $('#TitleIVeStaffInfo_NeedsActivityLog').is(':checked'),
        'OHCodeID': $('#TitleIVeStaffInfo_OHCodeID').val(),

    };

    var agencyInfo = [];
    $('.agencyInfoRow').each(function () {

        agencyInfo.push({
            'RoleTypeCodeID': $(this).find('.item_RoleTypeCodeID').val(),
            'AgencyID': $(this).find('.item_AgencyID').val(),
            'RoleStartDate': $(this).find('.item_RoleStartDate').val(),
            'RoleEndDate': $(this).find('.item_RoleEndDate').val(),
            //'Selected': $(this).find('.HomeAgency').is(':checked') ? 1 : 0,
            'RoleID': $(this).find('.item_RoleID').val(),
            'RecordStateID': $(this).is(':visible') ? 1 : 10
        });
    });

    var params = {
        'JcatsUserData': userInfo,
        'StaffInfo': staffInfo,
        'SelectedAgencyList': agencyInfo,
        'SupervisorPersonID': $("#SupervisorPersonID").val(),
        'SupervisorChanged': $("#SupervisorPersonID").IsValueChanged(),
        'TitleIVeStaffInfo': titleIVeStaffInfo,

    };
    console.log(params);

    $.ajax({
        type: "POST",
        url: '/Users/AddEdit',
        data: { model: params },
        success: function (result) {

            if (result.Status === "Done") {
                RequestSubmitted();
                notifySuccess('Data Saved Successfully!.');
                if (buttonId === 2) {
                    document.location.href = '/Users/AddEdit?jcatsUserID=' + result.UserID + '&personID=' + result.PersonID;
                } else {
                    document.location.href = '/Users';

                }
            } else {

                notifyDanger(result.Message);

            }
        },
        dataType: 'json'
    });
}

function populateSupervisor(agencyId) {
    if ($("#SupervisorPersonID").length <= 0)
        return;

    var options = '<option value=""></option>';
    var selectedVal = '';
    $("#SupervisorPersonID").empty();
    supervisorList.forEach(function (person) {
        if (agencyId !== '') {
            if (person.AgencyID === parseInt(agencyId)) {
                options += '<option value="' + person.PersonID + '">' + person.SupervisorDisplay + '</option>';
            }
        }
        else {
            options += '<option value="' + person.PersonID + '">' + person.SupervisorDisplay + '</option>';
        }
        if (person.Selected === 1) {
            selectedVal = person.PersonID;
        }
    });
    $("#SupervisorPersonID").html(options);
    $("#SupervisorPersonID").val(selectedVal);
}
$(function () {
    if ($jcatsUserID !== '' && $jcatsUserID !== '0') {
        $('#JcatsUserData_JcatsUserLoginName').prop('disabled', true);
    }

    if ($personID !== '' && $personID !== '0') {
        var agencyId = $("#JcatsUserData_AgencyID").val();
        populateSupervisor(agencyId);
    }

    setInitialFormValues('UserAddEditForm');
    $('#TitleIVeStaffInfo_MonthlySalaryAndBenefits').formatCurrency();
});


$('#TitleIVeStaffInfo_MonthlySalaryAndBenefits').on('blur', function (e) {

    $(this).formatCurrency();
    return;

});