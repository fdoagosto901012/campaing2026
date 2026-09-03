/*Consulta convenios que se denben*/
USE SindQR
DECLARE @cargo AS varchar(10)
SET @cargo='op-1000002'

SELECT [Id_Op]
      ,[Id_Secretaria]
      ,[Secretaria]
      ,[Id_Usuario]
      ,[Usuario]
      ,[Id_Concepto]
      ,[Concepto]
      ,[CargoA]
      ,[CargoANum]
      ,[CargoANombre]
      ,[AfavordeNum]
      ,[AfavordeNombre]
      ,[Monto]
      ,[Partidas]
      ,[Plazo]
      ,[PrimerVencimiento]
      ,[ImportePago]
      ,[FechaOp]
      ,[HoraOp]
      ,[FechaEdit]
      ,[Id_UsuarioEdit]
      ,[Status]
      ,[Observaciones]
      ,[TotalLetras]
      ,[REFAUTOSEG]
      ,[PAGOSaZUL]
      ,[FACTURASCARG]
  FROM [SindQR].[dbo].[Convenios] where CargoANum =@cargo


  /*Consulta convenios que se cobran*/
USE SindQR
DECLARE @afavor AS varchar(10)
SET @afavor='12'

SELECT [Id_Op]
      ,[Id_Secretaria]
      ,[Secretaria]
      ,[Id_Usuario]
      ,[Usuario]
      ,[Id_Concepto]
      ,[Concepto]
      ,[CargoA]
      ,[CargoANum]
      ,[CargoANombre]
      ,[AfavordeNum]
      ,[AfavordeNombre]
      ,[Monto]
      ,[Partidas]
      ,[Plazo]
      ,[PrimerVencimiento]
      ,[ImportePago]
      ,[FechaOp]
      ,[HoraOp]
      ,[FechaEdit]
      ,[Id_UsuarioEdit]
      ,[Status]
      ,[Observaciones]
      ,[TotalLetras]
      ,[REFAUTOSEG]
      ,[PAGOSaZUL]
      ,[FACTURASCARG]
  FROM [SindQR].[dbo].[Convenios] where AfavordeNum =@afavor


   /*Consulta detalles partidas de convenios*/
USE SindQR
DECLARE @convenio AS varchar(10)
SET @convenio='222410'

SELECT [Id_Op]
      ,[Folio]
      ,[Id_Producto]
      ,[Importe]
      ,[Vence]
      ,[Status]
      ,[Id_Pago]
      ,[FechaPago]
      ,[Id_PagoSoc]
      ,[FechaPS]
      ,[Id_UsuarioPago]
      ,[Id_UsuarioPS]
      ,[StatusInv]
  FROM [SindQR].[dbo].[ConveniosDetalle] where id_op =@convenio


