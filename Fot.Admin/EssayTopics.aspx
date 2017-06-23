<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EssayTopics.aspx.cs" Inherits="Fot.Admin.EssayTopics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
      <script type="text/javascript">

          function AddTopic() {
              var oWnd = radopen("Dialogs/AddOrEditEssayTopic.aspx?aid=<%=hidId.Value%>", "RadWindow1");
            oWnd.center();

        }

      

            function UpdateTopic(ID) {
                var oWnd = radopen("Dialogs/AddOrEditEssayTopic.aspx?tid=" + ID, "RadWindow1");
                oWnd.center();

            }



            function refreshGrid() {

                var radMgr = $find("<%=RadAjaxManager1.ClientID %>");

                           radMgr.ajaxRequest("Rebind");
            }


            $(document).ready(function() {

                $("#bttnAddTopic").click(function() {

                    AddTopic();
                    return false;

                });

            });
            
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Essay Topics</h1>

    <div style="padding: 10px; border: 1px solid #f0f0f0; height: 30px; margin: 10px; margin-left: 0px; margin-right: 0px; font-size: 16px;">
        <div style="float: left; padding-right: 10px;">
            <strong>Assessment Name:</strong>
        </div>
        <div style="float: left">
            <asp:Literal ID="lblAssessmentName" runat="server"></asp:Literal>
        </div>
    </div>


    <div style="padding: 0px; padding-bottom: 0px; margin-top: 0px;">
        <button id="bttnAddTopic" >Add Topic</button>


        <asp:HiddenField ID="hidId" runat="server" />


    </div>

    <div style="padding: 0px; padding-bottom: 0px; margin-top: 0px;">
        <telerik:RadGrid ID="RadGrid1" runat="server" AllowAutomaticDeletes="True" AllowPaging="True" AutoGenerateColumns="False" CellSpacing="0" DataSourceID="EssayTopicsDataSource" GridLines="None" Skin="Windows7">
            <ClientSettings>
                <Selecting AllowRowSelect="True" />
            </ClientSettings>
            <MasterTableView DataSourceID="EssayTopicsDataSource" DataKeyNames="EssayId">
                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    <HeaderStyle Width="20px"></HeaderStyle>
                </RowIndicatorColumn>

                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    <HeaderStyle Width="20px"></HeaderStyle>
                </ExpandCollapseColumn>

                <Columns>
                    <telerik:GridBoundColumn DataField="EssayId" DataType="System.Int32" Display="False" FilterControlAltText="Filter EssayId column" HeaderText="EssayId" SortExpression="EssayId" UniqueName="EssayId">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Topic" FilterControlAltText="Filter Topic column" HeaderText="Essay Topic" SortExpression="Topic" UniqueName="Topic">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column"
                        UniqueName="TemplateColumn">
                        <HeaderStyle Width="80px" />
                        <ItemStyle Width="80px" />
                        <ItemTemplate>
                            <a href="#" onclick='UpdateTopic(<%# Eval("EssayId") %>)'>Edit / Update</a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridButtonColumn CommandName="Delete"
                        ConfirmText="Delete this topic?" FilterControlAltText="Filter column column"
                        Text="Delete" UniqueName="column" ConfirmDialogType="RadWindow">
                        <HeaderStyle Width="60px" />
                        <ItemStyle Width="60px" />
                    </telerik:GridButtonColumn>
                </Columns>

                <EditFormSettings>
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                </EditFormSettings>
            </MasterTableView>

            <HeaderStyle Font-Bold="True" />

            <PagerStyle Mode="Slider" />

            <FilterMenu EnableImageSprites="False"></FilterMenu>
        </telerik:RadGrid>

    </div>

    <div>

        <asp:ObjectDataSource ID="EssayTopicsDataSource" runat="server" DeleteMethod="Delete" SelectMethod="GetTopics" TypeName="Fot.Admin.Services.EssayTopicService" SelectCountMethod="Count" MaximumRowsParameterName="maxRows" StartRowIndexParameterName="startRow" EnablePaging="True">
            <DeleteParameters>
                <asp:Parameter Name="EssayId" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="hidId" Name="AssessmentId" PropertyName="Value" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    <telerik:RadWindowManager
        ID="RadWindowManager1" runat="server" Height="360px" Skin="Windows7"
        Width="650px" Modal="True" VisibleStatusbar="False"
        Behaviors="Close, Move, Reload" DestroyOnClose="True">
    </telerik:RadWindowManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
   
    </telerik:RadAjaxManager>
    </div>
</asp:Content>
