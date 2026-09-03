DECLARE @REFERENCE NVARCHAR(100)
SET @REFERENCE = 'OP-55320';

delete ChristmasgiftPrinted where Christmasgiftid in (
	select c.id as cuponId from [user] as u 
	left join Christmasgift as c on u.id = c.userid
	where u.partnerReference = @REFERENCE
);

delete Christmasgift where id in (
	select c.id as cuponId from [user] as u 
	left join Christmasgift as c on u.id = c.userid
	where u.partnerReference = @REFERENCE
);