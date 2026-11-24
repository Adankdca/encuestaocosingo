<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Prueba.aspx.cs" Inherits="Encuestasopm.Prueba" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
.modal-lg { 
    max-width: 95% !important; 
} 
.modal-sm { 
    max-width: 50% !important; 
} 
</style>
<style>
.intermitente{
  border: 0px solid black;
  padding: 20px 20px;
  box-shadow: 0px 0px 20px;
  animation: infinite resplandorAnimation 3s;
  
}
@keyframes resplandorAnimation {
  0%,100%{
    box-shadow: 0px 0px 20px;
  }
  50%{
  box-shadow: 0px 0px 0px;
  
  }

}
</style>
<script  type="text/javascript">
    function descarga(archivo) {
        var filename = archivo;
        document.location = filename;

    }
</script>
<script type="text/javascript">
    $(document).ready(function () {
        $("#<%=hidden_link.ClientID%>").fancybox({
            'titlePosition': 'over',
            'titleFormat': 'formatTitle',
            'type': 'iframe',
            'width': '98%',
            'height': '100%',
            'hideOnOverlayClick': false,
            'hideOnContentClick': false,
            'overlayOpacity': 0.7,
            'enableEscapeButton': false,
            'openEffect': 'elastic',
            'closeEffect': 'elastic',
            iframe: {
                preload: false // fixes issue with iframe and IE
            }
        });
    });
    function miFuncion() {
        $("#<%=hidden_link.ClientID%>").trigger('click');
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<form id="form1" runat="server" enctype="multipart/form-data">
<%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
<a id="hidden_link" runat="server" data-fancybox-type="iframe" style="visibility: hidden;"></a>
<div class="kt-content  kt-grid__item kt-grid__item--fluid" id="kt_content" style="min-width:100%;">
<div class="kt-section" style="margin: 0 0 1rem 0; background-color:white;">
	
	<div class="kt-section__content kt-section__content--solid">
		<h3>
			Conoce a que se refiere cada Plan o Programa 
			<small class="text-muted">con la que deseas participar</small>
		</h3>
	</div>
</div>
<div class="kt-section__content kt-section__content--solid">
	<a href="javascript:void(0);" id="planes" class="btn btn-primary btn-wide" runat="server" onserverclick="planes_ServerClick"  ><i class="fa fa-print fa-3x"></i></a>
	<a href="javascript:void(0);" id="temas" class="btn btn-primary btn-wide" runat="server" onserverclick="temas_ServerClick"  ><i class="fa fa-print fa-3x"></i></a>
	<a href="javascript:void(0);" id="confianza" class="btn btn-primary btn-wide" runat="server" onserverclick="confianza_ServerClick"  ><i class="fa fa-print fa-3x"></i></a>

</div>
<div class="row">
<div class="col-xl-6 col-lg-12">

	<!--Begin::Portlet-->
	<div class="kt-portlet kt-portlet--height-fluid">
		<div class="kt-portlet__head">
			<div class="kt-portlet__head-label">
				<h3 class="kt-portlet__head-title" style="font-size:30px;">
					PLANES
				</h3>
			</div>
											
		</div>
		<div class="kt-portlet__body">
			<div class="kt-widget12">
			<div class="kt-widget12__content">
				<div class="kt-widget12__item">
					<div class="kt-widget12__info">
						<span class="kt-widget12__value"><a href="#" style="color: #6c7293;" class="kt-link kt-link--warning kt-font-bolder" data-toggle="kt-popover" title="Titulo" data-content="Definición de Pnal Municipal">	Plan Municipal</a></span>
					</div>
					<div class="kt-widget12__info">
						<span class="kt-widget12__value"><a href="#" style="color: #6c7293;font-size: 1.29rem;" class="kt-link kt-link--warning kt-font-bolder" data-toggle="kt-popover" title="Titulo" data-content="Definición de Plan municipal de desarrollo municipal">	Plan Municipal de Desarrollo Urbano</a></span>
					</div>
				</div>
				<div class="kt-widget12__item">
					<div class="kt-widget12__info">
						<span class="kt-widget12__value"><a href="#" style="color: #6c7293;" class="kt-link kt-link--warning kt-font-bolder" data-toggle="kt-popover" title="Titulo" data-content="Plan Municipal de Salud">	Plan Municipal de Salud</a></span>
					</div>
					<div class="kt-widget12__info">
						<span class="kt-widget12__value"><a href="#" style="color: #6c7293;" class="kt-link kt-link--warning kt-font-bolder" data-toggle="kt-popover" title="Titulo" data-content="Plan Municipal en Vivienda">	Plan Municipal en Vivienda</a></span>
					</div>
				</div>
				<div class="kt-widget12__item">
					<div class="kt-widget12__info">
						<span class="kt-widget12__value"><a href="#" style="color: #6c7293;" class="kt-link kt-link--warning kt-font-bolder" data-toggle="kt-popover" title="Titulo" data-content="Plan Municipal Educativo">	Plan Municipal Educativo</a></span>
					</div>
					<div class="kt-widget12__info">
						<span class="kt-widget12__value"><a href="#" style="color: #6c7293;" class="kt-link kt-link--warning kt-font-bolder" data-toggle="kt-popover" title="Titulo" data-content="Plan Municipal Económico">	Plan Municipal Económico</a></span>
					</div>
				</div>
			</div>
		</div>
			
			
		</div>
	</div>
	
	<!--End::Portlet-->
</div>
<div class="col-xl-6 col-lg-12">

	<!--Begin::Portlet-->
	<div class="kt-portlet kt-portlet--height-fluid">
		<div class="kt-portlet__head">
			<div class="kt-portlet__head-label">
				<h3 class="kt-portlet__head-title" style="font-size:30px;">
					PROGRAMAS
				</h3>
			</div>
											
		</div>
		<div class="kt-portlet__body">
			<div class="kt-widget12">
			<div class="kt-widget12__content">
				<div class="kt-widget12__item">
					<div class="kt-widget12__info">
						<span class="kt-widget12__value"><a href="#" style="color: #6c7293;" class="kt-link kt-link--warning kt-font-bolder" data-toggle="kt-popover" title="Titulo" data-content="Barriguita llena">	Barriguita llena</a></span>
					</div>
					<div class="kt-widget12__info">
						<span class="kt-widget12__value"><a href="#" style="color: #6c7293;" class="kt-link kt-link--warning kt-font-bolder" data-toggle="kt-popover" title="Titulo" data-content="Apoyos productivos">	Apoyos productivos</a></span>
					</div>
				</div>
				<div class="kt-widget12__item">
					<div class="kt-widget12__info">
						<span class="kt-widget12__value"><a href="#" style="color: #6c7293;" class="kt-link kt-link--warning kt-font-bolder" data-toggle="kt-popover" title="Titulo" data-content="Programa de apoyos DIF">	Programa de apoyos DIF</a></span>
					</div>
					<div class="kt-widget12__info">
						<span class="kt-widget12__value"><a href="#" style="color: #6c7293;" class="kt-link kt-link--warning kt-font-bolder" data-toggle="kt-popover" title="Titulo" data-content="Becas escolares">	Becas escolares</a></span>
					</div>
				</div>
				<div class="kt-widget12__item">
					<div class="kt-widget12__info">
						<span class="kt-widget12__value"><a href="#" style="color: #6c7293;" class="kt-link kt-link--warning kt-font-bolder" data-toggle="kt-popover" title="Titulo" data-content="Asuntos jurídicos">	Asuntos jurídicos</a></span>
					</div>
					<div class="kt-widget12__info">
						<span class="kt-widget12__value"><a href="#" style="color: #6c7293;" class="kt-link kt-link--warning kt-font-bolder" data-toggle="kt-popover" title="Titulo" data-content="Otros programas">	Otros programas</a></span>
					</div>
				</div>
			</div>
		</div>
	
		</div>
	</div>

	<!--End::Portlet-->
	
</div>
</div>
<div class="row">
	<div class="col-xl-12">
		<img src="img/encabezado2.png" class="img-fluid" />
	</div>
</div>
<div class="row">
<div class="col-xl-12">
<div class="kt-portlet  kt-portlet--border-bottom-brand">
			<div class="kt-portlet__body ">
				<div class="kt-portlet__content">
				<div class="row">
				<div class="col-xl-4">

					<img src="img/ojo.png" class="img-fluid rounded float-left" />											
	
				</div>
				<div class="col-xl-8">
					<div class="row" style="padding-bottom:5%;">
						<div class="col-xl-12">
							<h3 class="kt-label-font-color-2">Para que a través de este espacio, los ciudadanos de Ocosingo participen</h3>
							<h3>con sus opiniones en el mejoramiento de la gestión pública Municipal.</h3>
						</div>
					</div>
					<div class="row" style="padding-bottom:5%;">
						<div class="col-xl-7">
							<a href="#" onclick="encuestaobra2020()"><img src="img/ojoencuesta.png" class="img-fluid rounded float-left intermitente" style="color: #a7abc3;"/></a>
						</div>
						<div class="col-xl-5" style="justify-content: center;align-items: center;font-size:25px;">
							<div class="row">
								<div class="col-xl-12">

								</div>
							</div>
							<div class="row">
								<div class="col-xl-12" style="padding-top:6%">
									<img src="assets/media/icons/svg/Navigation/Angle-double-left.svg"/><img src="assets/media/icons/svg/Navigation/Angle-double-left.svg"/>Dar click para participar
								</div>
							</div>
							<div class="row">
								<div class="col-xl-12">

								</div>
							</div>
						</div>
					</div>
					<div class="row">
						<div class="col-xl-12">
							<strong style="color: #e8052a !important;">Sólo te llevará 3 minutos:</strong>Para el Gobierno Municipal es muy valiosa tu opinión, por lo que tu participación contribuye a cimentar una democracia de resultados.
						</div>
					</div>
		
				</div>
			</div>												
				
			</div>
		</div>
	</div>
</div>
	
</div>
<div class="modal fade" id="kt_modal_inicial" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabelbienvenida" aria-hidden="true">
				<div class="modal-dialog modal-sm" role="document">
					<div class="modal-content" >
						<div class="modal-header">
							<h5 class="modal-title" id="exampleModalLabel1">Ocosingo,Chiapas</h5>
							<button type="button" class="close" data-dismiss="modal" aria-label="Close">
							</button>
						</div>
						<div class="modal-body">
							<div class="row">
								<div class="col-lg-12" >
									<img src="img/bienvenida.jpg" class="img-fluid rounded" alt="Responsive image">

								</div>
							</div>
						</div>
					</div>
				</div>
		</div>
<div class="modal fade" id="kt_modal_1" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
		<div class="modal-dialog modal-lg" role="document">
			<div class="modal-content" >
				<div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Ocosingo,Chiapas</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    </button>
                </div>
				<div class="modal-body">
					<div class="row">
						<div class="col-lg-12" id="encuesta2020">
							
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>
</form>
</asp:Content>
