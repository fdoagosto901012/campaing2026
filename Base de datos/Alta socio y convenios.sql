/*Crear socio reemplazando existente */

select sp.numero,sp.nombre,isnull(sp.paterno,'') as paterno,isnull(sp.materno,'') as materno,isnull(sp.fraccionamiento,'') as fraccionamiento,isnull(sp.sm,'') as sm,isnull(sp.mz,'') as mz,isnull(sp.lote,'') as lote, isnull(sp.calle,'') as calle,isnull(sp.num,'') as num,isnull(sp.localidad,'') as localidad,isnull(sp.municipio,'') as municipio,isnull(sp.ubicacion,'') as ubicacion,isnull(sp.telefono,'') as telefono, isnull(sp.celular,'') as celular,isnull(sp.email,'') as email,isnull(sp.sexo,'') as sexo,sp.fechanacimiento,sp.fechaingreso from Soc_DatPersonales sp where sp.numero='5670'

EXECUTE AJR_ACTDATSOCIO 5670,'JESUS','PERERA','VALDEZ','GRAN SATA FE 2','321','024','016','GALICIA','016','CANCUN','BENITO JUAREZ','QUINTANA ROO','9982355141','9982355141','Y@F.COM','19860808','20240406','M'



/*Crear socio QUE NO EXISTE */

select sp.numero,sp.nombre,isnull(sp.paterno,'') as paterno,isnull(sp.materno,'') as materno,isnull(sp.fraccionamiento,'') as fraccionamiento,isnull(sp.sm,'') as sm,isnull(sp.mz,'') as mz,isnull(sp.lote,'') as lote, isnull(sp.calle,'') as calle,isnull(sp.num,'') as num,isnull(sp.localidad,'') as localidad,isnull(sp.municipio,'') as municipio,isnull(sp.ubicacion,'') as ubicacion,isnull(sp.telefono,'') as telefono, isnull(sp.celular,'') as celular,isnull(sp.email,'') as email,isnull(sp.sexo,'') as sexo,sp.fechanacimiento,sp.fechaingreso from Soc_DatPersonales sp where sp.numero='9000'

EXECUTE AJR_ALTASOCIOJR 9000,'JESUS','PERERA','VALDEZ','GRAN SANTA FE 2','321','24','016','GALICIA','016','CANCUN','BENITO JUAREZ','QUINTANA ROO','9982355141','9982355141','Y@U.COM','19860808','20240406','M'



/*Crear Convenio FDI ingreso*/

select * from Convenios where Id_Op='X'

select Id_Op,Folio,Id_Producto,Importe,Vence,Status From ConveniosDetalle where Id_Op='X'

SELECT * FROM SECRETARIAS order by secretaria

SELECT * FROM CONCEPTOSCONVENIOS order by concepto

SELECT DOCUMENTO, VALOR FROM CONSECUTIVOS WHERE DOCUMENTO='CONVENIOS'

exec sp_executesql N'UPDATE "SINDQR".."CONSECUTIVOS" SET "VALOR"=@P1 WHERE "DOCUMENTO"=@P2 AND "VALOR"=@P3',N'@P1 money,@P2 varchar(8000),@P3 money',$222071.0000,'CONVENIOS',$222070.0000

select id_conceptoconv,id_secretaria from ConceptosConveniosRelacion where id_conceptoconv='14'

SELECT NUMERO,NOMBRE+ ' ' + ISNULL(PATERNO,'') + ' ' + ISNULL(MATERNO,'') AS NOMBRE FROM SOC_DATPERSONALES WHERE NUMERO=5670

EXECUTE AJR_REGISTRACONVENIO '222071','15','FONDO DE DEFUNCIÓN','10-1','SISTEMAS','14','INSCR. FONDO DE DEF. DE SOC',2,'5670','JESUS  PERERA VALDEZ','SIND','SINDICATO DE CHOFERES,TAXISTAS Y SIMILARES, ANDRES QUINTANA ROO',300,1,7,'20240406',300,'20240406','17:03','VIGENTE','INGRESO FDI'

EXECUTE AJR_REGISTRACONVENIODET '222071',1,'23.222071.1.1',300,'20240406','VIGENTE'

SELECT C.Id_Op, C.Id_Secretaria, C.Secretaria, C.Id_Usuario, C.Usuario, C.Id_Concepto, C.Concepto, C.CargoA, C.CargoANum, C.CargoANombre, C.AfavordeNum, C.AfavordeNombre, C.Monto, C.Partidas, C.Plazo, C.PrimerVencimiento, C.ImportePago, C.FechaOp, C.HoraOp, C.FechaEdit, C.Id_UsuarioEdit, C.Status, C.Observaciones, C.TotalLetras, CD.ID_PRODUCTO,CD.IMPORTE FROM Convenios C INNER JOIN ConveniosDetalle CD ON CD.ID_OP=C.ID_OP where C.id_op='222071'



/*Crear Convenio incripcion socio*/

select * from Convenios where Id_Op='X'

select Id_Op,Folio,Id_Producto,Importe,Vence,Status From ConveniosDetalle where Id_Op='X'

SELECT * FROM SECRETARIAS order by secretaria

SELECT * FROM CONCEPTOSCONVENIOS order by concepto

SELECT DOCUMENTO, VALOR FROM CONSECUTIVOS WHERE DOCUMENTO='CONVENIOS'

exec sp_executesql N'UPDATE "SINDQR".."CONSECUTIVOS" SET "VALOR"=@P1 WHERE "DOCUMENTO"=@P2 AND "VALOR"=@P3',N'@P1 money,@P2 varchar(8000),@P3 money',$222072.0000,'CONVENIOS',$222071.0000

select id_conceptoconv,id_secretaria from ConceptosConveniosRelacion where id_conceptoconv='6'

SELECT NUMERO,NOMBRE+ ' ' + ISNULL(PATERNO,'') + ' ' + ISNULL(MATERNO,'') AS NOMBRE FROM SOC_DATPERSONALES WHERE NUMERO=5670

EXECUTE AJR_REGISTRACONVENIO '222072','5','TRABAJO','10-1','SISTEMAS','6','INSCRIPCION NUEVOS SOCIOS',2,'5670','JESUS  PERERA VALDEZ','SIND','SINDICATO DE CHOFERES,TAXISTAS Y SIMILARES, ANDRES QUINTANA ROO',5000,1,7,'20240406',5000,'20240406','17:05','VIGENTE','INCRIPCION SOCIO'

EXECUTE AJR_REGISTRACONVENIODET '222072',1,'23.222072.1.1',5000,'20240406','VIGENTE'

SELECT C.Id_Op, C.Id_Secretaria, C.Secretaria, C.Id_Usuario, C.Usuario, C.Id_Concepto, C.Concepto, C.CargoA, C.CargoANum, C.CargoANombre, C.AfavordeNum, C.AfavordeNombre, C.Monto, C.Partidas, C.Plazo, C.PrimerVencimiento, C.ImportePago, C.FechaOp, C.HoraOp, C.FechaEdit, C.Id_UsuarioEdit, C.Status, C.Observaciones, C.TotalLetras, CD.ID_PRODUCTO,CD.IMPORTE FROM Convenios C INNER JOIN ConveniosDetalle CD ON CD.ID_OP=C.ID_OP where C.id_op='222072'


