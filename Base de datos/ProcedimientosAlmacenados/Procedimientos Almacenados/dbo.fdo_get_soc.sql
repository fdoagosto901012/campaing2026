USE [SindQR]
GO
/****** Object:  StoredProcedure [dbo].[fdo_get_soc]    Script Date: 04/04/2024 06:16:48 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[fdo_get_soc](
    -- Parametros de busqueda para mi formulario.   
    @PageNo INT = 1,
    @PageSize INT = 10,
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

	SET @GafetValue = LTRIM(RTRIM(@GafetValue))
    SET @NameValue = LTRIM(RTRIM(@NameValue))
    SET @FatherLastname = LTRIM(RTRIM(@FatherLastname))
    SET @MotherLastname = LTRIM(RTRIM(@MotherLastname))


	SET @SQLQuery = '
	
	with CTE_Results as (
		select 
		cd.Id_Op,
		cd.Numero,
		cd.Nombre,
		cd.Paterno,
		cd.Materno,
		
		-- Padron 
		u.partnerReference,
		u.firstName,
		u.lastNameF,
		u.lastNameM,
		u.createdDt,
		u.partnerTypeId,
		u.birthPlace,
		u.sex,
		u.birthDate,
		u.userTypeId,
		u.dtLastUpdate,
		u.relationShipStatusId,
		u.statusId,
		u.cafecude,
		u.[at],
		u.active,
		u.reason,
		u.createdBy,
		u.editBy,
		u.meeting,
		u.death,
		u.enemy,
		u.payroll,
		u.ttesoc,
		u.employee,
		u.employees,
		u.noVote,
		u.sectionId,
		u.candidateId,
		u.candidateHardvoteId,
		u.candidateQuizID,
		u.gps_id,
		u.tsc,
		u.rt,
		u.googleplus,
		u.facebook,
		u.instagram,
		u.twitter,
		u.ttc_soc_eco,
		u.comments,
		u.rfc,
		u.ine,
		u.curp
		from Soc_DatPersonales as cd
		left join [user] as u on UPPER(cd.Numero) = UPPER(u.partnerReference) and u.active = 1
		WHERE (1=1)';
		--- MIS CONDICIONES DINAMICAS
		If @GafetValue Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.numero LIKE '''+ '%' + @GafetValue + '%' + ''''

		If @NameValue Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.NOMBRE LIKE '''+ '%' + @NameValue + '%' + ''''

		If @FatherLastname Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.Paterno LIKE '''+ '%' + @FatherLastname + '%' + ''''
		
		If @MotherLastname Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.Materno LIKE '''+ '%' + @MotherLastname + '%' + ''''

		Set @SQLQuery = @SQLQuery + '
		ORDER BY NOMBRE,Paterno,Materno asc
		OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
	), 
	
	CTE_TotalRows as (
		SELECT count(Numero) as TotalRows 
		from Soc_DatPersonales as cd
		where (1 = 1)';
		--- MIS CONDICIONES DINAMICAS
		If @GafetValue Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.numero LIKE '''+ '%' + @GafetValue + '%' + ''''

		If @NameValue Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.NOMBRE LIKE '''+ '%' + @NameValue + '%' + ''''

		If @FatherLastname Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.Paterno LIKE '''+ '%' + @FatherLastname + '%' + ''''
		
		If @MotherLastname Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.Materno LIKE '''+ '%' + @MotherLastname + '%' + ''''

		Set @SQLQuery = @SQLQuery + '
	)
	Select c.*,TotalRows from dbo.Soc_DatPersonales as c, CTE_TotalRows
	WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.Numero = c.Numero)
	order by NOMBRE,Paterno,Materno asc
	OPTION (RECOMPILE)';
	
	--select @SQLQuery as query, @PageNo as PageNo, @PageSize as PageSize;
	
	Set @ParamDefinition = '@PageNo INT,
		@PageSize INT'

	Execute sp_Executesql @SQLQuery,
				@ParamDefinition, 
                @PageNo, 
                @PageSize

END
