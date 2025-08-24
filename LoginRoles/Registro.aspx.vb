Imports System.Data.SqlClient
Imports LoginRoles.Helpers
Imports LoginRoles.Models
Imports Microsoft.Ajax.Utilities

Public Class Registro
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Si hay sesión y es Admin (RoleId=2), mostramos el selector de rol
            pnlRol.Visible = (Session("UsuarioId") IsNot Nothing AndAlso CInt(Session("RoleId")) = 2)
        End If
    End Sub

    ' Inserta usuario con RoleId (se mantiene tu misma lógica y helper)
    Protected Function RegistrarUsuario(usuario As Usuario) As Boolean
        Dim helper As New DatabaseHelper()

        ' IMPORTANTE: ahora incluimos RoleId en el INSERT
        Dim sql As String = "INSERT INTO Usuarios (Email, Pass, Nombre, Apellidos, RoleId)
                             VALUES (@Email, @Pass, @Nombre, @Apellidos, @RoleId)"

        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@Email", usuario.Email),
            New SqlParameter("@Pass", usuario.Pass),
            New SqlParameter("@Nombre", usuario.Nombre),
            New SqlParameter("@Apellidos", If(usuario.Apellidos, String.Empty)),
            New SqlParameter("@RoleId", usuario.RoleId)  ' << NUEVO
        }

        Return helper.ExecuteNonQuery(sql, parameters)
    End Function

    Protected Sub btnRegistrar_Click(sender As Object, e As EventArgs) Handles btnRegistrar.Click
        ' Aviso (tu prueba)
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

        ' Encriptar contraseña (misma clave)
        Dim wrapper As New Simple3Des("claveclavecita")
        Dim password As String = wrapper.EncryptData(pass)

        ' RoleId:
        ' - Si el panel es visible (Admin está registrando), tomamos el valor del DropDownList
        ' - Si NO es visible (registro público), asignamos 1 = Paciente
        Dim roleId As Integer = If(pnlRol.Visible, CInt(ddlRol.SelectedValue), 1)

        ' Crear objeto Usuario (misma lógica + RoleId)
        Dim Usuario As New Usuario() With {
            .Nombre = nombre,
            .Apellidos = "",      ' si no usas apellidos, dejamos vacío
            .Email = email,
            .Pass = password,
            .RoleId = roleId      ' << NUEVO
        }

        If RegistrarUsuario(Usuario) Then
            ' Tu feedback + redirección
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

    Protected Sub Prueba_Click(sender As Object, e As EventArgs) Handles Prueba.Click
        Dim js As String = "alert('test');"
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), js, True)
    End Sub
End Class
