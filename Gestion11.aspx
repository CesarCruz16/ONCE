<%@ Page Title="Gestion11" Language="C#" MasterPageFile="~/header.master" AutoEventWireup="true" Inherits="Gestion11" Codebehind="Gestion11.aspx.cs" %>

<%@ Register TagPrefix="ajx" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>

<%@ Register src="usrControl/ventanaSuperior.ascx" tagname="ventanaSuperior" tagprefix="uc1" %>
<%@ Register src="usrControl/ventanaInferior.ascx" tagname="ventanaInferior" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<ajx:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajx:ToolkitScriptManager>
<table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
    <tr>
        <td heigth="100%" align="center" valign="middle">
            <asp:Image ID="imgBienvenido" runat="server" ImageUrl="~/images/bienvenidos.jpg" />
        </td>
    </tr>
</table>
<br /><br />
<asp:Panel ID="pnlMensajes" runat="server" Visible="false">
    <table cellpadding="0" cellspacing="0" border="0" align="center">
        <tr>
            <td>
                <asp:GridView ID="gridMensajes" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" ForeColor="#333333" GridLines="None" 
                    DataKeyNames="id_mensaje" 
                    onselectedindexchanged="gridMensajes_SelectedIndexChanged" 
                    onrowdatabound="gridMensajes_RowDataBound">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="prioridad" HeaderText="Prioridad" />
                        <asp:BoundField DataField="titulo" HeaderText="Título" />
                        <asp:BoundField DataField="fch_envio" HeaderText="Fecha envío" />
                        <asp:BoundField DataField="nombre" HeaderText="Persona envía" />
                        <asp:BoundField DataField="leido" HeaderText="Leido">
                        <ItemStyle Font-Bold="False" />
                        </asp:BoundField>
                        <asp:CommandField ButtonType="Button" SelectText="Detalles" 
                            ShowSelectButton="True" />
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
<!--VENTANA ENVIA MENSAJES-->
<asp:Panel ID="MnsjPnlVentana" runat="server" BackColor="White">
    <uc1:ventanaSuperior ID="ventanaSuperior1" runat="server" />
    <table cellpadding="0" cellspacing="0" border="0" align="center" width="300">
        <tr>
            <td>De:
            </td>
            <td><asp:Label ID="lblDe" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td>
                Título:
            </td>
            <td>
                <asp:Label ID="lblTitulo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Expediente:</td>
            <td><asp:Label ID="lblExpediente" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td>Prioridad:</td>
            <td><asp:Label ID="lblPrioridad" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="2">
            Mensaje
            <hr />
            <asp:Panel ID="pnlMuestraMnsj" runat="server" Height="150px" ScrollBars="Vertical">
                <asp:Label ID="lblMensaje" runat="server"></asp:Label>
            </asp:Panel>
            </td>
        </tr>
    </table>
    <uc2:ventanaInferior ID="ventanaInferior1" runat="server" />
</asp:Panel>
<asp:LinkButton ID="MnsjLnk" runat="server"></asp:LinkButton>
<ajx:ModalPopupExtender ID="MnsjModal" runat="server" 
     BackgroundCssClass="FondoAplicacion" BehaviorID="MnsjModal" 
     DropShadow="true" PopupControlID="MnsjPnlVentana" TargetControlID="MnsjLnk">
</ajx:ModalPopupExtender>
<!--//VENTANA ENVIA MENSAJES-->
</asp:Content>