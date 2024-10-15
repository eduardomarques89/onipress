<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/geral.Master" AutoEventWireup="true" CodeBehind="acessoView.aspx.cs" Inherits="global.acessoView" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        /*@font-face {
            font-family: 'CS-Interface';
            src: url('/font/CS-Interface/fonts/CS-Interface.ttf') format('truetype');
            font-weight: normal;
            font-style: normal;

        }*/

        * {
            /*font-family: 'CS-Interface', sans-serif;*/
        }

        body {
            background-color: #002333;
            margin: 0;
            padding: 0;
            height: 100vh;
            /*font-family: 'CS-Interface', sans-serif;*/
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .container-box {
            background: #F6F6F6;
            width: 100%;
            max-width: 1000px;
            height: 674px;
            border-radius: 14px;
            display: flex;
            flex-direction: row;
            justify-content: space-between;
            align-items: center;
            box-shadow: 0 10px 20px rgba(0, 0, 0, 0.2);
            box-sizing: border-box;
            margin-right: 250px;
        }

        .divisor-side {
            height: 674px;
        }

        .content-register {
            flex: 1;
            display: flex;
            flex-direction: column;
            justify-content: center;
            padding: 20px;
        }

        .content-time,
        .content-location {
            margin-top: 20px;
        }

        .time,
        .localization {
            font-size: 1.75rem;
            margin-bottom: 20px;
        }

        .horizontal-container,
        .vertical-container {
            display: flex;
            gap: 20px;
            flex-wrap: wrap;
        }

        .horizontal-container {
            margin-left: 54px;
        }

        .input-text {
            width: 197px;
            height: 60px;
            max-width: 300px;
            margin-top: 5px;
            border-radius: 15px;
            border: 2px solid rgba(0, 0, 0, 0.2);
            padding: 0 20px;
            box-sizing: border-box;
            transition: border-color 0.3s;
        }

        .input-text:hover,
        .input-text:focus {
            border-color: #0176AB;
            outline: none;
        }

        .input-companies,
        .input-block,
        .input-unity {
            width: 300px;
            height: 60px;
            border-radius: 15px;
            margin-left: 54px;
            border: 2px solid rgba(0, 0, 0, 0.2);
            padding: 0 20px;
            box-sizing: border-box;
            transition: border-color 0.3s;
        }

        .label-input {
            font-size: 1.125rem;
            opacity: 70%;
            margin-bottom: 5px;
            display: block;
        }

        .button-advance {
            width: 150px;
            height: 50px;
            margin-left: 70%;
            background-color: #0176AB;
            color: #FFFFFF;
            border: none;
            border-radius: 15px;
            padding: 9px 14px 13px 30px;
            font-size: 1.125rem;
            font-weight: 100;
        }

        .cs-send {
            padding-left: 10px;
        }

        @media (max-width: 700px) {
            .container-box {
                margin-top: 200px;
                margin-left: -100px;
                margin-right: auto;
                display: flex;
                flex-direction: column;
                justify-content: center;
                align-items: flex-start;
                height: auto;
                width: 100%;
                max-width: 100%;
                padding: 10px;
            }

            .divisor-side {
                display: none;
            }

            .content-register {
                width: 100%;
                padding: 20px;
                box-sizing: border-box;
                text-align: left;
            }

            .button-advance {
                margin-left: 0;
                width: 100%;
                margin-top: 20px;
                padding: 10px;
                text-align: center;
                box-sizing: border-box;
            }
        }


    </style>

    <div class="container-box">
        <div class="divisor-side">
            <img src="img/photo_camera.svg" alt="Camera" />
        </div>
        <div class="content-register">
            <div class="content-time">
                <h3 class="time">Tempo de Acesso</h3>
                <div class="horizontal-container">
                    <div>
                        <asp:Label ID="lblPlaceHolderDataInitial" CssClass="label-input" runat="server" Text="Data Inicial" ReadOnly="true"></asp:Label>
                        <asp:TextBox ID="txtDataInicial" runat="server" TextMode="Date" CssClass="input-text" aria-describedby="lblPlaceHolderDataInitial" ReadOnly="true"></asp:TextBox>
                    </div>

                    <div>
                        <asp:Label ID="lblPlaceholderHourInitial" CssClass="label-input" runat="server" Text="Hora Inicial" ReadOnly="true"></asp:Label>
                        <asp:TextBox ID="txtHourInicial" runat="server" TextMode="Time" CssClass="input-text" aria-describedby="lblPlaceholderHourInitial" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="horizontal-container">
                    <div>
                        <asp:Label ID="lblPlaceHolderDataFinal" CssClass="label-input" runat="server" Text="Data Final"></asp:Label>
                        <asp:TextBox ID="txtDataFinal" runat="server" TextMode="Date" CssClass="input-text" aria-describedby="lblPlaceHolderDataFinal" ReadOnly="true"></asp:TextBox>
                    </div>

                    <div>
                        <asp:Label ID="lblPlaceHolderHourFinal" CssClass="label-input" runat="server" Text="Hora Final" ReadOnly="true"></asp:Label>
                        <asp:TextBox ID="txtHourFinal" runat="server" TextMode="Time" CssClass="input-text" aria-describedby="lblPlaceHolderHourFinal" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
            

            <asp:LinkButton ID="AvançcarPagina" CssClass="button-advance" runat="server" OnClick="AvancarPagina_Click">
                <span>Avançar</span>
                <img src="img/icon-send.svg" alt="Enviar" />
            </asp:LinkButton>

            <asp:Label ID="lblReposta" runat="server" CssClass="response-message"></asp:Label>
            <asp:Label ID="lblErro" runat="server" CssClass="error-message"></asp:Label>
        </div>
    </div>
</asp:Content>
