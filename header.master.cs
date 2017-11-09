using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Numeric;


public partial class header : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            Response.AddHeader("REFRESH", "1080;URL=" + HttpContext.Current.Request.ApplicationPath.ToString() + "/salir.aspx");
            //se revisan permisos
            if (Session["sParametros"] == null) Response.Redirect("~/default.aspx");
            return;
        }

        int punto;
        punto = 0;
        //se revisan permisos
        if (Session["sParametros"] == null) Response.Redirect("~/default.aspx");

        //leemos los parametros
        Hashtable hshParam = (Hashtable)Session["sParametros"];
        Hashtable usuario = (Hashtable)Session["sParamUsuario"];

        //valores de encabezado
        //lblNombreCliente.Text = hshParam["nombre"].ToString();
        lblNombreCliente.Text = ConfigurationManager.AppSettings.Get("nombreProyecto").ToString();
        imgLogo.ImageUrl = "~/" + hshParam["logo"].ToString();
        lblNombreUsuario.Text = usuario["idUsuario"].ToString() + " - " + usuario["nombreUsuario"].ToString() + " [" + usuario["nombrePerfil"].ToString() + "]";


        Response.AddHeader("REFRESH", "1080;URL=" + HttpContext.Current.Request.ApplicationPath.ToString() + "/salir.aspx");


        //buscamos el titulo se la seccion
        //punto = HttpContext.Current.Request.Url.AbsolutePath.ToString().LastIndexOf("/");
        punto = HttpContext.Current.Request.Url.AbsoluteUri.ToString().LastIndexOf("/");
        //if ((HttpContext.Current.Request.Url.AbsolutePath.ToString().Substring(punto + 1)) != "Gestion11.aspx")
        if ((HttpContext.Current.Request.Url.AbsoluteUri.ToString().Substring(punto + 1)) != "Gestion11.aspx")
        {
            //lblSeccion.Text = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            //string[] datoPagina = { HttpContext.Current.Request.Url.AbsolutePath.ToString().Substring(punto + 1) };
            string[] datoPagina = { HttpContext.Current.Request.Url.AbsoluteUri.ToString().Substring(punto + 1) };
            DataTable tablaPagina = Controladora.consultaDatos(sqlParam.headerTituloPagina, datoPagina);
            lblSeccion.Text = tablaPagina.Rows[0][0].ToString();
        }
        else
        {
            lblSeccion.Text = "Bienvenido";
        }
        lblUrl.Text = HttpContext.Current.Request.Url.AbsolutePath.ToString();


        //fecha y hora
        string[] pHora = { };
        DataTable hora = Controladora.consultaDatos(sqlParam.sqlFechaHora, pHora);
        lblFechaHora.Text = hora.Rows[0][0].ToString();

        //armamos el menu
        llenaMenuPadre("0");
    }



    private void llenaMenuPadre(string idpadre)
    {
        Hashtable usuario = (Hashtable)Session["sParamUsuario"];

        //validamos si traemos información, en caso contrario, solo nos salimos
        string[] menuPadre = { idpadre, usuario["idPerfil"].ToString() };

        DataTable tablaMnPadres = Controladora.consultaDatos(sqlParam.mnPadres, menuPadre);
        for (int i = 0; i < tablaMnPadres.Rows.Count; i++)
        {
            MenuItem elemento = new MenuItem();
            elemento.Value = tablaMnPadres.Rows[i]["id_seccion"].ToString();
            elemento.Text = tablaMnPadres.Rows[i]["name_seccion"].ToString();

            mnPrincipal.Items.Add(elemento);
            //pregunto si tiene hijos
            llenaMenuHijos(tablaMnPadres.Rows[i]["id_Seccion"].ToString(), usuario["idPerfil"].ToString());
        }
    }
    private void llenaMenuHijos(string paramIdSeccion, string paramPerfil)
    {
        //validamos si traemos información, en caso contrario, solo nos salimos
        string[] menuPadre = { paramIdSeccion, paramPerfil };

        DataTable tablaMnPadres = Controladora.consultaDatos(sqlParam.mnPadres, menuPadre);
        for (int i = 0; i < tablaMnPadres.Rows.Count; i++)
        {
            MenuItem elemento = new MenuItem();
            elemento.Value = tablaMnPadres.Rows[i]["id_seccion"].ToString();
            elemento.Text = tablaMnPadres.Rows[i]["name_seccion"].ToString();
            elemento.NavigateUrl = tablaMnPadres.Rows[i]["pagina"].ToString();
            //mnPrincipal.FindItem(id_seccion).ChildItems.Add(elemento);
            mnPrincipal.FindItem(paramIdSeccion).ChildItems.Add(elemento);
            //string[] datosHijos = { tablaMnPadres.Rows[i]["id_Seccion"].ToString() };
            //if (Convert.ToInt16(Controladora.numeroDeRegistros(sqlParam.cuantosHijos, datosHijos).ToString()) > 0)
            //{
            //    llenaMenuNietos(tablaMnPadres.Rows[i]["id_Seccion"].ToString());
            //}
        }
    }
    private void llenaMenuNietos(string id_seccion)
    {
        //validamos si traemos información, en caso contrario, solo nos salimos
        string[] menuPadre = { id_seccion };

        DataTable tablaMnPadres = Controladora.consultaDatos(sqlParam.mnPadres, menuPadre);
        for (int i = 0; i < tablaMnPadres.Rows.Count; i++)
        {
            MenuItem elemento = new MenuItem();
            elemento.Value = tablaMnPadres.Rows[i]["id_seccion"].ToString();
            elemento.Text = tablaMnPadres.Rows[i]["name_seccion"].ToString();
            mnPrincipal.FindItem(id_seccion).ChildItems.Add(elemento);
            //llenaMenuHijos(tablaMnPadres.Rows[i]["id_Seccion"].ToString());
        }
    }
}
