create table [cards](
	id int not null primary key identity(1,1),
	userId int not null, 
	partnerReference nvarchar(100) not null default '',
	taxi nvarchar(100) default '' not null,
	asignedDate date default getdate(),
	asgnedBy nvarchar(225) default '', 
	note nvarchar(225) default '',
	lastEditedDate date, 
	editedBy nvarchar(225),
	isActive bit default 1,
	turn int default 3,
	deliver bit,
	deliverDate date, 
	expiration_date date, 
	id_Op float, 
	[low] bit, 
	lowDate date,
	FOREIGN KEY (userId) REFERENCES [user](id)
);

create table cardsPrinted(
	id int not null primary key identity(1,1),
	cardId int not null,
	printednumber int not null default 0,
	ticket int,
	ticketsDate date,
	FOREIGN KEY (cardId) REFERENCES [cards](id)
);