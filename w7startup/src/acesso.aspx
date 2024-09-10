<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/geral.Master" AutoEventWireup="true" CodeBehind="acesso.aspx.cs" Inherits="global.acesso" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        @font-face {
            font-family: 'CS-Interface';
            src: url('src/font/fonts/CS-Interface.ttf') format('truetype'); 
            font-weight: normal;
            font-style: normal;
        }

        body {
            background-color: #002333;
            height: 100%;
            width: 100%;
            font-family: 'CS-Interface', sans-serif;
        }

        .container {
            position: absolute;
            top: 50%;
            left: 57%;
            transform: translate(-50%, -50%);
        }

        .box-bg {
            background: #F6F6F6;
            width: 1000px;
            height: 644px;
            border-radius: 14px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            box-shadow: 0 10px 20px rgba(0, 0, 0, 0.2);
            border: 1px solid #dddddd;
        }

        .content-register {
            flex: 1;
            display: flex;
            flex-direction: column;
            justify-content: center;
        }

        .content-time {
            margin-top: 37px;
        }

        .content-locantion {
            margin-top: 30px;
            flex-direction: column;
            display: flex;
            flex-direction: column; 
            gap: 10px;
            max-width: 300px;
        }

        .time {
            font-size: 1.75rem;            
            margin-left: 37px;
            margin-right: auto;
            font-family: 'CS-Interface', sans-serif;
        }

        .localization {
            font-size: 1.75rem;
            margin-left: 36px;
            font-family: 'CS-Interface', sans-serif;
        }

        .horizontal-container {
            display: flex;  
            align-items: center; 
            gap: 20px; 
            margin-left: 54px;
        }

        .input-dateinitial,
        .input-hourinitial,
        .input-datefinal,
        .input-hourfinal{
            width: 197px;
            height: 60px;
            margin-top: 23px;
            border-radius: 15px;
            border: 2px solid rgba(0, 0, 0, 0.2);
            padding: 0 20px;
            box-sizing: border-box;
            transition: border-color 0.3s;
            margin-left: 15px
        }

        .input-companies, .input-block, .input-unity {
            width: 300px;
            height: 60px;
            border-radius: 15px;            
            margin-left: 54px;
            border: 2px solid rgba(0, 0, 0, 0.2);
            padding: 0 20px;
            box-sizing: border-box;
            transition: border-color 0.3s;
        }

        .separator {
            width: 5px;
            height: 40px;
            margin-top: 30px;
            opacity: 30%;
        }

        .input-dateinitial:hover, .input-hourinitial:hover, .input-companies:hover {
            border-color: #0176AB;  
        }

        .input-dateinitial:focus, .input-hourinitial:focus, .input-companies:focus {
            border-color: #0176AB; 
            outline: none;
        }

        .content-register input::placeholder {
            font-size: 1.125rem;
            opacity: 40%;
        }

        .localization {
            margin: auto;
        }

        .button-advance {
            width: 150px;
            height: 50px;
            margin-left: 500px;
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

    </style>

    <div class="container">
        <div class="box-bg">
            <div class="divisor-side">
                <img src="img/photo_camera.svg" />
            </div>
            <div class="content-register">
                <div class="content-time">
                    <h3 class="time">Tempo de Acesso</h3>
                    <div class="horizontal-container">
                        <asp:TextBox ID="txtDataInicial" runat="server" class="input-dateinitial" placeholder="Data Inicial" Required></asp:TextBox>
                        <asp:Label ID="lblDataInitial" runat="server" Text=""></asp:Label>
    
                        <h1 class="separator">-</h1>

                        <asp:TextBox ID="txtHourInicial" runat="server" class="input-hourinitial" placeholder="Hora Inicial" Required></asp:TextBox>
                        <asp:Label ID="lblHourInitial" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="horizontal-container">
                        <asp:TextBox ID="txtDataFinal" runat="server" class="input-datefinal" placeholder="Data Final" Required></asp:TextBox>
                        <asp:Label ID="lblDateFinal" runat="server" Text=""></asp:Label>
    
                        <h1 class="separator">-</h1>

                        <asp:TextBox ID="txtHourFinal" runat="server" class="input-hourfinal" placeholder="Hora Final" Required></asp:TextBox>
                        <asp:Label ID="lblHourFinal" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="content-locantion">
                    <h3 class="localization">Locais de Acesso</h3>

                    <asp:TextBox ID="txtCompanies" runat="server" class="input-companies" placeholder="Empresa/Condomínio" Required></asp:TextBox>
                    <asp:Label ID="lblCompanies" runat="server" Text=""></asp:Label>

                    <asp:TextBox ID="txtBlock" runat="server" class="input-block" placeholder="Bloco" Required></asp:TextBox>
                    <asp:Label ID="lblBlock" runat="server" Text=""></asp:Label>

                    <asp:TextBox ID="txtUnity" runat="server" class="input-unity" placeholder="Unidade" Required></asp:TextBox>
                    <asp:Label ID="lblUnity" runat="server" Text=""></asp:Label>
                </div>
                <asp:LinkButton ID="EnviarDados" class="button-advance" runat="server" OnClick="EnviarDados_Click">
                    <span>Avançar</span>
                    <img src="img/icon-send.svg" alt="Enviar" />
                </asp:LinkButton>

                <br />
                <asp:Label ID="lblReposta" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblErro" runat="server" Text=""></asp:Label>

                <asp:Label ID="lblTeste" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
