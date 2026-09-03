/*SOC_estado_de_cuenta*/
USE SindQR

DECLARE @economico AS VARCHAR(15);

SET @economico='3565';

/*datos generales del socio*/
SELECT * FROM dbo.Soc_DatPersonales sdp WHERE sdp.Numero=@economico

/*mensajes y bloqueos*/
SELECT m.TAXI, m.CHOFER,m.MENSAJE,m.FECHA,CASE m.DETENER WHEN 0 THEN 'MENSAJE' ELSE 'BLOQUEO' END AS TIPO,m.SECRETARIA FROM dbo.MENSAJES m WHERE m.CHOFER=@economico OR m.TAXI=@economico

/*deuda actual del socio*/
SELECT I.ID_FAMILIA AS CLAVE,F.FAMILIA AS CARGO,S.SUBFAMILIA AS A, SUM(I.EXIST)AS DEBE,ROUND(SUM(I.Exist*I.PrecioConIva),2) AS IMPORTE_DEBE 
FROM INVENTARIO I 
INNER JOIN FAMILIA F ON F.ID_FAMILIA=I.ID_FAMILIA 
INNER JOIN SUBFAMILIA S ON S.ID_SUBFAMILIA=I.ID_SUBFAMILIA 
WHERE (I.ID_FAMILIA NOT IN ('36','0')) 
AND (I.ID_PROVEEDOR=@economico AND I.EXIST >0)  
GROUP BY I.ID_FAMILIA,F.FAMILIA,I.ID_SUBFAMILIA,S.SUBFAMILIA ORDER BY I.ID_SUBFAMILIA,I.ID_FAMILIA

/*consulta de convenios*/
SELECT c.Id_Op,c.Id_Concepto,c.Concepto,c.CargoANum,c.CargoANombre,cd.Status,COUNT(1) AS partidas,SUM(cd.Importe) AS importe FROM dbo.Convenios c
INNER JOIN dbo.ConveniosDetalle cd ON cd.Id_Op = c.Id_Op
WHERE c.CargoANum=@economico
AND cd.Status IN ('COBRADO','VIGENTE','PAGADO')
GROUP BY c.Id_Op,c.Id_Concepto,c.Concepto,c.CargoANum,c.CargoANombre,cd.Status
ORDER BY c.Id_Op,cd.Status

/*consulta de convenios*/
SELECT c.Id_Op,c.Id_Concepto,c.Concepto,c.CargoANum,c.CargoANombre,cd.Status,COUNT(1) AS partidas,SUM(cd.Importe) AS importe FROM dbo.Convenios c
INNER JOIN dbo.ConveniosDetalle cd ON cd.Id_Op = c.Id_Op
WHERE c.CargoANum=@economico
AND cd.Status IN ('CANCELADO')
GROUP BY c.Id_Op,c.Id_Concepto,c.Concepto,c.CargoANum,c.CargoANombre,cd.Status
ORDER BY c.Id_Op,cd.Status

