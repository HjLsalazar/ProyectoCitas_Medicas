Imports System.Web.UI.WebControls
Imports LoginRoles.Data


Partial Class Doctores

    Inherits System.Web.UI.Page

    ' Repositorio de Doctores
    Private ReadOnly _repo As New DoctorRepository()

    ' Control de acceso y carga inicial
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UsuarioId") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If

        Dim r As Integer = CInt(Session("RoleId"))

        ' Admin puede agregar; Paciente/Doctor NO
        pnlNuevo.Visible = (r = 2)

        If Not IsPostBack Then
            BindGrid()

            ' Columna Borrar solo para Admin
            If gvDoctores.Columns.Count > 0 Then
                gvDoctores.Columns(gvDoctores.Columns.Count - 1).Visible = (r = 2)
            End If
        End If
    End Sub

    ' Vincula el GridView con los datos del repositorio
    Private Sub BindGrid()
        gvDoctores.DataSource = _repo.GetAll()
        gvDoctores.DataBind()
    End Sub

    ' Evento del botón Agregar
    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Try
            Dim ok = _repo.Insert(
                txtNombre.Text.Trim(),
                txtEspecialidad.Text.Trim(),
                txtCorreo.Text.Trim(),
                txtTelefono.Text.Trim()
            )
            If ok Then
                lblInfo.Text = "Doctor agregado."
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

    ' Eventos del GridView para Editar, Actualizar, Cancelar y Borrar
    Protected Sub gvDoctores_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvDoctores.RowEditing
        gvDoctores.EditIndex = e.NewEditIndex
        BindGrid()
    End Sub

    ' Cancelar edición
    Protected Sub gvDoctores_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvDoctores.RowCancelingEdit
        gvDoctores.EditIndex = -1
        BindGrid()
    End Sub

    ' Actualizar datos
    Protected Sub gvDoctores_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvDoctores.RowUpdating
        Try
            Dim id = CInt(gvDoctores.DataKeys(e.RowIndex).Value)
            Dim row = gvDoctores.Rows(e.RowIndex)

            Dim nombre = CType(row.Cells(1).Controls(0), TextBox).Text.Trim()
            Dim esp = CType(row.Cells(2).Controls(0), TextBox).Text.Trim()
            Dim correo = CType(row.Cells(3).Controls(0), TextBox).Text.Trim()
            Dim tel = CType(row.Cells(4).Controls(0), TextBox).Text.Trim()
            ' Actualizar en la base de datos
            Dim ok = _repo.Update(id, nombre, esp, correo, tel)
            If ok Then
                gvDoctores.EditIndex = -1
                BindGrid()
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
            lblError.Visible = True
        End Try
    End Sub

    ' Borrar registro
    Protected Sub gvDoctores_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvDoctores.RowDeleting
        Try
            Dim id = CInt(gvDoctores.DataKeys(e.RowIndex).Value)
            If _repo.Delete(id) Then
                BindGrid()
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
            lblError.Visible = True
        End Try
    End Sub
End Class
