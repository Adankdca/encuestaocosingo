<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Encuestasopm.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<!-- ============================== PORTADA ============================== -->
<section class="portada" id="inicio">
    <div class="container">
        <div class="row align-items-center g-5">
            <div class="col-lg-7">
                <span class="portada__etiqueta">Plataforma digital · Planes y programas municipales</span>
                <h1>Tu opinión construye <em>Ocosingo</em></h1>
                <p class="lead mt-3">Participa en la Consulta Ciudadana y dinos cómo funcionan los planes y programas del Gobierno Municipal. Tu voz nos ayuda a mejorar la gestión pública.</p>
                <div class="d-flex flex-wrap gap-2 mt-4">
                    <button type="button" class="btn btn-lima btn-lg px-4" data-abrir-encuesta <%= EncuestaDisponible ? "" : "disabled" %>>Participar en la encuesta</button>
                    <a class="btn btn-outline-light btn-lg px-4" href="#planes">Conocer los planes</a>
                </div>
                <div class="portada__datos">
                    <div><strong>3 minutos</strong><span>es lo que te toma</span></div>
                    <div><strong>3 pasos</strong><span>datos, tema y opinión</span></div>
                    <div><strong>Anónima</strong><span>si así lo prefieres</span></div>
                </div>
            </div>
            <div class="col-lg-5 d-none d-lg-block">
                <div class="portada__logo">
                    <img src="img/logo-ocosingo.png" alt="H. Ayuntamiento de Ocosingo 2024–2027. Por ti, un nuevo gobierno, palabra de mujer" width="286" height="360" />
                </div>
            </div>
        </div>
    </div>
</section>

<% if (!EncuestaDisponible) { %>
<div class="container mt-4">
    <div class="alert alert-warning mb-0">La encuesta no está disponible en este momento. Por favor, intenta más tarde.</div>
</div>
<% } %>

<!-- ============================== CÓMO PARTICIPAR ============================== -->
<section class="seccion">
    <div class="container">
        <div class="seccion__titulo">
            <h2 class="subrayado">¿Cómo participar?</h2>
        </div>
        <div class="row g-3">
            <div class="col-md-4"><div class="paso"><div class="paso__num">1</div><h3 class="h5">Cuéntanos de ti</h3><p class="text-secundario mb-0">Edad, ocupación y localidad. Tu nombre es opcional: puedes participar de forma anónima.</p></div></div>
            <div class="col-md-4"><div class="paso"><div class="paso__num">2</div><h3 class="h5">Elige un plan o programa</h3><p class="text-secundario mb-0">Selecciona el plan o programa municipal sobre el que quieres opinar.</p></div></div>
            <div class="col-md-4"><div class="paso"><div class="paso__num">3</div><h3 class="h5">Da tu opinión</h3><p class="text-secundario mb-0">Señala los temas prioritarios, describe la problemática y, si quieres, adjunta tu propuesta.</p></div></div>
        </div>
    </div>
</section>

<!-- ============================== PLANES Y PROGRAMAS ============================== -->
<section class="seccion seccion--blanca" id="planes">
    <div class="container">
        <div class="seccion__titulo">
            <h2 class="subrayado">Planes y programas</h2>
            <p>Conoce a qué se refiere cada plan o programa y elige sobre cuál quieres participar.</p>
        </div>
        <div class="row g-4">
            <div class="col-lg-6">
                <h3 class="grupo-titulo">Planes <span class="badge text-bg-success">Municipales</span></h3>
                <div class="row g-3"><%= TarjetasHtml(1) %></div>
            </div>
            <div class="col-lg-6">
                <h3 class="grupo-titulo">Programas <span class="badge" style="background:var(--guinda)">Sociales</span></h3>
                <div class="row g-3"><%= TarjetasHtml(2) %></div>
            </div>
        </div>
    </div>
</section>

<!-- ============================== QUÉ ES LA CONSULTA ============================== -->
<section class="seccion consulta" id="consulta">
    <div class="container">
        <div class="row g-4 align-items-center">
            <div class="col-lg-7">
                <h2 class="subrayado">¿Qué es la consulta y para qué se realiza?</h2>
                <div class="consulta__cita mt-3">
                    <p class="mb-2">Es un mecanismo de participación ciudadana mediante el cual el Gobierno Municipal somete a consideración de la ciudadanía el funcionamiento de los planes y programas con los que cuenta el municipio de Ocosingo, en sus distintos ámbitos temáticos, sectoriales y territoriales.</p>
                    <p class="mb-0">Las opiniones recibidas se analizan para <strong>mejorar la gestión pública municipal</strong> y orientar las decisiones hacia lo que más necesitan nuestras comunidades.</p>
                </div>
            </div>
            <div class="col-lg-5">
                <div class="llamado">
                    <h2 class="h3">¡Tu participación cuenta!</h2>
                    <p>Para el Gobierno Municipal es muy valiosa tu opinión. Tu participación contribuye a construir una democracia de resultados.</p>
                    <button type="button" class="btn btn-light fw-bold" data-abrir-encuesta <%= EncuestaDisponible ? "" : "disabled" %>>Responder la encuesta</button>
                </div>
            </div>
        </div>
    </div>
</section>

<!-- ============================== MODAL DE BIENVENIDA ============================== -->
<div class="modal fade bienvenida" id="modalBienvenida" tabindex="-1" aria-labelledby="tituloBienvenida" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered modal-lg">
        <div class="modal-content">
            <div class="bienvenida__cabecera">
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                <small>Consulta Ciudadana Participativa</small>
                <h2 id="tituloBienvenida">Bienvenida y bienvenido</h2>
                <p class="mb-0 mt-1" style="color:#d7eadc">Plataforma digital de los planes y programas del Gobierno Municipal de Ocosingo, Chiapas</p>
            </div>
            <div class="bienvenida__cuerpo">
                <p>Este es un espacio de democracia participativa en el que el Gobierno Municipal somete a tu consideración el funcionamiento de sus planes y programas en los distintos ámbitos temáticos, sectoriales y territoriales de Ocosingo.</p>
                <ul class="text-secundario">
                    <li>Responder te toma alrededor de <strong>3 minutos</strong>.</li>
                    <li>Puedes participar de forma <strong>anónima</strong>.</li>
                </ul>
                <p class="bienvenida__lema mb-3">¡Tu participación cuenta!</p>
                <div class="d-flex flex-wrap gap-2">
                    <button type="button" class="btn btn-oco btn-lg px-4" data-abrir-encuesta <%= EncuestaDisponible ? "" : "disabled" %>>Participar ahora</button>
                    <button type="button" class="btn btn-contorno btn-lg" data-bs-dismiss="modal">Conocer primero</button>
                </div>
            </div>
            <div class="bienvenida__logos">
                <img src="img/logo-ocosingo-sm.png" alt="H. Ayuntamiento de Ocosingo 2024–2027" width="127" height="160" />
            </div>
        </div>
    </div>
</div>

<!-- ============================== MODAL DE LA ENCUESTA ============================== -->
<div class="modal fade encuesta" id="modalEncuesta" tabindex="-1" aria-labelledby="tituloEncuesta" aria-hidden="true" data-bs-backdrop="static">
    <div class="modal-dialog modal-xl modal-dialog-scrollable modal-fullscreen-md-down">
        <div class="modal-content">
            <div class="modal-header">
                <h2 class="modal-title" id="tituloEncuesta">Encuesta ciudadana · Ocosingo</h2>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Cerrar"></button>
            </div>
            <div class="modal-body">
                <form id="formEncuesta" novalidate autocomplete="off">
                    <ol class="pasos" aria-label="Avance">
                        <li data-indicador="1">Tus datos</li>
                        <li data-indicador="2">Plan o programa</li>
                        <li data-indicador="3">Tu opinión</li>
                    </ol>

                    <!-- Trampa para robots: las personas no ven este campo -->
                    <div class="campo-trampa" aria-hidden="true">
                        <label for="sitioweb">No llenes este campo</label>
                        <input type="text" id="sitioweb" name="sitioweb" tabindex="-1" autocomplete="off" />
                    </div>

                    <!-- ===== Paso 1 ===== -->
                    <div class="paso-form" data-paso="1">
                        <div class="row g-3">
                            <div class="col-md-7">
                                <label class="form-label" for="nombre">Nombre completo <span class="ayuda">(opcional)</span></label>
                                <input type="text" class="form-control" id="nombre" name="nombre" maxlength="150" placeholder="Puedes dejarlo en blanco" />
                            </div>
                        </div>

                        <fieldset class="mt-3" data-requerido="genero">
                            <legend class="requerido">Género</legend>
                            <div class="opciones">
                                <label class="opcion"><input type="radio" name="genero" value="F" /><span>Mujer</span></label>
                                <label class="opcion"><input type="radio" name="genero" value="M" /><span>Hombre</span></label>
                                <label class="opcion"><input type="radio" name="genero" value="O" /><span>Prefiero no decir</span></label>
                            </div>
                            <div class="error-campo">Selecciona una opción.</div>
                        </fieldset>

                        <fieldset data-requerido="edad">
                            <legend class="requerido">Rango de edad</legend>
                            <div class="opciones" id="opcionesEdad"></div>
                            <div class="error-campo">Selecciona tu rango de edad.</div>
                        </fieldset>

                        <div class="row g-3">
                            <div class="col-md-6" data-requerido="ocupacion">
                                <label class="form-label requerido" for="ocupacion">Ocupación</label>
                                <select class="form-select" id="ocupacion" name="ocupacion"><option value="">Selecciona…</option></select>
                                <div class="error-campo">Selecciona tu ocupación.</div>
                            </div>
                        </div>

                        <fieldset class="mt-3" data-requerido="residente">
                            <legend class="requerido">¿Vives en el municipio de Ocosingo?</legend>
                            <div class="opciones">
                                <label class="opcion"><input type="radio" name="residente" value="SI" /><span>Sí</span></label>
                                <label class="opcion"><input type="radio" name="residente" value="NO" /><span>No</span></label>
                            </div>
                            <div class="error-campo">Selecciona una opción.</div>
                        </fieldset>

                        <div id="bloqueLocalidad" class="row g-3" hidden>
                            <div class="col-md-5">
                                <fieldset class="mb-0">
                                    <legend>Vives en…</legend>
                                    <div class="opciones">
                                        <label class="opcion"><input type="radio" name="zona" value="C" /><span>Cabecera municipal</span></label>
                                        <label class="opcion"><input type="radio" name="zona" value="R" /><span>Localidad rural</span></label>
                                    </div>
                                </fieldset>
                            </div>
                            <div class="col-md-7">
                                <label class="form-label" for="localidad">Localidad, barrio o colonia</label>
                                <input type="text" class="form-control" id="localidad" name="localidad" maxlength="150" placeholder="Ej. nombre de tu comunidad o barrio" />
                            </div>
                        </div>
                    </div>

                    <!-- ===== Paso 2 ===== -->
                    <div class="paso-form" data-paso="2" hidden>
                        <fieldset data-requerido="plan">
                            <legend class="requerido">¿Sobre qué plan o programa quieres opinar?</legend>
                            <h3 class="h6 text-uppercase text-secundario mt-2">Planes</h3>
                            <div class="planes-grid mb-3" id="gridPlanes"></div>
                            <h3 class="h6 text-uppercase text-secundario">Programas</h3>
                            <div class="planes-grid" id="gridProgramas"></div>
                            <div class="error-campo">Selecciona un plan o programa.</div>
                        </fieldset>
                        <div class="plan-elegido" id="planDescripcion" hidden></div>
                    </div>

                    <!-- ===== Paso 3 ===== -->
                    <div class="paso-form" data-paso="3" hidden>
                        <div class="row g-3">
                            <div class="col-md-6">
                                <fieldset data-requerido="conoce">
                                    <legend class="requerido" id="preguntaConoce">¿Sabías que existía este plan o programa?</legend>
                                    <div class="opciones">
                                        <label class="opcion"><input type="radio" name="conoce" value="SI" /><span>Sí</span></label>
                                        <label class="opcion"><input type="radio" name="conoce" value="NO" /><span>No</span></label>
                                    </div>
                                    <div class="error-campo">Selecciona una opción.</div>
                                </fieldset>
                            </div>
                            <div class="col-md-6">
                                <fieldset data-requerido="apoyo">
                                    <legend class="requerido">¿Has recibido algún apoyo del Gobierno Municipal?</legend>
                                    <div class="opciones">
                                        <label class="opcion"><input type="radio" name="apoyo" value="SI" /><span>Sí</span></label>
                                        <label class="opcion"><input type="radio" name="apoyo" value="NO" /><span>No</span></label>
                                    </div>
                                    <div class="error-campo">Selecciona una opción.</div>
                                </fieldset>
                            </div>
                        </div>

                        <fieldset data-requerido="temas">
                            <legend class="requerido">¿Qué temas debe atender el gobierno con prioridad?</legend>
                            <p class="ayuda mt-n1">Puedes elegir varios.</p>
                            <div class="opciones opciones--cuadros" id="opcionesTemas"></div>
                            <div class="error-campo">Selecciona al menos un tema.</div>
                        </fieldset>

                        <div class="mb-3" data-requerido="problematica">
                            <label class="form-label requerido" for="problematica">Describe en términos generales la problemática</label>
                            <textarea class="form-control" id="problematica" name="problematica" rows="3" maxlength="2000" placeholder="¿Qué está pasando? ¿Dónde? ¿A quién afecta?"></textarea>
                            <div class="d-flex justify-content-between"><div class="error-campo">Escribe al menos 10 caracteres.</div><small class="ayuda ms-auto" data-contador="problematica">0 / 2000</small></div>
                        </div>

                        <div class="mb-3">
                            <label class="form-label" for="propuesta">¿Qué propones para resolverla? <span class="ayuda">(opcional)</span></label>
                            <textarea class="form-control" id="propuesta" name="propuesta" rows="3" maxlength="2000"></textarea>
                            <div class="d-flex"><small class="ayuda ms-auto" data-contador="propuesta">0 / 2000</small></div>
                        </div>

                        <div class="mb-3" data-requerido="archivo">
                            <label class="form-label" for="archivo">Si tienes una propuesta elaborada, adjúntala <span class="ayuda">(opcional)</span></label>
                            <input class="form-control" type="file" id="archivo" name="archivo" accept=".pdf,.jpg,.jpeg,.png,.doc,.docx,.xls,.xlsx" />
                            <div class="ayuda mt-1">PDF, imagen, Word o Excel. Máximo 5 MB.</div>
                            <div class="error-campo" id="errorArchivo"></div>
                        </div>

                        <fieldset data-requerido="confianza">
                            <legend class="requerido">¿Qué confianza tienes en el gobierno municipal?</legend>
                            <div class="opciones" id="opcionesConfianza"></div>
                            <div class="error-campo">Selecciona una opción.</div>
                        </fieldset>

                        <div class="form-check" data-requerido="aviso">
                            <input class="form-check-input" type="checkbox" id="aviso" name="aviso" value="1" />
                            <label class="form-check-label" for="aviso">He leído y acepto el <a href="AvisoPrivacidad.aspx" target="_blank" rel="noopener">aviso de privacidad</a>.</label>
                            <div class="error-campo">Debes aceptar el aviso de privacidad para enviar.</div>
                        </div>
                    </div>

                    <!-- ===== Resultado ===== -->
                    <div class="exito" id="pantallaExito" hidden>
                        <div class="exito__icono">
                            <svg width="44" height="44" viewBox="0 0 24 24" aria-hidden="true"><path fill="none" stroke="#0b5e33" stroke-width="2.6" stroke-linecap="round" stroke-linejoin="round" d="M5 12.5l4.5 4.5L19 7.5"/></svg>
                        </div>
                        <h3>¡Gracias por participar!</h3>
                        <p class="text-secundario mb-0">Tu opinión fue registrada correctamente.<br />Con tu participación nos ayudas a mejorar la gestión pública de Ocosingo.</p>
                    </div>
                </form>
            </div>
            <div class="modal-footer justify-content-between" id="piesEncuesta">
                <button type="button" class="btn btn-contorno" id="btnAtras">Atrás</button>
                <div class="d-flex gap-2">
                    <button type="button" class="btn btn-oco px-4" id="btnSiguiente">Siguiente</button>
                    <button type="button" class="btn btn-guinda px-4" id="btnEnviar" hidden>
                        <span class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true" hidden></span>
                        Enviar encuesta
                    </button>
                    <button type="button" class="btn btn-oco px-4" id="btnCerrar" data-bs-dismiss="modal" hidden>Cerrar</button>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script>window.CATALOGOS = <%= CatalogosJson %>;</script>
    <script src="<%= Encuestasopm.Recursos.Url(ResolveUrl("~/js/encuesta.js")) %>"></script>
</asp:Content>
