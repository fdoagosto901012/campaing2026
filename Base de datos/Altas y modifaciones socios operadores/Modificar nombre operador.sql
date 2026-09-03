/OP_nombre_corregir/
USE SindQR

DECLARE @gafete varchar(12)
DECLARE @nombre varchar(200)
DECLARE @apellidos varchar(200)

SET @gafete = '49691'
SET @gafete = 'OP-'+@gafete
SET @nombre = 'FELIPE DE JESUS '
SET @apellidos = 'CHE CAAMAL'

/* verificamos su nombre actualmente en la tabla chof_detalle*/
SELECT cd.* FROM Chof_Detalle AS cd WHERE cd.CHOFER = @gafete
/* verificamos su nombre actualmente en la tabla proveedores*/
SELECT p.* FROM dbo.Proveedores p WHERE p.Id_Proveedor=@gafete


BEGIN TRANSACTION  
UPDATE Chof_Detalle SET Chof_Detalle.NOMBRE = @nombre , 
                        Chof_Detalle.APELLIDOS = @apellidos
WHERE Chof_Detalle.CHOFER = @gafete

UPDATE Proveedores SET Proveedores.Proveedor = @nombre + ' ' + @apellidos
WHERE Proveedores.Id_Proveedor = @gafete

/* verificamos su nombre DESPUES de actualizar en la tabla chof_detalle*/
SELECT cd.*
  FROM Chof_Detalle AS cd
  WHERE cd.CHOFER = @gafete
 /* verificamos su nombre DESPUES de actualizar en la tabla proveedores*/
SELECT p.* FROM dbo.Proveedores p WHERE p.Id_Proveedor=@gafete

--COMMIT TRANSACTION
--ROLLBACK TRANSACTION