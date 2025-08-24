
Namespace Models
    ' Modelo de datos para una cita médica
    Public Class Cita
        Public Property CitaId As Integer
        Public Property PacienteId As Integer
        Public Property DoctorId As Integer
        Public Property FechaHora As DateTime
        Public Property DuracionMinutos As Integer
        Public Property Motivo As String
        Public Property Estado As String

        ' Constructor vacío
        Public Sub New()
            MyBase.New()
        End Sub

        ' Constructor con parámetros
        Public Sub New(citaId As Integer, pacienteId As Integer, doctorId As Integer,
                       fechaHora As DateTime, duracion As Integer, motivo As String, estado As String)
            Me.CitaId = citaId
            Me.PacienteId = pacienteId
            Me.DoctorId = doctorId
            Me.FechaHora = fechaHora
            Me.DuracionMinutos = duracion
            Me.Motivo = motivo
            Me.Estado = estado
        End Sub
    End Class
End Namespace
