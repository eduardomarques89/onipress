<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/second.Master" AutoEventWireup="true" CodeBehind="acesso.aspx.cs" Inherits="global.acessos" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="account-area">
        <div class="container">
            <div class="row">
                <div class="col-12">
                    <div class="register-form-content">
                        <div class="row">
                            <div class="content-register">
                                <div class="content-time">
                                    <h3 class="time">Tempo de Acesso</h3>
                                        <div class="col-12">
                                            <div class="form-group">
                                                <asp:Label ID="lblPlaceHolderDataInitial" CssClass="label-input" runat="server" Text="Data Inicial"></asp:Label>
                                                <asp:TextBox ID="txtDataInicial" runat="server" TextMode="Date" CssClass="form-control" aria-describedby="lblPlaceHolderDataInitial" Required></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-12">
                                            <div class="form-group">
                                                <asp:Label ID="lblPlaceholderHourInitial" CssClass="label-input" runat="server" Text="Hora Inicial"></asp:Label>
                                                <asp:TextBox ID="txtHourInicial" runat="server" TextMode="Time" CssClass="form-control" aria-describedby="lblPlaceholderHourInitial" Required></asp:TextBox>
                                            </div>
                                        </div>


                                    <div class="col-12">
                                        <div class="form-group">
                                            <asp:Label ID="lblPlaceHolderDataFinal" CssClass="label-input" runat="server" Text="Data Final"></asp:Label>
                                            <asp:TextBox ID="txtDataFinal" runat="server" TextMode="Date" CssClass="form-control" aria-describedby="lblPlaceHolderDataFinal" Required></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-12">
                                        <div class="form-group">
                                            <asp:Label ID="lblPlaceHolderHourFinal" CssClass="label-input" runat="server" Text="Hora Final"></asp:Label>
                                            <asp:TextBox ID="txtHourFinal" runat="server" TextMode="Time" CssClass="form-control" aria-describedby="lblPlaceHolderHourFinal" Required></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-12">
                                <div class="form-group mb--0">
                                    <asp:LinkButton ID="EnviarDados" CssClass="btn-register" runat="server" OnClick="EnviarDados_Click">
                                        <span>Avançar</span>
                                        <img src="img/icon-send.svg" alt="Enviar" />
                                    </asp:LinkButton>
                                </div>
                            </div>

                                <asp:Label ID="lblReposta" runat="server" CssClass="response-message"></asp:Label>
                                <asp:Label ID="lblErro" runat="server" CssClass="error-message"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    </section>
</asp:Content>
