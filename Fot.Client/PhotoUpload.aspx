<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PhotoUpload.aspx.cs" Inherits="Fot.Client.PhotoUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
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
    </style>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Photo Upload</h1>
    <div style="margin-top: 20px;">
        
        <div style="width: 750px; min-height: 150px; margin-right: auto; margin-left: auto; border: 1px solid #ccc; padding: 10px;" id="divPhoto" runat="server">
            <div>
                <h3 style="color: #666; font-family: Arial; font-size: 18px;">Upload your photograph before printing your invitation</h3>
                <p class="normalText">
                    The photograph should be in colour and of the size of 450px by 450px.

                    The photo-print should be clear and with a continuous-tone quality.

                    It should have full face, front view, eyes open.<br />

                    Photo should present full head from top of hair to bottom of chin.

                    The background should be a plain light colored background.
                    <br />
                </p>
            </div>
            <div>
                <table style="width: 100%;" class="mytable">
                    <tr>
                        <td style="width: 180px;">Select File</td>
                        <td>
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </td>
                        <td style="width: 80px;">
                            <asp:Button ID="bttnUpload" runat="server" Text="Upload" OnClick="bttnUpload_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="text-align: center; padding: 5px;">
                <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
            </div>
        </div>
        
        
         <div style="width: 750px; min-height: 150px; margin-right: auto; margin-left: auto;" id="divPhotoUploaded" runat="server">
             
            <asp:Image ID="img" runat="server" Width="250px" />

        </div>

    </div>
</asp:Content>
