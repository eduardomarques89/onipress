<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/second.Master" AutoEventWireup="true" CodeBehind="cadastrar.aspx.cs" Inherits="global.cadastrar" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="account-area">
    <div class="container">
        <div class="row">
            <div class="col-12">
                <div class="register-form-content">
                    <div class="row">
                        <h3>Cadastre-se</h3>
                        <div class="col-12">
                            <div class="form-group">
                                <asp:TextBox ID="txtName" runat="server" class="form-control" placeholder="Nome completo" Required></asp:TextBox>
                                <asp:Label ID="lblNome" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-12">
                            <div class="form-group">
                                <asp:TextBox ID="txtCpf" runat="server" class="form-control" placeholder="CPF" Required></asp:TextBox>
                                <asp:Label ID="lblCPF" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-12">
                            <div class="form-group">
                                <asp:TextBox ID="txtTell" runat="server" class="form-control" placeholder="Telefone" Required></asp:TextBox>
                                <asp:Label ID="lblTell" runat="server" Text=""></asp:Label>
                            </div>
                        </div>

                        <div class="col-12">
                            <div class="form-group mb--0">
                                <asp:LinkButton ID="EnviarDados" class="btn-register" runat="server" OnClick="EnviarDados_Click">
                                    <span>Avançar</span>
                                    <img src="img/icon-send.svg" alt="Enviar" />
                                </asp:LinkButton>
                            </div>
                        </div>
                        <br />
                        <asp:Label ID="lblReposta" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblErro" runat="server" Text=""></asp:Label>

                        <asp:Label ID="lblTeste" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </section>
</asp:Content>
