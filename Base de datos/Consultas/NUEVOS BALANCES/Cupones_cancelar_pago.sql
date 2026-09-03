/*Cupones_cancelar_pago*/
USE SindQR
DECLARE @cupon_a_cancelar AS money
DECLARE @sector_cupon AS varchar(5)

SET @cupon_a_cancelar=1748 
SET @sector_cupon='GA'

/*verificamos que exista el cupon que se va a cancelar*/
SELECT p.* FROM dbo.pagocupones p WHERE p.numero=@cupon_a_cancelar AND p.sector=@sector_cupon

DECLARE @registro1 AS int

SET @registro1=ISNULL((SELECT p.numero FROM dbo.pagocupones p WHERE p.numero=@cupon_a_cancelar AND p.sector=@sector_cupon),0)

IF @registro1<=0 BEGIN
    SELECT 'No se encontro un registro de pago de cupon con el numero: ' + CONVERT(varchar(15),CONVERT(int,@cupon_a_cancelar)) AS ERROR
    RETURN
END

BEGIN TRANSACTION
/*borramos el registro de pago del cupon a cancelar*/
DELETE TOP(1) dbo.pagocupones WHERE numero=@cupon_a_cancelar AND sector=@sector_cupon


/*verificamos que ya no exista registro del cupon a cancelar*/
SELECT p.* FROM dbo.pagocupones p WHERE p.numero=@cupon_a_cancelar AND p.sector=@sector_cupon

--COMMIT TRAN
--ROLLBACK TRAN



