select a.Numero, UPPER(REPLACE((CONCAT(a.Nombre, a.Paterno, a.Materno)), ' ','')) as nombrePracti, UPPER(REPLACE((CONCAT(u.firstName, u.lastNameF, u.lastNameM)), ' ','')) as nombrePadron from Soc_DatPersonales as a 
left join [user] as u on u.partnerReference = CONVERT(varchar, a.Numero)
where u.active = 1 and
UPPER(REPLACE((CONCAT(u.firstName, u.lastNameF, u.lastNameM)), ' ','')) <> UPPER(REPLACE((CONCAT(a.Nombre, a.Paterno, a.Materno)), ' ',''))
order by a.Numero asc

update [user] set active = 0
where partnerReference = '2922';

select * from [user] where partnerReference = '2922';