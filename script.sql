CREATE DATABASE BD_Condominio;
GO

USE BD_Condominio;
GO

CREATE TABLE Propietarios (
    -- Clave primaria autoincremental (Requerimiento 34)
    IdPropietario INT IDENTITY(1,1) PRIMARY KEY,
    -- Campos del propietario
    Nombre VARCHAR(120), -- Requerimiento 35
    Apellido VARCHAR(120), -- Requerimiento 36
    Torre VARCHAR(30), -- Requerimiento 37
    NumeroDepartamento VARCHAR(20), -- Requerimiento 38
    Telefono VARCHAR(15) -- Requerimiento 39
);
GO