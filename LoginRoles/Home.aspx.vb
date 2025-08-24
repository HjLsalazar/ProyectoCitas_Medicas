Public Class Home
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UsuarioId") Is Nothing Then
            Response.Redirect("Login.aspx")
        Else
            lblEmail.Text = Session("UsuarioEmail")
            lblNombre.Text = Session("UsuarioNombre") + " " + Session("UsuarioApellido")
        End If
    End Sub

End Class