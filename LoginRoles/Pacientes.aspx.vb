Imports System.Web.UI.WebControls
Imports LoginRoles.Data

Public Class Pacientes
    Inherits System.Web.UI.Page

    ' Repositorio de pacientes
    Private ReadOnly _repo As New PacienteRepository()

    ' Evento de carga de la página
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UsuarioId") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If

        ' Controla la visibilidad del panel de nuevo paciente según el rol
        Dim r As Integer = CInt(Session("RoleId"))

        pnlNuevo.Visible = (r = 1 OrElse r = 2 OrElse r = 3)

        If Not IsPostBack Then
            BindGrid()

            ' Muestra la columna de acciones solo para roles 1 (Admin) y 3 (Doctor)
            If gvPacientes.Columns.Count > 0 Then
                gvPacientes.Columns(gvPacientes.Columns.Count - 1).Visible = True
            End If
        End If
    End Sub

    ' Enlaza los datos al GridView
    Private Sub BindGrid()
        Dim roleId = CInt(Session("RoleId"))
        Dim usuarioId = CInt(Session("UsuarioId"))

        If roleId = 2 Then
            gvPacientes.DataSource = _repo.GetAll()
        Else
            gvPacientes.DataSource = _repo.GetByUsuarioId(usuarioId)
        End If
        gvPacientes.DataBind()
    End Sub

    ' Evento para agregar un nuevo paciente
    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Try
            ' Validaciones básicas
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

    ' Eventos del GridView para editar, actualizar y eliminar
    Protected Sub gvPacientes_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvPacientes.RowEditing
        gvPacientes.EditIndex = e.NewEditIndex
        BindGrid()
    End Sub

    Protected Sub gvPacientes_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvPacientes.RowCancelingEdit
        gvPacientes.EditIndex = -1
        BindGrid()
    End Sub

    ' Evento para actualizar un paciente
    Protected Sub gvPacientes_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvPacientes.RowUpdating
        Try

            Dim id = CInt(gvPacientes.DataKeys(e.RowIndex).Value)
            Dim row = gvPacientes.Rows(e.RowIndex)

            Dim cedula = CType(row.Cells(5).Controls(0), TextBox).Text.Trim()
            Dim telefono = CType(row.Cells(6).Controls(0), TextBox).Text.Trim()
            Dim direccion = CType(row.Cells(7).Controls(0), TextBox).Text.Trim()

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

    ' Evento para eliminar un paciente
    Protected Sub gvPacientes_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvPacientes.RowDeleting
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
