<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/principal.Master" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="global.dashboard" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfId" runat="server" />
    <asp:HiddenField ID="hdfIdUsuario" runat="server" />
    <div class="page-title-container">
        <div class="row">
            <%-- Título da Página --%>
            <div class="col-12 col-md-7">
                <a class="muted-link pb-2 d-inline-block hidden" href="#">
                    <span class="align-middle lh-1 text-small">&nbsp;</span>
                </a>
                <h1 class="mb-0 pb-0 display-4" id="title">
                    <%--<asp:Label ID="lblMensagemBoasVindas" runat="server"></asp:Label>--%>
                    Bem Vindo!
                </h1>
            </div>
        </div>
    </div>

    <!-- Filtros -->
    <div class="row">
        <div class="col-12">
            <div class="d-flex">
                <div class="dropdown-as-select me-3" data-setactive="false" data-childselector="span">
                    <a class="pe-0 pt-0 align-top lh-1 dropdown-toggle" href="#" data-bs-toggle="dropdown" aria-expanded="false" aria-haspopup="true">
                        <span class="small-title"></span>
                    </a>
                    <div class="dropdown-menu font-standard" aria-labelledby="dropdownMenuButton">
                        <div class="nav flex-column" role="tablist">
                            <a class="active dropdown-item text-medium" href="#" aria-selected="true" role="tab">Hoje</a>
                            <a class="dropdown-item text-medium" href="#" aria-selected="false" role="tab">Semana</a>
                            <a class="dropdown-item text-medium" href="#" aria-selected="false" role="tab">Mês</a>
                            <a class="dropdown-item text-medium" href="#" aria-selected="false" role="tab">Ano</a>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Cards --%>
            <div class="mb-5">
                <div class="row g-2">
                    <div class="col-6 col-md-4 col-lg-2">
                        <div class="card h-100 hover-scale-up cursor-pointer">
                            <div class="card-body d-flex flex-column align-items-center">
                                <div class="sw-6 sh-6 rounded-xl d-flex justify-content-center align-items-center border border-primary mb-4">
                                    <i data-acorn-icon="dollar" class="text-primary"></i>
                                </div>
                                <div class="mb-1 d-flex align-items-center text-alternate text-small lh-1-25">Mensalidades</div>
                                <div class="text-primary cta-4">
                                    <asp:Label ID="lblMensalidades" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-4 col-lg-2">
                        <div class="card h-100 hover-scale-up cursor-pointer">
                            <div class="card-body d-flex flex-column align-items-center">
                                <div class="sw-6 sh-6 rounded-xl d-flex justify-content-center align-items-center border border-primary mb-4">
                                    <i data-acorn-icon="shop" class="text-primary"></i>
                                </div>
                                <div class="mb-1 d-flex align-items-center text-alternate text-small lh-1-25">Empresas</div>
                                <div class="text-primary cta-4">
                                    <asp:Label ID="lblEmpresas" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-4 col-lg-2">
                        <div class="card h-100 hover-scale-up cursor-pointer">
                            <div class="card-body d-flex flex-column align-items-center">
                                <div class="sw-6 sh-6 rounded-xl d-flex justify-content-center align-items-center border border-primary mb-4">
                                    <i data-acorn-icon="server" class="text-primary"></i>
                                </div>
                                <div class="mb-1 d-flex align-items-center text-alternate text-small lh-1-25">Acessos</div>
                                <div class="text-primary cta-4">
                                    <asp:Label ID="lblAcessos" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-4 col-lg-2">
                        <div class="card h-100 hover-scale-up cursor-pointer">
                            <div class="card-body d-flex flex-column align-items-center">
                                <div class="sw-6 sh-6 rounded-xl d-flex justify-content-center align-items-center border border-primary mb-4">
                                    <i data-acorn-icon="user" class="text-primary"></i>
                                </div>
                                <div class="mb-1 d-flex align-items-center text-alternate text-small lh-1-25">Visitantes</div>
                                <div class="text-primary cta-4">
                                    <asp:Label ID="lblVisitantes" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-4 col-lg-2">
                        <div class="card h-100 hover-scale-up cursor-pointer">
                            <div class="card-body d-flex flex-column align-items-center">
                                <div class="sw-6 sh-6 rounded-xl d-flex justify-content-center align-items-center border border-primary mb-4">
                                    <i data-acorn-icon="arrow-top-left" class="text-primary"></i>
                                </div>
                                <div class="mb-1 d-flex align-items-center text-alternate text-small lh-1-25">Dispositivos</div>
                                <div class="text-primary cta-4">
                                    <asp:Label ID="lblDispositivos" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--<div class="col-6 col-md-4 col-lg-2">
                        <div class="card h-100 hover-scale-up cursor-pointer">
                            <div class="card-body d-flex flex-column align-items-center">
                                <div class="sw-6 sh-6 rounded-xl d-flex justify-content-center align-items-center border border-primary mb-4">
                                    <i data-acorn-icon="message" class="text-primary"></i>
                                </div>
                                <div class="mb-1 d-flex align-items-center text-alternate text-small lh-1-25">Suportes</div>
                                <div class="text-primary cta-4">
                                    <asp:Label ID="lblSuportes" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>--%>
                </div>
            </div>
        </div>
    </div>
    <!-- Stats End -->

    <div class="row">
        <!-- Recent Orders Start -->
        <div class="col-xl-6 mb-5">
            <h2 class="small-title">Mensalidades Recentes</h2>
            <div class="mb-n2 scroll-out">
                
            </div>
        </div>
        <!-- Recent Orders End -->

        <!-- Performance Start -->
        <div class="col-xl-6 mb-5">
            <div class="d-flex">
                <div class="dropdown-as-select me-3" data-setactive="false" data-childselector="span">
                    <a class="pe-0 pt-0 align-top lh-1 dropdown-toggle" href="#" data-bs-toggle="dropdown" aria-expanded="false" aria-haspopup="true">
                        <span class="small-title"></span>
                    </a>
                    <div class="dropdown-menu font-standard">
                        <div class="nav flex-column" role="tablist">
                            <a class="active dropdown-item text-medium" href="#" aria-selected="true" role="tab">Hoje's</a>
                            <a class="dropdown-item text-medium" href="#" aria-selected="false" role="tab">Semana</a>
                            <a class="dropdown-item text-medium" href="#" aria-selected="false" role="tab">Mês</a>
                            <a class="dropdown-item text-medium" href="#" aria-selected="false" role="tab">Ano</a>
                        </div>
                    </div>
                </div>
                <h2 class="small-title">Acessos</h2>
            </div>
            <div class="card sh-45 h-xl-100-card">
                <div class="card-body h-100">
                    <div class="h-100">
                        
                    </div>
                </div>
            </div>
        </div>
        <!-- Performance End -->
    </div>

    <div class="row gx-4 gy-5">
        <!-- Top Selling Items Start -->
        <div class="col-xl-6 mb-5">
            <h2 class="small-title">Quantidade de Acessos</h2>
            <div class="scroll-out mb-n2">
                
            </div>
        </div>
        <!-- Top Selling Items End -->

        <!-- Top Search Terms Start -->
        <div class="col-xl-6 mb-5">
            <h2 class="small-title">Quantidade de Dispositivos</h2>
            <div class="card sh-35 h-xl-100-card">
                <div class="card-body scroll-out h-100">
                    
                </div>
            </div>
        </div>
        <!-- Top Search Terms End -->
    </div>    
</asp:Content>
