using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Net.Sockets;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtUsuario.Focus();
    }
    protected void btnBuscaUsuario_Click(object sender, EventArgs e)
    {
        lblGeneral.Text = "";
        lblGeneral.Visible = false;
        //validamos que tenga datos el nombre de usuario y la contraseña
        if (txtUsuario.Text.ToString().Trim().Equals("") && txtPassword.Text.ToString().Trim().Equals(""))
        {
            lblGeneral.Text = "Debe ingresar el usuario y contraseña para validar";
            lblGeneral.CssClass = "labelError";
            lblGeneral.Visible = true;
            return;
        }
        if (txtUsuario.Text.ToString().Trim().Equals(""))
        {
            lblGeneral.Text = "Es necesario que ingrese el nombre de usuario";
            lblGeneral.CssClass = "labelError";
            lblGeneral.Visible = true;
            return;
        }
        if (txtPassword.Text.ToString().Trim().Equals(""))
        {
            lblGeneral.Text = "Es necesario que ingrese la contraseña";
            lblGeneral.CssClass = "labelError";
            lblGeneral.Visible = true;
            return;
        }
        //lo buscamos en la base de datos
        string[] datosUsuarios = { txtUsuario.Text.ToString(), txtPassword.Text.ToString() };
        DataTable tablaUsuario = Controladora.consultaDatos(sqlParam.sqlUsuarios, datosUsuarios);
        if (tablaUsuario.Rows.Count > 0)
        {
            //si se econtro el usuario

            string idUsuario = tablaUsuario.Rows[0]["id_usuario"].ToString();

            //asignamos los parámetros
            //abrimos la tabla de los parametros
            string[] sqlParamDatos = { tablaUsuario.Rows[0]["id_empresa"].ToString() };
            DataTable param = Controladora.consultaDatos(sqlParam.sqlParametros, sqlParamDatos);

            //preguntamos si aplicamos la hora del sistema
            if (param.Rows[0]["v7hr_aplica"].ToString().Equals("1"))
            {
                //si aplica la hora en la empresa
                Int32 hora = DateTime.Now.Hour;
                if ((hora > Convert.ToInt32(param.Rows[0]["v7hr_inicio"].ToString())) && (hora < Convert.ToInt32(param.Rows[0]["v7hr_fin"].ToString())))
                {
                    #region log usuarios

                    //preguntamos si este empresa requiere la validación
                    if (tablaUsuario.Rows[0]["tipo_Acceso"].ToString().Equals("1"))
                    {
                        //validamos si se encuentra ya logeado
                        string[] datosUsrActivo = { idUsuario };
                        DataTable tablaUsrActivo = Controladora.consultaDatos(sqlParam.bUsrActivo, datosUsrActivo);
                        if (tablaUsrActivo.Rows.Count > 0)
                        {
                            //ya esta logeado
                            lblGeneral.Text = "El usuario ya esta activo en otro equipo, es necesario que cierre la sesión para ingresar de nuevo";
                            lblGeneral.CssClass = "labelError";
                            lblGeneral.Visible = true;
                            return;
                        }
                        else
                        {
                            //insertamos el acceso del usuario
                            string[] datosAusrActivo = { idUsuario, HttpContext.Current.Session.SessionID.ToString() };
                            bool aUsrActivo = Controladora.registraDatos(sqlParam.aUsrActivo, datosAusrActivo);
                        }
                    }


                    //insertamos tambien en el log
                    string[] datosAusrLog = { idUsuario, GetUserIP(), Request.UserHostAddress.ToString(), Request.UserAgent.ToString(), "E" };
                    bool aUsrLog = Controladora.registraDatos(sqlParam.aUsrLog, datosAusrLog);

                    #endregion
                    //parametros cliente
                    Hashtable parametros = new Hashtable();
                    parametros.Add("idEmpresa", param.Rows[0]["id_empresa"].ToString());
                    parametros.Add("colorCliente", param.Rows[0]["color_fondo_superior"].ToString());
                    parametros.Add("nombre", param.Rows[0]["nombre"].ToString());
                    parametros.Add("logo", param.Rows[0]["logo"].ToString());
                    parametros.Add("colorTD", param.Rows[0]["colorTD"].ToString());


                    //obtenemos el id_flujo
                    string[] datosIdFlujo = { param.Rows[0]["id_empresa"].ToString() };
                    DataTable tablaIdFlujo = Controladora.consultaDatos(sqlParam.mIdFlujo, datosIdFlujo);

                    parametros.Add("idFlujo", tablaIdFlujo.Rows[0]["id_flujo"].ToString());

                    Session.Add("sParametros", parametros);

                    //parametros usuario
                    Hashtable paramUsuario = new Hashtable();
                    paramUsuario.Add("idUsuario", tablaUsuario.Rows[0]["id_usuario"].ToString());
                    paramUsuario.Add("nombreUsuario", tablaUsuario.Rows[0]["nombre"].ToString());
                    paramUsuario.Add("idPerfil", tablaUsuario.Rows[0]["id_perfil"].ToString());
                    paramUsuario.Add("nombrePerfil", tablaUsuario.Rows[0]["nombre_perfil"].ToString());
                    Session.Add("sParamUsuario", paramUsuario);
                    Response.Redirect("Gestion11.aspx");
                }
                else
                {
                    lblGeneral.Text = "En estos momentos no hay acceso al sistema";
                    lblGeneral.Visible = true;
                }
            }
            else
            {
                #region log usuarios

                //preguntamos si este empresa requiere la validación
                if (tablaUsuario.Rows[0]["tipo_Acceso"].ToString().Equals("1"))
                {
                    //validamos si se encuentra ya logeado
                    string[] datosUsrActivo = { idUsuario };
                    DataTable tablaUsrActivo = Controladora.consultaDatos(sqlParam.bUsrActivo, datosUsrActivo);
                    if (tablaUsrActivo.Rows.Count > 0)
                    {
                        //ya esta logeado
                        lblGeneral.Text = "El usuario ya esta activo en otro equipo, es necesario que cierre la sesión para ingresar de nuevo";
                        lblGeneral.CssClass = "labelError";
                        lblGeneral.Visible = true;
                        return;
                    }
                    else
                    {
                        //insertamos el acceso del usuario
                        string[] datosAusrActivo = { idUsuario, HttpContext.Current.Session.SessionID.ToString() };
                        bool aUsrActivo = Controladora.registraDatos(sqlParam.aUsrActivo, datosAusrActivo);
                    }
                }


                //insertamos tambien en el log
                string[] datosAusrLog = { idUsuario, GetUserIP(), Request.UserHostAddress.ToString(), Request.UserAgent.ToString(), "E" };
                bool aUsrLog = Controladora.registraDatos(sqlParam.aUsrLog, datosAusrLog);

                #endregion
                //parametros cliente
                Hashtable parametros = new Hashtable();
                parametros.Add("idEmpresa", param.Rows[0]["id_empresa"].ToString());
                parametros.Add("colorCliente", param.Rows[0]["color_fondo_superior"].ToString());
                parametros.Add("nombre", param.Rows[0]["nombre"].ToString());
                parametros.Add("logo", param.Rows[0]["logo"].ToString());
                parametros.Add("colorTD", param.Rows[0]["colorTD"].ToString());


                //obtenemos el id_flujo
                string[] datosIdFlujo = { param.Rows[0]["id_empresa"].ToString() };
                DataTable tablaIdFlujo = Controladora.consultaDatos(sqlParam.mIdFlujo, datosIdFlujo);

                parametros.Add("idFlujo", tablaIdFlujo.Rows[0]["id_flujo"].ToString());

                Session.Add("sParametros", parametros);

                //parametros usuario
                Hashtable paramUsuario = new Hashtable();
                paramUsuario.Add("idUsuario", tablaUsuario.Rows[0]["id_usuario"].ToString());
                paramUsuario.Add("nombreUsuario", tablaUsuario.Rows[0]["nombre"].ToString());
                paramUsuario.Add("idPerfil", tablaUsuario.Rows[0]["id_perfil"].ToString());
                paramUsuario.Add("nombrePerfil", tablaUsuario.Rows[0]["nombre_perfil"].ToString());
                Session.Add("sParamUsuario", paramUsuario);
                Response.Redirect("Gestion11.aspx");
            }

            
        }
        else
        {
            //no se encontró el usuario
            lblGeneral.Text = "No se encontró el nombre de usuario, favor de verificar";
            lblGeneral.CssClass = "labelError";
            lblGeneral.Visible = true;
            txtUsuario.Text = "";
            txtPassword.Text = "";
            return;
        }
    }
    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
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
