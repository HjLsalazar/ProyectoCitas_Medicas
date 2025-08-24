
Partial Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session.Abandon()              ' Termina la sesión
        Response.Redirect("~/Login.aspx") ' Regresa al login
    End Sub
End Class
