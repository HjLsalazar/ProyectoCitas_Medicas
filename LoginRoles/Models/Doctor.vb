
Namespace Models
    Public Class Doctor
        ' Clase que representa un Doctor
        Inherits Usuario

        ' Propiedades del Doctor
        Public Property DoctorId As Integer
        Public Property Nombre As String
        Public Property Especialidad As String
        Public Property Correo As String
        Public Property Telefono As String

        ' Constructores
        Public Sub New()
            MyBase.New()
        End Sub

        ' Constructor con parámetros
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
