/* Utilidades de gráficas (Chart.js 4) compartidas por el panel y el reporte.
   Estilo: marcas delgadas, extremos redondeados, cuadrícula tenue, valor al final de cada barra. */
(function (global) {
    'use strict';

    var COLOR = {
        serie: '#2f7d32',      // verde (serie única)
        plan: '#2f7d32',
        programa: '#9c1c2c',   // guinda Gobierno de Chiapas
        secuencial: ['#1f5f2a', '#5f9e3a', '#b6d3a3'],  // de más a menos (Mucha, Poca, Nada)
        tinta: '#1d2a22',
        tinta2: '#4b5a51',
        rejilla: '#e6ece7',
        superficie: '#ffffff'
    };

    var nf = new Intl.NumberFormat('es-MX');
    var pf = new Intl.NumberFormat('es-MX', { maximumFractionDigits: 1 });
    var MESES = ['ene', 'feb', 'mar', 'abr', 'may', 'jun', 'jul', 'ago', 'sep', 'oct', 'nov', 'dic'];

    function num(n) { return nf.format(n || 0); }
    function pct(parte, total) { return total ? pf.format(100 * parte / total) + '%' : '—'; }
    function pctNum(parte, total) { return total ? 100 * parte / total : 0; }
    function mes(anio, m) { return MESES[m - 1] + ' ' + String(anio).slice(2); }

    if (global.Chart) {
        Chart.defaults.font.family = '"Segoe UI", system-ui, -apple-system, Roboto, Arial, sans-serif';
        Chart.defaults.font.size = 12;
        Chart.defaults.color = COLOR.tinta2;
        Chart.defaults.plugins.legend.display = false;
        Chart.defaults.maintainAspectRatio = false;
        Chart.defaults.plugins.tooltip.backgroundColor = '#1d2a22';
        Chart.defaults.plugins.tooltip.padding = 10;
        Chart.defaults.plugins.tooltip.cornerRadius = 8;
        Chart.defaults.plugins.tooltip.displayColors = false;
    }

    // Etiqueta de valor al final de cada barra (texto en tinta, nunca del color de la serie)
    var pluginEtiquetas = {
        id: 'etiquetasValor',
        afterDatasetsDraw: function (chart, args, opts) {
            if (!opts || !opts.formato) return;
            var ctx = chart.ctx, horizontal = chart.options.indexAxis === 'y';
            ctx.save();
            ctx.font = '600 12px ' + Chart.defaults.font.family;
            ctx.fillStyle = COLOR.tinta;
            chart.data.datasets.forEach(function (ds, i) {
                chart.getDatasetMeta(i).data.forEach(function (barra, j) {
                    var texto = opts.formato(ds.data[j], j);
                    if (horizontal) { ctx.textAlign = 'left'; ctx.textBaseline = 'middle'; ctx.fillText(texto, barra.x + 6, barra.y); }
                    else { ctx.textAlign = 'center'; ctx.textBaseline = 'bottom'; ctx.fillText(texto, barra.x, barra.y - 4); }
                });
            });
            ctx.restore();
        }
    };

    var registro = {};
    function crear(id, config) {
        if (registro[id]) registro[id].destroy();
        var canvas = document.getElementById(id);
        if (!canvas) return null;
        config.plugins = (config.plugins || []).concat([pluginEtiquetas]);
        if (global.SIN_ANIMACION) config.options.animation = false;
        registro[id] = new Chart(canvas, config);
        return registro[id];
    }

    function ejeValores(max, horizontal, sufijo) {
        return {
            beginAtZero: true,
            suggestedMax: max,
            grid: { color: COLOR.rejilla, drawTicks: false },
            border: { display: false },
            ticks: { precision: 0, padding: 6, callback: function (v) { return nf.format(v) + (sufijo || ''); } }
        };
    }
    // Parte etiquetas largas en varias líneas para que no se recorten
    function partir(texto, max) {
        var palabras = String(texto).split(' '), lineas = [], actual = '';
        palabras.forEach(function (p) {
            if ((actual + ' ' + p).trim().length > max && actual) { lineas.push(actual); actual = p; }
            else actual = (actual + ' ' + p).trim();
        });
        if (actual) lineas.push(actual);
        return lineas.length > 1 ? lineas : lineas[0];
    }
    function ejeCategorias(max) {
        return {
            grid: { display: false }, border: { color: '#cfd8d1' },
            ticks: { color: COLOR.tinta, autoSkip: false, maxRotation: 0, callback: function (v) { return partir(this.getLabelForValue(v), max || 16); } }
        };
    }

    /**
     * Barras horizontales.
     * datos: [{ etiqueta, valor, color?, detalle? }]   op: { sufijo, formato, maxEje }
     */
    function barrasH(id, datos, op) {
        op = op || {};
        var valores = datos.map(function (d) { return d.valor; });
        var max = Math.max.apply(null, valores.concat([0]));
        var config = {
            type: 'bar',
            data: {
                labels: datos.map(function (d) { return d.etiqueta; }),
                datasets: [{
                    data: valores,
                    backgroundColor: datos.map(function (d) { return d.color || COLOR.serie; }),
                    borderRadius: 4, borderSkipped: 'start', maxBarThickness: 22, categoryPercentage: .8, barPercentage: .9
                }]
            },
            options: {
                indexAxis: 'y',
                layout: { padding: { right: 48 } },
                scales: { x: ejeValores(op.maxEje || max * 1.08, true, op.sufijo), y: ejeCategorias(op.maxEtiqueta || 22) },
                plugins: {
                    etiquetasValor: { formato: op.formato || function (v) { return nf.format(v); } },
                    tooltip: { callbacks: { label: function (c) { var d = datos[c.dataIndex]; return d.detalle || (op.formato ? op.formato(c.raw, c.dataIndex) : nf.format(c.raw)); } } }
                }
            }
        };
        return crear(id, config);
    }

    function columnas(id, datos, op) {
        op = op || {};
        var valores = datos.map(function (d) { return d.valor; });
        var max = Math.max.apply(null, valores.concat([0]));
        return crear(id, {
            type: 'bar',
            data: {
                labels: datos.map(function (d) { return d.etiqueta; }),
                datasets: [{ data: valores, backgroundColor: datos.map(function (d) { return d.color || COLOR.serie; }), borderRadius: 4, borderSkipped: 'start', maxBarThickness: 24 }]
            },
            options: {
                layout: { padding: { top: 20 } },
                scales: { y: ejeValores(max * 1.12, false), x: ejeCategorias(op.maxEtiqueta || 6) },
                plugins: {
                    etiquetasValor: { formato: op.formato || function (v) { return nf.format(v); } },
                    tooltip: { callbacks: { label: function (c) { var d = datos[c.dataIndex]; return d.detalle || nf.format(c.raw); } } }
                }
            }
        });
    }

    function linea(id, datos) {
        var valores = datos.map(function (d) { return d.valor; });
        var ultimo = valores.length - 1;
        return crear(id, {
            type: 'line',
            data: {
                labels: datos.map(function (d) { return d.etiqueta; }),
                datasets: [{
                    data: valores, borderColor: COLOR.serie, borderWidth: 2, tension: .25,
                    backgroundColor: 'rgba(47,125,50,.10)', fill: true,
                    pointRadius: valores.map(function (v, i) { return i === ultimo ? 5 : 0; }),
                    pointHoverRadius: 6, pointBackgroundColor: COLOR.serie,
                    pointBorderColor: COLOR.superficie, pointBorderWidth: 2, pointHitRadius: 14
                }]
            },
            options: {
                interaction: { mode: 'index', intersect: false },
                scales: { y: ejeValores(Math.max.apply(null, valores.concat([1])) * 1.15), x: { grid: { display: false }, border: { color: '#cfd8d1' }, ticks: { color: COLOR.tinta, maxRotation: 0 } } },
                plugins: { tooltip: { callbacks: { label: function (c) { return num(c.raw) + ' encuestas'; } } } }
            }
        });
    }

    global.G = { COLOR: COLOR, num: num, pct: pct, pctNum: pctNum, mes: mes, barrasH: barrasH, columnas: columnas, linea: linea };
})(window);
