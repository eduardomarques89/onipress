<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/geral.Master" AutoEventWireup="true" CodeBehind="homePage.aspx.cs" Inherits="global.homePage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container" style="width: 1026px; ">
        <h4 class="welcome" style="padding-top: 20px;">Bem vindo,</h4>
        <h1 class="title">ESCOLHA UMA OPÇÃO</h1>

        <div class="line">
        <div class="choice-box">
            <asp:LinkButton ID="morador_click" CssClass="choice-morador" runat="server" OnClick="Morador_Click">
              <img src="img/icon-person.svg" alt="person">
            </asp:LinkButton>
          <p>MORADOR</p>
        </div>
        <div class="choice-box">
            <asp:LinkButton ID="visitante_click" CssClass="choice-visitante" runat="server" OnClick="Visitante_Click">
                <img src="img/icon-car.svg" alt="car">
            </asp:LinkButton>
          <p>VISITANTE</p>
        </div>
        <div class="choice-box" style="margin-top: 30px;">
            <asp:LinkButton ID="ps_click" CssClass="choice-work" runat="server" OnClick="PS_Click">
                <img src="img/icon-work.svg" alt="work">
            </asp:LinkButton>
          <p>PRESTADOR DE SERVIÇOS</p>
        </div>
      </div>
      <div class="faq">
          <asp:LinkButton ID="faq_click" CssClass="faq_button" runat="server" OnClick="faq_Click">
              <img src="img/icon-faq.svg" alt="FAQ">
          </asp:LinkButton>
        <p>FAQ</p>
      </div>
    </div>
</asp:Content>
