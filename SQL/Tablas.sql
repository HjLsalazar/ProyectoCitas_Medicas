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
