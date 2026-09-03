/*SOC_cuotas_sindicales_quitar*/
USE SindQR
DECLARE @SOCIO AS varchar(5)
SET @SOCIO='4135'

DECLARE @autoriza AS varchar(6);
SET @autoriza ='HNC';

DECLARE @movimiento AS varchar(8);
SET @movimiento ='CANCELA'; --CANCELA/TRANSFE/CONVENIO

/*para saber el total de cuotas de FDI Esquema Antiguo*/
SELECT  COUNT(*) AS cant ,SUM(exist * precioconiva) AS importe_CUOTAS_SINDICALES FROM  inventario WHERE Id_Familia = '12' AND id_proveedor = @SOCIO AND exist > 0 --CUOTAS SINDICALES

BEGIN TRANSACTION
/*ponemos en 0 la existencia de cada registro de Cuota FDI Esquema Antiguo que tenga existencia*/
UPDATE Inventario SET Exist=0 ,F_CubiertoHasta = CONVERT(varchar(8) , GETDATE() , 112),Id_UsuarioEdit=@autoriza, Color=@movimiento WHERE Id_Familia = '12' AND id_proveedor = @SOCIO AND exist > 0 --CUOTAS SINDICALES

/*verificamos el total de cuotas de FDI Esquema Antiguo a cobrar*/
SELECT  COUNT(*) AS cant ,SUM(exist * precioconiva) AS importe_CUOTAS_SINDICALES FROM  inventario WHERE Id_Familia = '12' AND id_proveedor = @SOCIO AND exist > 0 --CUOTAS SINDICALES

--COMMIT TRAN
--ROLLBACK TRAN

/*LAS CUOTAS SINDICALES, NORMALMENTE SE VAN TODAS A CONVENIO Y NO SE COBRA NINGUNA EN EL MOMENTO
PERO SI NO SE VAN A IR TODAS A CONVENIOS ENTONCES HAY QUE REALIZAR EL PROCESO MANUALMENTE
HAY QUE ENTRAR AL EXPLORADOR DE OBJETOS DAR CLIC DERECHO SOBRE CUALQUIER TABLA Y
EN LA CONSULTA SQL PEGAR LA SIGUIENTE CONSULTA:

SELECT i.*
FROM dbo.Inventario i
WHERE i.Id_Familia = '12'
      AND i.Id_Proveedor = 'AQUI EL NUMERO DE SOCIO'
      AND i.Exist > 0
ORDER BY i.FechaOp;

Y MANUALMENTE IR PONIENDO EN 0 LA EXISTENCIA DE CADA REGISTRO HASTA DEJAR SOLO LOS QUE SE VAN A COBRAR
*/