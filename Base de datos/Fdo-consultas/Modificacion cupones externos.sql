--ALTER TABLE Cupons ADD serial_ex INT NULL;
select * from cupons;
select * from Hotels;

Declare @HotelId INT = 100;
DECLARE @SerialFrom INT = 36701;
DECLARE @SerialTo INT = 40000;

;WITH Numeros AS
(
    SELECT @SerialFrom AS Serial

    UNION ALL

    SELECT Serial + 1
    FROM Numeros
    WHERE Serial < @SerialTo
)
--INSERT INTO Cupons (Serial)
SELECT Serial
FROM Numeros
OPTION (MAXRECURSION 0);