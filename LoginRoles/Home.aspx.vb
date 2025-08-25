Public Class Home
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Si no hay sesión, enviar a Login
        If Session("UsuarioId") Is Nothing Then
            Response.Redirect("Login.aspx")
            Return
        End If

        If Not IsPostBack Then
            Dim email As String = CStr(Session("UsuarioEmail"))
            Dim roleId As Integer = CInt(Session("RoleId"))

            ' Mapear RoleId -> nombre de rol
            Dim rol As String = If(roleId = 1, "Paciente",
                              If(roleId = 2, "Administrador", "Doctor"))

            lblRol.Text = rol
            lblEmail.Text = email
        End If
    End Sub
End Class
