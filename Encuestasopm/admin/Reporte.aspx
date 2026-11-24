<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reporte.aspx.cs" Inherits="Encuestasopm.Admin.Reporte" %>
<!DOCTYPE html>
<html lang="es-MX">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="robots" content="noindex" />
    <title>Reporte de resultados · Consulta Ciudadana Ocosingo</title>
    <link rel="icon" type="image/png" href="../img/emblema-ocosingo.png" />
    <link rel="stylesheet" href="<%= Encuestasopm.Recursos.Url(ResolveUrl("~/lib/bootstrap.min.css")) %>" />
    <link rel="stylesheet" href="<%= Encuestasopm.Recursos.Url(ResolveUrl("~/css/app.css")) %>" />
    <link rel="stylesheet" href="<%= Encuestasopm.Recursos.Url(ResolveUrl("~/css/reporte.css")) %>" />
</head>
<body>
    <div class="barra-acciones no-imprimir">
        <span>Vista previa del reporte · tamaño carta</span>
        <div class="d-flex gap-2">
            <a class="btn btn-sm btn-contorno" href="Panel.aspx">Volver al panel</a>
            <button type="button" class="btn btn-sm btn-guinda" onclick="window.print()">Imprimir / Guardar PDF</button>
        </div>
    </div>

    <div class="hoja">
        <header class="rep-cabecera">
            <img src="../img/logo-ocosingo-sm.png" alt="H. Ayuntamiento de Ocosingo 2024–2027" height="92" />
            <div class="rep-titulo">
                <div class="rep-titulo__inst">H. Ayuntamiento de Ocosingo, Chiapas · 2024–2027</div>
                <h1>Reporte de resultados</h1>
                <div class="rep-titulo__sub">Consulta Ciudadana Participativa sobre planes y programas municipales</div>
            </div>
            <div style="width:73px" aria-hidden="true"></div>
        </header>
        <div class="franja" aria-hidden="true"><span></span><span></span><span></span><span></span><span></span><span></span><span></span><span></span></div>

        <table class="rep-ficha">
            <tr><th>Periodo</th><td><%= Periodo %></td><th>Alcance</th><td><%= Alcance %></td></tr>
            <tr><th>Fecha de emisión</th><td><%= DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy, HH:mm") %> h</td><th>Elaboró</th><td><%= Elaboro %></td></tr>
        </table>

        <% if (HayError) { %>
            <div class="alert alert-danger">No se pudo consultar la información. Revisa la conexión con la base de datos.</div>
        <% } %>

        <section>
            <h2>1. Resumen ejecutivo</h2>
            <ul class="rep-resumen" id="resumen"></ul>
        </section>

        <section>
            <h2>2. Indicadores principales</h2>
            <div class="rep-kpis" id="kpis"></div>
        </section>

        <section class="evitar-corte">
            <h2>3. Participación</h2>
            <div class="rep-fila">
                <figure class="rep-figura ancho-2">
                    <figcaption>Encuestas recibidas por mes</figcaption>
                    <div class="lienzo-rep"><canvas id="gMeses"></canvas></div>
                </figure>
                <figure class="rep-figura">
                    <figcaption>Confianza en el gobierno municipal (% de respuestas)</figcaption>
                    <div class="lienzo-rep"><canvas id="gConfianza"></canvas></div>
                </figure>
            </div>
        </section>

        <section class="evitar-corte">
            <h2>4. Planes y programas</h2>
            <div class="leyenda-rep"><span><i style="background:#2f7d32"></i>Plan</span><span><i style="background:#9c1c2c"></i>Programa</span></div>
            <div class="lienzo-rep alto"><canvas id="gPlanes"></canvas></div>
            <table class="tabla-rep mt-3" id="tPlanes"></table>
        </section>

        <section class="evitar-corte">
            <h2>5. Temas que la ciudadanía considera prioritarios</h2>
            <div class="rep-fila">
                <figure class="rep-figura ancho-2">
                    <figcaption>Distribución del total de menciones (cada participante podía elegir varios temas)</figcaption>
                    <div class="lienzo-rep medio"><canvas id="gTemas"></canvas></div>
                </figure>
                <div class="rep-figura"><table class="tabla-rep" id="tTemas"></table></div>
            </div>
        </section>

        <section class="evitar-corte">
            <h2>6. Perfil de las personas participantes</h2>
            <div class="rep-fila tres">
                <div><h3>Rango de edad</h3><table class="tabla-rep" id="tEdades"></table></div>
                <div><h3>Género</h3><table class="tabla-rep" id="tGenero"></table></div>
                <div><h3>Ocupación</h3><table class="tabla-rep" id="tOcupacion"></table></div>
            </div>
            <div class="rep-fila mt-2">
                <div><h3>Localidades con mayor participación</h3><table class="tabla-rep" id="tLocalidades"></table></div>
                <div><h3>Residencia</h3><table class="tabla-rep" id="tResidencia"></table></div>
            </div>
        </section>

        <section class="salto-antes">
            <h2>Anexo. Problemáticas y propuestas ciudadanas</h2>
            <p class="nota" id="notaAnexo"></p>
            <table class="tabla-rep anexo">
                <thead><tr><th style="width:13%">Folio / fecha</th><th style="width:17%">Plan o programa</th><th>Problemática</th><th>Propuesta</th></tr></thead>
                <tbody id="tAnexo"></tbody>
            </table>
        </section>

        <footer class="rep-pie">
            Información generada por el sistema de Consulta Ciudadana Participativa del H. Ayuntamiento de Ocosingo.
            Los resultados reflejan únicamente las opiniones de las personas que participaron voluntariamente; no constituyen una muestra estadística representativa de la población.
        </footer>
    </div>

    <script>window.SIN_ANIMACION = true; window.DATOS_REPORTE = <%= DatosJson %>;</script>
    <script src="<%= Encuestasopm.Recursos.Url(ResolveUrl("~/lib/chart.umd.js")) %>"></script>
    <script src="<%= Encuestasopm.Recursos.Url(ResolveUrl("~/js/graficas.js")) %>"></script>
    <script src="<%= Encuestasopm.Recursos.Url(ResolveUrl("~/js/reporte.js")) %>"></script>
</body>
</html>
