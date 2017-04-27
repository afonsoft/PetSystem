<%@ Page Title="" Language="C#" MasterPageFile="~/Store/Store.master" AutoEventWireup="true" Async="true" CodeBehind="Settings.aspx.cs" Inherits="Afonsoft.Petz.Store.Settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceStoreHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceStoreMain" runat="server">
    <div class="row">
        <div class="col-xs-4 col-md-4 col-lg-4">
            <div class="thumbnail">
                <a href="#" onclick="return OpenUpload('CONPANY','<%= CompanyId %>', '<%= ImgPetShop.ClientID %>');">
                    <asp:Image ID="ImgPetShop" runat="server" CssClass="img-responsive" AlternateText="" Style="width: 180px;" />
                </a>
            </div>
        </div>
        <div class="col-xs-8 col-md-8 col-lg-8">
            <div class="caption">
                <h3>
                    <asp:Literal ID="ltName" runat="server"></asp:Literal></h3>
                <p>
                    <asp:Literal ID="ltAddress" runat="server"></asp:Literal><br />
                    <asp:Literal ID="ltPhone" runat="server"></asp:Literal>
                </p>
            </div>
        </div>
    </div>
    <div class="clearfix">&nbsp;</div>

    <div id="content">
        <ul id="tabs" class="nav nav-tabs" data-tabs="tabs">
            <li class="active"><a href="#pane_1" data-toggle="tab"><i class="glyphicon glyphicon-user"></i>&nbsp;Permissões</a></li>
            <li><a href="#pane_2" data-toggle="tab"><i class="glyphicon glyphicon-th-list"></i>&nbsp;Serviços</a></li>
            <li><a href="#pane_3" data-toggle="tab"><i class="glyphicon glyphicon-facetime-video"></i>&nbsp;WebCam</a></li>
            <li><a href="#pane_4" data-toggle="tab"><i class="glyphicon glyphicon-pencil"></i>&nbsp;Cadastro</a></li>
        </ul>
        <div id="my-tab-content" class="tab-content">
            <div class="tab-pane active" id="pane_1">
                <h1>Permissões</h1>
                <p>Configurar as permissões dos funcionários</p>
                <div class="row">
                    <div class="col-md-offset-9 col-md-3" style="text-align:left;">
                        <button class="btn btn-primary">Adicionar Funcionário</button>
                    </div>
                </div>
            </div>
            <div class="tab-pane" id="pane_2">
                <h1>Serviços</h1>
                <p>Selecione os serviços disponiveis abaixo</p>
            </div>
            <div class="tab-pane" id="pane_3">
                <h1>WebCam</h1>
                <p>Configuração da WebCam do Banho dos Pets</p>
            </div>
            <div class="tab-pane" id="pane_4">
                <h1>Cadastro</h1>
                <div class="form-group row">
                    <label for="inputEmail3" class="col-md-2 col-form-label">Nome</label>
                    <div class="col-md-10">
                        <input type="email" class="form-control" id="inputEmail3" placeholder="Email">
                    </div>
                </div>
                <div class="form-group row">
                    <label for="inputEmail4" class="col-md-2 col-form-label">Apelido</label>
                    <div class="col-md-10">
                        <input type="email" class="form-control" id="inputEmail4" placeholder="Email">
                    </div>
                </div>
            </div>
        </div>
    </div>



</asp:Content>
