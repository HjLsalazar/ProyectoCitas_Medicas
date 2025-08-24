Imports System.Data
Imports System.Web.UI.WebControls
Imports LoginRoles.Data
Partial Class Citas
    Inherits System.Web.UI.Page

    ' Repositorios para acceder a datos
    Private ReadOnly _repoCita As New CitaRepository()
    Private ReadOnly _repoPaciente As New PacienteRepository()
    Private ReadOnly _repoDoctor As New DoctorRepository()

    ' Propiedades para obtener datos de sesión
    Private ReadOnly Property RoleId As Integer
        Get
            Return CInt(Session("RoleId"))
        End Get
    End Property

    ' UsuarioId del usuario en sesión
    Private ReadOnly Property UsuarioId As Integer
        Get
            Return CInt(Session("UsuarioId"))
        End Get
    End Property

    ' Cargar datos al iniciar la página
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UsuarioId") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If

        ' Mostrar u ocultar panel de nueva cita según rol
        Dim r As Integer = CInt(Session("RoleId"))

        ' Solo Paciente (3) no ve el panel
        pnlNueva.Visible = (r = 1 OrElse r = 2)

        If Not IsPostBack Then
            CargarDoctores()
            BindGrid()

            If gvCitas.Columns.Count > 0 Then
                gvCitas.Columns(gvCitas.Columns.Count - 1).Visible = (r = 1 OrElse r = 2)
            End If
        End If
    End Sub

    ' Cargar lista de doctores en el DropDownList
    Private Sub CargarDoctores()
        Dim dt = _repoDoctor.GetAll()
        ddlDoctores.Items.Clear()
        ddlDoctores.Items.Add(New ListItem("-- Seleccione --", ""))
        For Each r As DataRow In dt.Rows
            ddlDoctores.Items.Add(New ListItem($"{r("Nombre")} ({r("Especialidad")})", r("DoctorId").ToString()))
        Next
    End Sub

    ' Enlazar datos al GridView
    Private Sub BindGrid()
        gvCitas.DataSource = _repoCita.GetList(RoleId, UsuarioId)
        gvCitas.DataBind()
    End Sub

    ' Manejar clic en botón Agregar
    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Try
            ' Validar campos
            Dim dtPac = _repoPaciente.GetByUsuarioId(UsuarioId)
            If dtPac.Rows.Count = 0 Then Throw New Exception("Primero complete su ficha de Paciente.")

            Dim pacienteId = CInt(dtPac.Rows(0)("PacienteId"))
            Dim doctorId = CInt(ddlDoctores.SelectedValue)
            Dim fecha = DateTime.ParseExact(txtFechaHora.Text.Trim(), "yyyy-MM-dd HH:mm", Nothing)
            Dim dur = Integer.Parse(txtDuracion.Text.Trim())
            Dim motivo = txtMotivo.Text.Trim()
            ' Validaciones básicas
            Dim ok = _repoCita.Insert(pacienteId, doctorId, fecha, dur, motivo, "Pendiente")
            If ok Then
                lblInfo.Text = "Cita reservada."
                lblInfo.Visible = True : lblError.Visible = False
                BindGrid()
            End If
        Catch ex As Exception
            lblError.Text = ex.Message : lblError.Visible = True : lblInfo.Visible = False
        End Try
    End Sub

    ' Manejar edición de filas en el GridView
    Protected Sub gvCitas_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvCitas.RowEditing
        gvCitas.EditIndex = e.NewEditIndex
        BindGrid()
    End Sub

    ' Manejar cancelación de edición
    Protected Sub gvCitas_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvCitas.RowCancelingEdit
        gvCitas.EditIndex = -1
        BindGrid()
    End Sub

    ' Manejar actualización de filas
    Protected Sub gvCitas_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvCitas.RowUpdating
        Try
            Dim id = CInt(gvCitas.DataKeys(e.RowIndex).Value)
            Dim row = gvCitas.Rows(e.RowIndex)

            Dim fechaTxt = CType(row.Cells(1).Controls(0), TextBox).Text.Trim()
            Dim durTxt = CType(row.Cells(2).Controls(0), TextBox).Text.Trim()
            Dim motivoTxt = CType(row.Cells(3).Controls(0), TextBox).Text.Trim()
            Dim estadoTxt = CType(row.Cells(4).Controls(0), TextBox).Text.Trim()

            Dim doctorActual = _repoCita.GetDoctorIdByCita(id)  ' mantener doctor asignado
            Dim dtPac = _repoPaciente.GetByUsuarioId(UsuarioId)
            Dim pacienteId = If(dtPac.Rows.Count > 0, CInt(dtPac.Rows(0)("PacienteId")), 0)

            ' Validaciones básicas
            Dim ok = _repoCita.Update(
                id,
                pacienteId,
                doctorActual,
                DateTime.ParseExact(fechaTxt, "yyyy-MM-dd HH:mm", Nothing),
                Integer.Parse(durTxt),
                motivoTxt,
                If(String.IsNullOrWhiteSpace(estadoTxt), "Pendiente", estadoTxt)
            )
            If ok Then
                gvCitas.EditIndex = -1
                BindGrid()
            End If
        Catch ex As Exception
            lblError.Text = ex.Message : lblError.Visible = True
        End Try
    End Sub

    ' Manejar eliminación de filas
    Protected Sub gvCitas_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvCitas.RowDeleting
        Try
            Dim id = CInt(gvCitas.DataKeys(e.RowIndex).Value)
            If _repoCita.Delete(id) Then
                BindGrid()
            End If
        Catch ex As Exception
            lblError.Text = ex.Message : lblError.Visible = True
        End Try
    End Sub
End Class
