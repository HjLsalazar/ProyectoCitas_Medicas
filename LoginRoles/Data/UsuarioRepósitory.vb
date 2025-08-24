Imports System.Data
Imports System.Data.SqlClient
Imports LoginRoles.Helpers

Namespace Data
    ' Repositorio para la entidad Usuario
    Public Class UsuarioRepository
        Private ReadOnly db As New DatabaseHelper()

        ' Obtiene todos los usuarios
        Public Function GetAll() As DataTable
            Dim sql = "SELECT Id, Email, Nombre, Apellidos, RoleId FROM Usuarios ORDER BY Id DESC"
            Return db.ExecuteQuery(sql)
        End Function

        ' Obtiene un usuario por su Id
        Public Function Insert(email As String, pass As String, nombre As String, apellidos As String, roleId As Integer) As Boolean
            Dim sql = "INSERT INTO Usuarios(Email, Pass, Nombre, Apellidos, RoleId)
                       VALUES(@Email, @Pass, @Nombre, @Apellidos, @RoleId)"
            Dim p = New List(Of SqlParameter) From {
                db.CreateParameter("@Email", email),
                db.CreateParameter("@Pass", pass),
                db.CreateParameter("@Nombre", nombre),
                db.CreateParameter("@Apellidos", If(apellidos, String.Empty)),
                db.CreateParameter("@RoleId", roleId)
            }
            Return db.ExecuteNonQuery(sql, p)
        End Function

        ' Actualiza un usuario existente
        Public Function Update(id As Integer, email As String, nombre As String, apellidos As String, roleId As Integer) As Boolean
            Dim sql = "UPDATE Usuarios SET Email=@Email, Nombre=@Nombre, Apellidos=@Apellidos, RoleId=@RoleId
                       WHERE Id=@Id"
            Dim p = New List(Of SqlParameter) From {
                db.CreateParameter("@Email", email),
                db.CreateParameter("@Nombre", nombre),
                db.CreateParameter("@Apellidos", If(apellidos, String.Empty)),
                db.CreateParameter("@RoleId", roleId),
                db.CreateParameter("@Id", id)
            }
            Return db.ExecuteNonQuery(sql, p)
        End Function
        ' Elimina un usuario por su Id
        Public Function Delete(id As Integer) As Boolean
            Dim sql = "DELETE FROM Usuarios WHERE Id=@Id"
            Dim p = New List(Of SqlParameter) From {db.CreateParameter("@Id", id)}
            Return db.ExecuteNonQuery(sql, p)
        End Function
    End Class
End Namespace
