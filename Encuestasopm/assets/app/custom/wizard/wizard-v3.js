"use strict";

// Class definition
var KTWizard3 = function () {
	// Base elements
	var wizardEl;
	var formEl;
	var validator;
	var wizard;
	
	// Private functions
	var initWizard = function () {
		// Initialize form wizard
		wizard = new KTWizard('kt_wizard_denuncia', {
			startStep: 1,
		});

		// Validation before going to next page
		wizard.on('beforeNext', function(wizardObj) {
			if (validator.form() !== true) {
				wizardObj.stop();  // don't go to the next step
			}
		})

		// Change event
		wizard.on('change', function(wizard) {
			KTUtil.scrollTop();	
		});
	}

	var initValidation = function() {
		validator = formEl.validate({
			// Validate only visible fields
			ignore: ":hidden",

			// Validation rules
			rules: {
				//= Step 1
				txtnombre: {
					required: true 
				},
				txtdireccion: {
					required: true
				},
				txttelefono: {
					required: true
				},
				txtemail: {
					required: true
				},
				txtfechainiciocurrio: {
					required: true
				},
				txtfechafinocurrio: {
					required: true
				},
				txtnarracion: {
					required: true, minlength:50
				},
				txtnomdenunciado: {
					required: true
				},
				txtpatdenunciado: {
					required: true
				},
				txtmatdenunciado: {
					required: true
				},
				txtadscripcion: {
					required: true
				},
				txtpuesto: {
					required: true
				}
			},
			
			// Display error  
			invalidHandler: function(event, validator) {	 
				KTUtil.scrollTop();

				swal.fire({
					"title": "", 
					"text": "Por favor llena todos los campos de forma correcta.", 
					"type": "error",
					"confirmButtonClass": "btn btn-secondary"
				});
			},

			// Submit valid form
			submitHandler: function (form) {
				
			}
		});   
	}

	var initSubmit = function() {
		var btn = formEl.find('[data-ktwizard-type="action-submit"]');

		btn.on('click', function(e) {
			e.preventDefault();

			if (validator.form()) {
				// See: src\js\framework\base\app.js
				KTApp.progress(btn);
				//KTApp.block(formEl);

				// See: http://malsup.com/jquery/form/#ajaxSubmit
				formEl.ajaxSubmit({
					success: function () {
						var anonimo="0";
						var notificar="0";
						if ($('#chkanonimo').prop('checked')) {
							anonimo = "1";
						}
						if ($('#chknotificar').prop('checked')) {
							notificar = "1";
						}
						var data = "anonimo=" + anonimo;
						data += "&notificar=" + notificar;
						data += "&txtnombre=" + $("#txtnombre").val();
						data += "&txtdireccion=" + $("#txtdireccion").val();
						data += "&txttelefono=" + $("#txttelefono").val();
						data += "&txtemail=" + $("#txtemail").val();
						data += "&sltentidadocurre=" + $("#sltentidadocurre").val();
						data += "&txtfechainiciocurrio=" + $("#txtfechainiciocurrio").val();
						data += "&txtfechafinocurrio=" + $("#txtfechafinocurrio").val();
						data += "&txtnarracion=" + $("#txtnarracion").val();
						data += "&txtnomdenunciado=" + $("#txtnomdenunciado").val();
						data += "&txtpatdenunciado=" + $("#txtpatdenunciado").val();
						data += "&txtmatdenunciado=" + $("#txtmatdenunciado").val();
						data += "&sltDependencia=" + $("#sltDependencia").val();
						data += "&txtadscripcion=" + $("#txtadscripcion").val();
						data += "&txtpuesto=" + $("#txtpuesto").val();
						data += "&sltentidaddenunciado=" + $("#sltentidaddenunciado").val();
						data += "&txtnombreconsta=" + $("#txtnombreconsta").val();
						data += "&txtdireccionconsta=" + $("#txtdireccionconsta").val();
						data += "&txttelefonoconsta=" + $("#txttelefonoconsta").val();
						data += "&txtemailconsta=" + $("#txtemailconsta").val();
						data += "&radioservidor=" + $("#radioservidor").val();
						data += "&sltDependenciaConsta=" + $("#sltDependenciaConsta").val();
						data += "&txtcargoconsta=" + $("#txtcargoconsta").val();
						$.ajax({
							async: true,
							type: "GET",
							dataType: "json",
							contentType: "application/x-www-form-urlencoded",
							url: "./Home/EnviarDenuncia",
							data: data,
							success: function (result) {

							},
							error: function () { alert("Error AJAX al enviar Accion Indirecto.") }
						});
						KTApp.unprogress(btn);
						swal.fire({
							"title": "", 
							"text": "Informacion enviada satisfactoriamente!", 
							"type": "success",
							"confirmButtonClass": "btn btn-secondary"
						});
					}
				});
			}
		});
	}

	return {
		// public functions
		init: function() {
			wizardEl = KTUtil.get('kt_wizard_v3');
			formEl = $('#kt_form');

			initWizard(); 
			initValidation();
			initSubmit();
		}
	};
}();

jQuery(document).ready(function() {	
	KTWizard3.init();
});