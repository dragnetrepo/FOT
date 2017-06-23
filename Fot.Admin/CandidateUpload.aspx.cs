using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Context;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;
using OfficeOpenXml;

namespace Fot.Admin
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Admin)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Schedule)]
    public partial class CandidateUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadForAdd(id);
                }
                else
                {
                    Response.Redirect(UrlMapper.CampaignCandidates + "?id=" + id);
                }

            

            }
        }


    


        private void LoadForAdd(int id)
        {
            var item = new CampaignService().GetCampaign(id);

            if (item != null)
            {
                lblCampaignName.Text = item.CampaignName;

                hidId.Value = id.ToString();

            }
            else
            {
               Response.Redirect(UrlMapper.CampaignCandidates+"?id=" + id);
            }
        }

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            Session["UploadIssues"] = null;
            divIssues.Visible = false;

            if (IsValidExcelFile())
            {
                ProcessFile();
            }
        }

        private bool IsValidExcelFile()
        {
            if (!fileUpload.HasFile)
            {
                lblStatus.ShowMessage(new AppMessage { Message = "No file selected.", Status = MessageStatus.Error });
                return false;
            }

            if (!Path.GetExtension(fileUpload.PostedFile.FileName.ToLower()).Equals(".xlsx"))
            {
                lblStatus.ShowMessage(new AppMessage
                {
                    Message = "Excel 2007-2010 file format (*.xlsx) required.",
                    Status = MessageStatus.Error
                });

                return false;
            }
            return true;
        }

        public void ProcessFile()
        {
            var campaign = new CampaignService().GetCampaign(Int32.Parse(hidId.Value));

            Stream fileStream = fileUpload.PostedFile.InputStream;

            
           
            var excelEngine = new ExcelPackage(fileStream);
           
        
            var workBook = excelEngine.Workbook;
            var workSheet = workBook.Worksheets.First();

            var candidateService = new CandidateService();
            var entryService = new CampaignEntryService();
            var campaignId = Int32.Parse(hidId.Value);

            int totalSucceeded = 0;
            int errorCount = 0;

            var issuesList = new List<UploadTempViewModel>();

            var locationList = new LocationService().GetLocations(-1,-1);


            var context = ContextManager.AsSingleton<FotContext>();
            context.Configuration.ValidateOnSaveEnabled = false;
            


            for (int rowIndex = 2; rowIndex <= workSheet.Dimension.End.Row; rowIndex++) 
            {
               // if (workSheet.Cells[rowIndex, 1].Value == null || workSheet.Cells[rowIndex, 1].Text == string.Empty) continue;

                var candidate = new Candidate();

                try
                {





                    candidate.ClientUniqueID = string.IsNullOrWhiteSpace(workSheet.Cells[rowIndex, 1].Text)
                        ? null
                        : workSheet.Cells[rowIndex, 1].Text.Trim();
                    candidate.Email = workSheet.Cells[rowIndex, 2].Text.Trim();
                    candidate.Password = workSheet.Cells[rowIndex, 3].Text.Trim();
                    candidate.FirstName= workSheet.Cells[rowIndex , 4].Text;
                    candidate.LastName = workSheet.Cells[rowIndex , 5].Text;
                    candidate.MobileNo = workSheet.Cells[rowIndex , 6].Text;

                    var location = workSheet.Cells[rowIndex , 7].Text.ToUpper();

                    candidate.LocationId = locationList.Any(x => x.LocationName.Equals(location))
                                               ? locationList.First(x => x.LocationName.Equals(location)).LocationId
                                               : default(int?);
                    candidate.DateAdded = DateTime.Today;

                    #region Validation Region

                    var match = Regex.IsMatch(candidate.Email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

                    if (!candidate.LocationId.HasValue)
                    {
                        issuesList.Add(new UploadTempViewModel { RowNumber = rowIndex, ClientUniqueId = candidate.ClientUniqueID, Email = candidate.Email, Firstname = candidate.FirstName, Lastname = candidate.LastName, MobileNo = candidate.MobileNo, Password = candidate.Password, Location = location, Issue = "Invalid location" });
                        errorCount++;
                        continue;
                    }


                    if (!match)
                    {

                        issuesList.Add(new UploadTempViewModel
                        {
                            RowNumber = rowIndex,
                            ClientUniqueId = candidate.ClientUniqueID,
                            Email = candidate.Email,
                            Firstname = candidate.FirstName,
                            Lastname = candidate.LastName,
                            MobileNo = candidate.MobileNo,
                            Password = candidate.Password,
                            Location = location,
                            Issue = "Invalid Email"
                        });
                        errorCount++;
                        continue;
                    }

                    var mobileNumberValid = Regex.IsMatch(candidate.MobileNo, @"^\d{11}$");


                    if (!mobileNumberValid)
                    {
                        issuesList.Add(new UploadTempViewModel
                        {
                            RowNumber = rowIndex,
                            ClientUniqueId = candidate.ClientUniqueID,
                            Email = candidate.Email,
                            Firstname = candidate.FirstName,
                            Lastname = candidate.LastName,
                            MobileNo = candidate.MobileNo,
                            Password = candidate.Password,
                            Location = location,
                            Issue = "Invalid Mobile Number. Mobile number should be 11 digits (GSM format)"
                        });
                        errorCount++;
                        continue;
                    } 
                    #endregion


                    var app = candidateService.Add(candidate);

                        if (app.IsDone)
                        {
                            var entry = new CampaignEntry
                            {
                                CampaignId = campaignId,
                                CandidateId = (int)app.Data
                            };

                            if (campaign.IsUnproctored)
                            {
                                entry.CandidateAssessment = new CandidateAssessment
                                {
                                    CandidateGuid = Guid.NewGuid().ToString().Replace("-", "")
                                };
                            }


                            var tempApp = entryService.Add(entry);

                            if (tempApp.IsDone)
                            {
                                totalSucceeded++;
                            }
                            else
                            {
                                issuesList.Add(new UploadTempViewModel { RowNumber = rowIndex, ClientUniqueId = candidate.ClientUniqueID, Email = candidate.Email, Firstname = candidate.FirstName, Lastname = candidate.LastName, MobileNo = candidate.MobileNo, Password = candidate.Password, Location = location, Issue = tempApp.Message});
                                errorCount++;
                            }


                        }
                        else
                        {
                            issuesList.Add(new UploadTempViewModel { RowNumber = rowIndex, ClientUniqueId = candidate.ClientUniqueID, Email = candidate.Email, Firstname = candidate.FirstName, Lastname = candidate.LastName, MobileNo = candidate.MobileNo, Password = candidate.Password, Location = location, Issue = app.Message });
                            errorCount++;
                        }
                  
                }
                catch(Exception ex)
                {
                    issuesList.Add(new UploadTempViewModel { RowNumber = rowIndex, ClientUniqueId = candidate.ClientUniqueID, Email = candidate.Email, Firstname = candidate.FirstName, Lastname = candidate.LastName, MobileNo = candidate.MobileNo, Password = candidate.Password, Location = workSheet.Cells[rowIndex, 7].Text.ToUpper(), Issue = "An error occured. " + ex.Message });
                    errorCount++;
                }
            }

            lblStatus.ShowMessage(new AppMessage() { IsDone = true, Message = string.Format("Processed {0} entries successfully. Encountered {1} errors.", totalSucceeded, errorCount), Status = MessageStatus.Info });

            if (errorCount > 0)
            {
                Session["UploadIssues"] = issuesList;

                divIssues.Visible = true;

                RadGrid1.DataBind();

            }
            else
            {
                divIssues.Visible = false;
            }

            var admin = new AdminUserService().GetCurrentAdmin();

            new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "Candidate upload", LogEntryDetails = "User performed a candidate upload. Succeeded = ["+totalSucceeded+"], Failed = ["+errorCount+"]", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });

            int rows = candidateService.Context.Database.ExecuteSqlCommand(
                   "update Candidate set Username =  'DRG' + REPLACE(STR(CandidateId, 7), SPACE(1), '0') where Username is null");

            context.Configuration.ValidateOnSaveEnabled = true;
        }

        protected void bttnBackToCampaignCandidates_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.CampaignCandidates + "?id=" + hidId.Value);
        }



     

        public List<UploadTempViewModel> GetIssues(int startRow, int maxRows)
        {
            if (Session["UploadIssues"] != null)
            {
                var list = Session["UploadIssues"] as List<UploadTempViewModel>;

                if (startRow >= 0)
                {

                    return list.OrderBy(x => x.RowNumber).Skip(startRow).Take(maxRows).ToList();
                }
                else
                {
                   return  list.OrderBy(x => x.RowNumber).ToList();
                }
            }
            else
            {
                return new List<UploadTempViewModel>();
            }

        }

        public int CountIssues()
        {
           
                var list = Session["UploadIssues"] as List<UploadTempViewModel>;

                return list == null ? 0 : list.Count;
         

        }

        protected void bttnDownloadIssuesList_Click(object sender, EventArgs e)
        {
            DownloadIssues();
        }

        private void DownloadIssues()
        {
            var list = Session["UploadIssues"] as List<UploadTempViewModel>;

            if (list.Count > 0)
            {
                var sheetName = "Issues_List_" + DateTime.Today.ToString("dd-MMM-yyyy");

                using (var package = new ExcelPackage())
                {


                    var worksheet = package.Workbook.Worksheets.Add(sheetName);


                    #region headerRegion

                    worksheet.Cells[1, 1].Value = "ROW";
                    worksheet.Cells[1, 2].Value = "CLIENTUNIQUEID";
                    worksheet.Cells[1, 3].Value = "EMAIL";
                    worksheet.Cells[1, 4].Value = "PASSWORD";
                    worksheet.Cells[1, 5].Value = "FIRSTNAME";
                    worksheet.Cells[1, 6].Value = "LASTNAME";
                    worksheet.Cells[1, 7].Value = "MOBILE_NO";
                    worksheet.Cells[1, 8].Value = "LOCATION";
                    worksheet.Cells[1, 9].Value = "ISSUE";


                    using (var range = worksheet.Cells[1, 2, 1, 11])
                    {

                        range.AutoFitColumns(40);


                    }

                    using (var range = worksheet.Cells[1, 1, 1, 11])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Color.SetColor(Color.Teal);



                    }




                    #endregion

                    for (int i = 0; i < list.Count; i++)
                    {


                        int row = i + 2;



                        worksheet.Cells[row, 1].Value = list[i].RowNumber;
                        worksheet.Cells[row, 2].Value = list[i].ClientUniqueId;
                        worksheet.Cells[row, 3].Value = list[i].Email;
                        worksheet.Cells[row, 4].Value = list[i].Password;
                        worksheet.Cells[row, 5].Value = list[i].Firstname;
                        worksheet.Cells[row, 6].Value = list[i].Lastname;
                        worksheet.Cells[row, 7].Value = list[i].MobileNo;
                        worksheet.Cells[row, 8].Value = list[i].Location;
                        worksheet.Cells[row, 9].Value = list[i].Issue;


                    }



                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + sheetName + ".xlsx");

                }
            }
        }
    }
}