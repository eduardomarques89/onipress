<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/principal.Master" AutoEventWireup="true" CodeBehind="precadastro.aspx.cs" Inherits="global.precadastro" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField ID="hdfId" runat="server" />
    <script src="js/mascara.js"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <!-- Title and Top Buttons Start -->
    <div class="page-title-container">
        <div class="row g-0">
            <div class="col-auto mb-3 mb-md-0 me-auto">
                <div class="w-auto sw-md-30">
                    <a href="dashboard.aspx" class="muted-link pb-1 d-inline-block breadcrumb-back">
                        <i data-acorn-icon="chevron-left" data-acorn-size="13"></i>
                        <span class="text-small align-middle">Administrador</span>
                    </a>
                    <h1 class="mb-0 pb-0 display-4" id="title">Pré-Cadastro</h1>
                    <asp:Label ID="lblResposta" runat="server" Text=""></asp:Label>
                </div>
            </div>

            <%-- Botão --%>
            <div class="w-100 d-md-none"></div>
                <div class="col-12 col-sm-6 col-md-auto d-flex align-items-end justify-content-end mb-2 mb-sm-0 order-sm-3">
                    <button
                        type="button"
                        class="btn btn-outline-primary btn-icon btn-icon-start ms-0 ms-sm-1 w-100 w-md-auto"
                        data-bs-toggle="modal"
                        data-bs-target='<%= "#" + pnlModal.ClientID %>'>
                        <i data-acorn-icon="plus"></i>
                        <span>Realizar Pré-Cadastro</span>
                    </button>
                    <div class="dropdown d-inline-block d-xl-none">
                    </div>
                </div>
            </div>
        </div>



    <%-- Filtros --%>
    <div class="row mb-2">
        <!-- Search Start -->
        <div class="col-sm-12 col-md-5 col-lg-3 col-xxl-2 mb-1">
            <div class="d-flex">
                <!-- TextBox Container -->
                <div class="d-inline-block flex-grow-1 me-1 search-input-container shadow bg-foreground">
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Digite aqui para filtrar" style="width: 200px"></asp:TextBox>
                    <span class="search-magnifier-icon">
                        <i data-acorn-icon="search"></i>
                    </span>
                    <span class="search-delete-icon d-none">
                        <i data-acorn-icon="close"></i>
                    </span>
                </div>
                <!-- Button Container -->
                <div>
                    <asp:LinkButton ID="filtroClick" runat="server"
                        CssClass="btn btn-outline-primary btn-icon btn-icon-start" OnClick="lkbFiltro_Click">
                        <i data-acorn-icon="search"></i> Filtrar
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    
    <asp:GridView ID="gdvDados" Width="100%" runat="server" CellPadding="4" EmptyDataText="Não há dados para visualizar" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" DataSourceID="sdsDados" OnRowCommand="gdvDados_RowCommand"><AlternatingRowStyle />
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton 
                        runat="server" 
                        CommandArgument='<%# Eval("id") %>' 
                        CommandName="Excluir" 
                        ID="excluirRegistro" 
                        CssClass="btn btn-icon btn-icon-start btn-danger">
                        EXCLUIR
                    </asp:LinkButton>

                    <asp:LinkButton 
                        runat="server" 
                        CommandArgument='<%# Eval("id") %>' 
                        CommandName="Editar" 
                        ID="editarRegistro" 
                        CssClass="btn btn-icon btn-icon-start btn-primary">
                        EDITAR
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="telefone" HeaderText="Telefone" SortExpression="telefone" />
            <asp:BoundField DataField="tipopessoa" HeaderText="Tipo de Acesso" SortExpression="tipopessoa" />
        </Columns>
        <EditRowStyle BackColor="#7C6F57" />
        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <HeaderStyle />
        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle Height="4em" BackColor="White" ForeColor="#a59e9e" CssClass="fix-margin" />
        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F8FAFA" />
        <SortedAscendingHeaderStyle BackColor="#246B61" />
        <SortedDescendingCellStyle BackColor="#D4DFE1" />
        <SortedDescendingHeaderStyle BackColor="#15524A" />
    </asp:GridView>
    <asp:SqlDataSource ID="sdsDados" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select pc.id as id, pc.telefone as telefone, pc.tipopessoa as tipopessoa from OniPres_precadastro pc join OniPres_tipoPessoa pp on pp.id = pc.tipopessoa">
    </asp:SqlDataSource>


    <!-- Modal -->
    <asp:Panel ID="pnlModal" runat="server" CssClass="modal modal-right fade" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Pré-Cadastro</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label class="form-label">Telefone WhatsApp</label>
                        <asp:TextBox ID="txtTelefone" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>       
                    <div class="mb-3">
                        <label class="form-label">Tipo de Acesso</label>
                        <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control shadow dropdown-menu-end" DataSourceID="sdsTipo" DataTextField="nome" DataValueField="id"></asp:DropDownList>
                        <asp:SqlDataSource ID="sdsTipo" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand=
                            "select id, nome from OniPres_tipoPessoa"></asp:SqlDataSource>
                    </div>
                </div>

                <div class="modal-footer border-0">
                    <asp:Label ID="lblMensagem" runat="server" Text=""></asp:Label>
                    <br />
                    <asp:Button ID="btnSalvar" CssClass="btn btn-icon btn-icon-end btn-primary" runat="server" OnClick="btnSalvar_Click" Text="Adicionar"></asp:Button>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
