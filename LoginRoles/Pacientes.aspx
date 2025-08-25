<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Pacientes.aspx.vb"
    Inherits="LoginRoles.Pacientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Pacientes</h2>
    <asp:Label ID="lblInfo" runat="server" CssClass="text-success" Visible="False"></asp:Label>
    <asp:Label ID="lblError" runat="server" CssClass="text-danger" Visible="False"></asp:Label>
    
    <asp:Panel ID="pnlNuevo" runat="server"  CssClass="mb-3">
        <div class="row g-2">
            <div class="col-md-3">
                <asp:TextBox ID="txtCedula" runat="server" CssClass="form-control" placeholder="Cedula"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCedula" CssClass="text-danger" ErrorMessage="Cedula requerida" />
            </div>

            <div class="col-md-3">
                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" placeholder="Teléfono"></asp:TextBox>
            </div>

            <div class="col-md-4">
                <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" placeholder="Dirección"></asp:TextBox>
            </div>

            <div class="col-md-2">
                <asp:Button ID="btnAgregar" runat="server" CssClass="btn btn-primary w-100" Text="Agregar" OnClick="btnAgregar_Click" />
            </div>
        </div>
    </asp:Panel>

    <asp:GridView ID="gvPacientes" runat="server" CssClass="table table-striped"
        AutoGenerateColumns="False" DataKeyNames="PacienteId">
        <Columns>
            <asp:BoundField DataField="PacienteId" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="UsuarioId" HeaderText="UsuarioId" ReadOnly="True" />
            <asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="True" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" ReadOnly="True" />
            <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" ReadOnly="True" />
            <asp:BoundField DataField="Cedula" HeaderText="Cédula" />
            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
            <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
        </Columns>
        <EmptyDataTemplate>
            No hay pacientes registrados.
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>
