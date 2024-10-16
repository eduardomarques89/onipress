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
                                    <asp:Label ID="lblName" CssClass="label-input" runat="server" Text="Nome"></asp:Label>
                                    <asp:TextBox ID="txtName" runat="server" class="form-control" placeholder="Nome completo" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="form-group">
                                    <asp:Label ID="lblCPF" CssClass="label-input" runat="server" Text="CPF"></asp:Label>
                                    <asp:TextBox ID="txtCpf" runat="server" class="form-control" placeholder="CPF" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="form-group">
                                    <asp:Label ID="lblCelular" CssClass="label-input" runat="server" Text="Celular"></asp:Label>
                                    <asp:TextBox ID="txtTell" runat="server" class="form-control" placeholder="Telefone" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="form-group">
                                    <asp:Label ID="lblPlaceHolderDataInitial" CssClass="label-input" runat="server" Text="Data Inicial"></asp:Label>
                                    <asp:TextBox ID="txtDataInicial" runat="server" TextMode="Date" CssClass="form-control" aria-describedby="lblPlaceHolderDataInitial" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-6">
                                <div class="form-group">
                                    <asp:Label ID="lblPlaceHolderDataFinal" CssClass="label-input" runat="server" Text="Data Final"></asp:Label>
                                    <asp:TextBox ID="txtDataFinal" runat="server" TextMode="Date" CssClass="form-control" aria-describedby="lblPlaceHolderDataFinal" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <%-- QrCode e Foto --%>
                            <h3 class="title-txt">Escolha a melhor opção para sua identificação</h3>
                            <div class="container" style="padding: 0 10px 0 30px">
                                <div class="form-group">


                                    <div class="qrcode-container">
                                        <p>Aponte o QrCode abaixo:</p>
                                        <asp:Image ID="imgQrCode" runat="server" style="margin-left: 25%"/>
                                    </div>

                                    <div class="foto-container">
                                        <p>Ou envie uma foto e registre-se através de seu rosto</p>
                                        <div class="form-group mb--0">
                                            <asp:FileUpload ID="fileUpload" runat="server" Style="margin-block: 10px" />
                                        </div>
                                        <asp:LinkButton ID="EnviarFotos" CssClass="btn-register" runat="server" OnClick="EnviarFotos_Click">
                                <span>Enviar Foto</span>
                            </asp:LinkButton>
                                    </div>
                                    <asp:Label ID="lblReposta" runat="server" CssClass="response-message"></asp:Label>
                                    <asp:Label ID="lblErro" runat="server" CssClass="error-message"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
