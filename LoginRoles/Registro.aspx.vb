Imports System.Data.SqlClient
Imports LoginRoles.Helpers
Imports LoginRoles.Models
Imports Microsoft.Ajax.Utilities
Imports LoginRoles.Data   ' <-- para DoctorRepository

Public Class Registro
    Inherits System.Web.UI.Page

    ' Mostrar panel de selección de rol solo si el usuario en sesión es admin (RoleId = 2)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            pnlRol.Visible = (Session("UsuarioId") IsNot Nothing AndAlso CInt(Session("RoleId")) = 2)
        End If
    End Sub

    ' === INFERENCIA DE ROL POR EMAIL ===
    Private Function InferRoleFromEmail(email As String) As Integer
        If String.IsNullOrWhiteSpace(email) Then Return 1 ' Paciente por defecto
        Dim e = email.Trim().ToLowerInvariant()

        If e.Contains("@admin") Then
            Return 2  ' Administrador
        ElseIf e.Contains("@clinica") Then
            Return 3  ' Doctor
        Else
            Return 1  ' Paciente
        End If
    End Function

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
        ' (Dejé tu prueba de JS comentada para no interrumpir el flujo)
        'Dim js As String = "alert('test');"
        'ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), js, True)

        ' Obtener valores
        Dim email As String = txtEmail.Text
        Dim nombre As String = txtNombre.Text
        Dim pass As String = txtPass.Text

        ' Validación mínima
        If email.IsNullOrWhiteSpace Or nombre.IsNullOrWhiteSpace Then
            lblError.Text = "Todos los campos son requeridos"
            lblError.Visible = True
            Exit Sub
        End If

        ' Encriptar contraseña
        Dim wrapper As New Simple3Des("claveclavecita")
        Dim password As String = wrapper.EncryptData(pass)

        ' Rol: si admin ve el panel, respeta su selección; si no, inferir por email
        Dim sugerido As Integer = InferRoleFromEmail(email)
        Dim roleId As Integer = If(pnlRol.Visible, CInt(ddlRol.SelectedValue), sugerido)

        ' Crear objeto Usuario
        Dim Usuario As New Usuario() With {
            .Nombre = nombre,
            .Apellidos = "",
            .Email = email,
            .Pass = password,
            .RoleId = roleId
        }

        Try
            ' Registrar usuario
            If RegistrarUsuario(Usuario) Then
                ' Si queda como Doctor (3), crear también una fila base en Doctores
                If roleId = 3 Then
                    Dim dRepo As New DoctorRepository()
                    dRepo.Insert(
                        nombre.Trim(),   ' Nombre del doctor
                        "",              ' Especialidad (luego se edita)
                        email.Trim(),    ' Correo
                        ""               ' Teléfono (luego se edita)
                    )
                End If

                ' Redirección inmediata al Login
                Response.Redirect("Login.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
                Return
            Else
                ScriptManager.RegisterStartupScript(
                    Me, Me.GetType(),
                    "ServerControlScript",
                    "Swal.fire('Error al registrar el usuario. Inténtalo de nuevo.');",
                    True)
                lblError.Text = "Error al registrar el usuario. Inténtalo de nuevo."
                lblError.Visible = True
            End If

        Catch ex As Exception
            lblError.Text = ex.Message
            lblError.Visible = True
        End Try
    End Sub

End Class
