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

        .button {
            background-color: #0176AB;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            display: inline-block;
            text-align: center;
            margin-right: 10px;
        }

        .button-send {
            border: 2px solid #0176AB; 
            color: black;
            padding: 10px 20px;
            border-radius: 5px;
            cursor: pointer;
            display: inline-block;
            text-align: center;
            margin-right: 10px;
        }

        #fileUpload {
            display: none;
        }

        #file-name {
            font-size: 14px;
            font-weight: 100;
            color: #000;
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

        @media (max-width: 700px) {
    body {
        height: auto;
        padding: 20px;
        justify-content: flex-start;
    }

    .container-box {
        flex-direction: column;
        width: 350px;
        height: auto;
        margin-right: 0;
        margin-left: -200px;
        padding: 20px;
        box-sizing: border-box;
    }

    .options {
        flex-direction: column;
        gap: 20px;
    }

    .option {
        width: 100%;
    }

    .title-txt {
        font-size: 1.5rem;
        text-align: center;
    }

    .button-send, .button {
        margin-left: 0;
        width: 100%;
        text-align: center;
    }

    #file-name {
        text-align: center;
        display: block;
    }
}


    </style>

    <div class="container-box"> 
        <div class="content-register">
            <div class="content-time">
                <h1 class="title-txt">Escolha a melhor opção para sua identificação</h1>
                <div class="options">
                    <div class="option">
                        <p>Envie uma foto e registre-se através de seu rosto</p>
                        <div class="custom-file-upload" style="margin-bottom: 10px;">
                            <label for="fileUpload1" class="button-send" style="margin-left: 20px; margin-top: 50px;">
                                Escolher arquivo
                            </label>
                            <asp:FileUpload ID="fileUpload1" runat="server" style="display: none;" />
                        </div>
                        <span id="file-name">Nenhum arquivo escolhido</span>
                        <asp:FileUpload ID="fileUpload2" runat="server" style="display: none;" />
                        <asp:LinkButton ID="EnviarFotos" CssClass="button" runat="server" OnClick="EnviarFotos_Click">
                            <span>Enviar Foto</span>
                        </asp:LinkButton>
                    </div>
                    <div class="option">
                        <p>Ou se preferir, leia o QrCode abaixo:</p>
                        <asp:LinkButton ID="GerarQrCode" CssClass="button" runat="server" OnClick="GerarQrCode_Click">
                            <span>Gerar QrCode</span>
                        </asp:LinkButton>
                        <asp:Image ID="imgQrCode" runat="server" Style="margin-top: 50px;" />
                    </div>
                </div>
                <asp:Label ID="lblReposta" runat="server" CssClass="response-message"></asp:Label>
                <asp:Label ID="lblErro" runat="server" CssClass="error-message"></asp:Label>
            </div>
        </div>
    </div>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script>
        document.querySelector('.button-send').addEventListener('click', function () {
            document.getElementById('<%= fileUpload1.ClientID %>').click();
        });

        document.getElementById('<%= fileUpload1.ClientID %>').addEventListener('change', function () {
            var fileName = this.files[0] ? this.files[0].name : 'Nenhum arquivo escolhido';
            document.getElementById('file-name').textContent = fileName;
        });
    </script>
</asp:Content>
