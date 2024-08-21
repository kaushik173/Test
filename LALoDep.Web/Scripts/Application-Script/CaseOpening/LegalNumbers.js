
$(function () {


    setInitialFormValues('case-search-form');
    $('.SSNumber').mask("000-00-0000", { placeholder: "___-__-____" });
});

$('#saveAndContinue').on('click', function () {

    Save(1);

});
$('#saveAndAdd').on('click', function () {

    Save(2);

});

var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
    e.preventDefault();
    wizardUrl = $(this).attr('href');
    Save(5);

});

 

function Validation(buttonId) {
    var isValid = true;


    if (!hasFormChanged('case-search-form')) {

        if (buttonId == 1) {
            document.location.href = '/CaseOpening/CaseAddresses';
            return false;
        } else if (buttonId == 5) {
            document.location.href = wizardUrl;
            return false;
        }


        notifyDanger('Nothing was changed.' );
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


        roleList = [];
        $('#tblRoles tbody tr').each(function () {
            roleList.push({
                'PersonID': $(this).find('#item_PersonID').val(),
                'CCNumber': $(this).find('#item_CCNumber').val(),
                'HHSANumber': $(this).find('#item_HHSANumber').val(),
                'SSNumber': $(this).find('#item_SSNumber').val(),

            });

        });

        var model = {

            RoleList: roleList
        }
        var params = model;
        $.ajax({
            type: "POST", url: '/CaseOpening/LegalNumberSave', data: { model: params },
            success: function (result) {

                if (result.Status == "Done") {

                    notifySuccess('Data Saved Successfully!.' );
                    RequestSubmitted();


                    if (buttonId == 2) {
                        document.location.href = '/CaseOpening/LegalNumbers';
                    } else if (buttonId == 5) {
                        document.location.href = wizardUrl;
                        return false;
                    } else {
                        document.location.href = '/CaseOpening/CaseAddresses';

                    }
                } else {
                    document.location.href = result.URL;

                }
            },
            dataType: 'json'
        });
    }

}
