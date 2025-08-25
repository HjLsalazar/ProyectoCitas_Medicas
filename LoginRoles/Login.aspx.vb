Imports System.Data.SqlClient
Imports LoginRoles.Helpers
Imports LoginRoles.Models

Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Cargar email recordado al cargar la página
            Dim script As String = $"cargarEmail('{txtEmail.ClientID}','{ckbRecordar.ClientID}');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "cargarEmail", script, True)

            ' Asignar el evento OnClientClick al botón
            btnLogin.OnClientClick = $"guardarEmail('{txtEmail.ClientID}','{ckbRecordar.ClientID}');"
        End If
    End Sub

    ' Verifica usuario y guarda sesión
    Protected Function VerificarUsuario(usuario As Usuario) As Boolean
        Try
            Dim helper As New DatabaseHelper()
            Dim wrapper As New Simple3Des("claveclavecita")
            Dim pass As String = wrapper.EncryptData(usuario.Pass)

            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@Email", usuario.Email),
                New SqlParameter("@Password", pass)
            }

            Dim query As String =
                "SELECT [ID],[Nombre],[Apellidos],[Email],[RoleId]
                 FROM Usuarios
                 WHERE Email = @Email AND Pass = @Password;"

            Dim dataTable As DataTable = helper.ExecuteQuery(query, parametros)

            If dataTable.Rows.Count > 0 Then
                ' Usuario encontrado, guardar en sesión
                usuario = usuario.dtToUsuario(dataTable)

                Session("UsuarioId") = usuario.Id
                Session("UsuarioNombre") = usuario.Nombre
                Session("UsuarioApellido") = usuario.Apellidos
                Session("UsuarioEmail") = usuario.Email
                Session("Email") = usuario.Email
                Session("RoleId") = usuario.RoleId
                Session("User") = usuario

                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function

    ' Evento del botón Login
    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        Dim usuario As New Usuario() With {
            .Email = txtEmail.Text.Trim(),
            .Pass = txtPass.Text
        }

        If VerificarUsuario(usuario) Then
            ' Redirigir según rol
            Dim roleId As Integer = 0
            If Session("RoleId") IsNot Nothing Then
                roleId = CInt(Session("RoleId"))
            End If

            If roleId = 2 Then
                Response.Redirect("Admin.aspx", False)
            Else
                Response.Redirect("Home.aspx", False)
            End If

            ' Evitar ThreadAbortException
            Context.ApplicationInstance.CompleteRequest()
        Else
            lblError.Text = "Correo electrónico o contraseña inválidos."
            lblError.Visible = True
        End If
    End Sub



End Class
