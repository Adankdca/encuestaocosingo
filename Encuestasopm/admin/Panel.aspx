<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Panel.aspx.cs" Inherits="Encuestasopm.Admin.Panel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="d-flex flex-wrap justify-content-between align-items-end gap-2 mb-3">
    <div>
        <h1 class="h3 mb-0">Resumen estadístico</h1>
        <div class="text-secundario small" id="textoPeriodo">Todas las encuestas</div>
    </div>
    <div class="d-flex gap-2">
        <a class="btn btn-contorno" id="btnCsv" href="Datos.ashx?accion=csv">Exportar a Excel (CSV)</a>
        <a class="btn btn-guinda" id="btnReporte" href="Reporte.aspx" target="_blank" rel="noopener">Generar reporte</a>
    </div>
</div>

<!-- Filtros -->
<form class="filtros mb-4" id="formFiltros">
    <div>
        <label class="form-label" for="fDesde">Desde</label>
        <input type="date" class="form-control form-control-sm" id="fDesde" name="desde" />
    </div>
    <div>
        <label class="form-label" for="fHasta">Hasta</label>
        <input type="date" class="form-control form-control-sm" id="fHasta" name="hasta" />
    </div>
    <div>
        <label class="form-label" for="fTipo">Tipo</label>
        <select class="form-select form-select-sm" id="fTipo" name="tipo">
            <option value="">Planes y programas</option>
            <option value="1">Solo planes</option>
            <option value="2">Solo programas</option>
        </select>
    </div>
    <div>
        <label class="form-label" for="fPlan">Plan o programa</label>
        <select class="form-select form-select-sm" id="fPlan" name="plan"><option value="">Todos</option></select>
    </div>
    <div class="rangos btn-group btn-group-sm" role="group" aria-label="Periodos rápidos">
        <button type="button" class="btn btn-contorno" data-rango="30">Últimos 30 días</button>
        <button type="button" class="btn btn-contorno" data-rango="anio">Este año</button>
        <button type="button" class="btn btn-contorno active" data-rango="todo">Todo</button>
    </div>
</form>

<div id="zonaPanel">
    <!-- Indicadores -->
    <section class="kpis mb-4" id="kpis" aria-label="Indicadores principales"></section>

    <div class="row g-3 mb-3">
        <div class="col-lg-8">
            <div class="tarjeta">
                <div class="tarjeta__cabeza"><div><h2>Participación por mes</h2><div class="tarjeta__sub">Encuestas recibidas</div></div></div>
                <div class="lienzo"><canvas id="gMeses" role="img" aria-label="Encuestas por mes"></canvas></div>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="tarjeta">
                <div class="tarjeta__cabeza"><div><h2>Confianza en el gobierno</h2><div class="tarjeta__sub">% de respuestas</div></div></div>
                <div class="lienzo"><canvas id="gConfianza" role="img" aria-label="Confianza en el gobierno"></canvas></div>
            </div>
        </div>
    </div>

    <div class="row g-3 mb-3">
        <div class="col-lg-7">
            <div class="tarjeta">
                <div class="tarjeta__cabeza"><div><h2>Participación por plan o programa</h2><div class="tarjeta__sub">Número de encuestas</div></div></div>
                <div class="leyenda"><span><i style="background:#2f7d32"></i>Plan</span><span><i style="background:#9c1c2c"></i>Programa</span></div>
                <div class="lienzo" style="min-height:360px"><canvas id="gPlanes" role="img" aria-label="Encuestas por plan o programa"></canvas></div>
            </div>
        </div>
        <div class="col-lg-5">
            <div class="tarjeta">
                <div class="tarjeta__cabeza"><div><h2>Temas que el gobierno debe atender</h2><div class="tarjeta__sub">% del total de menciones (cada persona podía elegir varios)</div></div></div>
                <div class="lienzo" style="min-height:360px"><canvas id="gTemas" role="img" aria-label="Temas prioritarios"></canvas></div>
            </div>
        </div>
    </div>

    <div class="row g-3 mb-4">
        <div class="col-md-6 col-xl-3">
            <div class="tarjeta">
                <div class="tarjeta__cabeza"><h2>Rango de edad</h2></div>
                <div class="lienzo"><canvas id="gEdades" role="img" aria-label="Participantes por rango de edad"></canvas></div>
            </div>
        </div>
        <div class="col-md-6 col-xl-3">
            <div class="tarjeta">
                <div class="tarjeta__cabeza"><h2>Género</h2></div>
                <div class="lienzo"><canvas id="gGenero" role="img" aria-label="Participantes por género"></canvas></div>
            </div>
        </div>
        <div class="col-md-6 col-xl-3">
            <div class="tarjeta">
                <div class="tarjeta__cabeza"><h2>Ocupación</h2><span class="tarjeta__sub">Top 6</span></div>
                <div class="lienzo"><canvas id="gOcupacion" role="img" aria-label="Participantes por ocupación"></canvas></div>
            </div>
        </div>
        <div class="col-md-6 col-xl-3">
            <div class="tarjeta">
                <div class="tarjeta__cabeza"><h2>Localidades</h2><span class="tarjeta__sub">Más participación</span></div>
                <div id="tLocalidades"></div>
            </div>
        </div>
    </div>

    <!-- Detalle por plan -->
    <div class="tarjeta mb-4">
        <div class="tarjeta__cabeza"><div><h2>Indicadores por plan o programa</h2><div class="tarjeta__sub">Conocimiento, apoyo recibido e índice de confianza (0 a 100)</div></div></div>
        <div class="table-responsive"><table class="table table-sm align-middle mb-0" id="tPlanes"></table></div>
    </div>

    <!-- Listado -->
    <div class="tarjeta">
        <div class="tarjeta__cabeza flex-wrap">
            <div><h2>Problemáticas y propuestas ciudadanas</h2><div class="tarjeta__sub" id="textoListado">Últimas 300 encuestas</div></div>
            <input type="search" class="form-control form-control-sm" id="fBuscar" placeholder="Buscar folio, texto o localidad…" style="max-width:300px" />
        </div>
        <div class="table-responsive">
            <table class="table table-hover tabla-encuestas mb-0">
                <thead><tr><th>Folio</th><th>Fecha</th><th>Plan o programa</th><th>Problemática</th><th>Propuesta</th><th>Confianza</th></tr></thead>
                <tbody id="tEncuestas"></tbody>
            </table>
        </div>
        <div class="d-flex justify-content-end align-items-center gap-2 mt-2">
            <span class="small text-secundario" id="infoPagina"></span>
            <button type="button" class="btn btn-sm btn-contorno" id="pagAnterior">Anterior</button>
            <button type="button" class="btn btn-sm btn-contorno" id="pagSiguiente">Siguiente</button>
        </div>
    </div>
</div>

<!-- Detalle de una encuesta -->
<div class="modal fade" id="modalDetalle" tabindex="-1" aria-labelledby="tituloDetalle" aria-hidden="true">
    <div class="modal-dialog modal-lg modal-dialog-scrollable">
        <div class="modal-content">
            <div class="modal-header">
                <h2 class="modal-title h5" id="tituloDetalle">Encuesta</h2>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
            </div>
            <div class="modal-body"><dl class="detalle row" id="cuerpoDetalle"></dl></div>
        </div>
    </div>
</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script>window.PLANES = <%= PlanesJson %>;</script>
    <script src="<%= Encuestasopm.Recursos.Url(ResolveUrl("~/lib/chart.umd.js")) %>"></script>
    <script src="<%= Encuestasopm.Recursos.Url(ResolveUrl("~/js/graficas.js")) %>"></script>
    <script src="<%= Encuestasopm.Recursos.Url(ResolveUrl("~/js/panel.js")) %>"></script>
</asp:Content>
