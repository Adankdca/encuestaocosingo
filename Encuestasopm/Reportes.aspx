<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="Encuestasopm.Reportes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
<!--end::Fonts -->
<script src="assets/vendors/general/jquery/dist/jquery.js" type="text/javascript"></script>
<!--begin::Page Vendors Styles(used by this page) -->
<link href="assets/demo/demo9/base/style.bundle.css" rel="stylesheet" type="text/css" />
<!--==============Estilo de Fancy box================================================= -->
<link href="css/jquery.fancybox.css" type="text/css" rel="stylesheet" />
<script src="js/Reportes.js"></script>
<script type="text/javascript">$(document).ready(function () {
            $("#hidden_link1").fancybox({
                'titlePosition': 'over',
                'titleFormat': 'formatTitle',
                'type': 'iframe',
                buttons: [
                    "zoom",
                    "thumbs",
                    "close"
                ],
                'protect': true,
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
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
<%--<asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="36000000" runat="server"></asp:ScriptManager>--%>
<a id="hidden_link" runat="server" data-fancybox-type="iframe" style="visibility: hidden;"></a>
<div class="kt-content  kt-grid__item kt-grid__item--fluid" id="kt_content" style="min-width:100%;">
<div class="kt-section__content kt-section__content--solid">
	
    <a href="#" style="color: #6c7293;" class="kt-link kt-link--warning kt-font-bolder" data-toggle="kt-popover" title="Titulo" id="reporte1" runat="server" onserverclick="reporte1_ServerClick">	Plan Municipal Educativo</a>
    <a href="#" style="color: #6c7293;" class="kt-link kt-link--warning kt-font-bolder" data-toggle="kt-popover" title="Titulo" id="reporte2_temas" runat="server" onserverclick="reporte2_temas_ServerClick" >	Temas</a>

</div>
</div>
    </form>
<script src="assets/vendors/general/bootstrap/dist/js/bootstrap.min.js" type="text/javascript"></script>
<script src="js/jquery.fancybox.min.js" type="text/javascript"></script>
</body>
</html>
