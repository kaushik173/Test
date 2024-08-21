
$(function () {
    setInitialFormValues('case-search-form');

    $('.chkSelected').prop('checked', true);
   
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


var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
    e.preventDefault();
    wizardUrl = $(this).attr('href');
    Save(5);

});
$('body').on('click', '.deleteAssociate', function () {
    $this = $(this);
    confirmBox("Are you sure you want to delete?", function (result) {

        if (result) {
            $.ajax({
                type: "POST", url: '/CaseOpening/AssociationDelete/' + $this.attr('data-id'),
                success: function (result) {

                    if (result.Status == "Done") {
                        notifySuccess('Association Deleted Successfully!.');
                        document.location.href = '/CaseOpening/Associations' + dataEntryQueryString;

                    } else {
                        document.location.href = result.URL;

                    }
                },
                dataType: 'json'
            });

        }

    });



});


var associations = [];

function Validation() {
    var isValid = true;
    associations = [];



    if ($('#AssociationTypeID').val() != '' || $('#PersonID').val() != '' || $('.chkPersonID:checked').length > 0) {
        if ($('#PersonID').val() == '') {
            isValid = false;
            $('#PersonID').focus();
            notifyDanger('Person is required.');
            return false;
        }
        if ($('#AssociationTypeID').val() == '') {
            isValid = false;
            $('#AssociationTypeID').focus();
            notifyDanger('Association is required.');
            return false;
        }
        if ($('.chkPersonID:checked').length == 0) {
            isValid = false;
            $('#chkPersonID').focus();
            notifyDanger('Related Person is required.');
            return false;
        }
    }



    $('#tblAssociates tbody tr').each(function () {
        if ($(this).find('#item_AssociationStartDate').val() == '') {
            isValid = false;
            $(this).find('#item_AssociationStartDate').focus();
            notifyDanger('Start Date is required.');
            return false;

        }
        if ($(this).find('#item_AssociationEndDate').val() != '' && moment($(this).find('#item_AssociationStartDate').val()) > moment($(this).find('#item_AssociationEndDate').val())) {
            isValid = false;
            $(this).find('#item_AssociationEndDate').focus();
            notifyDanger('End Date must be after Start Date.');
            return false;

        }
        if (isValid) {
            if ($(this).find('#item_AssociationStartDate').val() != $(this).find('#item_AssociationStartDate').attr('data-val') || $(this).find('#item_AssociationEndDate').val() != $(this).find('#item_AssociationEndDate').attr('data-val')) {
                associations.push({
                    'AssociationID': $(this).find('#AssociationID').val(),
                    'AssociationStartDate': $(this).find('#item_AssociationStartDate').val(),
                    'AssociationEndDate': $(this).find('#item_AssociationEndDate').val()

                });
            }
        }

    });





    return isValid;
}
function Save(buttonId) {

    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return;
    }
    if (!hasFormChanged('case-search-form')) {

        if (buttonId == 1) {
            document.location.href = '/CaseOpening/LegalNumbers';
            return false;
        }
        else if (buttonId == 3) {
            document.location.href = '/Case/Main';
            return false;
        }
        else if (buttonId == 5) {
            document.location.href = wizardUrl;
            return false;
        }

        notifyDanger('Nothing was changed.');
        isValid = false;
        return false;
    }

    var isvalid = Validation();

    if (isvalid) {
        relatedPerson = [];
        suggestedPerson = [];
        $('#tblRoles tbody tr').each(function () {
            if ($(this).find('.chkPersonID').is(':checked')) {
                relatedPerson.push({
                    'PersonID': $(this).find('.chkPersonID').attr('data-id'),
                    'RoleStartDate': $(this).find('#item_RoleStartDate').val()


                });
            }
        });
        $('#tblSuggestedPerson tbody tr').each(function () {
            if ($(this).find('.chkSelected').is(':checked')) {
                suggestedPerson.push({
                    'PersonID1': $(this).find('#PersonID1').val(),
                    'PersonID2': $(this).find('#PersonID2').val(),
                    'StartDate': $(this).find('#StartDate').val(),
                    'AssociationTypeID': $(this).find('#item_SuggestedAssociationID').val()

                });
            }
        });
        var model = {
            'PersonID': $('#PersonID').val(),
            'AssociationTypeID': $('#AssociationTypeID').val(),
            AssociationsInCase: associations,
            RelatedPersonList: relatedPerson,
            SelectedAssociationSuggestions: suggestedPerson
        }
        var params = model;
        $.ajax({
            type: "POST", url: '/CaseOpening/AssociationSave', data: { model: params },
            success: function (result) {

                if (result.Status == "Done") {
                    notifySuccess('Data Saved Successfully!.');
                    RequestSubmitted();

                    if (buttonId == 2) {
                        document.location.href = '/CaseOpening/Associations' + dataEntryQueryString;
                    }
                    else if (buttonId == 3) {
                        document.location.href = '/Case/Main';
                        return false;
                    }
                    else if (buttonId == 5) {
                        document.location.href = wizardUrl;
                        return false;
                    } else {
                        document.location.href = '/CaseOpening/LegalNumbers';

                    }
                } else {
                    document.location.href = result.URL;

                }
            },
            dataType: 'json'
        });
    }

}
