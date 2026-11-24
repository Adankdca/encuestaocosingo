/* =====================================================================================
   OPCIONAL - Genera 400 encuestas ficticias para probar el panel y el reporte.
   NO lo ejecutes en producción. Para borrar los datos de prueba al terminar:
       DELETE FROM dbo.preguntas WHERE problematica LIKE N'[[]PRUEBA]%';
   ===================================================================================== */
USE [BDPlanesprogramas]
GO
SET NOCOUNT ON;

DECLARE @i int = 0, @r int, @loc nvarchar(100);
DECLARE @localidades TABLE(id int IDENTITY(1,1), nombre nvarchar(100));
INSERT @localidades(nombre) VALUES (N'Barrio de prueba 1'),(N'Barrio de prueba 2'),(N'Barrio de prueba 3'),
       (N'Colonia de prueba 1'),(N'Colonia de prueba 2'),(N'Localidad de prueba A'),(N'Localidad de prueba B'),
       (N'Localidad de prueba C'),(N'Localidad de prueba D'),(N'Localidad de prueba E');

WHILE @i < 400
BEGIN
    SET @r = ABS(CHECKSUM(NEWID()));
    SET @loc = (SELECT nombre FROM @localidades WHERE id = 1 + @r % 10);
    INSERT dbo.preguntas(idplanes, Nombre, Genero, idedad, idocupacion, residente, zona, localidad,
                         existencia, apoyo, inseguridad, bienestar, abandono, economico, desatencion, servicios,
                         problematica, propuesta, idconfianza, fechaencuesta)
    VALUES (
        1 + ABS(CHECKSUM(NEWID())) % 12,
        CASE WHEN @r % 3 = 0 THEN NULL ELSE N'Ciudadano de prueba ' + CAST(@i AS nvarchar(10)) END,
        CASE ABS(CHECKSUM(NEWID())) % 20 WHEN 0 THEN 'O' WHEN 1 THEN 'M' WHEN 2 THEN 'M' WHEN 3 THEN 'M'
             WHEN 4 THEN 'M' WHEN 5 THEN 'M' WHEN 6 THEN 'M' WHEN 7 THEN 'M' WHEN 8 THEN 'M' ELSE 'F' END,
        1 + ABS(CHECKSUM(NEWID())) % 5,
        1 + ABS(CHECKSUM(NEWID())) % 13,
        CASE WHEN ABS(CHECKSUM(NEWID())) % 10 = 0 THEN 'NO' ELSE 'SI' END,
        CASE WHEN ABS(CHECKSUM(NEWID())) % 5 < 3 THEN 'R' ELSE 'C' END,
        @loc,
        CASE WHEN ABS(CHECKSUM(NEWID())) % 10 < 4 THEN 'SI' ELSE 'NO' END,
        CASE WHEN ABS(CHECKSUM(NEWID())) % 10 < 3 THEN 'SI' ELSE 'NO' END,
        CASE WHEN ABS(CHECKSUM(NEWID())) % 10 < 6 THEN 1 ELSE 0 END,
        CASE WHEN ABS(CHECKSUM(NEWID())) % 10 < 4 THEN 1 ELSE 0 END,
        CASE WHEN ABS(CHECKSUM(NEWID())) % 10 < 3 THEN 1 ELSE 0 END,
        CASE WHEN ABS(CHECKSUM(NEWID())) % 10 < 5 THEN 1 ELSE 0 END,
        CASE WHEN ABS(CHECKSUM(NEWID())) % 10 < 2 THEN 1 ELSE 0 END,
        CASE WHEN ABS(CHECKSUM(NEWID())) % 10 < 7 THEN 1 ELSE 0 END,
        N'[PRUEBA] Problemática de ejemplo número ' + CAST(@i AS nvarchar(10)),
        CASE WHEN @r % 2 = 0 THEN N'Propuesta de ejemplo número ' + CAST(@i AS nvarchar(10)) ELSE NULL END,
        CASE WHEN ABS(CHECKSUM(NEWID())) % 10 < 2 THEN 1 WHEN ABS(CHECKSUM(NEWID())) % 10 < 6 THEN 2 ELSE 3 END,
        DATEADD(MINUTE, -(ABS(CHECKSUM(NEWID())) % (60 * 24 * 300)), GETDATE())
    );
    SET @i += 1;
END
PRINT 'Listo: 400 encuestas de prueba.';
GO
