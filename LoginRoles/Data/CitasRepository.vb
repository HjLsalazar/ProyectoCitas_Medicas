
Imports System.Data
Imports System.Data.SqlClient
Imports LoginRoles.Helpers

Namespace Data

    Public Class CitaRepository
        ' Repositorio para gestionar citas médicas
        Private ReadOnly db As New DatabaseHelper()

        ' Obtener lista de citas según rol:
        Public Function GetList(roleId As Integer, usuarioId As Integer) As DataTable
            If roleId = 2 Then ' Admin ve todas
                Dim sql = "SELECT C.CitaId, C.FechaHora, C.DuracionMinutos, C.Motivo, C.Estado,
                                  P.PacienteId, P.Cedula,
                                  U.Nombre + ' ' + U.Apellidos AS Paciente,
                                  D.DoctorId, D.Nombre AS Doctor, D.Especialidad
                           FROM Citas C
                           INNER JOIN Pacientes P ON P.PacienteId=C.PacienteId
                           INNER JOIN Usuarios U ON U.Id=P.UsuarioId
                           INNER JOIN Doctores D ON D.DoctorId=C.DoctorId
                           ORDER BY C.FechaHora DESC"
                Return db.ExecuteQuery(sql)
            Else ' Doctor ve solo sus citas
                Dim sql = "SELECT C.CitaId, C.FechaHora, C.DuracionMinutos, C.Motivo, C.Estado,
                                  D.DoctorId, D.Nombre AS Doctor, D.Especialidad
                           FROM Citas C
                           INNER JOIN Pacientes P ON P.PacienteId=C.PacienteId
                           INNER JOIN Usuarios U ON U.Id=P.UsuarioId
                           INNER JOIN Doctores D ON D.DoctorId=C.DoctorId
                           WHERE U.Id=@UsuarioId
                           ORDER BY C.FechaHora DESC"
                Dim p = New List(Of SqlParameter) From {db.CreateParameter("@UsuarioId", usuarioId)}
                Return db.ExecuteQuery(sql, p)
            End If
        End Function

        ' Verificar si el doctor está disponible en la fecha y hora dada
        Public Function EstaDisponible(doctorId As Integer, fechaHora As DateTime) As Boolean
            Dim sql = "SELECT COUNT(1) FROM Citas WHERE DoctorId=@DoctorId AND FechaHora=@FechaHora"
            Dim p = New List(Of SqlParameter) From {
                db.CreateParameter("@DoctorId", doctorId),
                db.CreateParameter("@FechaHora", fechaHora)
            }
            Dim n = db.ExecuteScalar(Of Integer)(sql, p)
            Return n = 0
        End Function
        ' Insertar nueva cita
        Public Function Insert(pacienteId As Integer, doctorId As Integer, fechaHora As DateTime,
                               duracionMin As Integer, motivo As String, estado As String) As Boolean
            If Not EstaDisponible(doctorId, fechaHora) Then
                Throw New Exception("El doctor ya tiene una cita a esa hora.")
            End If
            Dim sql = "INSERT INTO Citas(PacienteId, DoctorId, FechaHora, DuracionMinutos, Motivo, Estado)
                       VALUES(@PacienteId, @DoctorId, @FechaHora, @DuracionMinutos, @Motivo, @Estado)"
            Dim p = New List(Of SqlParameter) From {
                db.CreateParameter("@PacienteId", pacienteId),
                db.CreateParameter("@DoctorId", doctorId),
                db.CreateParameter("@FechaHora", fechaHora),
                db.CreateParameter("@DuracionMinutos", duracionMin),
                db.CreateParameter("@Motivo", motivo),
                db.CreateParameter("@Estado", If(String.IsNullOrWhiteSpace(estado), "Pendiente", estado))
            }
            Return db.ExecuteNonQuery(sql, p)
        End Function

        ' Actualizar cita existente
        Public Function Update(citaId As Integer, pacienteId As Integer, doctorId As Integer,
                               fechaHora As DateTime, duracionMin As Integer,
                               motivo As String, estado As String) As Boolean
            Dim sql = "UPDATE Citas SET
                         PacienteId=@PacienteId, DoctorId=@DoctorId, FechaHora=@FechaHora,
                         DuracionMinutos=@DuracionMinutos, Motivo=@Motivo, Estado=@Estado
                       WHERE CitaId=@CitaId"
            Dim p = New List(Of SqlParameter) From {
                db.CreateParameter("@PacienteId", pacienteId),
                db.CreateParameter("@DoctorId", doctorId),
                db.CreateParameter("@FechaHora", fechaHora),
                db.CreateParameter("@DuracionMinutos", duracionMin),
                db.CreateParameter("@Motivo", motivo),
                db.CreateParameter("@Estado", estado),
                db.CreateParameter("@CitaId", citaId)
            }
            Return db.ExecuteNonQuery(sql, p)
        End Function

        ' Eliminar cita por ID
        Public Function Delete(citaId As Integer) As Boolean
            Dim sql = "DELETE FROM Citas WHERE CitaId=@CitaId"
            Dim p = New List(Of SqlParameter) From {db.CreateParameter("@CitaId", citaId)}
            Return db.ExecuteNonQuery(sql, p)
        End Function


        ' Obtener DoctorId asociado a una cita
        Public Function GetDoctorIdByCita(citaId As Integer) As Integer
            Dim sql = "SELECT DoctorId FROM Citas WHERE CitaId=@Id"
            Dim p = New List(Of SqlParameter) From {db.CreateParameter("@Id", citaId)}
            Dim dt = db.ExecuteQuery(sql, p)
            If dt.Rows.Count = 0 Then Return 0
            Return CInt(dt.Rows(0)("DoctorId"))
        End Function
    End Class
End Namespace
