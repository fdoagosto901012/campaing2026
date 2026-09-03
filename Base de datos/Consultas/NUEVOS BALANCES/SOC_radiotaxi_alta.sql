/*SOC_radiotaxi_alta*/
USE SindQR
DECLARE @economico AS VARCHAR(15);

SET @economico='6938';

DECLARE @registro1 AS int
SET @registro1=ISNULL((SELECT i.Exist FROM dbo.Inventario i WHERE i.Id_Proveedor=@economico AND i.Id_Familia='11' AND i.Exist>0),0)

/*si aun debe cuotas de radio taxi entonces paramos aqui.*/
IF @registro1>0 BEGIN
    SELECT 'Este socio debe: ' + CONVERT(varchar(5),@registro1) + ', que se cobren o quiten esas cuotas de radio taxi pendientes para poder continar.' AS ERROR
    RETURN
END

BEGIN TRANSACTION
/*si ya existe un registro en el padron de radio taxi, entonces actualizamos los datos
si no lo damos de alta en el padron de radio taxi*/
IF EXISTS(SELECT prt.gafete FROM dbo.padronRadioTaxi prt WHERE prt.gafete=@economico AND prt.tipo='2') BEGIN
		  UPDATE padronRadioTaxi SET Status = 'A', fechaalta = CONVERT(varchar(8) , GETDATE() , 112)
		  WHERE gafete = @economico
		  AND tipo = '2'
    END
ELSE --si no existe el registro anterior de este gafete
    BEGIN
        INSERT INTO padronRadioTaxi(gafete, tipo, Status, fechaAlta)
        VALUES(@economico, '2', 'A', CONVERT(varchar(8) , GETDATE() , 112))
    END
-----

/*mostramos el resgistro del padron radio taxi*/
SELECT prt.* FROM dbo.padronRadioTaxi prt WHERE prt.gafete=@economico AND prt.tipo='2'

/*mostramos el registro del inventario donde se acumulan las cuotas de radio taxi,
si no nos devuelve un registro aqui es que el alta es totalmente nueva y
su regsitro en inventario se creará al dia siguiente con el trabajo que carga las 
cuotas de radio taxi*/
SELECT i.Id_Producto,i.Descripcion,i.Id_Proveedor,i.Exist FROM dbo.Inventario i WHERE i.Id_Proveedor=@economico AND i.Id_Familia='11'

--COMMIT TRANSACTION
--ROLLBACK TRANSACTION