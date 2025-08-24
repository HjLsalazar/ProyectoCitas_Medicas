<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Admin.aspx.vb" Inherits="LoginRoles.Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-3">Panel de Administración</h3>

    <ul class="nav nav-tabs" id="adminTabs" role="tablist">
        <li class="nav-item" role="presentation">
            <button class="nav-link active" id="tab-usuarios" data-bs-toggle="tab" data-bs-target="#pane-usuarios" type="button" role="tab">Usuarios</button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link" id="tab-pacientes" data-bs-toggle="tab" data-bs-target="#pane-pacientes" type="button" role="tab">Pacientes</button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link" id="tab-doctores" data-bs-toggle="tab" data-bs-target="#pane-doctores" type="button" role="tab">Doctores</button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link" id="tab-citas" data-bs-toggle="tab" data-bs-target="#pane-citas" type="button" role="tab">Citas</button>
        </li>
    </ul>

    <div class="tab-content p-3 bg-white rounded-bottom shadow-sm">

        <!-- =============== USUARIOS =============== -->
        <div class="tab-pane fade show active" id="pane-usuarios" role="tabpanel">
            <div class="row g-2 mb-3">
                <div class="col-md-3"><asp:TextBox ID="txtUEmail" runat="server" CssClass="form-control" placeholder="Email" /></div>
                <div class="col-md-2"><asp:TextBox ID="txtUPass" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password" /></div>
                <div class="col-md-2"><asp:TextBox ID="txtUNombre" runat="server" CssClass="form-control" placeholder="Nombre" /></div>
                <div class="col-md-2"><asp:TextBox ID="txtUApellidos" runat="server" CssClass="form-control" placeholder="Apellidos" /></div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlURol" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Paciente" Value="1" Selected="True" />
                        <asp:ListItem Text="Administrador" Value="2" />
                        <asp:ListItem Text="Doctor" Value="3" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-1">
                    <asp:Button ID="btnUAgregar" runat="server" CssClass="btn btn-primary w-100" Text="Agregar" />
                </div>
            </div>

            <asp:GridView ID="gvUsuarios" runat="server" CssClass="table table-striped"
                AutoGenerateColumns="False" DataKeyNames="Id"
                OnRowEditing="gvUsuarios_RowEditing"
                OnRowCancelingEdit="gvUsuarios_RowCancelingEdit"
                OnRowUpdating="gvUsuarios_RowUpdating"
                OnRowDeleting="gvUsuarios_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="true" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" />
                    <asp:BoundField DataField="RoleId" HeaderText="Rol (1/2/3)" />
                    <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
                </Columns>
            </asp:GridView>
        </div>

        <!-- =============== PACIENTES =============== -->
        <div class="tab-pane fade" id="pane-pacientes" role="tabpanel">
            <div class="row g-2 mb-3">
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlPUsuarios" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-2"><asp:TextBox ID="txtPCedula" runat="server" CssClass="form-control" placeholder="Cédula" /></div>
                <div class="col-md-2"><asp:TextBox ID="txtPTelefono" runat="server" CssClass="form-control" placeholder="Teléfono" /></div>
                <div class="col-md-4"><asp:TextBox ID="txtPDireccion" runat="server" CssClass="form-control" placeholder="Dirección" /></div>
                <div class="col-md-1"><asp:Button ID="btnPAgregar" runat="server" CssClass="btn btn-primary w-100" Text="Agregar" /></div>
            </div>

            <asp:GridView ID="gvPacientesAdmin" runat="server" CssClass="table table-striped"
                AutoGenerateColumns="False" DataKeyNames="PacienteId"
                OnRowEditing="gvPacientesAdmin_RowEditing"
                OnRowCancelingEdit="gvPacientesAdmin_RowCancelingEdit"
                OnRowUpdating="gvPacientesAdmin_RowUpdating"
                OnRowDeleting="gvPacientesAdmin_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="PacienteId" HeaderText="ID" ReadOnly="True" />
                    <asp:BoundField DataField="UsuarioId" HeaderText="UsuarioId" ReadOnly="True" />
                    <asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="True" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" ReadOnly="True" />
                    <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" ReadOnly="True" />
                    <asp:BoundField DataField="Cedula" HeaderText="Cédula" />
                    <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                    <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                    <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
                </Columns>
            </asp:GridView>
        </div>

        <!-- =============== DOCTORES =============== -->
        <div class="tab-pane fade" id="pane-doctores" role="tabpanel">
            <div class="row g-2 mb-3">
                <div class="col-md-3"><asp:TextBox ID="txtDNombre" runat="server" CssClass="form-control" placeholder="Nombre" /></div>
                <div class="col-md-3"><asp:TextBox ID="txtDEspecialidad" runat="server" CssClass="form-control" placeholder="Especialidad" /></div>
                <div class="col-md-3"><asp:TextBox ID="txtDCorreo" runat="server" CssClass="form-control" placeholder="Correo" /></div>
                <div class="col-md-2"><asp:TextBox ID="txtDTelefono" runat="server" CssClass="form-control" placeholder="Teléfono" /></div>
                <div class="col-md-1"><asp:Button ID="btnDAgregar" runat="server" CssClass="btn btn-primary w-100" Text="Agregar" /></div>
            </div>

            <asp:GridView ID="gvDoctoresAdmin" runat="server" CssClass="table table-striped"
                AutoGenerateColumns="False" DataKeyNames="DoctorId"
                OnRowEditing="gvDoctoresAdmin_RowEditing"
                OnRowCancelingEdit="gvDoctoresAdmin_RowCancelingEdit"
                OnRowUpdating="gvDoctoresAdmin_RowUpdating"
                OnRowDeleting="gvDoctoresAdmin_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="DoctorId" HeaderText="ID" ReadOnly="True" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                    <asp:BoundField DataField="Correo" HeaderText="Correo" />
                    <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                    <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
                </Columns>
            </asp:GridView>
        </div>

        <!-- =============== CITAS =============== -->
        <div class="tab-pane fade" id="pane-citas" role="tabpanel">
            <div class="row g-2 mb-3">
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlCPaciente" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlCDoctor" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-2"><asp:TextBox ID="txtCFechaHora" runat="server" CssClass="form-control" placeholder="AAAA-MM-DD HH:MM" /></div>
                <div class="col-md-2"><asp:TextBox ID="txtCDuracion" runat="server" CssClass="form-control" placeholder="Min" /></div>
                <div class="col-md-1"><asp:TextBox ID="txtCMotivo" runat="server" CssClass="form-control" placeholder="Motivo" /></div>
                <div class="col-md-1"><asp:Button ID="btnCAgregar" runat="server" CssClass="btn btn-primary w-100" Text="Agregar" /></div>
            </div>

            <asp:GridView ID="gvCitasAdmin" runat="server" CssClass="table table-striped"
                AutoGenerateColumns="False" DataKeyNames="CitaId"
                OnRowEditing="gvCitasAdmin_RowEditing"
                OnRowCancelingEdit="gvCitasAdmin_RowCancelingEdit"
                OnRowUpdating="gvCitasAdmin_RowUpdating"
                OnRowDeleting="gvCitasAdmin_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="CitaId" HeaderText="ID" ReadOnly="True" />
                    <asp:BoundField DataField="FechaHora" HeaderText="Fecha/Hora" />
                    <asp:BoundField DataField="DuracionMinutos" HeaderText="Min" />
                    <asp:BoundField DataField="Motivo" HeaderText="Motivo" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                    <asp:BoundField DataField="Doctor" HeaderText="Doctor" ReadOnly="true" />
                    <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" ReadOnly="true" />
                    <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
                </Columns>
            </asp:GridView>
        </div>

    </div>
</asp:Content>
