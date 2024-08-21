var isValidData = function () {

	if ($("#LastName").val() == "") {
		$("#LastName").focus();
		notifyDanger('Last Name is required.');
		return false;
	}
	if ($("#FirstName").val() == "") {
		$("#FirstName").focus();
		notifyDanger('First Name is required.');
		return false;
	} if ($("#DOBRequiredForChildren").val() == "1" && $("#PersonDOB").val() == "" && $("#RoleTypeCodeID").val() == "3") {
		$("#PersonDOB").focus();
		notifyDanger('DOB is required.');
		return false;
	}

	if ($("#PersonSexCodeID").val() == "") {
		$("#PersonSexCodeID").focus();
		notifyDanger('Gender is required.');
		return false;
	} if ($("#RoleTypeCodeID").val() == "") {

		notifyDanger('Role is required.');
		$("#RoleTypeCodeID").focus();
		return false;
	}

	if ($("#RoleStartDate").val() == "") {
		$("#RoleStartDate").focus();
		notifyDanger('Start Date is required.');
		return false;
	}

	if ($("#RoleEndDate").val() != "" && (new Date($("#RoleStartDate").val()) > new Date($("#RoleEndDate").val()))) {
		$("#RoleEndDate").focus();
		notifyDanger('End date can not be before Start Date.');
		return false;
	}

	if ($("#DeceasedDate").val() != "") {
		$("#IsDeceased").prop("checked", true);
	}
	if ($('#PersonRaceVerbalValue').hasValue()) {
		//   $('.chkPersonRace[data-other="1"]').prop("checked", true);
	}


	var classifications = $("tr.tblClassificationTr");
	for (var indx = 0; indx < classifications.length; indx++) {
		var classification = classifications.eq(indx);

		ddlPersonClassificationCodeID = classification.find(".PersonClassificationCodeID");
		txtPersonClassificationStartDate = classification.find(".PersonClassificationStartDate");
		txtPersonClassificationEndDate = classification.find(".PersonClassificationEndDate");
		ddlPersonClassificationEndReasonCodeID = classification.find(".PersonClassificationEndReasonCodeID");

		if (ddlPersonClassificationCodeID.val() != "" || txtPersonClassificationStartDate.val() != "" ||
			txtPersonClassificationEndDate.val() != "" || ddlPersonClassificationEndReasonCodeID.val() != "") {

			if (ddlPersonClassificationCodeID.val() == "") {
				ddlPersonClassificationCodeID.focus();
				notifyDanger('Classification is required.');
				return false;
			}

			if (txtPersonClassificationStartDate.val() == "") {
				txtPersonClassificationStartDate.focus();
				notifyDanger('Start date is required.');
				return false;
			}

			if (txtPersonClassificationEndDate.val() != "" && (new Date(txtPersonClassificationStartDate.val()) > new Date(txtPersonClassificationEndDate.val()))) {
				txtPersonClassificationEndDate.focus();
				notifyDanger('End date can not be before Start Date.');
				return false;
			}

			if (txtPersonClassificationEndDate.val() != "" && ddlPersonClassificationEndReasonCodeID.val() == "") {
				ddlPersonClassificationEndReasonCodeID.focus();
				notifyDanger('End Reason is required when End date is entered.');
				return false;
			}

			if (txtPersonClassificationEndDate.val() == "" && ddlPersonClassificationEndReasonCodeID.val() != "") {
				txtPersonClassificationEndDate.focus();
				notifyDanger('End Date is required when End reson is selected.');
				return false;
			}

		}
	}


	var allMedications = $("#tblMedication tbody tr");
	for (var indx = 0; indx < allMedications.length; indx++) {
		var medication = allMedications.eq(indx);

		var ddlMedicationCodeID = medication.find(".MedicationCodeID");
		var txtMedicationDosage = medication.find(".MedicationDosage");
		var ddlMedicationFrequencyCodeID = medication.find(".MedicationFrequencyCodeID");
		var txtMedicationStartDate = medication.find(".MedicationStartDate");
		var txtMedicationEndDate = medication.find(".MedicationEndDate");

		if (ddlMedicationCodeID.val() != "" || txtMedicationDosage.val() != "" ||
			ddlMedicationFrequencyCodeID.val() != "" || txtMedicationStartDate.val() != "" || txtMedicationEndDate.val() != "") {

			if (ddlMedicationCodeID.val() == "") {
				ddlMedicationCodeID.focus();
				notifyDanger('Medication is required.');
				return false;
			}

			if (txtMedicationDosage.val() == "") {
				txtMedicationDosage.focus();
				notifyDanger('Medication Dosage is required.');
				return false;
			}

			if (ddlMedicationFrequencyCodeID.val() == "") {
				ddlMedicationFrequencyCodeID.focus();
				notifyDanger('Medication Frequency is required.');
				return false;
			}

			if (txtMedicationStartDate.val() == "") {
				txtMedicationStartDate.focus();
				notifyDanger('Start Date is required.');
				return false;
			}

			if (txtMedicationEndDate.val() != "" && (new Date(txtMedicationStartDate.val()) > new Date(txtMedicationEndDate.val()))) {
				txtMedicationEndDate.focus();
				notifyDanger('End date can not be before Start Date.');
				return false;
			}
		}
	}


	if ($('#PersonRaceVerbalValue').hasValue() && $('.chkPersonRace:checked').length == 0) {

		$('.chkPersonRace:first').focus();
		notifyDanger('At least one Race must be checked if Client verbally identifies as is entered.');
		return false;
	}
	if (!$('#PersonRaceVerbalValue').hasValue() && $('.chkPersonRace[data-other="1"]:checked').length > 0) {

		$('#PersonRaceVerbalValue').focus();
		notifyDanger('Other Client verbally identifies as is required if Other is checked.');
		return false;
	}

	return true;

}
var getData = function (buttonId) {
	var data = {
		"PersonID": $("#PersonID").val(),
		"RecordStateID": $("#RecordStateID").val(),
		"AgencyID": $("#AgencyID").val(),

		"PersonNameID": $("#PersonNameID").val(),
		"PersonNameTypeCodeID": $("#PersonNameTypeCodeID").val(),
		"PersonNameMiddle": $("#PersonNameMiddle").val(),
		"PersonNameRecordStateID": $("#PersonNameRecordStateID").val(),

		"RoleID": $("#RoleID").val(),
		"RoleTypeCodeID": $("#RoleTypeCodeID").val(),

		"IsClient": $("#IsClient").is(":checked"),
		"LastName": $("#LastName").val(),
		"FirstName": $("#FirstName").val(),
		"PersonDOB": $("#PersonDOB").val(),
		"PersonRaceCodeID": $("#PersonRaceCodeID").val(),
		"PersonSexCodeID": $("#PersonSexCodeID").val(),
		"RoleStartDate": $("#RoleStartDate").val(),
		"RoleEndDate": $("#RoleEndDate").val(),

		"IsDeceased": $("#IsDeceased").is(":checked"),
		"DeceasedDate": $("#DeceasedDate").val(),
		"DeceasedDate_PersonClassID": $("#DeceasedDate_PersonClassID").val(),

		"PersonLanguageCodeID": $("#PersonLanguageCodeID").val(),

		//Case Attributes
		"DesignatedDayCaseAttrID": ($("#DesignatedDayCodeID").IsValueChanged() ? $("#DesignatedDayCaseAttrID").val() : ""),
		"DesignatedDayCodeID": ($("#DesignatedDayCodeID").IsValueChanged() ? $("#DesignatedDayCodeID").val() : ""),

		"VOCCaseAttrID": ($("#VOC").IsValueChanged() ? $("#VOCCaseAttrID").val() : ""),
		"VOC": ($("#VOC").IsValueChanged() ? $("#VOC").val() : ""),

		"VOCStatusCaseAttrID": ($("#VOCStatusCodeID").IsValueChanged() ? $("#VOCStatusCaseAttrID").val() : ""),
		"VOCStatusCodeID": ($("#VOCStatusCodeID").IsValueChanged() ? $("#VOCStatusCodeID").val() : ""),

		"PersonClassifications": [],
		"PersionMedications": [],
		"PersonRaceIUDList": [],
		"ButtonId": buttonId,

		"UpdateDeceased": ($("#IsDeceased").is(":Checked") != $("#IsDeceased").data("old-value-on-pageload") ||
			$("#DeceasedDate").IsValueChanged()),

		"UpdatePerson": ($("#PersonDOB").IsValueChanged() ||
			$("#PersonRaceCodeID").IsValueChanged() ||
			$("#PersonSexCodeID").IsValueChanged() ||
			$("#PersonLanguageCodeID").IsValueChanged()),

		"UpdatePersonName": ($("#FirstName").IsValueChanged() ||
			$("#LastName").IsValueChanged()),

		"UpdateRole": ($("#IsClient").is(":Checked") != $("#IsClient").data("old-value-on-pageload") ||
			$("#RoleStartDate").IsValueChanged() ||
			$("#RoleEndDate").IsValueChanged() || $("#RoleTypeCodeID").IsValueChanged())
	};

	var classifications = $("tr.tblClassificationTr");
	for (var indx = 0; indx < classifications.length; indx++) {
		var classification = classifications.eq(indx);

		ddlPersonClassificationCodeID = classification.find(".PersonClassificationCodeID");
		txtPersonClassificationStartDate = classification.find(".PersonClassificationStartDate");
		txtPersonClassificationEndDate = classification.find(".PersonClassificationEndDate");
		ddlPersonClassificationEndReasonCodeID = classification.find(".PersonClassificationEndReasonCodeID");
		chkDelClassification = classification.find(".DelClassification");

		//check classification is added/deleted or not
		if (ddlPersonClassificationCodeID.val() != "" || txtPersonClassificationStartDate.val() != "" ||
			txtPersonClassificationEndDate.val() != "" || ddlPersonClassificationEndReasonCodeID.val() != "" ||
			(chkDelClassification != undefined && chkDelClassification.is(":checked"))) {

			//Check is existing classification changed or not
			//if (classification.data("classificationcodeid") != ddlPersonClassificationCodeID.val()
			//    || classification.data("classificationstartdate") != txtPersonClassificationStartDate.val()
			//    || classification.data("classificationenddate") != txtPersonClassificationEndDate.val()
			//    || classification.data("classificationendreasoncodeid") != ddlPersonClassificationEndReasonCodeID.val()
			//    || (chkDelClassification != undefined && chkDelClassification.is(":checked"))) {

			if (ddlPersonClassificationCodeID.IsValueChanged()
				|| txtPersonClassificationStartDate.IsValueChanged()
				|| txtPersonClassificationEndDate.IsValueChanged()
				|| ddlPersonClassificationEndReasonCodeID.IsValueChanged()
				|| (chkDelClassification != undefined && chkDelClassification.is(":checked"))) {
				data.PersonClassifications.push({
					"PersonClassificationID": classification.data("id"),
					"PersonClassificationCodeID": ddlPersonClassificationCodeID.val(),
					"PersonClassificationStartDate": txtPersonClassificationStartDate.val(),
					"PersonClassificationEndDate": txtPersonClassificationEndDate.val(),
					"PersonClassificationEndReasonCodeID": ddlPersonClassificationEndReasonCodeID.val(),
					"RecordStateID": classification.data("recordstateid"),
					"DoDelete": (chkDelClassification != undefined && chkDelClassification.is(":checked"))
				});
			}

		}
	}


	var allMedications = $("#tblMedication tbody tr");
	for (var indx = 0; indx < allMedications.length; indx++) {
		var medication = allMedications.eq(indx);

		var ddlMedicationCodeID = medication.find(".MedicationCodeID");
		var txtMedicationDosage = medication.find(".MedicationDosage");
		var ddlMedicationFrequencyCodeID = medication.find(".MedicationFrequencyCodeID");
		var txtMedicationStartDate = medication.find(".MedicationStartDate");
		var txtMedicationEndDate = medication.find(".MedicationEndDate");
		var chkDelMedication = medication.find(".DelMedication");

		if (ddlMedicationCodeID.val() != "" || txtMedicationDosage.val() != "" || ddlMedicationFrequencyCodeID.val() != ""
			|| txtMedicationStartDate.val() != "" || txtMedicationEndDate.val() != ""
			|| (chkDelMedication != undefined && chkDelMedication.is(":checked"))) {

			//if (medication.data("codeid") != ddlMedicationCodeID.val() ||
			//    medication.data("dosage") != txtMedicationDosage.val() ||
			//    medication.data("frequencycodeid") != ddlMedicationFrequencyCodeID.val() ||
			//    medication.data("startdate") != txtMedicationStartDate.val() ||
			//    medication.data("enddate") != txtMedicationEndDate.val() ||
			//    (chkDelMedication != undefined && chkDelMedication.is(":checked"))) {
			if (ddlMedicationCodeID.IsValueChanged() ||
				txtMedicationDosage.IsValueChanged() ||
				ddlMedicationFrequencyCodeID.IsValueChanged() ||
				txtMedicationStartDate.IsValueChanged() ||
				txtMedicationEndDate.IsValueChanged() ||
				(chkDelMedication != undefined && chkDelMedication.is(":checked"))) {

				data.PersionMedications.push({
					"MedicationID": medication.data("id"),
					"MedicationCodeID": ddlMedicationCodeID.val(),
					"MedicationDosage": txtMedicationDosage.val(),
					"MedicationFrequencyCodeID": ddlMedicationFrequencyCodeID.val(),
					"MedicationStartDate": txtMedicationStartDate.val(),
					"MedicationEndDate": txtMedicationEndDate.val(),
					"DoDelete": (chkDelMedication != undefined && chkDelMedication.is(":checked"))
				});
			}
		}
	}



	$('.chkPersonRace').each(function () {
		if ($(this).IsCheckboxChanged() || $('#PersonRaceVerbalValue').IsValueChanged()) {
			if ($(this).attr('data-personraceid') > 0 && !$(this).is(':checked')) {
				data.PersonRaceIUDList.push({
					"IUD": "DELETE",
					"PersonRaceID": $(this).attr('data-personraceid'),
					"PersonID": $(this).attr('data-personid'),
					"PersonRaceCodeID": $(this).attr('data-personracecodeid'),
					"PersonRaceVerbalValue": $('#PersonRaceVerbalValue').val(),

				});
			} else if ($(this).attr('data-personraceid') > 0 && $(this).is(':checked') && $('#PersonRaceVerbalValue').IsValueChanged()) {
				data.PersonRaceIUDList.push({
					"IUD": "UPDATE",
					"PersonRaceID": $(this).attr('data-personraceid'),
					"PersonID": $(this).attr('data-personid'),
					"PersonRaceCodeID": $(this).attr('data-personracecodeid'),
					"PersonRaceVerbalValue": $('#PersonRaceVerbalValue').val(),
				});
			} else {
				if ($(this).is(':checked')) {
					data.PersonRaceIUDList.push({
						"IUD": "INSERT",
						"PersonRaceID": $(this).attr('data-personraceid'),
						"PersonID": $(this).attr('data-personid'),
						"PersonRaceCodeID": $(this).attr('data-personracecodeid'),
						"PersonRaceVerbalValue": $('#PersonRaceVerbalValue').val(),

					});
				}

			}


		}

	})

	return data;

	//if (!data.UpdatePerson && !data.UpdatePersonName && !data.UpdateRole
	//    && data.DesignatedDayCaseAttrID == "" && data.DesignatedDayCodeID == ""
	//    && data.VOCCaseAttrID == "" && data.VOC == ""
	//    && data.VOCStatusCaseAttrID == "" && data.VOCStatusCodeID == ""
	//    && data.PersonClassifications.length <= 0 && data.PersionMedications.length <= 0) {
	//    return null; // Nothing has been changed
	//}
	//else {
	//    return data;
	//}
}

var saveData = function (buttonId) {
	IPadKeyboardFix();
	if (!IsValidFormRequest()) {
		return false;
	}

	if (hasFormChanged("EditPersion-form")) {
		if (isValidData()) {
			if ($(".DelMedication:checked").length > 0 || $(".DelClassification:checked").length > 0) {
				confirmBox("Are you sure you want to remove selected records?", function (result) {
					if (result) {

						submitData(buttonId);
					}
				});
			} else {
				submitData(buttonId);
			}



		}
	}
	else {
		if (buttonId == 1) {
			notifyDanger("Nothing has been changed");
		} else if (buttonId == 3) {
			window.location.href = $('#btnSaveAppearanceSheet').data('href');
		}
		else {
			window.location.href = '/Case/Main';
		}
	}
}
function submitData(buttonId) {
	var data = getData(buttonId);
	$.ajax({
		type: "POST", dataType: 'json', url: '/Case/EditPerson', data: data,
		success: function (result) {
			RequestSubmitted();

			if (result.isSuccess) {
				if (buttonId == 3) {
					window.location.href = $('#btnSaveAppearanceSheet').data('href');
				} else if (result.URL != '') {
					window.location.href = result.URL;
				}
			}
			else {
				notifyDanger('There is something wrong while processing request.', 'bottom-right', '4000', 'danger', 'fa-info', true);
			}
		}
	});
}
$("#btnSave").on("click", function () {


	saveData(1);
});

$("#btnSaveMain").on("click", function () {
	saveData(2);
});


$("#btnSaveAppearanceSheet").on("click", function () {
	saveData(3);
});
$(document).ready(function () {

	$('select[data-default]').each(function () {

		$(this).val($(this).attr('data-default'))
	});
	$('#btnShowMore').click(function () {
		if ($('#btnShowMore').text() == 'Show More') {
			$('#btnShowMore').text('Show Less');
			$('.tblClassificationTrHidden').removeClass('hideTr')

		} else {
			$('#btnShowMore').text('Show More');
			$('.tblClassificationTrHidden').addClass('hideTr')
		}

	});


	$('#PersonDOB').change(function () {

		if ($("#PersonDOB").val().length > 0) {
			$('#hdnAge').val(moment().diff(moment($("#PersonDOB").val(), 'MM/DD/YYYY'), 'years'))

		}
		if ($('div[data-name="35_Preverbal"]').length > 0) {
			if ($('#hdnAge').val() < 5) {
				$('.divRaceBox').hide();
				$('.chkPersonRace').prop('checked', false);
				$('div[data-name="35_Preverbal"]').show();
			} else {
				$('.divRaceBox').show();
				$('.chkPersonRace').prop('checked', false);
				$('div[data-name="35_Preverbal"]').hide();
			}
			$('div[data-name="35_Preverbal"] .chkPersonRace').attr('disabled', 'disabled');
		}
	});

	//$("#PersonDOB").datepicker().on("changeDate", function (e) {

	// 	$("#PersonDOB").change();
	//})

	$('.chkPersonRace').click(function () {

		console.log($(this).closest('.divRaceBox').data('name'))
		if ($(this).is(':checked')) {
			var name = $(this).closest('.divRaceBox').data('name');
			if (name == 'Away from Care' || name == 'Client Declines To State' || name == 'Client Does Not Know' || name == 'NonVerbal') {
				$('.chkPersonRace').not($(this)).prop('checked', false);
			} else {
				$('.chkPersonRace:checked').each(function () {
					name = $(this).closest('.divRaceBox').data('name');
					if (name == 'Away from Care' || name == 'Client Declines To State' || name == 'Client Does Not Know' || name == 'NonVerbal') {
						$('.chkPersonRace').not($(this)).prop('checked', false);
					}
				});
			}
		}



	});

	//$("#PersonDOB").change();

	if ($("#PersonDOB").val().length > 0) {
		$('#hdnAge').val(moment().diff(moment($("#PersonDOB").val(), 'MM/DD/YYYY'), 'years'))

	}
	if ($('div[data-name="35_Preverbal"]').length > 0) {
		if ($('#hdnAge').val() < 5) {
			$('.divRaceBox').hide();
			//   $('.chkPersonRace').prop('checked', false);
			$('div[data-name="35_Preverbal"]').show();
		} else {
			$('.divRaceBox').show();
			//   $('.chkPersonRace').prop('checked', false);
			$('div[data-name="35_Preverbal"]').hide();
		}
	} else {
		$('.divRaceBox').show();
	}

	$('div[data-name="35_Preverbal"] .chkPersonRace').attr('disabled', 'disabled');
	setInitialFormValues("EditPersion-form");
});