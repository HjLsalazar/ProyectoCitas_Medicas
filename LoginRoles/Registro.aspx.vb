Imports System.Data.SqlClient
Imports LoginRoles.Helpers
Imports LoginRoles.Models
Imports Microsoft.Ajax.Utilities
Imports LoginRoles.Data

Public Class Registro
    Inherits System.Web.UI.Page

    ' Carga inicial de la página
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Redirigir si ya está logueado
        If Session("UsuarioId") IsNot Nothing AndAlso CInt(Session("RoleId")) = 3 Then
            Response.Redirect("Home.aspx", False)
            Context.ApplicationInstance.CompleteRequest()
            Return
        End If

        If Not IsPostBack Then
            ' Mostrar u ocultar panel de selección de rol
            pnlRol.Visible = (Session("UsuarioId") IsNot Nothing AndAlso CInt(Session("RoleId")) = 2)
        End If
    End Sub

    ' Inferir RoleId a partir del email (registro público)
    Private Function InferRoleFromEmail(email As String) As Integer
        If String.IsNullOrWhiteSpace(email) Then Return 1
        Dim e = email.Trim().ToLowerInvariant()
        If e.Contains("@admin") Then
            Return 2
        ElseIf e.Contains("@clinica") Then
            Return 3
        Else
            Return 1
        End If
    End Function

    ' Inserta usuario con RoleId
    Protected Function RegistrarUsuario(usuario As Usuario) As Boolean
        Dim helper As New DatabaseHelper()

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
        Dim email As String = txtEmail.Text.Trim()
        Dim nombre As String = txtNombre.Text.Trim()
        Dim pass As String = txtPass.Text

        If String.IsNullOrWhiteSpace(email) OrElse String.IsNullOrWhiteSpace(nombre) Then
            lblError.Text = "Todos los campos son requeridos"
            lblError.Visible = True
            Exit Sub
        End If

        Dim wrapper As New Simple3Des("claveclavecita")
        Dim password As String = wrapper.EncryptData(pass)

        ' Determinar rol
        Dim rolNombre As String

        If pnlRol.Visible Then
            ' Registro interno: por selección del admin
            Select Case ddlRol.SelectedValue
                Case "2" : rolNombre = "Administrador"
                Case "3" : rolNombre = "Doctor"
                Case Else : rolNombre = "Paciente"
            End Select
        Else
            ' Registro público: inferir por email
            Dim eLower As String = email.ToLowerInvariant()
            If eLower.Contains("@admin") Then
                rolNombre = "Administrador"
            ElseIf eLower.Contains("@clinica") Then
                rolNombre = "Doctor"
            Else
                rolNombre = "Paciente"
            End If
        End If

        ' Obtener RoleId real desde la tabla Roles
        Dim helper As New DatabaseHelper()
        Dim realRoleIdObj As Object = helper.ExecuteScalar(Of Integer)(
            "SELECT RoleId FROM Roles WHERE Nombre = @n",
            New List(Of SqlParameter) From {helper.CreateParameter("@n", rolNombre)}
        )

        ' Validar que el rol exista
        If realRoleIdObj Is Nothing OrElse realRoleIdObj Is DBNull.Value Then
            lblError.Text = "El rol '" & rolNombre & "' no existe en la tabla Roles. Verifica el seed de Roles."
            lblError.Visible = True
            Exit Sub
        End If

        ' Convertir a Integer
        Dim realRoleId As Integer = CInt(realRoleIdObj)

        '  Crear y registrar usuario 
        Dim Usuario As New Usuario() With {
            .Nombre = nombre,
            .Apellidos = "",
            .Email = email,
            .Pass = password,
            .RoleId = realRoleId
        }

        If RegistrarUsuario(Usuario) Then
            Response.Redirect("Login.aspx", False)
            Context.ApplicationInstance.CompleteRequest()
            Return
        Else
            ' Error al insertar
            ScriptManager.RegisterStartupScript(Me, Me.GetType(),
                "ServerControlScript",
                "Swal.fire('Error al registrar el usuario. Inténtalo de nuevo.');", True)
            lblError.Text = "Error al registrar el usuario. Inténtalo de nuevo."
            lblError.Visible = True
        End If
    End Sub

End Class
