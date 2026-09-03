USE [SindQR];
GO
BEGIN TRANSACTION;
SELECT
    cr.CouponrateID,
    cr.HotelID,
    h.Name AS Hotel,
    h.ShortName,
    cr.CupontypeID,
    ct.Name AS CouponType
FROM Couponrates cr
INNER JOIN Hotels h
    ON h.HotelID = cr.HotelID
INNER JOIN Cupontypes ct
    ON ct.CupontypeID = cr.CupontypeID
WHERE ct.Name = 'NA';


-- SIMULACION: 

SELECT
    'XC' AS Name,
    'Sistemas' AS CreateBy,
    GETDATE() AS CreateDate,
    'Sistemas' AS EditedBy,
    GETDATE() AS EditedDate,
    CAST(1 AS bit) AS active,
    CAST(0 AS bit) AS [lock],
    334 AS CouponratesID,
    CAST(0 AS bit) AS Paid,
    Folio AS serial_ex
FROM
(
    SELECT TOP (3300)
        36700 + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Folio
    FROM sys.all_objects a
    CROSS JOIN sys.all_objects b
) AS X
ORDER BY Folio;




INSERT INTO [dbo].[Cupons]
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
    'XC',
    'Sistemas',
    GETDATE(),
    'Sistemas',
    GETDATE(),
    1,
    0,
    334,
    0,
    X.Folio
FROM
(
    SELECT TOP (3300)
        36700 + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Folio
    FROM sys.all_objects a
    CROSS JOIN sys.all_objects b
) AS X
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.Cupons C
    WHERE C.CouponratesID = 334
      AND C.serial_ex = X.Folio
);



select * from Cupons;


-- migración

-- validaciones

-- COMMIT / ROLLBACK