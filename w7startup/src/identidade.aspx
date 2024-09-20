<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/geral.Master" AutoEventWireup="true" CodeBehind="identidade.aspx.cs" Inherits="global.identidade" %>

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
        }

        body {
            background-color: #002333;
            margin: 0;
            padding: 0;
            height: 100vh;
            font-family: 'CS-Interface', sans-serif;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .container-box {
            background: #F6F6F6;
            width: 1000px;
            max-width: 1000px;
            height: 674px;
            border-radius: 14px;
            display: flex;
            flex-direction: row;
            justify-content: center;
            align-items: center;
            box-shadow: 0 10px 20px rgba(0, 0, 0, 0.2);
            box-sizing: border-box;
            margin-right: 250px;
        }

        .title-txt {
            font-size: 1.75rem;
            /*margin-bottom: 85%;*/
        }

        .txt-photo {
            font-size: 1.125rem;
            width: 250px;
            text-align: center;
        }

        .divisor {
            transform: rotate(90deg);
            width: 450px
        }

        .txt-qrcode {
            font-size: 1.125rem;
            width: 250px;
            text-align: center;
        }
    </style>

    <div class="container-box"> 
        <div class="content-register">
            <div class="content-time">
                <h1 class="title-txt">Escolha a melhor opção para sua identificação</h1>
                <div class="identi-photo">
                    <p class="txt-photo">Tire uma foto e registre-se através de seu rosto</p>
                </div>
                <div class="divisor">
                    <hr />
                </div>
                <div class="identi-qrcode">
                    <p class="txt-qrcode">Gere um QrCode e utilize ele no aparelho de leitura</p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
