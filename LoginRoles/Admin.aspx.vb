Imports System.Data
Imports System.Data.SqlClient
Imports LoginRoles.Helpers
Imports LoginRoles.Data
Imports LoginRoles.Models
Public Class Admin
    Inherits System.Web.UI.Page


    ' repositorios
    Private ReadOnly uRepo As New UsuarioRepository()
    Private ReadOnly pRepo As New PacienteRepository()
    Private ReadOnly dRepo As New DoctorRepository()
    Private ReadOnly cRepo As New CitaRepository()
    Private ReadOnly db As New DatabaseHelper()

    ' carga inicial
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' validar sesión y rol
        If Session("UsuarioId") Is Nothing Then Response.Redirect("Login.aspx")
        If Session("RoleId") Is Nothing OrElse CInt(Session("RoleId")) <> 2 Then Response.Redirect("Home.aspx")

        If Not IsPostBack Then
            BindAll()
        End If
    End Sub

    ' enlazar todo
    Private Sub BindAll()
        BindUsuarios()
        BindPacientes()
        BindDoctores()
        BindCitas()

        ' llenar dropdowns
        ddlPUsuarios.DataSource = uRepo.GetAll()
        ddlPUsuarios.DataTextField = "Email"
        ddlPUsuarios.DataValueField = "Id"
        ddlPUsuarios.DataBind()

        ddlCPaciente.DataSource = pRepo.GetAll()
        ddlCPaciente.DataTextField = "Cedula"
        ddlCPaciente.DataValueField = "PacienteId"
        ddlCPaciente.DataBind()

        ddlCDoctor.DataSource = dRepo.GetAll()
        ddlCDoctor.DataTextField = "Nombre"
        ddlCDoctor.DataValueField = "DoctorId"
        ddlCDoctor.DataBind()
    End Sub


    ' ---------------------- USUARIOS ----------------------

    ' Mostrar usuarios
    Private Sub BindUsuarios()
        gvUsuarios.DataSource = uRepo.GetAll()
        gvUsuarios.DataBind()
    End Sub

    ' Agregar usuario
    Protected Sub btnUAgregar_Click(sender As Object, e As EventArgs) Handles btnUAgregar.Click
        Try
            Dim wrapper As New Simple3Des("claveclavecita")
            Dim passEnc = wrapper.EncryptData(txtUPass.Text.Trim())
            Dim ok = uRepo.Insert(
                txtUEmail.Text.Trim(),
                passEnc,
                txtUNombre.Text.Trim(),
                txtUApellidos.Text.Trim(),
                CInt(ddlURol.SelectedValue)
            )
            If ok Then
                txtUEmail.Text = "" : txtUPass.Text = "" : txtUNombre.Text = "" : txtUApellidos.Text = ""
                BindUsuarios()
            End If
        Catch ex As Exception

        End Try
    End Sub

    ' Editar, actualizar, cancelar, eliminar
    Protected Sub gvUsuarios_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gvUsuarios.EditIndex = e.NewEditIndex : BindUsuarios()
    End Sub

    ' Cancelar edición
    Protected Sub gvUsuarios_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gvUsuarios.EditIndex = -1 : BindUsuarios()
    End Sub

    ' Actualizar usuario
    Protected Sub gvUsuarios_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Dim id = CInt(gvUsuarios.DataKeys(e.RowIndex).Value)
        Dim row = gvUsuarios.Rows(e.RowIndex)
        Dim email = CType(row.Cells(1).Controls(0), TextBox).Text.Trim()
        Dim nombre = CType(row.Cells(2).Controls(0), TextBox).Text.Trim()
        Dim apellidos = CType(row.Cells(3).Controls(0), TextBox).Text.Trim()
        Dim roleId = CInt(CType(row.Cells(4).Controls(0), TextBox).Text.Trim())

        If uRepo.Update(id, email, nombre, apellidos, roleId) Then
            gvUsuarios.EditIndex = -1 : BindUsuarios()
        End If
    End Sub

    ' Eliminar usuario
    Protected Sub gvUsuarios_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim id = CInt(gvUsuarios.DataKeys(e.RowIndex).Value)
        If uRepo.Delete(id) Then BindUsuarios()
    End Sub

    ' ---------------------- PACIENTES ----------------------

    ' Mostrar pacientes
    Private Sub BindPacientes()
        gvPacientesAdmin.DataSource = pRepo.GetAll()
        gvPacientesAdmin.DataBind()
    End Sub

    ' Agregar paciente
    Protected Sub btnPAgregar_Click(sender As Object, e As EventArgs) Handles btnPAgregar.Click
        Try
            Dim ok = pRepo.Insert(
                CInt(ddlPUsuarios.SelectedValue),
                txtPCedula.Text.Trim(),
                txtPTelefono.Text.Trim(),
                txtPDireccion.Text.Trim()
            )
            If ok Then
                txtPCedula.Text = "" : txtPTelefono.Text = "" : txtPDireccion.Text = ""
                BindPacientes()
            End If
        Catch ex As Exception
        End Try
    End Sub

    ' Editar, actualizar, cancelar, eliminar
    Protected Sub gvPacientesAdmin_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gvPacientesAdmin.EditIndex = e.NewEditIndex : BindPacientes()
    End Sub

    ' Cancelar edición
    Protected Sub gvPacientesAdmin_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gvPacientesAdmin.EditIndex = -1 : BindPacientes()
    End Sub

    ' Actualizar paciente
    Protected Sub gvPacientesAdmin_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Dim id = CInt(gvPacientesAdmin.DataKeys(e.RowIndex).Value)
        Dim row = gvPacientesAdmin.Rows(e.RowIndex)
        Dim cedula = CType(row.Cells(5).Controls(0), TextBox).Text.Trim()
        Dim tel = CType(row.Cells(6).Controls(0), TextBox).Text.Trim()
        Dim dir = CType(row.Cells(7).Controls(0), TextBox).Text.Trim()

       
        If pRepo.Update(id, cedula, tel, dir) Then
            gvPacientesAdmin.EditIndex = -1 : BindPacientes()
        End If
    End Sub

    ' Eliminar paciente
    Protected Sub gvPacientesAdmin_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim id = CInt(gvPacientesAdmin.DataKeys(e.RowIndex).Value)
        If pRepo.Delete(id) Then BindPacientes()
    End Sub

    ' ---------------------- DOCTORES ----------------------

    ' Mostrar doctores
    Private Sub BindDoctores()
        gvDoctoresAdmin.DataSource = dRepo.GetAll()
        gvDoctoresAdmin.DataBind()
    End Sub

    ' Agregar doctor
    Protected Sub btnDAgregar_Click(sender As Object, e As EventArgs) Handles btnDAgregar.Click
        Try
            Dim ok = dRepo.Insert(
                txtDNombre.Text.Trim(),
                txtDEspecialidad.Text.Trim(),
                txtDCorreo.Text.Trim(),
                txtDTelefono.Text.Trim()
            )
            If ok Then
                txtDNombre.Text = "" : txtDEspecialidad.Text = "" : txtDCorreo.Text = "" : txtDTelefono.Text = ""
                BindDoctores()
            End If
        Catch ex As Exception
        End Try
    End Sub

    ' Editar, actualizar, cancelar, eliminar
    Protected Sub gvDoctoresAdmin_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gvDoctoresAdmin.EditIndex = e.NewEditIndex : BindDoctores()
    End Sub

    ' Cancelar edición
    Protected Sub gvDoctoresAdmin_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gvDoctoresAdmin.EditIndex = -1 : BindDoctores()
    End Sub

    ' Actualizar doctor
    Protected Sub gvDoctoresAdmin_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Dim id = CInt(gvDoctoresAdmin.DataKeys(e.RowIndex).Value)
        Dim row = gvDoctoresAdmin.Rows(e.RowIndex)
        Dim nombre = CType(row.Cells(1).Controls(0), TextBox).Text.Trim()
        Dim esp = CType(row.Cells(2).Controls(0), TextBox).Text.Trim()
        Dim correo = CType(row.Cells(3).Controls(0), TextBox).Text.Trim()
        Dim tel = CType(row.Cells(4).Controls(0), TextBox).Text.Trim()

        If dRepo.Update(id, nombre, esp, correo, tel) Then
            gvDoctoresAdmin.EditIndex = -1 : BindDoctores()
        End If
    End Sub

    ' Eliminar doctor
    Protected Sub gvDoctoresAdmin_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim id = CInt(gvDoctoresAdmin.DataKeys(e.RowIndex).Value)
        If dRepo.Delete(id) Then BindDoctores()
    End Sub

    ' ---------------------- CITAS ----------------------

    ' Mostrar citas
    Private Sub BindCitas()
        ' Mostrar citas con detalles de paciente y doctor
        gvCitasAdmin.DataSource = cRepo.GetList(2, 0)
        gvCitasAdmin.DataBind()
    End Sub

    ' Agregar cita
    Protected Sub btnCAgregar_Click(sender As Object, e As EventArgs) Handles btnCAgregar.Click
        Try
            Dim fecha = DateTime.ParseExact(txtCFechaHora.Text.Trim(), "yyyy-MM-dd HH:mm", Nothing)
            Dim ok = cRepo.Insert(
                CInt(ddlCPaciente.SelectedValue),
                CInt(ddlCDoctor.SelectedValue),
                fecha,
                Integer.Parse(txtCDuracion.Text.Trim()),
                txtCMotivo.Text.Trim(),
                "Pendiente"
            )
            If ok Then
                txtCFechaHora.Text = "" : txtCDuracion.Text = "" : txtCMotivo.Text = ""
                BindCitas()
            End If
        Catch ex As Exception
        End Try
    End Sub

    ' Editar, actualizar, cancelar, eliminar
    Protected Sub gvCitasAdmin_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gvCitasAdmin.EditIndex = e.NewEditIndex : BindCitas()
    End Sub
    Protected Sub gvCitasAdmin_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gvCitasAdmin.EditIndex = -1 : BindCitas()
    End Sub

    ' Actualizar cita
    Protected Sub gvCitasAdmin_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Try
            Dim id = CInt(gvCitasAdmin.DataKeys(e.RowIndex).Value)
            Dim row = gvCitasAdmin.Rows(e.RowIndex)

            Dim fechaTxt = CType(row.Cells(1).Controls(0), TextBox).Text.Trim()
            Dim durTxt = CType(row.Cells(2).Controls(0), TextBox).Text.Trim()
            Dim motivoTxt = CType(row.Cells(3).Controls(0), TextBox).Text.Trim()
            Dim estadoTxt = CType(row.Cells(4).Controls(0), TextBox).Text.Trim()

            ' Obtener PacienteId y DoctorId actuales desde la base de datos
            Dim dtP = db.ExecuteQuery("SELECT PacienteId, DoctorId FROM Citas WHERE CitaId=@Id",
                                      New List(Of SqlParameter) From {db.CreateParameter("@Id", id)})
            Dim pacId = CInt(dtP.Rows(0)("PacienteId"))
            Dim docId = CInt(dtP.Rows(0)("DoctorId"))

            ' Actualizar cita
            Dim ok = cRepo.Update(
                id,
                pacId,
                docId,
                DateTime.ParseExact(fechaTxt, "yyyy-MM-dd HH:mm", Nothing),
                Integer.Parse(durTxt),
                motivoTxt,
                If(String.IsNullOrWhiteSpace(estadoTxt), "Pendiente", estadoTxt)
            )
            If ok Then
                gvCitasAdmin.EditIndex = -1 : BindCitas()
            End If
        Catch ex As Exception
        End Try
    End Sub

    ' Eliminar cita
    Protected Sub gvCitasAdmin_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim id = CInt(gvCitasAdmin.DataKeys(e.RowIndex).Value)
        If cRepo.Delete(id) Then BindCitas()
    End Sub

    ' Limpia todos los controles del formulario de Citas (Admin)
    Private Sub ResetCitasForm()
        ' Paciente
        ddlCPaciente.ClearSelection()
        If ddlCPaciente.Items.FindByValue("") Is Nothing Then
            ddlCPaciente.Items.Insert(0, New ListItem("-- Seleccione --", ""))
        End If
        ddlCPaciente.SelectedValue = ""

        ' Doctor
        ddlCDoctor.ClearSelection()
        If ddlCDoctor.Items.FindByValue("") Is Nothing Then
            ddlCDoctor.Items.Insert(0, New ListItem("-- Seleccione --", ""))
        End If
        ddlCDoctor.SelectedValue = ""

        ' TextBox
        txtCFechaHora.Text = String.Empty
        txtCDuracion.Text = String.Empty
        txtCMotivo.Text = String.Empty

        ddlCPaciente.Focus()
    End Sub

End Class