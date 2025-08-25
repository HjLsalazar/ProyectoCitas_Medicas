Public Class Home
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Validar si el usuario ha iniciado sesión
        If Session("UsuarioId") Is Nothing Then
            Response.Redirect("Login.aspx")

            ' Si no hay sesión, redirigir a la página de inicio de sesión
        Else
            lblEmail.Text = Session("UsuarioEmail")
            lblNombre.Text = Session("UsuarioNombre") + " " + Session("UsuarioApellido")
        End If
    End Sub

End Class