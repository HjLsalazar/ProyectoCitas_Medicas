<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Registro.aspx.vb" Inherits="LoginRoles.Registro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="center-page">
        <div class="card shadow-lg p-4" style="max-width: 400px; width: 100%;">
            <div class="card-body">
                <h2 class="h4 mb-3 text-center">Create an Account</h2>

                <div class="form-floating">
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" TextMode="SingleLine" placeholder="Name"></asp:TextBox>
                    <label for="MainContent_txtNombre">Name</label>
                </div>

                <div class="form-floating">
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="Email"></asp:TextBox>
                    <label for="MainContent_txtEmail">Email address</label>
                </div>

                <div class="form-floating">
                    <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password"></asp:TextBox>
                    <label for="MainContent_txtPass">Password</label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorPass"
                        ControlToValidate="txtPass"
                        Display="Dynamic"
                        ErrorMessage="La contraseña es requerida"
                        CssClass="text-danger"
                        runat="server" />
                </div>

                <asp:Panel ID="pnlRol" runat="server" Visible="false" CssClass="mb-3">
                    <label class="form-label d-block">Rol</label>
                    <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Paciente" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Administrador" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Doctor" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                </asp:Panel>


                <asp:Button CssClass="btn btn-primary w-100 py-2" ID="btnRegistrar" runat="server" Text="Registrarse" OnClick="btnRegistrar_Click" />
                <a href="Login.aspx">¿Ya estas registrado?</a>

                <asp:Label ID="lblError" runat="server" Text="" CssClass="error"></asp:Label>
            </div>
        </div>
    </div>


</asp:Content>

