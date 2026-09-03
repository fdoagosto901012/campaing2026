/*OP_estado_de_cuenta*/
USE SindQR

DECLARE @gafete AS VARCHAR(15);

SET @gafete='17415';
SET @gafete='OP-'+@gafete;

/*datos generales del operador y su status*/
SELECT cd.CHOFER, cd.NOMBRE, cd.APELLIDOS, cd.STATUS FROM dbo.Chof_Detalle cd WHERE cd.CHOFER=@gafete

/*mensajes y bloqueos*/
SELECT m.CHOFER,m.MENSAJE,m.FECHA,CASE m.DETENER WHEN 0 THEN 'MENSAJE' ELSE 'BLOQUEO' END AS TIPO,
m.SECRETARIA FROM dbo.MENSAJES m WHERE m.CHOFER=@gafete

/*deuda actual del operador*/
SELECT I.ID_FAMILIA AS CLAVE,F.FAMILIA AS CARGO,S.SUBFAMILIA AS A, SUM(I.EXIST)AS DEBE,ROUND(SUM(I.Exist*I.PrecioConIva),2) AS IMPORTE_DEBE 
FROM INVENTARIO I 
INNER JOIN FAMILIA F ON F.ID_FAMILIA=I.ID_FAMILIA 
INNER JOIN SUBFAMILIA S ON S.ID_SUBFAMILIA=I.ID_SUBFAMILIA 
WHERE (I.ID_FAMILIA<>'36') 
AND (I.ID_PROVEEDOR=@gafete AND I.EXIST >0)  
GROUP BY I.ID_FAMILIA,F.FAMILIA,I.ID_SUBFAMILIA,S.SUBFAMILIA ORDER BY I.ID_SUBFAMILIA,I.ID_FAMILIA

/*consulta de convenios*/
SELECT c.Id_Op,c.Id_Concepto,c.Concepto,c.CargoANum,c.CargoANombre,cd.Status,COUNT(1) AS partidas,SUM(cd.Importe) AS importe FROM dbo.Convenios c
INNER JOIN dbo.ConveniosDetalle cd ON cd.Id_Op = c.Id_Op
WHERE c.CargoANum=@gafete
AND cd.Status IN ('COBRADO','VIGENTE','PAGADO')
GROUP BY c.Id_Op,c.Id_Concepto,c.Concepto,c.CargoANum,c.CargoANombre,cd.Status
ORDER BY c.Id_Op,cd.Status

/*consulta de convenios*/
SELECT c.Id_Op,c.Id_Concepto,c.Concepto,c.CargoANum,c.CargoANombre,cd.Status,COUNT(1) AS partidas,SUM(cd.Importe) AS importe FROM dbo.Convenios c
INNER JOIN dbo.ConveniosDetalle cd ON cd.Id_Op = c.Id_Op
WHERE c.CargoANum=@gafete
AND cd.Status IN ('CANCELADO')
GROUP BY c.Id_Op,c.Id_Concepto,c.Concepto,c.CargoANum,c.CargoANombre,cd.Status
ORDER BY c.Id_Op,cd.Status

/*consultas si estaen el padron de radio taxi*/
SELECT prt.* FROM dbo.padronRadioTaxi prt WHERE prt.gafete=@gafete AND prt.tipo='1'
