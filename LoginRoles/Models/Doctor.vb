
Namespace Models
    Public Class Doctor
        Inherits Usuario

        Public Property DoctorId As Integer
        Public Property Nombre As String
        Public Property Especialidad As String
        Public Property Correo As String
        Public Property Telefono As String

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(doctorId As Integer, nombre As String, especialidad As String,
                       correo As String, telefono As String)
            MyBase.New()
            Me.DoctorId = doctorId
            Me.Nombre = nombre
            Me.Especialidad = especialidad
            Me.Correo = correo
            Me.Telefono = telefono
        End Sub
    End Class
End Namespace
