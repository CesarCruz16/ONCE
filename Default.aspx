<%@ Page Title="Gestion11" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="_Default" Codebehind="Default.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
        <table width="100%" border="0" cellspacing="0" height="100%">
            <tr>
                <td height="100%" width="100%" align="center" valign="middle">
                    <table cellpadding="0" cellspacing="0" border="0" align="center">
                        <tr>
                            <td colspan="2">Usuario y contraseña</td>
                        </tr>
                        <tr>
                            <td>Usuario:</td>
                            <td><asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Password:</td>
                            <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2"><asp:Button ID="btnBuscaUsuario" runat="server" Text="Ingresar" OnClick="btnBuscaUsuario_Click" /></td>
                        </tr>
                    </table>
                    <asp:Label ID="lblGeneral" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
