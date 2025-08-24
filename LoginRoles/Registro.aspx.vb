Imports System.Data.SqlClient
Imports LoginRoles.Helpers
Imports LoginRoles.Models
Imports Microsoft.Ajax.Utilities

Public Class Registro
    Inherits System.Web.UI.Page
    ' Mostrar panel de selección de rol solo si el usuario es admin (RoleId = 2)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            pnlRol.Visible = (Session("UsuarioId") IsNot Nothing AndAlso CInt(Session("RoleId")) = 2)
        End If

    End Sub

    ' Inserta usuario con RoleId 
    Protected Function RegistrarUsuario(usuario As Usuario) As Boolean
        Dim helper As New DatabaseHelper()

        ' SQL con RoleId
        Dim sql As String = "INSERT INTO Usuarios (Email, Pass, Nombre, Apellidos, RoleId)
                             VALUES (@Email, @Pass, @Nombre, @Apellidos, @RoleId)"

        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@Email", usuario.Email),
            New SqlParameter("@Pass", usuario.Pass),
            New SqlParameter("@Nombre", usuario.Nombre),
            New SqlParameter("@Apellidos", If(usuario.Apellidos, String.Empty)),
            New SqlParameter("@RoleId", usuario.RoleId)
        }

        Return helper.ExecuteNonQuery(sql, parameters)
    End Function

    ' Evento del botón Registrar
    Protected Sub btnRegistrar_Click(sender As Object, e As EventArgs) Handles btnRegistrar.Click

        ' Ejemplo de uso de ScriptManager para alertas
        Dim js As String = "alert('test');"
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), js, True)

        ' Obtener valores
        Dim email As String = txtEmail.Text
        Dim nombre As String = txtNombre.Text
        Dim pass As String = txtPass.Text

        ' Validación mínima (misma lógica)
        If email.IsNullOrWhiteSpace Or nombre.IsNullOrWhiteSpace Then
            lblError.Text = "Todos los campos son requeridos"
            lblError.Visible = True
            Exit Sub
        End If

        ' Encriptar contraseña
        Dim wrapper As New Simple3Des("claveclavecita")
        Dim password As String = wrapper.EncryptData(pass)


        Dim roleId As Integer = If(pnlRol.Visible, CInt(ddlRol.SelectedValue), 1)

        ' Crear objeto Usuario 
        Dim Usuario As New Usuario() With {
            .Nombre = nombre,
            .Apellidos = "",
            .Email = email,
            .Pass = password,
            .RoleId = roleId
        }

        ' Intentar registrar usuario
        If RegistrarUsuario(Usuario) Then
            ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "ServerControlScript",
                "Swal.fire('Usuario Registrado').then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = 'Login.aspx';
                    }
                });",
                True)
            lblError.Visible = False
            ' Redirigir a Login.aspx
        Else
            ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "ServerControlScript",
                "Swal.fire('Error al registrar el usuario. Inténtalo de nuevo.');",
                True)
            lblError.Text = "Error al registrar el usuario. Inténtalo de nuevo."
            lblError.Visible = True
        End If
    End Sub

End Class
