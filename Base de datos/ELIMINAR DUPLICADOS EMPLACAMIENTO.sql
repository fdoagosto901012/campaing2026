SELECT * FROM [SindQR].[dbo].[Emplacamiento] where [No_Economico] = '6039.00';

select * from dbo.[user] where partnerReference = '6039';

select * from dbo.emplaBitacora where id_emplacamiento = '134170.00';
select * from dbo.emplaBitacora where id_emplacamiento = '138388.00';
select * from dbo.emplaBitacora where id_emplacamiento = '138390.00';
select * from dbo.emplaBitacora where id_emplacamiento = '138541.00';

DELETE from dbo.emplaBitacora where id in (17856, 17560, 17845, 17848, 17853, 17854, 17855, 17867);

DELETE from dbo.emplaBitacora where id in (17869, 17870);

delete Emplacamiento where Id_Op in ('134170.00','138388.00','138390.00');