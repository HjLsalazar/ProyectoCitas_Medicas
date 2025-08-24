Imports LoginRoles.Models

Public Class SiteMaster
    Inherits MasterPage
    Protected autenticado As Boolean
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim usuario As Usuario = Session("User")
        autenticado = usuario IsNot Nothing
        Dim isAdmin = usuario?.RoleId.Equals(2)
        liAdmin.Visible = autenticado And isAdmin
    End Sub

    Protected Sub logout_Click(sender As Object, e As EventArgs)
        Session.Abandon()
        Session.Clear()
        Response.Redirect("Login.aspx")
    End Sub
End Class