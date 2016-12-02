<%@ Page Title="" Language="C#" MasterPageFile="~/Store/StoreScript.Master" Async="true" AutoEventWireup="true" CodeBehind="CalenderDetail.aspx.cs" Inherits="Afonsoft.Petz.Store.CalenderDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row" style="font-size: 8pt;">
        <div class="col-md-12 col-sm-12">
            <div class="panel panel-info">
                <div class="panel-heading">Informações do Cliente</div>
                <div class="panel-body" style="font-size: 8pt;">
                    <div class="row">
                        <div class="col-sm-3 col-md-3">
                            <b>Cliente:</b>
                        </div>
                        <div class="col-sm-9 col-md-9">
                            <a href="/Store/clients.aspx?Token=<%= SecurityToken %>&ID=<%= ClientId %>" data-toggle="tooltip" data-placement="top" title="Mais informações do Cliente">
                                <asp:Literal ID="lNomeClient" runat="server"></asp:Literal></a>
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
                </div>
            </div>
            <div class="panel panel-info">
                <div class="panel-heading">Informações do Pet</div>
                <div class="panel-body" style="font-size: 8pt;">
                    <div class="row">
                        <div class="col-sm-5 col-md-5">
                            <b>Nome: </b><a href="/Store/pets.aspx?Token=<%= SecurityToken %>&ID=<%= PetId %>" data-toggle="tooltip" data-placement="top" title="Mais informações do Pet">
                                <asp:Literal ID="lNomePet" runat="server"></asp:Literal></a>
                        </div>
                        <div class="col-sm-2 col-md-2">
                            <b>Sexo: </b>
                            <asp:Literal ID="lSexoPet" runat="server"></asp:Literal>
                        </div>
                        <div class="col-sm-5 col-md-5">
                            <b>Tamanho: </b>
                            <asp:Literal ID="lTamanhoPet" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-7 col-md-7">
                            <b>Raça: </b>
                            <asp:Literal ID="ltRacaPet" runat="server"></asp:Literal>
                        </div>
                        <div class="col-sm-5 col-md-5">
                            <b>Cor: </b>
                            <asp:Literal ID="lCorPet" runat="server"></asp:Literal>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-info">
                <div class="panel-heading">Informações do Horario e Serviços</div>
                <div class="panel-body" style="font-size: 8pt;">
                    <div class="col-sm-4 col-md-4">
                        <div class="panel panel-default" style="font-size: 8pt;">
                            <div class="panel-heading">Horário</div>
                            <div class="panel-body" style="font-size: 8pt;">
                                <div class="row">
                                    <div class="col-sm-2 col-md-2">
                                        Data:
                                    </div>
                                    <div class="col-sm-10 col-md-10">
                                        <asp:Literal ID="ltDataPet" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2 col-md-2">
                                        Inicio:
                                    </div>
                                    <div class="col-sm-10 col-md-10">
                                        <asp:Literal ID="lHoraInicioPet" runat="server"></asp:Literal>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2 col-md-2">
                                        Fim:
                                    </div>
                                    <div class="col-sm-10 col-md-10">
                                        <asp:Literal ID="lHoraFimPet" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-8 col-md-8">
                        <div class="panel panel-default" style="font-size: 8pt;">
                            <div class="panel-heading">Serviços</div>
                            <div class="panel-body" style="font-size: 8pt;">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-info">
                <div class="panel-heading">Observações</div>
                <div class="panel-body" style="font-size: 8pt;">
                    <asp:Literal ID="ltObs" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
    </div>

    <script>
         $('.starrr').starrr({readOnly: true, rating: <%= ClientRating %>});
    </script>
</asp:Content>
