USE campaing;
GO

SET NOCOUNT ON;

------------------------------------------------------------
-- 1. Tabla temporal
------------------------------------------------------------

DROP TABLE IF EXISTS #ExcelImport;

CREATE TABLE #ExcelImport
(
    ArchivoOrigen NVARCHAR(255) NULL,
    HojaOrigen NVARCHAR(255) NULL,
    FilaExcel INT NULL,
    Sector NVARCHAR(100) NULL,
    Folio INT NULL,
    Perforacion NVARCHAR(100) NULL
);

------------------------------------------------------------
-- 2. Cargar CSV
------------------------------------------------------------

BULK INSERT #ExcelImport
FROM 'C:\CuponesExcelExtraidos.csv'
WITH
(
    FORMAT = 'CSV',
    FIRSTROW = 2,
    FIELDQUOTE = '"',
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '0x0a',
    CODEPAGE = '65001',
    TABLOCK
);

------------------------------------------------------------
-- 3. Verificar cuántos registros llegaron del Excel
------------------------------------------------------------
insert into CuponesConciliacion_Import (Sector, Folio, Perforacion)
SELECT Sector, Folio, Perforacion
FROM #ExcelImport
group by Sector, Folio, Perforacion;

select * from  CuponesConciliacion_Import;