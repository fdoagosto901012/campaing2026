select * from Inventario
where Id_Proveedor = 'OP-58439' and
Descripcion like '%PERMISO EXPRESS%';

update Inventario set CostoPromedio = 600, CostoUltimo = 600, Precio = 600, PrecioConIva = 600
where Id_Producto = '140.402.487';

update Inventario set CostoPromedio = 600, CostoUltimo = 600, Precio = 600, PrecioConIva = 600
where Id_Producto = '140.975.715';