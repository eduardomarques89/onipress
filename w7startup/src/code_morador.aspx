<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/geral.Master" AutoEventWireup="true" CodeBehind="code_morador.aspx.cs" Inherits="global.code_morador" %>

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
    }

    body {
      background-color: #002333;
      display: flex;
      justify-content: center;
      align-items: center;
    }

    .container {
      background-color: #F6F6F6;
      width: 1026px;
      height: 656px;
      border-radius: 15px;
      margin-top: 30px;
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: center;
    }

    .title h2 {
      font-size: 60px;
      font-weight: 200;
    }

    .code img {
      width: 400px;
      height: 400px;
      margin-top: 20px;
    }

    .return {
      margin-top: 20px;
      display: flex;
      align-items: initial;
    }

    .return a {
      background-color: #F6F6F6;
      border: none;
      display: flex;
      align-items: center;
      cursor: pointer;
      text-decoration: none;
      color: black;
    }

    .return p {
      font-size: 18px;
      margin-left: 10px;
      width: 100px;
    }
  </style>

    <div class="container" style="width: 1026px">
        <div class="title">
          <h2>LEIA O QRCODE ABAIXO</h2>
        </div>
        <div class="code">
          <img src="img/qrcode_morador.png" alt="">
        </div>

        <div class="return">
          <a href="homePage.aspx">
            <img src="img/icon-home.svg" alt="">
            <p>Volta para Tela Inicial</p>
          </a>
        </div>
    </div>

</asp:Content>
