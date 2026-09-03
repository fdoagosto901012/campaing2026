WITH UltimoReporte AS (
	select 
	Gafete,
	FechaReporte,
	u.id as userid,
	ticket,
	p.*,
	i.Exist,
	i.FechaOp,
	cd.CHOFER,
	u.partnerReference,
	ROW_NUMBER() OVER (PARTITION BY Gafete ORDER BY FechaReporte DESC) AS rn
	from HistAsistencias as h
	left join Chof_Detalle as cd on cd.CHOFER = Gafete 
	left join [user] as u on u.partnerReference = Gafete
	left join _permissions as p on CONCAT('P',p.id) = h.Ticket
	left join Inventario as i on i.Control = p.[control]
	where cd.express = 1
)
select * from (
SELECT
    Gafete,
    CHOFER,
	partnerReference,
    FechaReporte,
	userid
	ticket,
	[control],
	Exist,
	dateStart,
	dateEnd,
	taxi,
	CreatedBy,
	dateCreated,
	FechaOp,
	rn
FROM
    UltimoReporte as u
WHERE
rn = 1) as c
where CAST(DATEADD(DAY, -5, dateEnd) as DATE) between CAST(DATEADD(DAY, -25, GETDATE()) as DATE) and CAST(DATEADD(DAY, 35, GETDATE()) as DATE)
order by FechaReporte desc;