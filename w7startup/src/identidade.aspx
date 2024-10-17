<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/second.Master" AutoEventWireup="true" CodeBehind="identidade.aspx.cs" Inherits="global.identidade" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="account-area">
        <div class="container" style="text-align:center">
             <div class="row">
                <div class="col-12">                      

                            <%-- Dados de Cadastro --%>
                            <h3>Olá <asp:Label ID="txtName" CssClass="label-input" runat="server" Text=""></asp:Label></h3>
                            <%--<div class="col-12">
                                    <asp:Label ID="lblName" CssClass="label-input" runat="server" Text="Nome:"></asp:Label>
                                    
                                </div>
                            
                            <div class="col-12">
                                    <asp:Label ID="lblCPF" CssClass="label-input" runat="server" Text="CPF:"></asp:Label>
                                    <asp:Label ID="txtCpf" CssClass="label-input" runat="server" Text=""></asp:Label>
                              
                            </div>
                            <div class="col-12">
                                    <asp:Label ID="lblCelular" CssClass="label-input" runat="server" Text="Celular:"></asp:Label>
                                    <asp:Label ID="txtTell" CssClass="label-input" runat="server" Text=""></asp:Label>
                               
                            </div>
                            <div class="col-12">
                                    <asp:Label ID="lblPlaceHolderDataInitial" CssClass="label-input" runat="server" Text="Entrada:"></asp:Label>
                                    <asp:Label ID="txtDataInicial" CssClass="label-input" runat="server" Text=""></asp:Label>
                               
                            </div>

                            <div class="col-12">
                                    <asp:Label ID="lblPlaceHolderDataFinal" CssClass="label-input" runat="server" Text="Término:"></asp:Label>
                                    <asp:Label ID="txtDataFinal" CssClass="label-input" runat="server" Text=""></asp:Label>
                                
                            </div>--%>

                            <%-- QrCode e Foto --%><br /><br />
                            <h3 class="title-txt">Escolha sua opção de acesso</h3>
                                <div class="col-12" style="text-align:center">
                                        <h6>Aponte o QrCode abaixo:</h6>
                                        <asp:Image ID="imgQrCode" runat="server" />
                                  
                                </div>
                                <div class="col-12" style="text-align:center"><br /><br /><br />
                                        <h6>Ou envie uma foto e registre-se através de seu rosto</h6>
                                            <asp:FileUpload ID="fileUpload" runat="server" Style="margin-block: 10px" /><br />
                                        <asp:LinkButton ID="EnviarFotos" CssClass="btn btn-register" runat="server" OnClick="EnviarFotos_Click">
                                <span>Enviar Foto</span>
                            </asp:LinkButton><br /><br />
                                </div>
                                <div class="col-12">
                                    <asp:Label ID="lblReposta" runat="server" CssClass="response-message"></asp:Label>
                                    <asp:Label ID="lblErro" runat="server" CssClass="error-message"></asp:Label>
                                </div>
                            </div>
                    </div>
            </div>
    </section>
</asp:Content>
