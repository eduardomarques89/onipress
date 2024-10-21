<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/second.Master" AutoEventWireup="true" CodeBehind="code.aspx.cs" Inherits="global.code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="account-area">
        <div class="container">
            <div class="row">
                <div class="col-12">
                    <div class="register-form-content">
                        <div class="row">

                            <div class="col-12" style="text-align: center">
                                <div class="form-group mb--0">
                                    <h2>LEIA O QRCODE ABAIXO</h2>
                                    <asp:Image ID="imgQrCode" runat="server" style="height: 400px; padding-block: 20px"/>
                                </div>
                            </div>

                            <div class="col-12">
                                <div class="form-group mb--0">
                                    <asp:LinkButton ID="VoltarHome" CssClass="btn-register" runat="server" OnClick="VoltarHome_Click">
                                    <img src="img/icon-home.svg" alt="">
                                    <span>Volta para Tela Inicial</span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

</asp:Content>
