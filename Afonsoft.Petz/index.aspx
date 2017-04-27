<%@ Page Title="" Language="C#" MasterPageFile="~/Petz.Master" AutoEventWireup="true" Async="true" CodeBehind="index.aspx.cs" Inherits="Afonsoft.Petz.index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <style>
        .back-to-top {
            cursor: pointer;
            position: fixed;
            bottom: 20px;
            right: 20px;
            display: none;
        }

        .jumbotron {
            background: url("/Images/slider01_background.jpg") no-repeat center center;
            -webkit-background-size: 100% 100%;
            -moz-background-size: 100% 100%;
            -o-background-size: 100% 100%;
            background-size: 100% 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHeadMain" runat="server">

    <!-- Fixed navbar -->
    <nav class="navbar navbar-inverse navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="index.aspx">Pet System</a>
            </div>
            <div id="navbar" class="navbar-collapse collapse">
                <ul class="nav navbar-nav">
                    <li><a href="#up">Inicio</a></li>
                    <li><a href="#about">Sobre</a></li>
                    <li><a href="#cellphone">Celular</a></li>
                    <li><a href="#suport">Suporte</a></li>
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Desenvolvimento <span class="caret"></span></a>
                        <ul class="dropdown-menu">
                            <li><a href="#">API / WebService</a></li>
                            <li><a href="#">Documentação</a></li>
                        </ul>
                    </li>
                    <li><a href="#contact">Contato</a></li>
                </ul>
                <ul class="nav navbar-nav navbar-right">
                    <li><a href="login.aspx?op=cadastro">Inscreva-se</a></li>
                    <li class="active"><a href="login.aspx?op=login" role="button">Entrar</a></li>
                </ul>
            </div>
            <!--/.nav-collapse -->
        </div>
    </nav>

    <!-- Page Content -->
    <div class="container" id="up">
        <!-- Jumbotron Header -->
        <header class="jumbotron hero-spacer">
            <h1>Pet System</h1>
            <p>bla bla bla bla bla</p>
            <p>bla bla bla bla bla</p>
            <p>bla bla bla bla bla</p>
            <p>bla bla bla bla bla</p>
        </header>
        <hr>
        <!-- Title -->
        <div class="row">
            <div class="col-md-12">
                <h3>Latest Features</h3>
            </div>
        </div>
        <!-- /.row -->
        <!-- Page Features -->
        <div class="row">
            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">
                    <img src="Images/prod.jpg" alt="Produtividade">
                    <div class="caption">
                        <h3>Simples API</h3>
                        <p>
                            <span class="glyphicon glyphicon-briefcase"></span>
                            Uma API de utilitários e de linha de comando intuitivos permitem que você executar cargas de trabalho de produção em larga escala.
                        </p>
                        <p><a href="#" class="btn btn-primary" role="button" onclick="return NotifyWarning('Alert', null);">Informações</a></p>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">
                    <img src="Images/network.jpg" alt="Rede Rápida">
                    <div class="caption">
                        <h3>Network</h3>
                        <p>
                            <span class="glyphicon glyphicon-hourglass"></span>
                            99,9% de disponibilidade dos serviços
                        </p>
                        <p><a href="#" class="btn btn-primary" role="button" onclick="return NotifySuccess('Sucesso', null);">Informações</a></p>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">
                    <img src="Images/ssl.jpg" alt="Segurança">
                    <div class="caption">
                        <h3>Segurança</h3>
                        <p>
                            <span class="glyphicon glyphicon-hdd"></span>
                            Segurança total aos seus clientes com controle de acesso.
                        </p>
                        <p><a href="#" class="btn btn-primary" role="button" onclick="return NotifyInfo('Informações', null);">Informações</a></p>
                    </div>
                </div>
            </div>
        </div>
        <!-- /.row -->
        <hr>
        <a id="back-to-top" href="#" class="btn btn-primary btn-lg back-to-top" role="button" title="Ir para o topo da pagina" data-toggle="tooltip" data-placement="left"><span class="glyphicon glyphicon-chevron-up"></span></a>
        <!-- Footer -->
        <footer>
            <div class="row">
                <div class="col-sm-12" style="float: right;">
                    <p>Copyright &copy; AFONSOFT 2016</p>
                </div>
            </div>
        </footer>
    </div>
    <!-- /.container -->
    <script>
        jQuery(document).ready(function () {
            jQuery(window).scroll(function () {
                if (jQuery(this).scrollTop() > 50) {
                    jQuery('#back-to-top').fadeIn();
                } else {
                    jQuery('#back-to-top').fadeOut();
                }
            });
            // scroll body to 0px on click
            jQuery('#back-to-top').click(function () {
                jQuery('#back-to-top').tooltip('hide');
                jQuery('body,html').animate({ scrollTop: 0 }, 800);
                return false;
            });
            jQuery('#back-to-top').tooltip('show');
        });
    </script>
</asp:Content>
