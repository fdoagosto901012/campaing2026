/*Cupones_Cambio_de_numero*/
USE SindQR
DECLARE @cupon_capturado_con_error AS money
DECLARE @cupon_correcto AS money
DECLARE @sector_ambos_cupones AS varchar(5)

SET @cupon_capturado_con_error=150575
SET @cupon_correcto=150578
SET @sector_ambos_cupones='uvc'

/*verificamos que exista el cupon mal capturado con el sector dado*/
SELECT p.* FROM dbo.pagocupones p WHERE p.numero=@cupon_capturado_con_error AND p.sector=@sector_ambos_cupones

/*verificamos que no este ya pagado el cupon por el cual quieren hacer el cambio*/
SELECT p.* FROM dbo.pagocupones p WHERE p.numero=@cupon_correcto AND p.sector=@sector_ambos_cupones

DECLARE @registro1 AS int
DECLARE @registro2 AS int

SET @registro1=ISNULL((SELECT p.numero FROM dbo.pagocupones p WHERE p.numero=@cupon_capturado_con_error AND p.sector=@sector_ambos_cupones),0)
SET @registro2=ISNULL((SELECT p.numero FROM dbo.pagocupones p WHERE p.numero=@cupon_correcto AND p.sector=@sector_ambos_cupones),0)

IF @registro1<=0 BEGIN
    SELECT 'No se encontro un registro de pago de cupon con el numero: ' + CONVERT(varchar(15),CONVERT(int,@cupon_capturado_con_error)) AS ERROR
    RETURN
END

IF @registro2>0 BEGIN
    SELECT 'Se encontraron registros de pago con el numero de cupon que se quiere cambiar: ' + CONVERT(varchar(15),CONVERT(int,@cupon_correcto)) AS ERROR
    RETURN
END

BEGIN TRANSACTION
/*actualizamos el registro de pago del cupon con el numero de cupon correcto*/
UPDATE dbo.pagocupones SET dbo.pagocupones.numero = @cupon_correcto WHERE numero=@cupon_capturado_con_error AND sector=@sector_ambos_cupones


/*verificamos que ya no exista el cupon mal capturado con el sector dado*/
SELECT p.* FROM dbo.pagocupones p WHERE p.numero=@cupon_capturado_con_error AND p.sector=@sector_ambos_cupones

/*verificamos que exista el cupon por el cual se realizo el cambio*/
SELECT p.* FROM dbo.pagocupones p WHERE p.numero=@cupon_correcto AND p.sector=@sector_ambos_cupones

--COMMIT TRAN
--ROLLBACK TRAN
