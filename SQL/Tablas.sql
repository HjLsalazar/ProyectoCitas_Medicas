--Tabla Pacientes 
USE [II46]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE name='Pacientes')
BEGIN
    CREATE TABLE [dbo].[Pacientes](
        [PacienteId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [UsuarioId] INT NOT NULL, -- Vincula al usuario que se registró
        [Cedula] NVARCHAR(30) NOT NULL,
        [Telefono] NVARCHAR(30) NULL,
        [Direccion] NVARCHAR(200) NULL,
        CONSTRAINT [UQ_Pacientes_Cedula] UNIQUE([Cedula]),
        CONSTRAINT [FK_Pacientes_Usuarios] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuarios]([Id])
    );

    CREATE INDEX IX_Pacientes_Usuario ON [dbo].[Pacientes]([UsuarioId]);
END
GO


--Tabla Doctores
IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE name='Doctores')
BEGIN
    CREATE TABLE [dbo].[Doctores](
        [DoctorId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Nombre] NVARCHAR(100) NOT NULL,
        [Especialidad] NVARCHAR(100) NOT NULL,
        [Correo] NVARCHAR(100) NOT NULL,
        [Telefono] NVARCHAR(30) NULL,
        CONSTRAINT [UQ_Doctores_Correo] UNIQUE([Correo])
    );

    CREATE INDEX IX_Doctores_Especialidad ON [dbo].[Doctores]([Especialidad]);
END
GO

--Tabla Citas

IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE name='Citas')
BEGIN
    CREATE TABLE [dbo].[Citas](
        [CitaId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [PacienteId] INT NOT NULL,
        [DoctorId] INT NOT NULL,
        [FechaHora] DATETIME NOT NULL,        -- inicio de la cita
        [DuracionMinutos] INT NOT NULL,       -- p.ej. 30
        [Motivo] NVARCHAR(200) NULL,
        [Estado] NVARCHAR(30) NOT NULL DEFAULT('Pendiente'), -- Pendiente, Confirmada, Cancelada, Atendida
        CONSTRAINT [FK_Citas_Pacientes] FOREIGN KEY ([PacienteId]) REFERENCES [dbo].[Pacientes]([PacienteId]),
        CONSTRAINT [FK_Citas_Doctores] FOREIGN KEY ([DoctorId]) REFERENCES [dbo].[Doctores]([DoctorId])
    );

    -- Evitar doble reserva del mismo doctor a la misma hora exacta:
    CREATE UNIQUE INDEX UX_Citas_Doctor_FechaHora ON [dbo].[Citas]([DoctorId],[FechaHora]);
    CREATE INDEX IX_Citas_Paciente ON [dbo].[Citas]([PacienteId]);
END
GO

USE II46;
GO

-- Admin
UPDATE dbo.Usuarios SET RoleId = 2 WHERE Email = 'Hernan@lindo.com';

-- Pacientes
UPDATE dbo.Usuarios SET RoleId = 1 WHERE Email = 'juan@perez.com';
UPDATE dbo.Usuarios SET RoleId = 1 WHERE Email = 'ana@molina.com';
GO

-- Verificar
SELECT ID, Nombre, Apellidos, Email, RoleId
FROM dbo.Usuarios
WHERE Email IN ('Hernan@lindo.com','juan@perez.com','ana@molina.com');

-- Doctores de prueba
INSERT INTO dbo.Doctores (Nombre, Especialidad, Correo, Telefono)
VALUES ('Dra. Sofía Lozano','Cardiología','sofia@clinica.com','555-0101'),
       ('Dr. Marco Díaz','Dermatología','marco@clinica.com','555-0102');

-- Fichas de Paciente para los usuarios juan y ana
INSERT INTO dbo.Pacientes (UsuarioId, Cedula, Telefono, Direccion)
SELECT U.ID, 'P-0001', '555-1001', 'Calle 1 #100' FROM dbo.Usuarios U WHERE U.Email='juan@perez.com';

INSERT INTO dbo.Pacientes (UsuarioId, Cedula, Telefono, Direccion)
SELECT U.ID, 'P-0002', '555-1002', 'Calle 2 #200' FROM dbo.Usuarios U WHERE U.Email='ana@molina.com';

 select * from Usuarios
 select * from Pacientes
 select * from Doctores
 select * from Roles
 select * from Citas