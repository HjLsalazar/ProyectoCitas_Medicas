Imports System.Data
Imports System.Web.UI.WebControls
Imports LoginRoles.Data   ' <-- Namespace donde está tu CitaRepository, PacienteRepository, DoctorRepository

Partial Class Citas    ' Debe coincidir con Inherits="LoginRoles.Citas" del .aspx
    Inherits System.Web.UI.Page

    Private ReadOnly _repoCita As New CitaRepository()
    Private ReadOnly _repoPaciente As New PacienteRepository()
    Private ReadOnly _repoDoctor As New DoctorRepository()

    Private ReadOnly Property RoleId As Integer
        Get
            Return CInt(Session("RoleId"))
        End Get
    End Property

    Private ReadOnly Property UsuarioId As Integer
        Get
            Return CInt(Session("UsuarioId"))
        End Get
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UsuarioId") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If

        If Not IsPostBack Then
            CargarDoctores()
            BindGrid()
        End If
    End Sub

    Private Sub CargarDoctores()
        Dim dt = _repoDoctor.GetAll()
        ddlDoctores.Items.Clear()
        ddlDoctores.Items.Add(New ListItem("-- Seleccione --", ""))
        For Each r As DataRow In dt.Rows
            ddlDoctores.Items.Add(New ListItem($"{r("Nombre")} ({r("Especialidad")})", r("DoctorId").ToString()))
        Next
    End Sub

    Private Sub BindGrid()
        gvCitas.DataSource = _repoCita.GetList(RoleId, UsuarioId)
        gvCitas.DataBind()
    End Sub

    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Try
            ' Buscar PacienteId del usuario en sesión
            Dim dtPac = _repoPaciente.GetByUsuarioId(UsuarioId)
            If dtPac.Rows.Count = 0 Then Throw New Exception("Primero complete su ficha de Paciente.")

            Dim pacienteId = CInt(dtPac.Rows(0)("PacienteId"))
            Dim doctorId = CInt(ddlDoctores.SelectedValue)
            Dim fecha = DateTime.ParseExact(txtFechaHora.Text.Trim(), "yyyy-MM-dd HH:mm", Nothing)
            Dim dur = Integer.Parse(txtDuracion.Text.Trim())
            Dim motivo = txtMotivo.Text.Trim()

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

    ' Usa Handles para enlazar eventos sin tocar el .aspx
    Protected Sub gvCitas_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvCitas.RowEditing
        gvCitas.EditIndex = e.NewEditIndex
        BindGrid()
    End Sub

    Protected Sub gvCitas_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvCitas.RowCancelingEdit
        gvCitas.EditIndex = -1
        BindGrid()
    End Sub

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
