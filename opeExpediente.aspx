<%@ Page Language="C#" MasterPageFile="~/header.master" AutoEventWireup="true"
    Inherits="opeExpediente" Title="- Expedientes -" Codebehind="opeExpediente.aspx.cs" %>

<%@ Register TagPrefix="ajx" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Src="usrControl/ventanaSuperior.ascx" TagName="ventanaSuperior" TagPrefix="uc1" %>
<%@ Register Src="usrControl/ventanaInferior.ascx" TagName="ventanaInferior" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajx:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajx:ToolkitScriptManager>
    <script language="javascript" type="text/javascript">
        function mayusculas(obj) {
            var texo = obj.value;
            obj.value = texo.toUpperCase();
        }
    </script>
    <asp:Panel runat="server" ID="pnlBuscar" HorizontalAlign="Center">
        <table cellpadding="0" cellspacing="0" border="0" align="center">
            <tr>
                <th colspan="4">
                    Buscar expediente
                </th>
            </tr>
            <tr>
                <th>
                    Núm. Expediente
                </th>
                <th>
                    Cliente
                </th>
                <th>
                    Norma
                </th>
                <th>
                    Tipo Servicio
                </th>
               <th>
                    Persona Contacto
                </th>				
                <th>
                    &nbsp;
                </th>
            </tr>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="txtExpediente"></asp:TextBox>
                    <ajx:FilteredTextBoxExtender ID="filtroNumCrd" runat="server" TargetControlID="txtExpediente"
                        FilterType="Custom" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚ/-0123456789">
                    </ajx:FilteredTextBoxExtender>
                </td>
				<td>
					<asp:DropDownList ID="listaCliente" runat="server">
					</asp:DropDownList>
				</td>
                <td>                    
					<asp:DropDownList ID="listaNorma" runat="server">
					</asp:DropDownList>                   
                </td>
                <td>
					<asp:DropDownList ID="listaTipoServicio" runat="server">
					</asp:DropDownList>                   

                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPersonaContacto"></asp:TextBox>
                    <ajx:FilteredTextBoxExtender ID="filtroPersonaContacto" runat="server" TargetControlID="txtPersonaContacto"
                        FilterType="Custom" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚ/-0123456789">
                    </ajx:FilteredTextBoxExtender>
                </td>				
                <td>
                    <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" />
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" border="0" align="center">
            <tr>
                <th>
                    &nbsp;
                </th>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gridResultadoBuscar" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="id_expediente,id_etapa,id_flujo,id_cliente,id_sucursal,num_expediente,id_empresa,id_estatus_ligado"
                        OnSelectedIndexChanged="gridResultadoBuscar_SelectedIndexChanged" CellPadding="4"
                        ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="num_expediente" HeaderText="Expediente" />
                            <asp:BoundField DataField="nombre" HeaderText="Cliente" />
                            <asp:BoundField DataField="fch_expediente" HeaderText="Fecha alta" />
                            <asp:BoundField DataField="siglas" HeaderText="Servicio" />
                            <asp:BoundField DataField="estatus" HeaderText="Estatus" />
							<asp:BoundField DataField="Ligado" HeaderText="Ligado" />							
							<asp:BoundField DataField="padre" HeaderText="Padre" />	
                            <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gridPersonaSeleccionada" runat="server" Visible="False" CellPadding="4"
                        ForeColor="#333333" GridLines="None" AutoGenerateColumns="False">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="num_expediente" HeaderText="Crédito" />
                            <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="fch_expediente" HeaderText="Fecha alta" />
                            <asp:BoundField DataField="siglas" HeaderText="Producto" />
                            <asp:BoundField DataField="estatus" HeaderText="Estatus" />
							<asp:BoundField DataField="Ligado" HeaderText="Ligado" />							
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlTitulosEtapa" runat="server" Visible="false">
        <table cellpadding="0" cellspacing="5" border="0" align="center">
            <tr>
                <td>
                    <asp:Table ID="htmlTablaExpediente" runat="server">
                    </asp:Table>
                </td>
                <td>
                    <iframe id="frameObsEtapa" runat="server" src="~/varios/frameObsEtapa.aspx" scrolling="auto"
                        style="height: 166px; width: 512px;"></iframe>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblGeneralTitulo" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    Observaciones<asp:TextBox ID="txtObsEtapa" runat="server" Width="684px"></asp:TextBox><asp:Button
                        ID="btnAgregaObsEtapa" runat="server" Text="Agregar" OnClick="btnAgregaObsEtapa_Click"
                        Style="height: 26px" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlMensaje" runat="server" Visible="False">
        <div id="divPnlMensaje" align="center">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="labelNormal">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td align="center" valign="top">
                                    <asp:ImageButton ID="imgCreaMensaje" runat="server" ImageUrl="~/images/mensaje_crear.png"
                                        ToolTip="Click para crear un nuevo mensaje" OnClick="imgCreaMensaje_Click" />
                                    <asp:Button ID="btnCreaMensaje" runat="server" Text="Crear mensaje" OnClick="btnCreaMensaje_Click"
                                        Visible="False" />
                                </td>
                                <td align="center" valign="top">
                                    <asp:ImageButton ID="btnAltaUsuario" runat="server" ImageUrl="~/images/mensaje_normal.png"
                                        ToolTip="Alta Usuario" OnClick="btnAltaUsuario_Click" />
                                </td>
                                <td align="center" valign="top">
                                    <asp:ImageButton ID="btnv5uv_alta" runat="server" ImageUrl="~/images/notario.jpg"
                                        ToolTip="Asigna Unidad de Valuación/Notario" Visible="false" OnClick="btnv5uv_alta_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td class="BotonTxt" valign="top" style="padding-left: 5px; padding-right: 5px">
                                    Nuevo<br />
                                    Mensaje
                                </td>
                                <td class="BotonTxt" valign="top" style="padding-left: 5px; padding-right: 5px">
                                    Leer<br />
                                    Mensajes
                                </td>
                                <td class="BotonTxt" valign="top" style="padding-left: 5px; padding-right: 5px">
                                    <asp:Label ID="lblv5uv_alta" runat="server" Visible="False">Asigna<br>Unidad<br>Notario</br></br></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <!--PANEL DE MENSAJES ENTRE USUARIOS PARA EL EXPEDIENTE-->
    <asp:Panel ID="pnlLecturaMensajes" runat="server" Visible="false" Height="400px"
        ScrollBars="Auto" Width="1000px">
        <div id="divPnlLecturaMensajes" align="center">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="right">
                        <asp:ImageButton ID="imgCierraPnlLecturaMens" runat="server" ImageUrl="~/images/btnCancela.jpg"
                            OnClick="imgCierraPnlLecturaMens_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gridLecturaMensajes" runat="server" AutoGenerateColumns="False"
                            CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDataBound="gridLecturaMensajes_RowDataBound">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="prioridad" HeaderText="Prioridad" />
                                <asp:BoundField DataField="titulo" HeaderText="Asunto" />
                                <asp:BoundField DataField="fch_envio" HeaderText="Fecha Envío" />
                                <asp:BoundField DataField="nombre" HeaderText="Enviado" />
                                <asp:BoundField DataField="destinatario" HeaderText="Destinatario" />
                                <asp:BoundField DataField="leido" HeaderText="Leido" />
                                <asp:BoundField DataField="mensaje" HeaderText="Mensaje" />
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <!--//PANEL DE MENSAJES ENTRE USUARIOS PARA EL EXPEDIENTE-->

    <div id="divObjetos" align="center">
		<asp:Panel ID="pnlObjetos" runat="server" Visible="False">
			<fieldset>
			
			<legend>Datos Requeridos de la Etapa</legend>  
			
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        </br>
                    </td>
                </tr>							                
                <tr>
                    <td>
                        </br>
                    </td>
                </tr>				
                <tr>
                    <td>
                        <asp:Label ID="objLblNUM1" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtNUM1" runat="server" Visible="false"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="objTxtNUM1"
                            FilterType="Custom" ValidChars="0123456789.">
                        </ajx:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblNUM2" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtNUM2" runat="server" Visible="false"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="objTxtNUM2"
                            FilterType="Custom" ValidChars="0123456789.">
                        </ajx:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblNUM3" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtNUM3" runat="server" Visible="false"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="objTxtNUM3"
                            FilterType="Custom" ValidChars="0123456789.">
                        </ajx:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblTexto1" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtTexto1" runat="server" Visible="false"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="objTxtTexto1"
                            FilterType="Custom" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚ -_/#?¿!¡0123456789:">
                        </ajx:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblTexto2" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtTexto2" runat="server" Visible="false"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="objTxtTexto2"
                            FilterType="Custom" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚ -_/#?¿!¡0123456789:">
                        </ajx:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblTexto3" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtTexto3" runat="server" Visible="false"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="objTxtTexto3"
                            FilterType="Custom" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚ -_/#?¿!¡0123456789:">
                        </ajx:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblTexto4" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtTexto4" runat="server" Visible="false"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="objTxtTexto4"
                            FilterType="Custom" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚ -_/#?¿!¡0123456789:">
                        </ajx:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblTexto5" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtTexto5" runat="server" Visible="false"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="objTxtTexto5"
                            FilterType="Custom" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚ -_/#?¿!¡0123456789:">
                        </ajx:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblTexto6" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtTexto6" runat="server" Visible="false"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="objTxtTexto6"
                            FilterType="Custom" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚ -_/#?¿!¡0123456789:">
                        </ajx:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblTexto7" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtTexto7" runat="server" Visible="false"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="objTxtTexto7"
                            FilterType="Custom" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚ -_/#?¿!¡0123456789:">
                        </ajx:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblTexto8" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtTexto8" runat="server" Visible="false"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" TargetControlID="objTxtTexto8"
                            FilterType="Custom" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚ -_/#?¿!¡0123456789:">
                        </ajx:FilteredTextBoxExtender>
                    </td>
                </tr>
				                <tr>
                    <td>
                        <asp:Label ID="objLblTexto9" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtTexto9" runat="server" Visible="false"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="objTxtTexto9"
                            FilterType="Custom" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚ -_/#?¿!¡0123456789:">
                        </ajx:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblTexto10" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtTexto10" runat="server" Visible="false"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" TargetControlID="objTxtTexto10"
                            FilterType="Custom" ValidChars="abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóúÁÉÍÓÚ -_/#?¿!¡0123456789:">
                        </ajx:FilteredTextBoxExtender>
                    </td>
                </tr>
				
                <tr>
                    <td>
                        <asp:Label ID="objLblFecha1" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtFecha1" runat="server" MaxLength="10" Visible="false" Width="80px"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="objTxtFecha1"
                            FilterType="Custom" ValidChars="0123456789/">
                        </ajx:FilteredTextBoxExtender>
                        <ajx:CalendarExtender PopupPosition="TopRight" ID="CalendarExtender1" runat="server"
                            TargetControlID="objTxtFecha1" Format="dd/MM/yyyy">
                        </ajx:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblFecha2" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtFecha2" runat="server" MaxLength="10" Visible="false" Width="80px"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="objTxtFecha2"
                            FilterType="Custom" ValidChars="0123456789/">
                        </ajx:FilteredTextBoxExtender>
                        <ajx:CalendarExtender PopupPosition="TopRight" ID="CalendarExtender2" runat="server"
                            TargetControlID="objTxtFecha2" Format="dd/MM/yyyy">
                        </ajx:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblFecha3" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtFecha3" runat="server" MaxLength="10" Visible="false" Width="80px"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="objTxtFecha3"
                            FilterType="Custom" ValidChars="0123456789/">
                        </ajx:FilteredTextBoxExtender>
                        <ajx:CalendarExtender PopupPosition="TopRight" ID="CalendarExtender3" runat="server"
                            TargetControlID="objTxtFecha3" Format="dd/MM/yyyy">
                        </ajx:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblFecha4" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtFecha4" runat="server" MaxLength="10" Visible="false" Width="80px"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" TargetControlID="objTxtFecha4"
                            FilterType="Custom" ValidChars="0123456789/">
                        </ajx:FilteredTextBoxExtender>
                        <ajx:CalendarExtender PopupPosition="TopRight" ID="CalendarExtender4" runat="server"
                            TargetControlID="objTxtFecha4" Format="dd/MM/yyyy">
                        </ajx:CalendarExtender>
                    </td>
                </tr>				
                <tr>
                    <td>
                        <asp:Label ID="objLblFecha5" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtFecha5" runat="server" MaxLength="10" Visible="false" Width="80px"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender123" runat="server" TargetControlID="objTxtFecha5"
                            FilterType="Custom" ValidChars="0123456789/">
                        </ajx:FilteredTextBoxExtender>
                        <ajx:CalendarExtender PopupPosition="TopRight" ID="CalendarExtender5" runat="server"
                            TargetControlID="objTxtFecha5" Format="dd/MM/yyyy">
                        </ajx:CalendarExtender>
                    </td>
                </tr>				
                <tr>
                    <td>
                        <asp:Label ID="objLblFecha6" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="objTxtFecha6" runat="server" MaxLength="10" Visible="false" Width="80px"></asp:TextBox>
                        <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender122" runat="server" TargetControlID="objTxtFecha6"
                            FilterType="Custom" ValidChars="0123456789/">
                        </ajx:FilteredTextBoxExtender>
                        <ajx:CalendarExtender PopupPosition="TopRight" ID="CalendarExtender6" runat="server"
                            TargetControlID="objTxtFecha6" Format="dd/MM/yyyy">
                        </ajx:CalendarExtender>
                    </td>
                </tr>				

                <tr>
                    <td colspan="2" align="left">
                        <asp:CheckBox ID="objChkCheck1" runat="server" Text="Chk1" Visible="false" />
                        <asp:CheckBox ID="objChkCheck2" runat="server" Text="Chk2" Visible="false" />
                        <asp:CheckBox ID="objChkCheck3" runat="server" Text="Chk3" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblCombo1" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="objListaCombo1" runat="server" Visible="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblCombo2" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="objListaCombo2" runat="server" Visible="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblCombo3" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="objListaCombo3" runat="server" Visible="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblCombo4" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="objListaCombo4" runat="server" Visible="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblCombo5" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="objListaCombo5" runat="server" Visible="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="objLblCombo6" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="objListaCombo6" runat="server" Visible="false">
                        </asp:DropDownList>
                    </td>
                </tr>				
                <tr>
                    <td>
                        </br>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnCapturaCamposEspeciales" runat="server" Text="Guardar Datos"
                            OnClick="btnCapturaCamposEspeciales_Click1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        </br>
                    </td>
                </tr>				
            </table>
            <asp:Table ID="htmlTablaCamposEspeciales" runat="server">
            </asp:Table>
            <asp:Label ID="lblGeneralPnlCamposEspeciales" runat="server" Visible="false"></asp:Label>
		
		</fieldset>
		
		</asp:Panel>
	</div>
	
	
	<!--//PANEL DE MENSAJES ENTRE USUARIOS PARA EL EXPEDIENTE-->

	<div id="divProcesos" align="center">
		<asp:Panel ID="pnlProcesos" runat="server" Visible="False">
            <fieldset>
                <legend>Procesos en la Etapa</legend>                    
			    <tr>
                    <td>
                        </br>
                    </td>
                </tr>	
                
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnProcesoEtapa" runat="server" Text=""
                            OnClick="btnProcesoEtapa_Click" 
							Visible="False"
							/>							
										
                    </td>
                </tr>	
				<tr>
                    <td>
                        </br></br>
                    </td>
                </tr>	
			</fieldset>
		</asp:Panel>			
	</div>
    
			
    <!--/CAMPOS ESPECIALES-->
    <div id="divGeneral" align="center">
        <asp:Label ID="lblGeneral" runat="server" Text="Label" Visible="False"></asp:Label>
        <asp:Panel ID="pnlDocumentos" runat="server" Visible="false">
            <fieldset>
                <legend>Documentos</legend>
                <asp:Button ID="btnDoctosEtapaAnterior" runat="server" Text="Documentos de la etapa anterior"
                    OnClick="btnDoctosEtapaAnterior_Click" />
                <asp:Button ID="btnDoctosEtapaActual" runat="server" Text="Documentos de la etapa"
                    OnClick="btnDoctosEtapaActual_Click" />

                <asp:Panel ID="pnlBtnDoctosActual" runat="server">
                    <div id="divTitDocumentosEtapaActual" style="font-weight: bold">
                        Lista de documentos de la etapa
                    </div>
                    <asp:GridView ID="gridDoctosEtapa" runat="server" AutoGenerateColumns="False" EmptyDataText="No existen Documentos"
                        DataKeyNames="id_ope_documento,documento,id_captura,id_documento,nombre_archivo,fecha_modifica_documento,id_funcion_docto, nombre_tipo_documento, id_entidad_responsable"
                        OnSelectedIndexChanged="gridDoctosEtapa_SelectedIndexChanged" CellPadding="4"
                        ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="id_ope_documento" HeaderText="id" />
                            <asp:BoundField DataField="documento" HeaderText="Documento" />
							<asp:BoundField DataField="nombre_tipo_documento" HeaderText="Tipo Documento" />
							
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnSubeDoctoActual" runat="server" ImageUrl='<%# validaEstatus(Eval("id_captura")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
							
                            <asp:TemplateField>
                                <ItemTemplate>
									<asp:ImageButton ID="btnEnviaDoctoActual" runat="server" 
										OnCommand="btnEnviaDoctoActual_command" CommandName="funciones_documento"
										CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") + "|" + Eval("id_funcion_docto") + "|" + Eval("id_documento") + "|" + Eval("id_captura") + "|" + Eval("id_ope_documento") + "|" + Eval("nombre_archivo") + "|" + Eval("documento") + "|" + Eval("id_entidad_responsable") %>'
										ImageUrl='<%# validaFuncion(Eval("id_funcion_docto")) %>'
										Visible='<%# Convert.ToBoolean(validaVisible(Eval("id_funcion_docto"))) %>'
									/>
									
                                </ItemTemplate>
                            </asp:TemplateField>
							
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                    <asp:GridView ID="gridv5pvpDocumentos" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="id_proyecto_viv_documentos,nombre_documento,id_proyecto_vivienda,id_documento,id_usuario_cumplimiento,nombre_archivo"
                        OnSelectedIndexChanged="gridv5pvpDocumentos_SelectedIndexChanged" Visible="false">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="id_documento" HeaderText="ID docto" />
                            <asp:BoundField DataField="nombre_documento" HeaderText="documento" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnSubeDoctoActual" runat="server" ImageUrl='<%# validaEstatus(Eval("id_usuario_cumplimiento")) %>'
                                        CommandName="Select" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="pnlDoctosAnterior" runat="server" Visible="false">
                    <div id="div1" style="font-weight: bold">
                        Lista de documentos de la etapa Anterior
                    </div>
                    <asp:GridView ID="gridDoctosAnterior" runat="server" AutoGenerateColumns="False"
                        EmptyDataText="No existen Documentos" DataKeyNames="id_ope_documento,documento,id_captura,id_documento,nombre_archivo,fecha_modifica_documento"
                        OnSelectedIndexChanged="gridDoctosAnterior_SelectedIndexChanged" CellPadding="4"
                        ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="id_ope_documento" HeaderText="id" />
                            <asp:BoundField DataField="documento" HeaderText="Documento" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnSubeDoctoActual" runat="server" ImageUrl='<%# validaEstatus(Eval("id_captura")) %>'
                                        CommandName="Select" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblGeneralDoctosAnterior" runat="server" Text="lblGeneralDoctosAnterior" Visible="False"></asp:Label>
                </asp:Panel>
                <asp:Panel ID="pnlAvisosDoctosAnterior" runat="server" BackColor="White">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr bgcolor="#3f688d">
                            <td bgcolor="#3f688d">
                                <asp:Label ID="Label2" runat="server" CssClass="txtTituloModal" Font-Bold="True"
                                    ForeColor="White" Text="Documentos de la Etapa Anterior"></asp:Label>
                            </td>
                            <td align="right" bgcolor="#3f688d" width="16px">
                                <asp:ImageButton ID="btnCierraModalDoctosAnterior" runat="server" ImageUrl="~/images/btnCancela.jpg"
                                    ToolTip="Cerrar ventana" OnClick="btnCierraModalDoctosAnterior_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="txtContenidoModal" colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            Actualmente, ya tiene un documento asociado para
                                            <asp:Label ID="lblNombreDoctoAnterior" runat="server" Font-Bold="True"></asp:Label>
                                            <br />
                                            La fecha en que capturó el documento fue:
                                            <asp:Label ID="lblFchDoctoAnterior" runat="server" Font-Bold="True"></asp:Label>
                                            <br />
                                            Si desea eliminar este documento, haga clic en el botón indicado.
                                            <br />
                                            <br />
                                            Al eliminar el documento, automáticamente regresará a la etapa anterior y deberá
                                            <br />
                                            capturar las observaciones del porque elimina el documento y el porque
                                            <br />
                                            regresa a la etapa anterior<br />
                                            <br />
                                            Si no desea regresar a la etapa anterior, solo cierre esta ventana y no
                                            <br />
                                            modificará el documento.
                                        </td>
                                    </tr>
                                </table>
                                <asp:Label ID="lblGeneralModalDoctosAnterior" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
							<td align="center" colspan="2">
								Observaciones Documento:
                                <asp:TextBox ID="txtObservaDoctosAnterior" runat="server" Width="594px"></asp:TextBox>
                                <br />
                                Observaciones Regreso Etapa:
                                <asp:TextBox ID="txtObservaRegresaEtapa" runat="server" Width="579px"></asp:TextBox>
                                <div align="center" id="divPnlDoctosEtpAnterior">
                                    <asp:Label ID="lblGeneralPnlDoctosAnterior" runat="server"></asp:Label>
                                </div>
							</td>	
						</tr>
						<tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnRegresaEtapa" runat="server" Text="Eliminar documento" OnClick="btnRegresaEtapa_Click" />
                                <asp:Button ID="btnCierraAvisoDoctosAnterior" runat="server" Text="Cancelar" OnClick="btnCierraAvisoDoctosAnterior_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Panel ID="pnlImgDoctoAnterior" runat="server" Width="1000px" Height="450px">
                                    <iframe id="frameDoctosAnterior" runat="server" src="" scrolling="auto" height="440"
                                        width="990"></iframe>
                                </asp:Panel>
                                
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:LinkButton ID="lnkDPnlDoctosAnterior" runat="server"></asp:LinkButton>
                <ajx:ModalPopupExtender ID="modalDoctosAnterior" runat="server" BackgroundCssClass="FondoAplicacion"
                    BehaviorID="modalDoctosAnterior" DropShadow="true" PopupControlID="pnlAvisosDoctosAnterior"
                    TargetControlID="lnkDPnlDoctosAnterior">
                </ajx:ModalPopupExtender>
                <asp:Panel ID="pnlDoctosSiguiente" runat="server" Visible="false">
                    <div id="div2" style="font-weight: bold">
                        Lista de documentos de la etapa Siguiente
                    </div>
                    <asp:GridView ID="gridDoctosSiguiente" runat="server" AutoGenerateColumns="False"
                        EmptyDataText="No existen Documentos" DataKeyNames="id_ope_documento,documento,id_captura,id_documento,nombre_archivo,fecha_modifica_documento"
                        CellPadding="4" ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="id_ope_documento" HeaderText="id" />
                            <asp:BoundField DataField="documento" HeaderText="Documento" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnSubeDoctoActual" runat="server" ImageUrl='<%# validaEstatus(Eval("id_captura")) %>'
                                        CommandName="Select" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblGeneralDoctosSiguiente" runat="server" Text="lblGeneralDoctosSiguiente"
                        Visible="False"></asp:Label>
                </asp:Panel>
                <hr />
                <asp:Panel ID="pnlDoctosActualImg" runat="server" Visible="false">
                    <table>
                        <tr>
                            <th colspan="2">
                                Captura imagen
                            </th>
                        </tr>
                        <tr>
                            <td>
                                Documento:
                            </td>
                            <td>
                                <asp:Label ID="lblNombreDocumento" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:FileUpload ID="fileSubeDocto" runat="server" />
                                <asp:Button ID="btnSubeDoctoActual" runat="server" OnClick="btnSubeDoctoActual_Click"
                                    Text="Subir documento" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlAvisoDoctosActual" runat="server" BackColor="White">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr bgcolor="#3f688d">
                            <td bgcolor="#3f688d">
                                <asp:Label ID="lblTxtTitulo" runat="server" CssClass="txtTituloModal" Font-Bold="True"
                                    ForeColor="White" Text="Documentos de la Etapa Actual"></asp:Label>
                            </td>
                            <td align="right" bgcolor="#3f688d" width="16px">
                                <asp:ImageButton ID="btnCerrarVentanaDoctos" runat="server" ImageUrl="~/images/btnCancela.jpg"
                                    ToolTip="Cerrar ventana" OnClick="btnCerrarVentanaDoctos_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="txtContenidoModal" colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            Actualmente, ya tiene un documento asociado para
                                            <asp:Label ID="lblNombreDocto" runat="server" Font-Bold="True"></asp:Label>
                                            <br />
                                            La fecha en que capturó el documento fue:
                                            <asp:Label ID="lblFchSubeDocto" runat="server" Font-Bold="True"></asp:Label>
                                            <br />
                                            Si desea eliminar este documento, haga clic en el botón indicado,
                                            <br />
                                            y escriba las observaciones, en caso contrario, solo cierre la ventana
                                        </td>
                                    </tr>
                                </table>
                                <asp:Label ID="lblDoctosMensajes" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
							<td align="center" colspan="2">
								Observaciones:
                                <asp:TextBox ID="txtObservaDoctoActualElimina" runat="server" Width="667px"></asp:TextBox>
                                <br />
                                <asp:Label ID="lblGeneralPnlDoctosActual" runat="server" Visible="false"></asp:Label>
							</td>
						</tr>
						<tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnEliminaDoctoActual" runat="server" Text="Eliminar Documento" OnClick="btnEliminaDoctoActual_Click" />
                                <asp:Button ID="btnCancelarModDocto" runat="server" Text="Cancelar" OnClick="btnCancelarModDocto_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Panel ID="pnlImg" runat="server" Width="1000px" Height="450px">
                                								
                                    <iframe id="frameDoctosActual" runat="server" src="" scrolling="auto" height="440"
                                        width="990"></iframe>
                                </asp:Panel>
                                
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:LinkButton ID="lnkPnlDoctos" runat="server"></asp:LinkButton>
                <ajx:ModalPopupExtender ID="modalDoctosActual" runat="server" BackgroundCssClass="FondoAplicacion"
                    BehaviorID="modalDoctosActual" DropShadow="true" PopupControlID="pnlAvisoDoctosActual"
                    TargetControlID="lnkPnlDoctos">
                </ajx:ModalPopupExtender>
                <asp:Label ID="lblGeneralDoctos" runat="server" Text="texto" Visible="False"></asp:Label>
                <br />
                <iframe id="frameObsDoctos" runat="server" src="varios/frameObsEtapa.aspx" scrolling="auto"
                    style="height: 109px; width: 681px;"></iframe>
            </fieldset>
        </asp:Panel>
        <asp:Panel ID="pnlTramites" runat="server" Visible="false">
            <fieldset>
                <legend>Trámites</legend>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td>
                            <asp:Button ID="btnTramitesEtapaAnterior" runat="server" Text="Trámites etapa anterior"
                                OnClick="btnTramitesEtapaAnterior_Click" />
                            <asp:Button ID="btnTranitesEtapaActual" runat="server" Text="Trámites etapa actual"
                                OnClick="btnTranitesEtapaActual_Click" />

                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlTramitesBtnAnterior" runat="server" Visible="false">
                    <b>Trámites de la etapa Anterior</b>
                    <asp:GridView ID="gridTramitesAnterior" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="id_ope_tramite,estatus,nombre_tramite"
                        OnSelectedIndexChanged="gridTramitesAnterior_SelectedIndexChanged" EmptyDataText="No existen Trámites">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="id_ope_tramite" HeaderText="id" />
                            <asp:BoundField DataField="nombre_tramite" HeaderText="Trámite" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnVerificaEstatus" runat="server" ImageUrl='<%# validaEstatus(Eval("estatus")) %>'
                                        CommandName="Select" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="pnlAvisoTramitesAnterior" runat="server" BackColor="White">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr bgcolor="#3f688d">
                            <td bgcolor="#3f688d">
                                <asp:Label ID="lblTitTramitesAnterior" runat="server" CssClass="txtTituloModal" Font-Bold="true"
                                    ForeColor="White" Text="Trámites"></asp:Label>
                            </td>
                            <td align="right" bgcolor="#3f688d" width="16px">
                                <asp:ImageButton ID="btnCierraModalTramAnt" runat="server" ImageUrl="~/images/btnCancela.jpg"
                                    ToolTip="Cerrar ventana" OnClick="btnCierraModalTramAnt_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="txtContenidoModal" colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            Ya se había cumplido con el trámite
                                            <asp:Label ID="lblNombreTramiteEtpAnterior" runat="server" Font-Bold="True"></asp:Label>
                                            <br />
                                            Si desea cancelar el cumplimiento, haga clic en el botón indicado.
                                            <br />
                                            <br />
                                            Al eliminar el cumplimiento, automáticamente regresa el expediente a la etapa anterior<br />
                                            y deberá capturar las observaciones:<br />
                                            <asp:TextBox ID="txtObservaTrmEtp" runat="server" Width="562px"></asp:TextBox>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <asp:Label ID="lblGeneralModalTramEtAnt" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnEliminaTramEtpAnt" runat="server" Text="Eliminar Trámite" OnClick="btnEliminaTramEtpAnt_Click" />
                                <asp:Button ID="btnCierraModalTramitesAnterior" runat="server" Text="Cancelar" OnClick="btnCierraModalTramitesAnterior_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:LinkButton ID="lnkPnlTramAnterior" runat="server"></asp:LinkButton>
                <ajx:ModalPopupExtender ID="modalTramitesAnterior" runat="server" BackgroundCssClass="FondoAplicacion"
                    BehaviorID="modalTramitesAnterior" DropShadow="true" PopupControlID="pnlAvisoTramitesAnterior"
                    TargetControlID="lnkPnlTramAnterior">
                </ajx:ModalPopupExtender>
                <asp:Panel ID="pnlTramitesBtnActual" runat="server" Visible="false">
                    <b>Trámites de la etapa Actual</b>
                    <asp:GridView ID="gridTramitesActual" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="id_ope_tramite,estatus,nombre_tramite"
                        OnSelectedIndexChanged="gridTramitesActual_SelectedIndexChanged" EmptyDataText="No existen Trámites">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="id_ope_tramite" HeaderText="id" />
                            <asp:BoundField DataField="nombre_tramite" HeaderText="Trámite" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnVerificaEstatus" runat="server" ImageUrl='<%# validaEstatus(Eval("estatus")) %>'
                                        CommandName="Select" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                    <asp:GridView ID="gridv5pvpTramites" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="None" DataKeyNames="id_proyecto_viv_tramites,nombre_tramite,id_tramite,id_usuario_cumplimiento"
                        Visible="false">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="id_tramite" HeaderText="ID tramite" />
                            <asp:BoundField DataField="nombre_tramite" HeaderText="Trámite" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnSubeDoctoActual" runat="server" ImageUrl='<%# validaEstatus(Eval("id_usuario_cumplimiento")) %>'
                                        CommandName="Select" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="pnlTramitesBtnSiguiente" runat="server" Visible="false">
                    <b>Trámites de la etapa Siguiente</b>
                    <asp:GridView ID="gridTramitesSiguiente" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" ForeColor="#333333" GridLines="None" EmptyDataText="No existen Trámites">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="id_ope_tramite" HeaderText="id" />
                            <asp:BoundField DataField="nombre_tramite" HeaderText="Trámite" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnVerificaEstatus" runat="server" ImageUrl='<%# validaEstatus(Eval("estatus")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </asp:Panel>
                <asp:Label ID="lblGeneralTramites" runat="server" Visible="false"></asp:Label>
            </fieldset>
        </asp:Panel>
    </div>
    <br />
    <asp:Panel ID="pnlTabExpediente" runat="server">
        <div id="divTabExpediente" align="center">
            <ajx:TabContainer ID="DatosExpediente" runat="server" ActiveTabIndex="1" Visible="false"
                Width="645px">
                <ajx:TabPanel ID="TabGuardaValores" runat="server" HeaderText="TabPanel1">
                    <HeaderTemplate>
                        Presupuesto
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                            <tr>
                                <td>
                                    <asp:Table ID="htmlTablaGuardaValores" runat="server">
                                    </asp:Table>
                                    <br />
                                    <asp:Button ID="btnModGuardaValores" runat="server" Visible="False" OnClick="btnModGuardaValores_Click"
                                        Text="Modificar Datos" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajx:TabPanel>
                <ajx:TabPanel ID="TabNotario" runat="server" HeaderText="TabPanel2">
                    <HeaderTemplate>
                        Preliminar
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                            <tr>
                                <td>
                                    <asp:Table ID="htmlTablaNotario" runat="server">
                                    </asp:Table>
                                    <br />
                                    <asp:Button ID="btnModNotario" runat="server" Visible="False" Text="Modificar Datos"
                                        OnClick="btnModNotario_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajx:TabPanel>
                <ajx:TabPanel ID="TabVivienda" runat="server" HeaderText="TabPanel1">
                    <HeaderTemplate>
                        Gestión
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                            <tr>
                                <td align="center">
                                    <asp:Table ID="htmlTablaVivienda" runat="server">
                                    </asp:Table>
                                    <asp:Button ID="btnCopeVivienda" runat="server" Text="Actualiza datos de vivienda"
                                        OnClick="btnCopeVivienda_Click" />
                                    <br />
                                    <asp:Button ID="btnModVivienda" runat="server" Visible="False" Text="Modificar Datos"
                                        OnClick="btnModVivienda_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajx:TabPanel>
                <ajx:TabPanel ID="TabLaborales" runat="server" HeaderText="TabPanel1">
                    <HeaderTemplate>
                        Certificado
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                            <tr>
                                <td>
                                    <asp:Table ID="htmlTablaLaborales" runat="server">
                                    </asp:Table>
                                    <br />
                                    <asp:Button ID="btnCLaborales" runat="server" Text="Actualiza datos Laborales" OnClick="btnCLaborales_Click" />
                                    <asp:Button ID="btnModLaborales" runat="server" Visible="False" Text="Modificar Datos"
                                        OnClick="btnModLaborales_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajx:TabPanel>
                <ajx:TabPanel ID="TabCrediticio" runat="server" HeaderText="TabPanel1">
                    <HeaderTemplate>
                        Muestreo
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                            <tr>
                                <td>
                                    <asp:Table ID="htmlTablaCrediticio" runat="server">
                                    </asp:Table>
                                    <br />
                                    <asp:Button ID="btnModCrediticio" runat="server" Visible="False" Text="Modificar Datos" OnClick="btnModCrediticio_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajx:TabPanel>
                <ajx:TabPanel ID="TabPersona" runat="server" HeaderText="TabPanel1">
                    <HeaderTemplate>
                        Cliente
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                            <tr>
                                <td>
                                    <asp:Table ID="htmlTablaPersona" runat="server">
                                    </asp:Table>
                                    <asp:Button ID="btnCopePersona" runat="server" Text="Modifica datos Persona" OnClick="btnCopePersona_Click" />
                                    <br />
                                    <asp:Button ID="btnModPersona" runat="server" Visible="False" Text="Modificar Datos"
                                        OnClick="btnModPersona_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajx:TabPanel>
				
                <ajx:TabPanel ID="TabNorma" runat="server" HeaderText="TabPanel1">
                    <HeaderTemplate>
                        Norma
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                            <tr>
                                <td>
                                    <asp:Table ID="htmlTablaNorma" runat="server">
                                    </asp:Table>
                                    <asp:Button ID="btnModNorma" runat="server" Visible="False" Text="Modificar Datos" OnClick="btnModNorma_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajx:TabPanel>

                <ajx:TabPanel ID="TabNormasFamilias" runat="server" HeaderText="TabPanel1">
                    <HeaderTemplate>
                        Normas Familias Productos
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                            <tr>
                                <td>
									<!--VENTANA Muestra Normas Familias Productos-->
									<asp:Panel ID="pnlWinNormasFamilias" runat="server" BackColor="White" Visible="False">
										<table border="0" cellpadding="0" cellspacing="0">
											<tr>
												<td align="right" bgcolor="#3f688d" width="16px">                  
												<asp:GridView ID="gridNormasFamilias" runat="server" Visible="True"
														AutoGenerateColumns="True" CellPadding="4" ForeColor="#333333"
														GridLines="None">
													<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
													
													<EditRowStyle BackColor="#999999" />
													<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
													<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
													<PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
													<RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
													<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
													<SortedAscendingCellStyle BackColor="#E9E7E2" />
													<SortedAscendingHeaderStyle BackColor="#506C8C" />
													<SortedDescendingCellStyle BackColor="#FFFDF8" />
													<SortedDescendingHeaderStyle BackColor="#6F8DAE" />
												</asp:GridView>                    
												</td>
											</tr>											
										</table>
									</asp:Panel>									
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajx:TabPanel>

                <ajx:TabPanel ID="TabFacturasProyecto" runat="server" HeaderText="TabPanel1">
                    <HeaderTemplate>
                        Facturas del Proyecto
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                            <tr>
                                <td>
									<!--VENTANA Muestra Facturas del Proyecto-->
									<asp:Panel ID="pnlWinFacturasProyecto" runat="server" BackColor="White" Visible="False">
										<table border="0" cellpadding="0" cellspacing="0">
											<tr>
												<td align="right" bgcolor="#3f688d" width="16px">                  
												<asp:GridView ID="gridFacturasProyecto" runat="server" Visible="True"
														AutoGenerateColumns="True" CellPadding="4" ForeColor="#333333"
														GridLines="None">
													<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
													
													<EditRowStyle BackColor="#999999" />
													<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
													<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
													<PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
													<RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
													<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
													<SortedAscendingCellStyle BackColor="#E9E7E2" />
													<SortedAscendingHeaderStyle BackColor="#506C8C" />
													<SortedDescendingCellStyle BackColor="#FFFDF8" />
													<SortedDescendingHeaderStyle BackColor="#6F8DAE" />
												</asp:GridView>                    
												</td>
											</tr>											
										</table>
									</asp:Panel>									
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajx:TabPanel>

                <ajx:TabPanel ID="TabCertificados" runat="server" HeaderText="TabPanel1">
                    <HeaderTemplate>
                        Certificados del Proyecto
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                            <tr>
                                <td>
									<!--VENTANA Muestra Certificados del Proyecto-->
									<asp:Panel ID="pnlWinCertificadosProyecto" runat="server" BackColor="White" Visible="False">
										<table border="0" cellpadding="0" cellspacing="0">
											<tr>
												<td align="right" bgcolor="#3f688d" width="16px">                  
												<asp:GridView ID="gridCertificadosProyecto" runat="server" Visible="True"
														AutoGenerateColumns="True" CellPadding="4" ForeColor="#333333"
														GridLines="None">
													<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
													
													<EditRowStyle BackColor="#999999" />
													<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
													<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
													<PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
													<RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
													<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
													<SortedAscendingCellStyle BackColor="#E9E7E2" />
													<SortedAscendingHeaderStyle BackColor="#506C8C" />
													<SortedDescendingCellStyle BackColor="#FFFDF8" />
													<SortedDescendingHeaderStyle BackColor="#6F8DAE" />
												</asp:GridView>                    
												</td>
											</tr>											
										</table>
									</asp:Panel>									
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajx:TabPanel>

				
            </ajx:TabContainer>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlValidaciones" runat="server" Visible="False">
        <div align="center">
            Validaciones de etapa
            <br />
            <asp:Button ID="btnValidaDoctosEtapa" runat="server" OnClick="btnValidaDoctosEtapa_Click"
                Text="Valida documentos etapa" />
            <asp:Button ID="btnValidaTramitesEtapa" runat="server" OnClick="btnValidaTramitesEtapa_Click"
                Text="Valida trámites etapa" />
            <br />
            <asp:Button ID="btnRegresaExpInicioEtp" runat="server" OnClick="btnRegresaExpInicioEtp_Click"
                Text="Regresa Expediente al incio" />
            <asp:Button ID="btnRegresaEtpAnterior" runat="server" Text="Regresa a la etapa anterior"
                OnClick="btnRegresaEtpAnterior_Click" />
            <asp:Button ID="btnAvanzaEtapa" runat="server" Enabled="True" OnClick="btnAvanzaEtapa_Click"
                Text="Avanza etapa" />
            <br />
            <asp:Button ID="btnVerCalculoTasaInteres" runat="server" OnClick="btnVerCalculoTasaInteres_Click"
                Text="Ver Cálculo del Crédito" Visible="False" Style="height: 26px" />
            <br />
            <asp:Button ID="btnVerDocumentos" runat="server" Text="Ver documentos" OnClick="btnVerDocumentos_Click" />
            <br />
            <asp:Label ID="lblGeneralValidaciones" runat="server" Text="Label" Visible="False"></asp:Label>
            <br />
            <asp:Panel ID="pnlMgridDoctos" runat="server" Visible="false">
                <div id="divPnlMgidDoctos" align="center">
                    <asp:GridView runat="server" ID="gridMDoctos" CellPadding="4" ForeColor="#333333"
                        GridLines="None" AutoGenerateColumns="False" DataKeyNames="nombre_archivo,nombre_documento"
                        OnSelectedIndexChanged="gridMDoctos_SelectedIndexChanged">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="clasificacion" HeaderText="Clasificación" />
							<asp:BoundField DataField="nombre_documento" HeaderText="Documento" />
                            <asp:BoundField DataField="nombre_etapa" HeaderText="Etapa" />
                            <asp:BoundField DataField="usuario_alta" HeaderText="Alta por" />
                            <asp:BoundField DataField="fecha_alta" HeaderText="Fecha alta" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnMuestraDoctoSel" runat="server" CommandName="Select" ImageUrl="~/images/application_form_Muestra.png"
                                        Width="15px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </div>
            </asp:Panel>
        </div>
        <br />
        <br />
        <table cellpadding="10" cellspacing="0" border="0" align="center">
            <tr>
                <td>
                    Observaciones cuando se regresa a la etapa inicial
                </td>
                <td>
                    Observaciones cuando se regresa de etapa
                </td>
            </tr>
            <tr>
                <td>
                    <iframe id="frameObsRegresoEtapaInicial" runat="server" src="varios/frameObsEtapa.aspx"
                        scrolling="auto" style="height: 200px; width: 500px;"></iframe>
                </td>
                <td>
                    <iframe id="frameObsRegresaEtapa" runat="server" src="varios/frameObsEtapa.aspx"
                        scrolling="auto" style="height: 200px; width: 500px;"></iframe>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlAvisoEtapaSiguiente" runat="server" BackColor="White">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr bgcolor="#3f688d">
                    <td bgcolor="#3f688d">
                        <asp:Label ID="Label1" runat="server" CssClass="txtTituloModal" Font-Bold="true"
                            ForeColor="White" Text="Documentos"></asp:Label>
                    </td>
                    <td align="right" bgcolor="#3f688d" width="16px">
                        <asp:ImageButton ID="btnCierraAvisosEtpSig" runat="server" ImageUrl="~/images/btnCancela.jpg"
                            ToolTip="Cerrar ventana" OnClick="btnCierraAvisosEtpSig_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="center" class="txtContenidoModal" colspan="2">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    El expediente se ha pasado a la siguiente etapa
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnAceptaEtapaSig" runat="server" Text="Aceptar" OnClick="btnAceptaEtapaSig_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:LinkButton ID="lnkPnlAvisoEtpSig" runat="server"></asp:LinkButton>
        <ajx:ModalPopupExtender ID="modalAvisoEtpSig" runat="server" BackgroundCssClass="FondoAplicacion"
            BehaviorID="modalAvisoEtpSig" DropShadow="true" PopupControlID="pnlAvisoEtapaSiguiente"
            TargetControlID="lnkPnlDoctos">
        </ajx:ModalPopupExtender>
    </asp:Panel>
    <asp:Panel ID="pnlVentanaRegresaEtapaInicial" runat="server" BackColor="White">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr bgcolor="#3f688d">
                <td bgcolor="#3f688d">
                    <asp:Label ID="Label3" runat="server" CssClass="txtTituloModal" Font-Bold="True"
                        ForeColor="White" Text="Regresa Etapa Inicial"></asp:Label>
                </td>
                <td align="right" bgcolor="#3f688d" width="16px">
                    <asp:ImageButton ID="cierraModalRegresaEtapaInicial" runat="server" ImageUrl="~/images/btnCancela.jpg"
                        ToolTip="Cerrar ventana" OnClick="cierraModalRegresaEtapaInicial_Click" />
                </td>
            </tr>
            <tr>
                <td align="center" class="txtContenidoModal" colspan="2">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                ¿Esta seguro de regresar el Expediente hasta la etapa inicial?<br />
                                <br />
                                Si acepta, debe capturar las observaciones, de lo contrario solo cierre la ventana
                                <br />
                                Observaciones:<asp:TextBox ID="txtObsRegresaEtapaInicial" runat="server" Width="428px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblGeneralRegresaEtapaInicial" runat="server" Visible="false" Text="lblGeneralRegresaEtapaInicial"></asp:Label>
                    <br />
                    <asp:Button ID="btnAObsEtapaInicial" runat="server" Text="Aceptar" OnClick="btnAObsEtapaInicial_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:LinkButton ID="lnkVentanaRegresaEtapaInicial" runat="server"></asp:LinkButton>
    <ajx:ModalPopupExtender ID="modalVentanaRegresaEtapaInicial" runat="server" BackgroundCssClass="FondoAplicacion"
        BehaviorID="modalVentanaRegresaEtapaInicial" DropShadow="true" PopupControlID="pnlVentanaRegresaEtapaInicial"
        TargetControlID="lnkVentanaRegresaEtapaInicial">
    </ajx:ModalPopupExtender>
    <!--PANEL PARA MOSTRAR LA CAPTURA DE OBSERVACIONES CUANDO SE REGRESA A LA ETAPA ANTERIOR-->
    <asp:Panel ID="pnlVentanaRegresaEtapaAnterior" runat="server" BackColor="White">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr bgcolor="#3f688d">
                <td bgcolor="#3f688d">
                    <asp:Label ID="Label4" runat="server" CssClass="txtTituloModal" Font-Bold="True"
                        ForeColor="White" Text="Regresa Etapa Anterior"></asp:Label>
                </td>
                <td align="right" bgcolor="#3f688d" width="16px">
                    <asp:ImageButton ID="cierraVentanaRegresaEtapaAnteior" runat="server" ImageUrl="~/images/btnCancela.jpg"
                        ToolTip="Cerrar ventana" OnClick="cierraVentanaRegresaEtapaAnteior_Click" />
                </td>
            </tr>
            <tr>
                <td align="center" class="txtContenidoModal" colspan="2">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                ¿Esta seguro de regresar el Expediente a la etapa anterior?<br />
                                <br />
                                Si acepta, debe capturar las observaciones, de lo contrario solo cierre la ventana
                                <br />
                                Observaciones:<asp:TextBox ID="txtAobsRegresaEtapaAnterior" runat="server" Width="428px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblGeneralRegresaEtapaAnterior" runat="server" Visible="false" Text="lblGeneralRegresaEtapaAnterior"></asp:Label>
                    <br />
                    <asp:Button ID="btnAobsRegresaEtapaAnterior" runat="server" Text="Aceptar" OnClick="btnAobsRegresaEtapaAnterior_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:LinkButton ID="lnkRegresaEtapaAnterior" runat="server"></asp:LinkButton>
    <ajx:ModalPopupExtender ID="modalVentanaRegresEtapaAnterior" runat="server" BackgroundCssClass="FondoAplicacion"
        BehaviorID="modalVentanaRegresEtapaAnterior" DropShadow="true" PopupControlID="pnlVentanaRegresaEtapaAnterior"
        TargetControlID="lnkRegresaEtapaAnterior">
    </ajx:ModalPopupExtender>
    <asp:Panel ID="pnlVentanaAviso" runat="server" BackColor="White">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr bgcolor="#3f688d">
                <td bgcolor="#3f688d">
                    <asp:Label ID="lblAviso" runat="server" CssClass="txtTituloModal" Font-Bold="True"
                        ForeColor="White" Text="Aviso"></asp:Label>
                </td>
                <td align="right" bgcolor="#3f688d" width="16px">
                    <asp:ImageButton ID="cierraVentanaAvisoRegresaEtapaInicial" runat="server" ImageUrl="~/images/btnCancela.jpg"
                        ToolTip="Cerrar ventana" OnClick="cierraVentanaAvisoRegresaEtapaInicial_Click" />
                </td>
            </tr>
            <tr>
                <td align="center" class="txtContenidoModal" colspan="2">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="lblTextoAviso" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnAvisoRegresaEtapaInicial" runat="server" Text="Aceptar" OnClick="btnAvisoRegresaEtapaInicial_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:LinkButton ID="lnkVentanaAviso" runat="server"></asp:LinkButton>
    <ajx:ModalPopupExtender ID="modalVentanaAviso" runat="server" BackgroundCssClass="FondoAplicacion"
        BehaviorID="modalVentanaAviso" DropShadow="true" PopupControlID="pnlVentanaAviso"
        TargetControlID="lnkVentanaAviso">
    </ajx:ModalPopupExtender>
    <!--VENTANA GENERAL AVISO-->
    <asp:Panel ID="ventanaAviso" runat="server" BackColor="White">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr bgcolor="#3f688d">
                <td bgcolor="#3f688d">
                    <asp:Label ID="Label6" runat="server" CssClass="txtTituloModal" Font-Bold="True"
                        ForeColor="White" Text="Aviso"></asp:Label>
                </td>
                <td align="right" bgcolor="#3f688d" width="16px">
                    <asp:ImageButton ID="ventanaCierraX" runat="server" ImageUrl="~/images/btnCancela.jpg"
                        ToolTip="Cerrar ventana" OnClick="ventanaCierraX_Click" />
                </td>
            </tr>
            <tr>
                <td align="center" class="txtContenidoModal" colspan="2">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="ventanaMensaje" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="ventanaBotonCerrar" runat="server" Text="Aceptar" OnClick="ventanaBotonCerrar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:LinkButton ID="ventanaLink" runat="server"></asp:LinkButton>
    <ajx:ModalPopupExtender ID="ventanaModal" runat="server" BackgroundCssClass="FondoAplicacion"
        BehaviorID="ventanaModal" DropShadow="true" PopupControlID="ventanaAviso" TargetControlID="ventanaLink">
    </ajx:ModalPopupExtender>
    <!--/VENTANA GENERAL AVISO-->
    <!--VENTANA Modifica datos OPE_VIVIENDA-->
    <asp:Panel ID="pnlWinCopeVivienda" runat="server" BackColor="White">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr bgcolor="#3f688d">
                <td bgcolor="#3f688d">
                    <asp:Label ID="lblWinCopeViv" runat="server" CssClass="txtTituloModal" Font-Bold="True"
                        ForeColor="White" Text="Modifica datos de Vivienda"></asp:Label>
                </td>
                <td align="right" bgcolor="#3f688d" width="16px">
                    <asp:ImageButton ID="btnWinCcierraCopeV" runat="server" ImageUrl="~/images/btnCancela.jpg"
                        ToolTip="Cerrar ventana" OnClick="btnWinCcierraCopeV_Click" />
                </td>
            </tr>
            <tr>
                <td align="center" class="txtContenidoModal" colspan="2">
                    <table cellpadding="0" cellspacing="0" border="0" align="center">
                        <tr>
                            <td>
                                País
                            </td>
                            <td>
                                <asp:TextBox ID="txtPais" runat="server" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Entidad
                            </td>
                            <td>
                                <asp:DropDownList ID="listaEntidad" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Municipio
                            </td>
                            <td>
                                <asp:TextBox ID="txtMunicipio" runat="server" MaxLength="60"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Dirección
                            </td>
                            <td>
                                <asp:TextBox ID="txtDireccion" runat="server" Height="84px" TextMode="MultiLine"
                                    Width="266px" MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                C.P.
                            </td>
                            <td>
                                <asp:TextBox ID="txtCp" runat="server" MaxLength="5"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Ciudad
                            </td>
                            <td>
                                <asp:TextBox ID="txtCiudad" runat="server" MaxLength="60"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Colonia
                            </td>
                            <td>
                                <asp:TextBox ID="txtColonia" runat="server" MaxLength="60"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Téléfono
                            </td>
                            <td>
                                <asp:TextBox ID="txtVivTelefono" runat="server" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Celular
                            </td>
                            <td>
                                <asp:TextBox ID="txtVivCelular" runat="server" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblWinMensajeCopeViv" runat="server" Visible="false"></asp:Label>
                    <br />
                    <asp:Button ID="btnWinCopeViv" runat="server" Text="Modificar" OnClick="btnWinCopeViv_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:LinkButton ID="lnkWinCopeViv" runat="server"></asp:LinkButton>
    <ajx:ModalPopupExtender ID="modalWinCopeViv" runat="server" BackgroundCssClass="FondoAplicacion"
        BehaviorID="modalWinCopeViv" DropShadow="true" PopupControlID="pnlWinCopeVivienda"
        TargetControlID="lnkWinCopeViv">
    </ajx:ModalPopupExtender>
    <!--/VENTANA Modifica datos OPE_VIVIENDA-->
    <!--VENTANA Modifica datos OPE_LABORALES-->
    <asp:Panel ID="pnlWinOpeLaborales" runat="server" BackColor="White">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr bgcolor="#3f688d">
                <td bgcolor="#3f688d">
                    <asp:Label ID="lblWinTitVentana" runat="server" CssClass="txtTituloModal" Font-Bold="True"
                        ForeColor="White" Text="Modifica datos Laborales"></asp:Label>
                </td>
                <td align="right" bgcolor="#3f688d" width="16px">
                    <asp:ImageButton ID="btnWinCierraCopeLab" runat="server" ImageUrl="~/images/btnCancela.jpg"
                        ToolTip="Cerrar ventana" OnClick="btnWinCierraCopeLab_Click" />
                </td>
            </tr>
            <tr>
                <td align="center" class="txtContenidoModal" colspan="2">
                    <table cellpadding="0" cellspacing="0" border="0" align="center">
                        <tr>
                            <td>
                                Nombre de la dependencia
                            </td>
                            <td>
                                <asp:TextBox ID="txtLabDependencia" runat="server" MaxLength="60"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Dirección de adscripción
                            </td>
                            <td>
                                <asp:TextBox ID="txtLabDirAd" runat="server" TextMode="MultiLine" MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Área de trabajo
                            </td>
                            <td>
                                <asp:TextBox ID="txtLabAreaTrab" runat="server" MaxLength="60"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Puesto
                            </td>
                            <td>
                                <asp:TextBox ID="txtLabPuesto" runat="server" MaxLength="60"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Antigüedad
                            </td>
                            <td>
                                Años<asp:DropDownList ID="listaAntiguiAnios" runat="server">
                                </asp:DropDownList>
                                Meses<asp:DropDownList ID="listaAntiguoMeses" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                País
                            </td>
                            <td>
                                <asp:TextBox ID="txtLabPais" runat="server" MaxLength="60"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Estado
                            </td>
                            <td>
                                <asp:DropDownList ID="listaLabEstado" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Municipio
                            </td>
                            <td>
                                <asp:TextBox ID="txtLAbMunicipio" runat="server" MaxLength="60"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Dirección
                            </td>
                            <td>
                                <asp:TextBox ID="txtLabDireccion" runat="server" TextMode="MultiLine" MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                C.P.
                            </td>
                            <td>
                                <asp:TextBox ID="txtLabCP" runat="server" MaxLength="5"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Ciudad
                            </td>
                            <td>
                                <asp:TextBox ID="txtLabCiudad" runat="server" MaxLength="60"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Colonia
                            </td>
                            <td>
                                <asp:TextBox ID="txtLabColonia" runat="server" MaxLength="60"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Teléfono
                            </td>
                            <td>
                                <asp:TextBox ID="txtLabTelefono" runat="server" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Pensionado
                            </td>
                            <td>
                                <asp:DropDownList ID="listaLabPensionado" runat="server">
                                    <asp:ListItem Selected="True" Value="0">No pendionado</asp:ListItem>
                                    <asp:ListItem Value="1">Pensionado</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="listaLabTipoPensionado" runat="server">
                                    <asp:ListItem Selected="True" Value="0">Ninguno</asp:ListItem>
                                    <asp:ListItem Value="1">IMSS</asp:ListItem>
                                    <asp:ListItem Value="2">FOVISSSTE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblWinOpeLabGeneral" runat="server" Visible="False"></asp:Label>
                    <br />
                    <asp:Button ID="btnWinCopeLab" runat="server" Text="Modificar" OnClick="btnWinCopeLab_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:LinkButton ID="lnkWinCopeLab" runat="server"></asp:LinkButton>
    <ajx:ModalPopupExtender ID="modalWinCopeLab" runat="server" BackgroundCssClass="FondoAplicacion"
        BehaviorID="modalWinCopeLab" DropShadow="true" PopupControlID="pnlWinOpeLaborales"
        TargetControlID="lnkWinCopeLab">
    </ajx:ModalPopupExtender>
    <!--/VENTANA Modifica datos OPE_LABORALES-->
    <!--VENTANA Modifica datos OPE_PERSONA-->
    <asp:Panel ID="pnlWinOpePersona" runat="server" BackColor="White">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr bgcolor="#3f688d">
                <td bgcolor="#3f688d">
                    <asp:Label ID="Label7" runat="server" CssClass="txtTituloModal" Font-Bold="True"
                        ForeColor="White" Text="Modifica datos Persona"></asp:Label>
                </td>
                <td align="right" bgcolor="#3f688d" width="16px">
                    <asp:ImageButton ID="btnWinCierraOpePersona" runat="server" ImageUrl="~/images/btnCancela.jpg"
                        ToolTip="Cerrar ventana" OnClick="btnWinCierraOpePersona_Click" />
                </td>
            </tr>
            <tr>
                <td align="center" class="txtContenidoModal" colspan="2">
                    <table cellpadding="0" cellspacing="0" border="0" align="center">
                        <tr>
                            <td>
                                CLIENTE
                            </td>
                            <td>
                                <asp:TextBox ID="txtPerNombre" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                PLANTA
                            </td>
                            <td>
                                <asp:TextBox ID="txtApPaterno" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                AREA
                            </td>
                            <td>
                                <asp:TextBox ID="txtApMaterno" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Fecha Nacimiento
                            </td>
                            <td>
                                <asp:DropDownList ID="listaDiaNac" runat="server">
                                </asp:DropDownList>
                                <asp:DropDownList ID="listaMesNac" runat="server">
                                </asp:DropDownList>
                                <asp:DropDownList ID="listaAnioNac" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                C.U.R.P.
                            </td>
                            <td>
                                <asp:TextBox ID="txtCurp" runat="server" MaxLength="18"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                R.F.C.
                            </td>
                            <td>
                                <asp:TextBox ID="txtRfc" runat="server" MaxLength="13"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tipo Persona
                            </td>
                            <td>
                                <asp:DropDownList ID="listaTpoPersona" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Teléfono
                            </td>
                            <td>
                                <asp:TextBox ID="txtTelefono" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Celular
                            </td>
                            <td>
                                <asp:TextBox ID="txtCelular" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                E-mail
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Banco
                            </td>
                            <td>
                                <asp:DropDownList ID="listaPerBancos" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Clabe Interbancaria
                            </td>
                            <td>
                                <asp:TextBox ID="txtPerClabe" runat="server" MaxLength="18"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblWinGeneralCopePersona" runat="server" Visible="False"></asp:Label>
                    <br />
                    <asp:Button ID="btnWinCopePersona" runat="server" Text="Modificar" OnClick="btnWinCopePersona_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:LinkButton ID="lnkWinCopePersona" runat="server"></asp:LinkButton>
    <ajx:ModalPopupExtender ID="ModalWinCopePersona" runat="server" BackgroundCssClass="FondoAplicacion"
        BehaviorID="ModalWinCopePersona" DropShadow="true" PopupControlID="pnlWinOpePersona"
        TargetControlID="lnkWinCopePersona">
    </ajx:ModalPopupExtender>
    <!--/VENTANA Modifica datos OPE_PERSONA-->
    <!--MODAL CALCULO TASA INTERES-->
    <asp:Panel ID="pnlVentanaTI" runat="server" BackColor="White">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr bgcolor="#3f688d">
                <td bgcolor="#3f688d">
                    <asp:Label ID="Label5" runat="server" CssClass="txtTituloModal" Font-Bold="True"
                        ForeColor="White" Text="Cálculo del Crédito"></asp:Label>
                </td>
                <td align="right" bgcolor="#3f688d" width="16px">
                    <asp:ImageButton ID="cierraVentanaPnlTI" runat="server" ImageUrl="~/images/btnCancela.jpg"
                        ToolTip="Cerrar ventana" OnClick="cierraVentanaPnlTI_Click" />
                </td>
            </tr>
            <tr>
                <td align="center" class="txtContenidoModal" colspan="2">
                    <!--CONTENIDO MODAL-->
                    <asp:Panel ID="pnlCalculo" runat="server" Visible="true">
                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                            <tr>
                                <th colspan="2">
                                    Cálculo del Crédito
                                </th>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Sueldo
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSueldo" runat="server"></asp:TextBox>
                                </td>
                                <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtSueldo"
                                    FilterType="Custom, Numbers" ValidChars="0123456789.">
                                </ajx:FilteredTextBoxExtender>
                            </tr>
                            <tr>
                                <td>
                                    Monto crédito
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMontoCredito" runat="server"></asp:TextBox>
                                </td>
                                <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtMontoCredito"
                                    FilterType="Custom, Numbers" ValidChars="0123456789.">
                                </ajx:FilteredTextBoxExtender>
                            </tr>
                            <tr>
                                <td>
                                    Plazo (en meses)
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPlazo" runat="server"></asp:TextBox>
                                </td>
                                <ajx:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtPlazo"
                                    FilterType="Custom, Numbers" ValidChars="0123456789">
                                </ajx:FilteredTextBoxExtender>
                            </tr>
                            <tr>
                                <td>
                                    Periodo de pago
                                </td>
                                <td>
                                    <asp:DropDownList ID="listaPeriodo" runat="server">
                                        <asp:ListItem Text="Mensual" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Quincenal" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Semanal" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tipo Persona
                                </td>
                                <td>
                                    <asp:DropDownList ID="listaTipoPersona" runat="server">
                                        <asp:ListItem Text="Público" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Empleado" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th colspan="2">
                                    <asp:Button ID="btnCalcTIprevio" runat="server" Text="Previo" OnClick="btnCalcTIprevio_Click"
                                        Visible="False" />
                                    <asp:Button ID="btnCalcTIGraba" runat="server" Text="Genera Tasa Interés" OnClick="btnCalcTIGraba_Click" />
                                </th>
                            </tr>
                        </table>
                        <div id="div3" align="center">
                            <asp:Label ID="lblGnPnlCalculo" runat="server" Visible="False"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlResultados" runat="server" Visible="false" Height="400px">
                        <div id="divPnlRes" align="center">
                            <ajx:TabContainer ID="tabAmortiza" runat="server" ActiveTabIndex="0" Width="800px"
                                ScrollBars="Auto" Height="300px">
                                <ajx:TabPanel ID="tabAmortizaDatos" runat="server" HeaderText="TabPanel1">
                                    <HeaderTemplate>
                                        Datos
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Table ID="htmlTablaDatosCrd" runat="server">
                                        </asp:Table>
                                    </ContentTemplate>
                                </ajx:TabPanel>
                                <ajx:TabPanel ID="tabAmortizaVenta" runat="server" HeaderText="TabPanel1">
                                    <HeaderTemplate>
                                        Venta</HeaderTemplate>
                                    <ContentTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="gridVenta" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                        <EditRowStyle BackColor="#999999" />
                                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </ajx:TabPanel>
                                <ajx:TabPanel ID="tabAmortizaIsoluto" runat="server" HeaderText="TabPanel2">
                                    <HeaderTemplate>
                                        Insoluto</HeaderTemplate>
                                    <ContentTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="gridInsoluto" runat="server" CellPadding="4" ForeColor="#333333"
                                                        GridLines="None">
                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                        <EditRowStyle BackColor="#999999" />
                                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </ajx:TabPanel>
                            </ajx:TabContainer>
                        </div>
                    </asp:Panel>
                    <!--//CONTENIDO MODAL-->
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:LinkButton ID="lnkVentanaTI" runat="server"></asp:LinkButton>
    <ajx:ModalPopupExtender ID="modalVentanaTI" runat="server" BackgroundCssClass="FondoAplicacion"
        BehaviorID="modalVentanaTI" DropShadow="true" PopupControlID="pnlVentanaTI" TargetControlID="lnkVentanaTI">
    </ajx:ModalPopupExtender>
    <!--//MODAL CALCULO TASA INTERES-->
    <!--VENTANA MUESTRA DOCUMENTOS-->
    <asp:Panel ID="pnlMDoctos" runat="server" BackColor="White">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr bgcolor="#3f688d">
                <td bgcolor="#3f688d">
                    <asp:Label ID="Label8" runat="server" CssClass="txtTituloModal" Font-Bold="True"
                        ForeColor="White" Text="Documentos en el sistema"></asp:Label>
                </td>
                <td align="right" bgcolor="#3f688d" width="16px">
                    <asp:ImageButton ID="btnXmDoctos" runat="server" ImageUrl="~/images/btnCancela.jpg"
                        ToolTip="Cerrar ventana" OnClick="btnXmDoctos_Click" />
                </td>
            </tr>
            <tr>
                <td align="center" class="txtContenidoModal" colspan="2">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                Documento
                                <asp:Label ID="lblMdoctoNombre" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="pnlMdoctoFrame" runat="server" Width="1000px" Height="450px">
                        <iframe id="iFrameMdocto" runat="server" src="" scrolling="auto" height="440" width="990">
                        </iframe>
                        <asp:GridView ID="mnsjGridUsuariosExp" runat="server" AutoGenerateColumns="False"
                            CellPadding="4" DataKeyNames="id" EmptyDataText="Sin usuarios" ForeColor="#333333"
                            GridLines="None">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="ID" />
                                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                <asp:TemplateField HeaderText="Seleccionar">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkUsrSel" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:LinkButton ID="doctoMlnk" runat="server"></asp:LinkButton>
    <ajx:ModalPopupExtender ID="doctoMmodal" runat="server" BackgroundCssClass="FondoAplicacion"
        BehaviorID="doctoMmodal" DropShadow="true" PopupControlID="pnlMDoctos" TargetControlID="doctoMlnk">
    </ajx:ModalPopupExtender>
    <!--//VENTANA MUESTRA DOCUMENTOS-->
    <!--VENTANA ENVIA MENSAJES-->
    <asp:Panel ID="MnsjPnlVentana" runat="server" BackColor="White">
        <uc1:ventanaSuperior ID="ucVentanaMnsj" runat="server" />
        <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td>
                    Título
                </td>
                <td>
                    <asp:TextBox ID="mnsjTxtTitulo" runat="server" Width="444px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Prioridad
                </td>
                <td>
                    <asp:DropDownList ID="mnsjListaPrioridad" runat="server">
                        <asp:ListItem Text="Baja" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Media" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Alta" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Mensaje
                </td>
                <td>
                    <asp:TextBox ID="mnsjTxtMensaje" runat="server" Height="61px" TextMode="MultiLine"
                        Width="444px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td>
                    Usuarios
                    <hr />
                    <asp:Panel ID="mnsjPnlFrameUsuarios" runat="server" Height="75px" ScrollBars="Auto">
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    Supervisores
                    <hr />
                    <asp:Panel ID="mnsjPnlFrameSupervisor" runat="server" Height="75px" ScrollBars="Auto">
                        <asp:GridView ID="mnsjGridSupervisores" runat="server" CellPadding="4" EmptyDataText="Sin usuarios"
                            ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" DataKeyNames="id">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="ID" />
                                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                <asp:TemplateField HeaderText="Seleccionar">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkUsrSel" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                        <asp:ImageButton ID="ImageButton1" runat="server" />
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    Todos
                    <hr />
                    <asp:Panel ID="mnsjPnlFrameTodos" runat="server" Height="75px" ScrollBars="Auto">
                        <asp:GridView ID="mnsjGridTodos" runat="server" CellPadding="4" EmptyDataText="Sin usuarios"
                            ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" DataKeyNames="id">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="ID" />
                                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                <asp:TemplateField HeaderText="Seleccionar">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkUsrSel" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="mnsjBtnEnvia" runat="server" Text="Enviar mensaje" OnClick="mnsjBtnEnvia_Click" />
                    <br />
                    <asp:Label ID="mnsjLblGeneral" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
        <uc2:ventanaInferior ID="ventanaInferior1" runat="server" />
    </asp:Panel>
    <asp:LinkButton ID="MnsjLnk" runat="server"></asp:LinkButton>
    <ajx:ModalPopupExtender ID="MnsjModal" runat="server" BackgroundCssClass="FondoAplicacion"
        BehaviorID="MnsjModal" DropShadow="true" PopupControlID="MnsjPnlVentana" TargetControlID="MnsjLnk">
    </ajx:ModalPopupExtender>
    <!--//VENTANA ENVIA MENSAJES-->
    <!--VENTANA Modifica datos TABS-->
    <asp:Panel ID="pnlTABSvntn" runat="server" BackColor="White">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr bgcolor="#3f688d">
                <td bgcolor="#3f688d">
                    <asp:Label ID="lblTABSTitulo" runat="server" CssClass="txtTituloModal" Font-Bold="True"
                        ForeColor="White" Text="Modifica datos"></asp:Label>
                </td>
                <td align="right" bgcolor="#3f688d" width="16px">
                    <asp:ImageButton ID="btnTABSCierra" runat="server" ImageUrl="~/images/btnCancela.jpg"
                        ToolTip="Cerrar ventana" OnClick="btnTABSCierra_Click" />
                </td>
            </tr>
            <tr>
                <td align="center" class="txtContenidoModal" colspan="2">
                    <!--contenido-->
                    <iframe id="iframeTABS" runat="server" src="admin/cambios.aspx?nombreTabla=ope_cat_notarios"
                        scrolling="auto" style="height: 166px; width: 512px;"></iframe>
                    <!--//contenido-->
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:LinkButton ID="lnkTABS" runat="server"></asp:LinkButton>
    <ajx:ModalPopupExtender ID="modalTABS" runat="server" BackgroundCssClass="FondoAplicacion"
        BehaviorID="modalTABS" DropShadow="true" PopupControlID="pnlTABSvntn" TargetControlID="lnkTABS">
    </ajx:ModalPopupExtender>
    <!--/VENTANA Modifica datos TABS-->
    <!--VENTANA v5uv-->
    <asp:Panel ID="pnlv5uv" runat="server" BackColor="White">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr bgcolor="#3f688d">
                <td bgcolor="#3f688d">
                    <asp:Label ID="lblv5uvTituloVentana" runat="server" CssClass="txtTituloModal" Font-Bold="True"
                        ForeColor="White" Text="Asigna Unidad de Valuación/Notario"></asp:Label>
                </td>
                <td align="right" bgcolor="#3f688d" width="16px">
                    <asp:ImageButton ID="btnv5uvCierraVentana" runat="server" ImageUrl="~/images/btnCancela.jpg"
                        ToolTip="Cerrar ventana" OnClick="btnv5uvCierraVentana_Click" />
                </td>
            </tr>
            <tr>
                <td align="center" class="txtContenidoModal" colspan="2">
                    <!--contenido-->
                    Seleccione la opción:
                    <br />
                    <asp:RadioButton ID="radiov5uvUnidad" Text="Unidad de valuación" GroupName="uv" runat="server"
                        Checked AutoPostBack="True" OnCheckedChanged="radiov5uvUnidad_CheckedChanged" />
                    <asp:RadioButton ID="radiov5uvNotario" Text="Notario" GroupName="uv" runat="server"
                        AutoPostBack="True" OnCheckedChanged="radiov5uvNotario_CheckedChanged" />
                    <br />
                    <asp:DropDownList ID="listav5uvUnidadValuacion" runat="server">
                    </asp:DropDownList>
                    <asp:DropDownList ID="listav5uvNotario" runat="server" Visible="false">
                    </asp:DropDownList>
                    <!--//contenido-->
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnv5uvAsigna" runat="server" Text="Asigna" OnClick="btnv5uvAsigna_Click" />
                    <br />
                    <asp:GridView ID="gridv5uvAsignados" runat="server" CellPadding="4" EmptyDataText="Sin asignaciones"
                        ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:LinkButton ID="lnkv5uv" runat="server"></asp:LinkButton>
    <ajx:ModalPopupExtender ID="modalv5uv" runat="server" BackgroundCssClass="FondoAplicacion"
        BehaviorID="modalv5uv" DropShadow="true" PopupControlID="pnlv5uv" TargetControlID="lnkv5uv">
    </ajx:ModalPopupExtender>
    <!--/VENTANA v5uv-->
    <asp:HiddenField ID="ocultoIdExpediente" runat="server" />
    <asp:HiddenField ID="ocultoIDetapa" runat="server" />
    <asp:HiddenField ID="ocultoIDflujo" runat="server" />
    <asp:HiddenField ID="ocultoIDdocto" runat="server" />
    <asp:HiddenField ID="ocultoIDopeDocto" runat="server" />
    <asp:HiddenField ID="ocultoFchSubeImg" runat="server" />
    <asp:HiddenField ID="ocultoNombreImg" runat="server" />
    <asp:HiddenField ID="ocultoIDsiguienteEtapa" runat="server" />
    <asp:HiddenField ID="ocultoResultadoDocumentos" runat="server" Value="0" />
    <asp:HiddenField ID="ocultoResultadoTramites" runat="server" Value="0" />
    <asp:HiddenField ID="ocultoIDopeDoctoAnterior" runat="server" />
    <asp:HiddenField ID="ocultoNombreImgDoctoAnterior" runat="server" />
    <asp:HiddenField ID="ocultoIDtramiteEtpAnt" runat="server" />
    <asp:HiddenField ID="ocultoNombreTramiteEtpAnt" runat="server" />
    <asp:HiddenField ID="ocultoIDrglDoctoAnterior" runat="server" />
    <asp:HiddenField ID="ocultoIDfase" runat="server" />
    <asp:HiddenField ID="ocultoIDfaseAlterno" runat="server" />
    <asp:HiddenField ID="ventanaOcultoTpoCierra" runat="server" Value="0" />
    <asp:HiddenField ID="ocultoOpeViviendaDatos" runat="server" Value="0" />
    <asp:HiddenField ID="ocultoOpeLaboralesDatos" runat="server" Value="0" />
    <asp:HiddenField ID="ocultoIdCliente" runat="server" />
    <asp:HiddenField ID="ocultoOpePersonaDatos" runat="server" Value="0" />
    <asp:HiddenField ID="ocultoMdispCrd" runat="server" Value="0" />
    <asp:HiddenField ID="ocultoMontoCapital" runat="server" />
    <asp:HiddenField ID="ocultoIDsucursal" runat="server" />
    <asp:HiddenField ID="ocultoIDempresaExpediente" runat="server" />
    <asp:HiddenField ID="ocultoNombreVista" runat="server" />
    <asp:HiddenField ID="ocultoNombreVistaTAB" runat="server" />
    <asp:HiddenField ID="ocultoV5ftvMuestraDoctos" runat="server" Value="0" />
	<asp:HiddenField ID="ocultoValorCondicion" runat="server" Value="" />
	<asp:HiddenField ID="ocultoNumExpediente" runat="server" Value="" />
	<asp:HiddenField ID="ocultoLigado" runat="server" Value="" />
</asp:Content>
