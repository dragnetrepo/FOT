<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Fot.Admin.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <h1 class="PageHeader">
        Change Password</h1>
    <div class="contentDiv" style="padding: 10px;" align="center">
        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
    </div>
    <div style="padding: 10px; float:left;min-width:98%">
        <div class="formContainer">
            <div class="divFormlabel">
                Old Password</div>
            <div class="divFormInput">
                <asp:TextBox ID="txtOldPassword" runat="server" Width="250px" 
                    TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ErrorMessage="Enter old password" ControlToValidate="txtOldPassword" 
                    CssClass="Formerror"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="formContainer">
            <div class="divFormlabel">
                New Password</div>
            <div class="divFormInput">
                <asp:TextBox ID="txtNewPassword" runat="server" Width="250px" 
                    TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ErrorMessage="Enter new password" ControlToValidate="txtNewPassword" 
                    CssClass="Formerror"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="formContainer">
            <div class="divFormlabel">
                Re-type New Password</div>
            <div class="divFormInput">
                
                <asp:TextBox ID="txtConfirm" runat="server" Width="250px" TextMode="Password"></asp:TextBox>
           <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ErrorMessage="Passwords do not match" ControlToCompare="txtConfirm" 
                    ControlToValidate="txtNewPassword" CssClass="Formerror"></asp:CompareValidator> </div>
        </div>
        <div class="formContainer">
            <div class="divFormlabel">
                &nbsp;
            </div>
            <div class="divFormInput">
                <asp:Button ID="bttnUpdate" runat="server" Text="Update" 
                    onclick="bttnUpdate_Click" />
            </div>
        </div>
    </div>
</asp:Content>
