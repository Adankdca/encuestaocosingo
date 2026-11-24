<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Encuestasopm.Login" %>
<!DOCTYPE html>
<html lang="es-MX">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="robots" content="noindex" />
    <title>Acceso al panel | Consulta Ciudadana Ocosingo</title>
    <link rel="icon" type="image/png" href="img/emblema-ocosingo.png" />
    <link rel="stylesheet" href="<%= Encuestasopm.Recursos.Url(ResolveUrl("~/lib/bootstrap.min.css")) %>" />
    <link rel="stylesheet" href="<%= Encuestasopm.Recursos.Url(ResolveUrl("~/css/app.css")) %>" />
</head>
<body>
<div class="login">
    <form id="form1" runat="server" class="login__caja">
        <div class="login__cabecera">
            <img src="img/logo-ocosingo-sm.png" alt="H. Ayuntamiento de Ocosingo 2024–2027" width="87" height="110" />
            <h1 class="h4 mt-3 mb-0">Panel de la Consulta Ciudadana</h1>
            <p class="text-secundario small mb-0">Acceso exclusivo para personal autorizado</p>
        </div>
        <div class="franja" aria-hidden="true"><span></span><span></span><span></span><span></span><span></span><span></span><span></span><span></span></div>
        <div class="login__cuerpo pt-4">
            <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert alert-danger py-2" role="alert">
                <asp:Literal ID="litMensaje" runat="server" />
            </asp:Panel>
            <div class="mb-3">
                <label class="form-label fw-semibold" for="txtUsuario">Usuario</label>
                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control form-control-lg" MaxLength="50" autocomplete="username" autofocus="autofocus" />
            </div>
            <div class="mb-4">
                <label class="form-label fw-semibold" for="txtPassword">Contraseña</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control form-control-lg" MaxLength="100" autocomplete="current-password" />
            </div>
            <asp:Button ID="btnEntrar" runat="server" Text="Entrar" CssClass="btn btn-oco btn-lg w-100" OnClick="btnEntrar_Click" />
            <div class="text-center mt-3"><a class="small" href="Default.aspx">← Volver a la consulta</a></div>
        </div>
    </form>
</div>
</body>
</html>
