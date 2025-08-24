<%@ Page Title="" Language="vb" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Login.aspx.vb" Inherits="LoginRoles.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="center-page">
        <div class="card shadow-lg p-4" style="max-width: 400px; width: 100%;">
            <div class="d-flex align-items-center py-4 bg-body-tertiary">
                <main class="form-signin w-100 m-auto">
                    <h1 class="h3 mb-3 fw-normal">Iniciar Sesion</h1>

                    <div class="form-floating">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="SingleLine" placeholder="Email"></asp:TextBox>
                        <label for="MainContent_txtEmail">Email </label>
                    </div>

                    <div class="form-floating">
                        <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password"></asp:TextBox>
                        <label for="MainContent_txtPass">Contraseña</label>
                    </div>

                    <div class="form-check text-start my-3">
                        <asp:CheckBox ID="ckbRecordar" runat="server" OnCheckedChanged="ckbRecordar_CheckedChanged" CssClass="form-check-label" Text="Recordar"/>
                    </div>
                    <asp:Button CssClass="btn btn-primary w-100 py-2" ID="btnLogin" runat="server" Text="Acceder" OnClick="btnLogin_Click" />
                </main>
            </div>
            <a href="Registro.aspx">¿Primera vez que ingresa?</a>
            <asp:Label ID="lblError" runat="server" Text="" CssClass="alert alert-danger" Visible="false"></asp:Label>

        </div>
    </div>
    <script>
        function guardarEmail(emailId, recordarId) {
            var email = document.getElementById(emailId).value.trim();
            var cb = document.getElementById(recordarId);

            console.log(email, cb.checked)
            if (cb && cb.checked) {
                localStorage.setItem('login.email', email);
            } else {
                localStorage.removeItem('login.email');
            }
        }

        function cargarEmail(emailId, recordarId) {
            var saved = localStorage.getItem('login.email');
            if (saved) {
                document.getElementById(emailId).value = saved;
                var cb = document.getElementById(recordarId);
                if (cb) cb.checked = true;
            }
        }
    </script>
</asp:Content>
