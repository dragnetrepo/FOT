<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportAssessment.aspx.cs" Inherits="Fot.Admin.ImportAssessment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <h1>Import Assessment</h1>
    <div style="text-align: center; padding: 0px; margin: 0px; min-height: 20px;">

        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

    </div>
    <div style="padding-left: 20px;">

        <table style="width: 100%;">
            <tr>
                <td style="width: 160px;">Assessment File</td>
                <td>
                    <asp:FileUpload ID="byteFile" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 160px;">&nbsp;</td>
                <td>
                    <asp:Button ID="bttnImport" runat="server" OnClick="bttnImport_Click" Text="Upload / Import" />
                </td>
            </tr>
            <tr>
                <td style="width: 160px;">&nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            </table>
        </div>

</asp:Content>
