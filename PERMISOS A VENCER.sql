WITH UltimoReporte AS (
	select 
	Gafete,
	FechaReporte,
	u.id as userid,
	ROW_NUMBER() OVER (PARTITION BY Gafete ORDER BY FechaReporte DESC) AS rn
	from HistAsistencias
	left join Chof_Detalle as cd on cd.CHOFER = Gafete 
	left join [user] as u on u.partnerReference = Gafete
	where cd.express = 1
)
SELECT
    Gafete,
    FechaReporte,
	userid
FROM
    UltimoReporte as u
WHERE rn = 1 --and CAST(DATEADD(DAY, 5, GETDATE()) as DATE) > FechaReporte
order by FechaReporte desc;