USE [SindQR]
GO
/****** Object:  StoredProcedure [dbo].[fdo_cuotas_auto]    Script Date: 04/06/2026 02:17:00 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[fdo_cuotas_auto]
(
@Preview BIT = 1
)
AS
BEGIN
SET NOCOUNT ON;

DECLARE @IdProducto VARCHAR(50);
DECLARE @IdProductoTTE VARCHAR(50);
DECLARE @Siguiente VARCHAR(50);
DECLARE @SiguienteTTE VARCHAR(50);
DECLARE @MesActual VARCHAR(20);
DECLARE @AnioActual VARCHAR(4);
DECLARE @Descripcion VARCHAR(100);
DECLARE @Fecha VARCHAR(8);

SET @Fecha = CONVERT(VARCHAR(8), GETDATE(), 112);
SET @AnioActual = CAST(YEAR(GETDATE()) AS VARCHAR(4));
SET @MesActual = UPPER(FORMAT(GETDATE(), 'MMMM', 'es-MX'));

SET @Descripcion =
    'Cuota Sindical De '
    + UPPER(LEFT(@MesActual, 1))
    + LOWER(SUBSTRING(@MesActual, 2, LEN(@MesActual)))
    + ' '
    + @AnioActual;

IF EXISTS
(
    SELECT TOP 1 1
    FROM INVENTARIO
    WHERE DESCRIPCION = @Descripcion
)
BEGIN
    PRINT 'La cuota ya fue generada para este mes.';
    RETURN;
END;


-- Obtiene el último producto de cuotas sindicales
SELECT TOP 1
    @IdProducto = ID_PRODUCTO
FROM INVENTARIO
WHERE ID_PRODUCTO LIKE '10.%.1'
    AND ID_FAMILIA = 12
ORDER BY
    CAST(PARSENAME(ID_PRODUCTO, 3) AS INT) DESC,
    CAST(PARSENAME(ID_PRODUCTO, 2) AS INT) DESC,
    CAST(PARSENAME(ID_PRODUCTO, 1) AS INT) DESC;

-- Obtiene el último producto de cuotas TTE
SELECT TOP 1
    @IdProductoTTE = ID_PRODUCTO
FROM INVENTARIO
WHERE ID_PRODUCTO LIKE '11.1.%'
    AND ID_FAMILIA = 12
ORDER BY
    CAST(PARSENAME(ID_PRODUCTO, 3) AS INT) DESC,
    CAST(PARSENAME(ID_PRODUCTO, 2) AS INT) DESC,
    CAST(PARSENAME(ID_PRODUCTO, 1) AS INT) DESC;

SET @Siguiente =
    CONCAT(
        PARSENAME(@IdProducto, 3),
        '.',
        CAST(PARSENAME(@IdProducto, 2) AS INT) + 1,
        '.',
        PARSENAME(@IdProducto, 1)
    );

SET @SiguienteTTE =
    CONCAT(
        PARSENAME(@IdProductoTTE, 3),
        '.',
        PARSENAME(@IdProductoTTE, 2),
        '.',
        CAST(PARSENAME(@IdProductoTTE, 1) AS INT) + 1
    );

IF @Preview = 1
BEGIN
    SELECT
        @IdProducto AS Actual,
        @Siguiente AS Siguiente,
        @IdProductoTTE AS ActualTTE,
        @SiguienteTTE AS SiguienteTTE,
        @Descripcion AS Descripcion;

    ;WITH soc AS
    (
        SELECT Numero
        FROM SOC_DATPERSONALES
        WHERE
            NUMERO <> '0'
            AND (
                (NUMERO BETWEEN 0 AND 9500)
                OR (NUMERO BETWEEN 80000 AND 81000)
            )
    )
    SELECT
        '1' AS ID_TIENDA,
        @Siguiente AS ID_PRODUCTO,
        @Descripcion AS DESCRIPCION,
        'PRO' AS TIPO,
        NUMERO AS ID_PROVEEDOR,
        '2' AS ID_DEPTO,
        '12' AS ID_FAMILIA,
        '2' AS ID_SUBFAMILIA,
        '' AS ID_LOGO,
        0 AS CONSIGNADO,
        0 AS TIPOCOMISION,
        'PZA' AS UNIDADENT,
        'PZA' AS UNIDADSAL,
        0 AS FACTOR,
        1 AS EXIST,
        0 AS TIEMPOSURTIDO,
        0 AS MINIMO,
        0 AS MAXIMO,
        10 AS TASAIVA,
        10 AS TASAIVACOMPRA,
        20 AS COSTOPROMEDIO,
        20 AS COSTOULTIMO,
        0 AS TASAIMPESP,
        172.41 AS PRECIO,
        200 AS PRECIOCONIVA,
        0 AS DESCUENTO,
        @Fecha AS FECHAOP,
        '10-1' AS ID_USUARIO,
        '' AS COLOR,
        'ACTIVO' AS STATUS
    FROM soc;

    RETURN;
END;

BEGIN TRY

    BEGIN TRANSACTION;

    ;WITH soc AS
    (
        SELECT Numero
        FROM SOC_DATPERSONALES
        WHERE
            NUMERO <> '0'
            AND (
                (NUMERO BETWEEN 0 AND 9500)
                OR (NUMERO BETWEEN 80000 AND 81000)
            )
    )
    INSERT INTO INVENTARIO
    (
        ID_TIENDA, ID_PRODUCTO, DESCRIPCION, TIPO,
        ID_PROVEEDOR, ID_DEPTO, ID_FAMILIA, ID_SUBFAMILIA,
        ID_LOGO, CONSIGNADO, TIPOCOMISION,
        UNIDADENT, UNIDADSAL, FACTOR, EXIST,
        TIEMPOSURTIDO, MINIMO, MAXIMO,
        TASAIVA, TASAIVACOMPRA,
        COSTOPROMEDIO, COSTOULTIMO,
        TASAIMPESP, PRECIO, PRECIOCONIVA,
        DESCUENTO, FECHAOP, ID_USUARIO,
        COLOR, STATUS
    )
    SELECT
        '1', @Siguiente, @Descripcion, 'PRO',
        NUMERO, '2', '12', '2',
        '', 0, 0,
        'PZA', 'PZA', 0, 1,
        0, 0, 0,
        10, 10,
        20, 20,
        0, 172.41, 200,
        0, @Fecha, '10-1',
        '', 'ACTIVO'
    FROM soc;

    ;WITH tte AS
    (
        SELECT Numero
        FROM SOC_DATPERSONALES
        WHERE
            NUMERO <> '0'
            AND (NUMERO BETWEEN 10000 AND 12000)
            AND ASIGNACION NOT IN ('SECCION', 'SECCION3')
    )
    INSERT INTO INVENTARIO
    (
        ID_TIENDA, ID_PRODUCTO, DESCRIPCION, TIPO,
        ID_PROVEEDOR, ID_DEPTO, ID_FAMILIA, ID_SUBFAMILIA,
        ID_LOGO, CONSIGNADO, TIPOCOMISION,
        UNIDADENT, UNIDADSAL, FACTOR, EXIST,
        TIEMPOSURTIDO, MINIMO, MAXIMO,
        TASAIVA, TASAIVACOMPRA,
        COSTOPROMEDIO, COSTOULTIMO,
        TASAIMPESP, PRECIO, PRECIOCONIVA,
        DESCUENTO, FECHAOP, ID_USUARIO,
        COLOR, STATUS
    )
    SELECT
        '1', @SiguienteTTE, @Descripcion, 'PRO',
        NUMERO, '2', '12', '2',
        '', 0, 0,
        'PZA', 'PZA', 0, 1,
        0, 0, 0,
        16, 16,
        0, 0,
        0, 258.63, 600,
        0, @Fecha, '10-1',
        '', 'ACTIVO'
    FROM tte;

    COMMIT TRANSACTION;

    SELECT
        'Proceso completado correctamente.' AS Mensaje,
        @Siguiente AS ProductoSocios,
        @SiguienteTTE AS ProductoTTE;

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

END CATCH

END
