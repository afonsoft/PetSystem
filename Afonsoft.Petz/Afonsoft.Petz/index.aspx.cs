using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


/*
netsh advfirewall firewall add rule name="IISExpressWeb" dir=in protocol=tcp localport=5557 profile=private remoteip=localsubnet action=allow
netsh http add urlacl url=http://*:5557/ user=everyone

FTP:    ftp.afonsoft.com.br
User:   petsystem
PWD:    Senha#2016

 */


namespace Afonsoft.Petz
{
    public partial class index : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               // NotifySuccess("Teste Sucesso!");
               // NotifyWarning("Teste de Warning");
                //NotifyInfo("Teste de info");
                //NotifyError("Teste de err");
            }
        }
    }
}