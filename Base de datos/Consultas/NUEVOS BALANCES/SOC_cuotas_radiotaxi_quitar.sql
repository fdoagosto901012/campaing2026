/*SOC_cuotas_radiotaxi_quitar*/
USE SindQR;
DECLARE @economico AS VARCHAR(15);
DECLARE @cuotasradiotaxi_a_cobrar AS MONEY;

SET @economico='6503';

SET @cuotasradiotaxi_a_cobrar=0;

/*para saber cuentas cuotas de radio taxi tiene*/
SELECT SUM(Exist) AS faltas, SUM(exist * precioconiva) AS importe FROM inventario WHERE Id_Familia = '11' AND id_proveedor = @economico AND exist > 0;

BEGIN TRAN;
/*actualizamos la existencia con las cuotas de radio taxi a cobrar*/
UPDATE Inventario SET Exist=@cuotasradiotaxi_a_cobrar, fechaedit=CONVERT(VARCHAR(8), GETDATE(), 112),id_logo='r' WHERE Id_Familia = '11' AND id_proveedor = @economico AND exist > 0;

/*verificamos con cuantas cuotas de radio taxi quedo para cobrar*/
SELECT SUM(Exist) AS faltas, SUM(exist * precioconiva) AS importe FROM inventario WHERE Id_Familia = '11' AND id_proveedor = @economico AND exist > 0;

--COMMIT TRAN
--ROLLBACK TRAN