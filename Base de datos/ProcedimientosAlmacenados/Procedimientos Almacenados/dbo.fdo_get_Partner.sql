USE [SindQR]
GO
/****** Object:  StoredProcedure [dbo].[fdo_get_Partner]    Script Date: 04/04/2024 06:15:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Fernando Agosto Cruz>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[fdo_get_Partner](
	@gafet nvarchar(50) = null
)
AS
BEGIN
	SET NOCOUNT ON;


	select top 1
	Cd.*,
	u.id as [userId],
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
	u.sectionId, 
	u.cafecude, 
	u.at, 
	u.active, 
	u.reason, 
	u.createdBy,
	u.editBy,
	u.meeting,
	u.death,
	u.enemy,
	u.payroll,
	u.ttesoc,
	u.noVote,
	u.sectionId,
	u.candidateId,
	u.candidateHardvoteId,
	u.candidateQuizID,
	u.gps_id,
	u.googleplus, 
	u.facebook,
	u.instagram,
	u.twitter,
	u.comments, 
	u.rfc as urfc,
	u.ine, 
	u.curp,
	img.bucket,
	img.[key],
	uh.allergies, 
	uh.organDonor,
	rs.name as realtionship,
	b.name as bloodType
	from Chof_Detalle as cd
	left join [user] as u on u.partnerReference = cd.CHOFER and u.active = 1
	left join [amazonPictures] as img on img.userId = u.id and img.pictureTypeId = 2
	left join [userHealthInformation] as uh on uh.userId = u.id
	left join [relationShipStatus] as rs on u.relationShipStatusId = rs.id
	left join [userBloodType] as b on b.id = uh.bloodTypeID
	where cd.CHOFER = @gafet;
END
