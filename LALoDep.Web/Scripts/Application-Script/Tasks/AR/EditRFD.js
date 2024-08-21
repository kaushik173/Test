var oldModelData;

var validateTabClose = true;
$(function () {
	if (pageReadOnly) {
		$('input,select,textarea', "#AREdit").prop("disabled", true)
	}
	setInitialFormValuesWithOutSummernote('AREdit', true);
	oldModelData = $('#AREdit').serialize();
	
	$('.InvestigatorNote .NoteEntry').each(function () {

		 
		AutoSaveNote($(this).attr('id'), 'InvestigatorNote-' + $('#HearingReportFilingDueID').val(), 1, 'Investigator Recommendation/Evaluation', function () {

		});
	});
});

var autoSaveInterval = setInterval(function () {
	console.log('AutoSaveNote interval')
	$('.InvestigatorNote .NoteEntry').each(function () {
	 
		AutoSaveNote($(this).attr('id'), 'InvestigatorNote-' + $('#HearingReportFilingDueID').val(), 0);
	});
	
}, 30000);
 
$('#btnSave').on('click', function () {
	Save(1, false);
});
$('#btnSaveAndPrint').on('click', function () {
	Save(3, false);
});
$('#btnSaveExit').on('click', function () {
	Save(2, false);
});
$('#btnSaveAndMain').on('click', function () {
	Save(5, false);
});

$("input#CompletedDate").on("blur", function () {
	if ($(this).val() == "") {
		$("#chkCompleted").val(false);
	}
});

$('body').on('change', 'input#CompletedDate', function (ev) {
	if ($(this).val() == "") {
		$("#chkCompleted").val(false);
		$("#Completed").val('false');
	}
	else {
		$("#chkCompleted").val(true);
		$("#Completed").val('true');
	}
});

var wizardUrl = '';
$('.wizardstep a').on('click', function (e) {
	e.preventDefault();
	wizardUrl = $(this).attr('href');
	Save(4,false);
});

var associations = [];

function Validation(buttonId) {
	var isValid = true;
	associations = [];

	if (!hasFormChanged('AREdit')) {
		if (buttonId == 1) {
			document.location.href = '/Task/EditRFDAddress/' + $('#EncryptHearingReportFilingDueID').val();
			isValid = false;
			return false;
		} else if (buttonId == 2) {
			if ($exitUrl.length > 0) {
				document.location.href = $exitUrl;
				return false;
			}
			isValid = false;
			return false;
		} else if (buttonId == 3) {
			Print();
			isValid = false;
			return false;


		} else if (buttonId == 4) {
			document.location.href = wizardUrl;
			return false;
		} else if (buttonId == 5) {
			document.location.href = '/Case/Main';

			return false;
		}
		notifyDanger('Nothing was changed.');
		isValid = false;
		return false;
	}

	if ($('#RequestForID').val() == '') {
		isValid = false;
		$('#RequestForID').focus();
		notifyDanger('Request For is required.');
		return false;
	}
	if ($('#DueDate').val() == '') {
		isValid = false;
		$('#DueDate').focus();
		notifyDanger('Due Date is required.');
		return false;
	}
	if (moment($('#DueDate').val()) < moment($('#RequestDate').val())) {
		isValid = false;
		$('#DueDate').focus();
		notifyDanger('Due Date can not be before request date.');
		return false;
	} if (moment($('#CompletedDate').val()) < moment($('#RequestDate').val())) {
		isValid = false;
		$('#CompletedDate').focus();
		notifyDanger('Completion Date cannot be before Request Date.');
		return false;
	}
	if ($('#CompletedDate').is(':visible') && $('#CompletedDate').val() != '' && $('#CompletedDate').IsValueChanged() && moment($('#CompletedDate').val()) != moment()) {
		isValid = false;

		notifyDanger('Completion Date must be today\'s date');
		$('#CompletedDate').focus();
		return false;
	}
	$('.ContactDate').each(function () {
		if (moment($(this).val()) < moment($('#RequestDate').val())) {
			isValid = false;
			$(this).focus();
			notifyDanger('Date Completed cannot be before Request Date.');
			return false;
		} if (moment($(this).val()) > moment()) {
			isValid = false;
			$(this).focus();
			notifyDanger('Date Completed cannot be in the future.');
			return false;
		}
		if ($('#CompletedDate').is(':visible') && moment($(this).val()) > moment($('#CompletedDate').val())) {
			isValid = false;
			$(this).focus();
			notifyDanger('Date Completed cannot be greater than the Completion Date.');
			return false;
		}
	});
	$('.noteBoxList').each(function () {
		$tr = $(this);
		if (!isValid)
			return isValid;
		


			if ($tr.find('.NoteEntry').hasClass('required') && $tr.find('.NoteEntry').GetText() == '') {
				isValid = false;
				Notify($tr.find('.NoteEntry').data('title') + ' is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
				$tr.find('.NoteEntry').trigger('focus');
				return false;
			}
		 
	});

	return isValid;
}
function Save(buttonId, forceCreateARAnyway) {

	IPadKeyboardFix();

	if (!IsValidFormRequest()) {
		return;
	}


	var isvalid = Validation(buttonId);

	if (isvalid) {
		var disableAgain = false;
		if ($('#DueDate').is(':disabled')) {
			$('#DueDate').prop('disabled', false);
			disableAgain = true;
		}

		var params = serializeData($('#AREdit'));
		params.ForceCreateARAnyway = forceCreateARAnyway;
		if (disableAgain) {
			$('#DueDate').prop('disabled', true);
		}

		$.ajax({
			type: "POST", url: '/Task/EditRFD', data: params, dataType: 'json',
			success: function (result) {
				
				

				if (result.Status == "Done") {
					$('.InvestigatorNote .NoteEntry').each(function () {
						clearInterval(autoSaveInterval);
						AutoSaveNote($(this).attr('id'), 'InvestigatorNote-' + $('#HearingReportFilingDueID').val(), 2);
					});
					validateTabClose = false;
					notifySuccess('Data Saved Successfully!.');

					if (buttonId == 1) {
						RequestSubmitted();
						document.location.href = '/Task/EditRFDAddress/' + $('#EncryptHearingReportFilingDueID').val();
					} else if (buttonId == 2) {
						RequestSubmitted();
						if ($exitUrl.length > 0) {
							document.location.href = $exitUrl;
							return false;
						}

					} else if (buttonId == 4) {
						document.location.href = wizardUrl;
						return false;
					} else if (buttonId == 5) {
						document.location.href = '/Case/Main';

						return false;
					}
					else {
						Print();

						RequestSubmitted();
						setTimeout(function () {
							document.location.href = document.location.href;
						}, 2000)

					}

				} else if (result.Status == "AssignmentFail") {
					if (result.DialogType == 1) {
						AlertBoxWithCallback(result.ErrorMessage, function () {
							$('#RequestForID').focus();

						})
					} else if (result.DialogType == 2) {
						confirmBox(result.ErrorMessage, function (result) {
							if (result) {
								Save(buttonId, true);
							} else {
								$('#RequestForID').focus();
							}


						})
					}
				} else {
					validateTabClose = false;

					document.location.href = result.URL;

				}
			},
			dataType: 'json'
		});
	}

}
function Print() {
	var data = {
		'id': $('#EncryptHearingReportFilingDueID').val(),
	}

	$.download($('#hdnCurrentSessionGuidPath').val() + '/CaseOpening/ActionRequestPrint/' + $('#EncryptHearingReportFilingDueID').val(), data, "POST", 'target="_blank"');

}
function serializeData($form) {
	var $disabledFields = $form.find('[disabled]');
	$disabledFields.prop('disabled', false); // enable fields so they are included


	//  var data = $form.serialize();
	var data = {};
	var unindexed_array = $form.serializeArray();
	$.map(unindexed_array, function (n, i) {

		data[n['name']] = $('#' + n['name']).GetHtmlWithEscape();
	});

	data.NoteBoxList = [];
	$('.noteBoxList').each(function () {
		$tr = $(this);
		if ($tr.parents('.noteBoxList').length == 0) {


			data.NoteBoxList.push({
				'NoteEntry': $tr.find('.NoteEntry').GetHtmlWithEscape(),
				'NoteID': $tr.find('.NoteCodeHiddenNoteId').val(),
				'CodeID': $tr.find('.NoteCodeId').val(),
				'NoteEntityCodeID': $tr.find('.NoteEntityCodeID').val(),
				'NoteEntityTypeCodeID': $tr.find('.NoteEntityTypeCodeID').val(),
				'NoteEntityTypeCodeID': $tr.find('.NoteEntityTypeCodeID').val(),
				'NoteTypeCodeID': $tr.find('.NoteTypeCodeID').val(),

			});
		}
	});
	//if ($('#InvestigatorEvaluationNote.summernote').length > 0) {
	//    data = data.replace('InvestigatorEvaluationNote=', 'InvestigatorEvaluationNoteold=') + '&InvestigatorEvaluationNote=' + $('#InvestigatorEvaluationNote').GetHtmlWithEscape()

	//}
	//if ($('#CaretakerEvaluationNote.summernote').length > 0) {
	//    data = data.replace('CaretakerEvaluationNote=', 'CaretakerEvaluationNoteold=') + '&CaretakerEvaluationNote=' + $('#CaretakerEvaluationNote').GetHtmlWithEscape()

	//}
	//if ($('#SocialWorkerEvaluationNote.summernote').length > 0) {
	//    data = data.replace('SocialWorkerEvaluationNote=', 'SocialWorkerEvaluationNoteold=') + '&SocialWorkerEvaluationNote=' + $('#SocialWorkerEvaluationNote').GetHtmlWithEscape()


	//}
	//if ($('#TherapistEvaluationNote.summernote').length > 0) {
	//    data = data.replace('TherapistEvaluationNote=', 'TherapistEvaluationNoteold=') + '&TherapistEvaluationNote=' + $('#TherapistEvaluationNote').GetHtmlWithEscape()
	//}
	//if ($('#ProbationOfficerEvaluationNote.summernote').length > 0) {
	//    data = data.replace('ProbationOfficerEvaluationNote=', 'ProbationOfficerEvaluationNoteold=') + '&ProbationOfficerEvaluationNote=' + $('#ProbationOfficerEvaluationNote').GetHtmlWithEscape()

	//}
	$disabledFields.prop('disabled', true); // back to disabled
	return data;
}


window.onbeforeunload = function (e) {

	if (hasFormChanged('AREdit') && validateTabClose) {
		return 'There is unsaved data.';
	}
	return undefined;
}