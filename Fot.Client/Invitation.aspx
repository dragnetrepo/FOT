<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Invitation.aspx.cs" Inherits="Fot.Client.Invitation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test Invitation</title>

    <style type="text/css">
        .header {
            font-family: Arial;
            color: #666;
            font-size: 20pt;
        }



        .mytable td {
            font-family: Arial;
            color: #666;
            font-size: 12pt;
            padding-bottom: 10px;
            padding-top: 10px;
        }

        .normalText {
            font-family: Arial;
            color: #666;
            font-size: 12pt;
            line-height: 22px;
        }

        @media only print {
            
            #bttnPDF {
                display: none;
            }

        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 750px; min-height: 150px; margin-right: auto; margin-left: auto; border: 1px solid #ccc; padding: 10px;" id="divResponse" runat="server">
            <table style="width: 100%;" class="mytable">
                <tr>
                    <td style="font-size: 20px;">Please accept or reject invitation</td>
                </tr>
                <tr>
                    <td>
                        <div style="float: left">Yes I will be available for the test on the specified date.</div>
                        <div style="float: right">
                            <asp:Button ID="bttnAccept" runat="server" Text="Accept" Width="80px" OnClick="bttnAccept_Click" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="float: left; margin-top: 18px;">No I will not be available for the test.</div>
                        <div style="float: right;">
                            <table style="width: 100%;" class="mytable">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Reason:" Style="font-weight: 700"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtReason" runat="server" Width="300" ></asp:TextBox></td>
                                    <td>
                                        <asp:Button ID="bttnReject" runat="server" Text="Reject" Width="80px" OnClick="bttnReject_Click" /></td>
                                </tr>
                            </table>

                        </div>
                    </td>
                </tr>

            </table>

        </div>

        <div style="width: 750px; min-height: 150px; margin-right: auto; margin-left: auto; border: 1px solid #ccc; padding: 10px;" id="divPhoto" runat="server">
            <div>
                <h3 style="color: #666; font-family: Arial;">Upload your photograph before printing your invitation</h3>
            </div>
        </div>
        
        <div runat="server" ID="divPDF" style="width: 750px;margin-right: auto; margin-left: auto; padding: 10px; padding-left: 0;" Visible="False">
            <asp:Button ID="bttnPDF" runat="server" Text="Download As PDF" Height="33px" Width="150px" ClientIDMode="Static" OnClick="bttnPDF_Click" />
        </div>

        <div style="width: 750px; min-height: 650px; margin-right: auto; margin-left: auto; border: 1px solid #ccc; padding: 10px;" id="divInvitation" runat="server">
            <table style="width: 100%;">
                <tr>
                    <td class="header" style="border-bottom: 1px solid #ccc; height: 90px;">
                        <div style="float:left">
                            <asp:Image ID="imgLeft" runat="server" ImageUrl="~/photos/dragnet_logo.png" />
                        </div>
                        <div style="float: right;">
                            <asp:Image ID="ImgRight" runat="server" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="header" style="border-bottom: 1px solid #ccc;">Test Invitation</td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 100%;" class="mytable">
                            <tr>
                                <td style="width: 180px;"><b>Full Name</b></td>
                                <td>
                                    <asp:Label ID="lblFullName" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px;"><b>Username</b></td>
                                <td>
                                    <asp:Label ID="lblUsername" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px;"><b>Password</b></td>
                                <td>
                                    <asp:Label ID="lblPassword" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px;"><b>Exam Date / Time</b></td>
                                <td>
                                    <asp:Label ID="lblSessionDateTime" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b>Test Center Name</b></td>
                                <td>
                                    <asp:Label ID="lblCenterName" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top;"><b>Test Center Address</b></td>
                                <td>
                                    <asp:Label ID="lblCenterAddress" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top;"><b>Location</b></td>
                                <td>
                                    <asp:Label ID="lblLocation" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Image ID="img" runat="server" Width="250px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="header" style="border-bottom: 1px solid #ccc;">Instructions / Requirements</td>
                </tr>
                <tr class="mytable">
                    <td>
                        <asp:Literal ID="lblInstructions" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hidId" runat="server" />
    </form>
</body>
</html>
