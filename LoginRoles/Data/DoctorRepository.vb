' Breve: CRUD de Doctor separado de la clase de dominio.
Imports System.Data
Imports System.Data.SqlClient
Imports LoginRoles.Helpers

Namespace Data

    Public Class DoctorRepository
        Private ReadOnly db As New DatabaseHelper()

        Public Function GetAll() As DataTable
            Dim sql = "SELECT * FROM Doctores ORDER BY DoctorId DESC"
            Return db.ExecuteQuery(sql)
        End Function

        Public Function Insert(nombre As String, especialidad As String,
                               correo As String, telefono As String) As Boolean
            Dim sql = "INSERT INTO Doctores(Nombre, Especialidad, Correo, Telefono)
                       VALUES(@Nombre, @Especialidad, @Correo, @Telefono)"
            Dim p = New List(Of SqlParameter) From {
                db.CreateParameter("@Nombre", nombre),
                db.CreateParameter("@Especialidad", especialidad),
                db.CreateParameter("@Correo", correo),
                db.CreateParameter("@Telefono", telefono)
            }
            Return db.ExecuteNonQuery(sql, p)
        End Function

        Public Function Update(doctorId As Integer, nombre As String, especialidad As String,
                               correo As String, telefono As String) As Boolean
            Dim sql = "UPDATE Doctores SET
                         Nombre=@Nombre, Especialidad=@Especialidad, Correo=@Correo, Telefono=@Telefono
                       WHERE DoctorId=@DoctorId"
            Dim p = New List(Of SqlParameter) From {
                db.CreateParameter("@Nombre", nombre),
                db.CreateParameter("@Especialidad", especialidad),
                db.CreateParameter("@Correo", correo),
                db.CreateParameter("@Telefono", telefono),
                db.CreateParameter("@DoctorId", doctorId)
            }
            Return db.ExecuteNonQuery(sql, p)
        End Function

        Public Function Delete(doctorId As Integer) As Boolean
            Dim sql = "DELETE FROM Doctores WHERE DoctorId=@DoctorId"
            Dim p = New List(Of SqlParameter) From {db.CreateParameter("@DoctorId", doctorId)}
            Return db.ExecuteNonQuery(sql, p)
        End Function
    End Class
End Namespace
