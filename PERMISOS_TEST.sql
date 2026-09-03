select * from Consecutivos as c
where Documento   like '%OPERADORES%';
select CONVERT(VARchar(11), MAX(FECHAREPORTE)) from HISTASISTENCIAS WHERE GAFETE='OP-38211';
select top 20 * from HistAsistencias where Gafete = 'OP-38211';

select top 100 * from Inventario where Id_Proveedor = 'OP-1000035' order by FechaOp desc;

select top 10 * from HistAsistencias;