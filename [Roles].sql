USE [II46];
GO

/* 1) Crear tabla si no existe (tu definición original) */
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name='Roles' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE dbo.Roles
    (
        RoleId   INT IDENTITY(1,1) NOT NULL,
        RoleName NVARCHAR(50)      NOT NULL,
        CONSTRAINT PK_Roles PRIMARY KEY CLUSTERED (RoleId)
    );
END
GO

/* 2) Asegurar columna 'Nombre':
      - Si existe RoleName y NO existe Nombre → renombrar RoleName → Nombre
      - Si no existe ninguna → agregar Nombre
*/
IF COL_LENGTH('dbo.Roles','Nombre') IS NULL
BEGIN
    IF COL_LENGTH('dbo.Roles','RoleName') IS NOT NULL
    BEGIN
        EXEC sp_rename 'dbo.Roles.RoleName', 'Nombre', 'COLUMN';
    END
    ELSE
    BEGIN
        ALTER TABLE dbo.Roles ADD Nombre NVARCHAR(50) NULL;
    END
END
GO

/* 3) Asegurar NOT NULL de 'Nombre' y rellenar si quedó NULL */
IF EXISTS (SELECT 1 FROM sys.columns
           WHERE object_id = OBJECT_ID('dbo.Roles')
             AND name = 'Nombre' AND is_nullable=1)
BEGIN
    -- Rellenar desde RoleName si existe (por si se agregó como columna nueva)
    IF COL_LENGTH('dbo.Roles','RoleName') IS NOT NULL
        UPDATE dbo.Roles SET Nombre = ISNULL(Nombre, RoleName);

    -- Rellenar con vacío si quedó NULL
    UPDATE dbo.Roles SET Nombre = N'' WHERE Nombre IS NULL;

    ALTER TABLE dbo.Roles ALTER COLUMN Nombre NVARCHAR(50) NOT NULL;
END
GO

/* 4) Normalizar valores existentes a los 3 nombres esperados */
UPDATE dbo.Roles SET Nombre = N'Administrador'
WHERE UPPER(LTRIM(RTRIM(Nombre))) IN (N'ADMIN', N'ADMINISTRADOR');

UPDATE dbo.Roles SET Nombre = N'Paciente'
WHERE UPPER(LTRIM(RTRIM(Nombre))) IN (N'RESTRINGIDO', N'PACIENTE');

UPDATE dbo.Roles SET Nombre = N'Doctor'
WHERE UPPER(LTRIM(RTRIM(Nombre))) IN (N'DOCTOR');
GO

/* 5) Crear UNIQUE sobre Nombre si falta */
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'UQ_Roles_Nombre' AND object_id = OBJECT_ID('dbo.Roles')
)
BEGIN
    ALTER TABLE dbo.Roles ADD CONSTRAINT UQ_Roles_Nombre UNIQUE (Nombre);
END
GO

/* 6) Insertar los 3 roles si faltan (no asumimos RoleId fijo) */
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE Nombre = N'Paciente')
    INSERT INTO dbo.Roles (Nombre) VALUES (N'Paciente');

IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE Nombre = N'Administrador')
    INSERT INTO dbo.Roles (Nombre) VALUES (N'Administrador');

IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE Nombre = N'Doctor')
    INSERT INTO dbo.Roles (Nombre) VALUES (N'Doctor');
GO

/* 7) Verificación */
SELECT RoleId, Nombre FROM dbo.Roles ORDER BY RoleId;
GO
