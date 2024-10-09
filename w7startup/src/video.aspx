<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/geral.Master" AutoEventWireup="true" CodeBehind="video.aspx.cs" Inherits="global.video" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .video {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            margin: 0;
            padding: 0;
            z-index: -1;
            overflow: hidden;
        }
        .video video {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }
    </style>

    <div class="video">
        <video autoplay muted loop>
            <source src="img/video.mp4" type="video/mp4">
            Your browser does not support the video tag.
        </video>
    </div>
</asp:Content>
