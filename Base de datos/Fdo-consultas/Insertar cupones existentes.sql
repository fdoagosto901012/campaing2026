/*
Esta consulta inserta los cupones existentes del viejos esquema (Los que no tienen codigo QR)
por lo cual se inserta el folio en un campo externo.
*/

select * from Cupons;

select * from Couponrates as cr
left join Cupontypes as ct on cr.CupontypeID = ct.CupontypeID
left join Hotels as h on h.HotelID = cr.HotelID;


select * from Cupontypes;

insert into Cupontypes (Name, CreateBy, CreateDate) values ('NA','Sistemas', getdate());


