-- Crear tabla de Tipos de Vehículo  
CREATE TABLE TipoVehiculo (  
    Id INT PRIMARY KEY IDENTITY(1,1),  
    Nombre NVARCHAR(50) NOT NULL,  
    CapacidadPax INT NOT NULL,

	CreateBy nvarchar(50) NOT NULL DEFAULT '',
	CreateDate Datetime NOT NULL DEFAULT getdate(),
	EditBy nvarchar(50) ,
	EditDate Datetime 
);  
GO  
 

-- Insertar datos en la tabla de Tipos de Vehículo  
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('A', 1);
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('A', 2); 
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('A', 3); 
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('A', 4); 
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('A', 5); 
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('A', 6); 
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('A', 7); 
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('A', 8); 
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('A', 9); 
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('A', 10); 
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('C', 1);
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('C', 2);
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('C', 3);
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('C', 4);
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('C', 5);
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('C', 6);
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('C', 8);
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('C', 9);
INSERT INTO TipoVehiculo (Nombre, CapacidadPax) VALUES ('C', 10);
GO  
  
-- Crear tabla de Tickets  
CREATE TABLE Ticket (  
    Id INT PRIMARY KEY IDENTITY(1,1),  
    Folio NVARCHAR(20) UNIQUE NOT NULL,  
    IdTipoVehiculo INT NOT NULL,  
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),  
    FOREIGN KEY (IdTipoVehiculo) REFERENCES TipoVehiculo(Id),

	CreateBy nvarchar(50) NOT NULL DEFAULT '',
	CreateDate Datetime NOT NULL DEFAULT getdate(),
	EditBy nvarchar(50) ,
	EditDate Datetime   
);  
GO  

-- Crear tabla de Hoteles/Restaurantes  
CREATE TABLE  Zones(  
    Id INT PRIMARY KEY IDENTITY(1,1),  
    Nombre NVARCHAR(100) NOT NULL,

	CreateBy nvarchar(50) NOT NULL DEFAULT '',
	CreateDate Datetime NOT NULL DEFAULT getdate(),
	EditBy nvarchar(50) ,
	EditDate Datetime   
);  
GO  

-- Crear tabla de Hoteles/Restaurantes  
CREATE TABLE Establecimientos (  
    Id INT PRIMARY KEY IDENTITY(1,1),  
    Nombre NVARCHAR(100) NOT NULL,

	CreateBy nvarchar(50) NOT NULL DEFAULT '',
	CreateDate Datetime NOT NULL DEFAULT getdate(),
	EditBy nvarchar(50) ,
	EditDate Datetime   
);  
GO  
  
-- Crear tabla de Costos de Tickets  
CREATE TABLE TicketDetalle (  
    Id INT PRIMARY KEY IDENTITY(1,1),  
    IdTicket INT NOT NULL,  
    IdHotelRestaurante INT NOT NULL,  
    Costo DECIMAL(10, 2) NOT NULL,  
    FOREIGN KEY (IdTicket) REFERENCES Ticket(Id),  
    FOREIGN KEY (IdHotelRestaurante) REFERENCES Establecimientos(Id),

	CreateBy nvarchar(50) NOT NULL DEFAULT '',
	CreateDate Datetime NOT NULL DEFAULT getdate(),
	EditBy nvarchar(50) ,
	EditDate Datetime   
);  
GO  

-- Crear tabla de Hoteles/Restaurantes  
CREATE TABLE  Esquemaempresas(  
    Id INT PRIMARY KEY IDENTITY(1,1),  
    Nombre NVARCHAR(100) NOT NULL,
	IdHotelRestaurante INT NOT NULL, 
	FOREIGN KEY (IdHotelRestaurante) REFERENCES Establecimientos(Id),
	IdTipoVehiculo INT NOT NULL, 
	FOREIGN KEY (IdTipoVehiculo) REFERENCES TipoVehiculo(Id),
	IdZones INT NOT NULL, 
	FOREIGN KEY (IdZones) REFERENCES Zones(Id),
	CONSTRAINT UQ_x_IdHotelRestaurante_IdTipoVehiculo_IdZones UNIQUE (IdHotelRestaurante, IdTipoVehiculo, IdZones),

	CreateBy nvarchar(50) NOT NULL DEFAULT '',
	CreateDate Datetime NOT NULL DEFAULT getdate(),
	EditBy nvarchar(50) ,
	EditDate Datetime 
);  
GO  

  
-- Crear índices para optimizar búsquedas  
CREATE INDEX IDX_esquema ON Esquemaempresas(Id);  
CREATE INDEX IDX_Folio ON Ticket(Folio); 
CREATE INDEX IDX_Id ON Ticket(Id);  
CREATE INDEX IDX_IdTicketDetalle ON TicketDetalle(Id);  
CREATE INDEX IDX_IdHotelRestaurante ON TicketDetalle(IdHotelRestaurante);  
GO  