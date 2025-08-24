Public Class Admin
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UsuarioId") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If

        If Session("RoleId") Is Nothing Then
            Response.Redirect("Login.aspx")
        ElseIf Session("RoleId") <> 2 Then
            Response.Redirect("Home.aspx")
        End If

    End Sub

End Class