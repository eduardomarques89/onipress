<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/geral.Master" AutoEventWireup="true" CodeBehind="cadastrar.aspx.cs" Inherits="global.cadastrar" %>

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
            align-items: center;
        }

        .content-register h3 {
            font-size: 1.75rem;
            margin: 0;
            font-family: 'CS-Interface', sans-serif;
        }

        .input-name, .input-cpf, .input-tell {
            width: 600px;
            height: 60px;
            border-radius: 15px;
            margin-top: 40px;
            border: 2px solid rgba(0, 0, 0, 0.2);
            padding: 0 20px;
            box-sizing: border-box;
            transition: border-color 0.3s;
        }

        .input-name {
            margin-top: 59px; 
        }

        .content-register input::placeholder {
            font-size: 1.125rem;
            opacity: 40%;
        }

        .input-name:hover, .input-cpf:hover, .input-tell:hover {
            border-color: #0176AB;
        }

        .input-name:focus, .input-cpf:focus, .input-tell:focus {
            border-color: #0176AB;
            outline: none;
        }

        .button-advance {
            margin-top: 59px;
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

        /* Media Query para telas menores que 768px */
        @media (max-width: 768px) {
            .box-bg {
                width: 70%;
                height: 600px;
                border-radius: 10px;
                box-shadow: none;
                align-items: center;
            }

            .divisor-side img {
               height: 600px;
               width: 170px;
               border-radius: 15px;
            }

            .content-register {
                align-items: center;
                padding: 10px;
            }

            .input-name, .input-cpf, .input-tell {
                width: 100%;
                height: 50px;
                margin-top: 20px;
            }

            .button-advance {
                width: 100%;
                padding: 10px;
                font-size: 1rem;
                margin-top: 20px;
            }
        }

        /* Media Query para telas menores que 480px (por exemplo, celulares menores) */
        @media (max-width: 500px) {
            .box-bg {
                width: 80%;
                height: 600px;
                border-radius: 10px;
                box-shadow: none;
                align-items: center;
            }

            .input-name, .input-cpf, .input-tell {
                height: 45px;
                font-size: 1rem;
            }

            .content-register input::placeholder {
                font-size: 0.875rem;
                opacity: 40%;
            }

            .button-advance {
                padding: 8px;
                font-size: 0.875rem;
            }
        }

        /* Media Query para telas menores que 480px (por exemplo, celulares menores) */
        @media (max-width: 500px) {
            .box-bg {
                width: 80%;
                height: 600px;
                border-radius: 10px;
                box-shadow: none;
                align-items: center;
            }

            .content-register h3 {
                font-size: 1.563rem;
                margin: 0;
                font-family: 'CS-Interface', sans-serif;
            }

            .input-name, .input-cpf, .input-tell {
                height: 45px;
                font-size: 1rem;
            }

            .content-register input::placeholder {
                font-size: 0.813rem;
                opacity: 40%;
            }

            .button-advance {
                padding: 8px;
                font-size: 0.875rem;
            }
        }

    </style>

    <div class="container">
        <div class="box-bg">
            <div class="divisor-side">
                <img src="img/photo_camera.svg" />
            </div>
            <div class="content-register">
                <h3>Cadastre-se</h3>
                <input class="input-name" type="text" placeholder="Nome completo" />
                <input class="input-cpf" type="text" placeholder="CPF"/>
                <input class="input-tell" type="text" placeholder="Telefone"/>
                <button class="button-advance">
                    Avançar
                    <img src="img/icon-send.svg" alt="Enviar" />
                </button>
            </div>
        </div>
    </div>
</asp:Content>
