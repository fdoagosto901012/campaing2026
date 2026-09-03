create table userHistory(
	id int primary key identity(1,1),
	partnerReference nvarchar(20) default '',
	updatedBy nvarchar(50) default(''),
	updatedDate datetime default(getdate()),
	LastupdatedBy nvarchar(50) default(''),
	LastupdatedDate datetime default(getdate()),
	jsonObj nvarchar(4000) default ('')
)