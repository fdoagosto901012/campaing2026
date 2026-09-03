USE [SindQR];
GO

BEGIN TRANSACTION;

-- ============================================================
-- PARAMETROS DE MIGRACION
-- ============================================================

DECLARE @HotelID        INT = 25;
DECLARE @Codigo         NVARCHAR(10) = 'XO';
DECLARE @FolioInicial   INT = 99;
DECLARE @FolioFinal     INT = 200;
DECLARE @Tarifa         NVARCHAR(10) = 'NA';

-- ============================================================
-- OBTENER AUTOMATICAMENTE LA TARIFA NA DEL HOTEL
-- ============================================================

DECLARE @CouponrateID INT;

SELECT
    @CouponrateID = CR.CouponrateID
FROM dbo.Couponrates AS CR
INNER JOIN dbo.Cupontypes AS CT
    ON CT.CupontypeID = CR.CupontypeID
WHERE CR.HotelID = @HotelID
  AND CT.Name = @Tarifa;


-- ============================================================
-- VALIDACIONES PREVIAS
-- ============================================================

IF @CouponrateID IS NULL
BEGIN
    THROW 50001,
          'No existe una tarifa NA para el HotelID indicado.',
          1;
END;


IF @FolioInicial > @FolioFinal
BEGIN
    THROW 50002,
          'El FolioInicial no puede ser mayor que el FolioFinal.',
          1;
END;


-- Verificar hotel
SELECT
    HotelID,
    Name,
    shortname
FROM dbo.Hotels
WHERE HotelID = @HotelID;


-- Verificar tarifa encontrada
SELECT
    CR.CouponrateID,
    CR.HotelID,
    CT.CupontypeID,
    CT.Name AS CouponType
FROM dbo.Couponrates AS CR
INNER JOIN dbo.Cupontypes AS CT
    ON CT.CupontypeID = CR.CupontypeID
WHERE CR.CouponrateID = @CouponrateID;


-- ============================================================
-- INFORMACION DE LA MIGRACION
-- ============================================================

SELECT
    @Codigo AS Codigo,
    @HotelID AS HotelID,
    @CouponrateID AS CouponrateID,
    @FolioInicial AS FolioInicial,
    @FolioFinal AS FolioFinal,
    (@FolioFinal - @FolioInicial + 1) AS TotalCupones;


-- ============================================================
-- INSERT
-- ============================================================

INSERT INTO dbo.Cupons
(
    [Name],
    [CreateBy],
    [CreateDate],
    [EditedBy],
    [EditedDate],
    [active],
    [lock],
    [CouponratesID],
    [Paid],
    [serial_ex]
)
SELECT
    @Codigo,
    'Sistemas',
    GETDATE(),
    'Sistemas',
    GETDATE(),
    1,
    0,
    @CouponrateID,
    0,
    X.Folio
FROM
(
    SELECT
        @FolioInicial - 1
        + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Folio
    FROM sys.all_objects A
    CROSS JOIN sys.all_objects B
) X
WHERE X.Folio BETWEEN @FolioInicial AND @FolioFinal
  AND NOT EXISTS
  (
      SELECT 1
      FROM dbo.Cupons C
      WHERE C.CouponratesID = @CouponrateID
        AND C.serial_ex = X.Folio
  );


-- ============================================================
-- VALIDACION
-- ============================================================

SELECT
    @Codigo AS Codigo,
    @CouponrateID AS CouponrateID,
    COUNT(*) AS TotalInsertados,
    MIN(serial_ex) AS FolioInicial,
    MAX(serial_ex) AS FolioFinal
FROM dbo.Cupons
WHERE CouponratesID = @CouponrateID
  AND serial_ex BETWEEN @FolioInicial AND @FolioFinal;


-- ============================================================
-- REVISAR ANTES DE CONFIRMAR
-- ============================================================

-- COMMIT;
-- ROLLBACK;