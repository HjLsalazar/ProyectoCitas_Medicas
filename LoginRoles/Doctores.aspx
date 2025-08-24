<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Doctores.aspx.vb"
    Inherits="LoginRoles.Doctores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Doctores</h2>
    <asp:Label ID="lblInfo" runat="server" CssClass="text-success" Visible="False"></asp:Label>
    <asp:Label ID="lblError" runat="server" CssClass="text-danger" Visible="False"></asp:Label>


    <asp:Panel ID="pnlNuevo" runat="server" Visible="false" CssClass="mb-3">
        <div class="row g-2">
            <div class="col-md-3">
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Nombre"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre"
                    CssClass="text-danger" ErrorMessage="Nombre requerido" />
            </div>
            <div class="col-md-3">
                <asp:TextBox ID="txtEspecialidad" runat="server" CssClass="form-control" placeholder="Especialidad"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEspecialidad"
                    CssClass="text-danger" ErrorMessage="Especialidad requerida" />
            </div>

            <div class="col-md-3">
                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" placeholder="Correo"></asp:TextBox>
            </div>

            <div class="col-md-2">
                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" placeholder="Teléfono"></asp:TextBox>
            </div>

            <div class="col-md-1">
                <asp:Button ID="btnAgregar" runat="server" CssClass="btn btn-primary w-100" Text="Agregar" OnClick="btnAgregar_Click" />
            </div>
        </div>
    </asp:Panel>

    <asp:GridView ID="gvDoctores" runat="server" CssClass="table table-striped"
        AutoGenerateColumns="False" DataKeyNames="DoctorId">
        <Columns>
            <asp:BoundField DataField="DoctorId" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
            <asp:BoundField DataField="Correo" HeaderText="Correo" />
            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
        </Columns>
        <EmptyDataTemplate>
            No hay doctores registrados.
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>
