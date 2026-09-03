/*OP_permiso_quitar*/
USE SindQR;
DECLARE @gafete AS VARCHAR(15);

SET @gafete='43286';
SET @gafete='OP-'+@gafete;

/*verficamos que su ultima de fecha de reporte no sea una falta o un reporte diario
si es asi no podemos continuar con este script
*/
DECLARE @concepto AS varchar(1)
SET @concepto= ISNULL((SELECT TOP 1 ha.Concepto FROM dbo.HistAsistencias ha WHERE ha.Gafete=@gafete ORDER BY ha.FechaReporte DESC),'')
--SELECT @concepto
IF @concepto='' OR @concepto='1' OR @concepto='2' BEGIN
    SELECT 'ERROR: El ultimo reporte de este operador no es permiso, por lo cual no se puede continuar.'
    RETURN
END

/*mostramos hasta que dia es su ultimo reporte*/
SELECT TOP 1 'Tenia permiso hasta el: ' + CONVERT(varchar(8),ha.FechaReporte,112) FROM dbo.HistAsistencias ha WHERE ha.Gafete=@gafete ORDER BY ha.FechaReporte DESC

/*calulamos la fecha a la cual lo dejaremos
que simpre es la de un dia anterior a hoy
*/
DECLARE @dejaral AS datetime
SET @dejaral=GETDATE()-1
SET @dejaral=CONVERT(datetime,CONVERT(varchar(8),@dejaral,112))
SELECT 'Se deja al día: ' + CONVERT(varchar(8),@dejaral,112)

BEGIN TRANSACTION
/*borramos todos los registros donde la fecha de reporte
sea mayor a la fecha a dejar
*/
DELETE dbo.HistAsistencias WHERE dbo.HistAsistencias.Gafete=@gafete 
AND dbo.HistAsistencias.FechaReporte>@dejaral

/*verficamos como quedo su historico de asistencias*/
SELECT TOP 30 ha.* FROM dbo.HistAsistencias ha WHERE ha.Gafete=@gafete ORDER BY ha.FechaReporte DESC

--COMMIT TRANSACTION
--ROLLBACK TRANSACTION
