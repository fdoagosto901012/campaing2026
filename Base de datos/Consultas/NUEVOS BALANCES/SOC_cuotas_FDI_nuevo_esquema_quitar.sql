/*SOC_cuotas_FDI_nuevo_esquema_quitar*/
DECLARE @SOCIO AS varchar(5)
SET @SOCIO='6292'

DECLARE @autoriza AS varchar(6);
SET @autoriza ='JKH';

DECLARE @movimiento AS varchar(8);
SET @movimiento ='CANCELAR'; --CANCELA/TRANSFE/CONVENIO

/*para saber el total de cuotas de FDI Nuevo Esquema*/
SELECT  COUNT(*) AS cant ,SUM(exist * precioconiva) AS importe_CUOTAS_FDI_NUEVO_ESQUEMA FROM  inventario WHERE Id_Familia = '104' AND id_proveedor = @SOCIO AND exist > 0; --CUOTA DEFUNCION SOCIOS

BEGIN TRANSACTION
/*ponemos en 0 la existencia de cada registro de Cuota FDI Nuevo Esquema que tenga existencia*/
UPDATE Inventario SET Exist = 0 , F_CubiertoHasta = CONVERT(varchar(8) , GETDATE() , 112), Id_UsuarioEdit=@autoriza, Color=@movimiento 
WHERE Id_Familia = '104' AND id_proveedor = @SOCIO AND exist > 0; --CUOTA DEFUNCION SOCIOS

/*verificamos el total de cuotas de FDI Nuevo Esquema a cobrar*/
SELECT  COUNT(*) AS cant ,SUM(exist * precioconiva) AS importe_CUOTAS_FDI_NUEVO_ESQUEMA FROM  inventario WHERE Id_Familia = '104' AND id_proveedor = @SOCIO AND exist > 0; --CUOTA DEFUNCION SOCIOS

--COMMIT TRAN
--ROLLBACK TRAN