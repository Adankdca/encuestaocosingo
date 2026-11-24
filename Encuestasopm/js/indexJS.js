$(document).ready(inicializaJS);

function inicializaJS() {
    $('input[type=radio][name=radio6]').change(function () {
        if (this.value == 'N') {
            $("#sltDependenciaConsta").prop('disabled', true);
            $("#txtcargoconsta").prop('disabled', true);
            $("#radioservidor").val("N");
        }
        else if (this.value == 'S') {
            $("#sltDependenciaConsta").prop('disabled', false);
            $("#txtcargoconsta").prop('disabled', false);
            $("#radioservidor").val("S");
        }
    });
    
}