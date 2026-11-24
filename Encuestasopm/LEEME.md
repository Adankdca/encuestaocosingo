# Consulta Ciudadana Participativa · H. Ayuntamiento de Ocosingo 2024–2027

Versión 2.0 del sistema de encuesta (antes configurado para Chiapa de Corzo).
ASP.NET Web Forms · .NET Framework 4.7.2 · SQL Server 2016 o superior.

---

## 1. Diagnóstico del sistema anterior

### Errores que afectaban el funcionamiento
| # | Problema | Efecto |
|---|----------|--------|
| 1 | `WebService1.encuestaocosingo` siempre regresaba `"conectado"` y el error de BD se ignoraba (`EnviarencuestaBD` devolvía `"FALLO"` sin que nadie lo revisara) | El ciudadano veía "Información enviada" aunque no se guardara nada |
| 2 | La subida de archivos era un paso aparte (botón "Subir Archivo"); si el ciudadano no lo presionaba, el nombre quedaba en la BD pero el archivo nunca llegaba | Registros con archivos inexistentes |
| 3 | "Confianza" tenía `value="0"` en "(Seleccione)", así que la validación `required` siempre pasaba | Se podían guardar encuestas con `idconfianza = 0` |
| 4 | "Ocupación" no tenía opción vacía | Si no se elegía nada se guardaba "Estudiante" |
| 5 | Ocupación se guardaba como texto (`'3'`) sin relación con el catálogo | Estadísticas por ocupación poco confiables |
| 6 | Rangos de edad traslapados (`31-50` y `50-en adelante`) | El 50 caía en dos rangos |
| 7 | `Get_Rpt_Planes` / `Get_Rpt_Temas` usaban cursores innecesarios e ignoraban `@idanio` | Reportes lentos y sin filtro por periodo |
| 8 | `Update_documento` y `tabla1` hacían referencia a tablas de otro sistema | Objetos rotos en la BD |

### Riesgos de seguridad
- **Subida de archivos sin validar** en `/uploads/` con el nombre original: alguien podía subir un `.aspx` y ejecutarlo en el servidor, o sobrescribir archivos de otros.
- **El panel no tenía inicio de sesión**: cualquiera podía ver los nombres y opiniones de los ciudadanos.
- **Web.config** con una cadena de conexión a una IP pública (`187.157.115.235`) y su contraseña en texto plano, `debug="true"` y `customErrors="Off"` (mostraba errores internos a cualquier visitante).
- Cadena de conexión duplicada y fija en el código (`Classconexion.cs`).

### Rendimiento
- El sitio cargaba unas 30 librerías del tema Metronic (fullcalendar, owl carousel, datepickers, session-timeout, `app.bundle.js`, etc.) que la encuesta no usa.
- El modal de bienvenida era una imagen de 2.2 MB (`inicio.png`); el proyecto incluía más de 5,000 archivos de plantilla.
- El HTML de la encuesta se armaba concatenando cadenas en C# (1,500 líneas en `Controlador.cs`) y se pedía por AJAX dos veces.

---

## 2. Qué cambió

**Encuesta pública (`Default.aspx`)**
- Bootstrap 5 local, sin Metronic ni jQuery. La página completa pesa unos 180 KB (código comprimido + imágenes); antes eran varios MB.
- Colores del logotipo de Ocosingo (verdes y franja de colores) con guinda como color de acento. Solo se usa el logotipo del Ayuntamiento.
- Modal de bienvenida en HTML: aparece **una vez por visita** y funciona en celular.
- Encuesta en 3 pasos con validación en el navegador **y en el servidor**: Tus datos → Plan o programa → Tu opinión.
- Nombre **opcional** (participación anónima) y aceptación del **aviso de privacidad**.
- Nuevos campos: "¿Vives en Ocosingo?", **cabecera o localidad rural**, **localidad/colonia**, género "Prefiero no decir" y el tema **"Servicios públicos (agua, drenaje, caminos)"**.
- Las tarjetas de planes y programas tienen el botón "Opinar sobre este" (abre la encuesta con ese plan ya elegido).
- Datos y archivo se envían juntos en una sola petición a `Encuesta.ashx`. Cada encuesta recibe un **folio interno** (`OCO-000123`) que solo se ve en el panel y en el reporte; al ciudadano solo se le muestra un mensaje de agradecimiento.
- Si el ciudadano cierra la encuesta sin terminar, al abrirla de nuevo empieza desde el paso 1 con el formulario vacío.
- Protección anti-robots (campo trampa) y límite de 5 envíos cada 10 minutos por IP.

**Archivos adjuntos**
- Solo PDF, JPG, PNG, Word y Excel; máximo 5 MB; se revisa la firma real del archivo.
- Se guardan con nombre aleatorio en `App_Data/adjuntos` (IIS no publica esa carpeta) y se descargan desde el panel.

**Panel de control (`admin/Panel.aspx`)** · requiere iniciar sesión
- Filtros: fechas, tipo (plan/programa), plan específico y periodos rápidos.
- Temas prioritarios: se muestran como % del total de menciones (suma 100%); en la tabla y al pasar el cursor se ve también el % de participantes que eligió cada tema.
- Indicadores: encuestas recibidas, últimos 30 días, % que conocía el plan, % con apoyo, **índice de confianza (0–100)**, % con propuesta.
- Gráficas: participación por mes, confianza, por plan o programa, temas prioritarios, edad, género, ocupación y localidades.
- Tabla de indicadores por plan y listado de problemáticas y propuestas con búsqueda, detalle y descarga del adjunto.
- **Exportar a Excel (CSV)** con todos los registros filtrados.

**Reporte profesional (`admin/Reporte.aspx`)**
- Tamaño carta con logos institucionales. Incluye resumen ejecutivo redactado automáticamente, indicadores, gráficas, tablas, perfil de participantes y anexo de propuestas.
- Botón "Imprimir / Guardar PDF". Respeta los filtros del panel.
- Sustituye a Crystal Reports: ya no se necesita el runtime de Crystal en el servidor.

**Seguridad**
- Login con contraseñas cifradas (PBKDF2-SHA256), límite de intentos y cambio de contraseña desde el panel.
- Toda la carpeta `/admin` exige sesión (`Web.config`).
- La aplicación usa un usuario SQL que solo puede ejecutar procedimientos.

---

## 3. Instalación

1. **Respalda** la base `BDPlanesprogramas` actual si quieres conservar algo.
2. En SQL Server Management Studio ejecuta `BaseDatos/01_instalacion_ocosingo.sql`.
   - Al final del script hay un bloque comentado para crear el usuario `encuesta_app`. Cambia la contraseña, descoméntalo y ejecútalo.
   - Opcional para pruebas: `BaseDatos/02_datos_prueba_opcional.sql` genera 400 encuestas ficticias (al final dice cómo borrarlas).
3. En `Web.config` ajusta `connectionStrings > EncuestaDB` (servidor, usuario y contraseña).
4. Abre `Encuestasopm.csproj` en Visual Studio, restaura paquetes NuGet y compila.
5. En IIS da permiso de **escritura** sobre `App_Data` a la identidad del Application Pool (ahí se guardan adjuntos y la bitácora de errores `App_Data/logs`).
6. Entra a `Login.aspx` con **usuario `admin` / contraseña `Ocosingo2026!`** y **cámbiala de inmediato** (menú Administrador → Cambiar contraseña).
7. Con HTTPS activo, pon `requireSSL="true"` en `<forms>` del `Web.config`.

**Pendientes del Ayuntamiento**
- Completar los datos entre `[corchetes]` en `AvisoPrivacidad.aspx` y que lo valide la Unidad de Transparencia.
- Agregar domicilio y teléfono oficiales en el pie de `Site.Master`.
- Revisar nombres y descripciones de planes y programas (tabla `planesprogramas`). Se pueden editar en la BD sin tocar el código, y con `activo = 0` se ocultan.

Para agregar más usuarios del panel, por ahora se insertan con el mismo procedimiento que usa el script (hash PBKDF2); conviene agregar una pantalla de administración de usuarios en una siguiente versión.

---

## 4. Estructura

```
Default.aspx / .cs           Portada, modal de bienvenida y encuesta
AvisoPrivacidad.aspx         Aviso de privacidad (plantilla)
Login.aspx / .cs             Acceso al panel
Encuesta.ashx / .cs          Recibe y valida la encuesta (JSON)
Site.Master                  Plantilla pública
Site1.Master                 Plantilla del panel
admin/Panel.aspx             Resumen estadístico
admin/Reporte.aspx           Reporte imprimible
admin/Datos.ashx             Datos del panel, CSV, adjuntos y cambio de contraseña
Datos/                       Acceso a datos, seguridad y utilerías
css/ js/ lib/ img/           Estilos, scripts, Bootstrap 5.3 y Chart.js 4, imágenes optimizadas
BaseDatos/                   Scripts SQL
```

**Archivos del proyecto anterior que ya no se usan y se pueden eliminar:**
`Main.aspx`, `WebService1.asmx`, `Controladores/Controlador.cs`, `Modelos/combos.cs`, `Classconexion.cs`, `SQLHelper.cs`,
`Reportes.aspx`, `ReportesPlanes.aspx`, `Planes.xsd/.xsc/.xss/.Designer.cs`, `Reportes/*.rpt`, `assets/` completo,
`js/` y `css/` anteriores, `uploads/` (revisa primero si contiene archivos reales de ciudadanos) y la mayoría de `img/`.
También se quitaron las referencias a Crystal Reports y Newtonsoft.Json.
