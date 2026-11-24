/* Panel de control · Consulta Ciudadana Ocosingo */
(function () {
    'use strict';

    var SERVICIO = 'Datos.ashx';
    var form = document.getElementById('formFiltros');
    var fDesde = document.getElementById('fDesde'), fHasta = document.getElementById('fHasta');
    var fTipo = document.getElementById('fTipo'), fPlan = document.getElementById('fPlan');
    var fBuscar = document.getElementById('fBuscar');
    var zona = document.getElementById('zonaPanel');
    var listado = [];

    function el(tag, attrs, texto) {
        var e = document.createElement(tag);
        if (attrs) for (var k in attrs) if (attrs.hasOwnProperty(k)) e.setAttribute(k, attrs[k]);
        if (texto != null) e.textContent = texto;
        return e;
    }
    function fecha(iso, conHora) {
        if (!iso) return '';
        var d = new Date(iso);
        var s = d.toLocaleDateString('es-MX', { day: '2-digit', month: 'short', year: 'numeric' });
        return conHora ? s + ' ' + d.toLocaleTimeString('es-MX', { hour: '2-digit', minute: '2-digit' }) : s;
    }
    function isoLocal(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }

    /* ---------- Filtros ---------- */
    function llenarPlanes() {
        var tipo = fTipo.value, actual = fPlan.value;
        fPlan.length = 1;
        (window.PLANES || []).forEach(function (p) {
            if (!tipo || String(p.idtipo) === tipo) fPlan.appendChild(el('option', { value: p.idplanes }, p.nombre));
        });
        fPlan.value = actual;
        if (fPlan.value !== actual) fPlan.value = '';
    }

    function consulta(extra) {
        var q = new URLSearchParams();
        ['desde', 'hasta', 'tipo', 'plan'].forEach(function (n) { if (form.elements[n].value) q.append(n, form.elements[n].value); });
        if (extra) for (var k in extra) if (extra[k]) q.append(k, extra[k]);
        return q.toString();
    }

    function pedir(accion, extra) {
        return fetch(SERVICIO + '?accion=' + accion + '&' + consulta(extra), { headers: { 'X-Requested-With': 'fetch' }, cache: 'no-store' })
            .then(function (r) {
                var tipo = r.headers.get('content-type') || '';
                if (r.status === 401 || r.redirected || tipo.indexOf('json') < 0) { location.href = '../Login.aspx?ReturnUrl=' + encodeURIComponent(location.pathname); throw new Error('sesion'); }
                return r.json();
            });
    }

    function actualizarEnlaces() {
        var q = consulta();
        document.getElementById('btnCsv').href = SERVICIO + '?accion=csv' + (q ? '&' + q : '');
        document.getElementById('btnReporte').href = 'Reporte.aspx' + (q ? '?' + q : '');
        var t = [];
        if (fDesde.value || fHasta.value) t.push((fDesde.value ? fecha(fDesde.value + 'T00:00') : 'inicio') + ' – ' + (fHasta.value ? fecha(fHasta.value + 'T00:00') : 'hoy'));
        else t.push('Todo el periodo');
        if (fPlan.value) t.push(fPlan.options[fPlan.selectedIndex].text);
        else if (fTipo.value) t.push(fTipo.options[fTipo.selectedIndex].text);
        document.getElementById('textoPeriodo').textContent = t.join(' · ');
    }

    function cargar() {
        actualizarEnlaces();
        zona.classList.add('cargando');
        pedir('resumen').then(pintarResumen).catch(errorCarga).then(function () { zona.classList.remove('cargando'); });
        cargarListado();
    }

    function errorCarga(e) {
        if (e && e.message === 'sesion') return;
        var k = document.getElementById('kpis');
        k.textContent = '';
        k.appendChild(el('div', { 'class': 'alert alert-danger mb-0' }, 'No se pudo cargar la información. Revisa la conexión con la base de datos.'));
    }

    /* ---------- Indicadores ---------- */
    function kpi(cont, etiqueta, valor, detalle, medidor, destacado) {
        var d = el('div', { 'class': 'kpi' + (destacado ? ' kpi--destacado' : '') });
        d.appendChild(el('div', { 'class': 'kpi__etiqueta' }, etiqueta));
        d.appendChild(el('div', { 'class': 'kpi__valor' }, valor));
        if (medidor != null) {
            var m = el('div', { 'class': 'medidor', role: 'presentation' }), s = el('span');
            s.style.width = Math.max(0, Math.min(100, medidor)) + '%';
            m.appendChild(s); d.appendChild(m);
        }
        if (detalle) d.appendChild(el('div', { 'class': 'kpi__detalle mt-1' }, detalle));
        cont.appendChild(d);
    }

    function pintarResumen(r) {
        var k = r.kpi || {}, t = k.total || 0;
        var c = document.getElementById('kpis');
        c.textContent = '';
        kpi(c, 'Encuestas recibidas', G.num(t), t ? fecha(k.primera) + ' – ' + fecha(k.ultima) : 'Sin registros en el periodo', null, true);
        kpi(c, 'Últimos 30 días', G.num(k.ultimos30), G.pct(k.ultimos30, t) + ' del total');
        kpi(c, 'Conocían el plan o programa', G.pct(k.conocen, t), G.num(k.conocen) + ' personas', G.pctNum(k.conocen, t));
        kpi(c, 'Han recibido apoyo', G.pct(k.conapoyo, t), G.num(k.conapoyo) + ' personas', G.pctNum(k.conapoyo, t));
        kpi(c, 'Índice de confianza', t ? String(k.indiceconfianza) : '—', 'Escala 0 a 100 (Mucha = 100, Poca = 50, Nada = 0)', t ? k.indiceconfianza : 0);
        kpi(c, 'Con propuesta escrita', G.pct(k.conpropuesta, t), G.num(k.conarchivo) + ' con archivo adjunto', G.pctNum(k.conpropuesta, t));

        // Participación por mes (rellena con ceros los meses sin encuestas)
        var meses = [];
        if (r.meses.length) {
            var a = r.meses[0].anio, m = r.meses[0].mes, fin = r.meses[r.meses.length - 1], mapa = {};
            r.meses.forEach(function (x) { mapa[x.anio + '-' + x.mes] = x.total; });
            while (a < fin.anio || (a === fin.anio && m <= fin.mes)) {
                meses.push({ etiqueta: G.mes(a, m), valor: mapa[a + '-' + m] || 0 });
                m++; if (m > 12) { m = 1; a++; }
            }
        }
        G.linea('gMeses', meses);

        G.barrasH('gConfianza', r.confianza.map(function (x, i) {
            return { etiqueta: x.nombre, valor: G.pctNum(x.total, t), color: G.COLOR.secuencial[i], detalle: G.num(x.total) + ' respuestas' };
        }), { sufijo: '%', maxEje: 100, formato: function (v) { return Math.round(v) + '%'; } });

        G.barrasH('gPlanes', r.planes.map(function (p) {
            return { etiqueta: p.nombrecorto, valor: p.total, color: p.idtipo === 1 ? G.COLOR.plan : G.COLOR.programa, detalle: (p.idtipo === 1 ? 'Plan' : 'Programa') + ' · ' + G.num(p.total) + ' encuestas' };
        }));

        var menciones = r.temas.reduce(function (s, x) { return s + x.total; }, 0);
        G.barrasH('gTemas', r.temas.map(function (x) {
            return { etiqueta: x.nombre, valor: G.pctNum(x.total, menciones), detalle: G.num(x.total) + ' menciones · lo eligió el ' + G.pct(x.total, t) + ' de los participantes' };
        }), { sufijo: '%', formato: function (v) { return Math.round(v) + '%'; } });

        G.columnas('gEdades', r.edades.map(function (x) { return { etiqueta: x.nombre, valor: x.total, detalle: G.num(x.total) + ' (' + G.pct(x.total, t) + ')' }; }));
        G.barrasH('gGenero', r.genero.map(function (x) { return { etiqueta: x.nombre, valor: x.total, detalle: G.num(x.total) + ' (' + G.pct(x.total, t) + ')' }; }));

        var ocup = r.ocupaciones.filter(function (x) { return x.total > 0; });
        var top = ocup.slice(0, 6), resto = ocup.slice(6).reduce(function (s, x) { return s + x.total; }, 0);
        if (resto) top.push({ nombre: 'Otras', total: resto });
        G.barrasH('gOcupacion', top.map(function (x) { return { etiqueta: x.nombre.replace(/\s*\(.*\)$/, ''), valor: x.total, detalle: G.num(x.total) + ' (' + G.pct(x.total, t) + ')' }; }), { maxEtiqueta: 14 });

        // Localidades
        var tl = document.getElementById('tLocalidades');
        tl.textContent = '';
        if (!r.localidades.length) tl.appendChild(el('div', { 'class': 'vacio' }, 'Sin localidades capturadas.'));
        else {
            var tabla = el('table', { 'class': 'table table-sm tabla-datos mb-0' }), tb = el('tbody');
            r.localidades.forEach(function (x) { var tr = el('tr'); tr.appendChild(el('td', null, x.nombre)); tr.appendChild(el('td', null, G.num(x.total))); tb.appendChild(tr); });
            tabla.appendChild(tb); tl.appendChild(tabla);
        }

        // Tabla por plan
        var tp = document.getElementById('tPlanes');
        tp.textContent = '';
        var th = el('thead'), trh = el('tr');
        ['Plan o programa', 'Tipo', 'Encuestas', '% Conocían', '% Recibieron apoyo', 'Índice de confianza'].forEach(function (h, i) { trh.appendChild(el('th', i > 1 ? { 'class': 'text-end' } : null, h)); });
        th.appendChild(trh); tp.appendChild(th);
        var tbody = el('tbody');
        r.planes.forEach(function (p) {
            var tr = el('tr');
            tr.appendChild(el('td', null, p.nombre));
            tr.appendChild(el('td', null, p.idtipo === 1 ? 'Plan' : 'Programa'));
            tr.appendChild(el('td', { 'class': 'text-end' }, G.num(p.total)));
            tr.appendChild(el('td', { 'class': 'text-end' }, G.pct(p.conocen, p.total)));
            tr.appendChild(el('td', { 'class': 'text-end' }, G.pct(p.conapoyo, p.total)));
            tr.appendChild(el('td', { 'class': 'text-end' }, p.total ? String(p.indiceconfianza) : '—'));
            tbody.appendChild(tr);
        });
        tp.appendChild(tbody);
    }

    /* ---------- Listado ---------- */
    var POR_PAGINA = 25, pagina = 0;
    function cargarListado() {
        pedir('listado', { buscar: fBuscar.value.trim() }).then(function (filas) {
            listado = filas; pagina = 0;
            document.getElementById('textoListado').textContent = filas.length >= 300 ? 'Las 300 más recientes (exporta a Excel para ver todas)' : filas.length + ' encuestas';
            pintarPagina();
        }).catch(function () { });
    }

    function pintarPagina() {
        var tb = document.getElementById('tEncuestas');
        tb.textContent = '';
        if (!listado.length) {
            var tr0 = el('tr'); tr0.appendChild(el('td', { colspan: 6, 'class': 'vacio' }, 'No hay encuestas con estos filtros.'));
            tb.appendChild(tr0);
        }
        var ini = pagina * POR_PAGINA;
        listado.slice(ini, ini + POR_PAGINA).forEach(function (f, j) {
            var i = ini + j;
            var tr = el('tr', { 'data-i': i, tabindex: 0 });
            tr.appendChild(el('td', { 'class': 'fw-semibold text-nowrap' }, f.folio));
            tr.appendChild(el('td', { 'class': 'text-nowrap' }, fecha(f.fechaencuesta)));
            tr.appendChild(el('td', null, f.planprograma));
            var p = el('td'); p.appendChild(el('div', { 'class': 'recorte' }, f.problematica)); tr.appendChild(p);
            var q = el('td'); q.appendChild(el('div', { 'class': 'recorte' }, f.propuesta || '—'));
            if (f.archivo) q.appendChild(el('span', { 'class': 'badge text-bg-light border mt-1' }, 'Archivo adjunto'));
            tr.appendChild(q);
            tr.appendChild(el('td', null, f.confianza));
            tb.appendChild(tr);
        });
        var paginas = Math.max(1, Math.ceil(listado.length / POR_PAGINA));
        document.getElementById('infoPagina').textContent = listado.length ? (ini + 1) + '–' + Math.min(ini + POR_PAGINA, listado.length) + ' de ' + listado.length : '';
        document.getElementById('pagAnterior').disabled = pagina === 0;
        document.getElementById('pagSiguiente').disabled = pagina >= paginas - 1;
    }
    document.getElementById('pagAnterior').addEventListener('click', function () { if (pagina > 0) { pagina--; pintarPagina(); } });
    document.getElementById('pagSiguiente').addEventListener('click', function () { pagina++; pintarPagina(); });

    function verDetalle(f) {
        document.getElementById('tituloDetalle').textContent = 'Encuesta ' + f.folio;
        var dl = document.getElementById('cuerpoDetalle');
        dl.textContent = '';
        function par(t, v, ancho) {
            var dt = el('dt', null, t);
            var dd = el('dd');
            if (v instanceof Node) dd.appendChild(v); else dd.textContent = v || '—';
            var cont = el('div', { 'class': ancho ? 'col-12' : 'col-md-6' });
            cont.appendChild(dt); cont.appendChild(dd); dl.appendChild(cont);
        }
        par('Fecha', fecha(f.fechaencuesta, true));
        par('Plan o programa', f.tipo === 'PLAN' ? f.planprograma + ' (plan)' : f.planprograma + ' (programa)');
        par('Nombre', f.nombre || 'Anónimo');
        par('Género / edad', f.genero + ' · ' + f.edad);
        par('Ocupación', f.ocupacion);
        par('Vive en Ocosingo', f.residente === 'SI' ? 'Sí' + (f.zona ? ' · ' + f.zona : '') : 'No');
        par('Localidad', f.localidad);
        par('Conocía el plan/programa · Recibió apoyo', (f.conoce === 'SI' ? 'Sí' : 'No') + ' · ' + (f.apoyo === 'SI' ? 'Sí' : 'No'));
        par('Confianza en el gobierno', f.confianza);
        par('Temas prioritarios', f.temas, true);
        par('Problemática', f.problematica, true);
        par('Propuesta', f.propuesta, true);
        if (f.archivo) {
            var a = el('a', { href: SERVICIO + '?accion=adjunto&id=' + f.idpregunta, 'class': 'btn btn-sm btn-contorno' }, 'Descargar: ' + (f.archivooriginal || f.archivo));
            par('Archivo adjunto', a, true);
        }
        bootstrap.Modal.getOrCreateInstance(document.getElementById('modalDetalle')).show();
    }

    document.getElementById('tEncuestas').addEventListener('click', function (e) {
        var tr = e.target.closest('tr[data-i]');
        if (tr) verDetalle(listado[+tr.getAttribute('data-i')]);
    });
    document.getElementById('tEncuestas').addEventListener('keydown', function (e) {
        var tr = e.target.closest('tr[data-i]');
        if (tr && (e.key === 'Enter' || e.key === ' ')) { e.preventDefault(); verDetalle(listado[+tr.getAttribute('data-i')]); }
    });

    /* ---------- Eventos ---------- */
    var rangos = Array.prototype.slice.call(document.querySelectorAll('[data-rango]'));
    function marcarRango(b) { rangos.forEach(function (x) { x.classList.toggle('active', x === b); }); }
    rangos.forEach(function (b) {
        b.addEventListener('click', function () {
            var hoy = new Date(), r = b.getAttribute('data-rango');
            if (r === 'todo') { fDesde.value = ''; fHasta.value = ''; }
            else if (r === 'anio') { fDesde.value = hoy.getFullYear() + '-01-01'; fHasta.value = isoLocal(hoy); }
            else { var d = new Date(); d.setDate(d.getDate() - 29); fDesde.value = isoLocal(d); fHasta.value = isoLocal(hoy); }
            marcarRango(b);
            cargar();
        });
    });
    [fDesde, fHasta].forEach(function (i) { i.addEventListener('change', function () { marcarRango(null); cargar(); }); });
    fTipo.addEventListener('change', function () { llenarPlanes(); cargar(); });
    fPlan.addEventListener('change', cargar);
    form.addEventListener('submit', function (e) { e.preventDefault(); });

    var espera;
    fBuscar.addEventListener('input', function () { clearTimeout(espera); espera = setTimeout(cargarListado, 350); });

    llenarPlanes();
    cargar();
})();
