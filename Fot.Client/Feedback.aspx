<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Feedback.aspx.cs" Inherits="Fot.Client.Feedback" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
      
      td {
          padding: 5px;
      }

  </style>
    
    
    <script type="text/javascript">

        $(function() {


            $("#listFeedbackType").change(function() {

                ShowHideOther();

            });


        });
        

        function ShowHideOther() {

            var str = $("#listFeedbackType option:selected").val();

            if (str === "0") {

                $("#trOther").show();
               

            }
            else {

                $("#trOther").hide();
            }

        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Feedback</h1>
    <div style="height: 30px; text-align: center">
         <asp:Literal ID="lblStatus" runat="server"></asp:Literal>
    </div>
    
    <div style="margin-top: 10px;">
        <table style="width: 100%;" id="tableFeedback" runat="server">
            <tr>
                <td style="width: 150px;">Candidate</td>
                <td>
                    <asp:Label ID="lblCandidateName" runat="server" style="font-weight: 700"></asp:Label>
                </td>
            </tr>
            <tr>
                <td >Feedback Type</td>
                <td >
                    <asp:DropDownList ID="listFeedbackType" ClientIDMode="Static" runat="server" style="margin-bottom: 3px;" AppendDataBoundItems="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trOther" runat="server" ClientIDMode="Static">
                <td >Please Specify</td>
                <td >
                    <asp:TextBox ID="txtOther" runat="server" Width="600px" style="margin-bottom: 3px;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">Feedback / Enquiry</td>
                <td >
                            <telerik:radeditor ID="editor" Runat="server" EditModes="Design" 
                                Skin="Windows7" Height="190px" Width="613px">
                                       <ImageManager DeletePaths="~/img" UploadPaths="~/img" ViewPaths="~/img" />
            <Tools>
                <telerik:EditorToolGroup>
                    <telerik:EditorTool Name="Cut" />
                    <telerik:EditorTool Name="Copy" />
                    <telerik:EditorTool Name="Bold" />
                    <telerik:EditorTool Name="Underline" />
                    <telerik:EditorTool Name="Italic" />
                    <telerik:EditorTool Name="InsertSymbol" />
                    <telerik:EditorTool Name="InsertUnorderedList" />
                    <telerik:EditorTool Name="InsertOrderedList" />
                    <telerik:EditorTool Name="JustifyCenter" />
                    <telerik:EditorTool Name="JustifyFull" />
                    <telerik:EditorTool Name="JustifyLeft" />
                    <telerik:EditorTool Name="JustifyRight" />
                    <telerik:EditorTool Name="Superscript" />
                    <telerik:EditorTool Name="Subscript" />
                    <telerik:EditorTool Name="Indent" />
                </telerik:EditorToolGroup>
            </Tools>
                                <Content>
</Content>
                            </telerik:radeditor>
                        </td>
            </tr>
            <tr>
                <td >&nbsp;</td>
                <td >&nbsp;</td>
            </tr>
            <tr>
                <td >&nbsp;</td>
                <td >
                    <asp:Button ID="bttnSend" runat="server" Text="Send Feedback" OnClick="bttnSend_Click" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>

                    <asp:HiddenField ID="hidCandidateId" runat="server" />

                </td>
            </tr>
        </table>
    </div>
</asp:Content>
