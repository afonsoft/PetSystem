<%@ Page Title="" Language="C#" MasterPageFile="~/Store/StoreScript.Master" Async="true" AutoEventWireup="true" CodeBehind="ClientDetail.aspx.cs" Inherits="Afonsoft.Petz.Store.ClientDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row" style="font-size: 8pt;">
        <div class="col-md-12 col-sm-12">
            <div class="panel panel-info">
                <div class="panel-heading">Informações</div>
                <div class="panel-body" style="font-size: 8pt;">
                    <div class="row">
                        <div class="col-sm-3 col-md-3">
                            <b>Cliente:</b>
                        </div>
                        <div class="col-sm-9 col-md-9">
                            <asp:Literal ID="lNomeClient" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-md-3">
                            <b>Telefones:</b>
                        </div>
                        <div class="col-sm-5 col-md-5">
                            <asp:Literal ID="lTelefoneClient" runat="server"></asp:Literal>
                        </div>
                        <div class="col-sm-4 col-md-4">
                            <div class='starrr' onclick="return OpenRating('<%= lNomeClient.Text %>',<%= ClientId %>, 'CLIENT');"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 col-md-3">
                            <b>Documento:</b>
                        </div>
                        <div class="col-sm-3 col-md-3">
                            <asp:Literal ID="lDocumentoClient" runat="server"></asp:Literal>
                        </div>
                        <div class="col-sm-3 col-md-3">
                            <b>Sexo:</b>
                        </div>
                        <div class="col-sm-3 col-md-3">
                            <asp:Literal ID="lSexoClient" runat="server"></asp:Literal>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-info">
                <div class="panel-heading">Endereço</div>
                <div class="panel-body" style="font-size: 8pt;">
                    <div class="row">
                        <div class="col-md-12 col-sm-12">
                            <asp:Literal ID="lEnderecoClient" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 col-sm-12">
                            <div id="map-container" class="map-container" style="width: 600px;">
                                <div id="map" style="width: 593px; height: 400px"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../Scripts/maps/maps.js"></script>
    <script>
        $('.starrr').starrr({readOnly: true, rating: <%= ClientRating %>});
        initializeView('map');
    </script>
</asp:Content>
