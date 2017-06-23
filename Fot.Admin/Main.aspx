<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Fot.Admin.Main" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Overview / Stats</h1>


    <div style="padding: 20px;">
        <div style="font-size: 18px; font-weight: bold; color: #999; margin-top: 30px;">Assessments Stats</div>
        <div style="padding: 0px; margin-top: 5px; min-height: 200px; border: 1px solid #ccc;">


            <div style="padding: 10px;">

                <table style="width: 100%;">
                    <tr>
                        <td style="width: 150px"><b>Essays</b></td>
                        <td>
                            <asp:Label ID="lblEssays" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>MCQs</b></td>
                        <td>
                            <asp:Label ID="lblMcq" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b></b></td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td><b>Assessments</b></td>
                        <td>
                            <asp:Label ID="lblAssessments" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Questions</b></td>
                        <td>
                            <asp:Label ID="lblQuestions" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Options</b></td>
                        <td>
                            <asp:Label ID="lblOptions" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>

            </div>

        </div>

        <div style="font-size: 18px; font-weight: bold; color: #999; margin-top: 30px;">Administrators / Users Stats
        </div>
        <div style="padding: 0px; margin-top: 5px; min-height: 200px; border: 1px solid #ccc;">


            <div style="padding: 10px;">

                <table style="width: 100%;">
                    <tr>
                        <td style="width: 150px"><b>Administrators</b></td>
                        <td>
                            <asp:Label ID="lblAdministrators" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Candidates</b></td>
                        <td>
                            <asp:Label ID="lblCandidates" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td><b>Regular Admins</b></td>
                        <td>
                            <asp:Label ID="lblRegularAdmins" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Partner Admins</b></td>
                        <td>
                            <asp:Label ID="lblPartnerAdmins" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Center Admins</b></td>
                        <td>
                            <asp:Label ID="lblCenterAdmins" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>

            </div>

        </div>

        <div style="font-size: 18px; font-weight: bold; color: #999; margin-top: 30px;">
            Other Stats
        </div>
        <div style="padding: 0px; margin-top: 5px; min-height: 200px; border: 1px solid #ccc;">


            <div style="padding: 10px;">

                <table style="width: 100%;">
                    <tr>
                        <td style="width: 150px"><b>Partners</b></td>
                        <td>
                            <asp:Label ID="lblPartners" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Campaigns</b></td>
                        <td>
                            <asp:Label ID="lblCampaigns" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><strong>Centers</strong></td>
                        <td>
                            <asp:Label ID="lblCenters" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>

            </div>

        </div>


    </div>

</asp:Content>
