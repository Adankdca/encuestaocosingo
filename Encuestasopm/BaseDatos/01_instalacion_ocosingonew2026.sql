/* =====================================================================================
   CONSULTA CIUDADANA PARTICIPATIVA - H. AYUNTAMIENTO DE OCOSINGO, CHIAPAS 2024-2027
   Script de instalación limpia de la base de datos  (SQL Server 2014 o superior)

   ADVERTENCIA: este script ELIMINA y vuelve a crear las tablas y procedimientos del
   sistema. Los registros de prueba de Chiapa de Corzo se pierden. Si necesitas
   conservarlos, respalda la base antes de ejecutarlo.
   ===================================================================================== */
USE [master]
GO
IF DB_ID(N'BDEncuestaOcosingo') IS NULL
    CREATE DATABASE [BDEncuestaOcosingo];
GO
USE [BDEncuestaOcosingo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ---------------------------------------------------------------------------
   1. Limpieza de objetos anteriores (incluye restos de otros sistemas)
   --------------------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.Get_Rpt_Planes', N'P') IS NOT NULL DROP PROCEDURE dbo.Get_Rpt_Planes;
IF OBJECT_ID(N'dbo.Get_Rpt_Temas', N'P') IS NOT NULL DROP PROCEDURE dbo.Get_Rpt_Temas;
IF OBJECT_ID(N'dbo.Insert_Encuesta', N'P') IS NOT NULL DROP PROCEDURE dbo.Insert_Encuesta;
IF OBJECT_ID(N'dbo.SPObtenerOcupacion', N'P') IS NOT NULL DROP PROCEDURE dbo.SPObtenerOcupacion;
IF OBJECT_ID(N'dbo.SPObtenerplanprograma', N'P') IS NOT NULL DROP PROCEDURE dbo.SPObtenerplanprograma;
IF OBJECT_ID(N'dbo.Update_documento', N'P') IS NOT NULL DROP PROCEDURE dbo.Update_documento;
IF OBJECT_ID(N'dbo.SPObtenerCatalogos', N'P') IS NOT NULL DROP PROCEDURE dbo.SPObtenerCatalogos;
IF OBJECT_ID(N'dbo.Get_Panel_Resumen', N'P') IS NOT NULL DROP PROCEDURE dbo.Get_Panel_Resumen;
IF OBJECT_ID(N'dbo.Get_Encuestas_Listado', N'P') IS NOT NULL DROP PROCEDURE dbo.Get_Encuestas_Listado;
IF OBJECT_ID(N'dbo.Get_Encuesta_Archivo', N'P') IS NOT NULL DROP PROCEDURE dbo.Get_Encuesta_Archivo;
IF OBJECT_ID(N'dbo.usp_Usuario_Obtener', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Usuario_Obtener;
IF OBJECT_ID(N'dbo.usp_Usuario_RegistrarAcceso', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Usuario_RegistrarAcceso;
IF OBJECT_ID(N'dbo.usp_Usuario_CambiarPassword', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Usuario_CambiarPassword;
GO
IF OBJECT_ID(N'dbo.preguntas', N'U') IS NOT NULL DROP TABLE dbo.preguntas;
IF OBJECT_ID(N'dbo.planesprogramas', N'U') IS NOT NULL DROP TABLE dbo.planesprogramas;
IF OBJECT_ID(N'dbo.tipoplanesprogramas', N'U') IS NOT NULL DROP TABLE dbo.tipoplanesprogramas;
IF OBJECT_ID(N'dbo.Edades', N'U') IS NOT NULL DROP TABLE dbo.Edades;
IF OBJECT_ID(N'dbo.Ocupaciones', N'U') IS NOT NULL DROP TABLE dbo.Ocupaciones;
IF OBJECT_ID(N'dbo.Confianza', N'U') IS NOT NULL DROP TABLE dbo.Confianza;
IF OBJECT_ID(N'dbo.Temagobierno', N'U') IS NOT NULL DROP TABLE dbo.Temagobierno;
IF OBJECT_ID(N'dbo.tabla1', N'U') IS NOT NULL DROP TABLE dbo.tabla1;
IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NOT NULL DROP TABLE dbo.Usuarios;
GO

/* ---------------------------------------------------------------------------
   2. Catálogos
   --------------------------------------------------------------------------- */
CREATE TABLE dbo.tipoplanesprogramas(
    idtipo  int          NOT NULL CONSTRAINT PK_tipoplanesprogramas PRIMARY KEY,
    nombre  nvarchar(30) NOT NULL
);

CREATE TABLE dbo.planesprogramas(
    idplanes    int           NOT NULL CONSTRAINT PK_planesprogramas PRIMARY KEY,
    clave       nvarchar(10)  NOT NULL,
    idtipo      int           NOT NULL CONSTRAINT FK_planes_tipo REFERENCES dbo.tipoplanesprogramas(idtipo),
    nombre      nvarchar(150) NOT NULL,
    nombrecorto nvarchar(40)  NOT NULL,
    descripcion nvarchar(600) NULL,
    icono       nvarchar(60)  NULL,
    orden       int           NOT NULL CONSTRAINT DF_planes_orden DEFAULT(0),
    activo      bit           NOT NULL CONSTRAINT DF_planes_activo DEFAULT(1)
);

CREATE TABLE dbo.Edades(
    idedad int          NOT NULL CONSTRAINT PK_Edades PRIMARY KEY,
    edades nvarchar(30) NOT NULL
);

CREATE TABLE dbo.Ocupaciones(
    idocupaciones int          NOT NULL CONSTRAINT PK_Ocupaciones PRIMARY KEY,
    ocupacion     nvarchar(80) NOT NULL,
    orden         int          NOT NULL CONSTRAINT DF_ocup_orden DEFAULT(0)
);

CREATE TABLE dbo.Confianza(
    idconfianza int          NOT NULL CONSTRAINT PK_Confianza PRIMARY KEY,
    descripcion nvarchar(20) NOT NULL,
    valor       decimal(5,2) NOT NULL   -- puntaje para el índice de confianza (0 a 100)
);

CREATE TABLE dbo.Temagobierno(
    idtemagobierno int           NOT NULL CONSTRAINT PK_Temagobierno PRIMARY KEY,
    clave          varchar(20)   NOT NULL,  -- nombre de la columna en dbo.preguntas
    descripcion    nvarchar(100) NOT NULL
);
GO

/* ---------------------------------------------------------------------------
   3. Encuestas (se conserva el nombre de tabla "preguntas")
   --------------------------------------------------------------------------- */
CREATE TABLE dbo.preguntas(
    idpregunta      int IDENTITY(1,1) NOT NULL CONSTRAINT PK_preguntas PRIMARY KEY,
    folio           AS ('OCO-' + RIGHT('000000' + CAST(idpregunta AS varchar(10)), 6)) PERSISTED,
    idplanes        int            NOT NULL CONSTRAINT FK_preguntas_planes REFERENCES dbo.planesprogramas(idplanes),
    Nombre          nvarchar(150)  NULL,      -- opcional: el ciudadano puede participar de forma anónima
    Genero          char(1)        NOT NULL CONSTRAINT CK_preguntas_genero CHECK (Genero IN ('M','F','O')),
    idedad          int            NOT NULL CONSTRAINT FK_preguntas_edad REFERENCES dbo.Edades(idedad),
    idocupacion     int            NOT NULL CONSTRAINT FK_preguntas_ocup REFERENCES dbo.Ocupaciones(idocupaciones),
    residente       char(2)        NOT NULL CONSTRAINT CK_preguntas_residente CHECK (residente IN ('SI','NO')),
    zona            char(1)        NULL     CONSTRAINT CK_preguntas_zona CHECK (zona IN ('C','R')),   -- C = Cabecera, R = Rural
    localidad       nvarchar(150)  NULL,
    existencia      char(2)        NOT NULL CONSTRAINT CK_preguntas_exist CHECK (existencia IN ('SI','NO')),
    apoyo           char(2)        NOT NULL CONSTRAINT CK_preguntas_apoyo CHECK (apoyo IN ('SI','NO')),
    inseguridad     bit            NOT NULL CONSTRAINT DF_p_inseg DEFAULT(0),
    bienestar       bit            NOT NULL CONSTRAINT DF_p_bien  DEFAULT(0),
    abandono        bit            NOT NULL CONSTRAINT DF_p_aband DEFAULT(0),
    economico       bit            NOT NULL CONSTRAINT DF_p_econ  DEFAULT(0),
    desatencion     bit            NOT NULL CONSTRAINT DF_p_desat DEFAULT(0),
    servicios       bit            NOT NULL CONSTRAINT DF_p_serv  DEFAULT(0),
    problematica    nvarchar(2000) NOT NULL,
    propuesta       nvarchar(2000) NULL,
    archivo         nvarchar(260)  NULL,      -- nombre con el que se guardó en ~/App_Data/adjuntos
    archivooriginal nvarchar(260)  NULL,      -- nombre original que subió el ciudadano
    idconfianza     int            NOT NULL CONSTRAINT FK_preguntas_conf REFERENCES dbo.Confianza(idconfianza),
    aceptaaviso     bit            NOT NULL CONSTRAINT DF_p_aviso DEFAULT(1),
    fechaencuesta   datetime       NOT NULL CONSTRAINT DF_p_fecha DEFAULT(GETDATE())
);
CREATE INDEX IX_preguntas_fecha ON dbo.preguntas(fechaencuesta) INCLUDE (idplanes, idconfianza);
CREATE INDEX IX_preguntas_plan  ON dbo.preguntas(idplanes, fechaencuesta);
GO

/* ---------------------------------------------------------------------------
   4. Usuarios del panel (contraseña con PBKDF2-SHA256, nunca en texto plano)
   --------------------------------------------------------------------------- */
CREATE TABLE dbo.Usuarios(
    idusuario    int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Usuarios PRIMARY KEY,
    usuario      nvarchar(50)   NOT NULL CONSTRAINT UQ_Usuarios_usuario UNIQUE,
    nombre       nvarchar(120)  NOT NULL,
    passhash     varbinary(32)  NOT NULL,
    salt         varbinary(16)  NOT NULL,
    iteraciones  int            NOT NULL,
    activo       bit            NOT NULL CONSTRAINT DF_Usuarios_activo DEFAULT(1),
    ultimoacceso datetime       NULL
);
GO

/* ---------------------------------------------------------------------------
   5. Datos de catálogos
   --------------------------------------------------------------------------- */
INSERT dbo.tipoplanesprogramas(idtipo, nombre) VALUES (1, N'PLAN'), (2, N'PROGRAMA');

INSERT dbo.planesprogramas(idplanes, clave, idtipo, nombre, nombrecorto, icono, orden, descripcion) VALUES
 (1,  N'PLAN1', 1, N'Plan Municipal de Desarrollo',        N'Desarrollo Municipal', N'1p.png',    1, N'Establece los objetivos, estrategias y prioridades del Ayuntamiento para guiar el desarrollo social, económico y ambiental de Ocosingo.'),
 (2,  N'PLAN2', 1, N'Plan Municipal de Desarrollo Urbano', N'Desarrollo Urbano',    N'2p.png',    2, N'Ordena el crecimiento de la cabecera y de las localidades: uso de suelo, vialidades, servicios e infraestructura, en coordinación con los gobiernos federal y estatal.'),
 (3,  N'PLAN3', 1, N'Plan Municipal de Salud',             N'Salud',                N'3p.png',    3, N'Prioriza la atención a la salud de la población, la prevención y la coordinación con las instituciones de salud y asistencia social.'),
 (4,  N'PLAN4', 1, N'Plan Municipal de Vivienda',          N'Vivienda',             N'4p.png',    4, N'Busca garantizar el acceso a una vivienda digna y de calidad de acuerdo con las necesidades de cada familia.'),
 (5,  N'PLAN5', 1, N'Plan Municipal Educativo',            N'Educación',            N'5p.png',    5, N'Genera condiciones favorables para que niñas, niños y jóvenes aprendan y obtengan herramientas para la vida.'),
 (6,  N'PLAN6', 1, N'Plan Municipal Económico',            N'Economía',             N'6p.png',    6, N'Impulsa la actividad económica, el empleo y la producción local, en armonía con las necesidades básicas de la comunidad.'),
 (7,  N'PROG1', 2, N'Barriguita Llena',                    N'Barriguita Llena',     N'1prog.png', 7, N'Contribuye a que la población en edad escolar tenga acceso a alimentos nutritivos mediante la entrega de comidas calientes.'),
 (8,  N'PROG2', 2, N'Apoyos Productivos',                  N'Apoyos Productivos',   N'2prog.png', 8, N'Apoya proyectos productivos sustentables mediante la entrega de recursos e insumos.'),
 (9,  N'PROG3', 2, N'Programa de Apoyos DIF',              N'Apoyos DIF',           N'3prog.png', 9, N'Apoya a los sectores que más lo necesitan con insumos para alimentar, abrigar y proteger a niñas y niños.'),
 (10, N'PROG4', 2, N'Becas Escolares',                     N'Becas Escolares',      N'4prog.png', 10, N'Fomenta la permanencia escolar y el rendimiento de estudiantes de escasos recursos de primaria y secundaria, rurales y urbanos.'),
 (11, N'PROG5', 2, N'Asuntos Jurídicos',                   N'Asuntos Jurídicos',    N'5prog.png', 11, N'Brinda asesoría jurídica gratuita a la ciudadanía.'),
 (12, N'PROG6', 2, N'Otros Programas',                     N'Otros Programas',      N'6prog.png', 12, N'Otros programas en beneficio de la comunidad.');

INSERT dbo.Edades(idedad, edades) VALUES
 (1, N'Menor de 18'), (2, N'18 a 29'), (3, N'30 a 44'), (4, N'45 a 59'), (5, N'60 o más');

INSERT dbo.Ocupaciones(idocupaciones, ocupacion, orden) VALUES
 (1,  N'Estudiante', 1),
 (2,  N'Campesino(a) / Agricultor(a)', 2),
 (3,  N'Ama / Amo de casa', 3),
 (4,  N'Comerciante', 4),
 (5,  N'Empleado(a)', 5),
 (6,  N'Oficio (albañil, carpintero, etc.)', 6),
 (7,  N'Profesionista', 7),
 (8,  N'Docente', 8),
 (9,  N'Personal de salud', 9),
 (10, N'Servidor(a) público(a)', 10),
 (11, N'Jubilado(a) / Pensionado(a)', 11),
 (12, N'Sin empleo', 12),
 (13, N'Otro', 13);

INSERT dbo.Confianza(idconfianza, descripcion, valor) VALUES
 (1, N'Mucha', 100), (2, N'Poca', 50), (3, N'Nada', 0);

INSERT dbo.Temagobierno(idtemagobierno, clave, descripcion) VALUES
 (1, 'inseguridad', N'Inseguridad y corrupción'),
 (2, 'bienestar',   N'Falta de bienestar social'),
 (3, 'abandono',    N'Abandono educativo'),
 (4, 'economico',   N'Escaso desarrollo económico'),
 (5, 'desatencion', N'Desatención al medio ambiente'),
 (6, 'servicios',   N'Servicios públicos (agua, drenaje, caminos)');

/* Usuario inicial del panel:  usuario = admin   contraseña = Ocosingo2026!
   CÁMBIALA en el primer acceso (menú del usuario > Cambiar contraseña).          */
INSERT dbo.Usuarios(usuario, nombre, passhash, salt, iteraciones)
VALUES (N'admin', N'Administrador',
        0x5FF549381126846BB190B23539683B776EE2872D73DDEC5C4567E3BA820F0B5F,
        0x1CAA0B72F21BEC1752F722045BBAC3C7,
        100000);
GO

/* ---------------------------------------------------------------------------
   6. Procedimientos de la encuesta pública
   --------------------------------------------------------------------------- */
CREATE PROCEDURE dbo.SPObtenerCatalogos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.idplanes, p.idtipo, t.nombre AS tipo, p.nombre, p.nombrecorto, p.descripcion, p.icono
    FROM dbo.planesprogramas p
    JOIN dbo.tipoplanesprogramas t ON t.idtipo = p.idtipo
    WHERE p.activo = 1
    ORDER BY p.idtipo, p.orden;

    SELECT idedad AS id, edades AS nombre FROM dbo.Edades ORDER BY idedad;
    SELECT idocupaciones AS id, ocupacion AS nombre FROM dbo.Ocupaciones ORDER BY orden;
    SELECT idconfianza AS id, descripcion AS nombre FROM dbo.Confianza ORDER BY idconfianza;
    SELECT clave AS id, descripcion AS nombre FROM dbo.Temagobierno ORDER BY idtemagobierno;
END
GO

CREATE PROCEDURE dbo.Insert_Encuesta
      @idplanes        int
    , @Nombre          nvarchar(150)
    , @Genero          char(1)
    , @idedad          int
    , @idocupacion     int
    , @residente       char(2)
    , @zona            char(1)
    , @localidad       nvarchar(150)
    , @existencia      char(2)
    , @apoyo           char(2)
    , @inseguridad     bit
    , @bienestar       bit
    , @abandono        bit
    , @economico       bit
    , @desatencion     bit
    , @servicios       bit
    , @problematica    nvarchar(2000)
    , @propuesta       nvarchar(2000)
    , @archivo         nvarchar(260)
    , @archivooriginal nvarchar(260)
    , @idconfianza     int
    , @idpregunta      int         OUTPUT
    , @folio           varchar(20) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.preguntas
        (idplanes, Nombre, Genero, idedad, idocupacion, residente, zona, localidad, existencia, apoyo,
         inseguridad, bienestar, abandono, economico, desatencion, servicios,
         problematica, propuesta, archivo, archivooriginal, idconfianza, aceptaaviso, fechaencuesta)
    VALUES
        (@idplanes, NULLIF(LTRIM(RTRIM(@Nombre)), N''), @Genero, @idedad, @idocupacion, @residente, @zona,
         NULLIF(LTRIM(RTRIM(@localidad)), N''), @existencia, @apoyo,
         @inseguridad, @bienestar, @abandono, @economico, @desatencion, @servicios,
         @problematica, NULLIF(LTRIM(RTRIM(@propuesta)), N''), @archivo, @archivooriginal, @idconfianza, 1, GETDATE());

    SET @idpregunta = SCOPE_IDENTITY();
    SELECT @folio = folio FROM dbo.preguntas WHERE idpregunta = @idpregunta;
END
GO

/* ---------------------------------------------------------------------------
   7. Panel de control y reporte
      Todos los filtros son opcionales (NULL = sin filtro).
   --------------------------------------------------------------------------- */
CREATE PROCEDURE dbo.Get_Panel_Resumen
      @desde    date = NULL
    , @hasta    date = NULL
    , @idtipo   int  = NULL
    , @idplanes int  = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.idpregunta, p.idplanes, pp.idtipo, p.Genero, p.idedad, p.idocupacion, p.residente, p.zona,
           p.localidad, p.existencia, p.apoyo, p.inseguridad, p.bienestar, p.abandono, p.economico,
           p.desatencion, p.servicios, p.propuesta, p.archivo, p.idconfianza, p.fechaencuesta
    INTO #f
    FROM dbo.preguntas p
    JOIN dbo.planesprogramas pp ON pp.idplanes = p.idplanes
    WHERE (@desde    IS NULL OR p.fechaencuesta >= @desde)
      AND (@hasta    IS NULL OR p.fechaencuesta <  DATEADD(DAY, 1, @hasta))
      AND (@idtipo   IS NULL OR pp.idtipo   = @idtipo)
      AND (@idplanes IS NULL OR p.idplanes  = @idplanes)
    OPTION (RECOMPILE);

    DECLARE @total int = (SELECT COUNT(*) FROM #f);
    DECLARE @ref date  = ISNULL(@hasta, CAST(GETDATE() AS date));

    /* 1. Indicadores generales */
    SELECT
        @total                                                                    AS total,
        SUM(CASE WHEN f.fechaencuesta >= DATEADD(DAY, -29, @ref)
                  AND f.fechaencuesta <  DATEADD(DAY, 1, @ref) THEN 1 ELSE 0 END)  AS ultimos30,
        SUM(CASE WHEN f.existencia = 'SI' THEN 1 ELSE 0 END)                       AS conocen,
        SUM(CASE WHEN f.apoyo      = 'SI' THEN 1 ELSE 0 END)                       AS conapoyo,
        SUM(CASE WHEN f.residente  = 'SI' THEN 1 ELSE 0 END)                       AS residentes,
        SUM(CASE WHEN f.zona       = 'R'  THEN 1 ELSE 0 END)                       AS rurales,
        SUM(CASE WHEN f.zona       = 'C'  THEN 1 ELSE 0 END)                       AS cabecera,
        SUM(CASE WHEN f.propuesta IS NOT NULL THEN 1 ELSE 0 END)                   AS conpropuesta,
        SUM(CASE WHEN f.archivo   IS NOT NULL THEN 1 ELSE 0 END)                   AS conarchivo,
        CAST(ISNULL(AVG(c.valor), 0) AS decimal(5,1))                              AS indiceconfianza,
        MIN(f.fechaencuesta)                                                       AS primera,
        MAX(f.fechaencuesta)                                                       AS ultima
    FROM #f f
    LEFT JOIN dbo.Confianza c ON c.idconfianza = f.idconfianza;

    /* 2. Participación por plan o programa (incluye los que no tienen respuestas) */
    SELECT pp.idplanes, pp.idtipo, t.nombre AS tipo, pp.nombre, pp.nombrecorto,
           COUNT(f.idpregunta)                                        AS total,
           SUM(CASE WHEN f.existencia = 'SI' THEN 1 ELSE 0 END)       AS conocen,
           SUM(CASE WHEN f.apoyo      = 'SI' THEN 1 ELSE 0 END)       AS conapoyo,
           CAST(ISNULL(AVG(c.valor), 0) AS decimal(5,1))              AS indiceconfianza
    FROM dbo.planesprogramas pp
    JOIN dbo.tipoplanesprogramas t ON t.idtipo = pp.idtipo
    LEFT JOIN #f f             ON f.idplanes = pp.idplanes
    LEFT JOIN dbo.Confianza c  ON c.idconfianza = f.idconfianza
    WHERE pp.activo = 1
      AND (@idtipo   IS NULL OR pp.idtipo   = @idtipo)
      AND (@idplanes IS NULL OR pp.idplanes = @idplanes)
    GROUP BY pp.idplanes, pp.idtipo, t.nombre, pp.nombre, pp.nombrecorto, pp.orden
    ORDER BY COUNT(f.idpregunta) DESC, pp.orden;

    /* 3. Temas prioritarios */
    SELECT tg.clave AS id, tg.descripcion AS nombre, ISNULL(x.total, 0) AS total
    FROM dbo.Temagobierno tg
    LEFT JOIN (
        SELECT v.clave, SUM(v.marcado) AS total
        FROM #f f
        CROSS APPLY (VALUES ('inseguridad', CAST(f.inseguridad AS int)),
                            ('bienestar',   CAST(f.bienestar   AS int)),
                            ('abandono',    CAST(f.abandono    AS int)),
                            ('economico',   CAST(f.economico   AS int)),
                            ('desatencion', CAST(f.desatencion AS int)),
                            ('servicios',   CAST(f.servicios   AS int))) v(clave, marcado)
        GROUP BY v.clave
    ) x ON x.clave = tg.clave
    ORDER BY ISNULL(x.total, 0) DESC;

    /* 4. Confianza en el gobierno */
    SELECT c.idconfianza AS id, c.descripcion AS nombre, COUNT(f.idpregunta) AS total
    FROM dbo.Confianza c LEFT JOIN #f f ON f.idconfianza = c.idconfianza
    GROUP BY c.idconfianza, c.descripcion ORDER BY c.idconfianza;

    /* 5. Rango de edad */
    SELECT e.idedad AS id, e.edades AS nombre, COUNT(f.idpregunta) AS total
    FROM dbo.Edades e LEFT JOIN #f f ON f.idedad = e.idedad
    GROUP BY e.idedad, e.edades ORDER BY e.idedad;

    /* 6. Género */
    SELECT g.id, g.nombre, COUNT(f.idpregunta) AS total
    FROM (VALUES ('F', N'Mujer', 1), ('M', N'Hombre', 2), ('O', N'Prefiere no decir', 3)) g(id, nombre, orden)
    LEFT JOIN #f f ON f.Genero = g.id
    GROUP BY g.id, g.nombre, g.orden ORDER BY g.orden;

    /* 7. Ocupación */
    SELECT o.idocupaciones AS id, o.ocupacion AS nombre, COUNT(f.idpregunta) AS total
    FROM dbo.Ocupaciones o LEFT JOIN #f f ON f.idocupacion = o.idocupaciones
    GROUP BY o.idocupaciones, o.ocupacion ORDER BY COUNT(f.idpregunta) DESC, o.idocupaciones;

    /* 8. Participación por mes */
    SELECT YEAR(f.fechaencuesta) AS anio, MONTH(f.fechaencuesta) AS mes, COUNT(*) AS total
    FROM #f f
    GROUP BY YEAR(f.fechaencuesta), MONTH(f.fechaencuesta)
    ORDER BY anio, mes;

    /* 9. Localidades con más participación (top 10) */
    SELECT TOP (10) UPPER(LTRIM(RTRIM(f.localidad))) AS nombre, COUNT(*) AS total
    FROM #f f
    WHERE f.localidad IS NOT NULL
    GROUP BY UPPER(LTRIM(RTRIM(f.localidad)))
    ORDER BY COUNT(*) DESC, nombre;

    DROP TABLE #f;
END
GO

CREATE PROCEDURE dbo.Get_Encuestas_Listado
      @desde    date          = NULL
    , @hasta    date          = NULL
    , @idtipo   int           = NULL
    , @idplanes int           = NULL
    , @buscar   nvarchar(100) = NULL
    , @top      int           = NULL      -- NULL = todas (exportación)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @patron nvarchar(110) = N'%' + NULLIF(LTRIM(RTRIM(@buscar)), N'') + N'%';

    SELECT TOP (ISNULL(@top, 2147483647))
        p.idpregunta, p.folio, p.fechaencuesta,
        t.nombre AS tipo, pp.nombre AS planprograma,
        p.Nombre AS nombre,
        CASE p.Genero WHEN 'F' THEN N'Mujer' WHEN 'M' THEN N'Hombre' ELSE N'Prefiere no decir' END AS genero,
        e.edades AS edad, o.ocupacion,
        p.residente,
        CASE p.zona WHEN 'C' THEN N'Cabecera municipal' WHEN 'R' THEN N'Localidad rural' ELSE N'' END AS zona,
        p.localidad, p.existencia AS conoce, p.apoyo,
        STUFF(
            CASE WHEN p.inseguridad = 1 THEN N', Inseguridad y corrupción'      ELSE N'' END +
            CASE WHEN p.bienestar   = 1 THEN N', Bienestar social'              ELSE N'' END +
            CASE WHEN p.abandono    = 1 THEN N', Abandono educativo'            ELSE N'' END +
            CASE WHEN p.economico   = 1 THEN N', Desarrollo económico'          ELSE N'' END +
            CASE WHEN p.desatencion = 1 THEN N', Medio ambiente'                ELSE N'' END +
            CASE WHEN p.servicios   = 1 THEN N', Servicios públicos'            ELSE N'' END, 1, 2, N'') AS temas,
        p.problematica, p.propuesta, p.archivo, p.archivooriginal,
        c.descripcion AS confianza
    FROM dbo.preguntas p
    JOIN dbo.planesprogramas     pp ON pp.idplanes      = p.idplanes
    JOIN dbo.tipoplanesprogramas t  ON t.idtipo         = pp.idtipo
    JOIN dbo.Edades              e  ON e.idedad         = p.idedad
    JOIN dbo.Ocupaciones         o  ON o.idocupaciones  = p.idocupacion
    JOIN dbo.Confianza           c  ON c.idconfianza    = p.idconfianza
    WHERE (@desde    IS NULL OR p.fechaencuesta >= @desde)
      AND (@hasta    IS NULL OR p.fechaencuesta <  DATEADD(DAY, 1, @hasta))
      AND (@idtipo   IS NULL OR pp.idtipo  = @idtipo)
      AND (@idplanes IS NULL OR p.idplanes = @idplanes)
      AND (@patron   IS NULL OR p.problematica LIKE @patron OR p.propuesta LIKE @patron
                             OR p.localidad LIKE @patron OR p.folio LIKE @patron OR p.Nombre LIKE @patron)
    ORDER BY p.fechaencuesta DESC
    OPTION (RECOMPILE);
END
GO

CREATE PROCEDURE dbo.Get_Encuesta_Archivo
    @idpregunta int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT archivo, archivooriginal FROM dbo.preguntas WHERE idpregunta = @idpregunta;
END
GO

/* ---------------------------------------------------------------------------
   8. Usuarios del panel
   --------------------------------------------------------------------------- */
CREATE PROCEDURE dbo.usp_Usuario_Obtener
    @usuario nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idusuario, usuario, nombre, passhash, salt, iteraciones
    FROM dbo.Usuarios
    WHERE usuario = @usuario AND activo = 1;
END
GO

CREATE PROCEDURE dbo.usp_Usuario_RegistrarAcceso
    @idusuario int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Usuarios SET ultimoacceso = GETDATE() WHERE idusuario = @idusuario;
END
GO

CREATE PROCEDURE dbo.usp_Usuario_CambiarPassword
      @idusuario   int
    , @passhash    varbinary(32)
    , @salt        varbinary(16)
    , @iteraciones int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Usuarios
       SET passhash = @passhash, salt = @salt, iteraciones = @iteraciones
     WHERE idusuario = @idusuario;
END
GO

/* ---------------------------------------------------------------------------
   9. Usuario de SQL para la aplicación (ajusta la contraseña antes de ejecutar)
      Solo necesita ejecutar procedimientos; no puede leer ni borrar tablas directo.
   --------------------------------------------------------------------------- */
/*
USE [master];
CREATE LOGIN [encuesta_app] WITH PASSWORD = N'Cambia-Esta-Contraseña-2026', CHECK_POLICY = ON;
USE [BDEncuestaOcosingo];
CREATE USER [encuesta_app] FOR LOGIN [encuesta_app];
GRANT EXECUTE ON SCHEMA::dbo TO [encuesta_app];
*/
