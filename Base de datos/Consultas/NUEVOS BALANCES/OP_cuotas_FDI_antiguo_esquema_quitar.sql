/*OP_cuotas_FDI_esquema_antiguo_quitar*/
USE SindQR;
DECLARE @gafete AS varchar(10);
SET @gafete = '25650';
SET @gafete = 'OP-'+@gafete;

DECLARE @autoriza AS varchar(6);
SET @autoriza ='JDA';

DECLARE @movimiento AS varchar(8);
SET @movimiento ='CONVENIO'; --CANCELA/TRANSFE/CONVENIO


/*LAS FDI ESQUEMA ANTIGUO, NORMALMENTE SE VAN TODAS A CONVENIO Y NO SE COBRA NINGUNA EN EL MOMENTO
PERO SI NO SE VAN A IR TODAS A CONVENIOS ENTONCES HAY QUE REALIZAR EL PROCESO MANUALMENTE
HAY QUE ENTRAR AL EXPLORADOR DE OBJETOS DAR CLIC DERECHO SOBRE CUALQUIER TABLA Y
EN LA CONSULTA SQL PEGAR LA SIGUIENTE CONSULTA:

SELECT i.*
FROM dbo.Inventario i
WHERE i.Id_Familia = '28'
      AND i.Id_Proveedor = '23257'
      AND i.Exist > 0
ORDER BY i.FechaOp;

Y MANUALMENTE IR PONIENDO EN 0 LA EXISTENCIA DE CADA REGISTRO HASTA DEJAR SOLO LOS QUE SE VAN A COBRAR
*/

/*para saber el total de cuotas de FDI Esquema Antiguo*/
SELECT COUNT(*) AS cant , SUM(exist * precioconiva) AS importe FROM inventario WHERE Id_Familia='28' AND id_proveedor=@gafete AND exist>0;

BEGIN TRAN;
/*ponemos en 0 la existencia de cada registro de Cuota FDI Esquema Antiguo que tenga existencia*/
UPDATE Inventario SET Exist = 0 ,  F_CubiertoHasta = CONVERT(varchar(8) , GETDATE() , 112),Id_UsuarioEdit=@autoriza, Color=@movimiento WHERE Id_Familia='28' AND id_proveedor=@gafete AND exist>0;

/*verificamos el total de cuotas de FDI Esquema Antiguo a cobrar*/
SELECT COUNT(*) AS cant , SUM(exist * precioconiva) AS importe FROM inventario WHERE Id_Familia='28' AND id_proveedor=@gafete AND exist>0;

--COMMIT TRAN
--ROLLBACK TRAN