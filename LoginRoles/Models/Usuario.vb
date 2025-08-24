Namespace Models
    Public Class Usuario

        Public Property Id As Integer
        Public Property Nombre As String
        Public Property Apellidos As String
        Public Property Email As String
        Public Property Pass As String
        Public Property RoleId As Integer

        ' Constructor por defecto
        Public Sub New()
            Id = 0
            Nombre = String.Empty
            Apellidos = String.Empty
            Email = String.Empty
            Pass = String.Empty
            RoleId = 1
        End Sub

        ' Método para validar el usuario (ejemplo simple)
        Public Function Validar() As Boolean
            Return Not String.IsNullOrEmpty(Email) AndAlso Not String.IsNullOrEmpty(Pass)
        End Function

        ' Método para convertir un DataTable en un objeto Usuario
        Public Function dtToUsuario(dataTable As DataTable) As Usuario
            If dataTable IsNot Nothing AndAlso dataTable.Rows.Count > 0 Then
                Dim row As DataRow = dataTable.Rows(0)
                Dim roleId As Integer = If(IsDBNull(row("RoleId")), 1, Convert.ToInt32(row("RoleId")))
                Return New Usuario() With {
                    .Id = Convert.ToInt32(row("Id")),
                    .Nombre = Convert.ToString(row("Nombre")),
                    .Apellidos = Convert.ToString(row("Apellidos")),
                    .Email = Convert.ToString(row("Email")),
                    .RoleId = roleId
                }
            End If
            Return Nothing
        End Function

    End Class
End Namespace
