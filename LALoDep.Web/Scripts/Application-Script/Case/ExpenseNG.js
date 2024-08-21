
$(function () {
    $('.CurrencyFormat').formatCurrency();
    $('.CurrencyFormat').change(function () {
       $(this).formatCurrency();
    })

    setInitialFormValues('expenseng-form');

    $('.chkSelected').prop('checked', true);
    
});

$('.showHistory').on('click', function () {
    OpenCustomPopup('/Case/ExpenseNGStatusHistory/' + $(this).data('id'), 500, 300, 'Expense Status History');
 
});
$('#saveAndAdd').on('click', function () {
    Save(1);
});

$('#saveAndAttachFiles').on('click', function () {
    Save(2);
});

$('#btnAddNew').on('click', function () {
    Save(4);
});


$('#btnBack').on('click', function () {
    document.location.href = '/Case/ExpenseNG';
});

var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
    e.preventDefault();
    wizardUrl = $(this).attr('href');
    Save(3);

});
$('body').on('click', '.deleteExpense', function () {
    $this = $(this);
    confirmBox("Are you sure you want to delete?", function (result) {

        if (result) {
            $.ajax({
                type: "POST", url: '/Case/ExpenseNGDelete/' + $this.attr('data-id'),
                success: function (result) {

                    if (result.Status == "Done") {
                        notifySuccess('Association Deleted Successfully!.');
                        document.location.href = '/Case/ExpenseNG';

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


    if ($('#ExpenseDate').val() == '') {
        isValid = false;
        $('#ExpenseDate').focus();
        notifyDanger('Expense Date is required.'); 
        return false;
    }

    if (moment($('#ExpenseDate').val()) > moment()) {
        isValid = false;
        $('#ExpenseDate').focus();
      notifyDanger('Expense Date cannot be in the future.');
        return false;
    }
    if ($('#ExpenseTypeCodeID').val() == '') {
        isValid = false;
        $('#ExpenseTypeCodeID').focus();
        notifyDanger('Type is required.');
        return false;
    }
    if ($('#VendorName').val() == '') {
        isValid = false;
        $('#VendorName').focus();
        notifyDanger('Vendor Name is required.');
        return false;
    } if ($('#StaffPersonID').val() == '') {
        isValid = false;
        $('#StaffPersonID').focus();
        notifyDanger('Contractor is required.');
        return false;
    } if ($('#EligibleCodeID').val() == '') {
        isValid = false;
        $('#EligibleCodeID').focus();
        notifyDanger('IV-E Eligible is required.');
        return false;
    }
    if ($('#Amount').val() == '') {
        isValid = false;
        $('#Amount').focus();
        notifyDanger('Amount is required.');
        return false;
    }

    if ($('#Amount').val() <= 0) {
        isValid = false;
        $('#Amount').focus();
        notifyDanger('Amount must be greater than $0.');
        return false;
    }

    if ($('#Amount').val() > 5000) {
        isValid = false;
        $('#Amount').focus();
        notifyDanger('Amount cannot be greater than $5000.');
        return false;
    }
    if ($('#ExpenseTypeCodeID').val() == '26004' && $('#Note').val() == '') {
        isValid = false;
        $('#Note').focus();
        notifyDanger('Request Note is required.');
        return false;
    }
    if (($('#CurrentStatusCodeID').val() == '26012' || $('#PreviousAmount').val() !== '') && $('#AdminNote').val() == '') {
        isValid = false;
        $('#AdminNote').focus();
        notifyDanger('Admin Note is required.');
        return false;
    }
    return isValid;
}
function Save(buttonId) {

    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return;
    }
    if (!hasFormChanged('expenseng-form')) {

        if (buttonId == 1) {
            document.location.href = '/Case/ExpenseNG';
            return false;
        }
        else if (buttonId == 2) {
            document.location.href = '/Case/ExpenseNGFiles/' + $('#EncryptedExpenseID').val();
            return false;
        }
        else if (buttonId == 3) {
            document.location.href = wizardUrl;
            return false;
        }
        else if (buttonId == 4) {
            document.location.href = '/Case/ExpenseNG';
            return false;
        }
        notifyDanger('Nothing was changed.');
        isValid = false;
        return false;
    }

    var isvalid = Validation();

    if (isvalid) {

        $('.CurrencyFormat').toNumber();
        var params = $('#expenseng-form').serialize();
        $.ajax({
            type: "POST", url: '/Case/ExpenseNGSave', data: params,
            success: function (result) {

                if (result.Status == "Done") {
                    notifySuccess('Data Saved Successfully!.');
                    RequestSubmitted();

                    if (buttonId == 1) {
                        document.location.href = '/Case/ExpenseNG';
                        return false;
                    }
                    else if (buttonId == 2) {
                        document.location.href = '/Case/ExpenseNGFiles/' +  $('#EncryptedExpenseID').val();
                        return false;
                    }
                    else if (buttonId == 3) {
                        document.location.href = wizardUrl;
                        return false;
                    }
                    else if (buttonId == 4) {
                        document.location.href = '/Case/ExpenseNG';
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
