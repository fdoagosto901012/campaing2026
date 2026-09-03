SELECT I.ID_FAMILIA AS CLAVE, F.FAMILIA AS CARGO,I.ID_SUBFAMILIA AS IDA, 0 AS PAGA, I.PrecioConIva as PRECIO, I.Descripcion, I.FechaOp
from Inventario as I
INNER JOIN Familia as f on I.Id_Familia = f.Id_Familia
WHERE 
I.Id_Proveedor = '8624' and I.Id_Familia = '90' or 
I.Id_Proveedor = 'OP-38211' and I.Id_Familia = '90';