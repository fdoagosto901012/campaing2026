SELECT *
FROM Couponrates AS cr 
LEFT JOIN Cupontypes AS ct 
    ON ct.CupontypeID = cr.CupontypeID
LEFT JOIN Hotels AS h 
    ON h.HotelID = cr.HotelID
LEFT JOIN Cupones_viejo_esquema.dbo.ExistenciasExcel AS ex 
    ON ex.Codigo COLLATE Modern_Spanish_CI_AS
     = h.shortname COLLATE Modern_Spanish_CI_AS
WHERE ct.Name = 'NA';

select * from Cupones_viejo_esquema.dbo.ExistenciasExcel order by Codigo;
select * from Hotels order by shortname;


Select * from Hotels;


select * from Cupons;