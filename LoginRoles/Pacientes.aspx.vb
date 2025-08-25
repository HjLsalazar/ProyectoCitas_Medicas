Imports System.Web.UI.WebControls
Imports LoginRoles.Data

Public Class Pacientes
    Inherits System.Web.UI.Page

    ' Repositorio de Pacientes
    Private ReadOnly _repo As New PacienteRepository()

    ' Carga inicial de la página
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UsuarioId") Is Nothing Then
            Response.Redirect("Login.aspx")
            Return
        End If

        Dim r As Integer = CInt(Session("RoleId"))

        ' Sólo Admin ve el panel de nuevo paciente
        pnlNuevo.Visible = (r = 2)

        If Not IsPostBack Then
            BindGrid()

            ' Sólo Admin puede editar/eliminar (última columna con CommandField)
            If gvPacientes.Columns.Count > 0 Then
                gvPacientes.Columns(gvPacientes.Columns.Count - 1).Visible = (r = 2)
            End If
        End If
    End Sub

    ' Vincula el GridView con los datos del repositorio
    Private Sub BindGrid()
        Dim roleId = CInt(Session("RoleId"))
        Dim usuarioId = CInt(Session("UsuarioId"))

        If roleId = 2 OrElse roleId = 3 Then
            ' Admin y Doctor: todos los pacientes
            gvPacientes.DataSource = _repo.GetAll()
        Else
            ' Paciente: sólo sus datos
            gvPacientes.DataSource = _repo.GetByUsuarioId(usuarioId)
        End If

        gvPacientes.DataBind()
    End Sub

    ' Crear nuevo paciente: Sólo Admin
    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        If CInt(Session("RoleId")) <> 2 Then
            lblError.Text = "No autorizado."
            lblError.Visible = True
            Return
        End If

        Try
            Dim ok = _repo.Insert(
                CInt(Session("UsuarioId")),
                txtCedula.Text.Trim(),
                txtTelefono.Text.Trim(),
                txtDireccion.Text.Trim()
            )
            If ok Then
                lblInfo.Text = "Paciente agregado."
                lblInfo.Visible = True
                lblError.Visible = False
                BindGrid()
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
            lblError.Visible = True
            lblInfo.Visible = False
        End Try
    End Sub

    ' Editar paciente: Sólo Admin
    Protected Sub gvPacientes_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvPacientes.RowEditing
        If CInt(Session("RoleId")) <> 2 Then
            ' Bloquear edición para Doctor y Paciente
            lblError.Text = "Sólo el administrador puede editar."
            lblError.Visible = True
            Return
        End If
        gvPacientes.EditIndex = e.NewEditIndex
        BindGrid()
    End Sub

    ' Cancelar edición
    Protected Sub gvPacientes_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvPacientes.RowCancelingEdit
        gvPacientes.EditIndex = -1
        BindGrid()
    End Sub

    ' Actualizar paciente: Sólo Admin
    Protected Sub gvPacientes_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvPacientes.RowUpdating
        If CInt(Session("RoleId")) <> 2 Then
            lblError.Text = "No autorizado."
            lblError.Visible = True
            Return
        End If

        ' Obtener datos editados
        Try
            Dim id = CInt(gvPacientes.DataKeys(e.RowIndex).Value)
            Dim row = gvPacientes.Rows(e.RowIndex)

            Dim cedula = CType(row.Cells(5).Controls(0), TextBox).Text.Trim()
            Dim telefono = CType(row.Cells(6).Controls(0), TextBox).Text.Trim()
            Dim direccion = CType(row.Cells(7).Controls(0), TextBox).Text.Trim()

            ' Validaciones básicas
            Dim ok = _repo.Update(id, cedula, telefono, direccion)
            If ok Then
                gvPacientes.EditIndex = -1
                BindGrid()
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
            lblError.Visible = True
        End Try
    End Sub

    ' Eliminar paciente: Sólo Admin
    Protected Sub gvPacientes_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvPacientes.RowDeleting
        If CInt(Session("RoleId")) <> 2 Then
            e.Cancel = True
            lblError.Text = "No autorizado."
            lblError.Visible = True
            Return
        End If

        Try
            Dim id = CInt(gvPacientes.DataKeys(e.RowIndex).Value)
            If _repo.Delete(id) Then
                BindGrid()
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
            lblError.Visible = True
        End Try
    End Sub
End Class
