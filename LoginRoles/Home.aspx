<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Home.aspx.vb" Inherits="LoginRoles.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%-- Saludo y datos del usuario tomados desde sesión  --%>
  Bienvenido(a):
  <asp:Label ID="lblRol" runat="server" />
  &nbsp;–&nbsp;
  <asp:Label ID="lblEmail" runat="server" />

    <%-- Estilos del carrusel: altura fija y recorte proporcional --%>
    <style>
        #medCarousel .carousel-item img {
            height: 700px;
            object-fit: cover;
            width: 100%;
            display: block;
        }
    </style>
    <%-- Carrusel de imágenes médicas --%>
    <div id="medCarousel" class="carousel slide"
        data-bs-ride="carousel" data-bs-interval="4000" data-bs-pause="false"
        style="max-width: 1000px; margin: 0.75rem auto 0;">

        <div class="carousel-indicators">
            <button type="button" data-bs-target="#medCarousel" data-bs-slide-to="0" class="active"></button>
            <button type="button" data-bs-target="#medCarousel" data-bs-slide-to="1"></button>
            <button type="button" data-bs-target="#medCarousel" data-bs-slide-to="2"></button>
            <button type="button" data-bs-target="#medCarousel" data-bs-slide-to="3"></button>
            <button type="button" data-bs-target="#medCarousel" data-bs-slide-to="4"></button>
            <button type="button" data-bs-target="#medCarousel" data-bs-slide-to="5"></button>
            <button type="button" data-bs-target="#medCarousel" data-bs-slide-to="6"></button>
            <button type="button" data-bs-target="#medCarousel" data-bs-slide-to="7"></button>
        </div>


        <div class="carousel-inner rounded-3 shadow">
            <div class="carousel-item active">
                <img runat="server" src="~/Content/img/med/Sala.jpg" class="d-block w-100" alt="Sala médica" />
                <div class="carousel-caption d-none d-md-block">
                    <h5>Atención Médica</h5>
                    <p>Gestión de doctores, pacientes y citas</p>
                </div>
            </div>

            <div class="carousel-item">
                <img runat="server" src="~/Content/img/med/Estetoscopio.jpg" class="d-block w-100" alt="Estetoscopio" />
                <div class="carousel-caption d-none d-md-block">
                    <h5>Profesionales</h5>
                    <p>Equipo médico especializado</p>
                </div>
            </div>

            <div class="carousel-item">
                <img runat="server" src="~/Content/img/med/Hospital.jpg" class="d-block w-100" alt="Hospital moderno" />
                <div class="carousel-caption d-none d-md-block">
                    <h5>Infraestructura</h5>
                    <p>Instalaciones modernas y seguras</p>
                </div>
            </div>

            <div class="carousel-item">
                <img runat="server" src="~/Content/img/med/rayosx.jpg" class="d-block w-100" alt="Rayos X" />
                <div class="carousel-caption d-none d-md-block">
                    <h5>Diagnóstico</h5>
                    <p>Imagenología de alta precisión</p>
                </div>
            </div>

            <div class="carousel-item">
                <img runat="server" src="~/Content/img/med/laboratorio.jpg" class="d-block w-100" alt="Laboratorio" />
                <div class="carousel-caption d-none d-md-block">
                    <h5>Laboratorio</h5>
                    <p>Resultados confiables y oportunos</p>
                </div>
            </div>

            <div class="carousel-item">
                <img runat="server" src="~/Content/img/med/quirofano.jpg" class="d-block w-100" alt="Quirófano" />
                <div class="carousel-caption d-none d-md-block">
                    <h5>Cirugía</h5>
                    <p>Protocolos con estándares internacionales</p>
                </div>
            </div>

            <div class="carousel-item">
                <img runat="server" src="~/Content/img/med/telemedicina.jpg" class="d-block w-100" alt="Telemedicina" />
                <div class="carousel-caption d-none d-md-block">
                    <h5>Telemedicina</h5>
                    <p>Atención donde estés</p>
                </div>
            </div>

            <div class="carousel-item">
                <img runat="server" src="~/Content/img/med/Emergencias.jpg" class="d-block w-100" alt="Emergencias" />
                <div class="carousel-caption d-none d-md-block">
                    <h5>Emergencias</h5>
                    <p>Respuesta rápida y efectiva</p>
                </div>
            </div>
        </div>

        <button class="carousel-control-prev" type="button" data-bs-target="#medCarousel" data-bs-slide="prev">
            <span class="carousel-control-prev-icon"></span>
            <span class="visually-hidden">Anterior</span>
        </button>
        <button class="carousel-control-next" type="button" data-bs-target="#medCarousel" data-bs-slide="next">
            <span class="carousel-control-next-icon"></span>
            <span class="visually-hidden">Siguiente</span>
        </button>
    </div>

</asp:Content>


