<%@ Page Title="" Language="C#" MasterPageFile="~/Store/StoreScript.Master" AutoEventWireup="true" CodeBehind="RatingDetail.aspx.cs" Inherits="Afonsoft.Petz.Store.RatingDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12 col-sm-12">
            <div class="panel panel-info">
                <div class="panel-heading">Rating Histórico</div>
                <div class="panel-body">
                    <asp:Repeater ID="RepeaterRating" runat="server">
                        <HeaderTemplate>
                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                        </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("value") %>
                            </ItemTemplate>
                        <FooterTemplate>
                                </div>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
