/* Reporte imprimible · Consulta Ciudadana Ocosingo */
(function () {
    'use strict';
    var r = window.DATOS_REPORTE;
    if (!r) return;
    var k = r.kpi || {}, t = k.total || 0;

    function el(tag, attrs, texto) {
        var e = document.createElement(tag);
        if (attrs) for (var a in attrs) if (attrs.hasOwnProperty(a)) e.setAttribute(a, attrs[a]);
        if (texto != null) e.textContent = texto;
        return e;
    }
    function fecha(iso) { return iso ? new Date(iso).toLocaleDateString('es-MX', { day: '2-digit', month: '2-digit', year: 'numeric' }) : ''; }
    function tabla(id, encabezados, filas) {
        var tb = document.getElementById(id);
        var th = el('thead'), tr = el('tr');
        encabezados.forEach(function (h, i) { tr.appendChild(el('th', i > 0 ? { 'class': 'num' } : null, h)); });
        th.appendChild(tr); tb.appendChild(th);
        var body = el('tbody');
        filas.forEach(function (f) {
            var fila = el('tr');
            f.forEach(function (v, i) { fila.appendChild(el('td', i > 0 ? { 'class': 'num' } : null, v)); });
            body.appendChild(fila);
        });
        if (!filas.length) { var v = el('tr'); v.appendChild(el('td', { colspan: encabezados.length }, 'Sin datos.')); body.appendChild(v); }
        tb.appendChild(body);
    }
    function conPct(lista) { return lista.map(function (x) { return [x.nombre, G.num(x.total), G.pct(x.total, t)]; }); }

    /* ---------- 1. Resumen ejecutivo (redactado a partir de los datos) ---------- */
    var res = document.getElementById('resumen');
    function punto(html) { var li = el('li'); li.textContent = html; res.appendChild(li); }
    if (!t) {
        punto('No se registraron encuestas con los criterios seleccionados.');
    } else {
        punto('Se recibieron ' + G.num(t) + ' encuestas entre el ' + fecha(k.primera) + ' y el ' + fecha(k.ultima) + '.');
        var plan = r.planes.filter(function (p) { return p.total > 0; })[0];
        if (plan) punto('El ' + (plan.idtipo === 1 ? 'plan' : 'programa') + ' con mayor participación fue «' + plan.nombre + '», con ' + G.num(plan.total) + ' encuestas (' + G.pct(plan.total, t) + ' del total).');
        var temas = r.temas.slice().sort(function (a, b) { return b.total - a.total; });
        if (temas[0] && temas[0].total) punto('El tema que más personas señalaron como prioritario fue «' + temas[0].nombre + '», elegido por el ' + G.pct(temas[0].total, t) + ' de los participantes' + (temas[1] && temas[1].total ? ', seguido de «' + temas[1].nombre + '» (' + G.pct(temas[1].total, t) + ').' : '.'));
        punto(G.pct(k.conocen, t) + ' de las personas participantes conocía el plan o programa sobre el que opinó y ' + G.pct(k.conapoyo, t) + ' ha recibido algún apoyo del gobierno municipal.');
        var conf = r.confianza.slice().sort(function (a, b) { return b.total - a.total; })[0];
        punto('El índice de confianza en el gobierno municipal es de ' + k.indiceconfianza + ' puntos de 100; la respuesta más frecuente fue «' + conf.nombre + '» (' + G.pct(conf.total, t) + ').');
        punto(G.pct(k.conpropuesta, t) + ' de las encuestas incluye una propuesta escrita' + (k.conarchivo ? ' y ' + G.num(k.conarchivo) + ' adjuntan un documento.' : '.'));
    }

    /* ---------- 2. Indicadores ---------- */
    var kp = document.getElementById('kpis');
    [[G.num(t), 'Encuestas recibidas'],
     [G.num(k.ultimos30), 'En los últimos 30 días del periodo'],
     [G.pct(k.conocen, t), 'Conocían el plan o programa'],
     [G.pct(k.conapoyo, t), 'Han recibido apoyo'],
     [t ? k.indiceconfianza + ' / 100' : '—', 'Índice de confianza'],
     [G.pct(k.residentes, t), 'Viven en Ocosingo']].forEach(function (x) {
        var d = el('div', { 'class': 'rep-kpi' });
        d.appendChild(el('b', null, x[0])); d.appendChild(el('span', null, x[1]));
        kp.appendChild(d);
    });

    /* ---------- 3–5. Gráficas ---------- */
    var meses = [];
    if (r.meses.length) {
        var a = r.meses[0].anio, m = r.meses[0].mes, fin = r.meses[r.meses.length - 1], mapa = {};
        r.meses.forEach(function (x) { mapa[x.anio + '-' + x.mes] = x.total; });
        while (a < fin.anio || (a === fin.anio && m <= fin.mes)) { meses.push({ etiqueta: G.mes(a, m), valor: mapa[a + '-' + m] || 0 }); m++; if (m > 12) { m = 1; a++; } }
    }
    G.columnas('gMeses', meses, { maxEtiqueta: 3 });
    G.barrasH('gConfianza', r.confianza.map(function (x, i) { return { etiqueta: x.nombre, valor: G.pctNum(x.total, t), color: G.COLOR.secuencial[i] }; }),
        { sufijo: '%', maxEje: 100, formato: function (v) { return Math.round(v) + '%'; } });
    G.barrasH('gPlanes', r.planes.map(function (p) { return { etiqueta: p.nombrecorto, valor: p.total, color: p.idtipo === 1 ? G.COLOR.plan : G.COLOR.programa }; }), { maxEtiqueta: 40 });
    var menciones = r.temas.reduce(function (s, x) { return s + x.total; }, 0);
    G.barrasH('gTemas', r.temas.map(function (x) { return { etiqueta: x.nombre, valor: G.pctNum(x.total, menciones) }; }),
        { maxEtiqueta: 26, sufijo: '%', formato: function (v) { return Math.round(v) + '%'; } });

    tabla('tPlanes', ['Plan o programa', 'Encuestas', '% del total', '% Conocían', '% Recibieron apoyo', 'Índice de confianza'],
        r.planes.map(function (p) {
            return [p.nombre + (p.idtipo === 1 ? ' (plan)' : ' (programa)'), G.num(p.total), G.pct(p.total, t), G.pct(p.conocen, p.total), G.pct(p.conapoyo, p.total), p.total ? String(p.indiceconfianza) : '—'];
        }));
    var filasTemas = r.temas.map(function (x) { return [x.nombre, G.num(x.total), G.pct(x.total, menciones)]; });
    filasTemas.push(['Total de menciones', G.num(menciones), menciones ? '100%' : '—']);
    tabla('tTemas', ['Tema', 'Menciones', '% de menciones'], filasTemas);
    var nota = el('p', { 'class': 'nota mt-1' }, 'Cada participante podía elegir varios temas: ' + G.num(menciones) + ' menciones en ' + G.num(t) + ' encuestas (' + (t ? (menciones / t).toFixed(1) : '0') + ' temas por persona en promedio).');
    document.getElementById('tTemas').after(nota);

    /* ---------- 6. Perfil ---------- */
    tabla('tEdades', ['Rango', 'Personas', '%'], conPct(r.edades));
    tabla('tGenero', ['Género', 'Personas', '%'], conPct(r.genero));
    tabla('tOcupacion', ['Ocupación', 'Personas', '%'], conPct(r.ocupaciones.filter(function (x) { return x.total > 0; })));
    tabla('tLocalidades', ['Localidad', 'Encuestas'], r.localidades.map(function (x) { return [x.nombre, G.num(x.total)]; }));
    var sinZona = k.residentes - k.cabecera - k.rurales;
    var filasRes = [
        ['Vive en Ocosingo · cabecera municipal', G.num(k.cabecera), G.pct(k.cabecera, t)],
        ['Vive en Ocosingo · localidad rural', G.num(k.rurales), G.pct(k.rurales, t)]
    ];
    if (sinZona > 0) filasRes.push(['Vive en Ocosingo · sin especificar', G.num(sinZona), G.pct(sinZona, t)]);
    filasRes.push(['No vive en Ocosingo', G.num(t - k.residentes), G.pct(t - k.residentes, t)]);
    tabla('tResidencia', ['Respuesta', 'Personas', '%'], filasRes);

    /* ---------- Anexo ---------- */
    var anexo = r.anexo || [];
    document.getElementById('notaAnexo').textContent = anexo.length >= r.maxAnexo
        ? 'Se muestran las ' + r.maxAnexo + ' encuestas más recientes del periodo. El listado completo puede exportarse a Excel desde el panel.'
        : 'Se incluyen las ' + anexo.length + ' encuestas del periodo, de la más reciente a la más antigua.';
    var tb = document.getElementById('tAnexo');
    anexo.forEach(function (f) {
        var tr = el('tr');
        var c1 = el('td'); c1.appendChild(el('b', null, f.folio)); c1.appendChild(el('br')); c1.appendChild(document.createTextNode(fecha(f.fechaencuesta)));
        tr.appendChild(c1);
        tr.appendChild(el('td', null, f.planprograma));
        tr.appendChild(el('td', null, f.problematica));
        tr.appendChild(el('td', null, f.propuesta || '—'));
        tb.appendChild(tr);
    });
})();
