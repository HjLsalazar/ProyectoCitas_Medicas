
IF DB_ID('II46') IS NULL
BEGIN
    CREATE DATABASE II46;
END
GO

USE [II46];
GO



IF NOT EXISTS (SELECT 1
               FROM sys.tables
               WHERE name = 'Usuarios' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[Usuarios](
        [ID]        INT IDENTITY(1,1) NOT NULL,
        [Nombre]    NVARCHAR(100)     NOT NULL,
        [Apellidos] NVARCHAR(100)     NOT NULL,
        [Email]     NVARCHAR(100)     NOT NULL,
        [Pass]      NVARCHAR(100)     NOT NULL,
        [RoleId]    INT               NULL,
        CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED ([ID] ASC)
    );

    -- UNIQUE para Email 
    ALTER TABLE [dbo].[Usuarios]
        ADD CONSTRAINT [UQ_Usuarios_Email] UNIQUE NONCLUSTERED ([Email]);

    -- FK a Roles
    ALTER TABLE [dbo].[Usuarios] WITH CHECK
        ADD CONSTRAINT [FK_Usuarios_Roles]
        FOREIGN KEY([RoleId]) REFERENCES [dbo].[Roles]([RoleId]);

    ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_Roles];
END
ELSE
BEGIN
    /* --- Asegurar columnas (agrega si faltan) --- */
    IF COL_LENGTH('dbo.Usuarios','Nombre')    IS NULL ALTER TABLE dbo.Usuarios ADD Nombre    NVARCHAR(100) NULL;  -- se ajusta a NOT NULL abajo
    IF COL_LENGTH('dbo.Usuarios','Apellidos') IS NULL ALTER TABLE dbo.Usuarios ADD Apellidos NVARCHAR(100) NULL;
    IF COL_LENGTH('dbo.Usuarios','Email')     IS NULL ALTER TABLE dbo.Usuarios ADD Email     NVARCHAR(100) NULL;
    IF COL_LENGTH('dbo.Usuarios','Pass')      IS NULL ALTER TABLE dbo.Usuarios ADD Pass      NVARCHAR(100) NULL;
    IF COL_LENGTH('dbo.Usuarios','RoleId')    IS NULL ALTER TABLE dbo.Usuarios ADD RoleId    INT NULL;

    /* --- Forzar NOT NULL en las columnas obligatorias (si estaban NULL) --- */
 
    IF EXISTS (SELECT 1 FROM sys.columns
               WHERE object_id = OBJECT_ID('dbo.Usuarios') AND name='Nombre' AND is_nullable=1)
        ALTER TABLE dbo.Usuarios ALTER COLUMN Nombre NVARCHAR(100) NOT NULL;

    IF EXISTS (SELECT 1 FROM sys.columns
               WHERE object_id = OBJECT_ID('dbo.Usuarios') AND name='Apellidos' AND is_nullable=1)
        ALTER TABLE dbo.Usuarios ALTER COLUMN Apellidos NVARCHAR(100) NOT NULL;

    IF EXISTS (SELECT 1 FROM sys.columns
               WHERE object_id = OBJECT_ID('dbo.Usuarios') AND name='Email' AND is_nullable=1)
        ALTER TABLE dbo.Usuarios ALTER COLUMN Email NVARCHAR(100) NOT NULL;

    IF EXISTS (SELECT 1 FROM sys.columns
               WHERE object_id = OBJECT_ID('dbo.Usuarios') AND name='Pass' AND is_nullable=1)
        ALTER TABLE dbo.Usuarios ALTER COLUMN Pass NVARCHAR(100) NOT NULL;

    /* --- UNIQUE(Email) si aún no existe --- */
    IF NOT EXISTS (
        SELECT 1
        FROM sys.indexes
        WHERE name = 'UQ_Usuarios_Email'
          AND object_id = OBJECT_ID('dbo.Usuarios')
    )
    BEGIN
        ALTER TABLE [dbo].[Usuarios]
            ADD CONSTRAINT [UQ_Usuarios_Email] UNIQUE NONCLUSTERED ([Email]);
    END

    /* --- PK a Roles si aún no existe --- */
    IF NOT EXISTS (
        SELECT 1 FROM sys.foreign_keys
        WHERE name = 'PK_Usuarios_Roles' AND parent_object_id = OBJECT_ID('dbo.Usuarios')
    )
    BEGIN
        ALTER TABLE [dbo].[Usuarios]  WITH CHECK
            ADD CONSTRAINT [PK_Usuarios_Roles]
            FOREIGN KEY([RoleId]) REFERENCES [dbo].[Roles]([RoleId]);

        ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [PK_Usuarios_Roles];
    END
END
GO

/* (Opcional) Verificación rápida */
SELECT TOP(5) * FROM dbo.Usuarios ORDER BY ID DESC;
GO
