
$(function () {


    setInitialFormValues('frmCreateOtherAgencyCase', true);


});

$("#btnCreate").on("click", function () {
    SaveData()
});
function GetData() {
    selectedPersonIDs = '';
    $(".chkPerson:checked").each(function (index,item) {   
        if (selectedPersonIDs.length == 0) {
            selectedPersonIDs = $(item).val();
        }
        else {
            selectedPersonIDs += "," + $(item).val();
        }
    });

    var data = {
        "AttorneyAndAgencyID": $('#AttorneyAndAgencyID').val(),
        "ApptDate": $('#ApptDate').val(),
        "SelectedPersonIDs": selectedPersonIDs
    };

    
    return data;
}

function SaveData() {

    IPadKeyboardFix();
    if (!IsValidFormRequest()) {
        return;
    }
     
    if ($('#AttorneyAndAgencyID').val() == "" ) {
        notifyDanger('Select Attorney for New Case is rquired when creating an other agency case.');
        $('#AttorneyAndAgencyID').focus();
        return false;
    }
    if ($('#ApptDate').val() == "" ) {
        notifyDanger('Date Appointed is required when creating an other agency case.');
        $('#ApptDate').focus();
        return false;
    }
    if ($(".chkPerson:checked").length == 0) {
        notifyDanger('At least one client is required when creating an other agency case.');
        $(".chkPerson:first").focus();
        return false;
    }
    if ($(".chkPerson:checked").length > 1) {
        if ($(".chkPerson[data-childflag='1']:checked").length != $(".chkPerson:checked").length) {
            notifyDanger('If more than one client is selected then they must all be child roles.');
            $(".chkPerson:first").focus();
            return false;
        }

    }



    var data = JSON.stringify(GetData());


    isBottomBarAccessible(false);


    $.ajax({
        type: "POST", dataType: 'json', url: '/Case/CreateOtherAgencyCase', data: data, contentType: "application/json",
        success: function (result) {
            if (result.isSuccess) {
                RequestSubmitted();
                notifySuccess('Data Saved Successfully!');

                setTimeout(function () {

                    document.location.href = '/Case/CreateOtherAgencyCase';
                }, 2000);
            }
            else {
                notifyDanger('There is something wrong while processing request.');
            }
        }
    });


} 