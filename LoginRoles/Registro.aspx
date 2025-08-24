<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Registro.aspx.vb" Inherits="LoginRoles.Registro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                <asp:RequiredFieldValidator id="RequiredFieldValidatorPass"
                    ControlToValidate="txtPass"
                    Display="Dynamic"
                    ErrorMessage="La contraseña es requerida"
                    runat="server"/>
            </div>

            <asp:Button CssClass="btn btn-primary w-100 py-2" ID="btnRegistrar" runat="server" Text="Registrarse" OnClick="btnRegistrar_Click" />
        </div>

        <a href="Login.aspx">¿Ya estas registrado?</a>
    </div>
    <asp:Label ID="lblError" runat="server" Text="" CssClass="error"></asp:Label>

    <asp:Button ID="Prueba" runat="server" Text="Probar" OnClick="Prueba_Click" />

</asp:Content>

