<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/geral.Master" AutoEventWireup="true" CodeBehind="homePage.aspx.cs" Inherits="global.homePage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
    @font-face {
      font-family: 'CS-Interface';
      src: url('src/font/fonts/CS-Interface.ttf') format('truetype');
      font-weight: normal;
      font-style: normal;
    }

    * {
      font-family: 'CS-Interface', sans-serif;
      margin: 0;
      padding: 0;
    }

    body {
      background-color: #002333;
      display: flex;
      justify-content: center;
      align-items: center;
      margin: 0;
      padding: 0;
    }

    .container {
      background-color: #F6F6F6;
      border-radius: 15px;
      margin-left: -150px;
    }

    .welcome {
      font-size: 35px;
      font-weight: 400;
      color: #0176AB;
      margin-left: 100px;
    }

    .title {
      font-size: 60px;
      font-weight: 400;
      margin-left: 150px;
    }

    .line {
      margin-top: 90px;
      display: flex;
      justify-content: space-around;
      align-items: center;
    }

    .choice-box {
      display: flex;
      flex-direction: column;
      align-items: center;
    }

    .choice-morador,
    .choice-visitante,
    .choice-work {
      background-color: #0176AB;
      border-radius: 15px;
      border: none;
      width: 150px;
      height: 150px;
      cursor: pointer;
      display: flex;
      justify-content: center;
      align-items: center;
      transition: transform 0.3s ease, background-color 0.3s ease;
    }

    .choice-morador:hover,
    .choice-visitante:hover,
    .choice-work:hover {
      background-color: #015a84;
      transform: scale(1.1);
    }

    .line p {
      font-size: 25px;
      font-weight: 300;
      text-align: center;
      color: #333;
      margin-top: 10px;
      max-width: 200px;
      word-wrap: break-word;
    }

    .choice-morador img,
    .choice-visitante img,
    .choice-work img {
      width: 80px;
      height: 80px;
    }

    .faq {
      display: flex;
      flex-direction: column;
      align-items: center;
      margin-top: 25px;
    }

    .faq_button {
      background-color: #0176AB;
      border-radius: 15px;
      border: none;
      width: 60px;
      height: 60px;
      cursor: pointer;
      display: flex;
      justify-content: center;
      align-items: center;
      transition: transform 0.3s ease, background-color 0.3s ease;
      margin-right: 50px;
    }

    .faq_button:hover {
      background-color: #015a84;
      transform: scale(1.1);
    }

    .faq p {
      font-size: 20px;
      font-weight: 300;
      text-align: center;
      color: #333;
      margin-top: 3px;
      margin-right: 50px;
    }
  </style>

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
