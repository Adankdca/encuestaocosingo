/* Encuesta ciudadana · Ocosingo
   JavaScript sin dependencias (solo Bootstrap para los modales). */
(function () {
    'use strict';

    var cat = window.CATALOGOS;
    var form = document.getElementById('formEncuesta');
    if (!form) return;

    var modalEncuestaEl = document.getElementById('modalEncuesta');
    var modalBienvenidaEl = document.getElementById('modalBienvenida');
    var modalEncuesta = bootstrap.Modal.getOrCreateInstance(modalEncuestaEl);
    var modalBienvenida = bootstrap.Modal.getOrCreateInstance(modalBienvenidaEl);

    var btnAtras = document.getElementById('btnAtras');
    var btnSiguiente = document.getElementById('btnSiguiente');
    var btnEnviar = document.getElementById('btnEnviar');
    var btnCerrar = document.getElementById('btnCerrar');
    var pasoActual = 1;
    var TOTAL_PASOS = 3;
    var MAX_ARCHIVO = 5 * 1024 * 1024;
    var EXTENSIONES = ['pdf', 'jpg', 'jpeg', 'png', 'doc', 'docx', 'xls', 'xlsx'];

    function $(sel, ctx) { return (ctx || document).querySelector(sel); }
    function $$(sel, ctx) { return Array.prototype.slice.call((ctx || document).querySelectorAll(sel)); }
    function el(tag, attrs, texto) {
        var e = document.createElement(tag);
        for (var k in attrs) if (attrs.hasOwnProperty(k)) e.setAttribute(k, attrs[k]);
        if (texto != null) e.textContent = texto;
        return e;
    }
    function opcion(tipo, nombre, valor, texto) {
        var lbl = el('label', { 'class': 'opcion' });
        lbl.appendChild(el('input', { type: tipo, name: nombre, value: valor }));
        lbl.appendChild(el('span', {}, texto));
        return lbl;
    }

    /* ---------- Construye los campos a partir de los catálogos de la BD ---------- */
    function construir() {
        if (!cat) return;
        var cEdad = $('#opcionesEdad');
        cat.edades.forEach(function (x) { cEdad.appendChild(opcion('radio', 'edad', x.id, x.nombre)); });

        var sOcup = $('#ocupacion');
        cat.ocupaciones.forEach(function (x) { sOcup.appendChild(el('option', { value: x.id }, x.nombre)); });

        var cTemas = $('#opcionesTemas');
        cat.temas.forEach(function (x) { cTemas.appendChild(opcion('checkbox', 'temas', x.id, x.nombre)); });

        var cConf = $('#opcionesConfianza');
        cat.confianza.forEach(function (x) { cConf.appendChild(opcion('radio', 'confianza', x.id, x.nombre)); });

        cat.planes.forEach(function (p) {
            var cont = p.idtipo === 1 ? $('#gridPlanes') : $('#gridProgramas');
            var id = 'plan' + p.idplanes;
            var div = el('div', { 'class': 'plan-opcion' });
            div.appendChild(el('input', { type: 'radio', name: 'plan', value: p.idplanes, id: id }));
            var lbl = el('label', { 'for': id });
            lbl.appendChild(el('img', { src: 'img/' + p.icono, alt: '', width: 54, height: 54, loading: 'lazy' }));
            lbl.appendChild(el('span', {}, p.nombrecorto));
            div.appendChild(lbl);
            cont.appendChild(div);
        });
    }

    function planPorId(id) {
        id = parseInt(id, 10);
        for (var i = 0; i < cat.planes.length; i++) if (cat.planes[i].idplanes === id) return cat.planes[i];
        return null;
    }

    /* ---------- Navegación entre pasos ---------- */
    function mostrarPaso(n) {
        pasoActual = n;
        $$('.paso-form', form).forEach(function (p) { p.hidden = parseInt(p.getAttribute('data-paso'), 10) !== n; });
        $$('.pasos li', form).forEach(function (li) {
            var i = parseInt(li.getAttribute('data-indicador'), 10);
            li.classList.toggle('activo', i === n);
            li.classList.toggle('hecho', i < n);
            if (i === n) li.setAttribute('aria-current', 'step'); else li.removeAttribute('aria-current');
        });
        btnAtras.style.visibility = n > 1 ? 'visible' : 'hidden';
        btnSiguiente.hidden = n === TOTAL_PASOS;
        btnEnviar.hidden = n !== TOTAL_PASOS;
        var cuerpo = modalEncuestaEl.querySelector('.modal-body');
        if (cuerpo) cuerpo.scrollTop = 0;
    }

    function valor(nombre) {
        var marcado = form.querySelector('[name="' + nombre + '"]:checked');
        if (marcado) return marcado.value;
        var campo = form.elements[nombre];
        return campo && campo.value !== undefined && campo.type !== 'radio' && campo.type !== 'checkbox' ? campo.value.trim() : '';
    }

    function marcarError(contenedor, hayError, mensaje) {
        contenedor.classList.toggle('con-error', hayError);
        if (mensaje) { var m = $('.error-campo', contenedor); if (m) m.textContent = mensaje; }
        return !hayError;
    }

    function validarArchivo() {
        var input = $('#archivo');
        var cont = input.closest('[data-requerido]');
        var f = input.files && input.files[0];
        if (!f) return marcarError(cont, false);
        var ext = (f.name.split('.').pop() || '').toLowerCase();
        if (EXTENSIONES.indexOf(ext) < 0) return marcarError(cont, true, 'El archivo debe ser PDF, imagen (JPG o PNG), Word o Excel.');
        if (f.size > MAX_ARCHIVO) return marcarError(cont, true, 'El archivo pesa ' + (f.size / 1048576).toFixed(1) + ' MB; el máximo es 5 MB.');
        return marcarError(cont, false);
    }

    function validarPaso(n) {
        var ok = true;
        var paso = form.querySelector('[data-paso="' + n + '"]');
        $$('[data-requerido]', paso).forEach(function (c) {
            var campo = c.getAttribute('data-requerido');
            var valido = true;
            if (campo === 'temas') valido = form.querySelectorAll('[name="temas"]:checked').length > 0;
            else if (campo === 'aviso') valido = $('#aviso').checked;
            else if (campo === 'problematica') valido = valor('problematica').length >= 10;
            else if (campo === 'archivo') valido = validarArchivo();
            else valido = valor(campo) !== '';
            if (campo !== 'archivo') marcarError(c, !valido);
            ok = ok && valido;
        });
        if (!ok) {
            var primero = paso.querySelector('.con-error');
            if (primero) {
                primero.scrollIntoView({ behavior: 'smooth', block: 'center' });
                var foco = primero.querySelector('input, select, textarea');
                if (foco) foco.focus({ preventScroll: true });
            }
        }
        return ok;
    }

    // Quita el mensaje de error en cuanto el ciudadano corrige el campo
    form.addEventListener('change', function (e) {
        var c = e.target.closest('[data-requerido]');
        if (c && c.classList.contains('con-error')) {
            if (c.getAttribute('data-requerido') === 'archivo') validarArchivo();
            else c.classList.remove('con-error');
        }
        if (e.target.name === 'residente') {
            $('#bloqueLocalidad').hidden = e.target.value !== 'SI';
        }
        if (e.target.name === 'plan') actualizarPlan();
        if (e.target.id === 'archivo') validarArchivo();
    });
    form.addEventListener('input', function (e) {
        var cont = form.querySelector('[data-contador="' + e.target.name + '"]');
        if (cont) cont.textContent = e.target.value.length + ' / 2000';
        var c = e.target.closest('[data-requerido]');
        if (c && e.target.name === 'problematica' && e.target.value.trim().length >= 10) c.classList.remove('con-error');
    });

    function actualizarPlan() {
        var p = planPorId(valor('plan'));
        var desc = $('#planDescripcion');
        if (!p) { desc.hidden = true; return; }
        desc.hidden = false;
        desc.textContent = '';
        desc.appendChild(el('strong', {}, p.nombre + ': '));
        desc.appendChild(document.createTextNode(p.descripcion || ''));
        var articulo = /^(plan|programa)\b/i.test(p.nombre) ? 'el ' : (p.idtipo === 1 ? 'el plan ' : 'el programa ');
        $('#preguntaConoce').textContent = '¿Sabías que existía ' + articulo + p.nombre + '?';
    }

    btnSiguiente.addEventListener('click', function () {
        if (validarPaso(pasoActual)) mostrarPaso(pasoActual + 1);
    });
    btnAtras.addEventListener('click', function () {
        if (pasoActual > 1) mostrarPaso(pasoActual - 1);
    });

    /* ---------- Envío ---------- */
    btnEnviar.addEventListener('click', function () {
        if (!validarPaso(3)) return;
        var spinner = btnEnviar.querySelector('.spinner-border');
        btnEnviar.disabled = true; btnAtras.disabled = true; spinner.hidden = false;

        var datos = new FormData(form);
        if (form.elements.residente && valor('residente') !== 'SI') { datos.delete('zona'); datos.delete('localidad'); }

        fetch('Encuesta.ashx', { method: 'POST', body: datos, headers: { 'X-Requested-With': 'fetch' } })
            .then(function (r) {
                return r.json().catch(function () { return { ok: false, mensaje: 'Respuesta inesperada del servidor (' + r.status + ').' }; });
            })
            .then(function (res) {
                if (res.ok) {
                    mostrarExito();
                } else {
                    alerta(res.mensaje || 'No se pudo enviar la encuesta.');
                }
            })
            .catch(function () {
                alerta('No hay conexión con el servidor. Revisa tu internet e intenta de nuevo.');
            })
            .then(function () {
                btnEnviar.disabled = false; btnAtras.disabled = false; spinner.hidden = true;
            });
    });

    function alerta(msg) {
        var prev = $('#alertaEnvio');
        if (prev) prev.remove();
        var a = el('div', { 'class': 'alert alert-danger mt-3', role: 'alert', id: 'alertaEnvio' }, msg);
        form.querySelector('[data-paso="3"]').appendChild(a);
        a.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }

    function mostrarExito() {
        $$('.paso-form', form).forEach(function (p) { p.hidden = true; });
        $('.pasos', form).hidden = true;
        $('#pantallaExito').hidden = false;
        btnAtras.style.visibility = 'hidden';
        btnEnviar.hidden = true; btnSiguiente.hidden = true; btnCerrar.hidden = false;
        enviada = true;
    }

    var enviada = false;
    function reiniciar() {
        form.reset();
        $$('.con-error', form).forEach(function (c) { c.classList.remove('con-error'); });
        $$('[data-contador]', form).forEach(function (c) { c.textContent = '0 / 2000'; });
        var prev = $('#alertaEnvio'); if (prev) prev.remove();
        $('#bloqueLocalidad').hidden = true;
        $('#planDescripcion').hidden = true;
        $('.pasos', form).hidden = false;
        $('#pantallaExito').hidden = true;
        btnCerrar.hidden = true;
        enviada = false;
        mostrarPaso(1);
    }

    function abrirEncuesta(idplan) {
        if (!cat) return;
        reiniciar();
        if (idplan) {
            var r = form.querySelector('[name="plan"][value="' + idplan + '"]');
            if (r) { r.checked = true; actualizarPlan(); }
        }
        modalBienvenida.hide();
        modalEncuesta.show();
    }

    document.addEventListener('click', function (e) {
        var b = e.target.closest('[data-abrir-encuesta]');
        if (b) { e.preventDefault(); abrirEncuesta(b.getAttribute('data-plan')); }
    });
    // Si el ciudadano cierra la encuesta (terminada o no), la siguiente vez empieza desde el paso 1
    modalEncuestaEl.addEventListener('hidden.bs.modal', reiniciar);

    /* ---------- Arranque ---------- */
    construir();
    mostrarPaso(1);

    // El modal de bienvenida aparece una sola vez por visita
    var mostrada = false;
    try { mostrada = sessionStorage.getItem('bienvenidaOcosingo') === '1'; } catch (err) { }
    if (!mostrada) {
        modalBienvenida.show();
        try { sessionStorage.setItem('bienvenidaOcosingo', '1'); } catch (err) { }
    }
})();
