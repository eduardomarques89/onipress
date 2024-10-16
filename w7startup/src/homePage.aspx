<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/second.Master" AutoEventWireup="true" CodeBehind="homePage.aspx.cs" Inherits="global.homePage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="account-area">
        <div class="container">
            <div class="row">
                <div class="col-12">
                    <div class="register-form-content">
                        <h4 class="welcome" style="text-align: center">Bem vindo,</h4>
                        <h2 class="title" style="text-align: center">ESCOLHA UMA OPÇÃO</h2>
                        <div class="row" style="margin-top: 40px">
                            <div class="col-4">
                                <div class="form-group">
                                    <asp:LinkButton ID="morador_click" CssClass="btn-register" style="border-radius: 10px" runat="server" OnClick="Morador_Click">
                                  <img src="img/icon-person.svg" alt="person" style="width:50px">
                                    </asp:LinkButton>
                                    <p style="text-align: center">MORADOR</p>
                                </div>
                            </div>
                            <div class="col-4">
                                <div class="form-group">
                                    <asp:LinkButton ID="visitante_click" CssClass="btn-register" style="border-radius: 10px" runat="server" OnClick="Visitante_Click">
                                    <img src="img/icon-car.svg" alt="car" style="width:50px">
                                    </asp:LinkButton>
                                    <p style="text-align: center">VISITANTE</p>
                                </div>
                            </div>
                            <div class="col-4">
                                <div class="form-group">
                                    <asp:LinkButton ID="ps_click" CssClass="btn-register" style="border-radius: 10px" runat="server" OnClick="PS_Click">
                                    <img src="img/icon-work.svg" alt="work" style="width:50px">
                                    </asp:LinkButton>
                                    <p style="text-align: center">PRESTADOR DE SERVIÇOS</p>
                                </div>
                            </div>
                        </div>
                        <div class="col-2">
                            <div class="form-group">
                            <asp:LinkButton ID="faq_click" CssClass="btn-register" style="border-radius: 10px" runat="server" OnClick="faq_Click">
                                  <img src="img/icon-faq.svg" alt="FAQ">
                            </asp:LinkButton>
                            <p style="text-align: center">FAQ</p>
                        </div>
                    </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

</asp:Content>
