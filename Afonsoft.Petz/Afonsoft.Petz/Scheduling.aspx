<%@ Page Title="" Language="C#" MasterPageFile="~/Petz.Master" AutoEventWireup="true" CodeBehind="Scheduling.aspx.cs" Inherits="Afonsoft.Petz.API.Scheduling" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <style>

	body {
		margin: 40px 10px;
		padding: 0;
		font-family: "Lucida Grande",Helvetica,Arial,Verdana,sans-serif;
		font-size: 14px;
	}

	#calendar {
		max-width: 900px;
		margin: 0 auto;
	}

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHeadMain" runat="server">
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
            //Inicialização do JS

            //https://fullcalendar.io/docs/
            jQuery('#calendar').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,basicWeek,basicDay'
                },
                eventClick: function (event, element) {
                    alert(event.id);
                },
                googleCalendarApiKey: 'AIzaSyCxalzrw1gGPa_pOSXtvWrku514LyNL19A',
                defaultDate: '2016-10-12',
                navLinks: true, 
                editable: true,
                eventLimit: true, 
                events: [
                    {
                        id: 1,
                        title: 'All Day Event',
                        start: '2016-10-01'
                    },
                    {
                        id: 2,
                        title: 'Long Event',
                        start: '2016-10-07',
                        end: '2016-10-10'
                    },
                    {
                        id: 999,
                        title: 'Repeating Event',
                        start: '2016-10-09T16:00:00'
                    },
                    {
                        id: 999,
                        title: 'Repeating Event',
                        start: '2016-10-16T16:00:00'
                    },
                    {
                        id: 3,
                        title: 'Conference',
                        start: '2016-10-11',
                        end: '2016-10-13'
                    },
                    {
                        id: 4,
                        title: 'Meeting',
                        start: '2016-10-12T10:30:00',
                        end: '2016-10-12T12:30:00'
                    },
                    {
                        id: 5,
                        title: 'Lunch',
                        start: '2016-10-12T12:00:00'
                    },
                    {
                        id: 6,
                        title: 'Meeting',
                        start: '2016-10-12T14:30:00'
                    },
                    {
                        title: 'Happy Hour',
                        start: '2016-10-12T17:30:00'
                    },
                    {
                        id: 7,
                        title: 'Dinner',
                        start: '2016-10-12T20:00:00'
                    },
                    {
                        id: 8,
                        title: 'Birthday Party',
                        start: '2016-10-13T07:00:00'
                    },
                    {
                        id: 9,
                        title: 'Click for Google',
                        url: 'http://google.com/',
                        start: '2016-10-28'
                    }
                ]
            });
        }

    </script>

    <div id='calendar'></div>

</asp:Content>
