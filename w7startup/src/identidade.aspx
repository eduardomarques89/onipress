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
            max-width: 800px;
            height: 500px;
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

        .options {
            display: flex;
            justify-content: space-between;
            gap: 50px;
        }

        .option {
            width: 45%;
            padding: 20px;
            border-radius: 10px;
            display: flex;
            flex-direction: column;
            align-items: center;
            font-weight: 500;
        }

        .photo-box {
            background-color: #e0e0e0;
            width: 150px;
            height: 150px;
            margin-bottom: 10px;
            display: flex;
            justify-content: center;
            align-items: center;
            border-radius: 5px;
        }

        .img {
            width: 150px;
            height: 150px;
            border-radius: 10px;
        }

        button {
            background-color: #0176AB;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 10px;
            cursor: pointer;
            font-family: 'CS-Interface', sans-serif;
        }

        button:hover {
            background-color: #005f7f;
        }

        p {
            font-size: 15px;
            color: #555;
            margin-bottom: 10px;
            text-align: center;
            width: 200px;
        }
    </style>

    <div class="container-box"> 
        <div class="content-register">
            <div class="content-time">
                <h1 class="title-txt">Escolha a melhor opção para sua identificação</h1>
                <div class="options">
                    <div class="option">
                        <p>Tire uma foto e registre-se através de seu rosto</p>
                        <div class="photo-box">
                            <img class="img" src="img/photos.svg" alt="Placeholder da Foto">
                        </div>
                        <button class="button">Registrar Foto</button>
                    </div>
                    <div class="option">
                        <p>Gere um QrCode e utilize ele no aparelho de leitura</p>
                        <button class="button" style="margin-top: 50px">Gerar QrCode</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
