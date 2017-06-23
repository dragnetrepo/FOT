<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageLocations.aspx.cs" Inherits="Fot.Admin.ManageLocations" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    
     <script type="text/javascript">         

         $(document).ready(function() {

             $("#bttnAddLocation").click(function() {

                 AddLocation();
                 return false;
             });
             
             $("#bttnAddMapping").click(function () {

                 AddMapping();
                 return false;
             });

             $("#bttnAddLocation").hide();

         });

         function AddLocation() {
             var oWnd = radopen("Dialogs/AddOrEditLocation.aspx", "RadWindow1");
             oWnd.center();

         }

         function AddMapping() {
             var oWnd = radopen("Dialogs/AddOrEditMapping.aspx", "RadWindow1");
             oWnd.center();

         }

         function UpdateLocation(ID) {
             var oWnd = radopen("Dialogs/AddOrEditLocation.aspx?id=" + ID, "RadWindow1");
             oWnd.center();

         }

         function UpdateMapping(ID) {
             var oWnd = radopen("Dialogs/AddOrEditMapping.aspx?id=" + ID, "RadWindow1");
             oWnd.center();

         }


         function refreshGrid() {

             var radMgr = $find("<%=RadAjaxManager1.ClientID %>");

             radMgr.ajaxRequest("Rebind");
         }
         
     
     </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Manage Locations</h1>
        <div style="text-align: center; padding: 0px; margin: 0px; min-height: 20px;">

        <asp:Literal ID="lblStatus" runat="server"></asp:Literal>

    </div>

    <div style="padding: 0px; margin-top: 30px;">
        
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="0" Skin="Windows7" Width="100%">
            <Tabs>
                <telerik:RadTab runat="server" Selected="True" Text="Locations">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Location Mapping">
                </telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="RadMultiPage1" Runat="server" SelectedIndex="0" Width="100%">
            <telerik:RadPageView ID="RadPageView1" runat="server">
               <div style="padding: 0px; margin-top: 20px;">
                   <button id="bttnAddLocation">Add Location</button>
                   

                   <telerik:RadGrid ID="LocationGrid" runat="server" AllowAutomaticDeletes="True" Skin="Windows7" AllowPaging="True" AutoGenerateColumns="False" CellSpacing="0" DataSourceID="LocationDatasource" GridLines="None" ItemType="Fot.Admin.Models.Location">
                       <ClientSettings>
                           <Selecting AllowRowSelect="True" />
                       </ClientSettings>
                       <MasterTableView DataSourceID="LocationDatasource" DataKeyNames="LocationId">
                           <CommandItemSettings ExportToPdfText="Export to PDF" />
                           <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                               <HeaderStyle Width="20px" />
                           </RowIndicatorColumn>
                           <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                               <HeaderStyle Width="20px" />
                           </ExpandCollapseColumn>
                           <Columns>
                               <telerik:GridBoundColumn DataField="LocationId" DataType="System.Int32" Display="False" FilterControlAltText="Filter LocationId column" HeaderText="LocationId" SortExpression="LocationId" UniqueName="LocationId">
                               </telerik:GridBoundColumn>
                               <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Filter LocationName column" HeaderText="Location" SortExpression="LocationName" UniqueName="LocationName">
                               </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column"
                                    UniqueName="TemplateColumn" Visible="False">
                                    <HeaderStyle Width="80px" />
                                    <ItemStyle Width="80px" />
                                    <ItemTemplate>
                                        <a href="#" onclick='UpdateLocation(<%# Eval("LocationId") %>); return false;'>Edit / Update</a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                               <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" ConfirmText="Delete this location?" FilterControlAltText="Filter column column" Text="Delete" UniqueName="column" Visible="False">
                                   <HeaderStyle Width="60px" />
                                   <ItemStyle Width="60px" />
                               </telerik:GridButtonColumn>
                           </Columns>
                           <EditFormSettings>
                               <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                               </EditColumn>
                           </EditFormSettings>
                       </MasterTableView>
                       <HeaderStyle Font-Bold="True" />
                       <FilterMenu EnableImageSprites="False">
                       </FilterMenu>
                   </telerik:RadGrid>
                   

               </div>
            </telerik:RadPageView>
             <telerik:RadPageView ID="RadPageView2" runat="server">
               <div style="padding: 0px; margin-top: 20px;">
                    <button id="bttnAddMapping">Add Mapping</button>
                   
                   

                    <telerik:RadGrid ID="MappingGrid" runat="server" AllowAutomaticDeletes="True" Skin="Windows7" AllowPaging="True" AutoGenerateColumns="False" CellSpacing="0" DataSourceID="MappingDataSource" GridLines="None">
                        <ClientSettings>
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                        <MasterTableView DataSourceID="MappingDataSource" DataKeyNames="LocationId">
                            <CommandItemSettings ExportToPdfText="Export to PDF" />
                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="LocationId" DataType="System.Int32" Display="False" FilterControlAltText="Filter LocationId column" HeaderText="LocationId" SortExpression="LocationId" UniqueName="LocationId">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Filter LocationName column" HeaderText="Location" SortExpression="LocationName" UniqueName="LocationName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MappedToLocation" DataType="System.String" FilterControlAltText="Filter MappedToLocation column" HeaderText="Mapped To" SortExpression="MappedToLocation" UniqueName="MappedToLocation">
                                </telerik:GridBoundColumn>
                                      
                               <telerik:GridButtonColumn CommandName="Delete" ConfirmDialogType="RadWindow" ConfirmText="Remove this mapping?" FilterControlAltText="Filter column column" Text="Remove" UniqueName="column">
                                   <HeaderStyle Width="60px" />
                                   <ItemStyle Width="60px" />
                               </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                            </EditFormSettings>
                        </MasterTableView>
                        <HeaderStyle Font-Bold="True" />
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </telerik:RadGrid>
                   
                   

               </div>
            </telerik:RadPageView>
        </telerik:RadMultiPage>
        
    </div>
  
    <div>
          <asp:ObjectDataSource ID="LocationDatasource" runat="server" DeleteMethod="Delete" SelectMethod="GetLocations" TypeName="Fot.Admin.Services.LocationService" MaximumRowsParameterName="maxRows" SelectCountMethod="Count" StartRowIndexParameterName="startRow" EnablePaging="True">
              <DeleteParameters>
                  <asp:Parameter Name="LocationId" Type="Int32" />
              </DeleteParameters>
          </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="MappingDataSource" runat="server" DeleteMethod="DeleteMapping" SelectMethod="GetMappedLocations" TypeName="Fot.Admin.Services.LocationService" MaximumRowsParameterName="maxRows" SelectCountMethod="CountMappedLocations" StartRowIndexParameterName="startRow" EnablePaging="True">
        <DeleteParameters>
            <asp:Parameter Name="LocationId" Type="Int32" />
        </DeleteParameters>
          </asp:ObjectDataSource>
    <telerik:RadWindowManager
        ID="RadWindowManager1" runat="server" Height="260px" Skin="Windows7"
        Width="650px" Modal="True" VisibleStatusbar="False"
        Behaviors="Close, Move, Reload" DestroyOnClose="True">
    </telerik:RadWindowManager>
          <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
              <AjaxSettings>
                  <telerik:AjaxSetting AjaxControlID="LocationGrid">
                      <UpdatedControls>
                          <telerik:AjaxUpdatedControl ControlID="LocationGrid" />
                      </UpdatedControls>
                  </telerik:AjaxSetting>
                  <telerik:AjaxSetting AjaxControlID="MappingGrid">
                      <UpdatedControls>
                          <telerik:AjaxUpdatedControl ControlID="MappingGrid" />
                      </UpdatedControls>
                  </telerik:AjaxSetting>
                  <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                      <UpdatedControls>
                          <telerik:AjaxUpdatedControl ControlID="LocationGrid" />
                          <telerik:AjaxUpdatedControl ControlID="MappingGrid" />
                      </UpdatedControls>
                  </telerik:AjaxSetting>
              </AjaxSettings>
          </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="White" Height="100%" Skin="Default" Transparency="30" Width="100%">
    </telerik:RadAjaxLoadingPanel>

    </div>
</asp:Content>
