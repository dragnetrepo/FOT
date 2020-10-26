﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Fot.Lan;
using Fot.Client.Infrastructure;
using Fot.Client.Models;
using Fot.Client.Services;
using Telerik.Web.UI;
using System.Linq;
using System.IO;

namespace Fot.Client
{

    public partial class Test : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            // Utilities.CheckSerial();

            if (!Page.IsPostBack)
            {

            }
        }


        protected void bttnLogin_Click(object sender, EventArgs e)
        {
            DoLogin();
        }

        public void DoLogin()
        {
            var candidateService = new CandidateService();
            var ctx = candidateService.Context;
            var item = candidateService.GetCandidateByUsername(txtUsername.Text);


            if (item != null)
            {
                if (item.Password.Equals(txtPassword.Text))
                {


                    var list = candidateService.GetCandidateAssessments(item.CandidateId);



                    if (list.Count > 0)
                    {
                        trLogin.Visible = false;

                        if (list.Any(x => x.RequiresSEB))
                        {
                            if (string.IsNullOrWhiteSpace(item.SebGuid))
                            {
                                item.SebGuid = Guid.NewGuid().ToString();
                                ctx.SaveChanges();
                            }

                            trSeb.Visible = true;
                            lblMessage.Visible = false;

                            var files = new DirectoryInfo(Server.MapPath("~/seb")).GetFiles("*.seb");
                            foreach (var file in files)
                            {
                                if (DateTime.UtcNow - file.CreationTimeUtc > TimeSpan.FromDays(5))
                                {
                                    File.Delete(file.FullName);
                                }
                            }

                            var testUrl = ConfigurationManager.AppSettings["SebTestUrl"];
                            var configFileUrl = ConfigurationManager.AppSettings["SebUrlPrefix"];

                            var sebFileName = $"{item.Username}.seb";

                            var destPath = Server.MapPath("~/seb");

                            if (!File.Exists(Path.Combine(destPath, sebFileName)))
                            {
                                testUrl = testUrl + "?key=" + item.SebGuid;

                                var configXml = ConfigXml(testUrl);


                                var destFile = System.IO.Path.Combine(destPath, sebFileName);

                                var bytes = SebShared.SebConfigFileManager.EncryptSEBXmlWithPassword(configXml, item.Password, false, SebShared.SebSettings.sebConfigPurposes.sebConfigPurposeStartingExam, false);

                                File.WriteAllBytes(destFile, bytes);

                            }

                            var sebUrl = configFileUrl + sebFileName;

                            linkUrl.HRef = sebUrl;
                            lblStatus.Visible = false;
                        }
                        else
                        {
                            trTakeTest.Visible = true;

                            ListView1.DataSource = list;
                            ListView1.DataBind();
                        }



                    }
                    else
                    {
                        lblStatus.ShowMessage(new AppMessage { IsDone = false, Message = "You are not currently scheduled for an assessment.", Status = MessageStatus.Error });
                    }



                }
                else
                {
                    lblStatus.ShowMessage(new AppMessage { IsDone = false, Message = "Invalid username or password", Status = MessageStatus.Error });
                }
            }
            else
            {
                lblStatus.ShowMessage(new AppMessage { IsDone = false, Message = "Invalid username or password", Status = MessageStatus.Error });
            }
        }


        public string ConfigXml(string testUrl)
        {
            var xml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple Computer//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
  <dict>
    <key>originatorVersion</key>
    <string>SEB_Win_2.1.1</string>
    <key>startURL</key>
    <string>{testUrl}</string>
    <key>startResource</key>
    <string />
    <key>sebServerURL</key>
    <string />
    <key>hashedAdminPassword</key>
    <string>88A1881F0025CF7117501D07AEFC5D4BE6696656790D765678DF8BC48CA52687</string>
    <key>allowQuit</key>
    <true />
    <key>ignoreExitKeys</key>
    <true />
    <key>hashedQuitPassword</key>
    <string />
    <key>exitKey1</key>
    <integer>2</integer>
    <key>exitKey2</key>
    <integer>10</integer>
    <key>exitKey3</key>
    <integer>5</integer>
    <key>sebMode</key>
    <integer>0</integer>
    <key>browserMessagingSocket</key>
    <string>ws://localhost:8706</string>
    <key>browserMessagingPingTime</key>
    <integer>120000</integer>
    <key>sebConfigPurpose</key>
    <integer>0</integer>
    <key>allowPreferencesWindow</key>
    <true />
    <key>useAsymmetricOnlyEncryption</key>
    <false />
    <key>browserViewMode</key>
    <integer>1</integer>
    <key>browserWindowAllowAddressBar</key>
    <false />
    <key>newBrowserWindowAllowAddressBar</key>
    <false />
    <key>mainBrowserWindowWidth</key>
    <string>100%</string>
    <key>mainBrowserWindowHeight</key>
    <string>100%</string>
    <key>mainBrowserWindowPositioning</key>
    <integer>1</integer>
    <key>enableBrowserWindowToolbar</key>
    <false />
    <key>hideBrowserWindowToolbar</key>
    <false />
    <key>showMenuBar</key>
    <false />
    <key>showTaskBar</key>
    <true />
    <key>showSideMenu</key>
    <true />
    <key>taskBarHeight</key>
    <integer>40</integer>
    <key>touchOptimized</key>
    <false />
    <key>enableZoomText</key>
    <true />
    <key>enableZoomPage</key>
    <true />
    <key>zoomMode</key>
    <integer>0</integer>
    <key>allowSpellCheck</key>
    <false />
    <key>allowDictionaryLookup</key>
    <false />
    <key>allowSpellCheckDictionary</key>
    <array></array>
    <key>additionalDictionaries</key>
    <array></array>
    <key>showReloadButton</key>
    <true />
    <key>showTime</key>
    <true />
    <key>showInputLanguage</key>
    <true />
    <key>enableTouchExit</key>
    <false />
    <key>oskBehavior</key>
    <integer>2</integer>
    <key>audioControlEnabled</key>
    <true />
    <key>audioMute</key>
    <false />
    <key>audioVolumeLevel</key>
    <integer>25</integer>
    <key>audioSetVolumeLevel</key>
    <false />
    <key>allowDeveloperConsole</key>
    <false />
    <key>browserScreenKeyboard</key>
    <false />
    <key>newBrowserWindowByLinkPolicy</key>
    <integer>1</integer>
    <key>newBrowserWindowByScriptPolicy</key>
    <integer>2</integer>
    <key>newBrowserWindowByLinkBlockForeign</key>
    <false />
    <key>newBrowserWindowByScriptBlockForeign</key>
    <false />
    <key>newBrowserWindowByLinkWidth</key>
    <string>1000</string>
    <key>newBrowserWindowByLinkHeight</key>
    <string>100%</string>
    <key>newBrowserWindowByLinkPositioning</key>
    <integer>2</integer>
    <key>enablePlugIns</key>
    <true />
    <key>enableJava</key>
    <false />
    <key>enableJavaScript</key>
    <true />
    <key>blockPopUpWindows</key>
    <false />
    <key>allowVideoCapture</key>
    <false />
    <key>allowAudioCapture</key>
    <false />
    <key>allowBrowsingBackForward</key>
    <false />
    <key>newBrowserWindowNavigation</key>
    <true />
    <key>removeBrowserProfile</key>
    <true />
    <key>removeLocalStorage</key>
    <false />
    <key>enableSebBrowser</key>
    <false />
    <key>browserWindowAllowReload</key>
    <true />
    <key>newBrowserWindowAllowReload</key>
    <true />
    <key>showReloadWarning</key>
    <true />
    <key>newBrowserWindowShowReloadWarning</key>
    <false />
    <key>browserUserAgentWinDesktopMode</key>
    <integer>0</integer>
    <key>browserUserAgentWinDesktopModeCustom</key>
    <string />
    <key>browserUserAgentWinTouchMode</key>
    <integer>0</integer>
    <key>browserUserAgentWinTouchModeIPad</key>
    <string>Mozilla/5.0 (iPad; CPU OS 11_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/11.3 Mobile/15E216 Safari/605.1.15</string>
    <key>browserUserAgentWinTouchModeCustom</key>
    <string />
    <key>browserUserAgent</key>
    <string>Dragon54Dragnet21333</string>
    <key>browserUserAgentMac</key>
    <integer>0</integer>
    <key>browserUserAgentMacCustom</key>
    <string />
    <key>browserWindowTitleSuffix</key>
    <string />
    <key>allowPDFReaderToolbar</key>
    <false />
    <key>allowDownUploads</key>
    <true />
    <key>downloadDirectoryOSX</key>
    <string>~/Downloads</string>
    <key>downloadDirectoryWin</key>
    <string />
    <key>openDownloads</key>
    <false />
    <key>chooseFileToUploadPolicy</key>
    <integer>0</integer>
    <key>downloadPDFFiles</key>
    <false />
    <key>allowPDFPlugIn</key>
    <false />
    <key>downloadAndOpenSebConfig</key>
    <true />
    <key>backgroundOpenSEBConfig</key>
    <false />
    <key>examKeySalt</key>
    <data></data>
    <key>examSessionClearCookiesOnEnd</key>
    <true />
    <key>examSessionClearCookiesOnStart</key>
    <true />
    <key>browserExamKey</key>
    <string />
    <key>browserURLSalt</key>
    <true />
    <key>sendBrowserExamKey</key>
    <false />
    <key>quitURL</key>
    <string />
    <key>quitURLConfirm</key>
    <true />
    <key>restartExamURL</key>
    <string />
    <key>restartExamUseStartURL</key>
    <false />
    <key>restartExamText</key>
    <string />
    <key>restartExamPasswordProtected</key>
    <true />
    <key>additionalResources</key>
    <array></array>
    <key>monitorProcesses</key>
    <true />
    <key>allowSwitchToApplications</key>
    <false />
    <key>allowFlashFullscreen</key>
    <false />
    <key>permittedProcesses</key>
    <array>
      <dict>
        <key>active</key>
        <true />
        <key>autostart</key>
        <true />
        <key>iconInTaskbar</key>
        <true />
        <key>runInBackground</key>
        <false />
        <key>allowUserToChooseApp</key>
        <true />
        <key>strongKill</key>
        <true />
        <key>os</key>
        <integer>1</integer>
        <key>title</key>
        <string>Dragnet Assessment Window CM</string>
        <key>description</key>
        <string>Special version of Chrome Web Browser for use in establishing a web video connection.</string>
        <key>executable</key>
        <string>chrominimum.exe</string>
        <key>originalName</key>
        <string>Chrominimum.exe</string>
        <key>windowHandlingProcess</key>
        <string />
        <key>path</key>
        <string>c:\program files\chrominimum</string>
        <key>identifier</key>
        <string />
        <key>arguments</key>
        <array>
          <dict>
            <key>active</key>
            <true />
            <key>argument</key>
            <string>--start-url=""{testUrl}""</string>
          </dict>
		   <dict>
             <key>active</key>
                <true/>
                <key>argument</key>
                <string>--show-maximized</string>
           </dict>
           <dict>
             <key>active</key>
               <true/>
               <key>argument</key>
               <string>--user-agent-suffix=""SEB-Dragon54Dragnet21333""</string>
           </dict>
        </array>
      </dict>
    </array>
    <key>prohibitedProcesses</key>
    <array>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>join.me.exe</string>
        <key>originalName</key>
        <string>join.me.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>RPCSuite.exe</string>
        <key>originalName</key>
        <string>RPCSuite.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>RPCService.exe</string>
        <key>originalName</key>
        <string>RPCService.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>RemotePCDesktop.exe</string>
        <key>originalName</key>
        <string>RemotePCDesktop.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>beamyourscreen-host.exe</string>
        <key>originalName</key>
        <string>beamyourscreen-host.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>AeroAdmin.exe</string>
        <key>originalName</key>
        <string>AeroAdmin.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>Mikogo-host.exe</string>
        <key>originalName</key>
        <string>Mikogo-host.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>chromoting.exe</string>
        <key>originalName</key>
        <string>chromoting.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>vncserverui.exe</string>
        <key>originalName</key>
        <string>vncserverui.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>vncviewer.exe</string>
        <key>originalName</key>
        <string>vncviewer.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>vncserver.exe</string>
        <key>originalName</key>
        <string>vncserver.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>TeamViewer.exe</string>
        <key>originalName</key>
        <string>TeamViewer.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>GotoMeetingWinStore.exe</string>
        <key>originalName</key>
        <string>GotoMeetingWinStore.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>g2mcomm.exe</string>
        <key>originalName</key>
        <string>g2mcomm.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>SkypeHost.exe</string>
        <key>originalName</key>
        <string>SkypeHost.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>SkypeApp.exe</string>
        <key>originalName</key>
        <string>SkypeApp.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
      <dict>
        <key>active</key>
        <true />
        <key>currentUser</key>
        <true />
        <key>strongKill</key>
        <false />
        <key>os</key>
        <integer>1</integer>
        <key>executable</key>
        <string>Skype.exe</string>
        <key>originalName</key>
        <string>Skype.exe</string>
        <key>description</key>
        <string />
        <key>identifier</key>
        <string />
        <key>windowHandlingProcess</key>
        <string />
        <key>user</key>
        <string />
      </dict>
    </array>
    <key>enableURLFilter</key>
    <false />
    <key>enableURLContentFilter</key>
    <false />
    <key>URLFilterRules</key>
    <array></array>
    <key>URLFilterEnable</key>
    <false />
    <key>URLFilterEnableContentFilter</key>
    <false />
    <key>blacklistURLFilter</key>
    <string />
    <key>whitelistURLFilter</key>
    <string />
    <key>urlFilterTrustedContent</key>
    <true />
    <key>urlFilterRegex</key>
    <true />
    <key>embeddedCertificates</key>
    <array></array>
    <key>pinEmbeddedCertificates</key>
    <false />
    <key>proxySettingsPolicy</key>
    <integer>0</integer>
    <key>proxies</key>
    <dict>
      <key>ExceptionsList</key>
      <array></array>
      <key>ExcludeSimpleHostnames</key>
      <false />
      <key>AutoDiscoveryEnabled</key>
      <false />
      <key>AutoConfigurationEnabled</key>
      <false />
      <key>AutoConfigurationJavaScript</key>
      <string />
      <key>AutoConfigurationURL</key>
      <string />
      <key>FTPPassive</key>
      <true />
      <key>HTTPEnable</key>
      <false />
      <key>HTTPPort</key>
      <integer>80</integer>
      <key>HTTPProxy</key>
      <string />
      <key>HTTPRequiresPassword</key>
      <false />
      <key>HTTPUsername</key>
      <string />
      <key>HTTPPassword</key>
      <string />
      <key>HTTPSEnable</key>
      <false />
      <key>HTTPSPort</key>
      <integer>443</integer>
      <key>HTTPSProxy</key>
      <string />
      <key>HTTPSRequiresPassword</key>
      <false />
      <key>HTTPSUsername</key>
      <string />
      <key>HTTPSPassword</key>
      <string />
      <key>FTPEnable</key>
      <false />
      <key>FTPPort</key>
      <integer>21</integer>
      <key>FTPProxy</key>
      <string />
      <key>FTPRequiresPassword</key>
      <false />
      <key>FTPUsername</key>
      <string />
      <key>FTPPassword</key>
      <string />
      <key>SOCKSEnable</key>
      <false />
      <key>SOCKSPort</key>
      <integer>1080</integer>
      <key>SOCKSProxy</key>
      <string />
      <key>SOCKSRequiresPassword</key>
      <false />
      <key>SOCKSUsername</key>
      <string />
      <key>SOCKSPassword</key>
      <string />
      <key>RTSPEnable</key>
      <false />
      <key>RTSPPort</key>
      <integer>554</integer>
      <key>RTSPProxy</key>
      <string />
      <key>RTSPRequiresPassword</key>
      <false />
      <key>RTSPUsername</key>
      <string />
      <key>RTSPPassword</key>
      <string />
    </dict>
    <key>sebServicePolicy</key>
    <integer>1</integer>
    <key>sebServiceIgnore</key>
    <true />
    <key>allowVirtualMachine</key>
    <false />
    <key>allowScreenSharing</key>
    <false />
    <key>enablePrivateClipboard</key>
    <true />
    <key>createNewDesktop</key>
    <true />
    <key>killExplorerShell</key>
    <false />
    <key>enableLogging</key>
    <true />
    <key>allowApplicationLog</key>
    <false />
    <key>showApplicationLogButton</key>
    <false />
    <key>logDirectoryOSX</key>
    <string>~/Documents</string>
    <key>logDirectoryWin</key>
    <string />
    <key>allowWlan</key>
    <false />
    <key>lockOnMessageSocketClose</key>
    <true />
    <key>minMacOSVersion</key>
    <integer>4</integer>
    <key>enableAppSwitcherCheck</key>
    <true />
    <key>forceAppFolderInstall</key>
    <true />
    <key>allowUserAppFolderInstall</key>
    <false />
    <key>allowSiri</key>
    <false />
    <key>allowDictation</key>
    <false />
    <key>detectStoppedProcess</key>
    <true />
    <key>allowDisplayMirroring</key>
    <false />
    <key>allowedDisplaysMaxNumber</key>
    <integer>1</integer>
    <key>allowedDisplayBuiltin</key>
    <true />
    <key>enableChromeNotifications</key>
    <false />
    <key>enableWindowsUpdate</key>
    <false />
    <key>insideSebEnableSwitchUser</key>
    <false />
    <key>insideSebEnableLockThisComputer</key>
    <false />
    <key>insideSebEnableChangeAPassword</key>
    <false />
    <key>insideSebEnableStartTaskManager</key>
    <false />
    <key>insideSebEnableLogOff</key>
    <false />
    <key>insideSebEnableShutDown</key>
    <false />
    <key>insideSebEnableEaseOfAccess</key>
    <false />
    <key>insideSebEnableVmWareClientShade</key>
    <false />
    <key>insideSebEnableNetworkConnectionSelector</key>
    <false />
    <key>setVmwareConfiguration</key>
    <false />
    <key>hookKeys</key>
    <true />
    <key>enableEsc</key>
    <true />
    <key>enableCtrlEsc</key>
    <false />
    <key>enableAltEsc</key>
    <false />
    <key>enableAltTab</key>
    <true />
    <key>enableAltF4</key>
    <false />
    <key>enableStartMenu</key>
    <false />
    <key>enableRightMouse</key>
    <true />
    <key>enablePrintScreen</key>
    <false />
    <key>enableAltMouseWheel</key>
    <false />
    <key>enableF1</key>
    <true />
    <key>enableF2</key>
    <true />
    <key>enableF3</key>
    <true />
    <key>enableF4</key>
    <true />
    <key>enableF5</key>
    <true />
    <key>enableF6</key>
    <true />
    <key>enableF7</key>
    <true />
    <key>enableF8</key>
    <true />
    <key>enableF9</key>
    <true />
    <key>enableF10</key>
    <true />
    <key>enableF11</key>
    <true />
    <key>enableF12</key>
    <true />
  </dict>
</plist>";

            return xml;
        }
    }
}