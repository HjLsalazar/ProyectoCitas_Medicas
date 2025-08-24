Imports System.Data.SqlClient
Imports LoginRoles.Helpers

Public Class Paciente
    Public Property PacienteId As Integer
    Public Property UsuarioId As Integer
    Public Property Cedula As String
    Public Property Telefono As String
    Public Property Direccion As String

    ' Constructor por vacio
    Public Sub New()

    End Sub

    ' Constructor con parametros
    Public Sub New(pacienteId As Integer, usuarioId As Integer, cedula As String, telefono As String, direccion As String)
        Me.PacienteId = pacienteId
        Me.UsuarioId = usuarioId
        Me.Cedula = cedula
        Me.Telefono = telefono
        Me.Direccion = direccion
    End Sub

    ' Metodo para obtener todos los pacientes con su informacion de usuario asociada
    Public Shared Function GetAll() As DataTable
        Dim db = New DatabaseHelper()
        Dim sql = "SELECT P.PacienteId, P.UsuarioId, U.Email, U.Nombre, U.Apellidos, P.Cedula, P.Telefono, P.Direccion
                       FROM Pacientes P
                       INNER JOIN Usuarios U ON U.Id = P.UsuarioId
                       ORDER BY P.PacienteId DESC"
        Return db.ExecuteQuery(sql)
    End Function

    ' Metodo para obtener un paciente por su ID
    Public Shared Function GetByUsuarioId(usuarioId As Integer) As DataTable
        Dim db = New DatabaseHelper()
        Dim sql = "SELECT TOP 1 * FROM Pacientes WHERE UsuarioId=@UsuarioId"
        Dim p = New List(Of SqlParameter) From {db.CreateParameter("@UsuarioId", usuarioId)}
        Return db.ExecuteQuery(sql, p)
    End Function

    ' Metodo para insertar un nuevo paciente
    Public Function Insert() As Boolean
        Dim db = New DatabaseHelper()
        Dim sql = "INSERT INTO Pacientes (UsuarioId, Cedula, Telefono, Direccion) 
                   VALUES (@UsuarioId, @Cedula, @Telefono, @Direccion)"
        Dim parameters = New List(Of SqlParameter) From {
            db.CreateParameter("@UsuarioId", Me.UsuarioId),
            db.CreateParameter("@Cedula", Me.Cedula),
            db.CreateParameter("@Telefono", Me.Telefono),
            db.CreateParameter("@Direccion", Me.Direccion)
        }
        Return db.ExecuteNonQuery(sql, parameters) > 0
    End Function

    ' Metodo para actualizar un paciente existente
    Public Function Update() As Boolean
        Dim db = New DatabaseHelper()
        Dim sql = "UPDATE Pacientes 
                   SET Cedula=@Cedula, Telefono=@Telefono, Direccion=@Direccion 
                   WHERE PacienteId=@PacienteId"
        Dim parameters = New List(Of SqlParameter) From {
            db.CreateParameter("@Cedula", Me.Cedula),
            db.CreateParameter("@Telefono", Me.Telefono),
            db.CreateParameter("@Direccion", Me.Direccion),
            db.CreateParameter("@PacienteId", Me.PacienteId)
        }
        Return db.ExecuteNonQuery(sql, parameters) > 0
    End Function

    ' Metodo para eliminar un paciente por su ID
    Public Shared Function Delete(pacienteId As Integer) As Boolean
        Dim db = New DatabaseHelper()
        Dim sql = "DELETE FROM Pacientes WHERE PacienteId=@PacienteId"
        Dim parameters = New List(Of SqlParameter) From {
            db.CreateParameter("@PacienteId", pacienteId)
        }
        Return db.ExecuteNonQuery(sql, parameters) > 0
    End Function



End Class
