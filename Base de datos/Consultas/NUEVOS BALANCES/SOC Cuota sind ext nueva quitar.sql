/*SOC_cuotas_sindicales_extraordinarias nuevas_quitar*/
USE SindQR
DECLARE @SOCIO AS varchar(5)
SET @SOCIO='6449'

DECLARE @autoriza AS varchar(6);
SET @autoriza ='RC';

DECLARE @movimiento AS varchar(8);
SET @movimiento ='CONVENIO'; --CANCELA/TRANSFE/CONVENIO

/*para saber el total de cuotas sindicales extraordinaras*/
SELECT  COUNT(*) AS cant ,SUM(exist * precioconiva) AS importe_CUOTAS_SINDICALES_EXTRAORDINARIAS FROM  inventario WHERE Id_Familia = '123' AND id_proveedor = @SOCIO AND exist > 0 --CUOTAS SINDICALES EXTRAORDINARIAS

BEGIN TRANSACTION
/*ponemos en 0 la existencia de cada registro de cuota sindical extraordinaria que tenga existencia*/
UPDATE Inventario SET Exist=0 ,F_CubiertoHasta = CONVERT(varchar(8) , GETDATE() , 112),Id_UsuarioEdit=@autoriza, Color=@movimiento WHERE Id_Familia = '123' AND id_proveedor = @SOCIO AND exist > 0 --CUOTAS SINDICALES EXTRAORDINARIAS

/*verificamos el total de cuotas sindicales extraordinarias a cobrar*/
SELECT  COUNT(*) AS cant ,SUM(exist * precioconiva) AS importe_CUOTAS_SINDICALES_EXTRAORDINARIAS FROM  inventario WHERE Id_Familia = '123' AND id_proveedor = @SOCIO AND exist > 0 --CUOTAS SINDICALES EXTRAORDINARIAS

--COMMIT TRAN
--ROLLBACK TRAN