<%@ Page Title="" Language="C#" MasterPageFile="~/Petz.Master" AutoEventWireup="true" Async="true" CodeBehind="login.aspx.cs" Inherits="Afonsoft.Petz.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <style>
        body {
            background-color:#fff;
        }
        .panel-heading {
            padding: 5px 15px;
        }

        .panel-footer {
            padding: 1px 15px;
            color: #A0A0A0;
        }

        .profile-img {
            width: 96px;
            height: 96px;
            margin: 0 auto 10px;
            display: block;
            -moz-border-radius: 50%;
            -webkit-border-radius: 50%;
            border-radius: 50%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHeadMain" runat="server">

    <!-- container -->
    <div class="container" style="margin-top: 40px">
        <!-- Title -->
        <div class="row">
            <div class="col-md-12">
                <h3>Pet System</h3>
            </div>
        </div>
        <!-- Page Features -->
        <div class="row">
            <div class="col-sm-6 col-md-4 col-md-offset-4">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <strong>Efetue o login para continuar</strong>
                    </div>
                    <div class="panel-body">
                        <fieldset>
                            <div class="row">
                                <div class="center-block">
                                    <img class="profile-img"
                                        src="Images/photo.png" alt="">
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-10  col-md-offset-1 ">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon">
                                                <i class="glyphicon glyphicon-user"></i>
                                            </span>
                                            <asp:TextBox ID="loginname" CssClass="form-control" placeholder="Login" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon">
                                                <i class="glyphicon glyphicon-lock"></i>
                                            </span>
                                            <asp:TextBox ID="password" CssClass="form-control" placeholder="Password" TextMode="Password" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="input-group">
                                            <span class="input-group-addon">
                                                <i class="glyphicon glyphicon-question-sign"></i>
                                            </span>
                                            <div class="btn-group btn-group-justified btn-group-sm" role="group" data-toggle="buttons">
                                                <label class="btn btn-default active">
                                                    <input type="radio" name="StoreOrClinet" id="RdStore" runat="server" checked value="L">Logista
                                                </label>
                                                <label class="btn btn-default">
                                                    <input type="radio" name="StoreOrClinet" id="rdClinet" runat="server" value="C">Cliente
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Button ID="btnEntrar" runat="server" CssClass="btn btn-lg btn-primary btn-block" Text="Login" OnClick="btnEntrar_Click" />
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div class="panel-footer ">
                        Não tem conta? <a href="#" onclick="">Clique aqui</a>
                    </div>
                </div>
            </div>
        </div>

        <hr>
        <!-- Footer -->
        <footer>
            <div class="row">
                <div class="col-lg-12">
                    <p>Copyright &copy; AFONSOFT 2016</p>
                </div>
            </div>
        </footer>
        <!-- /.Footer -->
    </div>
    <!-- /.container -->
</asp:Content>
