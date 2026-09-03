/*OP_radiotaxi_baja*/
USE SindQR
DECLARE @gafete AS VARCHAR(15);

SET @gafete='14726'; --no lleva el (OP-)

DECLARE @registro1 AS int
SET @registro1=ISNULL((SELECT i.Exist FROM dbo.Inventario i WHERE i.Id_Proveedor='OP-'+@gafete AND i.Id_Familia='11' AND i.Exist>0),0)

/*si aun debe cuotas de radio taxi entonces paramos aqui.*/
IF @registro1>0 BEGIN
    SELECT 'Este operador debe: ' + CONVERT(varchar(5),@registro1) + ', que se cobren o quiten esas cuotas de radio taxi pendientes para poder continar.' AS ERROR
    RETURN
END

BEGIN TRANSACTION
/*debe de existir un registro en el padron de radio taxi,
si no es asi paramos aqui por que no hay nada que hacer */
IF EXISTS(SELECT prt.gafete FROM dbo.padronRadioTaxi prt WHERE prt.gafete=@gafete AND prt.tipo='1') BEGIN
		  UPDATE padronRadioTaxi SET Status = 'B', fechabaja = CONVERT(varchar(8) , GETDATE() , 112)
		  WHERE gafete = @gafete
		  AND tipo = '1'
    END
ELSE --si no existe el registro anterior de este gafete
    BEGIN
	   SELECT 'Este operador no esta registrado en el padron de radio taxi.' AS ERROR
	   RETURN
    END
-----

/*mostramos el resgistro del padron radio taxi*/
SELECT prt.* FROM dbo.padronRadioTaxi prt WHERE prt.gafete=@gafete AND prt.tipo='1'

/*mostramos el registro del inventario donde se acumulan las cuotas de radio taxi
el cual debe estar en 0*/
SELECT i.Id_Producto,i.Descripcion,i.Id_Proveedor,i.Exist FROM dbo.Inventario i WHERE i.Id_Proveedor='OP-'+@gafete AND i.Id_Familia='11'

--COMMIT TRANSACTION
--ROLLBACK TRANSACTION