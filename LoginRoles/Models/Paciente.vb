
Namespace Models
    Public Class Paciente

        ' Clase Paciente que hereda de Usuario
        Inherits Usuario

        ' Propiedades específicas de Paciente
        Public Property PacienteId As Integer
        Public Property UsuarioId As Integer
        Public Property Cedula As String
        Public Property Telefono As String
        Public Property Direccion As String

        ' Constructores
        Public Sub New()
            MyBase.New()
        End Sub

        ' Constructor con parámetros
        Public Sub New(pacienteId As Integer, usuarioId As Integer, cedula As String,
                       telefono As String, direccion As String)
            MyBase.New()
            Me.PacienteId = pacienteId
            Me.UsuarioId = usuarioId
            Me.Cedula = cedula
            Me.Telefono = telefono
            Me.Direccion = direccion
        End Sub
    End Class

End Namespace
