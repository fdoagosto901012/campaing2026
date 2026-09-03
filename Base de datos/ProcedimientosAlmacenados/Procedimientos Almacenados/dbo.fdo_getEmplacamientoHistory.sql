USE [SindQR]
GO
/****** Object:  StoredProcedure [dbo].[fdo_getEmplacamientoHistory]    Script Date: 04/04/2024 06:18:30 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[fdo_getEmplacamientoHistory] 
	@gafet int =  NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT e.*, em.Modelo, ema.Marca FROM [SindQR-Prueba].[dbo].[Emplacamiento] as e 
	left join EmplaModelo as em on em.Id_Modelo = e.Id_Modelo
	left join EmplaMarca as ema on ema.Id_Marca = e.Id_Marca
	where No_Economico = @gafet
	order by e.No_Economico, e.FechaOp desc;
END
