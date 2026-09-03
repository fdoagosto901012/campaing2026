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
    'AM',
    'Sistemas',
    GETDATE(),
    'Sistemas',
    GETDATE(),
    1,
    0,
    350,
    0,
    X.Folio
FROM
(
    SELECT TOP (565)
        1435 + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Folio
    FROM sys.all_objects A
    CROSS JOIN sys.all_objects B
) X
ORDER BY X.Folio;


select * from Cupons;


-- migración

-- validaciones

-- COMMIT / ROLLBACK