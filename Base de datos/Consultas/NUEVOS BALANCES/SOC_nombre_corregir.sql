/*SOC_nombre_corregir*/
USE sindqr

DECLARE @socio int
DECLARE @nombre varchar(50)
DECLARE @paterno varchar(50)
DECLARE @materno varchar(50)

SET @socio = '1239'
SET @nombre = 'AGUSTINA EUGENIO DEL SAGRARIO'
SET @paterno = 'MARTIN'
SET @materno = 'SANSORES'

/* verificamos su nombre actualmente en la tabla Soc_DatPersonales*/
SELECT sdp.Numero,sdp.Nombre,sdp.Paterno, sdp.Materno FROM Soc_DatPersonales AS sdp WHERE sdp.Numero = @socio
/* verificamos su nombre actualmente en la tabla proveedores*/
SELECT p.Id_Proveedor,p.Proveedor  FROM Proveedores AS p WHERE p.Id_Proveedor=CONVERT(varchar(15),@socio)


BEGIN TRANSACTION  
UPDATE Soc_DatPersonales SET Soc_DatPersonales.NOMBRE = @nombre , 
                             Soc_DatPersonales.Paterno = @paterno , 
                             Soc_DatPersonales.Materno = @materno , 
                             Soc_DatPersonales.socemplacamiento = @nombre + ' ' + @paterno
WHERE Soc_DatPersonales.Numero = @socio
    
UPDATE Proveedores SET Proveedores.Proveedor = @nombre + ' ' + @paterno + ' ' + @materno 
WHERE Proveedores.Id_Proveedor = CONVERT(varchar(12) , @socio)

/* verificamos su nombre DESPUES de actualizar en la tabla Soc_DatPersonales*/
SELECT sdp.Numero,sdp.Nombre,sdp.Paterno, sdp.Materno FROM Soc_DatPersonales AS sdp WHERE sdp.Numero = @socio
/* verificamos su nombre DESPUES de actualizar en la tabla proveedores*/
SELECT p.Id_Proveedor,p.Proveedor  FROM Proveedores AS p WHERE p.Id_Proveedor=CONVERT(varchar(15),@socio)

--COMMIT TRANSACTION
--ROLLBACK TRANSACTION
