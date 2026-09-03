SELECT e.Id_Op, e.No_Economico, e.Nombre, e.Operacion, ema.Marca, em.Modelo FROM [SindQR].[dbo].[Emplacamiento] as e 
left join EmplaModelo as em on em.Id_Modelo = e.Id_Modelo
left join EmplaMarca as ema on ema.Id_Marca = e.Id_Marca
where [No_Economico] = '6039.00';

select * from dbo.[user] where partnerReference = '6039';

update Emplacamiento set Operacion = 'HISTORICO'
where Id_Op in ('134170.00','138388.00','138390.00');