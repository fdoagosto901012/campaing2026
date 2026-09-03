USE [SindQR];
GO
BEGIN TRANSACTION;


select * from Cupons;



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
    'AQ',
    'Sistemas',
    GETDATE(),
    'Sistemas',
    GETDATE(),
    1,
    0,
    338,
    0,
    X.Folio
FROM
(
    SELECT
        330350 + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Folio
    FROM sys.all_objects A
    CROSS JOIN sys.all_objects B
) X
WHERE X.Folio BETWEEN 330351 AND 333200
  AND NOT EXISTS
  (
      SELECT 1
      FROM dbo.Cupons C
      WHERE C.CouponratesID = 338
        AND C.serial_ex = X.Folio
  );





select * from Cupons;


-- migración

-- validaciones

-- COMMIT / ROLLBACK