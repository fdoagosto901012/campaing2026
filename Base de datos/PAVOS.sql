-- obtener los operadores con reimpresiones
select u.partnerReference, count(-1) as impresiones from ChristmasgiftPrinted as cp
left join Christmasgift as c on c.id = cp.Christmasgiftid
left join [user] as u on c.userid = u.id
group by u.partnerReference
having count(-1) > 1;

-- obtener las reimpresiones de un operador.
select u.partnerReference, u.firstName, u.lastNameF, u.lastNameM, cp.PrintedBy, cp.PrintedDate from ChristmasgiftPrinted as cp
left join Christmasgift as c on c.id = cp.Christmasgiftid
left join [user] as u on c.userid = u.id
where u.partnerReference = 'OP-54929';

-- obtener impresiones por cajera.
select upper(cp.PrintedBy) as nombre , count(-1) as impresiones from ChristmasgiftPrinted as cp
left join Christmasgift as c on c.id = cp.Christmasgiftid
left join [user] as u on c.userid = u.id
group by cp.PrintedBy
order by impresiones desc;

-- obtener impresiones por cajera por dia.
select upper(cp.PrintedBy) as nombre , CONVERT(DATE,cp.PrintedDate) as fecha, count(-1) as impresiones from ChristmasgiftPrinted as cp
left join Christmasgift as c on c.id = cp.Christmasgiftid
left join [user] as u on c.userid = u.id
group by cp.PrintedBy,  CONVERT(DATE,cp.PrintedDate) 
order by impresiones desc;


-- pavos entregados---
select u.partnerReference, count(-1) as impresiones from ChristmasgiftPrinted as cp
left join Christmasgift as c on c.id = cp.Christmasgiftid
left join [user] as u on c.userid = u.id
group by u.partnerReference
having count(-1) > 1;


-- entrega de pavos por dia.
select CONVERT(DATE, c.DeliveryDate) as [date], count(-1) as amount from ChristmasgiftPrinted as cp
left join Christmasgift as c on c.id = cp.Christmasgiftid
left join [user] as u on c.userid = u.id
where c.DeliveryDate is not null
group by CONVERT(DATE, c.DeliveryDate) 
order by [date] asc;