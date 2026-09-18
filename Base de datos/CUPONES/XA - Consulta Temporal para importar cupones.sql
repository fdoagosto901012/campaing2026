DROP TABLE IF EXISTS dbo.CuponesConciliacion_Import;
GO

CREATE TABLE dbo.CuponesConciliacion_Import
(
    Id BIGINT IDENTITY(1,1) NOT NULL
        CONSTRAINT PK_CuponesConciliacion_Import PRIMARY KEY,

    ArchivoOrigen NVARCHAR(255) NULL,

    HojaOrigen NVARCHAR(255) NULL,

    HotelCodigo NVARCHAR(50) NULL,

    Folio INT NULL,

    Tarifa DECIMAL(18,2) NULL,

    FechaCarga DATETIME2(0) NOT NULL
        CONSTRAINT DF_CuponesConciliacion_Import_FechaCarga
        DEFAULT SYSDATETIME()
);
GO