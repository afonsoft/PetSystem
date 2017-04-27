<%@ Page Title="" Language="C#" MasterPageFile="~/Store/Store.master" AutoEventWireup="true" Async="true" CodeBehind="index.aspx.cs" Inherits="Afonsoft.Petz.Store.index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceStoreHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceStoreMain" runat="server">

    <div id='calendar'></div>

    <div id="contextMenuEdit" class="dropdown clearfix profile-contextMenu" style="position: absolute; display: none; z-index: 9999;">
        <ul class="dropdown-menu" role="menu" aria-labelledby="dropdownMenu" style="display: block; position: static; margin-bottom: 5px;">
            <li><a tabindex="-1" href="#" onclick="return ActionEvent(this, 'open');"><i class="glyphicon glyphicon-folder-open"></i>Abrir Evento</a></li>
            <li><a tabindex="-1" href="#" onclick="return ActionEvent(this, 'edit');"><i class="glyphicon glyphicon-list-alt"></i>Editar Evento</a></li>
            <li><a tabindex="-1" href="#" onclick="return ActionEvent(this, 'delete');"><i class="glyphicon glyphicon-trash"></i>Excluir Evento</a></li>
            <li class="divider"></li>
            <li><a tabindex="-1" href="#" onclick="return OpenScheduling(this);"><i class="glyphicon glyphicon-calendar"></i>Novo Agendamento</a></li>
        </ul>
    </div>

    <div id="contextMenuGeral" class="dropdown clearfix profile-contextMenu" style="position: absolute; display: none; z-index: 9999;">
        <ul class="dropdown-menu" role="menu" aria-labelledby="dropdownMenu" style="display: block; position: static; margin-bottom: 5px;">
            <li><a tabindex="-1" href="#" onclick="return OpenScheduling(this);"><i class="glyphicon glyphicon-calendar"></i>Novo Agendamento</a></li>
            <li class="divider"></li>
            <li><a tabindex="-1" href="#" onclick="return ChangeView(this,'basicDay');"><i class="glyphicon glyphicon-th-list"></i>Eventos do Dia</a></li>
            <li><a tabindex="-1" href="#" onclick="return ChangeView(this,'basicWeek');"><i class="glyphicon glyphicon-th-large"></i>Eventos da Semana</a></li>
            <li><a tabindex="-1" href="#" onclick="return ChangeView(this,'month');"><i class="glyphicon glyphicon-th"></i>Eventos do Mes</a></li>
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
            ConfigCalender();
            ConfigContextMenu();
        }

        function ConfigCalender() {
            //https://fullcalendar.io/docs/
            jQuery('#calendar').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,basicWeek,basicDay'
                }, themeButtonIcons: {
                    prev: "btn-sm btn-primary", next: "btn-sm btn-primary", today: "btn-sm btn-primary", month: "btn-sm btn-primary", basicWeek: "btn-sm btn-primary", basicDay: "btn-sm btn-primary"
                },
                viewRender: function (view, element) {
                    ConfigContextMenu();
                },
                dayClick: function (date, jsEvent, view) {
                    ConfigContextMenu();
                },
                eventClick: function (calEvent, jsEvent, view) {
                    OpenEvent(calEvent.title, calEvent.id, 'open');
                    ConfigContextMenu();
                },
                eventRender: function (event, element, view) {
                    jQuery(element).tooltip({ title: event.title });
                },
                googleCalendarApiKey: 'AIzaSyCxalzrw1gGPa_pOSXtvWrku514LyNL19A',
                defaultDate: '<%= DateTime.Now.ToString("yyyy-MM-dd") %>',
                navLinks: true,
                editable: true,
                eventLimit: true,
                events: AjaxJSON('/Store/index.aspx/GetCalender', "{ 'id' : '<%= CompanyId %>', 'addressId' : '<%= AddressId %>' }")
            });
        }

        function ConfigContextMenu() {

            jQuery('td.fc-day, td.fc-day-top, div.fc-content').click(function (event) {
                jQuery("#contextMenuEdit").hide();
                jQuery("#contextMenuGeral").hide();
            });

            jQuery("#contextMenuGeral, #contextMenuEdit").on("click", "a", function (event) {
                jQuery("#contextMenuGeral").hide();
                jQuery("#contextMenuEdit").hide();
            });

            jQuery('td.fc-day, td.fc-day-top').contextmenu(function (event) {
                jQuery("#contextMenuGeral").css({
                    display: "block",
                    left: event.pageX,
                    top: event.pageY
                });
                jQuery("#contextMenuGeral").offset({ left: event.pageX, top: event.pageY });
                var date = jQuery(this).attr('data-date');
                jQuery("#contextMenuGeral").find('ul > li').children().attr('data-date', date);
                event.preventDefault();
            });

            jQuery('div.fc-content').contextmenu(function (event) {
                jQuery("#contextMenuEdit").css({
                    display: "block",
                    left: event.pageX,
                    top: event.pageY
                });
                jQuery("#contextMenuEdit").offset({ left: event.pageX, top: event.pageY });
                var id = jQuery(this).attr('data-id');
                var date = jQuery(this).attr('data-date');
                jQuery("#contextMenuEdit").find('ul > li').children().attr('data-date', date);
                jQuery("#contextMenuEdit").find('ul > li').children().attr('data-id', id);
                var title = jQuery(this).find('span.fc-title')[0].innerText;
                jQuery("#contextMenuEdit").find('ul > li').children().attr('data-title', title);
            });
        }

        function ChangeView(event, viewName) {
            var date = new Date(jQuery(event).attr('data-date'));
            date.setDate(date.getDate() + 1);
            jQuery(document).ready(function () {
                jQuery('#calendar').fullCalendar('changeView', viewName);
                jQuery('#calendar').fullCalendar('gotoDate', date);
                ConfigContextMenu();
                return true;
            });
        }

        function ActionEvent(event, action) {
            var id = jQuery(event).attr('data-id');
            var title = jQuery(event).attr('data-title');
            return OpenEvent(title, id, action);
        }

        function OpenEvent(title, id, option) {
            if (option == 'delete') {
                if (confirm('Deseja excluir esse evento?\nEvento: ' + title)) {
                    NotifyInfo('Excluindo o evento ' + title, null);
                    var msgRturn = AjaxJSON('/Store/index.aspx/DeleteEvent', "{ 'companyId' : '<%= CompanyId %>', 'addressId' : '<%= AddressId %>' , 'eventId' : " + id + " }");
                    if (msgRturn.isOk) {
                        NotifySuccess(msgRturn.mensagem, null);
                    } else {
                        NotifyError(msgRturn.mensagem, null);
                    }
                    ConfigCalender();
                    ConfigContextMenu();
                }
            }
            if (option == 'open') {
                AjaxHTML('myModalInfoHTML', '/Store/CalenderDetail.aspx?ID=<%= CompanyId %>&AddressID=<%=AddressId%>&EventID=' + id + '&Token=<%=SecurityToken%>');
            } else if (option == 'edit') {
                AjaxHTML('myModalInfoHTML', '/Store/Scheduling.aspx?ID=<%= CompanyId %>&AddressID=<%=AddressId%>&EventID=' + id + '&Token=<%=SecurityToken%>&edit=true');
            }
        jQuery('#myModalLabelHTML').html(title);
        jQuery('#modal-dialog').css({ width: "auto", "min-width": "700px", "max-width": "850px" });
        jQuery('#myModalInfo').modal('show');
        jQuery('#myModalInfo').data('bs.modal').handleUpdate();
        jQuery("#myModalInfo").on("hidden.bs.modal", function () { jQuery('body').css({ 'background-color': "#F1F3FA !important" }); });
        ConfigContextMenu();
    }

    function OpenScheduling(e) {
        var date = new Date(jQuery(e).attr('data-date'));
        date.setDate(date.getDate() + 1);
        var today = new Date();
        if (date >= today) {
            AjaxHTML('myModalInfoHTML', '/Store/Scheduling.aspx?ID=<%= CompanyId %>&AddressID=<%=AddressId%>&EventDate=' + convertDate(date) + '&Token=<%=SecurityToken%>');
                jQuery('#myModalLabelHTML').html('Novo Agendamendo - ' + convertDate(date));
                jQuery('#modal-dialog').css({ width: "600px" });
                jQuery('#myModalInfo').modal('show');
                jQuery('#myModalInfo').data('bs.modal').handleUpdate();
                jQuery("#myModalInfo").on("hidden.bs.modal", function () { jQuery('body').css({ 'background-color': "#F1F3FA !important" }); });
            } else {
                NotifyError('Não é permitido incluir agendamendo passado (' + moment(date).format('DD/MM/YYYY') + ').<br/>Favor escolher uma data superior ou igual a hoje!', null);
            }
            ConfigContextMenu();
            return true;
        }
    </script>

</asp:Content>
