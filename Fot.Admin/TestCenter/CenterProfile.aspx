<%@ Page Title="" Language="C#" MasterPageFile="~/TestCenter/CenterMaster.Master" AutoEventWireup="true" CodeBehind="CenterProfile.aspx.cs" Inherits="Fot.Admin.TestCenter.CenterProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

    <script src="../js/jquery.validationEngine-en.js" type="text/javascript"></script>
    <script src="../js/jquery.validationEngine.js" type="text/javascript"></script>


    <link href="../css/validationEngine.jquery.css" rel="stylesheet" />



    <script type="text/javascript">


        $(document).ready(function () {

            $("#form1").validationEngine();
        });


    </script>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Center Profile</h1>
    
            <div style="padding: 10px; text-align: center; height: 30px;">
            <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
        </div>
    
    
    <div style="padding: 10px;">
        
        

            <table style="width: 100%;">
                <tr>
                    <td style="width: 150px;">Center Name</td>
                    <td>
                        <asp:TextBox ID="txtCenterName" runat="server" Width="400px" CssClass="validate[required]" data-errormessage-value-missing="Center Name is required!" data-prompt-position="centerRight"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px; vertical-align: top;">Address</td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" Width="400px" Height="30px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Location</td>
                    <td>
                        <asp:Label ID="lblLocation" runat="server" style="font-weight: 700"></asp:Label>
        
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Capacity</td>
                    <td>
                        <div style="float: left; margin-right: 50px;">
                            <asp:TextBox ID="txtCapacity" runat="server" Width="60px" CssClass="validate[required,custom[integer]]" data-errormessage-value-missing="Center capacity is required!" data-prompt-position="centerRight"></asp:TextBox>

                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Rate Per Tested</td>
                    <td>
                        <asp:Label ID="lblRate" runat="server" style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="bttnUpdate" runat="server" Text="Update" OnClick="bttnUpdate_Click" />
                        <asp:HiddenField ID="hidId" runat="server" />
                    </td>
                </tr>
            </table>
        
        

    </div>
</asp:Content>
