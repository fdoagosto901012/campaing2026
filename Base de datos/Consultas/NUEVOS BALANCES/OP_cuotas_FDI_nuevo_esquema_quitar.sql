/*OP_cuotas_FDI_nuevo_esquema_quitar*/
USE SindQR;
DECLARE @gafete AS varchar(10);
SET @gafete = '55422';
SET @gafete = 'OP-'+@gafete;

DECLARE @autoriza AS varchar(6);
SET @autoriza ='JKH';

DECLARE @movimiento AS varchar(8);
SET @movimiento ='RENUNCIA'; --CANCELA/TRANSFE/CONVENIO

/*para saber el total de cuotas de FDI Nuevo Esquema*/
SELECT COUNT(*) AS cant , SUM(exist * precioconiva) AS importe FROM inventario WHERE Id_Familia='105' AND id_proveedor=@gafete AND exist>0;

BEGIN TRAN;
/*ponemos en 0 la existencia de cada registro de Cuota FDI Nuevo Esquema que tenga existencia*/
UPDATE Inventario SET Exist = 0 , F_CubiertoHasta = CONVERT(varchar(8) , GETDATE() , 112),Id_UsuarioEdit=@autoriza, Color=@movimiento WHERE Id_Familia='105' AND id_proveedor=@gafete AND exist>0;

/*verificamos el total de cuotas de FDI Nuevo Esquema a cobrar*/
SELECT COUNT(*) AS cant , SUM(exist * precioconiva) AS importe FROM inventario WHERE Id_Familia='105' AND id_proveedor=@gafete AND exist>0;

--COMMIT TRAN
--ROLLBACK TRAN 