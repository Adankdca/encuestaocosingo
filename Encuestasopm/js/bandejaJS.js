//jQuery('#saluda').click(function () {
//    var nombre = jQuery('#entrada').val();
//    saludo(nombre);
//});

function seguimiento(idqueja)
{
    alert("seguimiento" + idqueja);
}
function verDetalle(idqueja) {
    var parametros = { idqueja: idqueja }
    //var parametros = "idqueja=" + queja;
        $.ajax({
            async: true,
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            //contentType: "application/x-www-form-urlencoded",
            url: '/WebService1.asmx/VerDetalleQueja',
            //url: '<%=ResolveUrl("~/WebService1.asmx/VerDetalleQueja") %>',
            //url: "./VerDetalleQueja",
            data: JSON.stringify(parametros),
            success: function (datos) {
                $('#kt_modal_4').modal('show');
                for (c = 0; c < datos.d.length; c++) {
                    $("#idqueja").html(datos[c].idqueja);
                    //$("#txtNombre").val(datos[c].folioqueja);
                    if (datos[c].tipo == "True") {
                        $("#chkanonimo").prop("checked", true);
                    }
                    if (datos[c].notificar == "True") {
                        $("#chknotificar").prop("checked", true);
                    }
                    
                    $("#txtnombre").val(datos[c].nomvic);
                    $("#txtdireccion").val(datos[c].direccionvic);
                    $("#txttelefono").val(datos[c].telefonovic);
                    $("#txtemail").val(datos[c].emailvic);
                    $('#sltentidadocurre').append("<option>" + datos[c].entidadhecho +"</option>");
                    $("#txtfechainiciocurrio").val(datos[c].fechaini);
                    $("#txtfechafinocurrio").val(datos[c].fechafin);
                    $("#txtnarracion").val(datos[c].narracion);
                    $("#txtnomdenunciado").val(datos[c].nombrecontra);
                    $('#sltDependencia').append("<option>" + datos[c].depcontra + "</option>");
                    $("#txtadscripcion").val(datos[c].adscontra);
                    $("#txtpuesto").val(datos[c].cargocontra);
                    $('#sltentidaddenunciado').append("<option>" + datos[c].entidadcontra + "</option>");
                    $("#txtnombreconsta").val(datos[c].nomconsta);
                    $("#txtdireccionconsta").val(datos[c].dirconsta);
                    $("#txttelefonoconsta").val(datos[c].telconsta);
                    $("#txtemailconsta").val(datos[c].emailconsta);
                    $("#male"). prop("checked", true);
                    $("#txtSupervisor").val(datos[c].publico);
                    if (datos[c].publico == "S") {
                        $("#radioSi").prop("checked", true);
                    }
                    else {
                        $("#radioNo").prop("checked", true);
                    }
                    $('#sltDependenciaConsta').append("<option>" + datos[c].depenconsta + "</option>");
                    $("#txtcargoconsta").val(datos[c].cargoconsta);
                    //$("#txtFechaFin").val(datos[c].fechareg);
                    //$("#txtSupervisor").val(datos[c].Estatus);
                }
                //initMap(latitud, longitud);
            },
            error: function () { alert("Error ajax, intentelo mas tarde") }
        });
    }