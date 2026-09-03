select h.shortname as pre, c.CuponsID as folio,c.serial_ex as serial_ex, ct.[Name] as Perfo, h.[Name] as Cliente,  cr.Valor as monto, c.Paid as pagado, c.PaymentDate as fechaPagado from Cupons as c 
left join Couponrates as cr on cr.CouponrateID = c.CouponratesID
left join Cupontypes as ct on ct.CupontypeID = cr.CupontypeID
left join Hotels as h on h.HotelID = cr.HotelID
order by ct.[Name] asc;