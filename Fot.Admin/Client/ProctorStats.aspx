<%@ Page Title="" Language="C#" MasterPageFile="PartnerMaster.Master" AutoEventWireup="true" CodeBehind="ProctorStats.aspx.cs" Inherits="Fot.Admin.Client.ProctorStats" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Proctor Stats</h1>

    <div style="padding: 0px; margin-top: 20px; font-size: 14px; width: 100%; float: left;">

        <table style="width: 100%; border: 1px solid #f0f0f0; margin-bottom: 5px;">

            <tr>
                <td style="padding-left: 3px; color: #666; width: 130px;"><strong>Candidate Name</strong></td>
                <td>

                    <div style="padding: 16px; float: left;">
                        <asp:Literal ID="lblCandidateName" runat="server"></asp:Literal>
                    </div>
                    <div style="float: right; padding: 0px; padding-right: 5px; padding-top: 2px;">
                        <asp:Button ID="bttnBackToResults" runat="server" Text="Return To Results Page" CssClass="myButton2" OnClick="bttnBackToResults_Click" />
                    </div>
                </td>

            </tr>

        </table>

    </div>


    <div style="padding: 0px; margin-top: 20px; font-size: 14px; width: 100%; float: left;">

        <asp:Literal ID="lblFrame" runat="server"></asp:Literal>
    </div>

    <div>
        <asp:HiddenField ID="hidId" runat="server" />
    </div>
</asp:Content>
