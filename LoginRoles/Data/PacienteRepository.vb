Imports System.Data
Imports System.Data.SqlClient
Imports LoginRoles.Helpers

Namespace Data
    Public Class PacienteRepository
        ' Repositorio de pacientes
        Private ReadOnly db As New DatabaseHelper()

        ' Obtiene todos los pacientes con información del usuario
        Public Function GetAll() As DataTable
            Dim sql = "SELECT P.PacienteId, P.UsuarioId, U.Email, U.Nombre, U.Apellidos,
                              P.Cedula, P.Telefono, P.Direccion
                       FROM Pacientes P
                       INNER JOIN Usuarios U ON U.Id = P.UsuarioId
                       ORDER BY P.PacienteId DESC"
            Return db.ExecuteQuery(sql)
        End Function

        ' Obtiene un paciente por su UsuarioId
        Public Function GetByUsuarioId(usuarioId As Integer) As DataTable
            Dim sql = "SELECT TOP 1 * FROM Pacientes WHERE UsuarioId=@UsuarioId"
            Dim p = New List(Of SqlParameter) From {db.CreateParameter("@UsuarioId", usuarioId)}
            Return db.ExecuteQuery(sql, p)
        End Function

        ' Inserta un nuevo paciente
        Public Function Insert(usuarioId As Integer, cedula As String,
                               telefono As String, direccion As String) As Boolean
            Dim sql = "INSERT INTO Pacientes(UsuarioId, Cedula, Telefono, Direccion)
                       VALUES(@UsuarioId, @Cedula, @Telefono, @Direccion)"
            Dim p = New List(Of SqlParameter) From {
                db.CreateParameter("@UsuarioId", usuarioId),
                db.CreateParameter("@Cedula", cedula),
                db.CreateParameter("@Telefono", telefono),
                db.CreateParameter("@Direccion", direccion)
            }
            Return db.ExecuteNonQuery(sql, p)
        End Function

        ' Actualiza un paciente existente
        Public Function Update(pacienteId As Integer, cedula As String,
                               telefono As String, direccion As String) As Boolean
            Dim sql = "UPDATE Pacientes
                       SET Cedula=@Cedula, Telefono=@Telefono, Direccion=@Direccion
                       WHERE PacienteId=@PacienteId"
            Dim p = New List(Of SqlParameter) From {
                db.CreateParameter("@Cedula", cedula),
                db.CreateParameter("@Telefono", telefono),
                db.CreateParameter("@Direccion", direccion),
                db.CreateParameter("@PacienteId", pacienteId)
            }
            Return db.ExecuteNonQuery(sql, p)
        End Function

        ' Elimina un paciente por su PacienteId
        Public Function Delete(pacienteId As Integer) As Boolean
            Dim sql = "DELETE FROM Pacientes WHERE PacienteId=@PacienteId"
            Dim p = New List(Of SqlParameter) From {db.CreateParameter("@PacienteId", pacienteId)}
            Return db.ExecuteNonQuery(sql, p)
        End Function
    End Class

End Namespace
