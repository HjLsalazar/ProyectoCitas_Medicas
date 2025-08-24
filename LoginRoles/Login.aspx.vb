Imports System.Data.SqlClient
Imports LoginRoles.Helpers
Imports LoginRoles.Models
Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Llamar JS para rellenar si hay un email guardado
            Dim script As String = $"cargarEmail('{txtEmail.ClientID}','{ckbRecordar.ClientID}');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "cargarEmail", script, True)

            ' Ejecutar guardarEmail en el cliente cuando el usuario pulse Acceder
            btnLogin.OnClientClick = $"guardarEmail('{txtEmail.ClientID}','{ckbRecordar.ClientID}');"
        End If
    End Sub


    Protected Function VerificarUsuario(usuario As Usuario) As Boolean
        Try
            Dim helper As New DatabaseHelper()
            Dim email As String = txtEmail.Text
            Dim wrapper As New Simple3Des("claveclavecita")
            Dim pass As String = wrapper.EncryptData(usuario.Pass)

            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@Email", usuario.Email),
                New SqlParameter("@Password", pass)
            }
            ' Ejecutar la consulta para verificar el usuario
            Dim query As String = "SELECT [ID],[Nombre],[Apellidos],[Email],[RoleId]
            FROM Usuarios WHERE Email = @Email AND Pass = @Password;"
            ' Utilizar ExecuteQuery para obtener el DataTable
            Dim dataTable As DataTable = helper.ExecuteQuery(query, parametros)

            ' Verificar si se encontró el usuario
            If dataTable.Rows.Count > 0 Then
                ' Usuario encontrado, puedes redirigir o realizar otra acción
                usuario = usuario.dtToUsuario(dataTable)
                Session.Add("UsuarioId", usuario.Id.ToString())
                Session.Add("UsuarioNombre", usuario.Nombre.ToString())
                Session.Add("UsuarioApellido", usuario.Apellidos.ToString())
                Session.Add("UsuarioEmail", usuario.Email.ToString())
                Session.Add("RoleId", usuario.RoleId)
                Session.Add("User", usuario)
                Return True
            Else
                ' Usuario no encontrado, manejar el error
                Return False

            End If

        Catch ex As Exception
            Return False
        End Try
    End Function

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        ' Obtener los valores de los campos de entrada
        Dim usuario As New Usuario() With {
            .Email = txtEmail.Text,
            .Pass = txtPass.Text
        }

        ' Validar el usuario
        If VerificarUsuario(usuario) Then

            Dim RoleId = Session("RoleId")
            If (RoleId = 1) Then
                Response.Redirect("Home.aspx")
            End If
            If (RoleId = 2) Then
                Response.Redirect("Admin.aspx")
            End If

        Else

            lblError.Text = "Correo electrónico o contraseña inválidos."
            lblError.Visible = True
        End If
    End Sub

    Protected Sub ckbRecordar_CheckedChanged(sender As Object, e As EventArgs)
    End Sub
End Class
