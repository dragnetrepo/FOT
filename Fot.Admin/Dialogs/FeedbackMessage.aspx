<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FeedbackMessage.aspx.cs" Inherits="Fot.Admin.Dialogs.FeedbackMessage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Feedback Message</title>

    <link href="../css/dialog.css" rel="stylesheet" />
    <style type="text/css">
        td
        {
            padding-top: 5px;
            padding-bottom: 5px;
        }
    </style>



</head>
<body>
    <form id="form1" runat="server">
        <div style="padding: 10px;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 150px;">Candidate</td>
                    <td>
                        <asp:Label ID="lblCandidate" runat="server" Style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Subject</td>
                    <td>
                        <asp:Label ID="lblSubject" runat="server" Style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">Date</td>
                    <td>
                        <asp:Label ID="lblDate" runat="server" Style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px; vertical-align: top;">Message</td>
                    <td>
                        <div style="width: 500px; height: 200px; overflow: auto; border: 1px solid #ccc; padding: 5px;">
                            <asp:Literal ID="lblMessage" runat="server"></asp:Literal></div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>

    </form>
</body>
</html>
