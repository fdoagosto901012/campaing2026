select * from Convenios as c
left join ConveniosDetalle as cd on c.Id_Op = cd.Id_Op
left join Inventario as i on i.Id_Producto = cd.Id_Producto
where 
c.Id_Op = '227028' and 
c.Status = 'CANCELADO' and 
c.FechaOp > '2025-05-01';


