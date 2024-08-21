var oldModelData;
$(function () {


    setInitialFormValues('question-form', true);


});


var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
    e.preventDefault();
    wizardUrl = $(this).attr('href');
    Save($('#btnUpdate').length > 0 ? 5 : 4);

}); $('#btnContinue').on('click', function () {

    Save(1);

});
$('#btnAddMore').on('click', function () {

    Save(2);

});
$('#btnCancel').on('click', function () {
    document.location.href = $(this).attr('data-href');


});
$('#btnUpdate').on('click', function () {
    Save(3);

});

$('.btnNote').on('click', function () {
    OpenPopup('/Task/ARProfileQuestionNote?profileId=' + $(this).attr('data-profileId'), 'Note');
});
$('.deleteProfile').on('click', function (e) {
    e.preventDefault();
    var $this = $(this);
    confirmBox("Are you sure you want to delete?", function (result) {

        if (result) {
            $.ajax({
                type: "POST",
                url: '/Task/DeleteRFDProfile/' + $this.attr('data-id'),

                dataType: 'json',
                success: function (data) {
                    if (data.Status == 'Done') {
                        notifySuccess('Profile Removed Successfully!.');
                        document.location.href = document.location.href;
                    } else {
                        document.location.href = data.URL;
                    }
                }
            });
        }

    });

});


var associations = [];

function Validation(buttonId) {
    var isValid = true;

    if (!hasFormChanged('question-form')) {
        if (buttonId == 1) {
            document.location.href = $('#NextQuestionUrl').val();
            isValid = false;
        }
        if (buttonId == 3) {
            document.location.href = $('#btnUpdate').attr('data-href');
            isValid = false;
        }
        if (buttonId == 2) {
            notifyDanger("Nothing was changed");
            isValid = false;
        } if (buttonId == 4 || buttonId == 5) {
            document.location.href = wizardUrl;
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
        if (buttonId == 3 || buttonId == 5) {//Update Record
            //var params = $('#question-form').serialize();
            //if ($('#NoteEntry.summernote').length > 0) {
            //    params = params.replace('NoteEntry=', 'NoteEntryold=') + '&NoteEntry=' + $('#NoteEntry').GetHtmlWithEscape()
            //}
            var data = {};
            var unindexed_array = $('#question-form').serializeArray();
            $.map(unindexed_array, function (n, i) {
               
                    data[n['name']] = $('#' + n['name']).GetHtml();
            });
            $.ajax({
                type: "POST", url: '/Task/UpdateRFDProfileQuestion', data: data, 
                success: function (result) {

                    if (result.Status == "Done") {
                        RequestSubmitted();
                        notifySuccess('Data Saved Successfully!.');
                        if (buttonId == 4 || buttonId == 5) {
                            document.location.href = wizardUrl;
                            return false;

                        }

                         document.location.href = result.URL;


                    } else {
                        document.location.href = result.URL;

                    }
                },
                dataType: 'json'
            });
        } else {//Add New
            var selectedRoleIds = "";
            $('.chkSelectPerson:checked').each(function () {
                if (selectedRoleIds != "")
                    selectedRoleIds += ",";
                selectedRoleIds += $(this).attr('data-roleid');
            });

            //var params = $('#question-form').serialize() + "&SelectedRoleIdsForCopyAnswer=" + selectedRoleIds;
            //if ($('#NoteEntry.summernote').length > 0) {
            //    params = params.replace('NoteEntry=', 'NoteEntryold=') + '&NoteEntry=' + $('#NoteEntry').GetHtmlWithEscape()
            //}
            var data = {};
            var unindexed_array = $('#question-form').serializeArray();
            $.map(unindexed_array, function (n, i) {

                data[n['name']] = $('#' + n['name']).GetHtml();
            });
            data["SelectedRoleIdsForCopyAnswer"] = selectedRoleIds;
            $.ajax({
                type: "POST", url: '/Task/SaveRFDProfileQuestion', data: data,
                success: function (result) {

                    if (result.Status == "Done") {

                        notifySuccess('Data Saved Successfully!.');

                        if (buttonId == 1) {
                            RequestSubmitted();
                            document.location.href = $('#NextQuestionUrl').val();
                        } else if (buttonId == 2) {
                            RequestSubmitted();
                            document.location.href = document.location.href;
                        } else if (buttonId == 4 || buttonId == 5) {
                            document.location.href = wizardUrl;
                            return false;

                        }


                    } else {
                        document.location.href = result.URL;

                    }
                },
                dataType: 'json'
            });
        }
    }

}