Imports System.Data.SqlClient
Imports LoginRoles.Helpers
Imports LoginRoles.Models
Imports Microsoft.Ajax.Utilities
Imports LoginRoles.Data

Public Class Registro
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Si ya hay sesión y es doctor, no puede registrar
        If Session("UsuarioId") IsNot Nothing AndAlso CInt(Session("RoleId")) = 3 Then
            Response.Redirect("Home.aspx", False)
            Context.ApplicationInstance.CompleteRequest()
            Return
        End If

        If Not IsPostBack Then
            ' Solo el admin ve el selector de rol
            pnlRol.Visible = (Session("UsuarioId") IsNot Nothing AndAlso CInt(Session("RoleId")) = 2)
        End If
    End Sub

    'delvuelve Role.Nombre obtiene RoleId (0 si no existe)
    Private Function GetRoleIdByNombre(rolNombre As String) As Integer
        Dim helper As New DatabaseHelper()

        ' Leer como Object para poder validar Nothing/DBNull
        Dim obj As Object = helper.ExecuteScalar(Of Object)(
        "SELECT RoleId FROM Roles WHERE Nombre = @n",
        New List(Of SqlParameter) From {helper.CreateParameter("@n", rolNombre)}
    )

        If obj Is Nothing OrElse Convert.IsDBNull(obj) Then Return 0
        Return Convert.ToInt32(obj)
    End Function

    ' INSERTA USUARIO Y DEVUELVE SU ID (0 si error)
    Private Function InsertUsuarioYDevolverId(u As Usuario) As Integer
        Dim helper As New DatabaseHelper()
        Dim sql As String =
"INSERT INTO Usuarios (Email, Pass, Nombre, Apellidos, RoleId)
 OUTPUT INSERTED.ID
 VALUES (@Email, @Pass, @Nombre, @Apellidos, @RoleId);"

        Dim pars As New List(Of SqlParameter) From {
        New SqlParameter("@Email", u.Email),
        New SqlParameter("@Pass", u.Pass),
        New SqlParameter("@Nombre", u.Nombre),
        New SqlParameter("@Apellidos", If(u.Apellidos, String.Empty)),
        New SqlParameter("@RoleId", u.RoleId)
    }

        ' lee como Object para poder validar Nothing/DBNull
        Dim obj As Object = helper.ExecuteScalar(Of Object)(sql, pars)
        If obj Is Nothing OrElse Convert.IsDBNull(obj) Then Return 0
        Return Convert.ToInt32(obj)
    End Function


    ' CREA FILA EN DOCTORES (si no existe)
    Private Sub InsertDoctorSiCorresponde(nombre As String, email As String)
        Try
            Dim helper As New DatabaseHelper()
            ' Usamos email como único identificador
            Dim sql As String =
"IF NOT EXISTS (SELECT 1 FROM Doctores WHERE Correo = @Correo)
   INSERT INTO Doctores (Nombre, Especialidad, Correo, Telefono)
   VALUES (@Nombre, N'General', @Correo, N'');"

            Dim pars As New List(Of SqlParameter) From {
                New SqlParameter("@Nombre", nombre),
                New SqlParameter("@Correo", email)
            }
            helper.ExecuteNonQuery(sql, pars)
        Catch
            ' Si falla por duplicado, lo ignoramos
        End Try
    End Sub


    ' CREA FILA EN PACIENTES (si no existe)
    Private Sub InsertPacienteSiCorresponde(usuarioId As Integer)
        Try
            Dim helper As New DatabaseHelper()
            Dim cedula As String = "AUTO-" & usuarioId.ToString()

            Dim sql As String =
"IF NOT EXISTS (SELECT 1 FROM Pacientes WHERE UsuarioId = @U)
   INSERT INTO Pacientes (UsuarioId, Cedula, Telefono, Direccion)
   VALUES (@U, @Ced, N'', N'');"

            Dim pars As New List(Of SqlParameter) From {
                New SqlParameter("@U", usuarioId),
                New SqlParameter("@Ced", cedula)
            }
            helper.ExecuteNonQuery(sql, pars)
        Catch
            ' Si falla por duplicado, lo ignoramos
        End Try
    End Sub

    ' Evento click del botón Registrar
    Protected Sub btnRegistrar_Click(sender As Object, e As EventArgs) Handles btnRegistrar.Click
        Dim email As String = txtEmail.Text.Trim()
        Dim nombre As String = txtNombre.Text.Trim()
        Dim pass As String = txtPass.Text

        If String.IsNullOrWhiteSpace(email) OrElse String.IsNullOrWhiteSpace(nombre) Then
            lblError.Text = "Todos los campos son requeridos"
            lblError.Visible = True
            Exit Sub
        End If

        ' Encriptar contraseña
        Dim wrapper As New Simple3Des("claveclavecita")
        Dim password As String = wrapper.EncryptData(pass)

        ' Determinar nombre del rol
        Dim rolNombre As String
        If pnlRol.Visible Then
            Select Case ddlRol.SelectedValue
                Case "2" : rolNombre = "Administrador"
                Case "3" : rolNombre = "Doctor"
                Case Else : rolNombre = "Paciente"
            End Select
        Else
            Dim eLower = email.ToLowerInvariant()
            If eLower.Contains("@admin") Then
                rolNombre = "Administrador"
            ElseIf eLower.Contains("@clinica") Then
                rolNombre = "Doctor"
            Else
                rolNombre = "Paciente"
            End If
        End If

        ' Obtener RoleId
        Dim roleId As Integer = GetRoleIdByNombre(rolNombre)
        If roleId = 0 Then
            lblError.Text = $"El rol '{rolNombre}' no existe en la tabla Roles."
            lblError.Visible = True
            Exit Sub
        End If

        ' Insertar en Usuarios y obtener ID
        Dim u As New Usuario With {
            .Nombre = nombre,
            .Apellidos = "",
            .Email = email,
            .Pass = password,
            .RoleId = roleId
        }

        Dim nuevoUsuarioId As Integer = InsertUsuarioYDevolverId(u)
        If nuevoUsuarioId <= 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(),
                "ServerControlScript",
                "Swal.fire('Error al registrar el usuario. Inténtalo de nuevo.');", True)
            lblError.Text = "Error al registrar el usuario. Inténtalo de nuevo."
            lblError.Visible = True
            Exit Sub
        End If

        ' Crear fila en Doctores o Pacientes si corresponde
        If rolNombre = "Doctor" Then
            InsertDoctorSiCorresponde(nombre, email)
        ElseIf rolNombre = "Paciente" Then
            InsertPacienteSiCorresponde(nuevoUsuarioId)
        End If

        ' Registro exitoso
        Response.Redirect("Login.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub
End Class