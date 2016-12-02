﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Store/Store.master" AutoEventWireup="true" Async="true" CodeBehind="Clients.aspx.cs" Inherits="Afonsoft.Petz.Store.ClientsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceStoreHead" runat="server">
    <style>
        .row {
            margin-top: 10px;
            padding: 0 10px;
        }

        .clickable {
            cursor: pointer;
        }

        .panel-heading div {
            margin-top: -18px;
            font-size: 15px;
        }

            .panel-heading div span {
                margin-left: 5px;
            }

        .panel-body {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceStoreMain" runat="server">
    <div class="row" style="font-size: 8pt;">
        <div class="col-md-12" style="text-align: right;">
            <button class="btn btn-sm btn-primary" onclick="return PopupAction(this, 'new');">Novo Cliente</button>
        </div>
    </div>
    <div class="row" style="font-size: 8pt;">
        <div class="col-md-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Clientes</h3>
                    <div class="pull-right">
                        <span class="clickable filter" data-toggle="tooltip" title="Filtro" data-container="body">
                            <i class="glyphicon glyphicon-filter"></i>
                        </span>
                    </div>
                </div>
                <div class="panel-body">
                    <input type="text" class="form-control" id="dev-table-filter" data-action="filter" data-filters="#dev-table" placeholder="Filtro de Clientes" />
                </div>
                <asp:Repeater ID="rptClientEntities" runat="server">
                    <HeaderTemplate>
                        <table class="table table-hover" id="dev-table">
                            <thead>
                                <tr>
                                    <th>#</th>
                                    <th>Nome</th>
                                    <th>Apelido</th>
                                    <th>E-Mail</th>
                                    <th>Documento</th>
                                    <th>Sexo</th>
                                    <th>Aniversário</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr data-id="<%# Eval("ID") %>" data-title="<%# Eval("Name") %>">
                            <td data-id="<%# Eval("ID") %>" data-title="<%# Eval("Name") %>"><%# Eval("ID") %></td>
                            <td data-id="<%# Eval("ID") %>" data-title="<%# Eval("Name") %>"><a onclick="<%# string.Format("return PopUpClient('{0}','{1}');", Eval("Name"),Eval("ID")) %>" href="#" data-toggle="tooltip" data-placement="top" title="Mais informações"><%# Eval("Name") %></a></td>
                            <td data-id="<%# Eval("ID") %>" data-title="<%# Eval("Name") %>"><%# Eval("NickName") %></td>
                            <td data-id="<%# Eval("ID") %>" data-title="<%# Eval("Name") %>"><%# Eval("Email") %></td>
                            <td data-id="<%# Eval("ID") %>" data-title="<%# Eval("Name") %>"><%# Eval("Document") %></td>
                            <td data-id="<%# Eval("ID") %>" data-title="<%# Eval("Name") %>"><%# (((Afonsoft.Petz.Model.EnumSex)Eval("Sex")) == Afonsoft.Petz.Model.EnumSex.Male ? "M" : "F") %></td>
                            <td data-id="<%# Eval("ID") %>" data-title="<%# Eval("Name") %>"><%#  String.Format("{0:dd/MM/yyyy}", Eval("Birthday")) %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                    </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>

    <div id="contextMenuGeral" class="dropdown clearfix profile-contextMenu" style="position: absolute; display: none; z-index: 9999;">
        <ul class="dropdown-menu" role="menu" aria-labelledby="dropdownMenu" style="display: block; position: static; margin-bottom: 5px;">
            <li><a tabindex="-1" href="#" onclick="return PopupAction(this, 'open');"><i class="glyphicon glyphicon-user"></i>Abrir Cliente</a></li>
            <li><a tabindex="-1" href="#" onclick="return PopupAction(this,'edit');"><i class="glyphicon glyphicon-list-alt"></i>Editar Cliente</a></li>
            <li class="divider"></li>
            <li><a tabindex="-1" href="#" onclick="return PopupAction(this,'pet');"><i class="glyphicon glyphicon-piggy-bank"></i>Visualizar Pets</a></li>
            <li class="divider"></li>
            <li><a tabindex="-1" href="#" onclick="return PopupAction(this, 'new');"><i class="glyphicon glyphicon-user"></i>Novo Cliente</a></li>
        </ul>
    </div>
    <div id="contextMenuBody" class="dropdown clearfix profile-contextMenu" style="position: absolute; display: none; z-index: 9998;">
        <ul class="dropdown-menu" role="menu" aria-labelledby="dropdownMenu" style="display: block; position: static; margin-bottom: 5px;">
            <li><a tabindex="-1" href="#" onclick="return PopupAction(this, 'new');"><i class="glyphicon glyphicon-user"></i>Novo Cliente</a></li>
        </ul>
    </div>


    <script>
        //AjaxToolKit (UpdatePanel)
        if (typeof (Sys) !== 'undefined') {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(EndRequestHandler);
            }
        }
        //jQuery com AjaxToolKit
        jQuery(document).ready(function () {
            EndRequestHandler(null, null);
        });

        function EndRequestHandler(sender, args) {
            ConfigFilter();
            ConfigContextMenu();
        }

        function ConfigContextMenu() {

            jQuery('table tr, div.profile-content').click(function (event) {
                jQuery("#contextMenuGeral").hide();
                jQuery("#contextMenuBody").hide();
            });

            jQuery('div.profile-content').contextmenu(function (event) {
                jQuery("#contextMenuBody").css({
                    display: "block",
                    left: event.pageX,
                    top: event.pageY
                });
                jQuery("#contextMenuBody").offset({ left: event.pageX, top: event.pageY });
                event.preventDefault();
            });

            jQuery('table tr').contextmenu(function (event) {
                jQuery("#contextMenuGeral").css({
                    display: "block",
                    left: event.pageX,
                    top: event.pageY
                });
                jQuery("#contextMenuGeral").offset({ left: event.pageX, top: event.pageY });
                var id = jQuery(this).attr('data-id');
                jQuery("#contextMenuGeral").find('ul > li').children().attr('data-id', id);
                var title = jQuery(this).attr('data-title');
                jQuery("#contextMenuGeral").find('ul > li').children().attr('data-title', title);
                event.preventDefault();
            });
        }

        function ConfigFilter() {
            // attach table filter plugin to inputs
            jQuery('[data-action="filter"]').filterTable();
            jQuery('.container').on('click', '.panel-heading span.filter', function (e) {
                var $this = $(this),
                    $panel = $this.parents('.panel');

                $panel.find('.panel-body').slideToggle();
                if ($this.css('display') != 'none') {
                    $panel.find('.panel-body input').focus();
                }
            });
            jQuery('[data-toggle="tooltip"]').tooltip();
        }

        function PopUpClient(title, clientId) {
            AjaxHTML('myModalInfoHTML', '/Store/ClientDetail.aspx?ID=' + clientId + '&CompanyID=<%=CompanyId%>&Token=<%=SecurityToken%>');
            jQuery('#myModalLabelHTML').html(title);
            jQuery('#myModalInfo').modal('show');
            jQuery('#myModalInfo').data('bs.modal').handleUpdate();
            jQuery("#myModalInfo").on("hidden.bs.modal", function () { jQuery('body').css({ 'background-color': "#F1F3FA !important" }); });
            return false;
        }

        function PopupAction(event, action) {
            if (action == 'new') {
                return false;
            }

            var id = jQuery(event).attr('data-id');
            var title = jQuery(event).attr('data-title');

            if (action == 'open') {
                PopUpClient(title, id);
            }
            return false;
        }
    </script>
</asp:Content>
