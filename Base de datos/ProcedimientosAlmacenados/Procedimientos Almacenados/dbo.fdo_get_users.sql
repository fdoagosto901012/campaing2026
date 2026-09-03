USE [SindQR]
GO
/****** Object:  StoredProcedure [dbo].[fdo_get_users]    Script Date: 04/04/2024 06:17:25 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[fdo_get_users](
    -- Parametros de busqueda para mi formulario.   
    @PageNo INT = 1,
    @PageSize INT = 10,
	@GafetValue NVARCHAR(50) = NULL,
	@NameValue NVARCHAR(50) = NULL,
	@FatherLastname NVARCHAR(50) = NULL
)
AS
BEGIN
	SET NOCOUNT ON;
	-- Declaramos la query dinamica
    Declare @SQLQuery AS NVarchar(4000)
    Declare @ParamDefinition AS NVarchar(2000) 
    Declare @SQLQueryPaginator AS NVarchar(4000)

	SET @GafetValue = LTRIM(RTRIM(@GafetValue))
    SET @NameValue = LTRIM(RTRIM(@NameValue))
    SET @FatherLastname = LTRIM(RTRIM(@FatherLastname))

	SET @SQLQuery = '
	with CTE_Results as (
		select ul.*,r.[name] as [RoleName] from userlog as ul 
		left join userRole as ur on ul.userId = ur.userId
		left join [role] as r on r.id = ur.roleId 
		left join [user] as u on ul.padronID = u.id
		WHERE (1=1) ';
		--- MIS CONDICIONES DINAMICAS

		If @GafetValue Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND u.partnerReference = ''' + @GafetValue  + ''''

		If @NameValue Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND ul.firstName LIKE '''+ '%' + @NameValue + '%' + ''''

		If @FatherLastname Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND ul.lastName LIKE '''+ '%' + @FatherLastname + '%' + ''''

		Set @SQLQuery = @SQLQuery + '
		ORDER BY ul.firstName, ul.lastName desc
		OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
	), 
	
	CTE_TotalRows as (
		SELECT count(ul.userId) as TotalRows from userlog as ul 
		left join userRole as ur on ul.userId = ur.userId
		left join [role] as r on r.id = ur.roleId 
		left join [user] as u on ul.padronID = u.id
		WHERE (1=1)';
		--- MIS CONDICIONES DINAMICAS
		If @GafetValue Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND u.partnerReference = ''' + @GafetValue  + ''''

		If @NameValue Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND ul.firstName LIKE '''+ '%' + @NameValue + '%' + ''''

		If @FatherLastname Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND ul.lastName LIKE '''+ '%' + @FatherLastname + '%' + ''''

		Set @SQLQuery = @SQLQuery + '
	)
	select ul.*, r.[name] as RoleName, u.partnerReference,TotalRows from userlog as ul
	left join userRole as ur on ul.userId = ur.userId
	left join [role] as r on r.id = ur.roleId
	left join [user] as u on ul.padronID = u.id
	,CTE_TotalRows
	WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.userId = ul.userId)
	order by ul.firstName, ul.lastName desc
	OPTION (RECOMPILE)';
	--select @SQLQuery as query, @PageNo as PageNo, @PageSize as PageSize;
	Set @ParamDefinition = '@PageNo INT,
		@PageSize INT'
	Execute sp_Executesql @SQLQuery,@ParamDefinition,@PageNo,@PageSize
END