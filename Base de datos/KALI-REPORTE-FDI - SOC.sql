Declare @family AS NVarchar(4000)
set @family = '135';

--SELECT ROUND(SUM(V.Importe),2) AS CORTE_CAJA, FechaOp, Id_Familia FROM [dbo].[VentasDetalle] V
with 
-- 2-3 segundos
CTE_Results as (
	SELECT VD.Id_Producto, sum (VD.Importe) as IMPORTE_MES  
	FROM [dbo].[VentasDetalle] VD
	left join [dbo].[Ventas] as V on VD.Id_Op = V.ID_OP
	WHERE Status not in('Cancelada') AND
	Id_Familia = '135' AND
	FechaOp  BETWEEN  '01/04/2024' AND '30/04/2024' 
	group by VD.Id_Producto
	--and VD.Id_Producto = '20.784.1'
), 
-- 7-8 Segundos
CTE_HISTORY as (
	SELECT [Id_Producto],SUM([dbo].[VentasDetalle].Importe) AS IMPORTE
	FROM [dbo].[VentasDetalle]
	left join [dbo].[Ventas] on [dbo].[VentasDetalle].Id_Op = [dbo].[Ventas].ID_OP
	WHERE Status not in('Cancelada') AND
	Id_Familia = '135' AND
	FechaOp  BETWEEN  '14/09/2021' AND '30/04/2024' 
	group by [Id_Producto]
)
select R.Id_Producto, R.IMPORTE_MES, RH.IMPORTE, F.socio, F.nombre from CTE_Results as R 
Inner join CTE_HISTORY as RH on R.Id_Producto = RH.Id_Producto
left join FDI_NE_soc_turnados as F on F.clave_cargo = R.Id_Producto;



/*
select R.Id_Producto, R.IMPORTE_MES, RH.IMPORTE, F.socio, F.nombre from CTE_Results as R
left join CTE_HISTORY as RH on R.Id_Producto = R.Id_Producto
left join FDI_NE_soc_turnados as F on F.clave_cargo = R.Id_Producto;
*/
--select * from Familia  where Familia like '%FDI%' order by Id_Familia;
--select * from FDI_NE_soc_turnados;