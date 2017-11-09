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

using System.Net; // Nueva
using System.Net.Mail;

using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

//using System.Windows.Forms;

public partial class opeExpediente : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            //creaControles();
            //validamos si aun tenemos sesion
            if (Session["sParamUsuario"] == null) Response.Redirect("~/default.aspx");
            return;
        }
        //validamos si aun tenemos sesion
        if (Session["sParamUsuario"] == null) Response.Redirect("~/default.aspx");
        //datos del usuario
        Hashtable usuario = (Hashtable)Session["sParamUsuario"];
        Session.Add("idUsuario", usuario["idUsuario"].ToString());
        //parametros
        Hashtable parametros = (Hashtable)Session["sParametros"];
        Session.Add("idFlujo", parametros["idFlujo"].ToString());
        Session.Add("idEmpresa", parametros["idEmpresa"].ToString());

        //propiedades de los paneles
        pnlAvisoDoctosActual.CssClass = "CajaDialogo";
        pnlAvisoDoctosActual.Style.Add("display", "none");
        pnlAvisoEtapaSiguiente.CssClass = "CajaDialogo";
        pnlAvisoEtapaSiguiente.Style.Add("display", "none");
        pnlVentanaRegresaEtapaInicial.CssClass = "CajaDialogo";
        pnlVentanaRegresaEtapaInicial.Style.Add("display", "none");
        pnlMDoctos.CssClass = "CajaDialogo";
        pnlMDoctos.Style.Add("display", "none");
        MnsjPnlVentana.CssClass = "CajaDialogo";
        MnsjPnlVentana.Style.Add("display", "none");


        pnlTABSvntn.CssClass = "CajaDialogo";
        pnlTABSvntn.Style.Add("display", "none");

        //pnlCamposEspecialesInicio.CssClass = "CajaDialogo";
        //pnlCamposEspecialesInicio.Style.Add("display", "none");


        //sIdEtapa
        Session.Add("sIdEtapa", "");

        if (Session["idFlujo"].ToString().Equals("2"))
        {
            btnVerCalculoTasaInteres.Visible = true;
        }
        else { btnVerCalculoTasaInteres.Visible = false; }


        listaTipoServicio_Fill();		
		listaCliente_Fill();
		listaNorma_Fill();


        //validaciones de los controles
        #region validaControles


        //se agrega la opción de convertir todo a mayúsculas

        foreach (Control txt in pnlWinCopeVivienda.Controls)
        {
            if (txt is TextBox)
            {
                ((TextBox)txt).Attributes.Add("onKeyUp", "javascript:mayusculas(this);");
            }
        }
        //pnlWinOpeLaborales
        foreach (Control txt in pnlWinOpeLaborales.Controls)
        {
            if (txt is TextBox)
            {
                ((TextBox)txt).Attributes.Add("onKeyUp", "javascript:mayusculas(this);");
            }
        }
        //pnlWinOpePersona
        foreach (Control txt in pnlWinOpePersona.Controls)
        {
            if (txt is TextBox)
            {
                ((TextBox)txt).Attributes.Add("onKeyUp", "javascript:mayusculas(this);");
            }
        }
        #endregion /validaControles


        if (Session["numExpediente"] == null)
        {

        }
        else
        {
            txtExpediente.Text = Session["numExpediente"].ToString();
        }


		ocultoValorCondicion.Value = "";
		
        //llenamos listas
        //Utilerias.Lista_Fill(listav5ftvProyectos, sqlExpediente.v5ftvListaProyectos, "SV");

    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        //limpiaVariablesTramites();
        pnlLecturaMensajes.Visible = false;
        pnlMensaje.Visible = false;
        pnlTitulosEtapa.Visible = false;
        //cerramos todos los paneles
        pnlBtnDoctosActual.Visible = false;
        pnlDoctosAnterior.Visible = false;
        pnlDoctosSiguiente.Visible = false;
        pnlTramites.Visible = false;
        pnlDocumentos.Visible = false;
        pnlValidaciones.Visible = false;
        gridPersonaSeleccionada.Visible = false;
        gridResultadoBuscar.Visible = true;
        pnlMgridDoctos.Visible = false;        

        //campos especiales        
        //CElblTit1.Visible = false;
        //CElistaDrop1.Visible = false;

        pnlObjetos.Visible = false;
        foreach (Control txt in this.pnlObjetos.Controls)
        {
            if (txt is TextBox)
            {
                ((TextBox)txt).Visible = false;
            }
            if (txt is CheckBox)
            {
                ((CheckBox)txt).Visible = false;
            }
            if (txt is Label)
            {
                ((Label)txt).Visible = false;
            }
            if (txt is DropDownList)
            {
                ((DropDownList)txt).Visible = false;
            }
        }

        //ocultamos botones
        btnCreaMensaje.Visible = false;
        //hacemos la busqueda de la persona

        lblGeneral.Visible = false;
        DatosExpediente.Visible = false;

        lblGeneralValidaciones.Text = "";
        lblGeneralValidaciones.Visible = false;
        //pnlDoctosActual.Visible = false;
        //creación de varaibles
        int totalControles;
        string where;
        //inicianilización de variables
        totalControles = 0;
        where = "";

        //validamos que al menos tenga dato en uno de los campos
        foreach (Control txt in this.pnlBuscar.Controls)
        {
            if (txt is TextBox)
            {
                //((TextBox)c).Enabled = false;
                if (((TextBox)txt).Text.ToString().Trim().Equals(""))
                {
                    totalControles++;
                }
            }
        }
        if (totalControles == 4)
        {
            lblGeneral.Text = "Debe ingresar al menos un criterio de búsqueda";
            lblGeneral.CssClass = "labelError";
            lblGeneral.Visible = true;
            return;
        }

        //armamos el where
        if (txtExpediente.Text.ToString().Trim().Length > 0)
        {
            where += " and xp.num_expediente like   '%" + txtExpediente.Text.ToString() + "%' ";
        }
		else if (txtPersonaContacto.Text.ToString().Trim().Length > 0)
        {
            where += " and xp.persona_contacto like   '%" + txtPersonaContacto.Text.ToString() + "%' ";
        }
		else
		{
			if (!listaCliente.SelectedValue.ToString().Equals("0"))
			{
				where += " and xp.id_cliente =  " + listaCliente.SelectedValue.ToString() + "  ";
			}
			else
			{
				if (!listaTipoServicio.SelectedValue.ToString().Equals("0"))
				{
					where += " and xp.id_tipo_servicio =  " + listaTipoServicio.SelectedValue.ToString() + "  ";
				}
				else
				{
					if (!listaNorma.SelectedValue.ToString().Equals("0"))
					{
						where += " and nrm.id_norma =  " + listaNorma.SelectedValue.ToString() + "  ";
					}
				}

			}
			
		}


		string[] datosExpediente = { where };
		
		    string errorSQL = Controladora.regresaSentenciaSQL(sqlExpediente.buscaExpediente, datosExpediente);
            string errorURL = HttpContext.Current.Request.Url.AbsolutePath.ToString();
            string errorIdent = "buscaExpediente";
            Utilerias.registraSQL(errorSQL, errorURL, errorIdent);
			
        //llenamos el grid
        
        DataTable tablaBuscar = Controladora.consultaDatos(sqlExpediente.buscaExpediente, datosExpediente);
        if (tablaBuscar.Rows.Count > 0)
        {
            if (tablaBuscar.Rows[0]["id_etapa"].ToString().Equals("0") || tablaBuscar.Rows[0]["id_etapa"].ToString().Trim().Equals(""))
            {
                lblGeneral.Text = "El expediente no cuenta con información para trabajarlo; cargue información con la confronta.";
                lblGeneral.CssClass = "labelError";
                lblGeneral.Visible = true;
            }
            else
            {
                gridResultadoBuscar.DataSource = tablaBuscar;
                gridResultadoBuscar.DataBind();
            }
        }
        else
        {
            lblGeneral.Text = "No se encontraron resultados con el criterio de búsqueda";
            lblGeneral.CssClass = "labelError";
            lblGeneral.Visible = true;
        }
    }

    //metodo para seleccionar un expediente
    protected void gridResultadoBuscar_SelectedIndexChanged(object sender, EventArgs e)
    {
        limpiaVariablesTramites();
        lblGeneral.Text = "";
        lblGeneral.Visible = false;
        lblGeneralDoctos.Text = "";
        lblGeneralDoctos.Visible = false;
        //campos especiales
        //pnlCamposEspecialesInicio.Visible = false;
        pnlMensaje.Visible = true;
        //CElblTit1.Visible = false;
        //CElistaDrop1.Visible = false;
        //pnlv5ftvSelFracc.Visible = false;
        ocultoV5ftvMuestraDoctos.Value = "0";

        //documentos
        gridDoctosEtapa.Visible = true;
        gridv5pvpDocumentos.Visible = false;
        //tramites
        gridTramitesActual.Visible = true;
        gridv5pvpTramites.Visible = false;

        GridViewRow row = gridResultadoBuscar.SelectedRow;
        int id = Convert.ToInt32(gridResultadoBuscar.DataKeys[row.RowIndex].Values["id_expediente"]);		
		        
        ocultoIdExpediente.Value = id.ToString();
        ocultoIdCliente.Value = gridResultadoBuscar.DataKeys[row.RowIndex].Values["id_cliente"].ToString();
		ocultoLigado.Value = gridResultadoBuscar.DataKeys[row.RowIndex].Values["id_estatus_ligado"].ToString();

        //SELECCIONAMOS LA INFORMACIÓN DEL EXPEDIENTE, PARA TENER LOS DATOS QUE VIENEN DE LA TABLA
        string[] v5ftvBuscaExpediente = { id.ToString() };
        DataTable tablaV5FTVExpediente = Controladora.consultaDatos(sqlExpediente.v5ftvMExpediente, v5ftvBuscaExpediente);

        ocultoIDetapa.Value = tablaV5FTVExpediente.Rows[0]["id_etapa"].ToString();

        ocultoIDetapa.Value = tablaV5FTVExpediente.Rows[0]["id_etapa"].ToString();
        ocultoIDflujo.Value = tablaV5FTVExpediente.Rows[0]["id_flujo"].ToString();

        ocultoIDsucursal.Value = tablaV5FTVExpediente.Rows[0]["id_sucursal"].ToString();
        ocultoIDempresaExpediente.Value = tablaV5FTVExpediente.Rows[0]["id_empresa"].ToString();

        Session.Add("numExpediente", tablaV5FTVExpediente.Rows[0]["num_expediente"].ToString());
		ocultoNumExpediente.Value = tablaV5FTVExpediente.Rows[0]["num_expediente"].ToString();
		

        if (ocultoIDetapa.Value.ToString().Equals("0") || ocultoIDetapa.Value.ToString().Trim().Equals(""))
        {// 1
            lblGeneral.Text = "El expediente no cuenta con información para trabajarlo; cargue información con la confronta.";
            lblGeneral.CssClass = "labelError";
            lblGeneral.Visible = true;
            DatosExpediente.Visible = false;
            pnlDocumentos.Visible = false;
            pnlTramites.Visible = false;
            pnlValidaciones.Visible = false;
            pnlTitulosEtapa.Visible = false;
            return;
        }
        else
        {// 1
            //validamos si el flujo y la empresa son de microcreditos
            //aqui podemos meter todas las validaciones que corresponda a la emrpesa 2
            //flujo 2 = microcreditos
            //empresa 2 = microcreditos



            //obtenemos el id de la siguiente etapa y lo guardamos en un hidden
            //idEtapaSiguiente(); lo quitamos pues genera error por el manejo de las condiciones

            //llenamos cada uno de los tabs
            DatosExpediente.Visible = true;
            pnlDocumentos.Visible = true;
            pnlTramites.Visible = true;
            pnlValidaciones.Visible = true;
            //llenatabas();

            //mostramos botones
            btnCreaMensaje.Visible = false;

            //ocultamos el grid principal de busqueda y mostramos el grid con la persona seleccionada
            string[] datosPersonaSeleccionada = { " AND xp.id_expediente = " + id.ToString() };
            gridPersonaSeleccionada.DataSource = Controladora.consultaDatos(sqlExpediente.buscaExpediente, datosPersonaSeleccionada);
            gridPersonaSeleccionada.DataBind();
            gridPersonaSeleccionada.Visible = true;
            gridResultadoBuscar.Visible = false;

            lblGeneralTitulo.Text = "etapa: " + ocultoIDetapa.Value.ToString() + "   flujo: " + ocultoIDflujo.Value.ToString() ;
            lblGeneralTitulo.Visible = true;

            #region titulos expediente
            titulosExpediente();

            #endregion


            #region Observaciones
            //<OBS>
            //mostramos las observaciones de la etapa
            HtmlControl htmlFrameObsEtapa = (HtmlControl)frameObsEtapa;
            htmlFrameObsEtapa.Attributes["src"] = "varios/frameObsEtapa.aspx?id_expediente=" + ocultoIdExpediente.Value.ToString() + "&id_tpo_obs=1&id_etapa=";
            //mostramos las obervaciones de los documentos
            HtmlControl htmlFrameObsDoctos = (HtmlControl)frameObsDoctos;
            htmlFrameObsDoctos.Attributes["src"] = "varios/frameObsEtapa.aspx?id_expediente=" + ocultoIdExpediente.Value.ToString() + "&id_tpo_obs=3&id_etapa=" + ocultoIDetapa.Value.ToString();
            //mostramos las observaciones del regreso a la etapa inicial
            HtmlControl htmlframeObsRegresoEtapaInicial = (HtmlControl)frameObsRegresoEtapaInicial;
            htmlframeObsRegresoEtapaInicial.Attributes["src"] = "varios/frameObsEtapa.aspx?id_expediente=" + ocultoIdExpediente.Value.ToString() + "&id_tpo_obs=4&id_etapa=";
            //mostramos las observaciones del regreso de etapa
            HtmlControl htmlframeObsRegresaEtapa = (HtmlControl)frameObsRegresaEtapa;
            htmlframeObsRegresaEtapa.Attributes["src"] = "varios/frameObsEtapa.aspx?id_expediente=" + ocultoIdExpediente.Value.ToString() + "&id_tpo_obs=2&id_etapa=";
            //</OBS>
            #endregion


            Session["sIdEtapa"] = ocultoIDetapa.Value.ToString();
            //creaControles();
            pnlTitulosEtapa.Visible = true;



            #region pnlCamposEspeciales SPEI
            //AQUÍ CONSULTAR EL ID_EMPRESA DEL expediente SELECCIONADO ope_crediticio ese id_empresa usarlo para hacer select a  RGL_PARAM_GPO y compararlo con id_empresa y OBTENER id_etapa, id_flujo de RGL_PARAM_GPO esos valores compararlos con ocultoIDflujo.Value.ToString() y ocultoIDetapa.Value.ToString()
            //ID_empresa del expediente
            string[] paramExpediente = { ocultoIdExpediente.Value.ToString() };

            //datos del expediente para saber su id_empresa
            DataTable datosExpediente = Controladora.consultaDatos(sqlExpediente.buscaEmpresa, paramExpediente);
            string idEmpresaExpediente = datosExpediente.Rows[0]["id_empresa"].ToString();


            #endregion            


            #region Permite la modificación de datos personales del expediente


            //validamos en que etapa estamos para mostrar los botones de modifica datos

            if (ocultoIDflujo.Value.ToString() == "2" && Session["idEmpresa"].ToString() == "2" && (Convert.ToInt16(ocultoIDetapa.Value.ToString()) > 19 && Convert.ToInt16(ocultoIDetapa.Value.ToString()) < 23))
            {
                btnCopeVivienda.Visible = true;
                btnCLaborales.Visible = true;
                btnCopePersona.Visible = true;
            }
            else
            {
                btnCopeVivienda.Visible = false;
                btnCLaborales.Visible = false;
                btnCopePersona.Visible = false;
            }


            #endregion

            #region Abre paneles de documentos y expedientes


            //MUESTRA DOCUMENTOS Y TRÁMITES CUANDO SE ABRE EL EXPEDIENTE
            pnlBtnDoctosActual.Visible = true;
            pnlTramitesBtnActual.Visible = true;

            //_v5ftv    ==================================================================================================================================================


            ocultoResultadoTramites.Value = "0";
            ocultoResultadoDocumentos.Value = "0";
            //btnAvanzaEtapa.Enabled = false;
            btnValidaTramitesEtapa.Enabled = true;
            btnValidaDoctosEtapa.Enabled = true;

            btnv5uv_alta.Visible = false;
            lblv5uv_alta.Visible = false;

            //string[] datosParamv5ftv = { "v5ftv", ocultoIDempresaExpediente.Value.ToString() };
            //DataTable tablaParamv5ftv = Controladora.consultaDatos(sqlExpediente.mModVistas, datosParamv5ftv);

            bool muestraDoctosNormal;
            muestraDoctosNormal = true;


			//no estan pidiendo el tipo de mercado
			llenaGridDoctosActual(ocultoIDetapa.Value.ToString());
			llenaGridTramites(gridTramitesActual, ocultoIDetapa.Value.ToString());
			ocultoV5ftvMuestraDoctos.Value = "0";
			
            #endregion



            #region pnlObjetos
            //GENERA LAS OPCIONES PARA LOS OBJETOS

            //llemos la tabla ope_objetos para traernos el id_objeto, pasándole por parámetro el id_expdiente y la etapa
            string[] datosMobjetos = { ocultoIdExpediente.Value.ToString(), ocultoIDetapa.Value.ToString() };
			/*
			string errorSQL = Controladora.regresaSentenciaSQL(sqlExpediente.mObjetos, datosMobjetos);
			string errorURL = HttpContext.Current.Request.Url.AbsolutePath.ToString();
			string errorIdent = "spLeerObjetos";
			Utilerias.registraSQL(errorSQL, errorURL, errorIdent);
			*/
			
            DataTable tablaMobjetos = Controladora.consultaDatos(sqlExpediente.mObjetos, datosMobjetos);
            if (tablaMobjetos.Rows.Count > 0)   //tablaMobjetos
            {// si tiene datos, mandamos a llamar la función que nos llena la tabla temporal de donde traemos los la información de los objetos
                for (int incTobj = 0; incTobj < tablaMobjetos.Rows.Count; incTobj++)
                {
                    string[] datosSPleerObjetos = { ocultoIdExpediente.Value.ToString(), tablaMobjetos.Rows[incTobj]["id_objeto"].ToString(), Session["idUsuario"].ToString() };
					/*
					 errorSQL = Controladora.regresaSentenciaSQL(sqlExpediente.spLeerObjetos, datosSPleerObjetos);
					 errorURL = HttpContext.Current.Request.Url.AbsolutePath.ToString();
					 errorIdent = "spLeerObjetos";
					Utilerias.registraSQL(errorSQL, errorURL, errorIdent);
					*/
                    DataTable tablaSPleerObjetos = Controladora.consultaDatos(sqlExpediente.spLeerObjetos, datosSPleerObjetos);
                }
                //ya se insertaron los objetos, leemos la tabla temproral
                string[] datosMtmpObjetos = { ocultoIdExpediente.Value.ToString(), ocultoIDetapa.Value.ToString() };
                DataTable tablaMtmpObjetos = Controladora.consultaDatos(sqlExpediente.mTmpObjetos, datosMtmpObjetos);

                //recorremos la tabla temporal de objetos para saber que controlos vamos a mostrar
                if (tablaMtmpObjetos.Rows.Count > 0)    //tntmpo
                {
                    //Debemos saber que control es el que encedemos, para eso, creamos un contador, el contador se va a incrementar
					//solo si entrar en el case que le corresponde
                    int contNum, contTxt, contFecha, contChk, contCombo;
                    //los declaramos en 1 porque tenemos los controles como 1
                    contNum = 1;
                    contTxt = 1;
                    contFecha = 1;
                    contChk = 1;
                    contCombo = 1;

                    //recorremos la tabla
                    for (int incTtmp = 0; incTtmp < tablaMtmpObjetos.Rows.Count; incTtmp++)
                    {   //for incTtmp
                        switch (tablaMtmpObjetos.Rows[incTtmp]["id_tipo_objeto"].ToString())
                        {
                            case "1":
                                //en un txt numérico
                                //validamos en que número de contador estamos para encender los controles
                                switch (contNum)
                                {
                                    case 1:
                                        objLblNUM1.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblNUM1.Visible = true;
                                        objTxtNUM1.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtNUM1.Visible = true;
                                        break;
                                    case 2:
                                        objLblNUM2.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblNUM2.Visible = true;
                                        objTxtNUM2.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtNUM2.Visible = true;
                                        break;
                                    case 3:
                                        objLblNUM3.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblNUM3.Visible = true;
                                        objTxtNUM3.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtNUM3.Visible = true;
                                        break;
                                }
                                contNum++;
                                break;
                            case "2":
                                //es un txt general
                                //validamos en que número de contador estamos para encender los controles
                                switch (contTxt)
                                {
                                    case 1:
                                        objLblTexto1.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto1.Visible = true;
                                        objTxtTexto1.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto1.Visible = true;
                                        break;
                                    case 2:
                                        objLblTexto2.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto2.Visible = true;
                                        objTxtTexto2.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto2.Visible = true;
                                        break;
                                    case 3:
                                        objLblTexto3.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto3.Visible = true;
                                        objTxtTexto3.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto3.Visible = true;
                                        break;
                                    case 4:
                                        objLblTexto4.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto4.Visible = true;
                                        objTxtTexto4.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto4.Visible = true;
                                        break;
                                    case 5:
                                        objLblTexto5.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto5.Visible = true;
                                        objTxtTexto5.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto5.Visible = true;
                                        break;
                                    case 6:
                                        objLblTexto6.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto6.Visible = true;
                                        objTxtTexto6.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto6.Visible = true;
                                        break;
                                    case 7:
                                        objLblTexto7.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto7.Visible = true;
                                        objTxtTexto7.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto7.Visible = true;
                                        break;
                                    case 8:
                                        objLblTexto8.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto8.Visible = true;
                                        objTxtTexto8.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto8.Visible = true;
                                        break;
                                    case 9:
                                        objLblTexto9.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto9.Visible = true;
                                        objTxtTexto9.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto9.Visible = true;
                                        break;
                                    case 10:
                                        objLblTexto10.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto10.Visible = true;
                                        objTxtTexto10.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto10.Visible = true;
                                        break;

                                }
                                contTxt++;
                                break;
                            case "3":
                                //es una fecha
                                //validamos en que número de contador estamos para encender los controles
                                switch (contFecha)
                                {
                                    case 1:
                                        objLblFecha1.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblFecha1.Visible = true;
										//Response.Write("fecha1");
										//Response.Write(tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString());
                                        objTxtFecha1.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
										//el objeto, tiene un valor vacio?
										if (!(objTxtFecha1.Text.ToString().Trim().Length > 0))
										{
											//revisa si. existe un valor predeterminado
											if (tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString().Trim().Length > 0)
											{
												//debe asignar un valor default? obtener el valor default
												string[] datosValorFecha1 = { tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString() };
												DataTable tablaValorFecha1 = Controladora.consultaDatos(sqlExpediente.DefaultFecha, datosValorFecha1);
												if (tablaValorFecha1.Rows.Count > 0)
												{
													objTxtFecha1.Text = tablaValorFecha1.Rows[0][0].ToString();
												}
											}
										}
                                        objTxtFecha1.Visible = true;
                                        break;
                                    case 2:
                                        objLblFecha2.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblFecha2.Visible = true;
										//Response.Write("fecha2");
										//Response.Write(tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString());										
                                        objTxtFecha2.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
										//el objeto, tiene un valor vacio?
										if (!(objTxtFecha2.Text.ToString().Trim().Length > 0))
										{
											//revisa si. existe un valor predeterminado
											if (tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString().Trim().Length > 0)
											{
												//el objeto, tiene un valor vacio? obtener el valor default
												string[] datosValorFecha2 = { tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString() };
												DataTable tablaValorFecha2 = Controladora.consultaDatos(sqlExpediente.DefaultFecha, datosValorFecha2);
												if (tablaValorFecha2.Rows.Count > 0)
												{
													objTxtFecha2.Text = tablaValorFecha2.Rows[0][0].ToString();
												}
											}
										}
                                        objTxtFecha2.Visible = true;
                                        break;
                                    case 3:
                                        objLblFecha3.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblFecha3.Visible = true;
										//Response.Write("fecha3");
										//Response.Write(tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString());
                                        objTxtFecha3.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
										//el objeto, tiene un valor vacio?
										if (!(objTxtFecha3.Text.ToString().Trim().Length > 0))
										{
											//revisa si. existe un valor predeterminado
											if (tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString().Trim().Length > 0)
											{
												//el objeto, tiene un valor vacio? obtener el valor default
												string[] datosValorFecha3 = { tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString() };
												DataTable tablaValorFecha3 = Controladora.consultaDatos(sqlExpediente.DefaultFecha, datosValorFecha3);
												if (tablaValorFecha3.Rows.Count > 0)
												{
													objTxtFecha3.Text = tablaValorFecha3.Rows[0][0].ToString();
												}
											}
										}										
                                        objTxtFecha3.Visible = true;
                                        break;
									case 4:
                                        objLblFecha4.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblFecha4.Visible = true;
										//Response.Write("fecha4");
										//Response.Write(tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString());
                                        objTxtFecha4.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
										//el objeto, tiene un valor vacio?
										if (!(objTxtFecha4.Text.ToString().Trim().Length > 0))
										{
											//revisa si. existe un valor predeterminado
											if (tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString().Trim().Length > 0)
											{
												//el objeto, tiene un valor vacio? obtener el valor default
												string[] datosValorFecha4 = { tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString() };
												DataTable tablaValorFecha4 = Controladora.consultaDatos(sqlExpediente.DefaultFecha, datosValorFecha4);
												if (tablaValorFecha4.Rows.Count > 0)
												{
													objTxtFecha4.Text = tablaValorFecha4.Rows[0][0].ToString();
												}
											}
										}										
                                        objTxtFecha4.Visible = true;
                                        break;	
									case 5:
                                        objLblFecha5.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblFecha5.Visible = true;
										//Response.Write("fecha5");
										//Response.Write(tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString());
                                        objTxtFecha5.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
										//el objeto, tiene un valor vacio?
										if (!(objTxtFecha5.Text.ToString().Trim().Length > 0))
										{
											//revisa si. existe un valor predeterminado
											if (tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString().Trim().Length > 0)
											{
												//el objeto, tiene un valor vacio? obtener el valor default
												string[] datosValorFecha5 = { tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString() };
												DataTable tablaValorFecha5 = Controladora.consultaDatos(sqlExpediente.DefaultFecha, datosValorFecha5);
												if (tablaValorFecha5.Rows.Count > 0)
												{
													objTxtFecha5.Text = tablaValorFecha5.Rows[0][0].ToString();
												}
											}
										}										
                                        objTxtFecha5.Visible = true;
                                        break;
									case 6:
                                        objLblFecha6.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblFecha6.Visible = true;
										//Response.Write("fecha6");
										//Response.Write(tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString());
                                        objTxtFecha6.Text = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
										//el objeto, tiene un valor vacio?
										if (!(objTxtFecha6.Text.ToString().Trim().Length > 0))
										{
											//revisa si. existe un valor predeterminado
											if (tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString().Trim().Length > 0)
											{
												//debe asignar un valor default? obtener el valor default
												string[] datosValorFecha6 = { tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString() };
												DataTable tablaValorFecha6 = Controladora.consultaDatos(sqlExpediente.DefaultFecha, datosValorFecha6);
												if (tablaValorFecha6.Rows.Count > 0)
												{
													objTxtFecha6.Text = tablaValorFecha6.Rows[0][0].ToString();
												}
											}
										}										
                                        objTxtFecha6.Visible = true;
                                        break;										
                                }
                                contFecha++;
                                break;
                            case "4":
                                //es un check
                                //validamos en que número de contador estamos para encender los controles
                                switch (contChk)
                                {
                                    case 1:
                                        objChkCheck1.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        if (tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString().Equals("true"))
                                        {
                                            objChkCheck1.Checked = true;
                                        }
                                        else
                                        {
                                            objChkCheck1.Checked = false;
                                        }
                                        objChkCheck1.Visible = true;
                                        break;
                                    case 2:
                                        objChkCheck2.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        if (tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString().Equals("true"))
                                        {
                                            objChkCheck2.Checked = true;
                                        }
                                        else
                                        {
                                            objChkCheck2.Checked = false;
                                        }
                                        objChkCheck2.Visible = true;
                                        break;
                                    case 3:
                                        objChkCheck3.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        if (tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString().Equals("true"))
                                        {
                                            objChkCheck3.Checked = true;
                                        }
                                        else
                                        {
                                            objChkCheck3.Checked = false;
                                        }
                                        objChkCheck3.Visible = true;
                                        break;
                                }
                                contChk++;
                                break;
                            case "5":
                                //es una lista
                                //validamos en que número de contador estamos para encender los controles
								ocultoValorCondicion.Value = "";
                                switch (contCombo)
                                {
                                    case 1:
                                        objLblCombo1.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblCombo1.Visible = true;
                                        //Obtenemos el valor del combo
                                        string[] sqlTablaOculta = { tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString() };
                                        Utilerias.llenaLista(objListaCombo1, sqlCamposTablas.sqlParametroTablaOculta, sqlTablaOculta, "txt", "id", "");
										//seleccionamos si trae un valor
                                        if (Convert.ToInt64(tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString()) > 0)
                                        {
                                            Utilerias.seleccionaValorComboRequest(objListaCombo1, tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString());
                                        }										
                                        objListaCombo1.Visible = true;
										//este objeto es una condicion para el flujo de la operacion?
										if (Convert.ToInt64(tablaMtmpObjetos.Rows[incTtmp]["condicion"].ToString()) > 0)
										{
											ocultoValorCondicion.Value = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
										}										
                                        break;
                                    case 2:
                                        objLblCombo2.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblCombo2.Visible = true;
                                        //Obtenemos el valor del combo
                                        string[] sqlTablaOculta2 = { tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString() };
                                        Utilerias.llenaLista(objListaCombo2, sqlCamposTablas.sqlParametroTablaOculta, sqlTablaOculta2, "txt", "id", "");
                                        //seleccionamos si trae un valor
                                        if (Convert.ToInt64(tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString()) > 0)
                                        {
                                            Utilerias.seleccionaValorComboRequest(objListaCombo2, tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString());
                                        }
                                        objListaCombo2.Visible = true;
										//este objeto es una condicion para el flujo de la operacion?
										if (Convert.ToInt64(tablaMtmpObjetos.Rows[incTtmp]["condicion"].ToString()) > 0)
										{
											ocultoValorCondicion.Value = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
										}										
                                        break;
                                    case 3:
                                        objLblCombo3.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblCombo3.Visible = true;
                                        //Obtenemos el valor del combo
                                        string[] sqlTablaOculta3 = { tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString() };
                                        Utilerias.llenaLista(objListaCombo3, sqlCamposTablas.sqlParametroTablaOculta, sqlTablaOculta3, "txt", "id", "");
                                        //seleccionamos si trae un valor
                                        if (Convert.ToInt64(tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString()) > 0)
                                        {
                                            Utilerias.seleccionaValorComboRequest(objListaCombo3, tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString());
                                        }
                                        objListaCombo3.Visible = true;
										//este objeto es una condicion para el flujo de la operacion?
										if (Convert.ToInt64(tablaMtmpObjetos.Rows[incTtmp]["condicion"].ToString()) > 0)
										{
											ocultoValorCondicion.Value = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
										}
                                        break;
									case 4:
                                        objLblCombo4.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblCombo4.Visible = true;
                                        //Obtenemos el valor del combo
                                        string[] sqlTablaOculta4 = { tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString() };
                                        Utilerias.llenaLista(objListaCombo4, sqlCamposTablas.sqlParametroTablaOculta, sqlTablaOculta4, "txt", "id", "");
                                        //seleccionamos si trae un valor
                                        if (Convert.ToInt64(tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString()) > 0)
                                        {
                                            Utilerias.seleccionaValorComboRequest(objListaCombo4, tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString());
                                        }
                                        objListaCombo4.Visible = true;
										//este objeto es una condicion para el flujo de la operacion?
										if (Convert.ToInt64(tablaMtmpObjetos.Rows[incTtmp]["condicion"].ToString()) > 0)
										{
											ocultoValorCondicion.Value = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
										}
                                        break;
									case 5:
                                        objLblCombo5.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblCombo5.Visible = true;
                                        //Obtenemos el valor del combo
                                        string[] sqlTablaOculta5 = { tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString() };
                                        Utilerias.llenaLista(objListaCombo5, sqlCamposTablas.sqlParametroTablaOculta, sqlTablaOculta5, "txt", "id", "");
                                        //seleccionamos si trae un valor
                                        if (Convert.ToInt64(tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString()) > 0)
                                        {
                                            Utilerias.seleccionaValorComboRequest(objListaCombo5, tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString());
                                        }
                                        objListaCombo5.Visible = true;
										//este objeto es una condicion para el flujo de la operacion?
										if (Convert.ToInt64(tablaMtmpObjetos.Rows[incTtmp]["condicion"].ToString()) > 0)
										{
											ocultoValorCondicion.Value = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
										}
                                        break;
									case 6:
                                        objLblCombo6.Text = tablaMtmpObjetos.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblCombo6.Visible = true;
                                        //Obtenemos el valor del combo
                                        string[] sqlTablaOculta6 = { tablaMtmpObjetos.Rows[incTtmp]["query_contenido"].ToString() };
                                        Utilerias.llenaLista(objListaCombo6, sqlCamposTablas.sqlParametroTablaOculta, sqlTablaOculta6, "txt", "id", "");
                                        //seleccionamos si trae un valor
                                        if (Convert.ToInt64(tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString()) > 0)
                                        {
                                            Utilerias.seleccionaValorComboRequest(objListaCombo5, tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString());
                                        }
                                        objListaCombo6.Visible = true;
										//este objeto es una condicion para el flujo de la operacion?
										if (Convert.ToInt64(tablaMtmpObjetos.Rows[incTtmp]["condicion"].ToString()) > 0)
										{
											ocultoValorCondicion.Value = tablaMtmpObjetos.Rows[incTtmp]["valor_objeto"].ToString();
										}
                                        break;
                                }
                                contCombo++;
                                break;
                        }   //switch
                    }   //for incTtmp
                }   //tntmpo
                pnlObjetos.Visible = true;
				//pnlProcesos.Visible = true;
				validaProcesoVisible(ocultoIdExpediente.Value.ToString());
            }   //tablaMobjetos cuando si tiene datos


            #endregion

        }
		
		llenaTabs();
		
		// valida si es expediente dependiente o independiente
		if (ocultoLigado.Value.ToString().Equals("1"))
		{
			//informa que no se deben hacer cambios en este expediente
			
			//deshabilita los botones que modifican informacion
			btnCapturaCamposEspeciales.Enabled = false;
			btnProcesoEtapa.Enabled = false;
			btnDoctosEtapaAnterior.Enabled = false;
			btnDoctosEtapaActual.Enabled = false;
			btnRegresaEtapa.Enabled = false;
			btnTramitesEtapaAnterior.Enabled = false;
			btnTranitesEtapaActual.Enabled = false;
			btnRegresaExpInicioEtp.Enabled = false;
			btnRegresaEtpAnterior.Enabled = false;
			btnAvanzaEtapa.Enabled = false;
			
		}
		
	
	
    }

    private void titulosExpediente()
    {
        string titEtapa, titFase, titEtapaFovi;
        titEtapa = string.Empty;
        titFase = string.Empty;
        titEtapaFovi = string.Empty;
        //mostramos los titulos de las etapas y fases
        string[] datosTitEtapa = { ocultoIDetapa.Value.ToString() };
        DataTable tablaTitEtapa = Controladora.consultaDatos(sqlExpediente.mTitEtapa, datosTitEtapa);
        if (tablaTitEtapa.Rows.Count > 0)
        {
            titEtapa = tablaTitEtapa.Rows[0][1].ToString();
        }
        else
        {
            titEtapa = "No existen dato";
        }


        //mostramos los titulos del expediente
        TableRow trEncabezado = new TableRow();
        TableCell tdColumna = new TableCell();
        tdColumna.ColumnSpan = 2;
        tdColumna.Attributes.Add("bgColor", "#5D7B9D");
        tdColumna.Attributes.Add("class", "txtTituloModal");
        tdColumna.Text = "Datos expediente";
        trEncabezado.Cells.Add(tdColumna);
        htmlTablaExpediente.Rows.Add(trEncabezado);


        string[] datosMSeleccionVista = { ocultoIDempresaExpediente.Value.ToString(), "resumen" };
        DataTable tablaSeleccionVista = Controladora.consultaDatos(sqlExpediente.mSeleccionVista, datosMSeleccionVista);

        string[] datosMtitExpediente = { tablaSeleccionVista.Rows[0]["vista"].ToString(), ocultoIdExpediente.Value.ToString() };
        DataTable tablaMtitExpediente = Controladora.consultaDatos(sqlExpediente.mDatosVista, datosMtitExpediente);

        //string[] datosMtitExpediente = { ocultoIdExpediente.Value.ToString() };
        //DataTable tablaMtitExpediente = Controladora.consultaDatos(sqlExpediente.mTitExpediente, datosMtitExpediente);


        for (int i = 0; i < tablaMtitExpediente.Columns.Count; i++)
        {
            TableRow renglon = new TableRow();

            TableCell colTitulo = new TableCell();
            TableCell colDatos = new TableCell();

            colTitulo.Attributes.Add("align", "left");
            //                colTitulo.Attributes.Add("bgcolor", hshParam["colorTD"].ToString());
            colTitulo.Text = tablaMtitExpediente.Columns[i].ColumnName.ToString();
            colDatos.Attributes.Add("align", "left");
            colDatos.Text = tablaMtitExpediente.Rows[0][i].ToString();

            //insert cells into a row
            renglon.Cells.Add(colTitulo);
            renglon.Cells.Add(colDatos);

            //insert row into a table
            htmlTablaExpediente.Rows.Add(renglon);
        }
    }

    //protected override void LoadViewState(object savedState)
    //{
    //    base.LoadViewState(savedState);
    //    //if (ViewState["controsladded"] == null)
    //        //creaControles();
    //        //CreaCamposModificaTABS();
    //}

    //private void creaControles()
    //{
    //    #region camposEspeciales
    //    Hashtable hshParam = (Hashtable)Session["sParametros"];
    //    //validamos si tenemos campos especiales
    //    //string[] datosCampoEspecial = { ocultoIDetapa.Value.ToString() };
    //    string[] datosCampoEspecial = { ocultoIDetapa.Value.ToString() };
    //    DataTable tablaCamposEspeciales = Controladora.consultaDatos(sqlExpediente.mCamposEspeciales, datosCampoEspecial);
    //    //validamos si traemos campos para mostrar la información
    //    if (tablaCamposEspeciales.Rows.Count > 0)
    //    {
    //        TableRow trEncabezado = new TableRow();
    //        TableCell tdColumna = new TableCell();
    //        tdColumna.ColumnSpan = 2;
    //        tdColumna.Attributes.Add("bgColor", "#5D7B9D");
    //        tdColumna.Attributes.Add("class", "txtTituloModal");
    //        tdColumna.Text = "Captura información Especial";

    //        trEncabezado.Cells.Add(tdColumna);
    //        htmlTablaCamposEspeciales.Rows.Add(trEncabezado);

    //        //tenemos que insertar el número de registros que vengan de la tabla
    //        for (int inc = 0; inc < tablaCamposEspeciales.Rows.Count; inc++)
    //        {
    //            TableRow tr = new TableRow();
    //            TableCell tdCampo = new TableCell();
    //            TableCell tdValor = new TableCell();
    //            tdCampo.Text = tablaCamposEspeciales.Rows[inc]["nombre"].ToString();

    //            TextBox txtDinamico = new TextBox();
    //            txtDinamico.ID = "n_" + tablaCamposEspeciales.Rows[inc]["id_regla"].ToString();
    //            tdValor.Controls.Add(txtDinamico);
    //            tdValor.Controls.Add(new LiteralControl(" "));

    //            tr.Cells.Add(tdCampo);
    //            tr.Cells.Add(tdValor);
    //            ViewState["controlsadded"] = true;
    //            htmlTablaCamposEspeciales.Rows.Add(tr);
    //        }
    //        pnlCamposEspeciales.Visible = true;
    //    }
    //    #endregion
    //}


    #region llenaListas
    //iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii
    private void listav5uvUnidadValuacion_Fill()
    {
        string[] datosUnidadValuacion = { string.Empty };
        Utilerias.llenaLista(listav5uvUnidadValuacion, sqlConstantes.listaUnidadValuacion, datosUnidadValuacion, "txt", "id", "");
    }
    private void listav5uvNotario_Fill()
    {
        string[] datosNotario = { string.Empty };
        Utilerias.llenaLista(listav5uvNotario, sqlConstantes.listaNotarios, datosNotario, "txt", "id", "");
    }
    //ttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt
    #endregion

    //llenamos los tabs
    #region Tabs
    private void llenaCotizacion()
    {

        //buscamos la vista dependiendo de la empresa y la pestaña a la que queremos consultar
        string[] datosMSeleccionVista = { ocultoIDempresaExpediente.Value.ToString(), "cotizacion" };
        DataTable tablaSeleccionVista = Controladora.consultaDatos(sqlExpediente.mSeleccionVista, datosMSeleccionVista);

        //con los datos del query anterior obtenemos el nombre de la vista
        string[] datosDatosVista = { tablaSeleccionVista.Rows[0]["vista"].ToString(), ocultoIdExpediente.Value.ToString() };
        DataTable tablaDatosVista = Controladora.consultaDatos(sqlExpediente.mDatosVista, datosDatosVista);

        generaTablaHTML(tablaDatosVista, "Información de COTIZACION", htmlTablaGuardaValores, tablaSeleccionVista.Rows[0]["vista"].ToString());
    }
    private void llenaPreliminar()
    {
        //buscamos la vista dependiendo de la empresa y la pestaña a la que queremos consultar
        string[] datosMSeleccionVista = { ocultoIDempresaExpediente.Value.ToString(), "preliminares" };
        DataTable tablaSeleccionVista = Controladora.consultaDatos(sqlExpediente.mSeleccionVista, datosMSeleccionVista);

        //con los datos del query anterior obtenemos el nombre de la vista
        string[] datosDatosVista = { tablaSeleccionVista.Rows[0]["vista"].ToString(), ocultoIdExpediente.Value.ToString() };
        DataTable tablaDatosVista = Controladora.consultaDatos(sqlExpediente.mDatosVista, datosDatosVista);

        generaTablaHTML(tablaDatosVista, "Información PRELIMINARES", htmlTablaNotario, tablaSeleccionVista.Rows[0]["vista"].ToString());
    }
    private void llenaGestion()
    {
        //buscamos la vista dependiendo de la empresa y la pestaña a la que queremos consultar
        string[] datosMSeleccionVista = { ocultoIDempresaExpediente.Value.ToString(), "gestion" };
        DataTable tablaSeleccionVista = Controladora.consultaDatos(sqlExpediente.mSeleccionVista, datosMSeleccionVista);

        //con los datos del query anterior obtenemos el nombre de la vista
        string[] datosDatosVista = { tablaSeleccionVista.Rows[0]["vista"].ToString(), ocultoIdExpediente.Value.ToString() };
        DataTable tablaDatosVista = Controladora.consultaDatos(sqlExpediente.mDatosVista, datosDatosVista);

        generaTablaHTML(tablaDatosVista, "Información de GESTION", htmlTablaVivienda, tablaSeleccionVista.Rows[0]["vista"].ToString());
    }
    private void llenaCertificado()
    {
        //buscamos la vista dependiendo de la empresa y la pestaña a la que queremos consultar
        string[] datosMSeleccionVista = { ocultoIDempresaExpediente.Value.ToString(), "certificado" };
        DataTable tablaSeleccionVista = Controladora.consultaDatos(sqlExpediente.mSeleccionVista, datosMSeleccionVista);

        //con los datos del query anterior obtenemos el nombre de la vista
        string[] datosDatosVista = { tablaSeleccionVista.Rows[0]["vista"].ToString(), ocultoIdExpediente.Value.ToString() };
        DataTable tablaDatosVista = Controladora.consultaDatos(sqlExpediente.mDatosVista, datosDatosVista);

        generaTablaHTML(tablaDatosVista, "Información de CERTIFICADO", htmlTablaLaborales, tablaSeleccionVista.Rows[0]["vista"].ToString());
    }
	
    private void llenaMuestreo()
    {
        //buscamos la vista dependiendo de la empresa y la pestaña a la que queremos consultar
        string[] datosMSeleccionVista = { ocultoIDempresaExpediente.Value.ToString(), "muestreo" };
        DataTable tablaSeleccionVista = Controladora.consultaDatos(sqlExpediente.mSeleccionVista, datosMSeleccionVista);

        //con los datos del query anterior obtenemos el nombre de la vista
        string[] datosDatosVista = { tablaSeleccionVista.Rows[0]["vista"].ToString(), ocultoIdExpediente.Value.ToString() };
        DataTable tablaDatosVista = Controladora.consultaDatos(sqlExpediente.mDatosVista, datosDatosVista);

        generaTablaHTML(tablaDatosVista, "Información de MUESTREO", htmlTablaCrediticio, tablaSeleccionVista.Rows[0]["vista"].ToString());
    }
	
    private void llenaCliente()
    {

        //buscamos la vista dependiendo de la empresa y la pestaña a la que queremos consultar
        string[] datosMSeleccionVista = { ocultoIDempresaExpediente.Value.ToString(), "cliente" };
        DataTable tablaSeleccionVista = Controladora.consultaDatos(sqlExpediente.mSeleccionVista, datosMSeleccionVista);

        //con los datos del query anterior obtenemos el nombre de la vista
        string[] datosDatosVista = { tablaSeleccionVista.Rows[0]["vista"].ToString(), ocultoIdExpediente.Value.ToString() };
        DataTable tablaDatosVista = Controladora.consultaDatos(sqlExpediente.mDatosVista, datosDatosVista);

        generaTablaHTML(tablaDatosVista, "Información de CLIENTE", htmlTablaPersona, tablaSeleccionVista.Rows[0]["vista"].ToString());
    }

	private void llenaNorma()
    {
        //buscamos la vista dependiendo de la empresa y la pestaña a la que queremos consultar
        string[] datosMSeleccionVista = { ocultoIDempresaExpediente.Value.ToString(), "norma" };
        DataTable tablaSeleccionVista = Controladora.consultaDatos(sqlExpediente.mSeleccionVista, datosMSeleccionVista);

        //con los datos del query anterior obtenemos el nombre de la vista
        string[] datosDatosVista = { tablaSeleccionVista.Rows[0]["vista"].ToString(), ocultoIdExpediente.Value.ToString() };
        DataTable tablaDatosVista = Controladora.consultaDatos(sqlExpediente.mDatosVista, datosDatosVista);

        generaTablaHTML(tablaDatosVista, "Información de NORMA", htmlTablaNorma, tablaSeleccionVista.Rows[0]["vista"].ToString());
    }

	
	private void llenaNormasFamilias()
    {        
		
		string[] datosVista = { ocultoIdExpediente.Value.ToString() };   
				                    		
		gridNormasFamilias.DataSource = Controladora.consultaDatos(sqlExpediente.NormasFamilias, datosVista);
		gridNormasFamilias.DataBind();
		gridNormasFamilias.Visible = true;		
		pnlWinNormasFamilias.Visible = true;
		        
    }

	private void llenaFacturasProyecto()
    {        
		
		string[] datosVista = { ocultoIdExpediente.Value.ToString() };   
				                    		
		gridFacturasProyecto.DataSource = Controladora.consultaDatos(sqlExpediente.facturasProyecto, datosVista);
		gridFacturasProyecto.DataBind();
		gridFacturasProyecto.Visible = true;		
		pnlWinFacturasProyecto.Visible = true;
		        
    }
	
	private void llenaCertificadosProyecto()
    {        
		
		string[] datosVista = { ocultoIdExpediente.Value.ToString() };   
				                    		
		gridCertificadosProyecto.DataSource = Controladora.consultaDatos(sqlCertificados.muestraCertificados, datosVista);
		gridCertificadosProyecto.DataBind();
		gridCertificadosProyecto.Visible = true;		
		pnlWinCertificadosProyecto.Visible = true;
		        
    }
	
    //genera la tabla html de forma dinámica para los tabs
    private void generaTablaHTML(DataTable pDataTable, string pTituloTabla, Table pNombreTabla, string pVista)
    {
        //en el título hago un split para saber cual es el titulo y hace el case para mostrar el boton
        #region split
        string[] tituloSplit;
        tituloSplit = pTituloTabla.Split(new char[] { ' ' });

        int tamanioSplit;
        tamanioSplit = tituloSplit.Length - 1;

        string nombreTabla;
        nombreTabla = tituloSplit[tamanioSplit].ToString();
        #endregion


        Hashtable hshParam = (Hashtable)Session["sParametros"];
        TableRow encabezado = new TableRow();
        TableCell columna = new TableCell();
        columna.ColumnSpan = 2;
        columna.Attributes.Add("bgColor", hshParam["colorTD"].ToString());
        columna.CssClass = "colotTextoTitulo";
        columna.Text = pTituloTabla;

        encabezado.Cells.Add(columna);

        //Add header to a table
        pNombreTabla.Rows.Add(encabezado);
        //htmlTablaPersona.Rows.Add(encabezado);

        //EN ESTA  PARTE SOLO MUESTRA UN REGISTRO, PORQUE SOLO RECORRE LAS COLUMNAS, HACE FALTA RECORRER LOS REGISTROS
        string ocultaID;
        int valorID, contador;
        ocultaID = string.Empty;
        valorID = 0;
        contador = 0;
        if (pDataTable.Rows.Count > 0)
        {
            for (int i = 0; i < pDataTable.Columns.Count; i++)
            {
                if (pDataTable.Columns[i].ColumnName.ToString().IndexOf("id_") < 0)
                {
                    TableRow renglon = new TableRow();

                    TableCell colTitulo = new TableCell();
                    TableCell colDatos = new TableCell();

                    colTitulo.Attributes.Add("align", "left");
                    colTitulo.Attributes.Add("bgcolor", hshParam["colorTD"].ToString());
                    colTitulo.CssClass = "colotTextoTitulo";

                    colTitulo.Text = pDataTable.Columns[i].ColumnName.ToString().Replace("_", " ");

                    colDatos.Attributes.Add("align", "left");
                    colDatos.Text = pDataTable.Rows[0][i].ToString();


                    //insert cells into a row
                    renglon.Cells.Add(colTitulo);
                    renglon.Cells.Add(colDatos);

                    //insert row into a table
                    pNombreTabla.Rows.Add(renglon);
                    contador++;
                }
            }
            if (contador == 0)
            {
                TableRow trFin = new TableRow();
                TableCell tdFin = new TableCell();
                tdFin.ColumnSpan = 2;
                tdFin.Attributes.Add("bgColor", hshParam["colorTD"].ToString());
                tdFin.Text = "No existe información";

                trFin.Cells.Add(tdFin);

                //Add header to a table
                pNombreTabla.Rows.Add(trFin);
            }
            if (contador > 0)
            {
                switch (nombreTabla)
                {
                    //LA MODIFICACIÓN DE LA VISTA ESTA CONTROLADA POR LA TABLA PARAM Y TIENE UN CAMPO POR CADA UNO DE LOS BOTONES DE MODIFICACIÓN.
                    case "VALORES":
                        //validamos si permite la modifcación de la vista
                        string[] datosValores = { "mod_valores", ocultoIDempresaExpediente.Value.ToString() };
                        DataTable tablaModVistas = Controladora.consultaDatos(sqlExpediente.mModVistas, datosValores);
                        if (tablaModVistas.Rows[0][0].ToString().Equals("1"))
                            btnModGuardaValores.Visible = true;
                        else
                            btnModGuardaValores.Visible = false;
                        break;
                    case "NOTARIO":
                        //validamos si permite la modifcación de la vista
                        string[] datosNotarios = { "mod_notario", ocultoIDempresaExpediente.Value.ToString() };
                        DataTable tablaModNotarios = Controladora.consultaDatos(sqlExpediente.mModVistas, datosNotarios);
                        if (tablaModNotarios.Rows[0][0].ToString().Equals("1"))
                            btnModNotario.Visible = true;
                        else
                            btnModNotario.Visible = false;
                        break;
                    case "VIVIENDA":
                        //validamos si permite la modifcación de la vista
                        string[] datosVivienda = { "mod_vivienda", ocultoIDempresaExpediente.Value.ToString() };
                        DataTable tablaModVivienda = Controladora.consultaDatos(sqlExpediente.mModVistas, datosVivienda);
                        if (tablaModVivienda.Rows[0][0].ToString().Equals("1"))
                            btnModVivienda.Visible = true;
                        else
                            btnModVivienda.Visible = false;
                        break;
                    case "LABORALES":
                        //validamos si permite la modifcación de la vista
                        string[] datosLaborales = { "mod_laborales", ocultoIDempresaExpediente.Value.ToString() };
                        DataTable tablaModLaborales = Controladora.consultaDatos(sqlExpediente.mModVistas, datosLaborales);
                        if (tablaModLaborales.Rows[0][0].ToString().Equals("1"))
                            btnModLaborales.Visible = true;
                        else
                            btnModLaborales.Visible = false;
                        break;
                    case "CREDITICIO":
                        //validamos si permite la modifcación de la vista
                        string[] datosCrediticio = { "mod_crediticio", ocultoIDempresaExpediente.Value.ToString() };
                        DataTable tablaModCrediticio = Controladora.consultaDatos(sqlExpediente.mModVistas, datosCrediticio);
                        if (tablaModCrediticio.Rows[0][0].ToString().Equals("1"))
                            btnModCrediticio.Visible = true;
                        else
                            btnModCrediticio.Visible = false;
                        break;
                    case "PERSONA":
                        //validamos si permite la modifcación de la vista
                        string[] datosPersona = { "mod_persona", ocultoIDempresaExpediente.Value.ToString() };
                        DataTable tablaModPersona = Controladora.consultaDatos(sqlExpediente.mModVistas, datosPersona);
                        if (tablaModPersona.Rows[0][0].ToString().Equals("1"))
                            btnModPersona.Visible = true;
                        else
                            btnModPersona.Visible = false;
                        break;
                }
            }
        }
        else
        {
            TableRow trFin = new TableRow();
            TableCell tdFin = new TableCell();
            tdFin.ColumnSpan = 2;
            tdFin.Attributes.Add("bgColor", hshParam["colorTD"].ToString());
            tdFin.Text = "No existe información";

            trFin.Cells.Add(tdFin);

            //Add header to a table
            pNombreTabla.Rows.Add(trFin);
        }
    }

    private void CreaCamposModificaTABS()
    {
        //obtengo los datos de la vista para trerme los campos y generalos dinámicamente
        string[] datosDatosVista = { ocultoNombreVista.Value.ToString(), ocultoIdExpediente.Value.ToString() };
        DataTable tablaDatosVista = Controladora.consultaDatos(sqlExpediente.mDatosVista, datosDatosVista);

        if (tablaDatosVista.Rows.Count > 0)
        {
            for (int i = 0; i < tablaDatosVista.Columns.Count; i++)
            {
                if (tablaDatosVista.Columns[i].ColumnName.ToString().IndexOf("id_") < 0)
                {
                    TextBox txtDinamico = new TextBox();
                    txtDinamico.ID = tablaDatosVista.Rows[0][i].ToString();
                    EnsureChildControls();
                    this.pnlTABSvntn.Controls.Add(txtDinamico);
                    ViewState["controlsadded"] = true;
                    ViewState.Add("1", txtDinamico.Text);
                }
            }
        }
    }

    //MUESTRA LA VENTANA PARA MODIFICAR LOS DATOS DEL GUARDA VALORES
    protected void btnModGuardaValores_Click(object sender, EventArgs e)
    {
        modalTABS.Show();
        //llenatabas();
        titulosExpediente();
        //buscamos la vista dependiendo de la empresa y la pestaña a la que queremos consultar
        string[] datosMSeleccionVista = { ocultoIDempresaExpediente.Value.ToString(), "guarda_valores" };
        DataTable tablaSeleccionVista = Controladora.consultaDatos(sqlExpediente.mSeleccionVista, datosMSeleccionVista);

        string tablaOriginal = tablaSeleccionVista.Rows[0]["tabla_original"].ToString();

        //con los datos del query anterior obtenemos el nombre de la vista
        string[] datosDatosVista = { tablaSeleccionVista.Rows[0]["vista"].ToString(), ocultoIdExpediente.Value.ToString() };
        DataTable tablaDatosVista = Controladora.consultaDatos(sqlExpediente.mDatosVista, datosDatosVista);

        int NumColumnas = tablaDatosVista.Columns.Count;

        generaIFrame(tablaSeleccionVista.Rows[0]["vista"].ToString(), "Guarda Valores", NumColumnas * 50, tablaOriginal);

    }
    protected void btnModNotario_Click(object sender, EventArgs e)
    {
        modalTABS.Show();
        //llenatabas();
        titulosExpediente();
        //buscamos la vista dependiendo de la empresa y la pestaña a la que queremos consultar
        string[] datosMSeleccionVista = { ocultoIDempresaExpediente.Value.ToString(), "notario" };
        DataTable tablaSeleccionVista = Controladora.consultaDatos(sqlExpediente.mSeleccionVista, datosMSeleccionVista);

        string tablaOriginal = tablaSeleccionVista.Rows[0]["tabla_original"].ToString();

        //con los datos del query anterior obtenemos el nombre de la vista
        string[] datosDatosVista = { tablaSeleccionVista.Rows[0]["vista"].ToString(), ocultoIdExpediente.Value.ToString() };
        DataTable tablaDatosVista = Controladora.consultaDatos(sqlExpediente.mDatosVista, datosDatosVista);

        int NumColumnas = tablaDatosVista.Columns.Count;

        generaIFrame(tablaSeleccionVista.Rows[0]["vista"].ToString(), "Notario", NumColumnas * 50, tablaOriginal);
    }
    protected void btnModVivienda_Click(object sender, EventArgs e)
    {
        modalTABS.Show();
        //llenatabas();
        titulosExpediente();
        //buscamos la vista dependiendo de la empresa y la pestaña a la que queremos consultar
        string[] datosMSeleccionVista = { ocultoIDempresaExpediente.Value.ToString(), "vivienda" };
        DataTable tablaSeleccionVista = Controladora.consultaDatos(sqlExpediente.mSeleccionVista, datosMSeleccionVista);

        string tablaOriginal = tablaSeleccionVista.Rows[0]["tabla_original"].ToString();

        //con los datos del query anterior obtenemos el nombre de la vista
        string[] datosDatosVista = { tablaSeleccionVista.Rows[0]["vista"].ToString(), ocultoIdExpediente.Value.ToString() };
        DataTable tablaDatosVista = Controladora.consultaDatos(sqlExpediente.mDatosVista, datosDatosVista);

        int NumColumnas = tablaDatosVista.Columns.Count;

        generaIFrame(tablaSeleccionVista.Rows[0]["vista"].ToString(), "Vivienda", NumColumnas * 30, tablaOriginal);
    }
    protected void btnModLaborales_Click(object sender, EventArgs e)
    {
        modalTABS.Show();
        //llenatabas();
        titulosExpediente();
        //buscamos la vista dependiendo de la empresa y la pestaña a la que queremos consultar
        string[] datosMSeleccionVista = { ocultoIDempresaExpediente.Value.ToString(), "laborales" };
        DataTable tablaSeleccionVista = Controladora.consultaDatos(sqlExpediente.mSeleccionVista, datosMSeleccionVista);

        string tablaOriginal = tablaSeleccionVista.Rows[0]["tabla_original"].ToString();

        //con los datos del query anterior obtenemos el nombre de la vista
        string[] datosDatosVista = { tablaSeleccionVista.Rows[0]["vista"].ToString(), ocultoIdExpediente.Value.ToString() };
        DataTable tablaDatosVista = Controladora.consultaDatos(sqlExpediente.mDatosVista, datosDatosVista);

        int NumColumnas = tablaDatosVista.Columns.Count;

        generaIFrame(tablaSeleccionVista.Rows[0]["vista"].ToString(), "Laborales", NumColumnas * 50, tablaOriginal);
    }
    protected void btnModCrediticio_Click(object sender, EventArgs e)
    {
        modalTABS.Show();
        //llenatabas();
        titulosExpediente();
        //buscamos la vista dependiendo de la empresa y la pestaña a la que queremos consultar
        string[] datosMSeleccionVista = { ocultoIDempresaExpediente.Value.ToString(), "crediticio" };
        DataTable tablaSeleccionVista = Controladora.consultaDatos(sqlExpediente.mSeleccionVista, datosMSeleccionVista);

        string tablaOriginal = tablaSeleccionVista.Rows[0]["tabla_original"].ToString();

        //con los datos del query anterior obtenemos el nombre de la vista
        string[] datosDatosVista = { tablaSeleccionVista.Rows[0]["vista"].ToString(), ocultoIdExpediente.Value.ToString() };
        DataTable tablaDatosVista = Controladora.consultaDatos(sqlExpediente.mDatosVista, datosDatosVista);

        int NumColumnas = tablaDatosVista.Columns.Count;

        generaIFrame(tablaSeleccionVista.Rows[0]["vista"].ToString(), "Crediticio", NumColumnas * 50, tablaOriginal);
    }
    protected void btnModPersona_Click(object sender, EventArgs e)
    {
        modalTABS.Show();
        //llenatabas();
        titulosExpediente();
        //buscamos la vista dependiendo de la empresa y la pestaña a la que queremos consultar
        string[] datosMSeleccionVista = { ocultoIDempresaExpediente.Value.ToString(), "persona" };
        DataTable tablaSeleccionVista = Controladora.consultaDatos(sqlExpediente.mSeleccionVista, datosMSeleccionVista);

        string tablaOriginal = tablaSeleccionVista.Rows[0]["tabla_original"].ToString();

        //con los datos del query anterior obtenemos el nombre de la vista
        string[] datosDatosVista = { tablaSeleccionVista.Rows[0]["vista"].ToString(), ocultoIdExpediente.Value.ToString() };
        DataTable tablaDatosVista = Controladora.consultaDatos(sqlExpediente.mDatosVista, datosDatosVista);

        int NumColumnas = tablaDatosVista.Columns.Count;

        generaIFrame(tablaSeleccionVista.Rows[0]["vista"].ToString(), "Persona", NumColumnas * 50, tablaOriginal);
    }

	protected void btnModNorma_Click(object sender, EventArgs e)
    {
        modalTABS.Show();
        //llenatabas();
        titulosExpediente();
        //buscamos la vista dependiendo de la empresa y la pestaña a la que queremos consultar
        string[] datosMSeleccionVista = { ocultoIDempresaExpediente.Value.ToString(), "norma" };
        DataTable tablaSeleccionVista = Controladora.consultaDatos(sqlExpediente.mSeleccionVista, datosMSeleccionVista);

        string tablaOriginal = tablaSeleccionVista.Rows[0]["tabla_original"].ToString();

        //con los datos del query anterior obtenemos el nombre de la vista
        string[] datosDatosVista = { tablaSeleccionVista.Rows[0]["vista"].ToString(), ocultoIdExpediente.Value.ToString() };
        DataTable tablaDatosVista = Controladora.consultaDatos(sqlExpediente.mDatosVista, datosDatosVista);

        int NumColumnas = tablaDatosVista.Columns.Count;

        generaIFrame(tablaSeleccionVista.Rows[0]["vista"].ToString(), "Norma", NumColumnas * 50, tablaOriginal);
    }
	

    private void generaIFrame(string pNombreVista, string pTituloTab, int pNumColumnas, string pTablaOriginal)
    {
        HtmlControl frame = (HtmlControl)iframeTABS; //.Attributes["src"] = "imgExpedientes/muestraDocto.asxp?archivo=" + nombreImagen;
        frame.Attributes["src"] = "admin/cambios.aspx?nombreTabla=" + pNombreVista + "&id_expediente=" + ocultoIdExpediente.Value.ToString() + "&nombre_tab=" + pTituloTab + "&tablaOriginal=" + pTablaOriginal;
        frame.Attributes["style"] = "height: " + pNumColumnas + "px; width: 512px;";
    }

    //****************************************************************************************************************************
    #endregion
    protected void btnDoctosEtapaActual_Click(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        pnlBtnDoctosActual.Visible = true;
        pnlDoctosSiguiente.Visible = false;
        pnlDoctosAnterior.Visible = false;
        llenaGridDoctosActual(ocultoIDetapa.Value.ToString());

    }

    protected void btnCerrarVentanaFact_Click(object sender, ImageClickEventArgs e)
    {

    }


    private void llenaGridDoctosActual(string paramValorEtapa)
    {
        string[] datosDoctosActual = { paramValorEtapa, ocultoIdExpediente.Value.ToString(), ocultoIDflujo.Value.ToString() };
        gridDoctosEtapa.DataSource = Controladora.consultaDatos(sqlExpediente.doctosEtapaActual, datosDoctosActual);
        gridDoctosEtapa.DataBind();
    }


    public string validaEstatus(object dato)
    {
        if (dato.ToString().Equals("0")) return "~/images/doctoNO.png"; else return "~/images/doctoSI.png";

    }

	public string validaFuncion(object dato)
    {

		if (dato.ToString().Equals("1")) 
		{
			return "~/images/upload.png";
		}
        else if (dato.ToString().Equals("2")) 
		{
			return "~/images/create.png";
		}
		else if (dato.ToString().Equals("3")) 
		{
			return "~/images/BotonCorreo.png";
		}	 
		else if (dato.ToString().Equals("4")) 
		{
			return "~/images/upload.png";
		}
		else if (dato.ToString().Equals("5")) 
		{
			return "~/images/EnviaBoton.png";				
		}
		else
		{
			return "~/images/noAplica.png";
		}
		
    }

	public string validaVisible(object dato)
    {

		if (dato.ToString().Equals("1")) 
		{
			return "true";
		}	
		else if (dato.ToString().Equals("2")) 
		{
			return "true";
		}	 
		else if (dato.ToString().Equals("3")) 
		{
			return "true";
		}
		else if (dato.ToString().Equals("4")) 
		{
			return "true";
		}
		else if (dato.ToString().Equals("5")) 
		{
			return "true";
		}
		else
		{
			return "false";
		}
		
		
    }
	
	protected void btnEnviaDoctoActual_command(object sender, CommandEventArgs e)
	//protected void btnEnviaDoctoActual_command(object sender, GridViewCommandEventArgs e)
	 
	{
		
		string v_comando = e.CommandName.ToString();
		/*
		Response.Write("comando:");
		Response.Write(v_comando);
		*/
		string [] parametro = e.CommandArgument.ToString().Split('|');		
		
		string v_renglon = parametro[0];
		string v_funcion_docto = parametro[1];
		string v_id_documento = parametro[2];
		string v_id_captura = parametro[3];
		string v_id_ope_documento = parametro[4];
		string v_nombre_archivo = parametro[5];
		string v_documento = parametro[6];
		string v_id_entidad = parametro[7];
		string v_fch_modifica = "";
		
		Response.Write("nombre_archivo:");
		Response.Write(v_nombre_archivo);
		Response.Write("documento:");		
		Response.Write(v_documento);

		
		/*
		Response.Write("renglon:");
		Response.Write(v_renglon);
		Response.Write("funcion_docto:");
		Response.Write(v_funcion_docto);		
		Response.Write("id_documento:");
		Response.Write(v_id_documento);
		Response.Write("id_captura:");
		Response.Write(v_id_captura);
		Response.Write("id_ope_documento:");
		Response.Write(v_id_ope_documento);
		Response.Write("nombre_archivo:");
		Response.Write(v_nombre_archivo);
		Response.Write("documento:");		
		Response.Write(v_documento);
		Response.Write("modifica:");
		Response.Write(v_fch_modifica);
		Response.Write("entidad:");
		Response.Write(v_id_entidad);
		*/
		
		if ( (v_funcion_docto == "1") || (v_funcion_docto == "4") ) // carga de archivo y carga de archivo opcional
		{
		
			titulosExpediente();
			lblGeneralDoctos.Visible = false;       

			ocultoIDdocto.Value = v_id_documento;
			ocultoIDopeDocto.Value = v_id_ope_documento;
			ocultoFchSubeImg.Value = v_fch_modifica;

			if (v_id_captura.Equals("0"))
			{
				//no tiene documentos, tenemos que subirlos
				lblNombreDocumento.Text = v_documento;
				pnlDoctosActualImg.Visible = true;
			}
			else
			{
				string ruta = Server.MapPath("~\\") + "archivos\\imgExpedientes\\" + v_nombre_archivo;

				HtmlControl frame = (HtmlControl)frameDoctosActual; //.Attributes["src"] = "imgExpedientes/muestraDocto.asxp?archivo=" + v_nombre_archivo;
				frame.Attributes["src"] = "imgExpedientes/muestraDocto.aspx?archivo=" + v_nombre_archivo;
				lblNombreDocto.Text = v_documento;
				lblFchSubeDocto.Text = ocultoFchSubeImg.Value.ToString();
				lblGeneralDoctos.Text = v_nombre_archivo;
				ocultoNombreImg.Value = v_nombre_archivo;
				lblGeneralDoctos.Visible = true;
				modalDoctosActual.Show();
				
			}
		
		}
		
		if ( (v_funcion_docto == "2") ) // generar
		{
			string v_num_expediente = "";
			string v_nombre_flujo = "";
			string v_nombre_etapa = "";
			string v_norma = "";
			string v_id_norma = "";
			string v_id_expediente = "";
			string v_id_etapa = "";
				
			string[] datosDatosProyecto = { ocultoIdExpediente.Value.ToString() };
            DataTable tablaDatosProyecto = Controladora.consultaDatos(sqlExpediente.LeerDatosProyecto, datosDatosProyecto);
			if (tablaDatosProyecto.Rows.Count > 0)
            {
				//Response.Write("row.count.proyecto");				
				v_num_expediente = tablaDatosProyecto.Rows[0][0].ToString();
				v_nombre_flujo = tablaDatosProyecto.Rows[0][1].ToString();
				v_nombre_etapa = tablaDatosProyecto.Rows[0][2].ToString();
				v_norma = tablaDatosProyecto.Rows[0][3].ToString();
				v_id_norma = tablaDatosProyecto.Rows[0][4].ToString();
				v_id_expediente = tablaDatosProyecto.Rows[0][5].ToString();
				v_id_etapa = tablaDatosProyecto.Rows[0][6].ToString();
			}
		
			btnCreaPDF(v_id_expediente);
			
		}
		
		if ( (v_funcion_docto == "3") ) // enviar
		{
			string ruta_adjunto = Server.MapPath("~");
			
			string documentoSalida = "";
			
			string v_num_expediente = "";
			string v_nombre_flujo = "";
			string v_nombre_etapa = "";
			string v_norma = "";
			string v_id_norma = "";
			string v_id_expediente = "";
			string v_id_etapa = "";
				
			string[] datosLeerDatosProyecto = { ocultoIdExpediente.Value.ToString() };
            DataTable tablaLeerDatosProyecto = Controladora.consultaDatos(sqlExpediente.LeerDatosProyecto, datosLeerDatosProyecto);
			if (tablaLeerDatosProyecto.Rows.Count > 0)
            {

				v_num_expediente = tablaLeerDatosProyecto.Rows[0][0].ToString();
				v_nombre_flujo = tablaLeerDatosProyecto.Rows[0][1].ToString();
				v_nombre_etapa = tablaLeerDatosProyecto.Rows[0][2].ToString();
				v_norma = tablaLeerDatosProyecto.Rows[0][3].ToString();
				v_id_norma = tablaLeerDatosProyecto.Rows[0][4].ToString();
				v_id_expediente = tablaLeerDatosProyecto.Rows[0][5].ToString();
				v_id_etapa = tablaLeerDatosProyecto.Rows[0][6].ToString();
			}
			
			ruta_adjunto = ruta_adjunto.Replace("\\","@");
			ruta_adjunto = ruta_adjunto.Replace("@","\\\\");
			
			//Response.Write("nombre archivo:");
			//Response.Write(v_nombre_archivo);
			//Response.Write("<br>");
			documentoSalida = ruta_adjunto +  v_nombre_archivo;
			documentoSalida = documentoSalida.Replace("\\\\","\\");
			documentoSalida = documentoSalida.Replace("\\","\\\\");
			//string v_chr = Convert.ToChar(92);
			//documentoSalida = documentoSalida.Replace(v_chr,"\\\\");
			//Response.Write("ruta completa:");
			//Response.Write(documentoSalida);
			//Response.Write("<br>");
			
			bool v_datos_contacto = false;
			string v_paterno = "";
			string v_materno = "";
			string v_nombre  = "";
			string v_correo1 = "";
			string v_correo2 = "";
			//int v_id_entidad_responsable = 0;
			

			//quien debe ser el destinatario del correo?
			if (v_id_entidad.Equals("3"))	//Cliente
			{
				//obtiene los datos del destinatario, debe ser  de planta
				//primero busca usando el id cliente
				string[] datosLeerPersona = { ocultoIdExpediente.Value.ToString() };
				DataTable tablaLeerPersona = Controladora.consultaDatos(sqlExpediente.LeerPersonaEntregas, datosLeerPersona);
				if (tablaLeerPersona.Rows.Count > 0)
				{
					v_datos_contacto = true;
					v_paterno = tablaLeerPersona.Rows[0][0].ToString();
					v_materno = tablaLeerPersona.Rows[0][1].ToString();
					v_nombre  = tablaLeerPersona.Rows[0][2].ToString();
					v_correo1 = tablaLeerPersona.Rows[0][3].ToString();
					v_correo2 = tablaLeerPersona.Rows[0][4].ToString();
					//Response.Write("uno<br>");
				}
				else
				{
					//despues prueba con los datos de la persona de contacto
					DataTable tablaLeerContacto = Controladora.consultaDatos(sqlExpediente.LeerPersonaContacto, datosLeerPersona);
					if (tablaLeerContacto.Rows.Count > 0)
					{
						v_datos_contacto = true;
						v_paterno = tablaLeerContacto.Rows[0][0].ToString();
						v_materno = tablaLeerContacto.Rows[0][1].ToString();
						v_nombre  = tablaLeerContacto.Rows[0][2].ToString();
						v_correo1 = tablaLeerContacto.Rows[0][3].ToString();
						v_correo2 = tablaLeerContacto.Rows[0][4].ToString();
						//Response.Write("dos<br>");
					}
					else
					{
						v_datos_contacto=false;
						//Response.Write("tres<br>");
					}

				}
			}
			if (v_id_entidad.Equals("5"))	//Evaluador
			{
				string[] datosLeerEvaluador = { ocultoIdExpediente.Value.ToString() };
				DataTable tablaLeerEvaluador = Controladora.consultaDatos(sqlExpediente.LeerEvaluador, datosLeerEvaluador);
				if (tablaLeerEvaluador.Rows.Count > 0)
				{
					v_datos_contacto = true;
					v_paterno = tablaLeerEvaluador.Rows[0][0].ToString();
					v_materno = tablaLeerEvaluador.Rows[0][1].ToString();
					v_nombre  = tablaLeerEvaluador.Rows[0][2].ToString();
					v_correo1 = tablaLeerEvaluador.Rows[0][3].ToString();										
				}
				else
				{					
					v_datos_contacto=false;						
				}
				
			}
			
			
			//obtiene el correo del usuario actual, para usarlo como remitente
			string remitente;
			string[] datosLeerRemitente = { Session["idUsuario"].ToString() };
			DataTable tablaLeerRemitente = Controladora.consultaDatos(sqlExpediente.LeerRemitente, datosLeerRemitente);
			if (tablaLeerRemitente.Rows.Count > 0)
			{				
				remitente = tablaLeerRemitente.Rows[0][0].ToString();				
			}
			else
			{				
				remitente = "certificacion@onncce.org.mx";
			}

				
			if (v_datos_contacto)
            {				
				//armar la informacion para el correo
				
				// titulo,mensaje, remitente, destinatario, estatus_envio, adjunto,origen_correo
				//Response.Write("cuatro<br>");
				// armo el correo 
				string titulo;
				string cuerpo;
				
				string destinatario;
				string origenCorreo;


				titulo = "ONNCCE: Entrega Documentacion: " + v_documento + "; Proyecto: " + v_num_expediente;
				
				destinatario = v_correo1;
				origenCorreo = "Envia Documento:" + v_id_expediente + "/Docto:" + v_id_documento + "/Etapa:" + v_id_etapa;

				cuerpo = "";
				cuerpo = cuerpo + "<big>";
				cuerpo = cuerpo + "Estimado: <span style=\"font-weight: bold;\">" + v_paterno + " " + v_materno + " " + v_nombre + "</span><br>";				
				cuerpo = cuerpo + "<br>";
				cuerpo = cuerpo + "Notificación de Entrega de Documento para el Proyecto " + v_num_expediente + "</big><br>";
				cuerpo = cuerpo + "<br>";
				cuerpo = cuerpo + "<table style=\"text-align: left;\" border=\"1\" cellpadding=\"0\" cellspacing=\"0\">";
				cuerpo = cuerpo + "  <tbody>";
				cuerpo = cuerpo + "    <tr style=\"color: white;\">";
				cuerpo = cuerpo + "      <td style=\"background-color: rgb(51, 51, 255);\" align=\"center\" valign=\"top\">Núm. Proyecto</td>";
				cuerpo = cuerpo + "      <td style=\"background-color: rgb(51, 51, 255);\" align=\"center\" valign=\"top\">Norma</td>";
				cuerpo = cuerpo + "      <td style=\"background-color: rgb(51, 51, 255);\" align=\"center\" valign=\"top\">Etapa Actual</td>";
				cuerpo = cuerpo + "      <td style=\"background-color: rgb(51, 51, 255);\" align=\"center\" valign=\"top\">Documento</td>"; 
				cuerpo = cuerpo + "    </tr>";
				cuerpo = cuerpo + "	<tr>";
				cuerpo = cuerpo + "		<td align=\"right\" valign=\"top\">" + v_num_expediente + "</td>";
				cuerpo = cuerpo + "		<td align=\"left\" valign=\"top\">" + v_norma + "</td>";
				cuerpo = cuerpo + "		<td align=\"right\" valign=\"top\">" + v_nombre_etapa + "</td>";
				cuerpo = cuerpo + "		<td align=\"right\" valign=\"top\">" + v_documento + "</td>";
				cuerpo = cuerpo + "	</tr>";
				cuerpo = cuerpo + "  </tbody>";
				cuerpo = cuerpo + "</table>";
				cuerpo = cuerpo + "<br>";
				cuerpo = cuerpo + "<big>";
				cuerpo = cuerpo + "Archivo Adjunto: <span style=\"font-weight: bold;\">" + v_nombre_archivo + "</span>";
				cuerpo = cuerpo + "<br>";
				cuerpo = cuerpo + "</big>";
				cuerpo = cuerpo + "<br><br>";
				cuerpo = cuerpo + "<span style=\"color: rgb(51, 51, 255);\">Usted recibe este mensaje pues esta registrado como Persona de Contacto en el ONNCCE para este Proyecto</span>";
				cuerpo = cuerpo + "<br>";
				cuerpo = cuerpo + "<span style=\"color: rgb(51, 51, 255);\">Cualquier duda respecto a este correo, por favor llame a su ejecutivo que lo atiende en el ONNCCE</span>";
				cuerpo = cuerpo + "<br><br>";
				cuerpo = cuerpo + "<span style=\"color: rgb(51, 51, 255);\">Mensaje Automático creado por Gestión ONNCCE; " + DateTime.Now.ToString("yyyy-MM-dd") + "</span>";

				//guarda el correo 
				string[] datos = { titulo, cuerpo, remitente, destinatario, origenCorreo, documentoSalida};
				
                string errorSQL = Controladora.regresaSentenciaSQL(sqlOperacion.aEnviaCorreos, datos);
                string errorURL = HttpContext.Current.Request.Url.AbsolutePath.ToString();
                string errorIdent = "EnviaDocumento";
                Utilerias.registraSQL(errorSQL, errorURL, errorIdent);
				
				//titulo,mensaje,remitente,destinatario,estatus_envio, origen_correo, adjunto
				Controladora.registraDatos(sqlOperacion.aEnviaCorreos, datos);
				
				//Response.Write("ope_envia_mensaje<br>");
				
				//y esperar que el proceso de correos haga su parte												
				MsgBox("Se ha generado el proceso de envio del documento", this.Page, this);
				

                //marca que el documento ya fue enviado
                string[] datosCdoctosActual = { v_id_ope_documento, Session["idUsuario"].ToString() };
                if (Controladora.actualizaDatos(sqlExpediente.doctosDocumentoEnviado, datosCdoctosActual))
                {                    
					//Response.Write("cinco<br>");
                    //se subió el archivo correctamente
                    pnlDoctosActualImg.Visible = false;
                    llenaGridDoctosActual(ocultoIDetapa.Value.ToString());
                    lblGeneralDoctos.Text = "Se generado el proceso de envio del documento";
                    lblGeneralDoctos.CssClass = "labelNormal";
                    lblGeneralDoctos.Visible = true;
                }
                else
                {
					//Response.Write("seis<br>");
                    //hubo problemas con el archivo, eliminamos el archivo que acabamos de subir
                    lblGeneralDoctos.Text = "Hubo un error al buscar datos de contacto,informe al administrador";
                    lblGeneralDoctos.CssClass = "labelError";
                    lblGeneralDoctos.Visible = true;
                }				

			}
			else
			{
				//Response.Write("siete<br>");				
				//hubo problemas con el archivo, eliminamos el archivo que acabamos de subir
				lblGeneralDoctos.Text = "Hubo un error al marcar el envio del documento,informe al administrador";
				lblGeneralDoctos.CssClass = "labelError";
				lblGeneralDoctos.Visible = true;
			}
			
		}
			
			
	}
			
	
    //s cuando seleccionan y quieren cambiar un tipo de documento
    protected void gridDoctosEtapa_SelectedIndexChanged(object sender, EventArgs e)
    {
				
        titulosExpediente();
        lblGeneralDoctos.Visible = false;
        
        GridViewRow row = gridDoctosEtapa.SelectedRow;
        string v_id_ope_documento = gridDoctosEtapa.DataKeys[row.RowIndex].Values["id_ope_documento"].ToString();//id del documento de la tabla ope_documentos
        string id_captura = gridDoctosEtapa.DataKeys[row.RowIndex].Values["id_captura"].ToString(); //id para saber si tiene documento o no
        string nombreDocumento = gridDoctosEtapa.DataKeys[row.RowIndex].Values["documento"].ToString();
        string nombreImagen = gridDoctosEtapa.DataKeys[row.RowIndex].Values["nombre_archivo"].ToString();        

        ocultoIDdocto.Value = gridDoctosEtapa.DataKeys[row.RowIndex].Values["id_documento"].ToString(); //id del documento de la tabla rgl_documentos
        ocultoIDopeDocto.Value = v_id_ope_documento;
        ocultoFchSubeImg.Value = gridDoctosEtapa.DataKeys[row.RowIndex].Values["fecha_modifica_documento"].ToString();

        if (id_captura.Equals("0"))
        {
            //no tiene documentos, tenemos que subirlos
            lblNombreDocumento.Text = nombreDocumento;
            pnlDoctosActualImg.Visible = true;
        }
        else
        {
			string ruta = Server.MapPath("~\\") + "archivos\\imgExpedientes\\" + nombreImagen;

            HtmlControl frame = (HtmlControl)frameDoctosActual; //.Attributes["src"] = "imgExpedientes/muestraDocto.asxp?archivo=" + nombreImagen;
            frame.Attributes["src"] = "imgExpedientes/muestraDocto.aspx?archivo=" + nombreImagen;
            lblNombreDocto.Text = nombreDocumento;
            lblFchSubeDocto.Text = ocultoFchSubeImg.Value.ToString();
            lblGeneralDoctos.Text = nombreImagen;
            ocultoNombreImg.Value = nombreImagen;
            lblGeneralDoctos.Visible = true;
            modalDoctosActual.Show();
			
        }


    }
	
    //boton para subir el documento fase actual
    protected void btnSubeDoctoActual_Click(object sender, EventArgs e)
    {
		
        //llenatabas();
        titulosExpediente();
        //validamos si traemos archivo
        if (fileSubeDocto.HasFile)
        {
            //si contiene archivo
            //variables para validar el archivo
            Boolean tipoArhivoOK;//valida si la extensión del archivo es correcta
            string ruta;
            String[] extensionesPermitidas = { ".gif", ".png", ".jpeg", ".jpg", ".pdf", ".xls", ".xlsx", ".doc", ".docx" };
            string extensionArchivo = Path.GetExtension(fileSubeDocto.FileName).ToLower();
            ruta = Server.MapPath("~\\") + "/archivos/imgExpedientes/";
            tipoArhivoOK = false;
            //validación de arhcivo: extensiones
            for (int i = 0; i < extensionesPermitidas.Length; i++)
            {
                if (extensionArchivo == extensionesPermitidas[i])
                {
                    tipoArhivoOK = true;
                }
            }

            if (tipoArhivoOK)
            {
                //el tipo de arhivo es correcto, lo subimos
                string nameArchivoOriginal, nameArchivoNuevo;

                nameArchivoOriginal = fileSubeDocto.FileName;

                //<obtenemos el nuevo nombre del archivo>
                string[] datosFchSisDB = { string.Empty };
                DataTable tablaFchSisDB = Controladora.consultaDatos(sqlConstantes.fechaBDddmmyyyy, datosFchSisDB);
                nameArchivoNuevo = ocultoIDdocto.Value.ToString() + "_" + ocultoIdExpediente.Value.ToString() + "_" + ocultoIDetapa.Value.ToString() + "_" + Session["idUsuario"].ToString() + "_" + tablaFchSisDB.Rows[0][0].ToString() + extensionArchivo;
                //el nombre del archivo se conforma por id_docto/id_expediente/id_epata/id_usuario/fhc_sistema
                //</obtenemos el nuevo nombre del archivo>

                //<subimos el archivo
                fileSubeDocto.SaveAs(ruta + nameArchivoNuevo);

                //marca que el documento ya fue cargado
                string[] datosCdoctosActual = { ocultoIDopeDocto.Value.ToString(), Session["idUsuario"].ToString(), nameArchivoNuevo };
                if (Controladora.actualizaDatos(sqlExpediente.doctosCsubeArchivo, datosCdoctosActual))
                {
                    //valido si debo mandar email
                    string[] datosVerifricaCorreo = { ocultoIdExpediente.Value.ToString(), ocultoIDdocto.Value.ToString() };
                    DataTable tablaVerificaCorreo = Controladora.consultaDatos(sqlExpediente.mExisteCorreo, datosVerifricaCorreo);
                    if (tablaVerificaCorreo.Rows.Count > 0)
                    {//SE ENVÍO CORREO DE AVISO DE ELIMINACIÓN DE DOCUMENTO
                        //creo el mensaje del correo
                        string[] datosFnNotificaResolucionDocumento = { ocultoIdExpediente.Value.ToString(), ocultoIDdocto.Value.ToString(), Session["idUsuario"].ToString() };
                        DataTable tableFnNotificaResolucionDocumento = Controladora.consultaDatos(sqlExpediente.fnNontificaResolucionDocummento, datosFnNotificaResolucionDocumento);

                        string[] datosIDemail = { tableFnNotificaResolucionDocumento.Rows[0][0].ToString() };
                        DataTable tablaDatosEmail = Controladora.consultaDatos(sqlExpediente.mEmail, datosIDemail);

                        enviaCorreoConfirmacionDatos(tablaDatosEmail.Rows[0]["titulo"].ToString(), tablaDatosEmail.Rows[0]["mensaje"].ToString(), tablaDatosEmail.Rows[0]["destinatario"].ToString());
                    }


                    //se subió el archivo correctamente
                    pnlDoctosActualImg.Visible = false;
                    llenaGridDoctosActual(ocultoIDetapa.Value.ToString());
                    lblGeneralDoctos.Text = "El archivo se subió con éxito";
                    lblGeneralDoctos.CssClass = "labelNormal";
                    lblGeneralDoctos.Visible = true;
                }
                else
                {
                    //hubo problemas con el archivo, eliminamos el archivo que acabamos de subir
                    lblGeneralDoctos.Text = "Hubo un error al subir el archivo,informe al administrador";
                    lblGeneralDoctos.CssClass = "labelError";
                    lblGeneralDoctos.Visible = true;
                }
            }
            else
            {
                lblGeneralDoctos.Text = "El tipo de archivo no esta permitido: " + extensionArchivo;
                lblGeneralDoctos.CssClass = "labelError";
                lblGeneralDoctos.Visible = true;
            }
        }
        else
        {
            //NO TENEMOS ARCHIVO
            lblGeneralDoctos.Text = "Debe seleccionar un archivo";
            lblGeneralDoctos.CssClass = "labelError";
            lblGeneralDoctos.Visible = true;
        }
		
    }
    protected void btnCerrarVentanaDoctos_Click(object sender, ImageClickEventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        modalDoctosActual.Hide();
    }
    protected void btnCancelarModDocto_Click(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        modalDoctosActual.Hide();
    }
    //actualiza la tabla OPE_DOCUMENTO y elimina el documento para que ya no aparezca en el sistema
    protected void btnEliminaDoctoActual_Click(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        //validamos que se capturó información en las observaciones
        if (txtObservaDoctoActualElimina.Text.ToString().Trim().Equals(""))
        {
            //debe capturar datos para la observación

            lblGeneralPnlDoctosActual.Text = "Debe capturar las observaciones";
            lblGeneralPnlDoctosActual.CssClass = "labelError";
            lblGeneralPnlDoctosActual.Visible = true;
            modalDoctosActual.Show();
            return;
        }
        else
        {
            ocultoResultadoDocumentos.Value = "0";
            string[] datosCdoctosActual = { ocultoIDopeDocto.Value.ToString() };
            if (Controladora.actualizaDatos(sqlExpediente.doctosCeliminaArchivo, datosCdoctosActual))
            {
                //se actualizó la información en la tabla
                //eliminado el archivo en el directorio
                string ruta;
                ruta = Server.MapPath("~\\") + "/archivos/imgExpedientes/";
                System.IO.File.Delete(ruta + ocultoNombreImg.Value.ToString());
                pnlDoctosActualImg.Visible = false;
                llenaGridDoctosActual(ocultoIDetapa.Value.ToString());
                lblGeneralDoctos.Text = "El archivo se eliminó con éxito";
                lblGeneralDoctos.CssClass = "labelNormal";
                lblGeneralDoctos.Visible = true;
                ocultoResultadoDocumentos.Value = "0";
                HtmlControl frame = (HtmlControl)frameDoctosActual; //.Attributes["src"] = "imgExpedientes/muestraDocto.asxp?archivo=" + nombreImagen;
                frame.Attributes["src"] = "imgExpedientes/muestraDocto.aspx?archivo=";

                //verificamos si es un documento que puede ver un externo
/* se omite este proceso, pues no hay operacion con terceros
                string[] datosDoctosUV = { "id_usuario", "ope_terceros_docs_subir", " nombre_archivo = '" + ocultoNombreImg.Value.ToString() + "'" };
                DataTable tablaDoctosUV = Controladora.consultaDatos(sqlCamposTablas.seleccionaCampo, datosDoctosUV);

                if (tablaDoctosUV.Rows.Count > 0)
                {
                    //tiene doctos de un tercero
                    string[] datosBdoctosTerceros = { "ope_terceros_docs_subir", "id_usuario = 0, nombre_archivo = ''", "nombre_archivo = '" + ocultoNombreImg.Value.ToString() + "'" };
                    bool cOpeTercerosDocsSubir = Controladora.actualizaDatos(sqlCamposTablas.upDate, datosBdoctosTerceros);


                    //busco el mail del usuario para enviarle el mensaje
                    string[] datosBuscaEmail = { tablaDoctosUV.Rows[0][0].ToString() };
                    DataTable tablaEmail = Controladora.consultaDatos(sqlExternos.mMailUsuario, datosBuscaEmail);
                    //se envía el mail
                    enviaCorreoConfirmacionDatos("Asignación de expediente", "Se ha eliminado un archivo del número de expediente: " + Session["numExpediente"].ToString() + "<br>Las razones son:" + txtObservaDoctoActualElimina.Text + "<br><br>Favor de verificar y volver a subirlo", tablaEmail.Rows[0][0].ToString());
                }
*/

                //guardamos un registros de la eliminacion en las observaciones
                string[] datosObssDoctosActualElimina = { ocultoIdExpediente.Value.ToString(), ocultoIDetapa.Value.ToString(), "3", txtObservaDoctoActualElimina.Text, Session["idUsuario"].ToString(), ocultoIDdocto.Value.ToString() };
                if (!Controladora.registraDatos(sqlExpediente.aObsEtapa, datosObssDoctosActualElimina))
                {
                    lblGeneralDoctos.Text = "El archivo se eliminó con éxito, pero las observaciones no se pudieron registrar";
                    lblGeneralDoctos.CssClass = "labelError";
                    lblGeneralDoctos.Visible = true;
                }
            }
        }
    }


    #region boton etapa anterior
    //permite verificar los documentos de la etapa anterior
    protected void btnDoctosEtapaAnterior_Click(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        pnlBtnDoctosActual.Visible = false;
        pnlDoctosActualImg.Visible = false;
        pnlDoctosSiguiente.Visible = false;
        pnlDoctosAnterior.Visible = true;//habilitamos el panel
        //necesitamos saber cual es la etapa anterior
        string[] datosEmpty = { ocultoIDflujo.Value.ToString(), ocultoIDetapa.Value.ToString() };
        DataTable tablaFnEtpAnt = Controladora.consultaDatos(sqlExpediente.ejecutaFnEtapaAnt, datosEmpty);
        string idEtapaAnteior = tablaFnEtpAnt.Rows[0][0].ToString();

        //llenamos el grid con la lista de los documentos de la etapa anterior
        string[] datosDoctosActual = { idEtapaAnteior, ocultoIdExpediente.Value.ToString(), ocultoIDflujo.Value.ToString() };
        gridDoctosAnterior.DataSource = Controladora.consultaDatos(sqlExpediente.doctosEtapaActual, datosDoctosActual);
        gridDoctosAnterior.DataBind();

        //asignamos la imagen del panel

    }

    //se lanza cuando hacen clic sobre la imagen del grid
    protected void gridDoctosAnterior_SelectedIndexChanged(object sender, EventArgs e)
    {
        //llenatabas();

        GridViewRow row = gridDoctosAnterior.SelectedRow;
        string id = gridDoctosAnterior.DataKeys[row.RowIndex].Values["id_ope_documento"].ToString();//id del documento de la tabla ope_documentos
        string id_captura = gridDoctosAnterior.DataKeys[row.RowIndex].Values["id_captura"].ToString(); //id para saber si tiene documento o no
        string nombreDocumento = gridDoctosAnterior.DataKeys[row.RowIndex].Values["documento"].ToString();
        string nombreImagen = gridDoctosAnterior.DataKeys[row.RowIndex].Values["nombre_archivo"].ToString();
        string fhcSubioImagen = gridDoctosAnterior.DataKeys[row.RowIndex].Values["fecha_modifica_documento"].ToString();
        //odct.nombre_archivo, odct.fecha_modifica_documento
        ocultoIDrglDoctoAnterior.Value = gridDoctosAnterior.DataKeys[row.RowIndex].Values["id_documento"].ToString(); //id del documento de la tabla rgl_documentos

        //ocultoIDdocto.Value = gridDoctosAnterior.DataKeys[row.RowIndex].Values["id_documento"].ToString(); //id del documento de la tabla rgl_documentos
        //ocultoIDopeDocto.Value = id;
        ocultoIDopeDoctoAnterior.Value = id;
        //ocultoFchSubeImg.Value = gridDoctosAnterior.DataKeys[row.RowIndex].Values["fecha_modifica_documento"].ToString();

        //ya tiene documentos
        HtmlControl frame = (HtmlControl)frameDoctosAnterior; //.Attributes["src"] = "imgExpedientes/muestraDocto.asxp?archivo=" + nombreImagen;
        frame.Attributes["src"] = "imgExpedientes/muestraDocto.aspx?archivo=" + nombreImagen;

        lblNombreDoctoAnterior.Text = nombreDocumento;
        lblFchDoctoAnterior.Text = fhcSubioImagen;
        ocultoNombreImgDoctoAnterior.Value = nombreImagen;

        modalDoctosAnterior.Show();
    }

    //cierra la modal del aviso
    protected void btnCierraModalDoctosAnterior_Click(object sender, ImageClickEventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        modalDoctosAnterior.Hide();
    }

    //cierra la modal del aviso
    protected void btnCierraAvisoDoctosAnterior_Click(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        modalDoctosAnterior.Hide();
    }

    //se dispara cuando hacen click en la modal de avisos etapa anterior en el boton de regresar a la etapa
    protected void btnRegresaEtapa_Click(object sender, EventArgs e)
    {
        //cambiamos el estatus del documento a cero
        //llenatabas();
        titulosExpediente();
        //valida observaciones
        if (txtObservaDoctosAnterior.Text.ToString().Trim().Equals(""))
        {
            lblGeneralPnlDoctosAnterior.Text = "Debe capturar las observaciones para eliminar el documento";
            lblGeneralPnlDoctosAnterior.CssClass = "labelError";
            lblGeneralPnlDoctosAnterior.Visible = true;
            modalDoctosAnterior.Show();
            return;
        }
        if (txtObservaRegresaEtapa.Text.ToString().Trim().Equals(""))
        {
            lblGeneralPnlDoctosAnterior.Text = "Debe capturar las observaciones para regresar a al etapa anterior";
            lblGeneralPnlDoctosAnterior.CssClass = "labelError";
            lblGeneralPnlDoctosAnterior.Visible = true;
            modalDoctosAnterior.Show();
            return;
        }
        string[] datosCdoctosActual = { ocultoIDopeDoctoAnterior.Value.ToString() };

        //obtengo el usuario que subio el archivo
        DataTable tablaIDusrCmp = Controladora.consultaDatos(sqlExpediente.mIDusrCump, datosCdoctosActual);
        string IDusrCump = tablaIDusrCmp.Rows[0][0].ToString();
        string idDocumento = tablaIDusrCmp.Rows[0]["id_documento"].ToString();

        //ACTUALIZAMOS OPE_DOCTOS_TERCEROS
        string[] datosBopeTercerosDocsSubir = { ocultoIdExpediente.Value.ToString(), idDocumento };
        bool cOpeTercerossubir = Controladora.actualizaDatos(sqlExternos.cOpeTercerosSubir, datosBopeTercerosDocsSubir);


        if (Controladora.actualizaDatos(sqlExpediente.doctosCeliminaArchivo, datosCdoctosActual))
        {
            //se actualizó la información en la tabla
            //eliminado el archivo en el directorio
            string ruta;
            ruta = Server.MapPath("~\\") + "/archivos/imgExpedientes/";
            System.IO.File.Delete(ruta + ocultoNombreImgDoctoAnterior.Value.ToString());
            //regresamos a la etapa anterior
            string[] datosFnRegresaEtAnt = { ocultoIdExpediente.Value.ToString(), ocultoIDflujo.Value.ToString(), ocultoIDetapa.Value.ToString(), Session["idUsuario"].ToString() };
            DataTable tablaFnRegresaEtAnt = Controladora.consultaDatos(sqlExpediente.fnRegresaEtAnt, datosFnRegresaEtAnt);
            if (tablaFnRegresaEtAnt.Rows[0][0].ToString().Equals("1"))
            {
                //capturo las observaciones
                string[] datosObservaDoctosAnterior = { ocultoIdExpediente.Value.ToString(), ocultoIDetapa.Value.ToString(), "3", txtObservaDoctosAnterior.Text, Session["idUsuario"].ToString(), ocultoIDrglDoctoAnterior.Value.ToString() };
                if (!Controladora.registraDatos(sqlExpediente.aObsEtapa, datosObservaDoctosAnterior))
                {
                    lblGeneralDoctos.Text = "El archivo se eliminó con éxito, pero las observaciones al documento no se pudieron registrar";
                    lblGeneralDoctos.CssClass = "labelError";
                    lblGeneralDoctos.Visible = true;
                }
                else
                {
                    string[] datosObservaRegresaEtapa = { ocultoIdExpediente.Value.ToString(), ocultoIDetapa.Value.ToString(), "2", txtObservaRegresaEtapa.Text, Session["idUsuario"].ToString(), "0" };
                    if (!Controladora.registraDatos(sqlExpediente.aObsEtapa, datosObservaRegresaEtapa))
                    {
                        lblGeneralDoctos.Text = "El archivo se eliminó con éxito, pero las observaciones a la etapa no se pudieron registrar";
                        lblGeneralDoctos.CssClass = "labelError";
                        lblGeneralDoctos.Visible = true;
                    }
                    else
                    {
                        //obtengo el id de la observación
                        string[] datosMaxIdObs = { ocultoIdExpediente.Value.ToString() };
                        DataTable tablaMaxIDObs = Controladora.consultaDatos(sqlExpediente.maxIDobsOpeObservaciones, datosMaxIdObs);


                        string[] datosFNnotificaRegresoXDocumento = { ocultoIDopeDoctoAnterior.Value.ToString(), tablaMaxIDObs.Rows[0][0].ToString(), Session["idUsuario"].ToString(), IDusrCump };
                        //id_ope_documento, id_observacion, id_usuario, id_usuario
                        DataTable tablaFNnotificaRegresoXDocumento = Controladora.consultaDatos(sqlExpediente.fnNotRegresoDocto, datosFNnotificaRegresoXDocumento);
                        //me regresa el id_ del mail para saber que mensaje mando
                        string[] datosIDemail = { tablaFNnotificaRegresoXDocumento.Rows[0][0].ToString() };
                        DataTable tablaDatosEmail = Controladora.consultaDatos(sqlExpediente.mEmail, datosIDemail);

                        enviaCorreoConfirmacionDatos(tablaDatosEmail.Rows[0]["titulo"].ToString(), tablaDatosEmail.Rows[0]["mensaje"].ToString(), tablaDatosEmail.Rows[0]["destinatario"].ToString());

                        ventanaMensaje.Text = "La información se ha generado correctamente y se envío el mail correspondiente";
                        ventanaOcultoTpoCierra.Value = "1";
                        ventanaModal.Show();

                        //si funciono y regreso a la busqueda
                        //direcciona();
                    }
                }
            }
            else
            {
                lblGeneralModalDoctosAnterior.Text = "ERROR: No se pudo regresar a la etapa anterior, NO USAR EL EXPEDIENTE [" + ocultoIdExpediente.Value.ToString() + "] POR INCONSISTENCIAS, avisar a sistemas sobre el error";
                lblGeneralModalDoctosAnterior.CssClass = "labelError";
                lblGeneralModalDoctosAnterior.Visible = true;
            }
        }
        else
        {
            lblGeneralModalDoctosAnterior.Text = "No se pudo eliminar el archivo, favor de intentarlo de nuevo. Código error {X(003}";
            lblGeneralModalDoctosAnterior.CssClass = "labelError";
            lblGeneralModalDoctosAnterior.Visible = true;
        }
    }

    #endregion

	
    #region boton siguiente etapa
	/*
    protected void btnDoctosEtapaSiguiente_Click(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        //#declara
        string valorSiguienteEtapa;

        //#asigna valores
        valorSiguienteEtapa = "0";

        pnlBtnDoctosActual.Visible = false;
        pnlDoctosActualImg.Visible = false;
        pnlDoctosAnterior.Visible = false;
        pnlDoctosSiguiente.Visible = true;

        //lanzamos la funciona para generar los documentos de la siguiente etapa
        string[] datosCreaDoctosSigEtapa = { ocultoIdExpediente.Value.ToString(), ocultoIDflujo.Value.ToString(), ocultoIDsiguienteEtapa.Value.ToString(), Session["idUsuario"].ToString() };
        DataTable tablaCreaDoctosSigEtapa = Controladora.consultaDatos(sqlExpediente.ejecutaCreaDoctosSigEtapa, datosCreaDoctosSigEtapa);
        //validamos que se generaron lso documentos



        switch (Convert.ToInt16(tablaCreaDoctosSigEtapa.Rows[0][0].ToString()))
        {
            case 1:
            case -4:
                //se registraron correctamente los datos
                //obtengo los documentos de la siguiente etapa
                string[] datosDoctosActual = { valorSiguienteEtapa, ocultoIdExpediente.Value.ToString(), ocultoIDflujo.Value.ToString() };
                gridDoctosSiguiente.DataSource = Controladora.consultaDatos(sqlExpediente.doctosEtapaActual, datosDoctosActual);
                gridDoctosSiguiente.DataBind();
                break;
            case -1:
                lblGeneralDoctosSiguiente.Text = "El expediente no esta en TRAMITE, no se puede crear el nuevo juego de tramites y documentos";
                lblGeneralDoctosSiguiente.CssClass = "labelError";
                lblGeneralDoctosSiguiente.Visible = true;
                break;
            case -2:
                lblGeneralDoctosSiguiente.Text = "El id_etapa solicitado, no existe o no esta activo en el flujo existente del expediente";
                lblGeneralDoctosSiguiente.CssClass = "labelError";
                lblGeneralDoctosSiguiente.Visible = true;
                break;
        }
    }
	*/

    /*
	private void idEtapaSiguiente()
    {
        string[] datosValorSiguienteEtapa = { ocultoIDflujo.Value.ToString(), ocultoIDetapa.Value.ToString() };
        DataTable tablaValorSiguienteEtapa = Controladora.consultaDatos(sqlExpediente.ejecutaVerificaSiguienteEtapa, datosValorSiguienteEtapa);
        ocultoIDsiguienteEtapa.Value = tablaValorSiguienteEtapa.Rows[0][0].ToString();
    }
	*/
	
    #endregion

    #region Trámites
    //boton para ejecutar los trámites de la etapa anterior
    protected void btnTramitesEtapaAnterior_Click(object sender, EventArgs e)
    {
        limpiaVariablesTramites();
        //llenatabas();
        titulosExpediente();
        pnlTramitesBtnAnterior.Visible = true;

        string[] datosEmpty = { ocultoIDflujo.Value.ToString(), ocultoIDetapa.Value.ToString() };
        DataTable tablaFnEtpAnt = Controladora.consultaDatos(sqlExpediente.ejecutaFnEtapaAnt, datosEmpty);
        string idEtapaAnteior = tablaFnEtpAnt.Rows[0][0].ToString();

        llenaGridTramites(gridTramitesAnterior, idEtapaAnteior);
    }

    //boton para ejecutar los trámites de la etapa actual
    protected void btnTranitesEtapaActual_Click(object sender, EventArgs e)
    {
        limpiaVariablesTramites();
        //llenatabas();
        titulosExpediente();
        pnlTramitesBtnActual.Visible = true;

        llenaGridTramites(gridTramitesActual, ocultoIDetapa.Value.ToString());
    }
	
	/*
    //botón para ejecutar los trámites de la siguiente etapa
    protected void btnTramitesEtapaSiguiente_Click(object sender, EventArgs e)
    {
        limpiaVariablesTramites();
        //llenatabas();
        titulosExpediente();
        pnlTramitesBtnSiguiente.Visible = true;

        //llenamos los trámitesde la siguiente etapa
        //SELECT fn_crea_tramites_sig_etapa(`param_id_expediente`, `param_id_flujo`, `param_id_etapa`, `param_id_usuario`)
        string[] datosCreaTramitesSigEtp = { ocultoIdExpediente.Value.ToString(), ocultoIDflujo.Value.ToString(), ocultoIDsiguienteEtapa.Value.ToString(), Session["idUsuario"].ToString() };
        DataTable tablaCreaTramSigEtp = Controladora.consultaDatos(sqlExpediente.ejecutaCreaTramitesSigEtp, datosCreaTramitesSigEtp);
        switch (Convert.ToInt16(tablaCreaTramSigEtp.Rows[0][0].ToString()))
        {
            case 1:
                //se crearon correctamente los trámites
                //lleno el grid
                llenaGridTramites(gridTramitesSiguiente, ocultoIDsiguienteEtapa.Value.ToString());
                break;
            case -1:
                //el expediente no esta en TRAMITE, no se puede crear el nuevo juego de tramites y documentos
                lblGeneralTramites.Text = "El expediente no esta en TRAMITE, no se puede crear el nuevo juego de tramites y documentos";
                lblGeneralTramites.CssClass = "labelError";
                lblGeneralTramites.Visible = true;
                break;
            case -2:
                //el id_etapa solicitado, no existe o no esta activo en el flujo existente del expediente
                lblGeneralTramites.Text = "El id_etapa solicitado, no existe o no esta activo en el flujo existente del expediente";
                lblGeneralTramites.CssClass = "labelError";
                lblGeneralTramites.Visible = true;
                break;
            case -3:
                //existen registros de tramites previos en esa etapa, con estatus de "no realizados"
                lblGeneralTramites.Text = "Existen registros de tramites previos en esa etapa, con estatus de no realizados";
                lblGeneralTramites.CssClass = "labelError";
                lblGeneralTramites.Visible = true;
                break;
            default:
                //no se puedieron crear los trámites de la siguente etapa, favor de intentarlo de nuevo código de error [X(002]
                lblGeneralTramites.Text = "No se puedieron crear los trámites de la siguente etapa, favor de intentarlo de nuevo código de error [X(002]";
                lblGeneralTramites.CssClass = "labelError";
                lblGeneralTramites.Visible = true;
                break;
        }
    }
	*/
	
    private void llenaGridTramites(GridView paramGridTramites, string paramIDetapa)
    {
        string[] datosListaTramites = { paramIDetapa, ocultoIdExpediente.Value.ToString(), ocultoIDflujo.Value.ToString() };
        paramGridTramites.DataSource = Controladora.consultaDatos(sqlExpediente.tramListaDoctosEtapa, datosListaTramites);
        paramGridTramites.DataBind();
    }

    //hace la selección en el grid para aceptar o eliminar el trámite
    protected void gridTramitesActual_SelectedIndexChanged(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        GridViewRow row = gridTramitesActual.SelectedRow;
        string idTramiteOpe = gridTramitesActual.DataKeys[row.RowIndex].Values["id_ope_tramite"].ToString();
        string estatus = gridTramitesActual.DataKeys[row.RowIndex].Values["estatus"].ToString();
        string nombreTramite = gridTramitesActual.DataKeys[row.RowIndex].Values["nombre_tramite"].ToString();
        //validamos el estatus para saber que ponemos en la BD, 0 no se ha cumplido con el trámite  1 Ya se cumplió con el trámite


        //valido si la etapa en la que me encuentro tiene un permiso especial

        string[] datosValidaEtapa = { ocultoIDetapa.Value.ToString() };
        DataTable tablaValidaEtapa = Controladora.consultaDatos(sqlExpediente.mValidaEtapa, datosValidaEtapa);
		
        if (tablaValidaEtapa.Rows.Count > 0)
        {
			//si tiene validaciones    [1]
            //pregunto si el usuario que tenemos esta en la tabla para dejarlo pasar, en caso contrario no le damos chance
            string[] datosValidaUsr = { Session["idUsuario"].ToString(), ocultoIDetapa.Value.ToString() };
            DataTable tablaValidaUsuario = Controladora.consultaDatos(sqlExpediente.mValidaUsuarioEtapa, datosValidaUsr);
			
			if (tablaValidaUsuario.Rows[0][0].ToString().Equals("1"))
            //if (tablaValidaUsuario.Rows.Count > 0)
            { 
				//si tiene permisos para aceptar el trámite       [2]
                switch (estatus)
                {
                    case "0":
                        //no ha cumplido con el trámite, actualizamos la tabla a 1
                        string[] datosCstsTramite = { "1", Session["idUsuario"].ToString(), idTramiteOpe };
                        if (Controladora.actualizaDatos(sqlExpediente.tramCstsTramite, datosCstsTramite))
                        {
                            //hizo los cambios, llenamos de nueva cuenta el grid
                            llenaGridTramites(gridTramitesActual, ocultoIDetapa.Value.ToString());
                            //mandamos el mensaje
                            lblGeneralTramites.Text = "Se ha cumplido con: <b>" + nombreTramite + "</b>";
                            lblGeneralTramites.CssClass = "labelNormal";
                            lblGeneralTramites.Visible = true;
                            //ocultoResultadoTramites.Value = "1";
                        }
                        else
                        {
                            //no hizo los cambios
                            lblGeneralTramites.Text = "Hubo un error al actualizar el sistema, favor de intentarlo de nuevo";
                            lblGeneralTramites.CssClass = "labelError";
                            lblGeneralTramites.Visible = false;
                            ocultoResultadoTramites.Value = "0";
                        }
                        break;
                    case "1":
                        //YA dió cumplimiento al trámite, lo eliminamos de la BD
                        string[] datosCstsTramiteElimina = { "0", "0", idTramiteOpe };
                        if (Controladora.actualizaDatos(sqlExpediente.tramCstsTramite, datosCstsTramiteElimina))
                        {
                            //hizo los cambios, llenamos de nueva cuenta el grid
                            llenaGridTramites(gridTramitesActual, ocultoIDetapa.Value.ToString());
                            //mandamos el mensaje
                            lblGeneralTramites.Text = "Se cambió el trámite: <b>" + nombreTramite + "</b> su estatus es de NO CUMPLIDO";
                            lblGeneralTramites.CssClass = "labelNormal";
                            lblGeneralTramites.Visible = true;
                            ocultoResultadoTramites.Value = "0";
                        }
                        else
                        {
                            //no hizo los cambios
                            lblGeneralTramites.Text = "Hubo un error al actualizar el sistema, favor de intentarlo de nuevo";
                            lblGeneralTramites.CssClass = "labelError";
                            lblGeneralTramites.Visible = false;
                            ocultoResultadoTramites.Value = "0";
                        }
                        break;
                    default:
                        //Trae un valor que no se reconoce
                        lblGeneralTramites.Text = "Existe una incosistencia en el sistema con el número XD001, favor de reportarlo a sistemas";
                        lblGeneralTramites.CssClass = "labelError";
                        lblGeneralTramites.Visible = false;
                        ocultoResultadoTramites.Value = "0";
                        break;
                }
            }
            else
            {//no tiene permisos, mandamos el mensaje       [2] 
                ventanaMensaje.Text = "No tiene permisos para modificar el estatus del trámite";
                ventanaOcultoTpoCierra.Value = "0";
                ventanaModal.Show();
            }
        }
        else
        { //no tiene validaciones   [1]
            //pueden validar los trámites
            switch (estatus)
            {
                case "0":
                    //no ha cumplido con el trámite, actualizamos la tabla a 1
                    string[] datosCstsTramite = { "1", Session["idUsuario"].ToString(), idTramiteOpe };
                    if (Controladora.actualizaDatos(sqlExpediente.tramCstsTramite, datosCstsTramite))
                    {
                        //hizo los cambios, llenamos de nueva cuenta el grid
                        llenaGridTramites(gridTramitesActual, ocultoIDetapa.Value.ToString());
                        //mandamos el mensaje
                        lblGeneralTramites.Text = "Se ha cumplido con: <b>" + nombreTramite + "</b>";
                        lblGeneralTramites.CssClass = "labelNormal";
                        lblGeneralTramites.Visible = true;
                        //ocultoResultadoTramites.Value = "1";
                    }
                    else
                    {
                        //no hizo los cambios
                        lblGeneralTramites.Text = "Hubo un error al actualizar el sistema, favor de intentarlo de nuevo";
                        lblGeneralTramites.CssClass = "labelError";
                        lblGeneralTramites.Visible = false;
                        ocultoResultadoTramites.Value = "0";
                    }
                    break;
                case "1":
                    //YA dió cumplimiento al trámite, lo eliminamos de la BD
                    string[] datosCstsTramiteElimina = { "0", "0", idTramiteOpe };
                    if (Controladora.actualizaDatos(sqlExpediente.tramCstsTramite, datosCstsTramiteElimina))
                    {
                        //hizo los cambios, llenamos de nueva cuenta el grid
                        llenaGridTramites(gridTramitesActual, ocultoIDetapa.Value.ToString());
                        //mandamos el mensaje
                        lblGeneralTramites.Text = "Se cambió el trámite: <b>" + nombreTramite + "</b> su estatus es de NO CUMPLIDO";
                        lblGeneralTramites.CssClass = "labelNormal";
                        lblGeneralTramites.Visible = true;
                        ocultoResultadoTramites.Value = "0";
                    }
                    else
                    {
                        //no hizo los cambios
                        lblGeneralTramites.Text = "Hubo un error al actualizar el sistema, favor de intentarlo de nuevo";
                        lblGeneralTramites.CssClass = "labelError";
                        lblGeneralTramites.Visible = false;
                        ocultoResultadoTramites.Value = "0";
                    }
                    break;
                default:
                    //Trae un valor que no se reconoce
                    lblGeneralTramites.Text = "Existe una incosistencia en el sistema con el número XD001, favor de reportarlo a sistemas";
                    lblGeneralTramites.CssClass = "labelError";
                    lblGeneralTramites.Visible = false;
                    ocultoResultadoTramites.Value = "0";
                    break;
            }
        }






    }

    //Se dispara cuando hacen clic en la imagen de los trámites de la etapa anterior, esto es para eliminarlo
    protected void gridTramitesAnterior_SelectedIndexChanged(object sender, EventArgs e)
    {
        //llenatabas();
        GridViewRow row = gridTramitesAnterior.SelectedRow;
        string idTramiteOpe = gridTramitesAnterior.DataKeys[row.RowIndex].Values["id_ope_tramite"].ToString();
        string estatus = gridTramitesAnterior.DataKeys[row.RowIndex].Values["estatus"].ToString();
        string nombreTramite = gridTramitesAnterior.DataKeys[row.RowIndex].Values["nombre_tramite"].ToString();
        ocultoIDtramiteEtpAnt.Value = idTramiteOpe;
        ocultoNombreTramiteEtpAnt.Value = nombreTramite;
        lblNombreTramiteEtpAnterior.Text = nombreTramite;
        modalTramitesAnterior.Show();
    }

    private void limpiaVariablesTramites()
    {
        pnlTramitesBtnAnterior.Visible = false;
        pnlTramitesBtnActual.Visible = false;
        pnlTramitesBtnSiguiente.Visible = false;
        lblGeneralTramites.Text = "";
        lblGeneralTramites.CssClass = "";
        lblGeneralTramites.Visible = false;
        gridTramitesAnterior.DataSource = "";
        gridTramitesAnterior.DataBind();
        gridTramitesActual.DataSource = "";
        gridTramitesActual.DataBind();
        gridTramitesSiguiente.DataSource = "";
        gridTramitesSiguiente.DataBind();
    }
    #endregion


    #region Validaciones
    //valida si todos los documentos ya fueron subidos
    protected void btnValidaDoctosEtapa_Click(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        //corremos la funcion
        string[] datosValidaDoctos = { ocultoIdExpediente.Value.ToString(), ocultoIDflujo.Value.ToString(), ocultoIDetapa.Value.ToString() };
        DataTable tablaValidaDoctos = Controladora.consultaDatos(sqlExpediente.ejecutaValidaDoctos, datosValidaDoctos);
        switch (Convert.ToInt16(tablaValidaDoctos.Rows[0][0].ToString()))
        {
            case -1:
                lblGeneralValidaciones.Text = "El expediente, tiene un estatus NO ACTIVO";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoDocumentos.Value = "0";
                break;
            case -4:
                lblGeneralValidaciones.Text = "Aún faltan documentos por incorporar";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoDocumentos.Value = "0";
                break;
            case -3:
                lblGeneralValidaciones.Text = "Existen Documentos aun no atendidos";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoDocumentos.Value = "0";
                break;
            case 1:
                lblGeneralValidaciones.Text = "Se ha cumplido con los documentos de la etapa";
                lblGeneralValidaciones.CssClass = "labelNormal";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoDocumentos.Value = "1";
                break;
            default:
                lblGeneralValidaciones.Text = "Hubo un error al verificar los documentos, favor de intentarlo de nuevo";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoDocumentos.Value = "0";
                break;
        }

        /*
		//VALIDAMOS LOS CAMPOS ESPECIALES
        if (ocultoIDflujo.Value.ToString().Equals("2") && Session["idEmpresa"].ToString().Equals("2") && ocultoIDetapa.Value.ToString().Equals("28"))
        {
            string[] datosCE = { ocultoIdExpediente.Value.ToString() };
            DataTable tablaCE = Controladora.consultaDatos(sqlExpediente.mValidaCamposEspeciales, datosCE);
            if (tablaCE.Rows.Count == 0)
            {
                lblGeneralValidaciones.Text = "No ha capturado los datos de la dispersión";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoTramites.Value = "0";
                ocultoResultadoDocumentos.Value = "0";
            }
        }
		*/

        if (ocultoResultadoDocumentos.Value.ToString().Equals("1") && ocultoResultadoTramites.Value.ToString().Equals("1"))
        {
            btnAvanzaEtapa.Enabled = true;
        }
        else
        {
            btnAvanzaEtapa.Enabled = false;
        }
    }
	
    //valida si se cumplió con los trámites
    protected void btnValidaTramitesEtapa_Click(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        //corremos la funcion
        string[] datosValidaTramites = { ocultoIdExpediente.Value.ToString(), ocultoIDflujo.Value.ToString(), ocultoIDetapa.Value.ToString() };
        DataTable tablaValidaDoctos = Controladora.consultaDatos(sqlExpediente.ejecutaValidaTramites, datosValidaTramites);
        switch (Convert.ToInt16(tablaValidaDoctos.Rows[0][0].ToString()))
        {
            case -1:
                lblGeneralValidaciones.Text = "El expediente, tiene un estatus NO ACTIVO";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoTramites.Value = "0";
                break;
            case -4:
                lblGeneralValidaciones.Text = "Aún faltan Trámites por incorporar";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoTramites.Value = "0";
                break;
            case -3:
                lblGeneralValidaciones.Text = "Existen tramites aun no resueltos";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoTramites.Value = "0";
                break;
            case 1:
                lblGeneralValidaciones.Text = "Se ha cumplido con los trámites de la etapa";
                lblGeneralValidaciones.CssClass = "labelNormal";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoTramites.Value = "1";
                break;
            default:
                lblGeneralValidaciones.Text = "Hubo un error al verificar los Trámites, favor de intentarlo de nuevo";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoTramites.Value = "0";
                break;
        }

        /*
		//VALIDAMOS LOS CAMPOS ESPECIALES
        if (ocultoIDflujo.Value.ToString().Equals("2") && Session["idEmpresa"].ToString().Equals("2") && ocultoIDetapa.Value.ToString().Equals("28"))
        {
            string[] datosCE = { ocultoIdExpediente.Value.ToString() };
            DataTable tablaCE = Controladora.consultaDatos(sqlExpediente.mValidaCamposEspeciales, datosCE);
            if (tablaCE.Rows.Count == 0)
            {
                lblGeneralValidaciones.Text = "No ha capturado los datos de la dispersión";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoTramites.Value = "0";
                ocultoResultadoDocumentos.Value = "0";
            }
        }
		*/

        if (ocultoResultadoDocumentos.Value.ToString().Equals("1") && ocultoResultadoTramites.Value.ToString().Equals("1"))
        {
            btnAvanzaEtapa.Enabled = true;
        }
        else
        {
            btnAvanzaEtapa.Enabled = false;
        }
    }
    protected void btnAvanzaEtapa_Click(object sender, EventArgs e)
    {
		// valida tramites
		string[] datosValidaTramites = { ocultoIdExpediente.Value.ToString(), ocultoIDflujo.Value.ToString(), ocultoIDetapa.Value.ToString() };
        DataTable tablaValidaTramites = Controladora.consultaDatos(sqlExpediente.ejecutaValidaTramites, datosValidaTramites);
        switch (Convert.ToInt16(tablaValidaTramites.Rows[0][0].ToString()))
        {
            case -1:
                lblGeneralValidaciones.Text = "El expediente, tiene un estatus NO ACTIVO";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoTramites.Value = "0";
                break;
            case -4:
                lblGeneralValidaciones.Text = "Aún faltan Trámites por incorporar";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoTramites.Value = "0";
                break;
            case -3:
                lblGeneralValidaciones.Text = "Existen tramites aun no resueltos";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoTramites.Value = "0";
                break;
            case 1:
                lblGeneralValidaciones.Text = "Se ha cumplido con los trámites de la etapa";
                lblGeneralValidaciones.CssClass = "labelNormal";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoTramites.Value = "1";
                break;
            default:
                lblGeneralValidaciones.Text = "Hubo un error al verificar los Trámites, favor de intentarlo de nuevo";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoTramites.Value = "0";
                break;
        }

		//valida documentos
		string[] datosValidaDoctos = { ocultoIdExpediente.Value.ToString(), ocultoIDflujo.Value.ToString(), ocultoIDetapa.Value.ToString() };
        DataTable tablaValidaDoctos = Controladora.consultaDatos(sqlExpediente.ejecutaValidaDoctos, datosValidaDoctos);
        switch (Convert.ToInt16(tablaValidaDoctos.Rows[0][0].ToString()))
        {
            case -1:
                lblGeneralValidaciones.Text = "El expediente, tiene un estatus NO ACTIVO";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoDocumentos.Value = "0";
                break;
            case -4:
                lblGeneralValidaciones.Text = "Aún faltan documentos por incorporar";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoDocumentos.Value = "0";
                break;
            case -3:
                lblGeneralValidaciones.Text = "Existen Documentos aun no atendidos";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoDocumentos.Value = "0";
                break;
            case 1:
                lblGeneralValidaciones.Text = "Se ha cumplido con los documentos de la etapa";
                lblGeneralValidaciones.CssClass = "labelNormal";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoDocumentos.Value = "1";
                break;
            default:
                lblGeneralValidaciones.Text = "Hubo un error al verificar los documentos, favor de intentarlo de nuevo";
                lblGeneralValidaciones.CssClass = "labelError";
                lblGeneralValidaciones.Visible = true;
                ocultoResultadoDocumentos.Value = "0";
                break;
        }

					
        if (ocultoResultadoDocumentos.Value.ToString().Equals("1") && ocultoResultadoTramites.Value.ToString().Equals("1"))
        {
            string montoApartar = "";
            string idCuenta = "";
            string fnOK = "1";
            //verifico si es gpo o microcreditos
            string[] paramExpediente = { ocultoIdExpediente.Value.ToString() };
            DataTable buscaEmpresa = Controladora.consultaDatos(sqlExpediente.buscaEmpresa, paramExpediente);
            
            //no debe ser expediente fovissste
            if (buscaEmpresa.Rows[0]["id_empresa"].ToString() != "1")
            {
                if (!fnOK.Equals("1"))
                {
                    lblGeneralValidaciones.Text = fnOK.ToString();
                    lblGeneralValidaciones.CssClass = "labelError";
                    return;
                }
            }
			
			//aqui voy
			//hay que revisar, si existe alguna validacion que se deba cumplir antes de poder hacer el avance de la etapa
			string[] datosValidaAvanceEtapa = { ocultoIdExpediente.Value.ToString(), Session["idUsuario"].ToString() };
			
                string errorSQL = Controladora.regresaSentenciaSQL(sqlExpediente.ValidaAvanceEtapa, datosValidaAvanceEtapa);
                string errorURL = HttpContext.Current.Request.Url.AbsolutePath.ToString();
                string errorIdent = "ValidaAvanceEtapa";
                Utilerias.registraSQL(errorSQL, errorURL, errorIdent);

			
			DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.ValidaAvanceEtapa, datosValidaAvanceEtapa);

			//string[] datosResultadoAvanceEtapa = { ocultoIdExpediente.Value.ToString(), Session["idUsuario"].ToString() };
			DataTable tablaResultadoAvanceEtapa = Controladora.consultaDatos(sqlExpediente.ResultadoAvanceEtapa, datosValidaAvanceEtapa);
			if (tablaResultadoAvanceEtapa.Rows.Count > 0)
			{
				//detenemos el proceso porque regreso un dato incorrecto
				ventanaOcultoTpoCierra.Value = "0";
				ventanaMensaje.Text = tablaResultadoAvanceEtapa.Rows[0][3].ToString();
				ventanaModal.Show();
				//error = false;
				//break;
			}
			else
			{
				//revisa, si se debe hacer la ejecucion de un proceso al momento de hacer el avance de etapa			
				DataTable buscaProceso = Controladora.consultaDatos(sqlExpediente.buscaProceso, paramExpediente);  
				if (buscaProceso.Rows.Count > 0 )
				{
					//lanza la ejecucion de un proceso
					string[] datosSPLanzaProceso = { ocultoIdExpediente.Value.ToString(), Session["idUsuario"].ToString() };
					DataTable tablaSPLanzaProceso = Controladora.consultaDatos(sqlExpediente.spLanzaProcesoAvanceEtapa, datosSPLanzaProceso);		
					MsgBox("Se lanzado el proceso " + buscaProceso.Rows[0]["titulo_proceso"].ToString() + " de esta etapa", this.Page, this);
				}
			
				//Hace el avance de etapa, pero revisa, si en esta etapa existe un objeto que tiene una decision y en consecuencia la 
				//siguiente etapa va a cambiar dependiendo de la condicion existente
				string[] datosAvanzaEtapa = { ocultoIdExpediente.Value.ToString(), ocultoIDflujo.Value.ToString(), ocultoIDetapa.Value.ToString(), Session["idUsuario"].ToString(), ocultoValorCondicion.Value.ToString() };

				DataTable tablaAvanzaEtapa = Controladora.consultaDatos(sqlExpediente.ejecutaAvanzaEtapaCondicion, datosAvanzaEtapa);
				switch (Convert.ToInt16(tablaAvanzaEtapa.Rows[0][0].ToString()))
				{
					case -1:
						lblGeneralValidaciones.Text = "-1";
						lblGeneralValidaciones.CssClass = "labelError";
						lblGeneralValidaciones.Visible = true;
						break;
					case -2:
						lblGeneralValidaciones.Text = "-2: No se encuentra la siguiente etapa";
						lblGeneralValidaciones.CssClass = "labelError";
						lblGeneralValidaciones.Visible = true;
						break;
					case -3:
						lblGeneralValidaciones.Text = "-3: Final del Flujo";
						lblGeneralValidaciones.CssClass = "labelError";
						lblGeneralValidaciones.Visible = true;
						break;
					case 1:
						//agregamos las observaciones
						modalAvisoEtpSig.Show();
						break;
					default:
						lblGeneralValidaciones.Text = "Error genérico";
						lblGeneralValidaciones.CssClass = "labelError";
						lblGeneralValidaciones.Visible = true;
						break;				
				}

			}
			
			
        }
        else
        {
            lblGeneralValidaciones.Text = "Favor de validar documentos o trámites";
            lblGeneralValidaciones.CssClass = "labelError";
            lblGeneralValidaciones.Visible = true;
            //btnAvanzaEtapa.Enabled = false;
        }

    }
	
    //cierra modal aviso etapa siguiente
    protected void btnCierraAvisosEtpSig_Click(object sender, ImageClickEventArgs e)
    {
        //al cerrar debemos actuaizar la página para que vuelvan a buscar el expediente
        modalAvisoEtpSig.Hide();
        direcciona();
    }
	
    //acepta aviso etapa siguiente
    protected void btnAceptaEtapaSig_Click(object sender, EventArgs e)
    {
        modalAvisoEtpSig.Hide();
        direcciona();
    }
	
    private void direcciona()
    {
        Response.Redirect("~/opeExpediente.aspx");
    }
    #endregion


    private void llenaTabs()
    {
        llenaCotizacion();
        llenaPreliminar();
        llenaGestion();
        llenaCertificado();
        llenaMuestreo();
        llenaCliente();
		llenaNorma();
		llenaNormasFamilias();
		llenaFacturasProyecto();
		llenaCertificadosProyecto();


        //llenaDatosTramite();
        //llenaDatosDocumentos();

    }
    protected void btnCierraModalTramAnt_Click(object sender, ImageClickEventArgs e)
    {
        modalTramitesAnterior.Hide();
    }
    protected void btnCierraModalTramitesAnterior_Click(object sender, EventArgs e)
    {
        modalTramitesAnterior.Hide();
    }
    //elimina trámite y regresa a la etapa anterior
    protected void btnEliminaTramEtpAnt_Click(object sender, EventArgs e)
    {
        //regresamos a la etapa anterior
        //validamos si las observaciones esta capturadas
        if (txtObservaTrmEtp.Text.ToString().Trim().Equals(""))
        {
            lblGeneralModalTramEtAnt.Text = "Debe capturar las observaciones";
            lblGeneralModalTramEtAnt.CssClass = "labelError";
            lblGeneralModalTramEtAnt.Visible = true;
            modalTramitesAnterior.Show();
            return;
        }
        string[] datosFnRegresaEtAnt = { ocultoIdExpediente.Value.ToString(), ocultoIDflujo.Value.ToString(), ocultoIDetapa.Value.ToString(), Session["idUsuario"].ToString() };
        DataTable tablaFnRegresaEtAnt = Controladora.consultaDatos(sqlExpediente.fnRegresaEtAnt, datosFnRegresaEtAnt);
        if (tablaFnRegresaEtAnt.Rows[0][0].ToString().Equals("1"))
        {
            //Ya dió cumplimiento al trámite, lo eliminamos de la BD
            string[] datosCstsTramiteElimina = { "0", "0", ocultoIDtramiteEtpAnt.Value.ToString() };
            if (Controladora.actualizaDatos(sqlExpediente.tramCstsTramite, datosCstsTramiteElimina))
            {
                //hizo los cambios
                //agregamos las observaciones
                string[] datosObservaRegresaEtapa = { ocultoIdExpediente.Value.ToString(), ocultoIDetapa.Value.ToString(), "2", txtObservaRegresaEtapa.Text, Session["idUsuario"].ToString(), "0" };
                if (!Controladora.registraDatos(sqlExpediente.aObsEtapa, datosObservaRegresaEtapa))
                {
                    lblGeneralDoctos.Text = "Se regresó a la etapa anterior con éxito, pero las observaciones a la etapa no se pudieron registrar, deberá realizar de nuevo la búsqueda del registro.";
                    lblGeneralDoctos.CssClass = "labelError";
                    lblGeneralDoctos.Visible = true;
                }
                else
                {
                    //si funciono y regreso a la busqueda
                    direcciona();
                }
            }
            else
            {
                //no hizo los cambios
                direcciona();
            }
        }
        else
        {
            lblGeneralModalTramEtAnt.Text = "No se pudo regresar a la fase anterior, favor de intentar nuevamente";
            lblGeneralModalTramEtAnt.CssClass = "labelError";
            lblGeneralModalTramEtAnt.Visible = false;
        }
    }
    protected void btnAgregaObsEtapa_Click(object sender, EventArgs e)
    {
        string[] datosObsEtapa = { ocultoIdExpediente.Value.ToString(), ocultoIDetapa.Value.ToString(), "1", txtObsEtapa.Text.ToString(), Session["idUsuario"].ToString(), "0" };
        if (Controladora.registraDatos(sqlExpediente.aObsEtapa, datosObsEtapa))
        {
            //se insertaron correctamente los datos
            //<OBS>
            //mostramos las observaciones
            HtmlControl htmlFrameObsEtapa = (HtmlControl)frameObsEtapa;
            htmlFrameObsEtapa.Attributes["src"] = "varios/frameObsEtapa.aspx?id_expediente=" + ocultoIdExpediente.Value.ToString() + "&id_tpo_obs=1&id_etapa=";
            //</OBS>
            txtObsEtapa.Text = "";
        }
        else
        {
            //no se insertaron datos
            lblGeneral.Text = "No se pudo insertar la observación";
            lblGeneral.CssClass = "labelError";
            lblGeneral.Visible = true;
        }
    }

    //regresa el expediente al inicio de todas las etapas
    protected void btnRegresaExpInicioEtp_Click(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        modalVentanaRegresaEtapaInicial.Show();
    }

    //captura las observaciones del regreso a la etapa inicial
    protected void btnAObsEtapaInicial_Click(object sender, EventArgs e)
    {
        //validamos que se capture el texto de las observaciones
        if (txtObsRegresaEtapaInicial.Text.ToString().Trim().Equals(""))
        {
            lblGeneralRegresaEtapaInicial.Text = "Es necesario que capture las observaciones";
            lblGeneralRegresaEtapaInicial.CssClass = "labelError";
            lblGeneralRegresaEtapaInicial.Visible = true;
            modalVentanaRegresaEtapaInicial.Show();
            return;
        }
        //insertamos las observaciones de la etapa inicial
        string[] datosObsRegresaEtapaInicial = { ocultoIdExpediente.Value.ToString(), ocultoIDetapa.Value.ToString(), "4", txtObsRegresaEtapaInicial.Text.ToString(), Session["idUsuario"].ToString(), "0" };
        if (Controladora.registraDatos(sqlExpediente.aObsEtapa, datosObsRegresaEtapaInicial))
        {//inserto correctamente
            string[] datosRegresaInicio = { ocultoIdExpediente.Value.ToString(), ocultoIDflujo.Value.ToString(), ocultoIDetapa.Value.ToString(), Session["idUsuario"].ToString() };
            DataTable tablaRegresaEtapaInicio = Controladora.consultaDatos(sqlExpediente.fnRegresaEtapaInicial, datosRegresaInicio);
            switch (Convert.ToInt16(tablaRegresaEtapaInicio.Rows[0][0].ToString()))
            {
                case 1:
                    //la función regresa que todo esta correcto
                    modalVentanaRegresaEtapaInicial.Hide();
                    lblTextoAviso.Text = "Se regreso el expediente a la etapa inicial";
                    modalVentanaAviso.Show();
                    break;
                default:
                    modalVentanaRegresaEtapaInicial.Hide();
                    lblTextoAviso.Text = "Hubo un error al regresar el documento a la etapa inicial";
                    modalVentanaAviso.Show();
                    break;
            }
        }
        else
        {//no inserto las observaciones en el sistema
            modalVentanaRegresaEtapaInicial.Hide();
            lblTextoAviso.Text = "No se insertaron las observaciones, favor de intentarlo de nuevo";
            modalVentanaAviso.Show();
            return;
        }
    }
    protected void cierraModalRegresaEtapaInicial_Click(object sender, ImageClickEventArgs e)
    {
        modalVentanaRegresaEtapaInicial.Hide();
    }
    protected void cierraVentanaAvisoRegresaEtapaInicial_Click(object sender, ImageClickEventArgs e)
    {
        direcciona();
    }
    protected void btnAvisoRegresaEtapaInicial_Click(object sender, EventArgs e)
    {
        direcciona();
    }

    //muestra la modal para regresar el expediente a la etapa anterior
    protected void btnRegresaEtpAnterior_Click(object sender, EventArgs e)
    {
        modalVentanaRegresEtapaAnterior.Show();
    }
    protected void cierraVentanaRegresaEtapaAnteior_Click(object sender, ImageClickEventArgs e)
    {
        modalVentanaRegresEtapaAnterior.Hide();
    }
    //regresa el expediente a la etapa anterior
    protected void btnAobsRegresaEtapaAnterior_Click(object sender, EventArgs e)
    {
        //regresamos a la etapa anterior
        string[] datosFnRegresaEtAnt = { ocultoIdExpediente.Value.ToString(), ocultoIDflujo.Value.ToString(), ocultoIDetapa.Value.ToString(), Session["idUsuario"].ToString() };
        DataTable tablaFnRegresaEtAnt = Controladora.consultaDatos(sqlExpediente.fnRegresaEtAnt, datosFnRegresaEtAnt);
        if (tablaFnRegresaEtAnt.Rows[0][0].ToString().Equals("1"))
        {
            //se regresó con éxito a la etapa anterior.
            //capturo las observaciones
            string[] datosObservaRegresaEtapa = { ocultoIdExpediente.Value.ToString(), ocultoIDetapa.Value.ToString(), "2", txtAobsRegresaEtapaAnterior.Text, Session["idUsuario"].ToString(), "0" };
            if (!Controladora.registraDatos(sqlExpediente.aObsEtapa, datosObservaRegresaEtapa))
            {
                //no se capturaron las observaciones, mandó error
                modalVentanaRegresEtapaAnterior.Hide();
                lblTextoAviso.Text = "Se regresó a la etapa anterior, pero no se pudieron capturar las observaciones";
                modalVentanaAviso.Show();
            }
            else
            {
                //si se registraron las observaciones y se registro el regreso de etapa
                modalVentanaRegresEtapaAnterior.Hide();
                lblTextoAviso.Text = "El expediente se regresó a la etapa anterior";
                modalVentanaAviso.Show();
            }
        }
        else
        {
            //no se pudo regresar a la etapa anterior
            modalVentanaRegresEtapaAnterior.Hide();
            lblTextoAviso.Text = "El expediente no se pudo regresar a la etapa anterior, deberá intentarlo de nuevo";
            modalVentanaAviso.Show();
        }
    }

    //abre la modal para ver el cálculo de la tasa de interés
    protected void btnVerCalculoTasaInteres_Click(object sender, EventArgs e)
    {//abre la modal para ver el cálculo de la tasa de interés, debe ir a verificar si ya existe un cálculo, si existe muestra la información
        //en ambos caso puede generar un nuevo cálculo
        //llenatabas();
        titulosExpediente();

        //obtengo la información para generar el calculo de la tasa
        string[] datosScredito = { ocultoIdExpediente.Value.ToString() };
        DataTable tablaDatosCredito = Controladora.consultaDatos(sqlExpediente.sDatosCredito, datosScredito);

        bool validaDatos;
        validaDatos = true;

        //validamos que tenga datos el query
        if (tablaDatosCredito.Rows.Count > 0)
        {//el registro si tiene datos de información
            if (tablaDatosCredito.Rows[0]["sueldo"].ToString().Equals("0")) validaDatos = false; else txtSueldo.Text = tablaDatosCredito.Rows[0]["sueldo"].ToString();
            if (tablaDatosCredito.Rows[0]["capital"].ToString().Equals("0")) validaDatos = false; else txtMontoCredito.Text = tablaDatosCredito.Rows[0]["capital"].ToString();
            if (tablaDatosCredito.Rows[0]["plazo"].ToString().Equals("0")) validaDatos = false; else txtPlazo.Text = tablaDatosCredito.Rows[0]["plazo"].ToString();
            if (tablaDatosCredito.Rows[0]["periodo"].ToString().Equals("0")) validaDatos = false; else Utilerias.seleccionaValorComboRequest(listaPeriodo, tablaDatosCredito.Rows[0]["periodo"].ToString());
            if (tablaDatosCredito.Rows[0]["cliente"].ToString().Equals("0")) validaDatos = false; else Utilerias.seleccionaValorComboRequest(listaTipoPersona, tablaDatosCredito.Rows[0]["cliente"].ToString());

            //validamos, si la variable validaDAtos trae false, entonces no corremos el proceso de la tasa de interes para que no falle
            if (validaDatos)
            {
                //mandamois a llamar la función fn_mic_simulador_credito la cual nos regresa el id de la tabla en donde genero los calculos
                string[] datosFnMicSimCrd = { txtSueldo.Text.ToString(), txtMontoCredito.Text.ToString(), txtPlazo.Text.ToString(), listaPeriodo.SelectedValue.ToString(), listaTipoPersona.SelectedValue.ToString(), ocultoIdExpediente.Value.ToString() };
                DataTable tablaFnMicSimCrd = Controladora.consultaDatos(sqlFinancieras.fnMicSimuladorCrd, datosFnMicSimCrd);
                //me regresa el id de la tabla, con el cual obtengo los datos para el calculo de la tasa
                string idTabla = tablaFnMicSimCrd.Rows[0][0].ToString();
                generaTasaTablas(idTabla);
            }
        }

        modalVentanaTI.Show();
    }

    private void generaTasa(string pIdExpediente)
    {
        inicializaValores();
        bool error;

        error = false;

        foreach (Control txt in this.pnlCalculo.Controls)
        {
            if (txt is TextBox)
            {
                if (((TextBox)txt).Text.ToString().Trim().Equals(""))
                {
                    lblGnPnlCalculo.Text = "Debe capturar información";
                    lblGnPnlCalculo.CssClass = "labelError";
                    lblGnPnlCalculo.Visible = true;
                    ((TextBox)txt).BackColor = Color.Wheat;
                    ((TextBox)txt).Focus();
                    error = true;
                    return;
                }
                if (error) break; else error = false;
            }
        }

        //validamos que solo permita números del 1 al 12, 18 o 24
        int validaPlazo;
        bool resultadoPlazo;
        validaPlazo = 0;
        resultadoPlazo = false;
        validaPlazo = Convert.ToInt16(txtPlazo.Text.ToString());
        if ((validaPlazo > 0 && validaPlazo < 13) || (validaPlazo == 18) || (validaPlazo == 24))
        {
            resultadoPlazo = true;
        }
        if (!resultadoPlazo)
        {
            lblGnPnlCalculo.Text = "El plazo solo puede ser entre 1 y 12 meses o 18 o 24 meses";
            lblGnPnlCalculo.CssClass = "labelError";
            lblGnPnlCalculo.Visible = true;
            return;
        }

        //mandamois a llamar la función fn_mic_simulador_credito la cual nos regresa el id de la tabla en donde genero los calculos
        string[] datosFnMicSimCrd = { txtSueldo.Text.ToString(), txtMontoCredito.Text.ToString(), txtPlazo.Text.ToString(), listaPeriodo.SelectedValue.ToString(), listaTipoPersona.SelectedValue.ToString(), pIdExpediente };
        DataTable tablaFnMicSimCrd = Controladora.consultaDatos(sqlFinancieras.fnMicSimuladorCrd, datosFnMicSimCrd);
        //me regresa el id de la tabla, con el cual obtengo los datos para el calculo de la tasa
        string idTabla = tablaFnMicSimCrd.Rows[0][0].ToString();
        generaTasaTablas(idTabla);
    }

    private void generaTasaTablas(string idTabla)
    {
        double tasa, plazo, pagoMensual, prestamo;
        tasa = 0;
        plazo = 0;
        pagoMensual = 0;
        prestamo = 0;

        //obtenemos lso datos de la tabla
        string[] datosMcalCrd = { idTabla };
        DataTable tablaCalCrd = Controladora.consultaDatos(sqlFinancieras.mCalcCrd, datosMcalCrd);
        plazo = Convert.ToDouble(tablaCalCrd.Rows[0][0].ToString());
        pagoMensual = (Convert.ToDouble(tablaCalCrd.Rows[0][1].ToString()) * -1);
        prestamo = Convert.ToDouble(tablaCalCrd.Rows[0][2].ToString());
        tasa = Financial.Rate(plazo, pagoMensual, prestamo, 0, 0);
        //actualizamos los datos de la tasa para generar los calculos
        string[] datosTasaCalculada = { idTabla, tasa.ToString(), ocultoIdExpediente.Value.ToString(), Session["idUsuario"].ToString() };
        DataTable tablaTasaCalculada = Controladora.consultaDatos(sqlFinancieras.fnTasaCalculada, datosTasaCalculada);
        string[] datosIDTabla = { idTabla };
        //generamos la tabla dinamica para hacerla vertical
        Hashtable hshParam = (Hashtable)Session["sParametros"];
        DataTable tablaDatosCrd = Controladora.consultaDatos(sqlFinancieras.mDatosCredito, datosIDTabla);

        TableRow encabezado = new TableRow();
        TableCell columna = new TableCell();
        columna.ColumnSpan = 2;
        columna.Attributes.Add("bgColor", hshParam["colorTD"].ToString());
        columna.Text = "Información del expediente";
        columna.ForeColor = Color.White;
        columna.Font.Bold = true;
        columna.HorizontalAlign = HorizontalAlign.Center;

        encabezado.Cells.Add(columna);

        //Add header to a table
        htmlTablaDatosCrd.Rows.Add(encabezado);

        for (int i = 0; i < tablaDatosCrd.Columns.Count; i++)
        {
            TableRow renglon = new TableRow();

            TableCell colTitulo = new TableCell();
            TableCell colDatos = new TableCell();

            colTitulo.Attributes.Add("align", "left");
            colTitulo.Attributes.Add("bgcolor", hshParam["colorTD"].ToString());
            colTitulo.Text = tablaDatosCrd.Columns[i].ColumnName.ToString();
            colTitulo.ForeColor = Color.White;
            colDatos.Attributes.Add("align", "left");
            colDatos.Text = tablaDatosCrd.Rows[0][i].ToString();

            //insert cells into a row
            renglon.Cells.Add(colTitulo);
            renglon.Cells.Add(colDatos);

            //insert row into a table
            htmlTablaDatosCrd.Rows.Add(renglon);
        }

        gridVenta.DataSource = Controladora.consultaDatos(sqlFinancieras.mDatosVenta, datosIDTabla);
        gridVenta.DataBind();
        gridInsoluto.DataSource = Controladora.consultaDatos(sqlFinancieras.mDatosInsoluto, datosIDTabla);
        gridInsoluto.DataBind();
        pnlResultados.Visible = true;
    }

    //inicializa los valores del panel de calcula tasa de interés
    private void inicializaValores()
    {
        gridInsoluto.DataSource = null;
        gridInsoluto.DataBind();
        gridVenta.DataSource = null;
        gridVenta.DataBind();
        lblGnPnlCalculo.Text = "";
        lblGnPnlCalculo.Visible = false;
        txtMontoCredito.BackColor = Color.White;
        txtPlazo.BackColor = Color.White;
        txtSueldo.BackColor = Color.White;
    }

    //cierra la ventana modal del calculo de interes
    protected void cierraVentanaPnlTI_Click(object sender, ImageClickEventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        modalVentanaTI.Hide();
    }
    protected void btnCalcTIprevio_Click(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        generaTasa("0");
        modalVentanaTI.Show();
    }
    protected void btnCalcTIGraba_Click(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        generaTasa(ocultoIdExpediente.Value.ToString());
        modalVentanaTI.Show();
    }





    #region VENTANA
    protected void ventanaCierraX_Click(object sender, ImageClickEventArgs e)
    {
        if (ventanaOcultoTpoCierra.Value.ToString().Equals("1"))
        {
            direcciona();
        }
        else
        {
            //llenatabas();
            titulosExpediente();
            ventanaModal.Hide();
        }
    }
    protected void ventanaBotonCerrar_Click(object sender, EventArgs e)
    {
        if (ventanaOcultoTpoCierra.Value.ToString().Equals("1"))
        {
            direcciona();
        }
        else
        {
            //llenatabas();
            titulosExpediente();
            ventanaModal.Hide();
        }
    }
    #endregion VENTANA

    #region cOpeVivienda
    //PERMITE ABRIR LA VENTANA PARA MODIFICAR LOS DATOS DE OPE_VIVIENDA
    protected void btnCopeVivienda_Click(object sender, EventArgs e)
    {
        //obtenemso los datos
        string[] datosOpeVivienda = { ocultoIdExpediente.Value.ToString() };
        DataTable tablaOpeVivienda = Controladora.consultaDatos(sqlExpediente.mOpeVivienda, datosOpeVivienda);
        if (tablaOpeVivienda.Rows.Count > 0)
        {// si trae datos
            //seleccionamos el item que traigo de la tabla
            Utilerias.seleccionaValorComboRequest(listaEntidad, tablaOpeVivienda.Rows[0]["id_entidad"].ToString());
            txtMunicipio.Text = tablaOpeVivienda.Rows[0]["municipio"].ToString();
            txtDireccion.Text = tablaOpeVivienda.Rows[0]["direccion"].ToString();
            txtCp.Text = tablaOpeVivienda.Rows[0]["cp"].ToString();
            txtCiudad.Text = tablaOpeVivienda.Rows[0]["ciudad"].ToString();
            txtColonia.Text = tablaOpeVivienda.Rows[0]["colonia"].ToString();
            txtVivTelefono.Text = tablaOpeVivienda.Rows[0]["telefono"].ToString();
            txtVivCelular.Text = tablaOpeVivienda.Rows[0]["celular"].ToString();
            ocultoOpeViviendaDatos.Value = "1";
        }
        else
        {//ope_vivienda no trae datos
            ocultoOpeViviendaDatos.Value = "0";
        }
        modalWinCopeViv.Show();
    }

    //LLENA LISTA ENTIDAD
    private void listaEntidad_Fill()
    {
        string[] datosListaE = { string.Empty };
        listaEntidad = Utilerias.llenaLista(listaEntidad, sqlExpediente.mListaEstados, datosListaE, "txt", "id", "");
    }


    private void listaTipoServicio_Fill()
    { 
        string[] datosTipoServicio = {string.Empty};
        Utilerias.llenaLista(listaTipoServicio, sqlCatalogos.listaTipoServicio, datosTipoServicio, "txt", "id", "");
    }


    private void listaCliente_Fill()
    { 
        string[] datosCliente = {string.Empty};
        Utilerias.llenaLista(listaCliente, sqlCatalogos.listaCliente, datosCliente, "txt", "id", "");
    }

	private void listaNorma_Fill()
    { 
        string[] datosNorma = {string.Empty};
        Utilerias.llenaLista(listaNorma, sqlCatalogos.listaNorma, datosNorma, "txt", "id", "");
    }
	
    protected void btnWinCcierraCopeV_Click(object sender, ImageClickEventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        modalWinCopeViv.Hide();
    }
    //MODIFICA LOS DATOS DE OPE_VIVIENDA
    protected void btnWinCopeViv_Click(object sender, EventArgs e)
    {
        //validamos que contenga datos
        bool error;
        error = false;
        foreach (Control txt in this.pnlWinCopeVivienda.Controls)
        {
            if (txt is TextBox)
            {
                if (((TextBox)txt).Text.ToString().Trim().Equals(""))
                {
                    lblWinMensajeCopeViv.Text = "Debe capturar información";
                    lblWinMensajeCopeViv.CssClass = "labelError";
                    lblWinMensajeCopeViv.Visible = true;
                    ((TextBox)txt).BackColor = Color.Gray;
                    ((TextBox)txt).Focus();
                    error = true;
                    modalWinCopeViv.Show();
                    return;
                }
                if (error)
                {
                    modalWinCopeViv.Show();
                    break;
                }
                else error = false;
            }
        }

        string vivPais = "0";
        string vivEntidad = listaEntidad.SelectedValue.ToString();
        string vivMunicipio = txtMunicipio.Text.ToString();
        string vivDireccion = txtDireccion.Text.ToString();
        string vivCP = txtCp.Text.ToString();
        string vivCiudad = txtCiudad.Text.ToString();
        string vivColonia = txtColonia.Text.ToString();
        string vivTelefono = txtVivTelefono.Text.ToString();
        string vivCelular = txtVivCelular.Text.ToString();
        string idEstatusExpediente = "2";
        string vivCUV = "0";
        string vivIdTipoVivienda = "1";
        string vivIdOferente = "0";
        string vivIdConsorcio = "0";
        string id_expediente = ocultoIdExpediente.Value.ToString();

        if (ocultoOpeViviendaDatos.Value.ToString().Equals("0"))
        {//insertamos como si fuera la primera vez
            string[] datosOpeVivienda = { vivPais, vivEntidad, vivMunicipio, vivDireccion, vivCP, vivCiudad, vivColonia, vivTelefono, vivCelular, id_expediente, idEstatusExpediente, vivCUV, vivIdTipoVivienda, vivIdOferente, vivIdConsorcio };

            if (Controladora.registraDatos(sqlExpediente.aOpeVivienda, datosOpeVivienda))
            {//SE INSERTARON LOS DATOS EN OPE_VIVIENDA
                modalWinCopeViv.Hide();
                ventanaOcultoTpoCierra.Value = "0";
                ventanaMensaje.Text = "Se capturaron correctamente los datos";
                //llenatabas();
                titulosExpediente();
                ventanaModal.Show();
            }
            else
            {//no se insertaron datos en ope_vivienda
                //capturo el tipo de error
                string errorSQL = Controladora.regresaSentenciaSQL(sqlExpediente.aOpeVivienda, datosOpeVivienda);
                string errorURL = HttpContext.Current.Request.Url.AbsolutePath.ToString();
                string errorIdent = "aOpeVivienda";
                Utilerias.registraSQL(errorSQL, errorURL, errorIdent);

                ventanaOcultoTpoCierra.Value = "0";
                ventanaMensaje.Text = "No se pudo capturar la información, favor de validar y volver a intentar";
                //llenatabas();
                titulosExpediente();
                modalWinCopeViv.Show();
                ventanaModal.Show();
            }
        }
        else
        {//actualizamos ope_vivienda
            string[] datosCopeVivienda = { vivPais, vivEntidad, vivMunicipio, vivDireccion, vivCP, vivCiudad, vivColonia, vivTelefono, vivCelular, id_expediente };
            if (Controladora.actualizaDatos(sqlExpediente.cOpeVivienda, datosCopeVivienda))
            {//actualizó ope_vivienda
                modalWinCopeViv.Hide();
                ventanaOcultoTpoCierra.Value = "0";
                ventanaMensaje.Text = "Se capturaron correctamente los datos";
                //llenatabas();
                titulosExpediente();
                ventanaModal.Show();
            }
            else
            {//no actualizó ope_vivienda
                //capturo el tipo de error
                string errorSQL = Controladora.regresaSentenciaSQL(sqlExpediente.cOpeVivienda, datosCopeVivienda);
                string errorURL = HttpContext.Current.Request.Url.AbsolutePath.ToString();
                string errorIdent = "cOpeVivienda";
                Utilerias.registraSQL(errorSQL, errorURL, errorIdent);

                ventanaOcultoTpoCierra.Value = "0";
                ventanaMensaje.Text = "No se pudo capturar la información, favor de validar y volver a intentar";
                //llenatabas();
                titulosExpediente();
                ventanaModal.Show();
            }
        }
    }
    #endregion cOpeVivienda

    #region cOpeLaborales
    //cierra ventana modal de ope_laborales
    protected void btnWinCierraCopeLab_Click(object sender, ImageClickEventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        modalWinCopeLab.Hide();
    }
    //MODIFICA DATOS OPE_LABORALES
    protected void btnCLaborales_Click(object sender, EventArgs e)
    {
        //consulto datos
        string[] datosOpeLaborales = { ocultoIdExpediente.Value.ToString() };
        DataTable tablaOpeLaborales = Controladora.consultaDatos(sqlExpediente.mOpeLaborales, datosOpeLaborales);
        if (tablaOpeLaborales.Rows.Count > 0)
        {//si trae datos
            txtLabDependencia.Text = tablaOpeLaborales.Rows[0]["dependencia"].ToString();
            txtLabDirAd.Text = tablaOpeLaborales.Rows[0]["direccion_ad"].ToString();
            txtLabAreaTrab.Text = tablaOpeLaborales.Rows[0]["area_trabajo"].ToString();
            txtLabPuesto.Text = tablaOpeLaborales.Rows[0]["puesto"].ToString();
            Utilerias.seleccionaValorComboRequest(listaAntiguiAnios, tablaOpeLaborales.Rows[0]["antiguedad_anios"].ToString());
            Utilerias.seleccionaValorComboRequest(listaAntiguoMeses, tablaOpeLaborales.Rows[0]["antiguedad_meses"].ToString());
            Utilerias.seleccionaValorComboRequest(listaLabEstado, tablaOpeLaborales.Rows[0]["id_estado"].ToString());
            txtLAbMunicipio.Text = tablaOpeLaborales.Rows[0]["municipio"].ToString();
            txtLabDireccion.Text = tablaOpeLaborales.Rows[0]["direccion"].ToString();
            txtLabCP.Text = tablaOpeLaborales.Rows[0]["cp"].ToString();
            txtLabCiudad.Text = tablaOpeLaborales.Rows[0]["ciudad"].ToString();
            txtLabColonia.Text = tablaOpeLaborales.Rows[0]["colonia"].ToString();
            txtLabTelefono.Text = tablaOpeLaborales.Rows[0]["telefono"].ToString();
            Utilerias.seleccionaValorComboRequest(listaLabPensionado, tablaOpeLaborales.Rows[0]["pensionado"].ToString());
            Utilerias.seleccionaValorComboRequest(listaLabTipoPensionado, tablaOpeLaborales.Rows[0]["tipo_pensionado"].ToString());
            ocultoOpeLaboralesDatos.Value = "1";
        }
        else
        {//NO TIENE DATOS
            ocultoOpeLaboralesDatos.Value = "0";
        }
        modalWinCopeLab.Show();
    }
    private void listaLabEstado_Fill()
    {
        string[] datosListaE = { string.Empty };
        listaLabEstado = Utilerias.llenaLista(listaLabEstado, sqlExpediente.mListaEstados, datosListaE, "txt", "id", "");
    }

    private void listaAntiguiAnios_Fill()
    {
        for (int a = 1; a < 71; a++)
        {
            System.Web.UI.WebControls.ListItem t = new System.Web.UI.WebControls.ListItem(a.ToString(), a.ToString());
            listaAntiguiAnios.Items.Insert(a - 1, t);
        }
    }
    private void listaAntiguoMeses_Fill()
    {
        for (int a = 0; a < 12; a++)
        {
            System.Web.UI.WebControls.ListItem t = new System.Web.UI.WebControls.ListItem(a.ToString(), a.ToString());
            listaAntiguoMeses.Items.Insert(a, t);
        }
    }
    protected void btnWinCopeLab_Click(object sender, EventArgs e)
    {
        //validamos datos
        bool error;
        error = false;
        foreach (Control txt in this.pnlWinOpeLaborales.Controls)
        {
            if (txt is TextBox)
            {
                if (((TextBox)txt).Text.ToString().Trim().Equals(""))
                {
                    lblWinOpeLabGeneral.Text = "Debe capturar información";
                    lblWinOpeLabGeneral.CssClass = "labelError";
                    lblWinOpeLabGeneral.Visible = true;
                    ((TextBox)txt).BackColor = Color.Gray;
                    ((TextBox)txt).Focus();
                    error = true;
                    modalWinCopeLab.Show();
                    return;
                }
                if (error)
                {
                    modalWinCopeLab.Show();
                    break;
                }
                else error = false;
            }
        }
        string labDependencia = txtLabDependencia.Text.ToString();
        string labDireccionAd = txtLabDirAd.Text.ToString();
        string labAreaTrabajo = txtLabAreaTrab.Text.ToString();
        string labPuesto = txtLabPuesto.Text.ToString();
        string labAntiguedadAnios = listaAntiguiAnios.SelectedValue.ToString();
        string labAntiguedadMeses = listaAntiguoMeses.SelectedValue.ToString();
        string labPais = "0"; //este es el id del pais, en el formulario aparece pero no creo que se use
        string labEstado = listaLabEstado.SelectedValue.ToString();
        string labMunicipio = txtLAbMunicipio.Text.ToString();
        string labDireccion = txtLabDireccion.Text.ToString();
        string labCP = txtLabCP.Text.ToString();
        string labCiudad = txtLabCiudad.Text.ToString();
        string labColonia = txtLabColonia.Text.ToString();
        string labTelefono = txtLabTelefono.Text.ToString();
        string labPensionado = listaLabPensionado.SelectedValue.ToString();
        string labTipoPensionado = listaLabTipoPensionado.SelectedValue.ToString();
        string IdCliente = ocultoIdCliente.Value.ToString();
        string id_expediente = ocultoIdExpediente.Value.ToString();

        string[] datosOpeLaborales = { labDependencia, labDireccionAd, labAreaTrabajo, labPuesto, labAntiguedadAnios, labAntiguedadMeses, labPais, labEstado, labMunicipio, labDireccion, labCP, labCiudad, labColonia, labTelefono, IdCliente, labPensionado, labTipoPensionado, id_expediente };
        //verifico si inserto o modifico
        if (ocultoOpeLaboralesDatos.Value.ToString().Equals("0"))
        {//inserto ope_laborales
            //guardamos OPE_LABORALES
            if (Controladora.registraDatos(sqlExpediente.aOpeLaborales, datosOpeLaborales))
            { //si guardo correctamente
                modalWinCopeLab.Hide();
                ventanaOcultoTpoCierra.Value = "0";
                ventanaMensaje.Text = "Se capturaron correctamente los datos";
                //llenatabas();
                titulosExpediente();
                ventanaModal.Show();
            }
            else
            { //no guardo datos ope_laborales
                //capturo el tipo de error
                string errorSQL = Controladora.regresaSentenciaSQL(sqlExpediente.aOpeLaborales, datosOpeLaborales);
                string errorURL = HttpContext.Current.Request.Url.AbsolutePath.ToString();
                string errorIdent = "aOpeLaborales";
                Utilerias.registraSQL(errorSQL, errorURL, errorIdent);

                ventanaOcultoTpoCierra.Value = "0";
                ventanaMensaje.Text = "No se pudo capturar la información, favor de validar y volver a intentar";
                //llenatabas();
                titulosExpediente();
                modalWinCopeLab.Show();
                ventanaModal.Show();
            }
        }
        else
        {//actualizo ope_laborales
            if (Controladora.actualizaDatos(sqlExpediente.cOpeLaborales, datosOpeLaborales))
            {//se actualizaron correctamente los datos
                modalWinCopeLab.Hide();
                ventanaOcultoTpoCierra.Value = "0";
                ventanaMensaje.Text = "Se capturaron correctamente los datos";
                //llenatabas();
                titulosExpediente();
                ventanaModal.Show();
            }
            else
            {//no se actualizaron los datos de ope_laborales
                string errorSQL = Controladora.regresaSentenciaSQL(sqlExpediente.cOpeLaborales, datosOpeLaborales);
                string errorURL = HttpContext.Current.Request.Url.AbsolutePath.ToString();
                string errorIdent = "cOpeLaborales";
                Utilerias.registraSQL(errorSQL, errorURL, errorIdent);

                ventanaOcultoTpoCierra.Value = "0";
                ventanaMensaje.Text = "No se pudo capturar la información, favor de validar y volver a intentar";
                //llenatabas();
                titulosExpediente();
                modalWinCopeLab.Show();
                ventanaModal.Show();
            }
        }
    }
    #endregion /cOpeLaborales

    //ABRE VENTANA PARA GUARDAR DATOS DE PERSONA
    protected void btnCopePersona_Click(object sender, EventArgs e)
    {
        //obtenemos los datos
        string[] datosOpePersona = { ocultoIdExpediente.Value.ToString() };
        DataTable tablaOpePersona = Controladora.consultaDatos(sqlExpediente.mOpePersona, datosOpePersona);
        if (tablaOpePersona.Rows.Count > 0)
        {//si traemos datos
            txtPerNombre.Text = tablaOpePersona.Rows[0]["nombre"].ToString();
            txtApPaterno.Text = tablaOpePersona.Rows[0]["paterno"].ToString();
            txtApMaterno.Text = tablaOpePersona.Rows[0]["materno"].ToString();

            //descomponemos la fecha
            //            fch_nacimiento
            string[] fecha = tablaOpePersona.Rows[0]["fch_nacimiento"].ToString().Split('/');
            Utilerias.seleccionaValorComboRequest(listaAnioNac, fecha[2].ToString());
            Utilerias.seleccionaValorComboRequest(listaMesNac, fecha[1].ToString());
            Utilerias.seleccionaValorComboRequest(listaDiaNac, fecha[0].ToString());

            txtCurp.Text = tablaOpePersona.Rows[0]["curp"].ToString();
            txtRfc.Text = tablaOpePersona.Rows[0]["rfc"].ToString();

            Utilerias.seleccionaValorComboRequest(listaTpoPersona, tablaOpePersona.Rows[0]["id_tipo_persona"].ToString());

            txtTelefono.Text = tablaOpePersona.Rows[0]["telefono"].ToString();
            txtCelular.Text = tablaOpePersona.Rows[0]["celular"].ToString();
            txtEmail.Text = tablaOpePersona.Rows[0]["correo_electronico"].ToString();

            Utilerias.seleccionaValorComboRequest(listaPerBancos, tablaOpePersona.Rows[0]["id_banco"].ToString());

            txtPerClabe.Text = tablaOpePersona.Rows[0]["clabe"].ToString();
            //string[] datosOpePersona = { nombre, paterno, materno, fch_nacimiento, curp, rfc, id_tipo_persona, telefono, celular, correo_electronico, id_expediente, perIdBanco, perClabe };
            ocultoOpePersonaDatos.Value = "1";
        }
        else
        {//la tabla ope_persona viene vacía
            ocultoOpePersonaDatos.Value = "0";
        }
        ModalWinCopePersona.Show();
    }

    //modifica ope_persona
    protected void btnWinCopePersona_Click(object sender, EventArgs e)
    {
        //validamos datos
        bool error;
        error = false;
        foreach (Control txt in this.pnlWinOpePersona.Controls)
        {
            if (txt is TextBox)
            {
                if (((TextBox)txt).Text.ToString().Trim().Equals(""))
                {
                    lblWinOpeLabGeneral.Text = "Debe capturar información";
                    lblWinOpeLabGeneral.CssClass = "labelError";
                    lblWinOpeLabGeneral.Visible = true;
                    ((TextBox)txt).BackColor = Color.Gray;
                    ((TextBox)txt).Focus();
                    error = true;
                    ModalWinCopePersona.Show();
                    return;
                }
                if (error)
                {
                    ModalWinCopePersona.Show();
                    break;
                }
                else error = false;
            }
        }
        if (txtRfc.Text.ToString().Trim().Length < 10)
        {
            lblWinGeneralCopePersona.Text = "El R.F.C no es correcto";
            lblWinGeneralCopePersona.CssClass = "labelError";
            lblWinGeneralCopePersona.Visible = true;
            txtRfc.BackColor = Color.Wheat;
            txtRfc.Focus();
            ModalWinCopePersona.Show();
            return;
        }

        if (txtCurp.Text.ToString().Trim().Length != 18)
        {
            lblWinGeneralCopePersona.Text = "La C.U.R.P. es incorrecta";
            lblWinGeneralCopePersona.CssClass = "labelError";
            lblWinGeneralCopePersona.Visible = true;
            txtCurp.BackColor = Color.Wheat;
            txtCurp.Focus();
            ModalWinCopePersona.Show();
            return;
        }
        if (txtPerClabe.Text.ToString().Trim().Length != 18)
        {
            lblWinGeneralCopePersona.Text = "La CLABE interbancaria es incorrecta";
            lblWinGeneralCopePersona.CssClass = "labelError";
            lblWinGeneralCopePersona.Visible = true;
            txtPerClabe.BackColor = Color.Wheat;
            txtPerClabe.Focus();
            ModalWinCopePersona.Show();
            return;
        }


        string nombre = txtPerNombre.Text.ToString();
        string paterno = txtApPaterno.Text.ToString();
        string materno = txtApMaterno.Text.ToString();
        string fch_nacimiento = listaAnioNac.SelectedValue.ToString() + "-" + listaMesNac.SelectedValue.ToString() + "-" + listaDiaNac.SelectedValue.ToString();
        string curp = txtCurp.Text.ToString();
        string rfc = txtRfc.Text.ToString();
        string id_tipo_persona = listaTpoPersona.SelectedValue.ToString();
        string telefono = txtTelefono.Text.ToString();
        string celular = txtCelular.Text.ToString();
        string correo_electronico = txtEmail.Text.ToString();
        string id_expediente = ocultoIdExpediente.Value.ToString();
        string perIdBanco = listaPerBancos.SelectedValue.ToString();
        string perClabe = txtPerClabe.Text.ToString();

        string[] datosOpePersona = { nombre, paterno, materno, fch_nacimiento, curp, rfc, id_tipo_persona, telefono, celular, correo_electronico, perIdBanco, perClabe, id_expediente };
        if (ocultoOpePersonaDatos.Value.ToString().Equals("0"))
        {//guardo datos en ope_persona
            if (Controladora.registraDatos(sqlExpediente.aOpePersona, datosOpePersona))
            {//se actualizaron correctamente los datos
                ModalWinCopePersona.Hide();
                ventanaOcultoTpoCierra.Value = "0";
                ventanaMensaje.Text = "Se capturaron correctamente los datos";
                //llenatabas();
                titulosExpediente();
                ventanaModal.Show();
            }
            else
            {//no se actualizaron los datos de ope_laborales
                string errorSQL = Controladora.regresaSentenciaSQL(sqlExpediente.aOpePersona, datosOpePersona);
                string errorURL = HttpContext.Current.Request.Url.AbsolutePath.ToString();
                string errorIdent = "aOpePersona";
                Utilerias.registraSQL(errorSQL, errorURL, errorIdent);

                ventanaOcultoTpoCierra.Value = "0";
                ventanaMensaje.Text = "No se pudo capturar la información, favor de validar y volver a intentar";
                //llenatabas();
                titulosExpediente();
                ModalWinCopePersona.Show();
                ventanaModal.Show();
            }
        }
        else
        {//actualizo ope_persona
            if (Controladora.actualizaDatos(sqlExpediente.cOpePersona, datosOpePersona))
            {//actualiza correctamente
                ModalWinCopePersona.Hide();
                ventanaOcultoTpoCierra.Value = "0";
                ventanaMensaje.Text = "Se capturaron correctamente los datos";
                //llenatabas();
                titulosExpediente();
                ventanaModal.Show();
            }
            else
            {//no se pudo actualizar
                string errorSQL = Controladora.regresaSentenciaSQL(sqlExpediente.cOpePersona, datosOpePersona);
                string errorURL = HttpContext.Current.Request.Url.AbsolutePath.ToString();
                string errorIdent = "cOpePersona";
                Utilerias.registraSQL(errorSQL, errorURL, errorIdent);

                ventanaOcultoTpoCierra.Value = "0";
                ventanaMensaje.Text = "No se pudo capturar la información, favor de validar y volver a intentar";
                //llenatabas();
                titulosExpediente();
                ModalWinCopePersona.Show();
                ventanaModal.Show();
            }
        }
    }
    private void listaDiaNac_Fill()
    {
        for (int a = 1; a < 32; a++)
        {
            string da;
            da = string.Empty;
            if (a < 10)
            {
                da = "0" + a.ToString();
            }
            else
            {
                da = a.ToString();
            }
            System.Web.UI.WebControls.ListItem t = new System.Web.UI.WebControls.ListItem(da.ToString(), da.ToString());
            listaDiaNac.Items.Insert(a - 1, t);
        }
    }

    private void listaMesNac_Fill()
    {
        System.Web.UI.WebControls.ListItem lista = new System.Web.UI.WebControls.ListItem("Enero", "01");
        listaMesNac.Items.Insert(0, lista);
        System.Web.UI.WebControls.ListItem lista1 = new System.Web.UI.WebControls.ListItem("Febrero", "02");
        listaMesNac.Items.Insert(1, lista1);
        System.Web.UI.WebControls.ListItem lista2 = new System.Web.UI.WebControls.ListItem("Marzo", "03");
        listaMesNac.Items.Insert(2, lista2);
        System.Web.UI.WebControls.ListItem lista3 = new System.Web.UI.WebControls.ListItem("Abril", "04");
        listaMesNac.Items.Insert(3, lista3);
        System.Web.UI.WebControls.ListItem lista4 = new System.Web.UI.WebControls.ListItem("Mayo", "05");
        listaMesNac.Items.Insert(4, lista4);
        System.Web.UI.WebControls.ListItem lista5 = new System.Web.UI.WebControls.ListItem("Junio", "06");
        listaMesNac.Items.Insert(5, lista5);
        System.Web.UI.WebControls.ListItem lista6 = new System.Web.UI.WebControls.ListItem("Julio", "07");
        listaMesNac.Items.Insert(6, lista6);
        System.Web.UI.WebControls.ListItem lista7 = new System.Web.UI.WebControls.ListItem("Agosto", "08");
        listaMesNac.Items.Insert(7, lista7);
        System.Web.UI.WebControls.ListItem lista8 = new System.Web.UI.WebControls.ListItem("Septiembre", "09");
        listaMesNac.Items.Insert(8, lista8);
        System.Web.UI.WebControls.ListItem lista9 = new System.Web.UI.WebControls.ListItem("Octubre", "10");
        listaMesNac.Items.Insert(9, lista9);
        System.Web.UI.WebControls.ListItem lista10 = new System.Web.UI.WebControls.ListItem("Noviembre", "11");
        listaMesNac.Items.Insert(10, lista10);
        System.Web.UI.WebControls.ListItem lista11 = new System.Web.UI.WebControls.ListItem("Diciembre", "12");
        listaMesNac.Items.Insert(11, lista11);
    }

    private void listaAnioNac_Fill()
    {
        int indice = 0;
        for (int anio = 1930; anio < 2013; anio++)
        {
            System.Web.UI.WebControls.ListItem t = new System.Web.UI.WebControls.ListItem(anio.ToString(), anio.ToString());
            listaAnioNac.Items.Insert(indice, t);
            indice++;
        }
    }

    private void listaTpoPersona_Fill()
    {
        string[] datosTpoPersona = { string.Empty };
        listaTpoPersona = Utilerias.llenaLista(listaTpoPersona, sqlExpediente.mTpoPersona, datosTpoPersona, "txt", "id", "");
    }
    private void listaPerBancos_Fill()
    {
        string[] datosListaBancos = { string.Empty };
        listaPerBancos = Utilerias.llenaLista(listaPerBancos, sqlExpediente.mListaBancos, datosListaBancos, "txt", "id", "");
    }
    protected void btnWinCierraOpePersona_Click(object sender, ImageClickEventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        ModalWinCopePersona.Hide();
    }


    private void enviaCorreoConfirmacionDatos(string pTitEmail, string pCuerpo, string pPara)
    {
        //ConfigurationManager.AppSettings.Get("from")
        try
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtserver = new SmtpClient("smtp.gmail.com", 587);

            //mail.From = new MailAddress("richer2412@gmail.com", "richer", System.Text.Encoding.UTF8);
            mail.From = new MailAddress(ConfigurationManager.AppSettings.Get("correoDesde"), ConfigurationManager.AppSettings.Get("nombreDesde"), System.Text.Encoding.UTF8);

            mail.Subject = pTitEmail;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = pCuerpo;
            mail.IsBodyHtml = true;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.To.Add(pPara);

            smtserver.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings.Get("correoUser"), ConfigurationManager.AppSettings.Get("correoPass"));
            smtserver.EnableSsl = true;
            smtserver.Send(mail);
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.Message + "');</script>");
        }
    }
    protected void btnVerDocumentos_Click(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        string[] datosMdoctos = { ocultoIdExpediente.Value.ToString() };
        gridMDoctos.DataSource = Controladora.consultaDatos(sqlExpediente.mDoctosSistema, datosMdoctos);
        gridMDoctos.DataBind();
        pnlMgridDoctos.Visible = true;
    }
    //se activa cuando se selecciona ver el documento
    protected void gridMDoctos_SelectedIndexChanged(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        lblGeneralDoctos.Visible = false;
        //GridViewRow row = gridDoctosEtapa.SelectedRow;
        GridViewRow row = gridMDoctos.SelectedRow;
        string nombreDocumento = gridMDoctos.DataKeys[row.RowIndex].Values["nombre_documento"].ToString();
        string nombreImagen = gridMDoctos.DataKeys[row.RowIndex].Values["nombre_archivo"].ToString();
        //odct.nombre_archivo, odct.fecha_modifica_documento

        HtmlControl frame = (HtmlControl)iFrameMdocto; //.Attributes["src"] = "imgExpedientes/muestraDocto.asxp?archivo=" + nombreImagen;

        if (nombreImagen.Substring(nombreImagen.IndexOf('.') + 1).ToLower() != "doc" && nombreImagen.Substring(nombreImagen.IndexOf('.') + 1).ToLower() != "docx")
        {
            frame.Attributes["src"] = "imgExpedientes/muestraDocto.aspx?archivo=" + nombreImagen;
        }
        else
        {
            frame.Attributes["src"] = "imgExpedientes/muestraWord.aspx?archivo=" + nombreImagen;
        }
        //frame.Attributes["src"] = "imgExpedientes/muestraDocto.aspx?archivo=" + nombreImagen;


        lblMdoctoNombre.Text = nombreDocumento;
        doctoMmodal.Show();
    }
    //cierra la modal donde muestra el documento seleccionado.
    protected void btnXmDoctos_Click(object sender, ImageClickEventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        doctoMmodal.Hide();
    }
    //crea un mensaje para enviarlo a un usuario o grupo de usuarios
    protected void btnCreaMensaje_Click(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        //ponemos titulo a la ventana
        ucVentanaMnsj.ucTituloVentana("Mensajes");
        //llenamos los grid
        if (!ocultoIDsucursal.Value.ToString().Equals("0"))
        {
            mnsjGridUsuariosExp_Fill();
        }
        mnsjGridSupervisores_Fill();
        mnsjGridTodos_Fill();

        MnsjModal.Show();
        //hacemos el llamado del boton en el usercontrol para pasarle la accion
        ucVentanaMnsj.CierraVentana += new EventHandler(ucVentanaMnsj_CierraVentana);
    }
    //recibe la acción del user control
    protected void ucVentanaMnsj_CierraVentana(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        MnsjModal.Hide();
    }
    private void mnsjGridUsuariosExp_Fill()
    {
        string[] datosUserExp = { ocultoIDsucursal.Value.ToString() };
        mnsjGridUsuariosExp.DataSource = Controladora.consultaDatos(sqlExpediente.mUsrExpediente, datosUserExp);
        mnsjGridUsuariosExp.DataBind();
    }
    private void mnsjGridSupervisores_Fill()
    {
        string[] datosUserSup = { string.Empty };
        mnsjGridSupervisores.DataSource = Controladora.consultaDatos(sqlExpediente.mUsrSupervisor, datosUserSup);
        mnsjGridSupervisores.DataBind();
    }
    private void mnsjGridTodos_Fill()
    {
        string[] datosUserTodos = { string.Empty };
        mnsjGridTodos.DataSource = Controladora.consultaDatos(sqlExpediente.mUsrTodos, datosUserTodos);
        mnsjGridTodos.DataBind();
    }
    //envia el mensaje a las personas seleccionadas
    protected void mnsjBtnEnvia_Click(object sender, EventArgs e)
    {
        //llenatabas();
        titulosExpediente();

        //validamos que capturaron datos
        if (mnsjTxtTitulo.Text.ToString().Trim().Length == 0)
        {

            //ventanaOcultoTpoCierra.Value = "0";
            //ventanaMensaje.Text = "Debe capturar el título del mensaje";
            //ventanaModal.Show();
            MnsjModal.Show();
            mnsjLblGeneral.Text = "Debe capturar el título del mensaje";
            mnsjLblGeneral.CssClass = "labelError";
            mnsjLblGeneral.Visible = true;
            return;
        }
        if (mnsjTxtMensaje.Text.ToString().Trim().Length == 0)
        {

            //ventanaOcultoTpoCierra.Value = "0";
            //ventanaMensaje.Text = "Debe capturar el mensaje";

            MnsjModal.Show();
            //ventanaModal.Show();
            mnsjLblGeneral.Text = "Debe capturar el mensaje";
            mnsjLblGeneral.CssClass = "labelError";
            mnsjLblGeneral.Visible = true;
            return;
        }

        //validamos que al menos tenga un registro seleccionado
        bool validaSeleccionados;
        string usuario;

        validaSeleccionados = false;
        usuario = string.Empty;
        //recorremos el grid usuarios
        for (int iu = 0; iu < mnsjGridUsuariosExp.Rows.Count; iu++)
        {
            if (((CheckBox)mnsjGridUsuariosExp.Rows[iu].FindControl("chkUsrSel")).Checked == true)
            {
                validaSeleccionados = true;
            }
        }
        //recorremos el grid supervisores
        for (int isup = 0; isup < mnsjGridSupervisores.Rows.Count; isup++)
        {
            if (((CheckBox)mnsjGridSupervisores.Rows[isup].FindControl("chkUsrSel")).Checked == true)
            {
                validaSeleccionados = true;
            }
        }
        //recorremos el grid de todos
        for (int it = 0; it < mnsjGridTodos.Rows.Count; it++)
        {
            if (((CheckBox)mnsjGridTodos.Rows[it].FindControl("chkUsrSel")).Checked == true)
            {
                validaSeleccionados = true;
            }
        }

        if (validaSeleccionados)
        {

            string[] datosUsrExp = { mnsjListaPrioridad.SelectedValue.ToString(), mnsjTxtTitulo.Text.ToString(), mnsjTxtMensaje.Text.ToString(), Session["idUsuario"].ToString(), ocultoIdExpediente.Value.ToString() };
            if (Controladora.registraDatos(sqlExpediente.aMensaje, datosUsrExp))
            {
                //obtebemnos el utlimo id
                string[] datosUltimoIDMnsj = { Session["idUsuario"].ToString(), ocultoIdExpediente.Value.ToString() };
                DataTable tablaUltimoIDmnsj = Controladora.consultaDatos(sqlExpediente.idUltimoMnsj, datosUltimoIDMnsj);
                //recorremos el grid usuarios
                for (int iu = 0; iu < mnsjGridUsuariosExp.Rows.Count; iu++)
                {
                    if (((CheckBox)mnsjGridUsuariosExp.Rows[iu].FindControl("chkUsrSel")).Checked == true)
                    {
                        validaSeleccionados = true;
                        DataKey llave = mnsjGridUsuariosExp.DataKeys[iu];
                        usuario = llave["id"].ToString();
                        string[] datosMnsjDest = { tablaUltimoIDmnsj.Rows[0][0].ToString(), usuario };
                        bool altaDest = Controladora.registraDatos(sqlExpediente.aMensajeDest, datosMnsjDest);
                    }
                    usuario = string.Empty;
                }
                //recorremos el grid supervisores
                for (int isup = 0; isup < mnsjGridSupervisores.Rows.Count; isup++)
                {
                    if (((CheckBox)mnsjGridSupervisores.Rows[isup].FindControl("chkUsrSel")).Checked == true)
                    {
                        validaSeleccionados = true;
                        DataKey llave = mnsjGridSupervisores.DataKeys[isup];
                        usuario = llave["id"].ToString();
                        string[] datosMnsjDest = { tablaUltimoIDmnsj.Rows[0][0].ToString(), usuario };
                        bool altaDest = Controladora.registraDatos(sqlExpediente.aMensajeDest, datosMnsjDest);
                    }
                    usuario = string.Empty;
                }
                //recorremos el grid de todos
                for (int it = 0; it < mnsjGridTodos.Rows.Count; it++)
                {
                    if (((CheckBox)mnsjGridTodos.Rows[it].FindControl("chkUsrSel")).Checked == true)
                    {
                        validaSeleccionados = true;
                        DataKey llave = mnsjGridTodos.DataKeys[it];
                        usuario = llave["id"].ToString();
                        string[] datosMnsjDest = { tablaUltimoIDmnsj.Rows[0][0].ToString(), usuario };
                        bool altaDest = Controladora.registraDatos(sqlExpediente.aMensajeDest, datosMnsjDest);
                    }
                    usuario = string.Empty;
                }

                ventanaOcultoTpoCierra.Value = "0";
                ventanaMensaje.Text = "Se envío el mensaje a los destinatarios";
                ventanaModal.Show();
                MnsjModal.Hide();
            }
            else
            {
                //no inserto registros
                MnsjModal.Show();
                mnsjLblGeneral.Text = "Hubo un error al intentar enviar el mensaje, favor de intetarlo de nuevo";
                mnsjLblGeneral.CssClass = "labelError";
                mnsjLblGeneral.Visible = true;

                string errorSQL = Controladora.regresaSentenciaSQL(sqlExpediente.aMensaje, datosUsrExp);
                string errorURL = HttpContext.Current.Request.Url.AbsolutePath.ToString();
                string errorIdent = "Captura mensaje";
                Utilerias.registraSQL(errorSQL, errorURL, errorIdent);
            }
        }
        else
        {
            MnsjModal.Show();
            mnsjLblGeneral.Text = "Debe seleccionar al menos un destinatario";
            mnsjLblGeneral.CssClass = "labelError";
            mnsjLblGeneral.Visible = true;
        }
    }



    //captura de campos especiales pnlObjetos
    protected void btnCapturaCamposEspeciales_Click1(object sender, EventArgs e)
    {
        //ya se insertaron los objetos, leemos la tabla temproral
        string[] datosMtmpObjetos = { ocultoIdExpediente.Value.ToString(), ocultoIDetapa.Value.ToString() };
        DataTable tablaMtmpObjetos = Controladora.consultaDatos(sqlExpediente.mTmpObjetos, datosMtmpObjetos);

        //recorremos la tabla temporal de objetos para saber que controlos vamos a mostrar
        if (tablaMtmpObjetos.Rows.Count > 0)    //tntmpo
        {
            //Debemos saber que control es el que encedemos, para eso, creamos un contador, el contador se va a incrementar solo si entrar en el case que le corresponde
            int contNum, contTxt, contFecha, contChk, contCombo;
            bool error;
            //los declaramos en 1 porque tenemos los controles como 1
            contNum = 1;
            contTxt = 1;
            contFecha = 1;
            contChk = 1;
            contCombo = 1;

            error = true;

            //recorremos la tabla
            for (int incTtmp = 0; incTtmp < tablaMtmpObjetos.Rows.Count; incTtmp++)
            {   //for incTtmp
                switch (tablaMtmpObjetos.Rows[incTtmp]["id_tipo_objeto"].ToString())
                {
                    case "1":
                        //en un txt numérico
                        //validamos en que número de contador estamos para encender los controles
                        switch (contNum)
                        {
                            case 1:
                                //valido que traiga datos
                                if (objTxtNUM1.Text.ToString().Trim().Length > 0)
                                {
                                    //armo los datos de inserción, id_expediente, id_objeto, valor
                                    string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objTxtNUM1.Text.ToString() };
                                    DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                    string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                    DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                    if (tablaValidaObjeto.Rows.Count > 0)
                                    {
                                        //detenemos el proceso porque regreso un dato incorrecto
                                        objTxtNUM1.BackColor = Color.Wheat;
                                        objTxtNUM1.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    objTxtNUM1.BackColor = Color.Wheat;
                                    objTxtNUM1.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
                            case 2:
                                //valido que traiga datos
                                if (objTxtNUM2.Text.ToString().Trim().Length > 0)
                                {
                                    //armo los datos de inserción, id_expediente, id_objeto, valor
                                    string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objTxtNUM2.Text.ToString() };
                                    DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                    string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                    DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                    if (tablaValidaObjeto.Rows.Count > 0)
                                    {
                                        //detenemos el proceso porque regreso un dato incorrecto
                                        objTxtNUM2.BackColor = Color.Wheat;
                                        objTxtNUM2.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    objTxtNUM2.BackColor = Color.Wheat;
                                    objTxtNUM2.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
                        }
                        contNum++;
                        break;
                    case "2":
                        //es un txt
                        //validamos en que número de contador estamos para encender los controles
                        switch (contTxt)
                        {
                            case 1:
                                if (objTxtTexto1.Text.ToString().Trim().Length > 0)
                                {
                                    //armo los datos de inserción, id_expediente, id_objeto, valor
                                    string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objTxtTexto1.Text.ToString() };
                                    DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                    string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                    DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                    if (tablaValidaObjeto.Rows.Count > 0)
                                    {
                                        //detenemos el proceso porque regreso un dato incorrecto
                                        objTxtTexto1.BackColor = Color.Wheat;
                                        objTxtTexto1.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    objTxtTexto1.BackColor = Color.Wheat;
                                    objTxtTexto1.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.1";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
                            case 2:
                                if (objTxtTexto2.Text.ToString().Trim().Length > 0)
                                {
                                    //armo los datos de inserción, id_expediente, id_objeto, valor
                                    string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objTxtTexto2.Text.ToString() };
                                    DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                    string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                    DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                    if (tablaValidaObjeto.Rows.Count > 0)
                                    {
                                        //detenemos el proceso porque regreso un dato incorrecto
                                        objTxtTexto2.BackColor = Color.Wheat;
                                        objTxtTexto2.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    objTxtTexto2.BackColor = Color.Wheat;
                                    objTxtTexto2.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.2";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
                            case 3:
                                if (objTxtTexto3.Text.ToString().Trim().Length > 0)
                                {
                                    //armo los datos de inserción, id_expediente, id_objeto, valor
                                    string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objTxtTexto3.Text.ToString() };
                                    DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                    string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                    DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                    if (tablaValidaObjeto.Rows.Count > 0)
                                    {
                                        //detenemos el proceso porque regreso un dato incorrecto
                                        objTxtTexto3.BackColor = Color.Wheat;
                                        objTxtTexto3.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    objTxtTexto3.BackColor = Color.Wheat;
                                    objTxtTexto3.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.3";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
                            case 4:
                                if (objTxtTexto4.Text.ToString().Trim().Length > 0)
                                {
                                    //armo los datos de inserción, id_expediente, id_objeto, valor
                                    string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objTxtTexto4.Text.ToString() };
                                    DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                    string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                    DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                    if (tablaValidaObjeto.Rows.Count > 0)
                                    {
                                        //detenemos el proceso porque regreso un dato incorrecto
                                        objTxtTexto4.BackColor = Color.Wheat;
                                        objTxtTexto4.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    objTxtTexto4.BackColor = Color.Wheat;
                                    objTxtTexto4.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.4";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
                            case 5:
							//Response.Write("txt5");
                                if (objTxtTexto5.Text.ToString().Trim().Length > 0)
                                {
                                    //armo los datos de inserción, id_expediente, id_objeto, valor
                                    string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objTxtTexto5.Text.ToString() };
                                    DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                    string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                    DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                    if (tablaValidaObjeto.Rows.Count > 0)
                                    {
                                        //detenemos el proceso porque regreso un dato incorrecto
                                        objTxtTexto5.BackColor = Color.Wheat;
                                        objTxtTexto5.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                }
                                else
                                {
									//Response.Write("vacio5");
                                    objTxtTexto5.BackColor = Color.Wheat;
                                    objTxtTexto5.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.5";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;

                            case 6:
							//Response.Write("txt6");
                                if (objTxtTexto6.Text.ToString().Trim().Length > 0)
                                {
                                    //armo los datos de inserción, id_expediente, id_objeto, valor
                                    string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objTxtTexto6.Text.ToString() };
                                    DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                    string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                    DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                    if (tablaValidaObjeto.Rows.Count > 0)
                                    {
                                        //detenemos el proceso porque regreso un dato incorrecto
                                        objTxtTexto6.BackColor = Color.Wheat;
                                        objTxtTexto6.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                }
                                else
                                {
									//Response.Write("vacio6");
                                    objTxtTexto6.BackColor = Color.Wheat;
                                    objTxtTexto6.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.6";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;

                            case 7:
							//Response.Write("txt7");
                                if (objTxtTexto7.Text.ToString().Trim().Length > 0)
                                {
                                    //armo los datos de inserción, id_expediente, id_objeto, valor
                                    string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objTxtTexto7.Text.ToString() };
                                    DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                    string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                    DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                    if (tablaValidaObjeto.Rows.Count > 0)
                                    {
                                        //detenemos el proceso porque regreso un dato incorrecto
                                        objTxtTexto7.BackColor = Color.Wheat;
                                        objTxtTexto7.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                }
                                else
                                {
									//Response.Write("vacio7");
                                    objTxtTexto7.BackColor = Color.Wheat;
                                    objTxtTexto7.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.7";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;

                            case 8:
							//Response.Write("txt8");
                                if (objTxtTexto8.Text.ToString().Trim().Length > 0)
                                {
                                    //armo los datos de inserción, id_expediente, id_objeto, valor
                                    string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objTxtTexto8.Text.ToString() };
                                    DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                    string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                    DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                    if (tablaValidaObjeto.Rows.Count > 0)
                                    {
                                        //detenemos el proceso porque regreso un dato incorrecto
                                        objTxtTexto8.BackColor = Color.Wheat;
                                        objTxtTexto8.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                }
                                else
                                {
									//Response.Write("vacio8");
                                    objTxtTexto8.BackColor = Color.Wheat;
                                    objTxtTexto8.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.8";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;

                            case 9:
							//Response.Write("txt9");
                                if (objTxtTexto9.Text.ToString().Trim().Length > 0)
                                {
                                    //armo los datos de inserción, id_expediente, id_objeto, valor
                                    string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objTxtTexto9.Text.ToString() };
                                    DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                    string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                    DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                    if (tablaValidaObjeto.Rows.Count > 0)
                                    {
                                        //detenemos el proceso porque regreso un dato incorrecto
                                        objTxtTexto9.BackColor = Color.Wheat;
                                        objTxtTexto9.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                }
                                else
                                {
									//Response.Write("vacio9");
                                    objTxtTexto9.BackColor = Color.Wheat;
                                    objTxtTexto9.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.9";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;

                            case 10:
							//Response.Write("txt10");
                                if (objTxtTexto10.Text.ToString().Trim().Length > 0)
                                {
                                    //armo los datos de inserción, id_expediente, id_objeto, valor
                                    string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objTxtTexto10.Text.ToString() };
                                    DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                    string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                    DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                    if (tablaValidaObjeto.Rows.Count > 0)
                                    {
                                        //detenemos el proceso porque regreso un dato incorrecto
                                        objTxtTexto10.BackColor = Color.Wheat;
                                        objTxtTexto10.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                }
                                else
                                {
									//Response.Write("vacio10");
                                    objTxtTexto10.BackColor = Color.Wheat;
                                    objTxtTexto10.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.10";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
								
								
								
								
								
                        }
                        contTxt++;
                        break;
                    case "3":
                        //es una fecha
                        //validamos en que número de contador estamos para encender los controles
                        switch (contFecha)
                        {
                            case 1:
                                //valido que traiga datos
                                if (objTxtFecha1.Text.ToString().Trim().Length > 0)
                                {
                                    string fecha;
                                    fecha = string.Empty;
									fecha = objTxtFecha1.Text.ToString();
									/*
                                    switch (ConfigurationManager.AppSettings.Get("servidor").ToString())
                                    {
                                        case "e":
                                            //dd/mm/aaaa
                                            fecha = objTxtFecha1.Text.ToString();
                                            break;
                                        case "i":
                                            //mm/dd/aaaa
                                            fecha = Utilerias.ConvierteFechas(objTxtFecha1.Text.ToString(), 3);
                                            break;
                                    }
									*/
                                    //validamos que sea una fecha correcta
                                    if (!Utilerias.IsDate(fecha))
                                    {
                                        objTxtFecha1.BackColor = Color.Wheat;
                                        objTxtFecha1.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = "La fecha seleccionada no es correcta";
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                    else
                                    {
                                        //armo los datos de inserción, id_expediente, id_objeto, valor
                                        string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), Utilerias.ConvierteFechas(objTxtFecha1.Text.ToString(), 2) };
                                        DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                        string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                        DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                        if (tablaValidaObjeto.Rows.Count > 0)
                                        {
                                            //detenemos el proceso porque regreso un dato incorrecto
                                            objTxtFecha1.BackColor = Color.Wheat;
                                            objTxtFecha1.Focus();
                                            ventanaOcultoTpoCierra.Value = "0";
                                            ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                            ventanaModal.Show();
                                            error = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    objTxtFecha1.BackColor = Color.Wheat;
                                    objTxtFecha1.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
                            case 2:
                                //valido que traiga datos
                                if (objTxtFecha2.Text.ToString().Trim().Length > 0)
                                {
                                    string fecha;
                                    fecha = string.Empty;
									fecha = objTxtFecha2.Text.ToString();
									/*
                                    switch (ConfigurationManager.AppSettings.Get("servidor").ToString())
                                    {
                                        case "e":
                                            //dd/mm/aaaa
                                            fecha = objTxtFecha2.Text.ToString();
                                            break;
                                        case "i":
                                            //mm/dd/aaaa
                                            fecha = Utilerias.ConvierteFechas(objTxtFecha2.Text.ToString(), 3);
                                            break;
                                    }
									*/
                                    //validamos que sea una fecha correcta
                                    if (!Utilerias.IsDate(fecha))
                                    {
                                        objTxtFecha2.BackColor = Color.Wheat;
                                        objTxtFecha2.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = "La fecha seleccionada no es correcta";
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                    else
                                    {
                                        //armo los datos de inserción, id_expediente, id_objeto, valor
                                        string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), Utilerias.ConvierteFechas(objTxtFecha2.Text.ToString(), 2) };
                                        DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                        string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                        DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                        if (tablaValidaObjeto.Rows.Count > 0)
                                        {
                                            //detenemos el proceso porque regreso un dato incorrecto
                                            objTxtFecha2.BackColor = Color.Wheat;
                                            objTxtFecha2.Focus();
                                            ventanaOcultoTpoCierra.Value = "0";
                                            ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                            ventanaModal.Show();
                                            error = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    objTxtFecha2.BackColor = Color.Wheat;
                                    objTxtFecha2.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
                            case 3:
                                //valido que traiga datos
                                if (objTxtFecha3.Text.ToString().Trim().Length > 0)
                                {
                                    string fecha;
                                    fecha = string.Empty;
									fecha = objTxtFecha3.Text.ToString();
									/*
                                    switch (ConfigurationManager.AppSettings.Get("servidor").ToString())
                                    {
                                        case "e":
                                            //dd/mm/aaaa
                                            fecha = objTxtFecha2.Text.ToString();
                                            break;
                                        case "i":
                                            //mm/dd/aaaa
                                            fecha = Utilerias.ConvierteFechas(objTxtFecha2.Text.ToString(), 3);
                                            break;
                                    }
									*/
                                    //validamos que sea una fecha correcta
                                    if (!Utilerias.IsDate(fecha))
                                    {
                                        objTxtFecha3.BackColor = Color.Wheat;
                                        objTxtFecha3.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = "La fecha seleccionada no es correcta";
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                    else
                                    {
                                        //armo los datos de inserción, id_expediente, id_objeto, valor
                                        string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), Utilerias.ConvierteFechas(objTxtFecha3.Text.ToString(), 2) };
                                        DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                        string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                        DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                        if (tablaValidaObjeto.Rows.Count > 0)
                                        {
                                            //detenemos el proceso porque regreso un dato incorrecto
                                            objTxtFecha3.BackColor = Color.Wheat;
                                            objTxtFecha3.Focus();
                                            ventanaOcultoTpoCierra.Value = "0";
                                            ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                            ventanaModal.Show();
                                            error = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    objTxtFecha3.BackColor = Color.Wheat;
                                    objTxtFecha3.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
                            case 4:
                                //valido que traiga datos
                                if (objTxtFecha4.Text.ToString().Trim().Length > 0)
                                {
                                    string fecha;
                                    fecha = string.Empty;
									fecha = objTxtFecha4.Text.ToString();
									/*
                                    switch (ConfigurationManager.AppSettings.Get("servidor").ToString())
                                    {
                                        case "e":
                                            //dd/mm/aaaa
                                            fecha = objTxtFecha2.Text.ToString();
                                            break;
                                        case "i":
                                            //mm/dd/aaaa
                                            fecha = Utilerias.ConvierteFechas(objTxtFecha2.Text.ToString(), 3);
                                            break;
                                    }
									*/
                                    //validamos que sea una fecha correcta
                                    if (!Utilerias.IsDate(fecha))
                                    {
                                        objTxtFecha4.BackColor = Color.Wheat;
                                        objTxtFecha4.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = "La fecha seleccionada no es correcta";
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                    else
                                    {
                                        //armo los datos de inserción, id_expediente, id_objeto, valor
                                        string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), Utilerias.ConvierteFechas(objTxtFecha4.Text.ToString(), 2) };
                                        DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                        string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                        DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                        if (tablaValidaObjeto.Rows.Count > 0)
                                        {
                                            //detenemos el proceso porque regreso un dato incorrecto
                                            objTxtFecha4.BackColor = Color.Wheat;
                                            objTxtFecha4.Focus();
                                            ventanaOcultoTpoCierra.Value = "0";
                                            ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                            ventanaModal.Show();
                                            error = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    objTxtFecha4.BackColor = Color.Wheat;
                                    objTxtFecha4.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;

							case 5:
                                //valido que traiga datos
                                if (objTxtFecha5.Text.ToString().Trim().Length > 0)
                                {
                                    string fecha;
                                    fecha = string.Empty;
									fecha = objTxtFecha5.Text.ToString();
									/*
                                    switch (ConfigurationManager.AppSettings.Get("servidor").ToString())
                                    {
                                        case "e":
                                            //dd/mm/aaaa
                                            fecha = objTxtFecha2.Text.ToString();
                                            break;
                                        case "i":
                                            //mm/dd/aaaa
                                            fecha = Utilerias.ConvierteFechas(objTxtFecha2.Text.ToString(), 3);
                                            break;
                                    }
									*/
                                    //validamos que sea una fecha correcta
                                    if (!Utilerias.IsDate(fecha))
                                    {
                                        objTxtFecha5.BackColor = Color.Wheat;
                                        objTxtFecha5.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = "La fecha seleccionada no es correcta";
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                    else
                                    {
                                        //armo los datos de inserción, id_expediente, id_objeto, valor
                                        string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), Utilerias.ConvierteFechas(objTxtFecha5.Text.ToString(), 2) };
                                        DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                        string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                        DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                        if (tablaValidaObjeto.Rows.Count > 0)
                                        {
                                            //detenemos el proceso porque regreso un dato incorrecto
                                            objTxtFecha5.BackColor = Color.Wheat;
                                            objTxtFecha5.Focus();
                                            ventanaOcultoTpoCierra.Value = "0";
                                            ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                            ventanaModal.Show();
                                            error = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    objTxtFecha5.BackColor = Color.Wheat;
                                    objTxtFecha5.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;								

							case 6:
                                //valido que traiga datos
                                if (objTxtFecha6.Text.ToString().Trim().Length > 0)
                                {
                                    string fecha;
                                    fecha = string.Empty;
									fecha = objTxtFecha6.Text.ToString();
									/*
                                    switch (ConfigurationManager.AppSettings.Get("servidor").ToString())
                                    {
                                        case "e":
                                            //dd/mm/aaaa
                                            fecha = objTxtFecha2.Text.ToString();
                                            break;
                                        case "i":
                                            //mm/dd/aaaa
                                            fecha = Utilerias.ConvierteFechas(objTxtFecha2.Text.ToString(), 3);
                                            break;
                                    }
									*/
                                    //validamos que sea una fecha correcta
                                    if (!Utilerias.IsDate(fecha))
                                    {
                                        objTxtFecha6.BackColor = Color.Wheat;
                                        objTxtFecha6.Focus();
                                        ventanaOcultoTpoCierra.Value = "0";
                                        ventanaMensaje.Text = "La fecha seleccionada no es correcta";
                                        ventanaModal.Show();
                                        error = false;
                                        break;
                                    }
                                    else
                                    {
                                        //armo los datos de inserción, id_expediente, id_objeto, valor
                                        string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), Utilerias.ConvierteFechas(objTxtFecha6.Text.ToString(), 2) };
                                        DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                        string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                        DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                        if (tablaValidaObjeto.Rows.Count > 0)
                                        {
                                            //detenemos el proceso porque regreso un dato incorrecto
                                            objTxtFecha6.BackColor = Color.Wheat;
                                            objTxtFecha6.Focus();
                                            ventanaOcultoTpoCierra.Value = "0";
                                            ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                            ventanaModal.Show();
                                            error = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    objTxtFecha6.BackColor = Color.Wheat;
                                    objTxtFecha6.Focus();
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = "Debe capturar información.";
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
								
								
                        }
                        contFecha++;
                        break;
						
                    case "4":
                        //es un check
                        //validamos en que número de contador estamos para encender los controles
                        switch (contChk)
                        {
                            case 1:
                                string datoChk;
                                datoChk = string.Empty;
                                if (objChkCheck1.Checked) datoChk = "true"; else datoChk = "false";

                                //armo los datos de inserción, id_expediente, id_objeto, valor
                                string[] datosSPguardarObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), datoChk };
                                DataTable spGuardarObjeto = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto);
                                string[] datosValidaObjeto = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                DataTable tablaValidaObjeto = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto);
                                if (tablaValidaObjeto.Rows.Count > 0)
                                {
                                    //detenemos el proceso porque regreso un dato incorrecto
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = tablaValidaObjeto.Rows[0][0].ToString();
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
                            case 2:
                                string datoChk2;
                                datoChk2 = string.Empty;
                                if (objChkCheck2.Checked) datoChk2 = "true"; else datoChk2 = "false";

                                //armo los datos de inserción, id_expediente, id_objeto, valor
                                string[] datosSPguardarObjeto2 = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), datoChk2 };
                                DataTable spGuardarObjeto2 = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto2);
                                string[] datosValidaObjeto2 = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                DataTable tablaValidaObjeto2 = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto2);
                                if (tablaValidaObjeto2.Rows.Count > 0)
                                {
                                    //detenemos el proceso porque regreso un dato incorrecto
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = tablaValidaObjeto2.Rows[0][0].ToString();
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
                        }
                        contChk++;
                        break;
                    case "5":
                        //es una lista
                        //validamos en que número de contador estamos para encender los controles
                        switch (contCombo)
                        {
                            case 1:
                                //armo los datos de inserción, id_expediente, id_objeto, valor
                                string[] datosSPguardarObjeto1 = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objListaCombo1.SelectedValue.ToString() };
                                DataTable spGuardarObjeto1 = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto1);
                                string[] datosValidaObjeto1 = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                DataTable tablaValidaObjeto1 = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto1);
                                if (tablaValidaObjeto1.Rows.Count > 0)
                                {
                                    //detenemos el proceso porque regreso un dato incorrecto
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = tablaValidaObjeto1.Rows[0][0].ToString();
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
                            case 2:
                                string[] datosSPguardarObjeto2 = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objListaCombo2.SelectedValue.ToString() };
                                DataTable spGuardarObjeto2 = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto2);
                                string[] datosValidaObjeto2 = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                DataTable tablaValidaObjeto2 = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto2);
                                if (tablaValidaObjeto2.Rows.Count > 0)
                                {
                                    //detenemos el proceso porque regreso un dato incorrecto
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = tablaValidaObjeto2.Rows[0][0].ToString();
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
                            case 3:
                                string[] datosSPguardarObjeto3 = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objListaCombo3.SelectedValue.ToString() };
                                DataTable spGuardarObjeto3 = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto3);
                                string[] datosValidaObjeto3 = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                DataTable tablaValidaObjeto3 = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto3);
                                if (tablaValidaObjeto3.Rows.Count > 0)
                                {
                                    //detenemos el proceso porque regreso un dato incorrecto
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = tablaValidaObjeto3.Rows[0][0].ToString();
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
                            case 4:
                                string[] datosSPguardarObjeto4 = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objListaCombo4.SelectedValue.ToString() };
                                DataTable spGuardarObjeto4 = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto4);
                                string[] datosValidaObjeto4 = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                DataTable tablaValidaObjeto4 = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto4);
                                if (tablaValidaObjeto4.Rows.Count > 0)
                                {
                                    //detenemos el proceso porque regreso un dato incorrecto
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = tablaValidaObjeto4.Rows[0][0].ToString();
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
                            case 5:
                                string[] datosSPguardarObjeto5 = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objListaCombo5.SelectedValue.ToString() };
                                DataTable spGuardarObjeto5 = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto5);
                                string[] datosValidaObjeto5 = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                DataTable tablaValidaObjeto5 = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto5);
                                if (tablaValidaObjeto5.Rows.Count > 0)
                                {
                                    //detenemos el proceso porque regreso un dato incorrecto
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = tablaValidaObjeto5.Rows[0][0].ToString();
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;
                            case 6:
                                string[] datosSPguardarObjeto6 = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString(), objListaCombo6.SelectedValue.ToString() };
                                DataTable spGuardarObjeto6 = Controladora.consultaDatos(sqlExpediente.spGuardarObjeto, datosSPguardarObjeto6);
                                string[] datosValidaObjeto6 = { ocultoIdExpediente.Value.ToString(), tablaMtmpObjetos.Rows[incTtmp]["id_objeto"].ToString() };
                                DataTable tablaValidaObjeto6 = Controladora.consultaDatos(sqlExpediente.mValidaObjeto, datosValidaObjeto6);
                                if (tablaValidaObjeto6.Rows.Count > 0)
                                {
                                    //detenemos el proceso porque regreso un dato incorrecto
                                    ventanaOcultoTpoCierra.Value = "0";
                                    ventanaMensaje.Text = tablaValidaObjeto6.Rows[0][0].ToString();
                                    ventanaModal.Show();
                                    error = false;
                                    break;
                                }
                                break;								
                        }
                        contCombo++;
                        break;
                }   //switch
            }   //for incTtmp
            if (error)
            {
                ventanaOcultoTpoCierra.Value = "0";
                ventanaMensaje.Text = "Se registraron correctamente los datos";
                ventanaModal.Show();
            }
        }   //tntmpo
		
		            #region pnlObjetosRefresh
            //GENERA LAS OPCIONES PARA LOS OBJETOS

            //llemos la tabla ope_objetos para traernos el id_objeto, pasándole por parámetro el id_expdiente y la etapa
            string[] datosMobjetos = { ocultoIdExpediente.Value.ToString(), ocultoIDetapa.Value.ToString() };
            DataTable tablaMobjetos = Controladora.consultaDatos(sqlExpediente.mObjetos, datosMobjetos);
            if (tablaMobjetos.Rows.Count > 0)   //tablaMobjetos
            {// si tiene datos, mandamos a llamar la función que nos llena la tabla temporal de donde traemos los la información de los objetos
                for (int incTobj = 0; incTobj < tablaMobjetos.Rows.Count; incTobj++)
                {
                    string[] datosSPleerObjetos = { ocultoIdExpediente.Value.ToString(), tablaMobjetos.Rows[incTobj]["id_objeto"].ToString(), Session["idUsuario"].ToString() };
                    DataTable tablaSPleerObjetos = Controladora.consultaDatos(sqlExpediente.spLeerObjetos, datosSPleerObjetos);
                }
                //ya se insertaron los objetos, leemos la tabla temproral
                string[] datosMtmpObjetosRef = { ocultoIdExpediente.Value.ToString(), ocultoIDetapa.Value.ToString() };
                DataTable tablaMtmpObjetosRef = Controladora.consultaDatos(sqlExpediente.mTmpObjetos, datosMtmpObjetosRef);

                //recorremos la tabla temporal de objetos para saber que controlos vamos a mostrar
                if (tablaMtmpObjetosRef.Rows.Count > 0)    //tntmpo
                {
                    //Debemos saber que control es el que encedemos, para eso, creamos un contador, el contador se va a incrementar solo si entrar en el case que le corresponde
                    int contNum, contTxt, contFecha, contChk, contCombo;
                    //los declaramos en 1 porque tenemos los controles como 1
                    contNum = 1;
                    contTxt = 1;
                    contFecha = 1;
                    contChk = 1;
                    contCombo = 1;

                    //recorremos la tabla
                    for (int incTtmp = 0; incTtmp < tablaMtmpObjetosRef.Rows.Count; incTtmp++)
                    {   //for incTtmp
                        switch (tablaMtmpObjetosRef.Rows[incTtmp]["id_tipo_objeto"].ToString())
                        {
                            case "1":
                                //en un txt numérico
                                //validamos en que número de contador estamos para encender los controles
                                switch (contNum)
                                {
                                    case 1:
                                        objLblNUM1.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblNUM1.Visible = true;
                                        objTxtNUM1.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtNUM1.Visible = true;
                                        break;
                                    case 2:
                                        objLblNUM2.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblNUM2.Visible = true;
                                        objTxtNUM2.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtNUM2.Visible = true;
                                        break;
                                    case 3:
                                        objLblNUM3.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblNUM3.Visible = true;
                                        objTxtNUM3.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtNUM3.Visible = true;
                                        break;
                                }
                                contNum++;
                                break;
                            case "2":
                                //es un txt general
                                //validamos en que número de contador estamos para encender los controles
                                switch (contTxt)
                                {
                                    case 1:
                                        objLblTexto1.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto1.Visible = true;
                                        objTxtTexto1.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto1.Visible = true;
                                        break;
                                    case 2:
                                        objLblTexto2.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto2.Visible = true;
                                        objTxtTexto2.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto2.Visible = true;
                                        break;
                                    case 3:
                                        objLblTexto3.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto3.Visible = true;
                                        objTxtTexto3.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto3.Visible = true;
                                        break;
                                    case 4:
                                        objLblTexto4.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto4.Visible = true;
                                        objTxtTexto4.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto4.Visible = true;
                                        break;
                                    case 5:
                                        objLblTexto5.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto5.Visible = true;
                                        objTxtTexto5.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto5.Visible = true;
                                        break;
                                    case 6:
                                        objLblTexto6.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto6.Visible = true;
                                        objTxtTexto6.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto6.Visible = true;
                                        break;
                                    case 7:
                                        objLblTexto7.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto7.Visible = true;
                                        objTxtTexto7.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto7.Visible = true;
                                        break;
                                    case 8:
                                        objLblTexto8.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto8.Visible = true;
                                        objTxtTexto8.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto8.Visible = true;
                                        break;
                                    case 9:
                                        objLblTexto9.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto9.Visible = true;
                                        objTxtTexto9.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto9.Visible = true;
                                        break;
                                    case 10:
                                        objLblTexto10.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblTexto10.Visible = true;
                                        objTxtTexto10.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtTexto10.Visible = true;
                                        break;
                                    								
                                }
                                contTxt++;
                                break;
                            case "3":
                                //es una fecha
                                //validamos en que número de contador estamos para encender los controles
                                switch (contFecha)
                                {
                                    case 1:
                                        objLblFecha1.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblFecha1.Visible = true;
                                        objTxtFecha1.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtFecha1.Visible = true;
                                        break;
                                    case 2:
                                        objLblFecha2.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblFecha2.Visible = true;
                                        objTxtFecha2.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtFecha2.Visible = true;
                                        break;
                                    case 3:
                                        objLblFecha3.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblFecha3.Visible = true;
                                        objTxtFecha3.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtFecha3.Visible = true;
                                        break;
                                    case 4:
                                        objLblFecha4.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblFecha4.Visible = true;
                                        objTxtFecha4.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtFecha4.Visible = true;
                                        break;
                                    case 5:
                                        objLblFecha5.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblFecha5.Visible = true;
                                        objTxtFecha5.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtFecha5.Visible = true;
                                        break;
                                    case 6:
                                        objLblFecha6.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblFecha6.Visible = true;
                                        objTxtFecha6.Text = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
                                        objTxtFecha6.Visible = true;
                                        break;
										
                                }
                                contFecha++;
                                break;
                            case "4":
                                //es un check
                                //validamos en que número de contador estamos para encender los controles
                                switch (contChk)
                                {
                                    case 1:
                                        objChkCheck1.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        if (tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString().Equals("true"))
                                        {
                                            objChkCheck1.Checked = true;
                                        }
                                        else
                                        {
                                            objChkCheck1.Checked = false;
                                        }
                                        objChkCheck1.Visible = true;
                                        break;
                                    case 2:
                                        objChkCheck2.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        if (tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString().Equals("true"))
                                        {
                                            objChkCheck2.Checked = true;
                                        }
                                        else
                                        {
                                            objChkCheck2.Checked = false;
                                        }
                                        objChkCheck2.Visible = true;
                                        break;
                                    case 3:
                                        objChkCheck3.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        if (tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString().Equals("true"))
                                        {
                                            objChkCheck3.Checked = true;
                                        }
                                        else
                                        {
                                            objChkCheck3.Checked = false;
                                        }
                                        objChkCheck3.Visible = true;
                                        break;
                                }
                                contChk++;
                                break;
                            case "5":
                                //es una lista
                                //validamos en que número de contador estamos para encender los controles
								ocultoValorCondicion.Value = "";
                                switch (contCombo)
                                {
                                    case 1:
                                        objLblCombo1.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblCombo1.Visible = true;
                                        //Obtenemos el valor del combo
                                        string[] sqlTablaOculta = { tablaMtmpObjetosRef.Rows[incTtmp]["query_contenido"].ToString() };
                                        Utilerias.llenaLista(objListaCombo1, sqlCamposTablas.sqlParametroTablaOculta, sqlTablaOculta, "txt", "id", "");
                                        //seleccionamos si trae un valor
										//Response.Write("<<<<");
										//Response.Write(tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString());
										//Response.Write(">>>>");
										
                                        if (Convert.ToInt64(tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString()) > 0)
                                        {
                                            Utilerias.seleccionaValorComboRequest(objListaCombo1, tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString());
                                        }
										
                                        objListaCombo1.Visible = true;
										//este objeto es una condicion para el flujo de la operacion?
										if (Convert.ToInt64(tablaMtmpObjetosRef.Rows[incTtmp]["condicion"].ToString()) > 0)
										{
											ocultoValorCondicion.Value = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
										}										
                                        break;
                                    case 2:
                                        objLblCombo2.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblCombo2.Visible = true;
                                        //Obtenemos el valor del combo
                                        string[] sqlTablaOculta2 = { tablaMtmpObjetosRef.Rows[incTtmp]["query_contenido"].ToString() };
                                        Utilerias.llenaLista(objListaCombo2, sqlCamposTablas.sqlParametroTablaOculta, sqlTablaOculta2, "txt", "id", "");
                                        //seleccionamos si trae un valor
                                        if (Convert.ToInt64(tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString()) > 0)
                                        {
                                            Utilerias.seleccionaValorComboRequest(objListaCombo2, tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString());
                                        }
                                        objListaCombo2.Visible = true;
										//este objeto es una condicion para el flujo de la operacion?
										if (Convert.ToInt64(tablaMtmpObjetosRef.Rows[incTtmp]["condicion"].ToString()) > 0)
										{
											ocultoValorCondicion.Value = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
										}										
                                        break;
                                    case 3:
                                        objLblCombo3.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblCombo3.Visible = true;
                                        //Obtenemos el valor del combo
                                        string[] sqlTablaOculta3 = { tablaMtmpObjetosRef.Rows[incTtmp]["query_contenido"].ToString() };
                                        Utilerias.llenaLista(objListaCombo3, sqlCamposTablas.sqlParametroTablaOculta, sqlTablaOculta3, "txt", "id", "");
                                        //seleccionamos si trae un valor
                                        if (Convert.ToInt64(tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString()) > 0)
                                        {
                                            Utilerias.seleccionaValorComboRequest(objListaCombo3, tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString());
                                        }
                                        objListaCombo3.Visible = true;
										//este objeto es una condicion para el flujo de la operacion?
										if (Convert.ToInt64(tablaMtmpObjetosRef.Rows[incTtmp]["condicion"].ToString()) > 0)
										{
											ocultoValorCondicion.Value = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
										}
                                        break;
                                    case 4:
                                        objLblCombo4.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblCombo4.Visible = true;
                                        //Obtenemos el valor del combo
                                        string[] sqlTablaOculta4 = { tablaMtmpObjetosRef.Rows[incTtmp]["query_contenido"].ToString() };
                                        Utilerias.llenaLista(objListaCombo4, sqlCamposTablas.sqlParametroTablaOculta, sqlTablaOculta4, "txt", "id", "");
                                        //seleccionamos si trae un valor
                                        if (Convert.ToInt64(tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString()) > 0)
                                        {
                                            Utilerias.seleccionaValorComboRequest(objListaCombo4, tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString());
                                        }
                                        objListaCombo4.Visible = true;
										//este objeto es una condicion para el flujo de la operacion?
										if (Convert.ToInt64(tablaMtmpObjetosRef.Rows[incTtmp]["condicion"].ToString()) > 0)
										{
											ocultoValorCondicion.Value = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
										}
                                        break;
									case 5:
                                        objLblCombo5.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblCombo5.Visible = true;
                                        //Obtenemos el valor del combo
                                        string[] sqlTablaOculta5 = { tablaMtmpObjetosRef.Rows[incTtmp]["query_contenido"].ToString() };
                                        Utilerias.llenaLista(objListaCombo5, sqlCamposTablas.sqlParametroTablaOculta, sqlTablaOculta5, "txt", "id", "");
                                        //seleccionamos si trae un valor
                                        if (Convert.ToInt64(tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString()) > 0)
                                        {
                                            Utilerias.seleccionaValorComboRequest(objListaCombo5, tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString());
                                        }
                                        objListaCombo5.Visible = true;
										//este objeto es una condicion para el flujo de la operacion?
										if (Convert.ToInt64(tablaMtmpObjetosRef.Rows[incTtmp]["condicion"].ToString()) > 0)
										{
											ocultoValorCondicion.Value = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
										}
                                        break;
                                    case 6:
                                        objLblCombo6.Text = tablaMtmpObjetosRef.Rows[incTtmp]["etiqueta"].ToString();
                                        objLblCombo6.Visible = true;
                                        //Obtenemos el valor del combo
                                        string[] sqlTablaOculta6 = { tablaMtmpObjetosRef.Rows[incTtmp]["query_contenido"].ToString() };
                                        Utilerias.llenaLista(objListaCombo6, sqlCamposTablas.sqlParametroTablaOculta, sqlTablaOculta6, "txt", "id", "");
                                        //seleccionamos si trae un valor
                                        if (Convert.ToInt64(tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString()) > 0)
                                        {
                                            Utilerias.seleccionaValorComboRequest(objListaCombo6, tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString());
                                        }
                                        objListaCombo6.Visible = true;
										//este objeto es una condicion para el flujo de la operacion?
										if (Convert.ToInt64(tablaMtmpObjetosRef.Rows[incTtmp]["condicion"].ToString()) > 0)
										{
											ocultoValorCondicion.Value = tablaMtmpObjetosRef.Rows[incTtmp]["valor_objeto"].ToString();
										}
                                        break;										
                                }
                                contCombo++;
                                break;
                        }   //switch
                    }   //for incTtmp
                }   //tntmpo
                pnlObjetos.Visible = true;
            }   //tablaMobjetos cuando si tiene datos


            #endregion

    }
    //se genera cuando dan clic en leer mensajes
    protected void btnAltaUsuario_Click(object sender, ImageClickEventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        pnlLecturaMensajes.Visible = true;
        //llenamos el grid con la consulta
        string[] datosMnsjXusr = { ocultoIdExpediente.Value.ToString() };
        gridLecturaMensajes.DataSource = Controladora.consultaDatos(sqlExpediente.mMensajesXexpediente, datosMnsjXusr);
        gridLecturaMensajes.DataBind();
    }
    //cierra el panel de lectura de mensajes
    protected void imgCierraPnlLecturaMens_Click(object sender, ImageClickEventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        pnlLecturaMensajes.Visible = false;
    }
    //crea un nuevo mensaje en el panel de lectura de mensajes
    protected void imgCreaMensaje_Click(object sender, ImageClickEventArgs e)
    {
        //llenatabas();
        titulosExpediente();
        //ponemos titulo a la ventana
        ucVentanaMnsj.ucTituloVentana("Mensajes");
        //llenamos los grid
        if (!ocultoIDsucursal.Value.ToString().Equals("0"))
        {
            mnsjGridUsuariosExp_Fill();
        }
        mnsjGridSupervisores_Fill();
        mnsjGridTodos_Fill();

        MnsjModal.Show();
        //hacemos el llamado del boton en el usercontrol para pasarle la accion
        ucVentanaMnsj.CierraVentana += new EventHandler(ucVentanaMnsj_CierraVentana);
    }
    //pone mensaje de leido no leido en el grid de luectura de mensajes
    protected void gridLecturaMensajes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string leido = e.Row.Cells[5].Text;
            if (leido.Length < 10)
            {
                e.Row.Font.Bold = true;
                e.Row.Cells[5].Text = "No leido";
                e.Row.Cells[5].Font.Bold = true;
            }
            else
                e.Row.Font.Bold = false;

            if (e.Row.RowState == DataControlRowState.Alternate)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.cursor='hand';this.style.backgroundColor='#ffffcc'; ");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white'; this.style.color='black'");
            }
            else
            {
                e.Row.Attributes.Add("onmouseover", "this.style.cursor='hand';this.style.backgroundColor='#ffffcc'; ");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#eff3fb'; this.style.color='black'");
            }
        }
    }
    //cierra panel TABS
    protected void btnTABSCierra_Click(object sender, ImageClickEventArgs e)
    {
        //llenatabas();
        titulosExpediente();
    }
    #region v5uv_controles
    //iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii    
    //abre la ventana para capturar la unidad de valuación o notario
    protected void btnv5uv_alta_Click(object sender, ImageClickEventArgs e)
    {
        //muestro las asignaciones
        string[] datosMopeTercerosAsignados = { ocultoIdExpediente.Value.ToString() };
        gridv5uvAsignados.DataSource = Controladora.consultaDatos(sqlExpediente.mOpeTercerosAsignados, datosMopeTercerosAsignados);
        gridv5uvAsignados.DataBind();
        modalv5uv.Show();
    }
    //cierra la venta v5uv Asigna unidad de valuación - notario
    protected void btnv5uvCierraVentana_Click(object sender, ImageClickEventArgs e)
    {
        titulosExpediente();
        //llenatabas();
        modalv5uv.Hide();
    }
    //se activa cuando selecciona la unidad de valuación
    protected void radiov5uvUnidad_CheckedChanged(object sender, EventArgs e)
    {
        modalv5uv.Show();
        listav5uvUnidadValuacion.Visible = true;
        listav5uvNotario.Visible = false;
        listav5uvNotario.SelectedIndex = 0;
    }
    //se activa cuando seleccionan el notario
    protected void radiov5uvNotario_CheckedChanged(object sender, EventArgs e)
    {
        modalv5uv.Show();
        listav5uvUnidadValuacion.Visible = false;
        listav5uvNotario.Visible = true;
        listav5uvNotario.SelectedIndex = 0;
    }
    //asigna la unidad de valuación en el sistema
    protected void btnv5uvAsigna_Click(object sender, EventArgs e)
    {
        //OBTENEMOS EL TIPO DE ENTIDAD RESPONSABLE DE ope_cat_entidad_responsable, DONDE 2: NOTARIO, 3:UNIDAD DE VALUACIÓN
        string tipo, idEntidadResponsable, mensaje;
        tipo = string.Empty;
        idEntidadResponsable = string.Empty;
        mensaje = string.Empty;
        if (radiov5uvNotario.Checked)
        {
            tipo = "2";
            idEntidadResponsable = listav5uvNotario.SelectedValue.ToString();
            mensaje = "l notario";
        }
        if (radiov5uvUnidad.Checked)
        {
            tipo = "3";
            idEntidadResponsable = listav5uvUnidadValuacion.SelectedValue.ToString();
            mensaje = " la unidad de valuación";
        }
        string[] datosv5uvA = { ocultoIdExpediente.Value.ToString(), idEntidadResponsable, tipo, Session["idUsuario"].ToString() };
        if (Controladora.registraDatos(sqlExpediente.aOpeTerceros, datosv5uvA))
        {
            //CREO LOS DOCUMENTOS QUE PUEDE VISUALIZAR EL USUARIO A TRAVES DE LA FUNCIÓN
            string[] datosFnCreaDoctosTerceros = { ocultoIdExpediente.Value.ToString(), tipo };
            DataTable tablaFnCreaDoctosTerceros = Controladora.consultaDatos(sqlExternos.fnCreaDocsTerceros, datosFnCreaDoctosTerceros);

            //busco el mail del usuario para enviarle el mensaje
            string[] datosBuscaEmail = { idEntidadResponsable, tipo };
            DataTable tablaEmail = Controladora.consultaDatos(sqlExternos.mMailUsuarioExterno, datosBuscaEmail);
            //se envía el mail
            // enviaCorreoConfirmacionDatos("Asignación de expediente", "Se le ha asignado el número de expediente: " + Session["numExpediente"].ToString(), tablaEmail.Rows[0][0].ToString());



            // armo el correo 
            string titulo;
            string cuerpo;
            string remitente;
            string destinatario;
            string origenCorreo;


            titulo = "Asignación de expediente";
            remitente = "avisosCoOrigina@gmail.com";
            destinatario = tablaEmail.Rows[0][0].ToString();
            origenCorreo = "Sistema Co Origina";

            cuerpo = "Se le ha asignado el número de expediente: " + Session["numExpediente"].ToString();

            string[] datos = { titulo, cuerpo, remitente, destinatario, origenCorreo };
            Controladora.registraDatos(sqlOperacion.aEnviaCorreos, datos);


            //hacemos el cambio de flujo
            if (tipo.Equals("3"))
            {
                string[] datosv5ftvFnCambiaFlujoVariables = { ocultoIdExpediente.Value.ToString(), "1", "1", Session["idUsuario"].ToString() };
                DataTable tablaFnCamviaFlujoVariables = Controladora.consultaDatos(sqlExpediente.fnCambiaFlujoVariables, datosv5ftvFnCambiaFlujoVariables);
                gridResultadoBuscar_SelectedIndexChanged(sender, e);
            }
            //se muestra mensaje
            ventanaOcultoTpoCierra.Value = "0";
            ventanaModal.Show();
            ventanaMensaje.Text = "Se asigno el expediente a" + mensaje + ", se ha programado el envío del correo electrónico";
        }
        else
        {
            string errorSQL = Controladora.regresaSentenciaSQL(sqlExpediente.aOpeTerceros, datosv5uvA);
            string errorURL = HttpContext.Current.Request.Url.AbsolutePath.ToString();
            string errorIdent = "v5avAopeTerceros";
            Utilerias.registraSQL(errorSQL, errorURL, errorIdent);
            ventanaOcultoTpoCierra.Value = "0";
            ventanaModal.Show();
            ventanaMensaje.Text = "Hubo un error al registrar el dato, favor de intentarlo mas tarde";
        }
    }
    //fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff
    #endregion




    //muestra documentos de proyectos
    protected void gridv5pvpDocumentos_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gridv5pvpDocumentos.SelectedRow;
        string nombreDocumento = gridv5pvpDocumentos.DataKeys[row.RowIndex].Values["nombre_documento"].ToString();//id del documento de la tabla ope_documentos
        string archivo = gridv5pvpDocumentos.DataKeys[row.RowIndex].Values["nombre_archivo"].ToString();//id del documento de la tabla ope_documentos


        ocultoIDdocto.Value = gridv5pvpDocumentos.DataKeys[row.RowIndex].Values["id_documento"].ToString(); //id del documento

        if (!gridv5pvpDocumentos.DataKeys[row.RowIndex].Values["id_usuario_cumplimiento"].ToString().Equals("0"))
        {
            ///FALTA CAMBIAR ElementInformation NOMBRE delegate doctoMlnk PROQUE EnableViewState OTRO DIRECTORIO
            lblMdoctoNombre.Text = nombreDocumento;

            HtmlControl frame = (HtmlControl)iFrameMdocto; //.Attributes["src"] = "imgExpedientes/muestraDocto.asxp?archivo=" + nombreImagen;
            frame.Attributes["src"] = "imgExpedientes/muestraDoctoExterno.aspx?archivo=" + archivo;
            lblMdoctoNombre.Text = nombreDocumento;
            doctoMmodal.Show();
        }
    }
	
	public void MsgBox(String ex, Page pg,Object obj) 
	{
		string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
		Type cstype = obj.GetType();
		ClientScriptManager cs = pg.ClientScript;
		cs.RegisterClientScriptBlock(cstype, s, s.ToString());
	}
	
	public void btnCreaPDF(string v_id_expediente)
    {

		string rutaSalida, rutaFuente,  html_replace, documentoSalida, documentoFuente;
		string v_id_documento="";
		string file_salida="";
		string nombre_archivo="";
        int v_cuantos = 0;
		string[] arr_valores = new string[1];
		DataTable tablaDetailDocumento = null;
		
		html_replace = string.Empty;
		
		documentoSalida="";
		documentoFuente="";
		
		rutaSalida = Server.MapPath("~\\") + "/doctos_generados/";
		rutaFuente = Server.MapPath("~\\") + "/doctos_modelos/";
		
		//leer los datos del documento a Generar
		v_id_documento="94";
		string[] datosHeadDocumento = { v_id_documento };
		DataTable tablaHeadDocumento = Controladora.consultaDatos(sqlDocumentos.HeadDocumento, datosHeadDocumento);				
		
		if (tablaHeadDocumento.Rows.Count > 0)
        {		
			v_cuantos = Convert.ToInt32(tablaHeadDocumento.Rows[0][0].ToString());
			
			nombre_archivo=tablaHeadDocumento.Rows[0][3].ToString();
			documentoSalida = ocultoNumExpediente.Value.ToString();
			documentoSalida = documentoSalida.Replace("/","-");			
			documentoSalida = documentoSalida + "-" + tablaHeadDocumento.Rows[0][3].ToString();	//salida
			
			file_salida = rutaSalida + documentoSalida;		
			
			documentoFuente = tablaHeadDocumento.Rows[0][2].ToString();	//fuente								
			documentoFuente = rutaFuente + documentoFuente;	
			
			string v_etiqueta = "";
			string v_campo = "";
			string v_tabla = "";
			//string v_id_expediente = ocultoIdExpediente.Value.ToString();
			
			string errorSQL = "";
			string errorURL = "";
			string errorIdent = "";
			
			Array.Resize(ref arr_valores, v_cuantos);
			
			DataTable tablaValorCampo = null;
			string[] datosValorCampo = { "", "", ocultoIdExpediente.Value.ToString() };
			
			//ahora leer los campos para llenar el documento
			string[] datosDetailDocumento = { v_id_documento };
			tablaDetailDocumento = Controladora.consultaDatos(sqlDocumentos.DetailDocumento, datosDetailDocumento);
			if (tablaDetailDocumento.Rows.Count > 0)
			{
				int v_limite = tablaDetailDocumento.Rows.Count;
				int v_recorre = 0;
				while (v_recorre < v_limite) 
				{
					
					v_etiqueta = tablaDetailDocumento.Rows[v_recorre][8].ToString();
					v_campo = tablaDetailDocumento.Rows[v_recorre][4].ToString();
					v_tabla = tablaDetailDocumento.Rows[v_recorre][3].ToString();
					//Response.Write("etiqueta:");
					//Response.Write(v_etiqueta);
					//Response.Write("campo:");
					//Response.Write(v_campo);
					//Response.Write("tabla:");
					//Response.Write(v_tabla);
					
					//datosValorCampo = { v_campo, v_tabla, v_id_expediente };
					datosValorCampo[0] = v_campo;
					datosValorCampo[1] = v_tabla;
					datosValorCampo[2] = v_id_expediente;
					/*
					errorSQL = Controladora.regresaSentenciaSQL(sqlDocumentos.ValorCampo, datosValorCampo);
					errorURL = HttpContext.Current.Request.Url.AbsolutePath.ToString();
					errorIdent = "SolicitudProdNOM";
					Utilerias.registraSQL(errorSQL, errorURL, errorIdent);
					*/
					tablaValorCampo = Controladora.consultaDatos(sqlDocumentos.ValorCampo, datosValorCampo);
					arr_valores[v_recorre] = tablaValorCampo.Rows[0][0].ToString();
					//Response.Write(" valor:");
					//Response.Write(arr_valores[v_recorre]);					
					//Response.Write("<br>");
					v_recorre+=1;
					
				}
			}				
			
		}
		
        			
		//creacion del objeto para abrir el html            
	    StreamReader v_ReaderFuente = new StreamReader(documentoFuente);
		string linea = null;
		
		linea = v_ReaderFuente.ReadLine();
		while ((linea != null))
		{

			for (int v_recorre2 = 0; v_recorre2 < arr_valores.Length; v_recorre2++)
			{
				//hace los replaces correspondientes de la etiqueta en el html, por la variable que tiene el valor correspondiente
				linea = linea.Replace(tablaDetailDocumento.Rows[v_recorre2][8].ToString(), arr_valores[v_recorre2]);
			}
			
			//la linea "modificada" la guarda en el nuevo html_modificado
			html_replace += linea;
			//leer una linea de texto del HTML
			linea = v_ReaderFuente.ReadLine();
		}
		//cierra el archivo html de entrada
		v_ReaderFuente.Close();				
			
		//creacion del documento PDF		
		Document documentoCrearPdf = new Document(PageSize.LETTER, 30, 20, 20, 20);
		//...definimos el autor del documento.
		documentoCrearPdf.AddAuthor("Gestión ONNCCE");
		//...el creador, que será el mismo eh!
		documentoCrearPdf.AddCreator("Gestión ONNCCE");
		//hacemos que se inserte la fecha de creación para el documento
		documentoCrearPdf.AddCreationDate();
		//...título
		documentoCrearPdf.AddTitle("Generación de Documentos");
		//... el asunto
		documentoCrearPdf.AddSubject(nombre_archivo + ": " + ocultoNumExpediente.Value.ToString());
		//... palabras claves
		documentoCrearPdf.AddKeywords(nombre_archivo + ": " + ocultoNumExpediente.Value.ToString());

		PdfWriter.GetInstance(documentoCrearPdf, new FileStream(file_salida, FileMode.Create));
		documentoCrearPdf.Open();

		foreach (IElement E in HTMLWorker.ParseToList(new StringReader(html_replace), new StyleSheet()))
			documentoCrearPdf.Add(E);

		documentoCrearPdf.Close();


		//es un pdf            
		Response.Clear();
		Response.ClearContent();
		Response.ClearHeaders();
		Response.AddHeader("Content-Disposition", "attachment; filename=" + documentoSalida); //con esta linea provocamos el download, si la quitamos, abrimos el documento en la misma ventana
		Response.ContentType = "application/pdf";
		Response.TransmitFile(file_salida);
		Response.End();	

    }

	protected void btnProcesoEtapa_Click(object sender, EventArgs e)
    {
        /*
		Response.Write("IdExpediente");
		Response.Write( ocultoIdExpediente.Value.ToString() );
		Response.Write("IdUsuario");
		Response.Write( Session["idUsuario"].ToString() );		
		*/
		//lanzar la ejecucion del proceso
		string[] datosSPLanzaProceso = { ocultoIdExpediente.Value.ToString(), Session["idUsuario"].ToString() };
        DataTable tablaSPLanzaProceso = Controladora.consultaDatos(sqlExpediente.spLanzaProcesoEtapa, datosSPLanzaProceso);
		
		MsgBox("Se generado el proceso " + btnProcesoEtapa.Text + " de esta etapa", this.Page, this);
		
    }
	
	protected void validaProcesoVisible(string v_id_expediente)
    {

		string[] paramExpediente = { ocultoIdExpediente.Value.ToString() };

		//existe un proceso en esta etapa?
		DataTable datosExiste = Controladora.consultaDatos(sqlExpediente.HayProcesoEtapa, paramExpediente);
		
		if (datosExiste.Rows.Count > 0)
		{
			string existe = datosExiste.Rows[0]["existe"].ToString();
			
			if (existe.ToString().Equals("1")) 
			{
				pnlProcesos.Visible = true;
				btnProcesoEtapa.Visible = true;
				btnProcesoEtapa.Text = datosExiste.Rows[0]["titulo_proceso"].ToString();
				//Response.Write("true");			
			}		
			else
			{
				pnlProcesos.Visible = false;
				btnProcesoEtapa.Visible = false;
				//Response.Write("false");		
			}
		}
		else
		{
			pnlProcesos.Visible = false;
			btnProcesoEtapa.Visible = false;
			//Response.Write("false");		
		}
		
    }
	
}