Imports System.Data.SqlClient
Imports LoginRoles.Helpers
Imports LoginRoles.Models
Imports Microsoft.Ajax.Utilities

Public Class Registro
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Function RegistrarUsuario(usuario As Usuario) As Boolean
        Dim helper As New DatabaseHelper()
        Dim sql As String = "INSERT INTO Usuarios (Email, Pass, Nombre, Apellidos) VALUES (@Email, @Pass, @Nombre, @Apellidos)"
        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@Email", usuario.Email),
            New SqlParameter("@Pass", usuario.Pass),
            New SqlParameter("@Nombre", usuario.Nombre),
            New SqlParameter("@Apellidos", usuario.Apellidos)
        }
        Return helper.ExecuteNonQuery(sql, parameters)
    End Function

    Protected Sub btnRegistrar_Click(sender As Object, e As EventArgs)
        Dim js As String = "alert('test');"
        ' Usa Page como control y una key única para asegurar que siempre se ejecute
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), js, True)

        ' Obtener los valores de los campos
        Dim email As String = txtEmail.Text
        Dim nombre As String = txtNombre.Text
        Dim pass As String = txtPass.Text
        ' Validar que los campos no estén vacíos
        If email.IsNullOrWhiteSpace Or nombre.IsNullOrWhiteSpace Then
            lblError.Text = "Todos los campos son requeridos"
            Exit Sub
        End If
        ' Encriptar la contraseña
        Dim wrapper As New Simple3Des("claveclavecita")
        Dim password As String = wrapper.EncryptData(pass)
        ' Crear el objeto Usuario y registrar
        Dim Usuario As New Usuario() With {
            .Nombre = txtNombre.Text,
            .Email = txtEmail.Text,
            .Pass = password
        }

        If RegistrarUsuario(Usuario) Then
            ' Redirigir al login o a la página de inicio

            ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "ServerControlScript",
                "Swal.fire('Usuario Registrado').then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = 'Login.aspx';
                    }
                });",
                True)

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

    ' MasterPage: Site.master.vb

    Protected Sub Prueba_Click(sender As Object, e As EventArgs) Handles Prueba.Click
        Dim js As String = "alert('test');"
        ' Usa Page como control y una key única para asegurar que siempre se ejecute
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), js, True)
    End Sub



End Class