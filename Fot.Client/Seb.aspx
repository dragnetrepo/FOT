<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Seb.aspx.cs" Inherits="Fot.Client.Seb" %>

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
                    
                    <table cellpadding="5" cellspacing="5" width="100%" border="0">
                        <tr>
                            <td style="width: 40%"></td>
                            <td style="width: 420px; height: 30px;" align="center">

                                <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

                            </td>
                            <td style="width: 40%">&nbsp;</td>
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
                                                                <td style="margin-bottom: 10px;"><%# Item.AssessmentName %></td><td style="width: 80px; margin-bottom: 10px;"> <Button id="bttnTest" style="margin-bottom: 3px;" runat="server" onserverclick="bttnTest_ServerClick">Take Test</Button></td>
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
                    </table>

                </div>
            </div>
            <!--footer -->

            <br class="clear" />
        </form>
    </div>

</body>
</html>
