
--consulta FDI socios 

SELECT [Id_Producto],DESCRIPCION,COUNT(Id_Cliente)AS PAGOS

      
  FROM [dbo].[VentasDetalle]
    left join [dbo].[Ventas] on [dbo].[VentasDetalle].Id_Op = [dbo].[Ventas].ID_OP

	WHERE Id_Familia ='135'
	and Status not in('Cancelada') AND FechaOp > '2021-01-01'
	 GROUP BY Id_Producto,DESCRIPCION ORDER BY Id_Producto

--consulta FDI operadores 
SELECT [Id_Producto],COUNT(Id_Cliente)AS PAGOS
      
  FROM [dbo].[VentasDetalle]
    left join [dbo].[Ventas] on [dbo].[VentasDetalle].Id_Op = [dbo].[Ventas].ID_OP

	WHERE Id_Familia ='136'
	and Status not in('Cancelada') AND FechaOp > '2021-01-01'
	 GROUP BY Id_Producto ORDER BY Id_Producto

