<%@ Page Title="" Language="C#" MasterPageFile="~/Store/StoreScript.Master" AutoEventWireup="true" Async="true" CodeBehind="UserDetail.aspx.cs" Inherits="Afonsoft.Petz.Store.UserDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .input-group-xs > .form-control,
        .input-group-xs > .input-group-addon,
        .input-group-xs > .input-group-btn > .btn {
            height: 22px;
            padding: 1px 5px;
            font-size: 12px;
            line-height: 1.5;
        }

        .starrr {
            display: inline-block;
        }

            .starrr a {
                font-size: 12px;
                padding: 0 1px;
                cursor: pointer;
                color: #FFD119;
                text-decoration: none;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12 col-sm-12">
            <div class="panel panel-info">
                <div class="panel-heading">Informações do Usuário</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-3 col-md-3">
                            <a href="#" onclick="return OpenUpload('CLIENT',<%= UserId %>, 'ImgUserPic');">
                                <img id="ImgUserPic" class="img-responsive" src="/ImageHandler.ashx?ID=<%= UserId %>&type=USER&token=<%= SecurityToken %>" style="min-width: 100px; min-height: 80px;" />
                            </a>
                        </div>
                        <div class="col-sm-9 col-md-9">
                            <div class="row">
                                <div class="col-sm-8 col-md-8">
                                    <label><b>Nome</b></label><br />
                                    <asp:Literal ID="ltUserName" runat="server"></asp:Literal>
                                </div>
                                <div class="col-sm-4 col-md-4">
                                    <label><b>Classificação</b></label><br />
                                    <div class='starrr' onclick="return OpenRating('<%= UsuarioLogado.Name %>',<%= UserId %>, 'USER');"></div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-12">
                                    <label><b>E-Mail</b></label><br />
                                    <asp:Literal ID="ltUserEmail" runat="server"></asp:Literal>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6 col-md-6">
                                    <b>Usuário:&nbsp;</b><asp:Literal ID="ltUserLogin" runat="server"></asp:Literal>
                                </div>
                                <div class="col-sm-6 col-md-6">
                                    <b>Senha:&nbsp;</b><a href="#" onclick="return ChangePassword('CLIENT',<%= UserId %>);"><asp:Literal ID="ltUserPass" runat="server" Text="************"></asp:Literal></a>
                                </div>
                            </div>
                            <div id="myContentPWD" style="display: none;">
                                <div class="row">
                                    <div class="col-sm-12 col-md-12">
                                        <div class="input-group input-group-xs" role="group">
                                            <span class="input-group-addon" id="basic-addon1">Senha Atual</span>
                                            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" CssClass="form-control" placeholder="Senha Atual" aria-describedby="basic-addon1"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12 col-md-12">
                                        <div class="input-group input-group-xs" role="group">
                                            <span class="input-group-addon" id="basic-addon2">Nova Senha </span>
                                            <asp:TextBox ID="txtNewPassword1" TextMode="Password" runat="server" CssClass="form-control" placeholder="Nova Senha" aria-describedby="basic-addon2"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12 col-md-12">
                                        <div class="input-group input-group-xs" role="group">
                                            <span class="input-group-addon" id="basic-addon3">Nova Senha </span>
                                            <asp:TextBox ID="txtNewPassword2" TextMode="Password" runat="server" CssClass="form-control" placeholder="Nova Senha" aria-describedby="basic-addon3"></asp:TextBox>
                                            <span class="input-group-btn">
                                                <button class="btn btn-primary" type="button" onclick="return SavePassord();">Salvar</button>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        $('.starrr').starrr({readOnly: true, rating: <%= UserRating %>});

        function SavePassord() {
            var pwd1 = $("#<%= txtNewPassword1.ClientID %>").val();
            var pwd2 = $("#<%= txtNewPassword2.ClientID %>").val();
            var pwd = $("#<%= txtPassword.ClientID %>").val();

            if (pwd == "" || pwd2 == "" || pwd1 == "") {
                NotifyError("Favor inserir as senhas", null);
                return false;
            }
            if (pwd2 != pwd1) {
                NotifyError("Senha não confere!", null);
                return false;
            }

            var msgRturn = AjaxJSON('/Store/UserDetail.aspx/SetPassword', "{ 'OldPWD' : '" + pwd + "', 'NewPWD' : '" + pwd2 + "' }");
            if (msgRturn.isOk) {
                NotifySuccess(msgRturn.mensagem, null);
                ChangePassword(this, 1);
            } else {
                NotifyError(msgRturn.mensagem, null);
            }
            return false;
        }

        function ChangePassword(type, id) {
            jQuery("#myContentPWD").toggle();
        }
    </script>
</asp:Content>
