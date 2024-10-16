<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/second.Master" AutoEventWireup="true" CodeBehind="identidade.aspx.cs" Inherits="global.identidade" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="account-area">
        <div class="container">
            <div class="row">
                <div class="col-12">
                    <div class="register-form-content">
                        <div class="row">

                            <%-- Dados de Cadastro --%>
                            <h3>Dados de Cadastro</h3>
                            <div class="col-12">
                                <div class="form-group">
                                    <asp:TextBox ID="txtName" runat="server" class="form-control" placeholder="Nome completo" ReadOnly="true"></asp:TextBox>
                                    <asp:Label ID="lblNome" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="col-12">
                                <div class="form-group">
                                    <asp:TextBox ID="txtCpf" runat="server" class="form-control" placeholder="CPF" ReadOnly="true"></asp:TextBox>
                                    <asp:Label ID="lblCPF" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="col-12">
                                <div class="form-group">
                                    <asp:TextBox ID="txtTell" runat="server" class="form-control" placeholder="Telefone" ReadOnly="true"></asp:TextBox>
                                    <asp:Label ID="lblTell" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="col-12">
                                <div class="form-group">
                                    <asp:Label ID="lblPlaceHolderDataInitial" CssClass="label-input" runat="server" Text="Data Inicial" ></asp:Label>
                                    <asp:TextBox ID="txtDataInicial" runat="server" TextMode="Date" CssClass="form-control" aria-describedby="lblPlaceHolderDataInitial" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-12">
                                <div class="form-group">
                                    <asp:Label ID="lblPlaceHolderDataFinal" CssClass="label-input" runat="server" Text="Data Final"></asp:Label>
                                    <asp:TextBox ID="txtDataFinal" runat="server" TextMode="Date" CssClass="form-control" aria-describedby="lblPlaceHolderDataFinal" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <%-- QrCode e Foto --%>
                            <h3 class="title-txt">Escolha a melhor opção para sua identificação</h3>
                                <div class="col-12">
                                    <div class="form-group">

                                        <p>Envie uma foto e registre-se através de seu rosto</p>
                                        <div class="col-12">
                                            <div class="form-group mb--0">
                                                <asp:FileUpload ID="fileUpload" runat="server" style= "margin-block: 10px"/>
                                            </div>
                                        </div>

                                        <div class="col-12">
                                            <div class="form-group mb--0">
                                                <asp:LinkButton ID="EnviarFotos" CssClass="btn-register" runat="server" OnClick="EnviarFotos_Click">
                                                    <span>Enviar Foto</span>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
        
                                    <p>Ou se preferir, leia o QrCode abaixo:</p>
                                    <asp:Image ID="imgQrCode" runat="server" />
                                </div>
                            </div>
                            <asp:Label ID="lblReposta" runat="server" CssClass="response-message"></asp:Label>
                            <asp:Label ID="lblErro" runat="server" CssClass="error-message"></asp:Label>                        
                        </div>
                    </div>
                </div>
            </div>
    </section>
</asp:Content>
