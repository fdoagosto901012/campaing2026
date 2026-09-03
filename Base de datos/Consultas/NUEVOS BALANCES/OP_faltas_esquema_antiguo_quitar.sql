/*OP_faltas_esquema_antiguo_quitar*/
USE SindQR;
DECLARE @gafete AS VARCHAR(15);
DECLARE @faltas_a_cobrar AS MONEY;

SET @gafete='21384';
SET @gafete='OP-'+@gafete;

SET @faltas_a_cobrar=0;

/*para saber cuentas faltas antiguo esquema tiene*/
SELECT SUM(Exist) AS faltas, SUM(exist * precioconiva) AS importe FROM inventario WHERE Id_Familia = '113' AND id_proveedor = @gafete AND exist > 0;

BEGIN TRAN;
/*actualizamos la existencia con las faltas antiguo esquema a cobrar*/
UPDATE Inventario SET Exist=@faltas_a_cobrar, fechaedit=CONVERT(VARCHAR(8), GETDATE(), 112),id_logo='r' WHERE Id_Familia = '113' AND id_proveedor = @gafete AND exist > 0;

/*verificamos con cuaantas faltas antiguo esquema quedo para cobrar*/
SELECT SUM(Exist) AS faltas, SUM(exist * precioconiva) AS importe FROM inventario WHERE Id_Familia = '113' AND id_proveedor = @gafete AND exist > 0;

--COMMIT TRAN
--ROLLBACK TRAN