
--consulta FDI OP 

SELECT ROUND(SUM(V.Importe),2) AS CORTE_CAJA 
       FROM [dbo].[VentasDetalle] V
    left join [dbo].[Ventas] on V.Id_Op = [dbo].[Ventas].ID_OP
		WHERE Id_Familia ='136'
	and Status not in('Cancelada') AND FechaOp ='20240131' 
	 GROUP BY Id_Familia 

	 
	 -------------------------------------------------------------------------------
	 
SELECT [Id_Producto],COUNT(Id_Cliente)AS PAGOS
		FROM [dbo].[VentasDetalle]
    left join [dbo].[Ventas] on [dbo].[VentasDetalle].Id_Op = [dbo].[Ventas].ID_OP

	WHERE Id_Familia ='136'
	and Status not in('Cancelada') AND FechaOp ='20240131' 
	 GROUP BY Id_Producto ORDER BY Id_Producto


     -------------------------------------------------------------------------------
SELECT Id_Familia,COUNT(Id_Cliente)AS PAGOS
       FROM [dbo].[VentasDetalle]
    left join [dbo].[Ventas] on [dbo].[VentasDetalle].Id_Op = [dbo].[Ventas].ID_OP
		WHERE Id_Familia ='136'
	and Status not in('Cancelada') AND FechaOp ='20240131' 
	 GROUP BY Id_Familia 

	 
--consulta FDI SOC 

SELECT ROUND(SUM(V.Importe),2) AS CORTE_CAJA 
       FROM [dbo].[VentasDetalle] V
    left join [dbo].[Ventas] on V.Id_Op = [dbo].[Ventas].ID_OP
		WHERE Id_Familia ='135'
	and Status not in('Cancelada') AND FechaOp ='20240130' 
	 GROUP BY Id_Familia 
	 -------------------------------------------------------------------------------
SELECT [Id_Producto],COUNT(Id_Cliente)AS PAGOS
		FROM [dbo].[VentasDetalle]
    left join [dbo].[Ventas] on [dbo].[VentasDetalle].Id_Op = [dbo].[Ventas].ID_OP

	WHERE Id_Familia ='135'
	and Status not in('Cancelada') AND FechaOp ='20240131' 
	 GROUP BY Id_Producto ORDER BY Id_Producto
	 -------------------------------------------------------------------------------
SELECT Id_Familia,COUNT(Id_Cliente)AS PAGOS
       FROM [dbo].[VentasDetalle]
    left join [dbo].[Ventas] on [dbo].[VentasDetalle].Id_Op = [dbo].[Ventas].ID_OP
		WHERE Id_Familia ='135'
	and Status not in('Cancelada') AND FechaOp ='20240129' 
	 GROUP BY Id_Familia 