<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Citas.aspx.vb"
    Inherits="LoginRoles.Citas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Citas</h2>
    <asp:Label ID="lblInfo" runat="server" CssClass="text-success" Visible="False"></asp:Label>
    <asp:Label ID="lblError" runat="server" CssClass="text-danger" Visible="False"></asp:Label>


    
    <asp:Panel ID="pnlNueva" runat="server" CssClass="mb-3">
        <div class="row g-2">
            <div class="col-md-3">
                <asp:DropDownList ID="ddlDoctores" runat="server" CssClass="form-select"></asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlDoctores"
                    InitialValue="" CssClass="text-danger" ErrorMessage="Seleccione doctor" />
            </div>

            <div class="col-md-3">
                <asp:TextBox ID="txtFechaHora" runat="server" CssClass="form-control" placeholder="AAAA-MM-DD HH:MM"></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtFechaHora"
                    CssClass="text-danger" ValidationExpression="^\d{4}\-\d{2}\-\d{2}\s\d{2}:\d{2}$"
                    ErrorMessage="Formato válido: AAAA-MM-DD HH:MM" />
            </div>

            <div class="col-md-2">
                <asp:TextBox ID="txtDuracion" runat="server" CssClass="form-control" placeholder="Minutos"></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtDuracion"
                    CssClass="text-danger" ValidationExpression="^\d{1,3}$" ErrorMessage="Solo minutos (1-999)" />
            </div>

            <div class="col-md-3">
                <asp:TextBox ID="txtMotivo" runat="server" CssClass="form-control" placeholder="Motivo"></asp:TextBox>
            </div>

            <div class="col-md-1">
                <asp:Button ID="btnAgregar" runat="server" CssClass="btn btn-primary w-100" Text="Reservar"
                    OnClick="btnAgregar_Click" />
            </div>
        </div>
    </asp:Panel>

 
    <asp:GridView ID="gvCitas" runat="server" CssClass="table table-striped"
        AutoGenerateColumns="False" DataKeyNames="CitaId">
        <Columns>
            <asp:BoundField DataField="CitaId" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="FechaHora" HeaderText="Fecha/Hora" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
            <asp:BoundField DataField="Min" HeaderText="Min" />
            <asp:BoundField DataField="Motivo" HeaderText="Motivo" />
            <asp:BoundField DataField="Estado" HeaderText="Estado" />
            <asp:BoundField DataField="Doctor" HeaderText="Doctor" ReadOnly="True" />
            <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" ReadOnly="True" />
            <asp:BoundField DataField="Paciente" HeaderText="Paciente" ReadOnly="True" />

            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />

  
            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:LinkButton ID="btnCancelar" runat="server"
                        Text="Cancelar"
                        CssClass="link-danger"
                        CommandName="Cancelar"
                        CommandArgument='<%# Eval("CitaId") %>'
                        OnClientClick="return confirm('¿Desea cancelar esta cita?');">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
