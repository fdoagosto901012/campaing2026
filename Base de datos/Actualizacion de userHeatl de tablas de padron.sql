with USERH as(
	select u.id as _id, uh.* from [user] as u
	left join userHealthInformation as uh on u.id = uh.userId
	where u.active = 1
)
INSERT INTO userHealthInformation (allergies, organDonor, userId, bloodTypeID)
select 'Ninguna', 0, u._id, 9 from USERH as u 
where u.id is null;
update userHealthInformation set bloodTypeID = 9
where bloodTypeID is null;