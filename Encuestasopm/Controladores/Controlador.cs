using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Encuestasopm.Modelos;
using Encuestasopm.Controladores;
using System.Web.UI.HtmlControls;
namespace Encuestasopm.Controladores
{
    public class Controlador
    {
       Encuesta.Cnn cn;
        string sql, html, Json, tabs, datoscontrato, datospagos, categorias, query, datoseguimiento, datosestimaciones, datosdocumentos;
        DataTable bloque1, bloque2, bloque3, bloque4, bloqued, bloques, bloquev;

        public String CadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["BDLOCALonnectionString"].ConnectionString;
        public SqlConnection conexion;
        public List<planesprogramas> ComboPlanPrograma()
        {
            combos BDEtica = new combos();
            DataTable dt = new DataTable();

            dt = BDEtica.CargarplanprogramaBD();

            List<planesprogramas> data = new List<planesprogramas>();
            foreach (DataRow dr in dt.Rows)
            {
                data.Add(new planesprogramas
                {
                    idplanes = dr["idplanes"].ToString(),
                    nombre = dr["nombre"].ToString()
                });
            }
            return data;
        }
        public List<entidadocurreClass> ComboEntidad()
        {
            combos BDEtica = new combos();
            DataTable dt = new DataTable();

            dt = BDEtica.CargarOcupacionBD();

            List<entidadocurreClass> data = new List<entidadocurreClass>();
            foreach (DataRow dr in dt.Rows)
            {
                data.Add(new entidadocurreClass
                {
                    idMunOcurre = dr["IDentidad"].ToString(),
                    munocurre = dr["Entidadfederativa"].ToString()
                });
            }
            return data;
        }
        public List<entidadocurreClass> Comboocupacion()
        {
            combos BDEtica = new combos();
            DataTable dt = new DataTable();

            dt = BDEtica.CargarOcupacionBD();

            List<entidadocurreClass> data = new List<entidadocurreClass>();
            foreach (DataRow dr in dt.Rows)
            {
                data.Add(new entidadocurreClass
                {
                    idMunOcurre = dr["idocupaciones"].ToString(),
                    munocurre = dr["ocupacion"].ToString()
                });
            }
            return data;
        }
        public List<dependenciadenunciadoClass> ComboDependencia()
        {
            combos BDEtica = new combos();
            DataTable dt = new DataTable();

            dt = BDEtica.CargarDependenciaBD();

            List<dependenciadenunciadoClass> data = new List<dependenciadenunciadoClass>();
            foreach (DataRow dr in dt.Rows)
            {
                data.Add(new dependenciadenunciadoClass
                {
                    idDepenDenunciado = dr["IDdependencia"].ToString(),
                    Dependenunciado = dr["Dependencia"].ToString()
                });
            }
            return data;
        }
        //public string EnviarDenuncia(String anonimo, String notificar, String txtnombre, String txtdireccion, String txttelefono, String txtemail,
        //    String sltentidadocurre, String txtfechainiciocurrio, String txtfechafinocurrio, String txtnarracion, String txtnomdenunciado, String txtpatdenunciado,
        //    String txtmatdenunciado, String sltDependencia, String txtadscripcion, String txtpuesto, String sltentidaddenunciado, String txtnombreconsta,
        //    String txtdireccionconsta, String txttelefonoconsta, String txtemailconsta, String radioservidor, String sltDependenciaConsta, String txtcargoconsta)
        //{
        //    String resultado = "";
        //    String respBD;
        //    combos BDEtica = new combos();
        //    try
        //    {
        //        respBD = BDEtica.EnviarDenunciaBD(anonimo, notificar, txtnombre, txtdireccion, txttelefono, txtemail, sltentidadocurre, txtfechainiciocurrio,
        //                txtfechafinocurrio, txtnarracion, txtnomdenunciado, txtpatdenunciado, txtmatdenunciado, sltDependencia, txtadscripcion,
        //                txtpuesto, sltentidaddenunciado, txtnombreconsta, txtdireccionconsta, txttelefonoconsta, txtemailconsta, radioservidor,
        //                sltDependenciaConsta, txtcargoconsta);
        //        switch (respBD)
        //        {
        //            case "EI":
        //                resultado = "EI";
        //                break;
        //            case "EEX":
        //                resultado = "EEX";
        //                break;
        //            case "UEX":
        //                resultado = "UEX";
        //                break;
        //            default:
        //                resultado = "OK";
        //                //EnviarCorreoElectronicoReg(respBD, txtnombre, txttelefono, txtemail, "00007-20");
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resultado = "Ocurrió un error en el servidor. Inténtelo de nuevo." + ex;
        //    }
        //    return resultado;
        //}
        #region Metodos Generales

        public Boolean AbrirConexion()
        {
            Boolean exito;
            try
            {
                conexion = new SqlConnection(CadenaConexion);
                conexion.Open();
                exito = true;
            }
            catch (Exception)
            {
                exito = false;
            }
            return exito;
        }
        public void CerrarConexion()
        {
            if (conexion == null) { return; }
            conexion.Close();
            conexion = null;
        }


        #endregion
        public List<bandejaquejasClass> cargarBandeja()
        {
            combos BDEtica = new combos();
            DataTable dt = new DataTable();
            dt = BDEtica.cargarBandejaBD();
            List<bandejaquejasClass> data = new List<bandejaquejasClass>();
            foreach (DataRow dr in dt.Rows)
            {
                data.Add(new bandejaquejasClass
                {
                    idqueja = dr["IDqueja"].ToString(),
                    nombre = dr["Nombre"].ToString(),
                    telefono = dr["Telefono"].ToString(),
                    email = dr["Email"].ToString(),
                    nomservidor = dr["nombrecontra"].ToString(),
                    depenservidor = dr["Entidadfederativa"].ToString(),
                    adscripcion = dr["Entidadfederativa"].ToString(),
                    hechos = dr["Entidadfederativa"].ToString(),
                    idstatus = dr["Entidadfederativa"].ToString(),
                    status = dr["Entidadfederativa"].ToString(),
                    fechaini = dr["Entidadfederativa"].ToString(),
                    fechafin = dr["Entidadfederativa"].ToString()
                });
            }
            return data;
        }
        //================================================================================================================
        //==============================================wizard============================================================
        //================================================================================================================
        public string getwizard()
        {
            html = "";
            html += "<div class='kt-content  kt-grid__item kt-grid__item--fluid' id='kt_content' style='min-width:100%;'>";
            html += "		<div class='row' style='zoom:100%;'>";
            html += "			<div class='col-lg-12'>";
            html += "				<div class='kt-portlet'>";
            //html += "					<div class='kt-portlet__head'>";
            //html += "						<div class='kt-portlet__head-label'>";
            //html += "							<h3 class='kt-portlet__head-title'>";
            //html += "								TU PARTICIPACIÓN ES MUY IMPORTANTE";
            //html += "                            </h3>";
            //html += "						</div>";
            //html += "					</div>";
            html += "					<div class='kt-form'>";
            html += "							<div class='kt-portlet__body'>";
            html += "								<div class='kt-portlet__body kt-portlet__body--fit'>";
            html += "									<div class='kt-grid kt-wizard-v3 kt-wizard-v3--white' id='kt_wizard_denuncia' data-ktwizard-state='step-first'>";
            html += "										<div class='kt-grid__item'>";
            html += "											<div class='kt-wizard-v3__nav'>";
            html += "												<div class='kt-wizard-v3__nav-items'>";
            //=============================================elementos de  wizard================================================================================================
            //html += "													<a class='kt-wizard-v3__nav-item' href='#' data-ktwizard-type='step' data-ktwizard-state='current'>";
            //html += "														<div class='kt-wizard-v3__nav-body'>";
            //html += "															<div class='kt-wizard-v3__nav-label'>";
            //html += "																<span>1</span> Mensaje de Bienvenida";
            //html += "                                                            </div>";
            //html += "															<div class='kt-wizard-v3__nav-bar'></div>";
            //html += "														</div>";
            //html += "													</a>";
           
            //nuevo 05102020
            html += "													<a class='kt-wizard-v3__nav-item' href='#' data-ktwizard-type='step' data-ktwizard-state='current'>";
            html += "														<div class='kt-wizard-v3__nav-body'>";
            html += "															<div class='kt-wizard-v3__nav-label'>";
            html += "																<span>1</span> Registro";
            html += "                                                            </div>";
            html += "															<div class='kt-wizard-v3__nav-bar'></div>";
            html += "														</div>";
            html += "													</a>";


            html += "													<a class='kt-wizard-v3__nav-item' href='#' data-ktwizard-type='step' >";
            html += "														<div class='kt-wizard-v3__nav-body'>";
            html += "															<div class='kt-wizard-v3__nav-label'>";
            html += "																<span>2</span>  Plan o Programa";
            html += "                                                            </div>";
            html += "															<div class='kt-wizard-v3__nav-bar'></div>";
            html += "														</div>";
            html += "													</a>";
            //nuevo 05102020
            html += "													<a class='kt-wizard-v3__nav-item' href='#' data-ktwizard-type='step'>";
            html += "														<div class='kt-wizard-v3__nav-body'>";
            html += "															<div class='kt-wizard-v3__nav-label'>";
            html += "																<span>3</span> Preguntas";
            html += "                                                            </div>";
            html += "															<div class='kt-wizard-v3__nav-bar'></div>";
            html += "														</div>";
            html += "													</a>";
            //=============================================elementos de  wizard================================================================================================
            html += "												</div>";
            html += "											</div>";
            html += "										</div>";
            html += "										<div class='kt-grid__item kt-grid__item--fluid kt-wizard-v3__wrapper'>";
            html += "											<form class='kt-form' id='kt_form'>";
            //=============================================primer wizard mensaje de bienvenida================================================================================================
            //html += "												<div class='kt-wizard-v3__content' data-ktwizard-type='step-content' data-ktwizard-state='current'>";
            //html += "													<div class='kt-form__section kt-form__section--first'>";
            //html += "														<div class='kt-wizard-v3__form'>";
            //html += "                                                        <div class='kt-portlet kt-portlet--height-fluid kt-widget19'>";
            //html += "							                                        <div class='kt-portlet__body kt-portlet__body--fit'>";
            //html += "								                                        <div class='kt-widget19__pic kt-portlet-fit--top kt-portlet-fit--sides' style='min-height:450px; background-image: url(img/7.png)'>";
            //html += "									                                        <h3 class='kt-widget19__title kt-font-light'>";
            //html += "                                                                           </h3>";
            //html += "									                                        <div class='kt-widget19__shadow'></div>";
            //html += "									                                        <div class='kt-widget19__labels'>";
            //html += "										                                        <a href = '#' class='btn btn-label-light-o2 btn-bold btn-sm'>Recent</a>";
            //html += "									                                        </div>";
            //html += "								                                        </div>";
            //html += "							                                        </div>";
            //html += "							                                        <div class='kt-portlet__body'>";
            //html += "								                                        <div class='kt-widget19__wrapper'>";
            //html += "									                                        <div class='kt-widget19__text'>";
            //html += "										                                        Tu Opinión es muy importante queremos un municipio donde participen la ciudadania....";
            //html += "									                                        </div>";
            //html += "								                                        </div>";
            //html += "							                                        </div>";
            //html += "						                                        </div>";

            //         html += "															<div class='form-group row'>";
            //         html += "																<label class='col-lg-3 col-form-label'>Tipo Persona:</label>";
            //         html += "																<div class='col-lg-6'>";
            //         html += "																	<div class='kt-checkbox-inline'>";
            //         html += "																		<label class='kt-checkbox kt-checkbox--brand'>";
            //         html += "																			<input id = 'chkanonimo' type='checkbox'> Anonimo";
            //         html += "                                                                              <span></span>";
            //         html += "																		</label>";
            //         html += "																		<label class='kt-checkbox kt-checkbox--brand'>";
            //         html += "																			<input id = 'chknotificar' type='checkbox'> Persona a notificar";
            //         html += "                                                                              <span></span>";
            //         html += "																		</label>";
            //         html += "																	</div>";
            //         html += "																</div>";
            //         html += "															</div>";
            //         html += "															<div class='form-group row'>";
            //         html += "																<label class='col-form-label col-lg-1 col-sm-12'>Nombre:</label>";
            //         html += "																<div class='col-lg-4'>";
            //         html += "																	<input type = 'text' ID='txtnombre' name='txtnombre' class='form-control' placeholder='Escribir el nombre'>";
            //         html += "																	<span class='form-text text-muted'>Introducir el nombre completo</span>";
            //         html += "																</div>";
            //         html += "																<label class='col-form-label col-lg-1 col-sm-12'>Dirección:</label>";
            //         html += "																<div class='col-lg-6'>";
            //         html += "																	<input type = 'text' id='txtdireccion' name='txtdireccion' class='form-control' placeholder='Introduce una direccion'>";
            //         html += "																	<span class='form-text text-muted'>Introducir el domicilio</span>";
            //         html += "																</div>";
            //         html += "															</div>";

            //         html += "															<div class='form-group row'>";
            //         html += "																<label class='col-lg-1 col-form-label' for='exampleSelect1'>Entidad Federativa:</label>";
            //html += "																<div class='col-lg-6'>";
            //         html += "																	<select id = 'sltentidadocurre' name='sltentidadocurre' class='form-control m-bootstrap-select m_selectpicker' data-live-search='true'>";
            //																		   Controlador enc = new Controlador();
            //																		   foreach (var data in enc.ComboEntidad() )
            //																		   {
            //         html += "																				<option value = '" +data.idMunOcurre + "'>" + data.munocurre + " </option>";

            //                                                                                    }
            //         html += "																	</select>";
            //         html += "																	<span class='form-text text-muted'>Seleccione una entidad.</span>";
            //         html += "																</div>";
            //         html += "															</div>";
            //html += "															<div class='form-group row'>";
            //html += "																<label class='col-form-label col-lg-2 col-sm-12'>Fecha Inicio</label>";
            //html += "																<div class='col-lg-4'>";
            //html += "																	<div class='input-group date'>";
            //html += "																		<input type = 'text' class='form-control' readonly placeholder='Selecciona una fecha' ID='txtfechainiciocurrio' name='txtfechainiciocurrio' />";
            //html += "																		<div class='input-group-append'>";
            //html += "																			<span class='input-group-text'>";
            //html += "																				<i class='la la-calendar-check-o'></i>";
            //html += "																			</span>";
            //html += "																		</div>";
            //html += "																	</div>";
            //html += "																	<span class='form-text text-muted'>Introducir fecha.</span>";
            //html += "																</div>";
            //html += "																<label class='col-form-label col-lg-2 col-sm-12'>Fecha Fin</label>";
            //html += "																<div class='col-lg-4'>";
            //html += "																	<div class='input-group date'>";
            //html += "																		<input type = 'text' class='form-control' readonly placeholder='Selecciona una fecha' ID='txtfechafinocurrio' name='txtfechafinocurrio' />";
            //html += "																		<div class='input-group-append'>";
            //html += "																			<span class='input-group-text'>";
            //html += "																				<i class='la la-calendar-check-o'></i>";
            //html += "																			</span>";
            //html += "																		</div>";
            //html += "																	</div>";
            //html += "																	<span class='form-text text-muted'>Introducir fecha.</span>";
            //html += "																</div>";
            //html += "															</div>";


            //estos 3 div son el cierre del primer wizard
            //html += "														</div>";
            //html += "													</div>";
            //html += "												</div>";
            //=============================================primer wizard mensaje de bienvenida================================================================================================
            //=============================================segundo wizard seleccion del plan o programa=======================================================================================
            html += "												<div class='kt-wizard-v3__content' data-ktwizard-type='step-content' data-ktwizard-state='current'>";
            html += "													<div class='kt-heading kt-heading--md'>Llena los espacios con tus datos personales</div>";
            html += "													<div class='kt-form__section kt-form__section--first'>";
            html += "														<div class='kt-wizard-v3__form'>";

            html += "															<div class='form-group row'>";
            html += "																<label class='col-form-label col-lg-2 col-sm-12'>Nombre Completo<strong style='color:red'>*</strong> :</label>";
            html += "																<div class='col-lg-6'>";
            html += "																	<input ID = 'txtnomciudadano' name='txtnomciudadano' type='text' class='form-control' placeholder='Nombre Completo'>";
            html += "																	<span class='form-text text-muted'>Escriba su nombre completo por favor</span>";
            html += "																</div>";
            html += "																<label class='col-lg-1 col-form-label'>Género<strong style='color:red'>*</strong>:</label>";
            html += "																<div class='col-lg-3'>";
            html += "																	<select id = 'cbogenero' name='cbogenero' class='form-control kt-selectpicker' >";
            html += "                                                                            <option value=\"\">(Seleccione) </option>";
            html += "                                                                            <option value=\"M\">Masculino </option>";
            html += "                                                                            <option value=\"F\">Femenino </option>";
            html += "																	</select>";
            html += "																	<span class='form-text text-muted'>Seleccione un genero </span>";
            html += "																</div>";
            html += "															</div>";

            html += "                                                           <div class='form-group row'>";
            html += "                                                                    <div class='col-lg-12'><label>¿Seleccione el rango de su Edad?<strong style='color:red'>*</strong></label></div>";
            html += "                                                                    <div class='col-lg-12'>";
            html += "                                                                        <div class='kt-radio-inline'>";
            html += "                                                                            <input type = 'hidden' id='radioservidoredad' value='1'/>";
            html += "                                                                            <label class='kt-radio kt-radio--bold kt-radio--brand'>";
            html += "                                                                                <input type = 'radio' name='radio1' value='1' checked> 1-15 años";
            html += "                                                                                  <span></span>";
            html += "                                                                            </label>";
            html += "                                                                            <label class='kt-radio kt-radio--bold kt-radio--brand'>";
            html += "                                                                                <input type = 'radio' name='radio1' value='2'> 16-30 años";
            html += "                                                                                  <span></span>";
            html += "                                                                            </label>";
            html += "                                                                            <label class='kt-radio kt-radio--bold kt-radio--brand'>";
            html += "                                                                                <input type = 'radio' name='radio1' value='3'> 31-50 años";
            html += "                                                                                  <span></span>";
            html += "                                                                            </label>";
            html += "                                                                            <label class='kt-radio kt-radio--bold kt-radio--brand'>";
            html += "                                                                                <input type = 'radio' name='radio1' value='4'> 50-en adelante";
            html += "                                                                                  <span></span>";
            html += "                                                                            </label>";
            html += "                                                                        </div>";
            html += "                                                                    </div>";
            html += "                                                           </div>";

            html += "															<div class='form-group row'>";
            html += "																<label class='col-lg-1 col-form-label' for='exampleSelect1'>Ocupación:</label>";
            html += "																<div class='col-lg-6'>";
            html += "																	<select id = 'cboocupacion' name='cboocupacion' class='form-control m-bootstrap-select m_selectpicker' data-live-search='true'>";
            Controlador enc = new Controlador();
            foreach (var data in enc.Comboocupacion())
            {
                html += "																				<option value = '" + data.idMunOcurre + "'>" + data.munocurre + " </option>";

            }
            html += "																	</select>";
            html += "																	<span class='form-text text-muted'>Seleccione una entidad.</span>";
            html += "																</div>";

            html += "																<label class='col-lg-2 col-form-label'>Nacido en Chiapas:</label>";
            html += "																<div class='col-lg-3'>";
            html += "																	<select id = 'cbonacido' name='cbonacido' class='form-control kt-selectpicker' >";
            html += "                                                                            <option value=\"\">(Seleccione una opción) </option>";
            html += "                                                                            <option value=\"SI\">SI </option>";
            html += "                                                                            <option value=\"NO\">NO </option>";
            html += "																	</select>";
            html += "																	<span class='form-text text-muted'>Seleccione una opción </span>";
            html += "																</div>";
            html += "															</div>";

            html += "                                                           <div class='alert alert-success' role='alert'>";
            html += "											                <strong>Datos obligatorios</strong><strong style='color:red'>*</strong>";
            html += "										                    </div>";
            html += "														</div>";
            html += "													</div>";
            html += "												</div>";

            //=============================================segundo wizard seleccion del plan o programa=======================================================================================
            //=============================================tercer wizard datos personales=====================================================================================================
            html += "												<div class='kt-wizard-v3__content' data-ktwizard-type='step-content' >";
            html += "													<div class='kt-heading kt-heading--md'>Seleccione un Plan o programa para la Encuesta</div>";
            html += "													<div class='kt-form__section kt-form__section--first'>";
            html += "														<div class='kt-wizard-v3__form'>";

            //html += "															<div class='form-group row'>";
            //html += "																<label class='col-lg-1 col-form-label' for='exampleSelect1'>Seleccione el Plan o Programa:</label>";
            //html += "																<div class='col-lg-6'>";
            //html += "																	<select id = 'sltentidadocurre' name='sltentidadocurre' class='form-control kt-selectpicker' >";
            //html += "                                                                            <option value=\"\">(Seleccione Plan o Programa) </option>";
            //Controlador enc = new Controlador();
            //foreach (var data in enc.ComboPlanPrograma())
            //{
            //    html += "<option value = '" + data.idplanes + "'>" + data.nombre + " </option>";
            //}
            //html += "																	</select>";
            //html += "																	<span class='form-text text-muted'>Seleccione el tipo de plan o programa para realizar la encuesta </span>";
            //html += "																</div>";
            //html += "															</div>";

            //**********************************************************************elemnetos iconos******************************************
            html += "<div class='row'>";
            html += "        <div class='col-lg-12'>";
            html += "            <div class='row'>";
            html += "                <div class='col-lg-5' style='margin-bottom:3%;'>";
            html += "                        <div class='kt-widget31'>";
            html += "                            <div class='kt-widget31__item'>";
            html += "                                <div class='kt-widget31__content'>";
            html += "                                    <div class='kt-widget31__pic'>";
            html += "	                                    <img src = 'img/plan.png' alt='' style='width: 10rem;border-radius: 40%;'>";
            html += "                                    </div>";
            html += "                                    <div class='kt-widget31__info'>";
            html += "	                                    <a href = '#' class='kt-widget31__username' style='font-size: 50px;'>";
            html += "		                                    Planes";
            html += "                                        </a>";
            html += "	                                    <p class='kt-widget31__text'>";
            html += "	                                    </p>";
            html += "                                    </div>";
            html += "                                </div>";
            html += "                            </div>";
            html += "                        </div>";
            //html += "                           <div class='alert alert-info' role='alert' style='background: #cb7e94;border: 1px solid #cb7e94;'>";
            //html += "								<div class='alert-text'><h1>PLANES</h1></div>";
            //html += "							</div>";
            html += "                </div>";
            html += "                <div class='col-lg-2'>";
            html += "                </div>";
            html += "                <div class='col-lg-5' style='margin-bottom:3%;'>";
            html += "                        <div class='kt-widget31'>";
            html += "                            <div class='kt-widget31__item'>";
            html += "                                <div class='kt-widget31__content'>";
            html += "                                    <div class='kt-widget31__pic'>";
            html += "	                                    <img src = 'img/programa.png' alt='' style='width: 10rem;border-radius: 40%;'>";
            html += "                                    </div>";
            html += "                                    <div class='kt-widget31__info'>";
            html += "	                                    <a href = '#' class='kt-widget31__username' style='font-size: 50px;'>";
            html += "		                                    Programas";
            html += "                                        </a>";
            html += "	                                    <p class='kt-widget31__text'>";
            html += "	                                    </p>";
            html += "                                    </div>";
            html += "                                </div>";
            html += "                            </div>";
            html += "                        </div>";
            html += "                </div>";
            html += "            </div>";
            html += "            <div class='row'>";
            html += "                <div class='col-lg-5'>";
            html += "	                <div class='form-group row form-group-marginless'>";
            html += "		                <div class='col-lg-12'>";
            html += "			                <div class='row'>";

            html += "                              <input type = 'hidden' id='radioplaprog' value=''/>";
            html += "				                <div class='col-lg-2'>";
            html += "					                <label class='kt-option kt-option--plain'>";
            html += "						                <span class='kt-option__control'>";
            html += "							                <span class='kt-radio kt-radio--brand'>";
            html += "								                <input type = 'radio' name='idplanes' value='1'>";
            html += "								                <span></span>";
            html += "							                </span>";
            html += "						                </span>";
            html += "						                <span class='kt-option__label'>";
            html += "							                <span class='kt-option__head'>";
            html += "								                <span class='kt-option__title'>";
            html += "									                <img style = 'width: 3.5rem;border-radius: 4px;' src='img/1p.png' alt=''>";
            html += "								                </span>";
            html += "							                </span>";
            html += "							                <span class='kt-option__body'>";
            html += "								                Plan Municipal";
            html += "                                            </span>";
            html += "						                </span>";
            html += "					                </label>";
            html += "				                </div>";

            html += "				                <div class='col-lg-2'>";
            html += "					                <label class='kt-option kt-option--plain'>";
            html += "						                <span class='kt-option__control'>";
            html += "							                <span class='kt-radio kt-radio--brand'>";
            html += "								                <input type = 'radio' name='idplanes' value='2'>";
            html += "								                <span></span>";
            html += "							                </span>";
            html += "						                </span>";
			html += "						                <span class='kt-option__label'>";
            html += "							                <span class='kt-option__head'>";
            html += "								                <span class='kt-option__title'>";
            html += "									                <img style = 'width: 3.5rem;border-radius: 4px;' src='img/2p.png' alt=''>";
            html += "								                </span>";
            html += "							                </span>";
            html += "							                <span class='kt-option__body'>";
            html += "								                 Desarrollo Municipal";
            html += "                                            </span>";
            html += "						                </span>";
            html += "					                </label>";
            html += "				                </div>";

            html += "				                <div class='col-lg-2'>";
            html += "					                <label class='kt-option kt-option--plain'>";
            html += "						                <span class='kt-option__control'>";
            html += "							                <span class='kt-radio kt-radio--brand'>";
            html += "								                <input type = 'radio' name='idplanes' value='3'>";
            html += "								                <span></span>";
            html += "							                </span>";
            html += "						                </span>";
            html += "						                <span class='kt-option__label'>";
            html += "							                <span class='kt-option__head'>";
            html += "								                <span class='kt-option__title'>";
            html += "									                <img style = 'width: 4rem;border-radius: 4px;' src='img/3p.png' alt=''>";
            html += "								                </span>";
            html += "							                </span>";
            html += "							                <span class='kt-option__body'>";
            html += "								                Salud";
            html += "                                            </span>";
            html += "						                </span>";
            html += "					                </label>";
            html += "				                </div>";

            html += "				                <div class='col-lg-2'>";
            html += "					                <label class='kt-option kt-option--plain'>";
            html += "						                <span class='kt-option__control'>";
            html += "							                <span class='kt-radio kt-radio--brand'>";
            html += "								                <input type = 'radio' name='idplanes' value='4'>";
            html += "								                <span></span>";
            html += "							                </span>";
            html += "						                </span>";
            html += "						                <span class='kt-option__label'>";
            html += "							                <span class='kt-option__head'>";
            html += "								                <span class='kt-option__title'>";
            html += "									                <img style = 'width: 4rem;border-radius: 4px;' src='img/4p.png' alt=''>";
            html += "								                </span>";
            html += "							                </span>";
            html += "							                <span class='kt-option__body'>";
            html += "								                Vivienda";
            html += "                                            </span>";
            html += "						                </span>";
            html += "					                </label>";
            html += "				                </div>";

            html += "				                <div class='col-lg-2'>";
            html += "					                <label class='kt-option kt-option--plain'>";
            html += "						                <span class='kt-option__control'>";
            html += "							                <span class='kt-radio kt-radio--brand'>";
            html += "								                <input type = 'radio' name='idplanes' value='5'>";
            html += "								                <span></span>";
            html += "							                </span>";
            html += "						                </span>";
            html += "						                <span class='kt-option__label'>";
            html += "							                <span class='kt-option__head'>";
            html += "								                <span class='kt-option__title'>";
            html += "									                <img style = 'width: 4rem;border-radius: 4px;' src='img/5p.png' alt=''>";
            html += "								                </span>";
            html += "							                </span>";
            html += "							                <span class='kt-option__body'>";
            html += "								                Educativo";
            html += "                                            </span>";
            html += "						                </span>";
            html += "					                </label>";
            html += "				                </div>";

            html += "				                <div class='col-lg-2'>";
            html += "					                <label class='kt-option kt-option--plain'>";
            html += "						                <span class='kt-option__control'>";
            html += "							                <span class='kt-radio kt-radio--brand'>";
            html += "								                <input type = 'radio' name='idplanes' value='6'>";
            html += "								                <span></span>";
            html += "							                </span>";
            html += "						                </span>";
            html += "						                <span class='kt-option__label'>";
            html += "							                <span class='kt-option__head'>";
            html += "								                <span class='kt-option__title'>";
            html += "									                <img style = 'width: 4rem;border-radius: 4px;' src='img/6p.png' alt=''>";
            html += "								                </span>";
            html += "							                </span>";
            html += "							                <span class='kt-option__body'>";
            html += "								                Económico";
            html += "                                            </span>";
            html += "						                </span>";
            html += "					                </label>";
            html += "				                </div>";

            html += "			                </div>";
            html += "		                </div>";
            html += "	                </div>";
            html += "                </div>";


            html += "<div class='col-lg-2'></div>";
            html += "               <div class='col-lg-5'>";
            html += "	                <div class='form-group row form-group-marginless'>";
            html += "		                <div class='col-lg-12'>";
            html += "			                <div class='row'>";

            html += "				                <div class='col-lg-2'>";
            html += "					                <label class='kt-option kt-option--plain'>";
            html += "						                <span class='kt-option__control'>";
            html += "							                <span class='kt-radio kt-radio--brand'>";
            html += "								                <input type = 'radio' name='idplanes' value='7'>";
            html += "								                <span></span>";
            html += "							                </span>";
            html += "						                </span>";
            html += "						                <span class='kt-option__label'>";
            html += "							                <span class='kt-option__head'>";
            html += "								                <span class='kt-option__title'>";
            html += "									                <img style = 'width: 4rem;border-radius: 4px;' src='img/1prog.png' alt=''>";
            html += "								                </span>";
            html += "							                </span>";
            html += "							                <span class='kt-option__body'>";
            html += "								                Barriguita Llena";
            html += "                                            </span>";
            html += "						                </span>";
            html += "					                </label>";
            html += "				                </div>";

            html += "				                <div class='col-lg-2'>";
            html += "					                <label class='kt-option kt-option--plain'>";
            html += "						                <span class='kt-option__control'>";
            html += "							                <span class='kt-radio kt-radio--brand'>";
            html += "								                <input type = 'radio' name='idplanes' value='8'>";
            html += "								                <span></span>";
            html += "							                </span>";
            html += "						                </span>";
            html += "						                <span class='kt-option__label'>";
            html += "							                <span class='kt-option__head'>";
            html += "								                <span class='kt-option__title'>";
            html += "									                <img style = 'width: 4rem;border-radius: 4px;' src='img/2prog.png' alt=''>";
            html += "								                </span>";
            html += "							                </span>";
            html += "							                <span class='kt-option__body'>";
            html += "								                Apoyos Productivos";
            html += "                                            </span>";
            html += "						                </span>";
            html += "					                </label>";
            html += "				                </div>";

            html += "				                <div class='col-lg-2'>";
            html += "					                <label class='kt-option kt-option--plain'>";
            html += "						                <span class='kt-option__control'>";
            html += "							                <span class='kt-radio kt-radio--brand'>";
            html += "								                <input type = 'radio' name='idplanes' value='9'>";
            html += "								                <span></span>";
            html += "							                </span>";
            html += "						                </span>";
            html += "						                <span class='kt-option__label'>";
            html += "							                <span class='kt-option__head'>";
            html += "								                <span class='kt-option__title'>";
            html += "									                <img style = 'width: 4rem;border-radius: 4px;' src='img/3prog.png' alt=''>";
            html += "								                </span>";
            html += "							                </span>";
            html += "							                <span class='kt-option__body'>";
            html += "								                Apoyos Dif";
            html += "                                            </span>";
            html += "						                </span>";
            html += "					                </label>";
            html += "				                </div>";

            html += "				                <div class='col-lg-2'>";
            html += "					                <label class='kt-option kt-option--plain'>";
            html += "						                <span class='kt-option__control'>";
            html += "							                <span class='kt-radio kt-radio--brand'>";
            html += "								                <input type = 'radio' name='idplanes' value='10'>";
            html += "								                <span></span>";
            html += "							                </span>";
            html += "						                </span>";
            html += "						                <span class='kt-option__label'>";
            html += "							                <span class='kt-option__head'>";
            html += "								                <span class='kt-option__title'>";
            html += "									                <img style = 'width: 4rem;border-radius: 4px;' src='img/4prog.png' alt=''>";
            html += "								                </span>";
            html += "							                </span>";
            html += "							                <span class='kt-option__body'>";
            html += "								                Becas Escolares";
            html += "                                            </span>";
            html += "						                </span>";
            html += "					                </label>";
            html += "				                </div>";

            html += "				                <div class='col-lg-2'>";
            html += "					                <label class='kt-option kt-option--plain'>";
            html += "						                <span class='kt-option__control'>";
            html += "							                <span class='kt-radio kt-radio--brand'>";
            html += "								                <input type = 'radio' name='idplanes' value='11'>";
            html += "								                <span></span>";
            html += "							                </span>";
            html += "						                </span>";
            html += "						                <span class='kt-option__label'>";
            html += "							                <span class='kt-option__head'>";
            html += "								                <span class='kt-option__title'>";
            html += "									                <img style = 'width: 4rem;border-radius: 4px;' src='img/5prog.png' alt=''>";
            html += "								                </span>";
            html += "							                </span>";
            html += "							                <span class='kt-option__body'>";
            html += "								                Asuntos Jurídicos";
            html += "                                            </span>";
            html += "						                </span>";
            html += "					                </label>";
            html += "				                </div>";

            html += "				                <div class='col-lg-2'>";
            html += "					                <label class='kt-option kt-option--plain'>";
            html += "						                <span class='kt-option__control'>";
            html += "							                <span class='kt-radio kt-radio--brand'>";
            html += "								                <input type = 'radio' name='idplanes' value='12'>";
            html += "								                <span></span>";
            html += "							                </span>";
            html += "						                </span>";
            html += "						                <span class='kt-option__label'>";
            html += "							                <span class='kt-option__head'>";
            html += "								                <span class='kt-option__title'>";
            html += "									                <img style = 'width: 4rem;border-radius: 4px;' src='img/6prog.png' alt=''>";
            html += "								                </span>";
            html += "							                </span>";
            html += "							                <span class='kt-option__body'>";
            html += "								                Otros Programas";
            html += "                                            </span>";
            html += "						                </span>";
            html += "					                </label>";
            html += "				                </div>";
            html += "			                </div>";
            html += "		                </div>";
            html += "	                </div>";
            html += "                </div>";
            html += "            </div>";
            html += "        </div>";
            html += "    </div>";
            //**********************************************************************elemnetos iconos******************************************
            html += "														</div>";
            html += "													</div>";
            html += "												</div>";


            //=============================================tercer wizard datos personales=====================================================================================================
            //=============================================cuarto wizard preguntas de acuerdo al tipo de plan oprogram========================================================================
            html += "												<div class='kt-wizard-v3__content' data-ktwizard-type='step-content'>";
            html += "													<div class='kt-heading kt-heading--md'>Encuesta</div>";
            html += "													<div class='kt-form__section kt-form__section--first'>";
            html += "														<div class='kt-wizard-v3__form' id='contenidoencuesta'>";
            //html += "															<div class='form-group row'>";
            //html += "																<label class='col-lg-1 col-form-label'>Nombre(s) :</label>";
            //html += "																<div class='col-lg-3'>";
            //html += "																	<input type = 'text' ID='txtnombreconsta' name='txtnombreconsta' class='form-control' placeholder='Escribir el nombre'>";
            //html += "																	<span class='form-text text-muted'>Introducir el nombre completo</span>";
            //html += "																</div>";
            //html += "																<label class='col-lg-1 col-form-label'>Dirección:</label>";
            //html += "																<div class='col-lg-4'>";
            //html += "																	<input type = 'text' ID='txtdireccionconsta' name='txtdireccionconsta' class='form-control' placeholder='Domicilio...'>";
            //html += "																	<span class='form-text text-muted'>Introducir el domicilio</span>";
            //html += "																</div>";
            //html += "															</div>";
            html += "														</div>";
            html += "													</div>";
            html += "												</div>";
            //=============================================cuarto wizard preguntas de acuerdo al tipo de plan oprogram========================================================================
            //=============================================botones wizard=====================================================================================================================
            html += "												<div class='kt-form__actions'>";
            html += "													<div class='btn btn-secondary btn-md btn-tall btn-wide kt-font-bold kt-font-transform-u' data-ktwizard-type='action-prev'>";
            html += "														Atras";
            html += "                                                    </div>";
            html += "													<div class='btn btn-success btn-md btn-tall btn-wide kt-font-bold kt-font-transform-u' data-ktwizard-type='action-submit'>";
            html += "														Enviar";
            html += "                                                    </div>";
            html += "													<div class='btn btn-brand btn-md btn-tall btn-wide kt-font-bold kt-font-transform-u' data-ktwizard-type='action-next'>";
            html += "														Siguiente";
            html += "                                                    </div>";
            html += "												</div>";
            //=============================================botones wizard=====================================================================================================================
            html += "											</form>";
            html += "										</div>";
            html += "									</div>";
            html += "								</div>";
            html += "							</div>";
            html += "						</div>";
            html += "				</div>";
            html += "			</div>";
            html += "		</div>";
            html += "</div>";



            //===============================================================mandar a llamar el wizard 
            html += "<script language=\"javascript\" type=\"text/javascript\">";
            html += "\"use strict\";";
            html += " var KTWizard3 = function() {";
            html += "    var wizardEl;";
            html += "    var formEl;";
            html += "    var validator;";
            html += "    var wizard;";
            html += "    var initWizard = function() {";
            html += "        wizard = new KTWizard('kt_wizard_denuncia', {";
            html += "    startStep: 1,";
            html += "});";
            html += "  wizard.on('beforeNext', function(wizardObj) {";
            html += "            if (validator.form() !== true)";
            html += "            {";
            html += "                wizardObj.stop();";
            html += "            }";
            html += "        });";
            html += " wizard.on('change', function(wizard) {";
            html += "            KTUtil.scrollTop();";
            html += "        });";
            html += "    };";

            html += " var initValidation = function() {";
            html += "        validator = formEl.validate({";
            html += "        ignore: \":hidden\",";
            html += "	rules:";
            html += "        {";
            //======================bloque de registro de datos del ciudadano===============
            html += "            txtnomciudadano:";
            html += "                {";
            html += "                required: true";
            html += "                },";
            html += "		     cbogenero:";
            html += "                {";
            html += "                required: true";
            html += "                },";
            html += "		     cboocupacion:";
            html += "                {";
            html += "                required: true";
            html += "                },";
            //======================bloque de registro de datos del ciudadano===============
            //======================bloque de selección de plan o programa==================
            //html += "            sltentidadocurre:";
            //html += "                {";
            //html += "                required: true";
            //html += "                }";
            //======================bloque de selección de plan o programa==================
            //======================bloque de selección de planes===========================
            html += "            idplanes:";
            html += "                {";
            html += "                required: true";
            html += "                },";
            //======================bloque de selección de planes===========================
            //======================bloque al seleccionar un plan o programa================
            html += "            cboexistencia:";
            html += "                {";
            html += "                required: true";
            html += "                },";
            html += "            cboapoyo:";
            html += "                {";
            html += "                required: true";
            html += "                },";
            html += "            txtproblematica:";
            html += "                {";
            html += "                required: true";
            html += "                },";
            html += "            cboconfianza:";
            html += "                {";
            html += "                required: true";
            html += "                }";
            //======================bloque al seleccionar un plan o programa================
            html += "         },";
            html += "	invalidHandler: function(event, validator) {";
            html += " KTUtil.scrollTop();";
            html += " swal.fire({";
            html += "    'title': '',";
            html += "			'text': 'Por favor llenar los campos obligatorios.',";
            html += "			'type': 'error',";
            html += "			'confirmButtonClass': 'btn btn-secondary'";
            html += "        });";
            html += "},";
            html += "	submitHandler: function (form) {";
            html += "	}";
            html += "});";
            html += "};";

            html += " var initSubmit = function() {";
            html += " var btn = formEl.find('[data-ktwizard-type=\"action-submit\"]');";
            html += " btn.on('click', function(e) {";
            html += "    e.preventDefault();";
            html += "    if (validator.form())";
            html += "    {";
            html += "        KTApp.progress(btn);";
            html += "        formEl.ajaxSubmit({";
            html += "        success: function() {";
            html += "                var inseguridad = \"0\";";
            html += "                var bienestar = \"0\";";
            html += "                var abandono = \"0\";";
            html += "                var economico = \"0\";";
            html += "                var desatencion = \"0\";";
            html += "                if ($('#chkinseguridad').prop('checked')) {";
            html += "                    inseguridad = '1';";
            html += "                }";
            html += "                if ($('#chkbienestar').prop('checked')) {";
            html += "                    bienestar = '1';";
            html += "                }";
            html += "                if ($('#chkabandono').prop('checked')) {";
            html += "                    abandono = '1';";
            html += "                }";
            html += "                if ($('#chkeconomico').prop('checked')) {";
            html += "                    economico = '1';";
            html += "                }";
            html += "                if ($('#chkdesatencion').prop('checked')) {";
            html += "                    desatencion = '1';";
            html += "                }";


            //=====================================================definimos que tipo de preguntas es la que se va a guardar en la base de datos==================================
            html += "var idtipoplan=$('#radioplaprog').val();";
            html += "var parametros='';";
            html += "var archivosubida='';";
            html += "if(document.getElementById('myfile').files.length != 0)";
            html += "{";
            html += "archivosubida=document.getElementById('myfile').files[0].name";
            html += "}";
            html += "                 parametros = {";
            //====================================bloque de nombre del coudadano====================================
            html += "					            txtnomciudadano: $('#txtnomciudadano').val(),";
            html += "					            cbogenero: $('#cbogenero').val(),";
            html += "					            radioservidoredad: $('#radioservidoredad').val(),";
            html += "					            cboocupacion: $('#cboocupacion').val(),";
            html += "					            cbonacido: $('#cbonacido').val(),";
            //====================================bloque de nombre del coudadano====================================
            //====================================bloque de plan o programa=========================================
            html += "					            idplanes: $('#radioplaprog').val(),";
            //====================================bloque de plan o programa=========================================
            //====================================bloque de preguntas===============================================
            html += "					            cboexistencia: $('#cboexistencia').val(),";
            html += "					            cboapoyo: $('#cboapoyo').val(),";
            html += "					            txtproblematica: $('#txtproblematica').val(),";
            html += "					            txtpropuesta: $('#txtpropuesta').val(),";
            html += "					            inseguridad: inseguridad,";
            html += "					            bienestar: bienestar,";
            html += "					            abandono: abandono,";
            html += "					            economico: economico,";
            html += "					            desatencion: desatencion,";
           // html += "                               archivo:document.getElementById('myfile').files[0].name,";
            html += "                               archivo:archivosubida,";
            html += "					            cboconfianza: $('#cboconfianza').val()";
            //====================================bloque de preguntas===============================================
            html += "                            };";
            
            //html += "if(idtipoplan==\"2\")";
            //html += "{";
            //html += "                 parametros = {";
            //html += "					            sltentidadocurre: $('#sltentidadocurre').val(),";
            //html += "					            txtnomciudadano: $('#txtnomciudadano').val(),";
            //html += "					            txtpatciudadano: $('#txtpatciudadano').val(),";
            //html += "					            txtmatciudadano: $('#txtmatciudadano').val()";
            //html += "                            };";
            //html += "}";
            //=====================================================definimos que tipo de preguntas es la que se va a guardar en la base de datos==================================
            //=====================================================definimos que tipo de preguntas es la que se va a guardar en la base de datos con ajax=========================
            html += "				            $.ajax({";
            html += "                               async: true,";
            html += "					            type: 'POST',";
            html += "					            dataType: 'json',";
            html += "					            contentType: 'application/json; charset=utf-8',";
            html += "					            url: 'WebService1.asmx/encuestaocosingo',";
            html += "					            data: JSON.stringify(parametros),";
            html += "					            success: function(response) {";
            html += "                               if (response)";
            html += "                               {";
            html += "                                $('#kt_modal_1').modal('hide');";
            html += "                                swal.fire({";
            html += "                               'title': 'Encuesta',";
            html += "								            'text': 'Informacion enviada satisfactoriamente! , gracias por su participación...',";
            html += "								            'type': 'success',";
            html += "								            'confirmButtonClass': 'btn btn-secondary'";
            html += "                                        });";
            html += "                                }";
            html += "                              },";
            html += "					           error: function() { alert('Error AJAX al enviar los datos.') }";
            html += "            });";
            //=====================================================definimos que tipo de preguntas es la que se va a guardar en la base de datos con ajax=========================
            html += "            KTApp.unprogress(btn);";
            html += "        }";
            html += "    });";
            html += "}";
            html += " });";
            html += " };";

            html += " return {";
            html += " init: function() {";
            html += "        wizardEl = KTUtil.get('kt_wizard_denuncia');";
            html += "        formEl = $('#kt_form');";
            html += "        initWizard();";
            html += "        initValidation();";
            html += "        initSubmit();";
            html += "    }";
            html += " };";
            html += "    }();";
            html += "jQuery(document).ready(function() {";
            html += "    KTWizard3.init();";
            html += "});";
            html += "</script>";

            //html += "<script language=\"javascript\" type=\"text/javascript\">";
            //html += "  var plan = \"0\";";
            //html += " $('select[name=sltentidadocurre]').change(function() {";
            //html += " plan=$('select[name=sltentidadocurre]').val() ;";
            //html += " if(plan != \"\") ";
            //html += " {";
            //html += "		$.ajax({";
            //html += "               type: 'GET', cache: false, url: \"Main.aspx?tipo=1&idplan=\" + plan, success: function(msg) {";
            //html += "				$('#contenidoencuesta').html(msg);";
            //html += "               }";
            //html += "             });";
            //html += " }";
            //html += "  });";
            //html += "</script>";

            html += "<script language=\"javascript\" type=\"text/javascript\">";
            html += "$('input[type=radio][name=radio1]').change(function() {";
            html += "    if (this.value == '1')";
            html += "    {";
            html += "			$('#radioservidoredad').val('1');";
            html += "    }";
            html += "    if (this.value == '2')";
            html += "    {";
            html += "		$('#radioservidoredad').val('2');";
            html += "    }";
            html += "    if (this.value == '3')";
            html += "    {";
            html += "		$('#radioservidoredad').val('3');";
            html += "    }";
            html += "    if (this.value == '4')";
            html += "    {";
            html += "		$('#radioservidoredad').val('4');";
            html += "    }";
            html += "});";
            html += "</script>";

            html += "<script language=\"javascript\" type=\"text/javascript\">";
            html += "  var plan = \"0\";";
            html += "$('input[type=radio][name=idplanes]').change(function() {";
            html += "    if (this.value == '1')";
            html += "    {";
            html += "			$('#radioplaprog').val('1');";
            html += "    }";
            html += "    if (this.value == '2')";
            html += "    {";
            html += "		$('#radioplaprog').val('2');";
            html += "    }";
            html += "    if (this.value == '3')";
            html += "    {";
            html += "		$('#radioplaprog').val('3');";
            html += "    }";
            html += "    if (this.value == '4')";
            html += "    {";
            html += "		$('#radioplaprog').val('4');";
            html += "    }";
            html += "    if (this.value == '5')";
            html += "    {";
            html += "		$('#radioplaprog').val('5');";
            html += "    }";
            html += "    if (this.value == '6')";
            html += "    {";
            html += "		$('#radioplaprog').val('6');";
            html += "    }";
            html += "    if (this.value == '7')";
            html += "    {";
            html += "		$('#radioplaprog').val('7');";
            html += "    }";
            html += "    if (this.value == '8')";
            html += "    {";
            html += "		$('#radioplaprog').val('8');";
            html += "    }";
            html += "    if (this.value == '9')";
            html += "    {";
            html += "		$('#radioplaprog').val('9');";
            html += "    }";
            html += "    if (this.value == '10')";
            html += "    {";
            html += "		$('#radioplaprog').val('10');";
            html += "    }";
            html += "    if (this.value == '11')";
            html += "    {";
            html += "		$('#radioplaprog').val('11');";
            html += "    }";
            html += "    if (this.value == '12')";
            html += "    {";
            html += "		$('#radioplaprog').val('12');";
            html += "    }";
            html += " plan=$('#radioplaprog').val() ;";
            html += " if(plan != \"\") ";
            html += " {";
            html += "		$.ajax({";
            html += "               type: 'GET', cache: false, url: \"Main.aspx?tipo=1&idplan=\" + plan, success: function(msg) {";
            html += "				$('#contenidoencuesta').html(msg);";
            html += "               }";
            html += "             });";
            html += " }";
            html += "  });";
            //html += "});";
            html += "</script>";








            //html += "<script src=\"assets/vendors/general/bootstrap-datepicker/dist/js/bootstrap-datepicker.js\" type = \"text/javascript\"></script>";
            //html += " <script src=\"assets/vendors/general/bootstrap-datepicker/js/locales/bootstrap-datepicker.es.js\" type = \"text/javascript\"></script>";
            //html += "<script language=\"javascript\" type=\"text/javascript\">";
            //html += "         var KTBootstrapDatepicker = function() {";
            //html += "          var arrows;";
            //html += "          if (KTUtil.isRTL())";
            //html += "             {";
            //html += "            arrows = {";
            //html += "            leftArrow: '<i class=\"la la-angle-right\"></i>',";
            //html += "            rightArrow: '<i class=\"la la-angle-left\"></i>'";
            //html += "                    }";
            //html += "            }";
            //html += "           else";
            //html += "           {";
            //html += "           arrows = {";
            //html += "            leftArrow: '<i class=\"la la-angle-left\"></i>',";
            //html += "            rightArrow: '<i class=\"la la-angle-right\"></i>'";
            //html += "                    }";
            //html += "                };";
            //html += "        var demos = function() {";
            //html += "        $('#txtfechainiciocurrio, #kt_datepicker_1_validate').datepicker({";
            //html += "            rtl: KTUtil.isRTL(),";
            //html += "            todayHighlight: true,";
            //html += "            orientation: \"bottom left\",";
            //html += "            templates: arrows,";
            //html += "            language: 'es'";
            //html += "        });";
            //html += "        $('#txtfechafinocurrio, #kt_datepicker_1_validate').datepicker({";
            //html += "            rtl: KTUtil.isRTL(),";
            //html += "            todayHighlight: true,";
            //html += "            orientation: 'bottom left',";
            //html += "            templates: arrows,";
            //html += "            language: 'es'";
            //html += "        });";
            //html += "                };";
            //html += "                return {";
            //html += "                init: function() {";
            //html += "                        demos();";
            //html += "                    }";
            //html += "                };";
            //html += "            } ();";
            //html += "            jQuery(document).ready(function() {";
            //html += "                KTBootstrapDatepicker.init();";
            //html += "            });";
            //html += "</script>";





            return html;
        }

        public string preguntas(string idplan)
        {
            html = "";
            if (idplan == "1" || idplan == "2" || idplan == "3" || idplan == "4" || idplan == "5" || idplan == "6")
            {
                html += "															<div class='form-group row'>";
                html += "																<label class='col-lg-3 col-form-label' style='color:#4a7de4;'>¿Sabías que existía un Plan?</label>";
                html += "																<div class='col-lg-3'>";
                html += "																	<select id = 'cboexistencia' name='cboexistencia' class='form-control kt-selectpicker'>";
                html += "                                                                            <option value=\"\">(Seleccione)</option>";
                html += "                                                                            <option value=\"SI\">SI </option>";
                html += "                                                                            <option value=\"NO\">NO </option>";
                html += "																	</select>";
                html += "																	<span class='form-text text-muted'>Seleccione un opción </span>";
                html += "																</div>";
                html += "															</div>";

            }
            if (idplan == "7" || idplan == "8" || idplan == "9" || idplan == "10" || idplan == "11" || idplan == "12")
            {
                html += "															<div class='form-group row'>";
                html += "																<label class='col-lg-3 col-form-label' style='color:#4a7de4;'>¿Sabías que existía un Programa?</label>";
                html += "																<div class='col-lg-3'>";
                html += "																	<select id = 'cboexistencia' name='cboexistencia' class='form-control kt-selectpicker'>";
                html += "                                                                            <option value=\"\">(Seleccione)</option>";
                html += "                                                                            <option value=\"SI\">SI </option>";
                html += "                                                                            <option value=\"NO\">NO </option>";
                html += "																	</select>";
                html += "																	<span class='form-text text-muted'>Seleccione un opción </span>";
                html += "																</div>";
                html += "															</div>";
            }
            
               // html += "";
                //html += "                                                           <div class='form-group row'>";
                //html += "                                                                    <div class='col-lg-12'><label>¿El trabajo de infraestructura de desarrollo social ha sido beneficioso a su comunidad?</label></div>";
                //html += "                                                                    <div class='col-lg-12'>";
                //html += "                                                                        <div class='kt-radio-inline'>";
                //html += "                                                                            <input type = 'hidden' id='radioservidor' value='1'/>";
                //html += "                                                                            <label class='kt-radio kt-radio--bold kt-radio--brand'>";
                //html += "                                                                                <input type = 'radio' name='radio2' value='0' checked> SI";
                //html += "                                                                                  <span></span>";
                //html += "                                                                            </label>";
                //html += "                                                                            <label class='kt-radio kt-radio--bold kt-radio--brand'>";
                //html += "                                                                                <input type = 'radio' name='radio2' value='1'> NO";
                //html += "                                                                                  <span></span>";
                //html += "                                                                            </label>";
                //html += "                                                                        </div>";
                //html += "                                                                    </div>";
                //html += "                                                             </div>";
                

                html += "															<div class='form-group row'>";
                html += "																<label class='col-lg-3 col-form-label' style='color:#4a7de4;'>¿Has recibido apoyo del Gobierno?</label>";
                html += "																<div class='col-lg-3'>";
                html += "																	<select id = 'cboapoyo' name='cboapoyo' class='form-control kt-selectpicker'>";
                html += "                                                                            <option value=\"\">(Seleccione) </option>";
                html += "                                                                            <option value=\"SI\">SI </option>";
                html += "                                                                            <option value=\"NO\">NO </option>";
                html += "																	</select>";
                html += "																	<span class='form-text text-muted'>Seleccione un opción </span>";
                html += "																</div>";
                html += "															</div>";
                html += "                                                           <div class='form-group row'>";
                html += "									                                    <label class='col-lg-12 col-sm-12' style='color:#4a7de4;'>¿Cuáles consideras que son los principales cinco temas que el gobierno debe atender?</ label>";
                html += "									                                    <div class='col-lg-12 col-md-12 col-sm-12'>";
                html += "										                                    <div class='kt-checkbox-list' style='color:#6d75a0'>";
                html += "											                                    <label class='kt-checkbox'>";
                html += "												                                    <input type = 'checkbox' id='chkinseguridad' name='chkinseguridad'> Inseguridad y Corrupción";
                html += "												                                    <span></span>";
                html += "											                                    </label>";
                html += "											                                    <label class='kt-checkbox'>";
                html += "												                                    <input type = 'checkbox' id='chkbienestar' name='chkbienestar'> Falta de Bienestar Social";
                html += "												                                    <span></span>";
                html += "											                                    </label>";
                html += "											                                    <label class='kt-checkbox'>";
                html += "												                                    <input type = 'checkbox' id='chkabandono' name='chkabandono'> Abandono Educativo";
                html += "												                                    <span></span>";
                html += "											                                    </label>";
                html += "                                                                                <label class='kt-checkbox'>";
                html += "												                                    <input type = 'checkbox' id='chkeconomico' name='chkeconomico'> Escaso Desarrollo Económico";
                html += "                                                                                    <span></span>";
                html += "											                                    </label>";
                html += "                                                                                <label class='kt-checkbox'>";
                html += "												                                    <input type = 'checkbox' id='chkdesatencion' name='chkdesatencion'> Desatención al medio Ambiente";
                html += "                                                                                    <span></span>";
                html += "											                                    </label>";
                html += "										                                    </div>";
                html += "										                                    <span class='form-text text-muted'>Por favor seleccione las opciones</span>";
                html += "									                                    </div>";
                html += "								                                    </div>";



                html += "                                                    <div class='form-group row'>";
                html += "                                                        <label class='col-lg-3 col-form-label' for='exampleTextarea' style='color:#4a7de4;'>Describe en términos generales la problemática señalada:</label>";
                html += "                                                        <div class='col-lg-9'>";
                html += "                                                            <textarea ID = 'txtproblematica' name='txtproblematica' class='form-control' rows='3'></textarea>";
                html += "                                                            <span class='form-text text-muted'>Escribe la problemática </span>";
                html += "                                                        </div>";
                html += "                                                    </div>";

                html += "                                                    <div class='form-group row'>";
                html += "                                                        <label class='col-lg-3 col-form-label' for='exampleTextarea' style='color:#4a7de4;'>Escriba una Propuesta:</label>";
                html += "                                                        <div class='col-lg-9'>";
                html += "                                                            <textarea ID = 'txtpropuesta' name='txtpropuesta' class='form-control' rows='3'></textarea>";
                html += "                                                            <span class='form-text text-muted'>Escribe una propuesta </span>";
                html += "                                                        </div>";
                html += "                                                    </div>";


                html += "                       <div class='row'>";
                html += "                            <div class='col-xl-12'>";
                html += "                                <div class='m-form__section m-form__section--first'>";
                html += "                                    <div class='m-form__heading'>";
                html += "                                        <h4 class='m-form__heading-title'>Si cuenta con una propuesta elaborada en algún formato, adjunte archivo</h4>";
                html += "                                    </div>";
                html += "                                    <div class='form-group m-form__group row'>";
                html += "                                        <div class='col-lg-12' style='margin:0 auto;'>";
                html += "                                    </div>";
                html += "                                </div>";
                html += "                            </div>";
                html += "                        </div>";
                html += "                    </div>";


                html += " <div class='row'>";
                html += "  <div class='col-lg-12'>";
                html += "    <table class='table table-bordered kt-datatable__table' >";
                html += "        <thead>";
                html += "        <tr>";
                html += "            <th>Seleccione Archivo</th>";
                html += "            <th></th>";
                html += "            <th>Tamaño de Archivo</th>";
                html += "            <th></th>";
                html += "            <th>Tipo de Archivo</th>";
                html += "            <th></th>";
                html += "            <th>Subir Archivo</th>";
                html += "            <th>Proceso de Carga</th>";
                html += "        </tr>";
                html += "        </thead>";
                html += "        <tbody>";
                html += "        <tr>";
                html += "            <td >";
                html += "               <input type = 'hidden' id='archivocarga' value=''/>";
                html += "				<span class='kt-list-timeline__text'><input type = 'file' id='myfile' name='myfile' class='rm-input' onchange='selectedFile();'/></span>";
                html += "				<span class='kt-list-timeline__time'>Seleccione archivo</span>";
                html += "            </td>";
                html += "            <td>  </td>   ";
                html += "			 <td><span class='kt-list-timeline__text'><div id = 'fileSize' ></div ></span>";
                html += "            </td>";
                html += "            <td>  </td>   ";
                html += "			 <td><span class='kt-list-timeline__text'><div id = 'fileType' ></div></span>";
                html += "            </td>";
                html += "            <td>  </td>   ";
                html += "			 <td><span class='kt-list-timeline__text'><input type='button' value='Subir Archivo' onclick='uploadFile()' class='rm-button' /></span>";
                html += "            </td>";
                html += "			 <td><span class='kt-list-timeline__text'><progress id='progressBar' value='0' max='100' class='rm-progress'></progress></span><div id = 'percentageCalc' ></div>";
                html += "            </td>";

                html += "        </tr>";
                html += "        </tbody>";
                html += "        </table>";
                html += "  </div>";
                html += "</div>";


                // html += "       <div class='kt-list-timeline' style='border-bottom: 0.17rem dashed #d5d7dc;'>";
                // html += "		  <div class='kt-list-timeline__items'>";
                // html += "			<div class='kt-list-timeline__item'>";
                // html += "               <input type = 'hidden' id='archivocarga' value=''/>"; 
                // html += "				<span class='kt-list-timeline__badge kt-list-timeline__badge--brand'></span>";
                // html += "				<span class='kt-list-timeline__text'><input type = 'file' id='myfile' name='myfile' class='rm-input' onchange='selectedFile();'/></span>";
                //// html += "				<span class='kt-list-timeline__time'>Seleccione archivo</span>";
                // html += "			</div>";
                // html += "			<div class='kt-list-timeline__item'>";
                // html += "				<span class='kt-list-timeline__badge kt-list-timeline__badge--info'></span>";
                // html += "				<span class='kt-list-timeline__text'><div id = 'fileSize' ></div ></span>";
                // html += "				<span class='kt-list-timeline__time'></span>";
                // html += "			</div>";
                // html += "			<div class='kt-list-timeline__item'>";
                // html += "				<span class='kt-list-timeline__badge kt-list-timeline__badge--success'></span>";
                // html += "				<span href = '' class='kt-list-timeline__text'><div id = 'fileType' ></div></span>";
                // html += "				<span class='kt-list-timeline__time'></span>";
                // html += "			</div>";
                // html += "            <div class='kt-list-timeline__item'>";
                // html += "					<span class='kt-list-timeline__badge kt-list-timeline__badge--brand'></span>";
                // html += "				<span class='kt-list-timeline__text'><input type = 'button' value='Subir Archivo' onclick='uploadFile()' class='rm-button' /></span>";
                // html += "				<span class='kt-list-timeline__time'></span>";
                // html += "			</div>";
                // html += "            <div class='kt-list-timeline__item'>";
                // html += "				<span class='kt-list-timeline__badge kt-list-timeline__badge--info'></span>";
                // html += "				<span class='kt-list-timeline__text'><progress id = 'progressBar' value='0' max='100' class='rm-progress'></progress></span>";
                // html += "                <span class='kt-list-timeline__time'><div id = 'percentageCalc' ></div></span>";
                // html += "			</div>";
                // html += "		</div>";
                // html += "	</div>";

                html += "															<div class='form-group row' style='margin-top: 2%;'>";
                html += "																<label class='col-lg-3 col-form-label' style='color:#4a7de4;'>¿Que confianza tienes en el gobierno?</label>";
                html += "																<div class='col-lg-3'>";
                html += "																	<select id = 'cboconfianza' name='cboconfianza' class='form-control kt-selectpicker'>";
                html += "                                                                            <option value=\"0\">(Seleccione)</option>";
                html += "                                                                            <option value=\"1\">Mucha </option>";
                html += "                                                                            <option value=\"2\">Poca </option>";
                html += "                                                                            <option value=\"3\">Nada </option>";
                html += "																	</select>";
                html += "																	<span class='form-text text-muted'>Seleccione un opción </span>";
                html += "																</div>";
                html += "															</div>";

                html += "<script language=\"javascript\" type=\"text/javascript\">";
                html += " ";
                html += "function selectedFile()";
                html += "{";
                html += "    var archivoSeleccionado = document.getElementById(\"myfile\");";
                html += "    var file = archivoSeleccionado.files[0];";
                
                html += "    if (file)";
                html += "    {";
                html += "        var fileSize = 0;";
                html += "        if (file.size > 1048576)";
                html += "            fileSize = (Math.round(file.size * 100 / 1048576) / 100).toString() + ' MB';";
                html += "        else";
                html += "            fileSize = (Math.round(file.size * 100 / 1024) / 100).toString() + ' Kb';";
                html += "        var divfileSize = document.getElementById('fileSize');";
                html += "        var divfileType = document.getElementById('fileType');";
                html += "        divfileSize.innerHTML = 'Tamaño: ' + fileSize;";
                html += "        divfileType.innerHTML = 'Tipo: ' + file.type;";
                
                html += "    }";
                html += "}";
                html += "function uploadFile()";
                html += "{";
                html += "    var url = 'WebService1.asmx/UploadFile';";
                html += "    var archivoSeleccionado = document.getElementById('myfile');";
                html += "    var file = archivoSeleccionado.files[0];";
                html += "    var fd = new FormData();";
                html += "    fd.append('archivo', file);";
                html += "    var xmlHTTP = new XMLHttpRequest();";
                html += "    xmlHTTP.upload.addEventListener('progress', progressFunction, false);";
                html += "    xmlHTTP.addEventListener('load', transferCompleteFunction, false);";
                html += "    xmlHTTP.addEventListener('error', uploadFailed, false);";
                html += "    xmlHTTP.addEventListener('abort', uploadCanceled, false);";
                html += "    xmlHTTP.open('POST', url, true);";
                html += "    xmlHTTP.send(fd);";
                html += "}";
                html += "function transferCompleteFunction(evt)";
                html += "{";
                html += "                                swal.fire({";
                html += "                               'title': 'Encuesta',";
                html += "								            'text': 'Transferencia Completada',";
                html += "								            'type': 'success',";
                html += "								            'confirmButtonClass': 'btn btn-secondary'";
                html += "                                        });";
                //html += "     alert('Transferencia completa');";
                html += "    var progressBar = document.getElementById('progressBar');";
                html += "    var percentageDiv = document.getElementById('percentageCalc');";
                html += "    progressBar.value = 100;";
                html += "    percentageDiv.innerHTML = '100%';";
                html += "}";
                html += "function progressFunction(evt)";
                html += "{";
                html += "    var progressBar = document.getElementById('progressBar');";
                html += "    var percentageDiv = document.getElementById('percentageCalc');";
                html += "    if (evt.lengthComputable)";
                html += "    {";
                html += "        progressBar.max = evt.total;";
                html += "        progressBar.value = evt.loaded;";
                html += "        percentageDiv.innerHTML = Math.round(evt.loaded / evt.total * 100) + '%';";
                html += "    }";
                html += "}";
                html += "function loadStartFunction(evt)";
                html += "{";
                html += "    alert('Comenzando a subir el archivo');";
                html += "}";
                html += "function transferCompleteFunction(evt)";
                html += "{";
                html += "                                swal.fire({";
                html += "                               'title': 'Encuesta',";
                html += "								            'text': 'Transferencia Completada',";
                html += "								            'type': 'success',";
                html += "								            'confirmButtonClass': 'btn btn-secondary'";
                html += "                                        });";
                //html += "    alert('Transferencia completa');";
                html += "    var progressBar = document.getElementById('progressBar');";
                html += "    var percentageDiv = document.getElementById('percentageCalc');";
                html += "    progressBar.value = 100;";
                html += "    percentageDiv.innerHTML = '100%';";
                html += "}";
                html += "function uploadFailed(evt)";
                html += "{";
                html += "    alert('Hubo un error al subir el archivo.');";
                html += "}";
                html += "function uploadCanceled(evt)";
                html += "{";
                html += "    alert('La operación se canceló o la conexión fue interrunpida.');";
                html += "}";
                html += "</script>";


            
            
                //html += "<link href = 'assets/vendors/general/dropzone/dist/dropzone.css' rel = 'stylesheet' type = 'text/css' />";
                //html += "<script src='assets/vendors/general/dropzone/dist/dropzone.js' type='text/javascript'></script>";
                //html += "<script src='assets/app/custom/general/crud/forms/widgets/dropzone.js' type='text/javascript'></script>";
                

                //html += "<script language=\"javascript\" type=\"text/javascript\">";
                //html += "$(document).ready(function () {";
                //html += "    Dropzone.options.cargarevidenciasDroneZone = {";
                //html += "    paramName: \"file\", ";
                //html += "    maxFiles: 1,";
                //html += "    maxFilesize: 30, ";
                //html += "    addRemoveLinks: true,";
                //html += "    acceptedFiles: \"image/*,application/pdf\",";
                //html += "    renameFilename: function(filename) {";
                //html += "                return new Date().getTime() + '_' + filename;";
                //html += "            },";
                //html += "    accept: function(file, done) {";
                //html += "                if (file.name == \"justinbieber.jpg\")";
                //html += "                {";
                //html += "                    done(\"Naha, you don't.\");";
                //html += "                }";
                //html += "                else";
                //html += "                {";
                //html += "                 alert('archivo:' + file.name);";
                //html += "                    done();";
                //html += "                }";
                //html += "            }";
                //html += "        };";
                //html += "})";
                //html += "</script>";
            

            //html += "<script language=\"javascript\" type=\"text/javascript\">";
            //html += "$('input[type=radio][name=radio1]').change(function() {";
            //html += "    if (this.value == '0')";
            //html += "    {";
            //html += "			$('#radioservidor').val('0');";
            //html += "    }";
            //html += "    else if (this.value == '1')";
            //html += "    {";
            //html += "		$('#radioservidor').val('1');";
            //html += "    }";
            //html += "});";
            
            //html += "$('input[type=radio][name=radio6]').change(function() {";
            //html += "    if (this.value == '1')";
            //html += "    {";
            //html += "			$('#radioservidor').val('1');";
            //html += "    }";
            //html += "    else if (this.value == 'S')";
            //html += "    {";
            //html += "		$('#radioservidor').val('S');";
            //html += "    }";
            //html += "});";

            //html += "</script>";

            return html;
        }
    }
}