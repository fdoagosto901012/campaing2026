SELECT SUM(CASE
        WHEN ABS(Id_Familia) IN(0, 113)
        THEN Exist
        ELSE 0
    END) AS faltas,
SUM(CASE
        WHEN ABS(Id_Familia) IN(28, 105)
        THEN Exist * PrecioConIva
        ELSE 0
    END) AS cuotas_fdi,
SUM(CASE
        WHEN ABS(Id_Familia) NOT IN(0, 113, 28, 105, 103)
        THEN Exist * PrecioConIva
        ELSE 0
    END) AS otros_cargos
    FROM Inventario
WHERE Id_Proveedor = 'OP-57759' AND Exist > 0