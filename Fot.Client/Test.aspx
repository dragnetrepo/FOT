<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Fot.Client.Test" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Face Of Testing</title>
    <link href="css/homestyle.css" rel="stylesheet" type="text/css" />
    
      <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    

    <script src="js/jquery.validationEngine-en.js" type="text/javascript"></script>
    <script src="js/jquery.validationEngine.js" type="text/javascript"></script>
    

    <link href="css/validationEngine.jquery.css" rel="stylesheet" />
    


    <script type="text/javascript">
<!--
    function MM_openBrWindow(theURL, winName, features) { //v2.0

        var x = window.screen.width;
        var y = window.screen.height;
        window.open(theURL, winName, features + 'width=' + x + ',height=' + y);
    }
        //-->
    

    function TakeTest(cua) {
        
        //MM_openBrWindow('AppPage.aspx?id=' + cua, 'Assessment', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,');

        var location = "Tests/TakeTest/";

        MM_openBrWindow(location + cua,'Assessment','toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,');

            return false;
    }
    

    $(document).ready(function () {

        $("#Form1").validationEngine();
    });


    </script>

</head>
<body>

    <div id="outer_wrapper_inner" style="left: 0px; top: 0px">
        <form id="Form1" runat="server">
            <div id="wrapper">
                <!--header -->
                <div id="header" style="height: 80px">
                    <img src="images/newlogo2.png" alt="" id="logo" />
                    &nbsp;
                    <img src="images/exam-text.png" alt="" id="logo2" /></div>
                <!--Menu Area -->
                <!--content area -->
                <div id="content">
                    
                     <div style=" width: 700px; height: 40px; margin: auto; padding: 10px; margin-top: 20px; text-align: center">
                         <asp:Literal runat="server" ID="lblMessage">
         <h1 style="line-height: 25px;">Provide your login details to take the online test. Only candidates who have been notified will be able to login.</h1>
                             </asp:Literal>

                     </div>

                    <table cellpadding="5" cellspacing="5" width="100%" border="0">
                        <tr>
                            <td style="width: 40%"></td>
                            <td style="width: 420px; height: 30px;" align="center">

                                <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

                            </td>
                            <td style="width: 40%">&nbsp;</td>
                        </tr>
                        <tr id="trLogin" runat="server">
                            <td>&nbsp;</td>
                            <td style="height: 300px" valign="top">

                                <div class="loginDiv">

                                    <table border="0" cellpadding="2" cellspacing="0"
                                        style="width: 400px;">
                                        <tr>
                                            <td>
                                                <h1 style="padding-bottom: 20px; color: #003C60">Candidate Login</h1>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Username</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtUsername" runat="server" Width="400px" CssClass="validate[required]" data-errormessage-value-missing="Username is required!"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Password</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtPassword" runat="server" Width="400px" TextMode="Password" CssClass="validate[required]" data-errormessage-value-missing="Password is required!"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Button ID="bttnLogin" runat="server" Text="Login"
                                                    OnClick="bttnLogin_Click" />
                                            </td>
                                        </tr>
                                    </table>

                                </div>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr id="trTakeTest" runat="server" visible="False">
                            <td>&nbsp;</td>
                            <td style="height: 300px" valign="top">

                                <div class="loginDiv" style="width: 520px; min-height: 300px;">

                                    <table border="0" cellpadding="2" cellspacing="0"
                                        style="width: 526px; height: 219px;">
                                        <tr>
                                            <td style="height: 40px;">
                                                <h1 style="padding-bottom: 20px; color: #003C60">Your Assessment Details</h1>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 30px;">
                                                <strong>Assessment</strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ListView ID="ListView1" runat="server" ItemPlaceholderID="ItemPlaceHolder" ItemType="Fot.Client.Models.CandidateAssessmentViewModel">
                                                    <LayoutTemplate>
                                                        <table width="100%" cellpadding="2">
                                                            <asp:PlaceHolder ID="ItemPlaceHolder" runat="server"></asp:PlaceHolder>
                                                        </table>
                                                       
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                         <tr>
                                                                <td style="margin-bottom: 10px;"><%# Item.AssessmentName %></td><td style="width: 80px; margin-bottom: 10px;"> <button onclick='TakeTest("<%# Item.CandidateGuid %>")' style="margin-bottom: 3px;">Take Test</button></td>
                                                            </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>

                                            </td>
                                        </tr>

                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>

                                </div>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                           <tr id="trSeb" runat="server" visible="False">
                            <td>&nbsp;</td>
                            <td style="height: 300px" valign="top">

                                <div class="loginDiv" style="width: 820px; min-height: 550px;">

                                    <table border="0" cellpadding="2" cellspacing="0"
                                        style="width: 826px; height: 419px;">
                                        <tr>
                                            <td style="height: 40px;">
                                                <h1 style="padding-bottom: 20px; color: #003C60; font-size: 30px;">Assessment Instructions</h1>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 30px; font-size: 20px; line-height: 2; margin-bottom: 20px;">
                                                This assessment requires a <strong>Secure Web Browser</strong> <br />
                                                To complete this assessment, you must meet the following preconditions: 
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                               
                                                <p style="font-size: 20px; line-height: 2; margin-top: 40px;">Windows 10 (or above) is required. Please download and install the file below if you have not already done so. <br />

                                                    <strong>Safe Exam Browser: </strong> Download and install <a href="https://github.com/SafeExamBrowser/seb-win/releases/download/v2.4.1/SafeExamBrowserInstaller.exe">Safe Exam Browser</a> <br />
                                                  
                                                </p>
                                                <p style="font-size: 20px; line-height: 2; margin-top: 30px; ">
                                                    Be sure to download the file above. INSTALL by clicking on the downloaded file and approving all prompts during the installation process.

                                                </p>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 20px; margin-top: 60px; line-height: 2;">
                                                Click <a id="linkUrl" href="#" runat="server"><strong>here</strong></a> to launch the assessment after you have downloaded and installed <strong>Safe Exam Browser</strong>. Ensure you have your password as this will be required once the assessment is launched. 
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>

                                </div>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>

                </div>
            </div>
            <!--footer -->

            <br class="clear" />
        </form>
    </div>

</body>
</html>
