Imports System.Data
Imports System.Data.SqlClient
Imports LoginRoles.Helpers

Namespace Data

    Public Class CitaRepository
        ' Repositorio para gestionar citas médicas
        Private ReadOnly db As New DatabaseHelper()


        Public Function GetList(roleId As Integer, usuarioId As Integer) As DataTable
            Dim baseSelect As String =
            "SELECT 
                c.CitaId,
                c.FechaHora,
                c.DuracionMinutos AS Min,
                c.Motivo,
                c.Estado,
                d.Nombre        AS Doctor,
                d.Especialidad,
                (u.Nombre + ' ' + u.Apellidos) AS Paciente
            FROM dbo.Citas c
            INNER JOIN dbo.Doctores  d ON d.DoctorId  = c.DoctorId
            INNER JOIN dbo.Pacientes p ON p.PacienteId = c.PacienteId
            INNER JOIN dbo.Usuarios  u ON u.ID        = p.UsuarioId"

            Dim sql As String
            Dim pars As List(Of SqlParameter) = Nothing

            Select Case roleId
                Case 2, 3
                    sql = baseSelect & " ORDER BY c.FechaHora DESC"

                Case 1    ' Paciente: sólo sus citas (por UsuarioId del paciente)
                    sql = baseSelect & " WHERE p.UsuarioId = @uid ORDER BY c.FechaHora DESC"
                    pars = New List(Of SqlParameter) From {db.CreateParameter("@uid", usuarioId)}

                Case Else
                    sql = baseSelect & " WHERE 1=0"
            End Select

            Return db.ExecuteQuery(sql, pars)
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

        ' Marcar una cita como Cancelada (para el paciente dueño de la cita)
        Public Function Cancelar(citaId As Integer, pacienteId As Integer) As Boolean
            Return db.ExecuteNonQuery(
                "UPDATE Citas SET Estado='Cancelada' WHERE CitaId=@Id AND PacienteId=@Pac",
                New List(Of SqlParameter) From {
                    db.CreateParameter("@Id", citaId),
                    db.CreateParameter("@Pac", pacienteId)
                }
            )
        End Function

    End Class
End Namespace
