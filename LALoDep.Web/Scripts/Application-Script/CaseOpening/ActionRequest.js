
$('body').on('click', '.deleteAR', function () {
    var $this = $(this);
    confirmBox("Are you sure you want to delete?", function (result) {

        if (result) {
            $.ajax({
                type: "POST", url: '/CaseOpening/ActionRequestDelete/' + $this.attr('data-id'),
                success: function (result) {
                    if (result.Status == "Done") {
                        Notify('Action Request Deleted Successfully!.', 'bottom-right', '3000', 'success', 'fa-smile-o', true);
                        document.location.href = document.location.href;
                    } else {
                        document.location.href = result.URL;

                    }
                },
                dataType: 'json'
            });

        }

    });

 

});

$('.printAR').on('click', function (e) {

    e.preventDefault();
    PrintAR($(this).attr('data-id'));


});

$('.printProfile').on('click', function () {
    PrintARProfile($(this));
});
function PrintAR(id) {
    var data = {
        'id': id,
    }
   $.download($('#hdnCurrentSessionGuidPath').val()+'/CaseOpening/ActionRequestPrint', data, "POST", 'target="_blank"');
  
}
function PrintARProfile(el) {
    var data = {
        rfdId: $(el).attr('data-rfdId'),
        roleId: $(el).attr('data-roleid'),
        profileTypeCodeId: $(el).attr('data-profileTypeCodeId'),
    }

   $.download($('#hdnCurrentSessionGuidPath').val()+'/Task/PrintARProfile/', data, "POST", 'target="_blank"');
}
$('.addActionRequest').on('click', function (e) {

    e.preventDefault();
    var url = $(this).attr('href');
    if (isCaseClosed == '1') {
      
        confirmBox("This is a closed case, are you sure you want to add an action request to this case?", function (result) {

            if (result) {
                document.location.href = url;

            }

        });
    }
    else {
        document.location.href = url;
    }



});

