USE [SindQR]
GO
/****** Object:  StoredProcedure [dbo].[fdo_get_Partners]    Script Date: 04/04/2024 06:16:19 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[fdo_get_Partners](
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
		cd.CHOFER,
		cd.NOMBRE,
		cd.APELLIDOS,
		cd.SOBRENOMB, 
		cd.SEXO, 
		cd.FECHANAC,
		cd.LUGARNAC,
		cd.EDOCIVIL,
		cd.CREDELECTOR,
		cd.SECCION,
		cd.OCUPACION,
		cd.TIPOSANGRE,
		cd.CARTILLA,
		cd.ALERGIAS,
		cd.ESCOLARIDAD,
		cd.CONYUGE,
		cd.TELEFONO,
		cd.FONDDEF,
		cd.[STATUS],
		cd.SOC_ALTA,
		cd.FECHAING,
		cd.PATRON,
		cd.DOMICACT,
		cd.CIUDADACT,
		cd.DOMICANT,
		cd.CIUDADANT,
		cd.LICENCIA,
		cd.LICVENCE,
		
		cd.FECHABAJ,
		cd.OBS,
		cd.COMEN1,
		cd.FECHAOP,
		cd.ID_USUARIO,
		cd.FECHAEDIT,
		cd.ID_USUARIOEDIT,
		cd.FECHAREACTIVACION,
		cd.FIANZA,
		cd.Mayacaribe,
		cd.UltTaxi,
		cd.ULTTURNO,
		cd.SECRETARIA,
		cd.PUESTO,
		cd.PosicionPadron,
		cd.postemp,
		cd.strenta,
		cd.impacto,
		cd.imprimirenpadron,
		cd.edicioncontrolada,
		
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
		from Chof_Detalle as cd
		left join [user] as u on UPPER(cd.CHOFER) = UPPER(u.partnerReference) and u.active = 1
		WHERE (1=1)';
		--- MIS CONDICIONES DINAMICAS
		If @GafetValue Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.CHOFER = ''' + @GafetValue  + ''''

		If @NameValue Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.NOMBRE LIKE '''+ '%' + @NameValue + '%' + ''''

		If @FatherLastname Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.APELLIDOS LIKE '''+ '%' + @FatherLastname + '%' + ''''

		Set @SQLQuery = @SQLQuery + '
		ORDER BY NOMBRE,APELLIDOS asc
		OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
	), 
	
	CTE_TotalRows as (
		SELECT count(CHOFER) as TotalRows 
		from Chof_Detalle as cd
		where (1 = 1)';
		--- MIS CONDICIONES DINAMICAS
		If @GafetValue Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.CHOFER = ''' + @GafetValue  + ''''

		If @NameValue Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.NOMBRE LIKE '''+ '%' + @NameValue + '%' + ''''

		If @FatherLastname Is Not Null 
			Set @SQLQuery = @SQLQuery + ' AND cd.APELLIDOS LIKE '''+ '%' + @FatherLastname + '%' + ''''

		Set @SQLQuery = @SQLQuery + '
	)
	Select c.*, img.bucket, img.[key], null as urlImage,TotalRows from dbo.Chof_Detalle as c 
	left join [user] as u on u.partnerReference = c.CHOFER
	left join [amazonPictures] as img on img.userId = u.id and img.pictureTypeId = 2
	,CTE_TotalRows
	WHERE EXISTS (SELECT 1 FROM CTE_Results WHERE CTE_Results.CHOFER = c.CHOFER)
	order by NOMBRE,APELLIDOS asc
	OPTION (RECOMPILE)';
	
	--select @SQLQuery as query, @PageNo as PageNo, @PageSize as PageSize;
	
	Set @ParamDefinition = '@PageNo INT,
		@PageSize INT'

	Execute sp_Executesql @SQLQuery,
				@ParamDefinition, 
                @PageNo, 
                @PageSize

END
