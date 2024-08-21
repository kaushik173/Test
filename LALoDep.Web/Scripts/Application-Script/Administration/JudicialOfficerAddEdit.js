function isValid() {
    if ($("#FirstName").val().length == 0) {
        $("#FirstName").focus();
        notifyDanger("First name is rerquired.");
        return false;
    }

    if ($("#LastName").val().length == 0) {
        $("#LastName").focus();
        notifyDanger("Last name is rerquired.");
        return false;
    }

    if ($("#StartDate").val().length == 0) {
        $("#StartDate").focus();
        notifyDanger("Start Date is rerquired.");
        return false;
    }

    if ($("#EndDate").val() != "" && (new Date($("#StartDate").val()) > new Date($("#EndDate").val()))) {
        $("#EndDate").focus();
        notifyDanger("End date can not be before start date.");
        return false;
    }

    if ($("#dvAgencies .chk-agency:checked").length == 0) {
        $("#dvAgencies .chk-agency:first").focus();
        notifyDanger("Atleast one agency must be selected.");
        return false;
    }

    return true;
}

function getData() {
    var data = {
        PersonID: $("#PersonID").val(),
        LastName: $("#LastName").val(),
        FirstName: $("#FirstName").val(),
        StartDate: $("#StartDate").val(),
        EndDate: $("#EndDate").val(),
        AgencyList: []
    };

    $("#dvAgencies .chk-agency").each(function () {
        var isChecked = $(this).is(":checked");
        if (isChecked) {
            data.AgencyList.push({
                AgencyID: $(this).data("id"),
                RoleID: $(this).data("roleid"),
                Selected: 1,
                RecordStateID: $(this).data("recordstateid")
            });
        }
        else {
            if ($(this).data("selected") == 1) { // Agencies to Delete
                data.AgencyList.push({
                    AgencyID: $(this).data("id"),
                    RoleID: $(this).data("roleid"),
                    Selected: 0,
                    RecordStateID: $(this).data("recordstateid")
                });
            }
        }
    });

    return data;
}

function saveData() {
    IPadKeyboardFix();
    if (isValid()) {
        var data = getData();
        $.ajax({
            type: "POST", url: "/JudicialOfficer/AddEditSave", data: data,
            success: function (result) {
                RequestSubmitted();
                if (result.isSuccess) {
                    window.location.href = "/JudicialOfficer/Search"
                }
            }
        });
    }
}
$("#chkSelectAll").on("click", function () {
    var isChecked = $(this).is(":checked");
    $("#dvAgencies .chk-agency").each(function (e) {
        $(this).prop("checked", isChecked);
    });
});

$("#dvAgencies").on("click", ".chk-agency", function () {
    var allSelected = $("#dvAgencies .chk-agency").length == $("#dvAgencies .chk-agency:checked").length;
    $("#chkSelectAll").prop("checked", allSelected);
});

$("#btnSave").on("click", function (e) {
    e.preventDefault();

    if (!IsValidFormRequest()) {
        return false;
    }

    if (hasFormChanged("JudicialOfficer-AddEdit-form")) {
        saveData();
    }
    else {
        window.location.href = "/JudicialOfficer/Search";
    }
});

$("#btnDelete").on("click", function () {
    confirmBox("Are you sure want to delete?", function (result) {
        if (result) {
            $.ajax({
                type: "POST", url: '/JudicialOfficer/DeleteJudicialOfficer/' + $("#EncryptedPersonID").val(),
                success: function (data) {
                    if (data.isSuccess) {
                        //notifySuccess("Judicial officer is deleted successfuly");
                        window.location.href = "/JudicialOfficer/Search"
                    }
                },
                dataType: 'json'
            });
        }
    });
});

$(document).ready(function () {
    setInitialFormValues("JudicialOfficer-AddEdit-form");
});