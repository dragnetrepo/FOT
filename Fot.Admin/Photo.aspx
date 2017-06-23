<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Photo.aspx.cs" Inherits="Fot.Admin.Photo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <table style="width: 100%;">
                <tr>

                    <td>
                        <asp:Image ID="imgPhoto" runat="server" /></td>
                </tr>
                <tr>
                    <td>
                        <div style="font-weight: bold; float: left; margin-right: 10px;">Captured By:</div>
                        <div style="float: left;">
                            <asp:Literal ID="lblAdmin" runat="server"></asp:Literal>
                        </div>
                    </td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>
