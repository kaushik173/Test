function getData() {
    var fData = $('#TrainingEdit-form').serialize();
    return fData;
}

function isValidTraining() {
    if ($("#CourseTitle").val().length == 0) {
        $("#CourseTitle").focus();
        notifyDanger("Course title is required.");
        return false;
    }
    if ($("#TrainingProvider").val().length == 0) {
        $("#TrainingProvider").focus();
        notifyDanger("Training provider is required.");
        return false;
    }

    if ($("#SubjectMatter").val().length == 0) {
        $("#SubjectMatter").focus();
        notifyDanger("Subject matter is required.");
        return false;
    }

    if ($("#StartDate").val().length == 0) {
        $("#StartDate").focus();
        notifyDanger("Start date is required.");
        return false;
    }

    if ($("#EndDate").val().length > 0 && (new Date($("#EndDate").val()) < new Date($("#StartDate").val())))
    {
        $("#EndDate").focus();
        notifyDanger("End date can not be before start date.");
        return false;
    }
    if ($("#Hours").val().length == 0) {
        $("#Hours").focus();
        notifyDanger("Hours is required.");
        return false;
    }
    if (parseFloat($("#Hours").val())<0) {
        $("#Hours").focus();
        notifyDanger("Hours should not be negative.");
        return false;
    }
    if ($("#CreditTypeID").val().length == 0) {
        $("#CreditTypeID").focus();
        notifyDanger("Credit type is required.");
        return false;
    }
    if ($("#VenueID").val().length == 0) {
        $("#VenueID").focus();
        notifyDanger("Venue is required.");
        return false;
    }
    if ($("#TrainingIVeEligibleCodeID").val().length == 0 && moment($("#StartDate").val()) > moment($("#TrainingIVeEligibleCodeID").data('valdatestartdate'))) {
        $("#TrainingIVeEligibleCodeID").focus();
        notifyDanger("IV-E Eligible is required.");
        return false;
    }
    return true;
}

function saveData() {
    IPadKeyboardFix();

   
    if (!($("#TrainingIVeEligibleCodeID").val().length == 0 && moment($("#StartDate").val()) > moment($("#TrainingIVeEligibleCodeID").data('valdatestartdate')))) {
        
    
        if (!hasFormChanged("TrainingEdit-form")) {
            var personId = $("#hdn_personID").val();
            window.location.href = "/Administration/MyTraining/" + personId;
            return false;
        }
    }
    if (isValidTraining()) {
        var data = getData();
        $.ajax({
            type: "POST", url: '/TrainingSummary/TrainingEdit', data: data,
            success: function (data) {
                RequestSubmitted();
                window.location.href = "/Administration/MyTraining/" + data.Id;
            },
            dataType: 'json'
        });
    }
}


$('#update').on('click', function () {
    if (!IsValidFormRequest()) {
        return false;
    }
    else {
        saveData();
    }
});

$("#cancel").on('click', function () {
    var personId = $("#hdn_personID").val();
    window.location.href = "/Administration/MyTraining/" + personId;
});

$(document).ready(function () {
    setInitialFormValues("TrainingEdit-form");
});