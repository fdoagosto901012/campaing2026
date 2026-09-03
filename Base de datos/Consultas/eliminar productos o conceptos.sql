DECLARE @OPERADORES AS varchar(50)
DECLARE @PRODUCTO AS varchar(50)
DECLARE @MOTIVO as varchar(10);
DECLARE @AUTORIZA as varchar(6);

SET @OPERADORES = 'OP-15422'
SET @PRODUCTO = '128.1.1'
SET @MOTIVO = 'DUPLICADO'
set @AUTORIZA = 'JR'

select * from Inventario where Id_Proveedor = @OPERADORES;

BEGIN TRANSACTION
update Inventario set Exist = 0, Color = @MOTIVO, Id_UsuarioEdit = @AUTORIZA 
where 
Id_Proveedor = @OPERADORES and 
Id_Producto = @PRODUCTO

--COMMIT TRAN
--ROLLBACK TRAN