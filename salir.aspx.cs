using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Numeric;

public partial class salir : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //cerramos la sesion en el log
        if (Session["sParamUsuario"] == null) Response.Redirect("~/default.aspx");
        //datos del usuario
        Hashtable usuario = (Hashtable)Session["sParamUsuario"];
        Session.Add("idUsuario", usuario["idUsuario"].ToString());

        //ELIMINAMOS LA SESION
        string[] datosBusrActivo = { Session["idUsuario"].ToString() };
        bool bUsrActivo = Controladora.eliminaDatos(sqlParam.eUsrActivo, datosBusrActivo);

        //MARCAMOS LA SALIDA
        string[] datosAusrLog = { Session["idUsuario"].ToString(), GetUserIP(), Request.UserHostAddress.ToString(), Request.UserAgent.ToString(), "S" };
        bool aUsrLog = Controladora.registraDatos(sqlParam.aUsrLog, datosAusrLog);

        //parámetros pagina
        Session.Add("sParametros", null);
        //parametros usuario
        Session.Add("sParamUsuario", null);
        Response.Redirect("~/default.aspx");

        
    }

    private string GetUserIP()
    {
        string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (!string.IsNullOrEmpty(ipList))
        {
            return ipList.Split(',')[0];
        }

        return Request.ServerVariables["REMOTE_ADDR"];
    }
}