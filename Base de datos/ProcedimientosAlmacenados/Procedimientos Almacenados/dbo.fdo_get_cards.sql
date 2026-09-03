USE [SindQR]
GO
/****** Object:  StoredProcedure [dbo].[fdo_get_cards]    Script Date: 04/04/2024 06:10:21 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[fdo_get_cards](
    -- Parametros de busqueda para mi formulario.   
    @PageNo INT = 1,
    @PageSize INT = 10,
	@taxi NVARCHAR(50) = NULL,
	@GafetValue NVARCHAR(50) = NULL,
	@NameValue NVARCHAR(50) = NULL,
	@FatherLastname NVARCHAR(50) = NULL,
	@MotherLastname NVARCHAR(50) = NULL
)
AS
BEGIN
	SET NOCOUNT ON;
	-- Declaramos la query dinamica
    Declare @SQLQuery AS NVarchar(4000)
    Declare @ParamDefinition AS NVarchar(2000) 
    Declare @SQLQueryPaginator AS NVarchar(4000)

	SET @taxi = LTRIM(RTRIM(@taxi))
	SET @GafetValue = LTRIM(RTRIM(@GafetValue))
    SET @NameValue = LTRIM(RTRIM(@NameValue))
    SET @FatherLastname = LTRIM(RTRIM(@FatherLastname))
    SET @MotherLastname = LTRIM(RTRIM(@MotherLastname))

	SET @SQLQuery = '
	with CTE_Results as (
		select
		*
		from cards as c
		left join Chof_Detalle as cd on cd.CHOFER = c.partnerReference
		WHERE (1=1) ';
		--- MIS CONDICIONES DINAMICAS
		If @taxi Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND c.taxi = ''' + @taxi  + ''''

		If @GafetValue Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.CHOFER = ''' + @GafetValue  + ''''

		If @NameValue Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.NOMBRE LIKE '''+ '%' + @NameValue + '%' + ''''

		If @FatherLastname Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.APELLIDOS LIKE '''+ '%' + @FatherLastname + '%' + ''''

		Set @SQLQuery = @SQLQuery + '
		ORDER BY c.asignedDate desc
		OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
	), 
	
	CTE_TotalRows as (
		SELECT count(c.id) as TotalRows
		from cards as c
		left join Chof_Detalle as cd on cd.CHOFER = c.partnerReference
		WHERE (1=1)';
		--- MIS CONDICIONES DINAMICAS
		If @taxi Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND c.taxi = ''' + @taxi  + ''''
		If @GafetValue Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.CHOFER = ''' + @GafetValue  + ''''
		If @NameValue Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.NOMBRE LIKE '''+ '%' + @NameValue + '%' + ''''
		If @FatherLastname Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.APELLIDOS LIKE '''+ '%' + @FatherLastname + '%' + ''''
		Set @SQLQuery = @SQLQuery + '
	)
	select c.*, u.*,TotalRows from cards as c 
	left join Chof_Detalle as cd on cd.CHOFER = c.partnerReference
	left join [user] as u on u.partnerReference = c.partnerReference
	,CTE_TotalRows
	WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.id = c.id)
	order by c.asignedDate desc
	OPTION (RECOMPILE)';
	--select @SQLQuery as query, @PageNo as PageNo, @PageSize as PageSize, @taxi as Taxi;
	Set @ParamDefinition = '@PageNo INT,
		@PageSize INT'
	Execute sp_Executesql @SQLQuery,@ParamDefinition,@PageNo,@PageSize
END