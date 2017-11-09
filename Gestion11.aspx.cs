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

public partial class Gestion11 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            //validamos si aun tenemos sesion
            if (Session["sParamUsuario"] == null) Response.Redirect("~/default.aspx");
            return;
        }
        //validamos si aun tenemos sesion
        if (Session["sParamUsuario"] == null) Response.Redirect("~/default.aspx");
        //datos del usuario
        Hashtable usuario = (Hashtable)Session["sParamUsuario"];
        Session.Add("idUsuario", usuario["idUsuario"].ToString());



        MnsjPnlVentana.CssClass = "CajaDialogo";
        MnsjPnlVentana.Style.Add("display", "none");


        VerificaMail();
    }

    private void VerificaMail()
    {
        //BUSCAMOS SI ESTE USUARIO TIENE MENSAJES NO LEIDOS
        string[] datosMnsjXusr = { Session["idUsuario"].ToString() };
        DataTable tablaMnsjXusr = Controladora.consultaDatos(sqlExpediente.mMensajesXusuario, datosMnsjXusr);
        if (tablaMnsjXusr.Rows.Count > 0)
        {
            //mostramos el panel
            pnlMensajes.Visible = true;
            //llenamos el grid
            gridMensajes.DataSource = tablaMnsjXusr;
            gridMensajes.DataBind();
        }
        else
        {
            pnlMensajes.Visible = false;
        }
    }
    //selecciono un registro del grid
    protected void gridMensajes_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gridMensajes.SelectedRow;
        string idMensaje = gridMensajes.DataKeys[row.RowIndex].Values["id_mensaje"].ToString();
        //abro el menesaje
        string[] datosMnsjDet = { idMensaje };
        DataTable tablaMnsjDetalle = Controladora.consultaDatos(sqlExpediente.mMensajeSeleccionado, datosMnsjDet);
        lblDe.Text = tablaMnsjDetalle.Rows[0]["nombre"].ToString();
        lblTitulo.Text = tablaMnsjDetalle.Rows[0]["titulo"].ToString();
        lblPrioridad.Text = tablaMnsjDetalle.Rows[0]["prioridad"].ToString();
        lblMensaje.Text = tablaMnsjDetalle.Rows[0]["mensaje"].ToString();
        lblExpediente.Text = tablaMnsjDetalle.Rows[0]["num_credito"].ToString();

        //actualizamos como leido el mensaje
        bool leido = Controladora.actualizaDatos(sqlExpediente.cMensajeLeido, datosMnsjDet);

        VerificaMail();
        MnsjModal.Show();
    }
    protected void gridMensajes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string leido = e.Row.Cells[4].Text;
            if (leido.Length < 10)
            {
                e.Row.Font.Bold = true;
                e.Row.Cells[4].Text = "No leido";
                e.Row.Cells[4].Font.Bold = true;
            }
            else
                e.Row.Font.Bold = false;
        }
    }
}