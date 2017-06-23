<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Site.Master" AutoEventWireup="true" CodeBehind="Expunge.aspx.cs" Inherits="Fot.Lan.admin.Expunge" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Photo Expunge</h1>
    
       <div style="width: 100%; float: right; margin-top: 30px;">
             <table style="width: 100%; border: 1px solid #f0f0f0; margin-bottom: 5px;">
        
            <tr>
                <td style="width: 180px; padding-left: 3px;">Search By Username</td>
                <td>
                    <asp:TextBox ID="txtSearch" runat="server" Width="335px" style="margin-right: 5px;"></asp:TextBox>
                    <asp:Button ID="bttnSearch" runat="server" Text="Search"  style="margin-bottom: 1px" OnClick="bttnSearch_Click"   />
                </td>
               
            </tr>
        
        </table>
    </div>
    
    <div style="height: 50px; width: 600px; float: left;">
         <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
    </div>
    

    <div style="width: 100%; float: right; margin-top: 10px;" runat="server" ID="divPhoto" Visible="False">
        
        <table style="width: 400px; height: 300px; vertical-align: middle; text-align: center; border: 1px solid #f0f0f0; margin-bottom: 5px;">
        
            <tr><td>
                    
                <asp:Image ID="img" runat="server" />
                

                </td>
               
                
                </tr>
            </table>
         <div style="height: 40px; width:400px; float: left; text-align: right;">
                    <asp:HiddenField ID="hidId" runat="server" />
                    <asp:Button ID="bttnDelete" runat="server" Text="Delete Photo"  style="margin-bottom: 1px" OnClick="bttnDelete_Click"   />
                </div>
   </div>
</asp:Content>
