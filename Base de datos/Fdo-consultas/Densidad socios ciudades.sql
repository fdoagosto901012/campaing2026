with a as (
 select u.partnerReference, u.partnerTypeId, addres.* from [user] as u
 CROSS APPLY
        (
        SELECT  TOP 1 *
        FROM    userAddress as ua
        WHERE   u.id = ua.userId and ua.active = 1 and u.active = 1 and u.partnerReference not like 'OP-%' and u.partnerTypeId = 1
        ) addres
  
)
select c.[name] as Ciudad, count(-1) as Habitantes from a as u
left join city as c on u.cityId = c.id
group by c.[name]
order by c.[name];