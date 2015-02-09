using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Management;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Threading;

using ICSharpCode.SharpZipLib.GZip;
using iFaith.My;
using iFaith.My.Resources;
using System.Net;
using CFManzana;


namespace iFaith
{
    public partial class DFU : Form
    {
        FormMain objFormMain;
        bool appletvMode;
        bool SHSHMode;
        public bool QuitDFU;
        private string Results;
        private BackgroundWorker _BackgroundWorker1;
        private BackgroundWorker _Prepare;
        private iFaith objiFaith;
        private delUpdate Finished;
        //DE RUN
        private string buildsAvailable;
        public bool DumpMode;
        private bool exploited;
        private delUpdate Finished_shsh;
        private string latestIPSWURL;
        private string Results_shsh;
        private bool runninglimera1n;
        private System.Threading.Timer Timer;
        private int timercount;
        //FIN DE RUN

        public DFU(FormMain vobjFormMain)
        {
            InitializeComponent();
            objFormMain = vobjFormMain;
            appletvMode = false;
            labelAppleTVMode.Hide();
            labelNonAppleTVMode.Hide();
            buttonStartDFU.Hide();
            buttonRestartDFU.Hide();
            this.Label1.Text = "";
            objiFaith = new iFaith(this);
            this.dfuinstructionsLabel.Text = "";
            this.dfuinstructionstxt.Text = "";
            this.PrepareTXT.Text = "";
            this.Prepare = new BackgroundWorker();
            this._BackgroundWorker1 = new BackgroundWorker();
            this.labelTimer1.Text = "";
            base.FormClosing += new FormClosingEventHandler(this.DFU_ByeBye);
            if (vobjFormMain == null)
            {
                SHSHMode = true;
                labelTool.Text = "Dump SHSH";
                LoadGUI();
            }
            else
            {
                SHSHMode = false;
                labelTool.Text = "iReb";
            }
            base.Load += new EventHandler(this.DFU_Load);
            this.QuitDFU = false;
            this.Finished = new delUpdate(this.UpdateText);
            
            //this.InitializeComponent();
            
            this.timercount = 0;
            this.latestIPSWURL = string.Empty;
            this.DumpMode = false;
            
            this.runninglimera1n = false;
            this.exploited = false;
            
            this.buildsAvailable = string.Empty;
            
        }


        // IMPORTADO DE RUN
        public void Center_status()
        {
            //this.Status.Left = (int)Math.Round((double)((((double)this.Width) / 2.0) - (((double)this.Status.Width) / 2.0)));
            //Application.DoEvents();
        }
        // IMPORTADO DE RUN
        public void Check()
        {
            if (this.getenvbox.Text.Contains("ready"))
            {
                //MyProject.Forms.MDIMain.dumpbtn.Enabled = true;
                //MyProject.Forms.MDIMain.dumpbtn.Checked = true;
                //MyProject.Forms.MDIMain.dumptxt.ForeColor = Color.White;
                this.PrepareTXT.Text = "Dumping...";
                //this.Status.Invoke(new MethodInvoker(this.Center_status));
                iFaith.DumpIt();
                this.GoGoGadgetsn0wbreeze();
            }
            else if (this.getenvbox.Text.Contains("failed-1"))
            {
                Interaction.MsgBox("iFaith failed to load the kernel from the filesystem!", MsgBoxStyle.Critical, null);
            }
            else if (this.getenvbox.Text.Contains("failed-2"))
            {
                Interaction.MsgBox("iFaith was unable to find a KBAG from the kernel!\r\rIs this an old bootrom?", MsgBoxStyle.Critical, null);
            }
            else if (this.getenvbox.Text.Contains("failed-3"))
            {
                Interaction.MsgBox("One or more flash images on the device do not contain a SHSH blob!", MsgBoxStyle.Critical, null);
            }
            else if (this.getenvbox.Text.Contains("failed-4"))
            {
                Interaction.MsgBox("Unknown iOS detected!\r\rAre you running the latest iFaith?", MsgBoxStyle.Critical, null);
            }
            else if (this.getenvbox.Text.Contains("failed-5"))
            {
                Interaction.MsgBox("No valid block device containing flash images was found!", MsgBoxStyle.Critical, null);
            }
            else if (this.getenvbox.Text.Contains("failed-6"))
            {
                Interaction.MsgBox("[NOTICE] This iDevice has the 24kpwn untether applied!\r----------------------------------------------------------\rSince the 24kpwn untether is applied to your iDevice,\rno SHSH blobs are on your iDevice. As long as you\rrestore to custom firmware, no SHSH blobs are needed.\r----------------------------------------------------------", MsgBoxStyle.Critical, null);
            }
            else if (this.getenvbox.Text.Contains("pending") | this.getenvbox.Text.Contains(modProcessCmd.board))
            {
                this.getenvbox.Text = string.Empty;
                iFaith.getenv("status");
                modProcessCmd.Delay(1.0);
                this.Check();
            }
            else
            {
                Interaction.MsgBox("An invalid response was recieved from the device!\r\rResponse: " + this.getenvbox.Text, MsgBoxStyle.Critical, null);
                iFaith.setenv("auto-boot", "true");
                iFaith.iRecovery_cmd("reset");
            }
            if (this.getenvbox.Text.Contains("failed"))
            {
                iFaith.setenv("auto-boot", "true");
                iFaith.iRecovery_cmd("reset");
            }
        }
        // IMPORTADO DE RUN
        public String getSHSHBlob()
        {
            return this.shshblob.Text;
        }
        // IMPORTADO DE RUN
        public String getEnvBox()
        {
            return this.getenvbox.Text;
        }
        // IMPORTADO DE RUN
        public void setEnvBox(String text)
        {
            this.getenvbox.Text = text;
        }
        // IMPORTADO DE RUN
        private void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled && (e.Error == null))
                {
                    this.latestIPSWURL = e.Result;
                }
                else
                {
                    this.latestIPSWURL = "failed";
                }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                this.latestIPSWURL = "failed";
                ProjectData.ClearProjectError();
            }
        }
        // IMPORTADO DE RUN
        private void DownloadStringCompleted_ineal(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled && (e.Error == null))
                {
                    this.buildsAvailable = e.Result;
                }
                else
                {
                    this.buildsAvailable = "error";
                }
                if (this.buildsAvailable.Replace(" ", "") == string.Empty)
                {
                    this.buildsAvailable = "error";
                }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                this.buildsAvailable = "error";
                Application.DoEvents();
                ProjectData.ClearProjectError();
            }
        }

        // IMPORTADO DE RUN
        public void GoGoGadgetsn0wbreeze()
        {
            int num = 0;
            //MyProject.Forms.MDIMain.dumpbtn.Enabled = false;
            //MyProject.Forms.MDIMain.dumptxt.ForeColor = Color.DimGray;
            //MyProject.Forms.MDIMain.createipsw.Enabled = true;
            //MyProject.Forms.MDIMain.createipsw.Checked = true;
            //MyProject.Forms.MDIMain.createipswtxt.ForeColor = Color.White;
            //MyProject.Forms.MDIMain.Activate();
            //MyProject.Forms.MDIMain.TopMost = false;
            Application.DoEvents();
            this.tehfile.Text = Encoding.UTF8.GetString(iFaith.String_To_Bytes(this.shshblob.Text));
            for (num = 0; num != this.tehfile.Lines.Length; num++)
            {
                if (this.tehfile.Lines[num].Contains("ecid"))
                {
                    iFaith.ECID = this.tehfile.Lines[num].Replace("<ecid>", "").Replace("</ecid>", "");
                }
                else if (this.tehfile.Lines[num].Contains("ios"))
                {
                    iFaith.realiosversion = this.tehfile.Lines[num].Replace("<ios>", "").Replace("</ios>", "");
                }
                else if (this.tehfile.Lines[num].Contains("board"))
                {
                    modProcessCmd.board = this.tehfile.Lines[num].Replace("<board>", "").Replace("</board>", "");
                }
            }
            this.ProgressBar1.Style = ProgressBarStyle.Marquee;
            this.PrepareTXT.Text = "Prepare to choose a saving location in [3]";
            //this.Center_status();
            modProcessCmd.Delay(1.0);
            this.PrepareTXT.Text = "Prepare to choose a saving location in [2]";
            modProcessCmd.Delay(1.0);
            this.PrepareTXT.Text = "Prepare to choose a saving location in [1]";
            modProcessCmd.Delay(1.0);
            this.PrepareTXT.Text = "Choose a saving Location...";
            //this.Center_status();
            this.SaveBlobs.FileName = iFaith.ECID + "_" + modProcessCmd.iDevice.Replace(" ", "_") + "-" + iFaith.realiosversion.Replace(" ", "_") + "-blobs";
            this.SaveBlobs.ShowDialog();
            iFaith.setenv("auto-boot", "true");
            iFaith.iRecovery_cmd("reset");
            this.PrepareTXT.Text = "Saving cache (locally)...";
            //this.Center_status();
            string fileName = this.SaveBlobs.FileName;
            if (!fileName.EndsWith(".ifaith"))
            {
                fileName = fileName + ".ifaith";
            }
            this.tehfile.SaveFile(fileName, RichTextBoxStreamType.PlainText);
            this.tehfile.SaveFile(fileName, RichTextBoxStreamType.PlainText);
            if (iFaith.Debug_Mode)
            {
                Interaction.MsgBox("DEBUG MODE ENABLED!\r\riFaith will not send this cache to the server...", MsgBoxStyle.Exclamation, null);
            }
            this.PrepareTXT.Text = "Communicating with iNeal.me...";
            //this.Center_status();
            Application.DoEvents();
            WebClient client = new WebClient();
            MDIMain objMDIMain = new MDIMain();
            client.Headers.Add("user-agent", "iFaith-v" + objMDIMain.VersionNumber);
            this.buildsAvailable = string.Empty;
            client.DownloadStringAsync(new Uri("http://api.ineal.me/tss/" + modProcessCmd.board + "/if"));
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.DownloadStringCompleted_ineal);
            num = 0;
            while (!((!client.IsBusy & (this.buildsAvailable != string.Empty)) | (num == iFaith.webTimeOut)))
            {
                Application.DoEvents();
                modProcessCmd.Delay(1.0);
                num++;
            }
            client.Dispose();
            if ((this.buildsAvailable == "error") | (this.buildsAvailable == string.Empty))
            {
                this.buildsAvailable = "latest";
            }
            bool flag = false;
            string iosVersion = string.Empty;
            string iosBuild = string.Empty;
            string expression = string.Empty;
            string str5 = string.Empty;
            string str6 = string.Empty;
            string str7 = string.Empty;
            int chipIDvar = 0;
            int boardIDvar = 0;
            string producttypevar = string.Empty;
            string deviceNamevar = string.Empty;
            iFaith.giveMeDetails(modProcessCmd.board, ref chipIDvar, ref boardIDvar, ref deviceNamevar, ref producttypevar);
            string[] array = Strings.Split(this.buildsAvailable, "#", -1, CompareMethod.Binary);
            producttypevar = producttypevar.ToLower();
            Array.Sort<string>(array, StringComparer.OrdinalIgnoreCase);
            foreach (string str10 in array)
            {
                if (str10.Contains("b") & str10.Contains("."))
                {
                    WebClient client2 = new WebClient();
                    client2.Headers.Add("user-agent", "iFaith-v" + objMDIMain.VersionNumber);
                    this.latestIPSWURL = string.Empty;
                    client2.DownloadStringAsync(new Uri("http://api.ineal.me/tss/request.plist/" + modProcessCmd.board + "/" + str10.Substring(str10.IndexOf("-") + 1, (str10.Length - str10.IndexOf("-")) - 1)));
                    client2.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.DownloadStringCompleted);
                    num = 0;
                    while (!((!client2.IsBusy & (this.latestIPSWURL != string.Empty)) | (num == iFaith.webTimeOut)))
                    {
                        Application.DoEvents();
                        modProcessCmd.Delay(1.0);
                        num++;
                    }
                    client2.Dispose();
                    if (this.latestIPSWURL == string.Empty)
                    {
                        this.latestIPSWURL = "error";
                    }
                    if (this.latestIPSWURL == "error")
                    {
                        str5 = str5 + "Unknown (" + str10 + ")\r";
                        Interaction.MsgBox("iFaith failed to download the " + str10 + " SHSH blobs!\r\rEnsure you have the latest iFaith, and have an active internet connection!", MsgBoxStyle.Critical, null);
                    }
                }
                else
                {
                    this.PrepareTXT.Text = "Communicating with icj.me...";
                    //this.Center_status();
                    WebClient client3 = new WebClient();
                    client3.Headers.Add("user-agent", "iFaith-v" + objMDIMain.VersionNumber);
                    this.latestIPSWURL = string.Empty;
                    client3.DownloadStringAsync(new Uri("http://api.ios.icj.me/v2/" + modProcessCmd.board + "/" + str10 + "/url"));
                    client3.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.DownloadStringCompleted);
                    for (num = 0; !((!client3.IsBusy & (this.latestIPSWURL != string.Empty)) | (num == iFaith.webTimeOut)); num++)
                    {
                        Application.DoEvents();
                        modProcessCmd.Delay(1.0);
                    }
                    client3.Dispose();
                    if (this.latestIPSWURL == string.Empty)
                    {
                        this.latestIPSWURL = "failed";
                    }
                }
                string s = string.Empty;
                if (str10.Contains("b") & str10.Contains("."))
                {
                    iosVersion = str10.Substring(0, str10.IndexOf("-"));
                    iosBuild = str10.Substring(str10.IndexOf("-") + 1, (str10.Length - str10.IndexOf("-")) - 1);
                    s = this.latestIPSWURL.Replace("$ECID$", Conversions.ToString(Convert.ToInt64(iFaith.ECID, 0x10)));
                    RichTextBox box = new RichTextBox();
                    box.Text = s;
                }
                else if (this.latestIPSWURL != "failed")
                {
                    iFaith.DownloadFromURL(this.latestIPSWURL, "BuildManifest.plist", iFaith.temppath + @"\buildmanifest.plist");
                    My.MyComputer objComp = new My.MyComputer();
                    if (objComp.FileSystem.GetFileInfo(iFaith.temppath + @"\buildmanifest.plist").Length > 1L)
                    {
                        s = iFaith.MakeTSSRequestFromBuildManifest(iFaith.temppath + @"\buildmanifest.plist", modProcessCmd.board, ref iosVersion, ref iosBuild, ref boardIDvar, ref chipIDvar, ref producttypevar).Replace("$ECID$", Conversions.ToString(Convert.ToInt64(iFaith.ECID, 0x10)));
                    }
                }
                if (this.latestIPSWURL == "failed")
                {
                    str5 = str5 + "Unknown (" + str10 + ")\r";
                }
                else
                {
                    this.PrepareTXT.Text = "Fetching " + iosVersion + " (" + iosBuild + ") blobs from Apple...";
                    //this.Center_status();
                    string str12 = iFaith.SendPOSTRequest("http://" + iFaith.AppleTSSIP + "/TSS/controller?action=2", Encoding.ASCII.GetBytes(s), "InetURL/1.0");
                    if (str12.Contains("STATUS=0&MESSAGE=SUCCESS"))
                    {
                        str7 = str7 + iosVersion + " (" + iosBuild + ")\r";
                        flag = true;
                        this.PrepareTXT.Text = "Submitting " + iosVersion + " (" + iosBuild + ") blobs to Cydia...";
                        //this.Center_status();
                        if (iFaith.SendPOSTRequest("http://cydia.saurik.com/tss@home/api/store/" + Conversions.ToString(Convert.ToInt64(Conversions.ToString(chipIDvar), 0x10)) + "/" + Conversions.ToString(boardIDvar) + "/" + Conversions.ToString(Convert.ToInt64(iFaith.ECID, 0x10)), Encoding.ASCII.GetBytes(str12), "iFaith-v" + objMDIMain.VersionNumber) != "failed")
                        {
                            expression = expression + iosVersion + " (" + iosBuild + ")\r";
                        }
                        else
                        {
                            str6 = str6 + iosVersion + " (" + iosBuild + ")\r";
                            this.PrepareTXT.Text = "Cydia's TSS@Home rejected our blobs :(";
                            //this.Center_status();
                        }
                        string path = Environment.GetEnvironmentVariable("UserProfile") + @"\.shsh";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        System.IO.File.WriteAllText(iFaith.temppath + @"\tss-response.xml", str12.Substring(str12.IndexOf("<?xml"), (str12.IndexOf("</plist>") - str12.IndexOf("<?xml")) + 8));
                        iFaith.xml_to_bplist(iFaith.temppath + @"\tss-response.xml", iFaith.temppath + @"\tss-response.plist");
                        string strFile = path + @"\" + Conversions.ToString(Convert.ToInt64(iFaith.ECID, 0x10)) + "-" + producttypevar + "-" + iosVersion + ".shsh";
                        if (!modFile.File_Exists(strFile))
                        {
                            using (Stream stream = System.IO.File.OpenRead(iFaith.temppath + @"\tss-response.plist"))
                            {
                                using (FileStream stream2 = System.IO.File.Create(strFile))
                                {
                                    using (Stream stream3 = new GZipOutputStream(stream2))
                                    {
                                        int num6 = 0;
                                        byte[] buffer = new byte[0xfff];
                                        while (iFaith.InlineAssignHelper<int>(ref num6, stream.Read(buffer, 0, buffer.Length)) != 0)
                                        {
                                            stream3.Write(buffer, 0, num6);
                                        }
                                    }
                                }
                            }
                        }
                        iFaith.Delete_File(iFaith.temppath + @"\tss-response.plist");
                        iFaith.Delete_File(iFaith.temppath + @"\tss-response.xml");
                        iFaith.Delete_File(iFaith.temppath + @"\buildmanifest.plist");
                    }
                    else
                    {
                        str5 = str5 + iosVersion + " (" + iosBuild + ")\r";
                    }
                }
            }
            this.PrepareTXT.Text = "Submitting Dumped Blobs to iAcqua...";
            //this.Center_status();
            string str16 = iFaith.SendPOSTRequest("http://iacqua.ih8sn0w.com/submit.php?ecid=" + iFaith.ECID + "&board=" + modProcessCmd.board + "&ios=" + iFaith.realiosversion.Replace(" ", "%20"), Encoding.ASCII.GetBytes(this.tehfile.Text), iFaith.fairydust);
            this.tehfile.Dispose();
            this.createipsw.Enabled = false;
            this.createipsw.Checked = false;
            this.createipswtxt.ForeColor = Color.DimGray;
            this.PrepareTXT.Text = "Done! :)";
            //this.Center_status();
            this.Text = "iFaith v" + objMDIMain.VersionNumber + " -- By: iH8sn0w";
            string prompt = "iFaith has finished dumping your " + iFaith.realiosversion + " SHSH Blobs!\r\rKeep them in a VERY safe spot! Even email them to yourself!\r\r";
            if (str16 == "done")
            {
                prompt = prompt + "The dumped iOS " + iFaith.realiosversion + " blobs were successfully submitted to iFaith's SHSH caching server.\r\r";
            }
            if (str7 != string.Empty)
            {
                prompt = prompt + "iFaith has fetched the following shsh blobs directly from Apple: \r\r";
                foreach (string str18 in Strings.Split(str7, "\r", -1, CompareMethod.Binary))
                {
                    prompt = prompt + str18 + "\r";
                }
                prompt = prompt + "They have been stored locally at: \r" + Environment.GetEnvironmentVariable("UserProfile") + @"\.shsh\" + Conversions.ToString(Convert.ToInt64(iFaith.ECID, 0x10)) + "-*";
                if (expression != string.Empty)
                {
                    prompt = prompt + "\r\riFaith successfully submitted the following fetched blobs to Cydia: \r\r";
                    foreach (string str19 in Strings.Split(expression, "\r", -1, CompareMethod.Binary))
                    {
                        prompt = prompt + str19 + "\r";
                    }
                }
            }
            prompt = prompt + "You will now be returned to the main menu where you can create a\rsigned IPSW with your blobs or download available SHSH blobs from \rCydia/iFaith's SHSH server.";
            if (str5 != string.Empty)
            {
                string str20 = "iFaith has failed to fetch the following blobs:\r\r";
                foreach (string str21 in Strings.Split(str5, "\r", -1, CompareMethod.Binary))
                {
                    str20 = str20 + str21 + "\r";
                }
                Interaction.MsgBox(str20 + "\rAre you running the latest version of iFaith?\r\r* It is possible that Apple has stopped signing the above firmwares!", MsgBoxStyle.Critical, null);
            }
            Interaction.MsgBox(prompt, MsgBoxStyle.Information, null);
            this.Restore_Window();
        }
        // IMPORTADO DE RUN
        public void gogogo()
        {
            MDIMain objMain = new MDIMain();
            objMain.Prepbtn.Enabled = false;
            objMain.preptxt.ForeColor = Color.DimGray;
            objMain.exploitbtn.Enabled = true;
            objMain.exploitbtn.Checked = true;
            objMain.exploittxt.ForeColor = Color.White;
            this.runninglimera1n = true;
            if (modProcessCmd.iDevice == "iPod Touch 2G")
            {
                this.PrepareTXT.Text = "Exploiting with steaks4uce...";
                this.PictureBox1.Image = My.Resources.Resources.steaks4uce;
            }
            else
            {
                this.PrepareTXT.Text = "Exploiting with limera1n...";
                this.PictureBox1.Image = My.Resources.Resources.limera1n;
            }
            this.PrepareTXT.Invoke(new MethodInvoker(this.Center_status));
            iFaith.iRecovery_exploit();
            this.exploited = true;
            this.PrepareTXT.Text = "Waiting for " + modProcessCmd.iDevice + "...";
            this.PrepareTXT.Invoke(new MethodInvoker(this.Center_status));
            this.PictureBox1.Image = My.Resources.Resources.Dove;
            iFaith.Wait_For_DFU();
            objMain.exploitbtn.Enabled = false;
            objMain.exploittxt.ForeColor = Color.DimGray;
            objMain.uploadibssbtn.Enabled = true;
            objMain.uploadibssbtn.Checked = true;
            objMain.uploadibsstxt.ForeColor = Color.White;
            this.PrepareTXT.Text = "Uploading iBSS...";
            this.PrepareTXT.Invoke(new MethodInvoker(this.Center_status));
            iFaith.iRecovery_file(iFaith.temppath + @"\iBSS." + modProcessCmd.board + ".RELEASE.dfu");
            this.PrepareTXT.Text = "Waiting for " + modProcessCmd.iDevice + "...";
            this.PrepareTXT.Invoke(new MethodInvoker(this.Center_status));
            if (modProcessCmd.iDevice == "iPod Touch 2G")
            {
                iFaith.Wait_For_iBoot();
            }
            else
            {
                iFaith.Wait_For_DFU();
            }
            objMain.uploadibssbtn.Enabled = false;
            objMain.uploadibsstxt.ForeColor = Color.DimGray;
            objMain.uploadibecbtn.Enabled = true;
            objMain.uploadibecbtn.Checked = true;
            objMain.uploadibectxt.ForeColor = Color.White;
            this.PrepareTXT.Text = "Uploading iBEC...";
            this.PrepareTXT.Invoke(new MethodInvoker(this.Center_status));
            iFaith.iRecovery_file(iFaith.temppath + @"\iBEC." + modProcessCmd.board + ".RELEASE.dfu");
            if (modProcessCmd.iDevice == "iPod Touch 2G")
            {
                iFaith.iRecovery_cmd("go");
            }
            this.PrepareTXT.Text = "Waiting for " + modProcessCmd.iDevice + "...";
            this.PrepareTXT.Invoke(new MethodInvoker(this.Center_status));
            iFaith.Wait_For_iBoot();
            objMain.uploadibecbtn.Enabled = false;
            objMain.uploadibectxt.ForeColor = Color.DimGray;
            this.PrepareTXT.Text = "Validating stuff with " + modProcessCmd.iDevice + "...";
            this.PrepareTXT.Invoke(new MethodInvoker(this.Center_status));
            this.Luv();
        }

        // IMPORTADO DE RUN
        public void Luv()
        {
            int num = 0;
            while (num != 30)
            {
                iFaith.getenv("config_board");
                modProcessCmd.Delay(1.0);
                if (this.getenvbox.Text.Replace(" ", "") != string.Empty)
                {
                    break;
                }
                num++;
            }
            if ((num == 30) | (this.getenvbox.Text.Replace(" ", "").Replace("\r", "") != modProcessCmd.board))
            {
                Interaction.MsgBox("An invalid response was recieved from the device!\r\rResponse: " + this.getenvbox.Text, MsgBoxStyle.Critical, null);
                iFaith.setenv("auto-boot", "true");
                iFaith.iRecovery_cmd("reset");
                this.Restore_Window();
            }
            else
            {
                this.getenvbox.Text = string.Empty;
                iFaith.iRecovery_cmd("go ready");
                iFaith.getenv("status");
                for (num = 0; !((num == 30) | (this.getenvbox.Text.Replace(" ", "").Replace("\r", "") != string.Empty)); num++)
                {
                    modProcessCmd.Delay(1.0);
                    if (this.getenvbox.Text.Replace(" ", "") != string.Empty)
                    {
                        break;
                    }
                    iFaith.getenv("status");
                }
                this.Check();
                this.Restore_Window();
            }
        }
        // IMPORTADO DE RUN
        public void PrepareDoWork()
        {
            //this.PictureBox1.Image = My.Resources.Resources.Dove;
            this.PrepareTXT.Text = "Downloading Essentials...";
            this.ProgressBar1.Style = ProgressBarStyle.Marquee;
            this.ProgressBar1.MarqueeAnimationSpeed = 50;
            //this.Status.Invoke(new MethodInvoker(this.Center_status));
            this.PrepareTXT.Invoke(new MethodInvoker(iFaith.Downloader));
            this.PrepareTXT.Invoke(new MethodInvoker(this.gogogo));
        }
        // IMPORTADO DE RUN
        public void Restore_Window()
        {
            //MDIMain objMain = new MDIMain();
            //objMain.Text = "iFaith v" + objMain.VersionNumber + " -- By: iH8sn0w";
            //Form form = new Form();
            //form.MdiParent = MDIMain;
            //form.Width = this.Width / 2;
            //form.Height = this.Height / 2;
            //form.Show();
            //form.Hide();
            //Application.DoEvents();
            //MyProject.Forms.Welcome.MdiParent = MyProject.Forms.MDIMain;
            //MyProject.Forms.Welcome.Show();
            //MyProject.Forms.Welcome.Button1.Enabled = false;
            //Application.DoEvents();
            //MyProject.Forms.About.MdiParent = MyProject.Forms.MDIMain;
            // MyProject.Forms.About.Show();
            //MyProject.Forms.About.BringToFront();
            //Application.DoEvents();
            iFaith.Hide_Stages();
            Application.DoEvents();
            this.GoGoGadgetCleanUp();
            if (BackgroundWorker1 != null)
            {
                this.BackgroundWorker1.Dispose();
            }
            if (BackgroundWorker2 != null)
            {
                this.BackgroundWorker2.Dispose();
            }
            iFaith.KilliRecovery();
            if (objFormMain != null)
            {
                objFormMain.Show();
            }
            else
            {
                DumpSHSH objDump = new DumpSHSH(null);
                objDump.Show();
            }
            CleaniREB();

            Dispose();
        }
        // IMPORTADO DE RUN
        private void Tick(object state)
        {
            if (this.runninglimera1n & !this.exploited)
            {
                this.timercount++;
                if (this.timercount == 15)
                {
                    MDIMain objMain = new MDIMain();
                    iFaith.clsTopMostMessageBox.Show("iFaith-v" + objMain.VersionNumber, "This is taking longer than usual.\r\rEnsure you are not using a USB 3.0 port and try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    iFaith.KilliRecovery();
                    Process.GetCurrentProcess().Kill();
                    this.Dispose();
                }
            }
        }
        // IMPORTADO DE RUN
        public void update_1_shsh(object sender, DataReceivedEventArgs e)
        {
            this.UpdateTextBox_shsh(e.Data);
        }
        // IMPORTADO DE RUN
        private void UpdateText_shsh()
        {
            RichTextBox shshblob = this.shshblob;
            shshblob.Text = shshblob.Text + this.Results_shsh;
        }
        // IMPORTADO DE RUN
        private void UpdateTextBox_shsh(string Tex)
        {
            if (this.InvokeRequired)
            {
                UpdateTextBoxDelegate method = new UpdateTextBoxDelegate(this.UpdateTextBox_shsh);
                object[] args = new object[] { Tex };
                this.Invoke(method, args);
            }
            else
            {
                RichTextBox shshblob = this.shshblob;
                shshblob.Text = shshblob.Text + Tex;
                this.shshblob.Text = this.shshblob.Text.Replace(" ", "");
            }
        }


        // IMPORTADO DE RUN
        private delegate void delUpdate_shsh();
        // IMPORTADO DE RUN
        private delegate void UpdateTextBoxDelegate_shsh(string Text);
        // IMPORTADO DE RUN
        
        //FIN DE LO IMPORTADO DE RUN

        public void LoadGUI()
        {

            this.dfuinstructions.Visible = true;
            this.exploitbtn.Visible = true;
            this.Prepbtn.Visible = true;
            this.dumpbtn.Visible = true;
            this.uploadibecbtn.Visible = true;
            this.uploadibssbtn.Visible = true;
            this.createipsw.Visible = true;
            //this.blue.Visible = true;
            //this.dumptxt.Visible = true;
            //this.preptxt.Visible = true;
            //this.dfuinstructionstxt.Visible = true;
            //this.exploittxt.Visible = true;
            //this.createipswtxt.Visible = true;
            //this.uploadibsstxt.Visible = true;
            //this.uploadibectxt.Visible = true;
            //this.Button1.Visible = true;
        }


        private void DFU_ByeBye(object Sender, EventArgs e)
        {
            //MyProject.Forms.iAcqua.iLeft = true;
            iFaith.OhNoesShutDOWN = true;
            iFaith.KilliRecovery();
            string strError = "";
            modFile.Folder_Delete(iFaith.temppath, ref strError);
            //Process.GetCurrentProcess().Kill();
            this.Dispose();
        }


        private void DFU_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.GoGoGadgetCleanUp();
            if (BackgroundWorker1!=null)
            {
                this.BackgroundWorker1.Dispose();
            }
            if (BackgroundWorker2!=null)
	        {
		        this.BackgroundWorker2.Dispose();
	        }
            iFaith.KilliRecovery();
            if (objFormMain!=null)
            {
                objFormMain.Show();
            }
            else
            {
                DumpSHSH objDump = new DumpSHSH(null);
                objDump.Show();
            }
            CleaniREB();
            
            Dispose();
        }


        private void buttonGetSHSH_Click(object sender, EventArgs e)
        {
            this.appletvMode = true;
            this.labelAppleTVMode.Show();
            this.buttonAppleTV.Hide();
            this.buttonNonAppleTV.Hide();
            this.buttonStartDFU.Show();
            this.buttonRestartDFU.Show();
            this.Label1.Text = "Plug-in your Apple TV 2 via USB.";
            this.dfuinstructionstxt.Text = "Press Start only when your Apple TV 2 is blinking.";
        }

        private void buttoniReb_Click(object sender, EventArgs e)
        {
            this.appletvMode = false;
            this.labelNonAppleTVMode.Show();
            this.buttonAppleTV.Hide();
            this.buttonNonAppleTV.Hide();
            this.buttonStartDFU.Show();
            this.buttonRestartDFU.Show();
            this.Label1.Text = "Please power off your iDevice.Then press \"Start\".";
        }

        private void buttonStartDFU_Click(object sender, EventArgs e)
        {
            this.buttonRestartDFU.Enabled = true;
            this.buttonStartDFU.Enabled = false;
            //Incluido original
            this.Label1.Visible = false;
            iFaith.OhNoesShutDOWN = false;
            //fin
            //if (modProcessCmd.iDevice == "Apple TV 2")
            if (appletvMode==true)
            {
                //DFU para Apple TV
                if (SHSHMode==false)
                {
                    Definitions.iREB_mode = true;
                }
                else
                {
                    Definitions.iREB_mode = false;
                }
                
                //this.atv2animation.Visible = false;
                this.PictureBox1.Visible = false;
                this.dfuinstructionstxt.Visible = false;
                //this.atv2.Visible = true;
                this.DFUInstructions_ATV2();
            }
            else
            {
                //DFU GENERICO
                if (SHSHMode == false)
                {
                    Definitions.iREB_mode = true;
                }
                else
                {
                    Definitions.iREB_mode = false;
                }
                //this.animation.Visible = false;
                this.DFUInstructions_Normal();
            }
            
        }

        public void Search_DFU(object sender, DoWorkEventArgs e)
        {
            if (!modFile.File_Exists(iFaith.temppath + @"\s-irecovery.exe"))
            {
                modFile.SaveToDisk("s-irecovery.exe", iFaith.temppath + @"\s-irecovery.exe");
            }
            iFaith.DFUConnected = false;
            iFaith.Wait_For_DFU();
            iFaith.DFUConnected = true;
        }

       
       

        public void BackgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (this.ProgressBar1.Value != 99)
                {
                    if (iFaith.OhNoesShutDOWN)
                    {
                        return;
                    }
                    this.ProgressBar1.Invoke(new MethodInvoker(this.Increase));
                    //modProcessCmd.Sleep(45);
                }
                this.ProgressBar1.Invoke(new MethodInvoker(this.one00Percent));
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                ProjectData.ClearProjectError();
            }
        }

        public void Increase()
        {
            if (this.ProgressBar1.Value != 100)
            {
                this.ProgressBar1.Value += 3;
            }
        }

        public void one00Percent()
        {
            this.ProgressBar1.Value = 100;
        }

        public void Center_Prepare()
        {
            //this.Prepare.Left = (int)Math.Round((double)((((double)this.Width) / 2.0) - (((double)this.Prepare.Width) / 2.0)));
        }

        public void GoGoGadgetiFaith()
        {
            this.QuitDFU = true;
            iFaith.DFUConnected = false;
            iFaith.ResetDFUInstructions = true;
            this.iDetector();
            if (modProcessCmd.board == "gtfo")
            {
                modProcessCmd.board = string.Empty;
                iFaith.Hide_Stages();
                this.CleaniREB();
            }
            else
            {
                //MyProject.Forms.MDIMain.TopMost = true;
                //MyProject.Forms.MDIMain.Activate();
                if (Definitions.iREB_mode)
                {
                    //this.PictureBox2.Visible = true;
                    this.ProgressBar1.Visible = true;
                    //this.PictureBox2.BringToFront();
                    this.ProgressBar1.BringToFront();
                    this.BackgroundWorker2.RunWorkerAsync();
                    if (!modFile.File_Exists(iFaith.temppath + @"\s-irecovery.exe"))
                    {
                        modFile.SaveToDisk("s-irecovery.exe", iFaith.temppath + @"\s-irecovery.exe");
                    }
                    iFaith.GoGoGadgetiREB();
                    //MyProject.Forms.MDIMain.TopMost = false;
                    //MessageBox.Show("Your device is now in a PWNED DFU state (black screen).\r\rYou may now launch iTunes and SHIFT + RESTORE\rto the custom *signed* IPSW located on your desktop!");
                    this.CleaniREB();
                }
                else
                {
                    //MyProject.Forms.Run.MdiParent = MyProject.Forms.MDIMain;
                    //
                    this.ProgressBar1.Visible = true;
                    this.ProgressBar1.BringToFront();
                    this.BackgroundWorker1.RunWorkerAsync();
                    this.Prepare.WorkerSupportsCancellation = true;
                    this.Prepare.RunWorkerAsync();
                    //
                    //this.getenvbox.Visible = iFaith.Debug_Mode;
                    //this.shshblob.Visible = iFaith.Debug_Mode;
                    dfuinstructionstxt.ForeColor = Color.DimGray;
                    dfuinstructions.Enabled = false;
                    //MyProject.Forms.MDIMain.Text = "iFaith v" + MyProject.Forms.MDIMain.VersionNumber + " -- By: iH8sn0w -- [" + modProcessCmd.iDevice + "]";
                    Prepbtn.Enabled = true;
                    Prepbtn.Checked = true;
                    preptxt.ForeColor = Color.Crimson;
                    //this.PictureBox1.Image = My.Resources.Resources.Dove;

                    //this.Finished = new delUpdate(this.UpdateText);
                    this.Timer = new System.Threading.Timer(new TimerCallback(this.Tick), null, 1000, 1000);
                    this.Finished_shsh = new delUpdate(this.UpdateText_shsh);
                    
                    //MyProject.Forms.Run.Show();
                    //this.Dispose();
                }
            }
        }

        public void update_1(object sender, DataReceivedEventArgs e)
        {
            this.UpdateTextBox(e.Data);
        }

        private void UpdateText()
        {
            this.idetect.Text = this.Results;
        }

        private void UpdateTextBox(string Tex)
        {
            if (this.InvokeRequired)
            {
                UpdateTextBoxDelegate method = new UpdateTextBoxDelegate(this.UpdateTextBox);
                object[] args = new object[] { Tex };
                this.Invoke(method, args);
            }
            else
            {
                RichTextBox idetect = this.idetect;
                idetect.Text = idetect.Text + Tex;
                this.idetect.Text = this.idetect.Text.Replace(" ", "");
            }
        }

        public void iDetector()
        {
            try
            {
                this.idetect.Text = string.Empty;
                Process process = new Process();
                process.StartInfo.FileName = iFaith.temppath + @"\s-irecovery.exe";
                process.StartInfo.Arguments = "-detect";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardError = true;                
                process.OutputDataReceived += new DataReceivedEventHandler(this.update_1);
                process.Start();
                StreamWriter standardInput = process.StandardInput;
                process.BeginOutputReadLine();
                modProcessCmd.Delay(2.0);
                while (!process.HasExited)
                {
                    modProcessCmd.Delay(1.0);
                }
                if (this.idetect.Text.Contains("n88ap"))
                {
                    modProcessCmd.iDevice = "iPhone 3GS";
                    modProcessCmd.board = "n88ap";
                }
                else if (this.idetect.Text.Contains("n72ap"))
                {
                    modProcessCmd.iDevice = "iPod Touch 2G";
                    modProcessCmd.board = "n72ap";
                }
                else if (this.idetect.Text.Contains("n18ap"))
                {
                    modProcessCmd.iDevice = "iPod Touch 3G";
                    modProcessCmd.board = "n18ap";
                }
                else if (this.idetect.Text.Contains("n81ap"))
                {
                    modProcessCmd.iDevice = "iPod Touch 4";
                    modProcessCmd.board = "n81ap";
                }
                else if (this.idetect.Text.Contains("n90ap"))
                {
                    modProcessCmd.iDevice = "iPhone 4";
                    modProcessCmd.board = "n90ap";
                }
                else if (this.idetect.Text.Contains("n92ap"))
                {
                    modProcessCmd.iDevice = "iPhone 4";
                    modProcessCmd.board = "n92ap";
                }
                else if (this.idetect.Text.Contains("n90bap"))
                {
                    modProcessCmd.iDevice = "iPhone 4";
                    modProcessCmd.board = "n90bap";
                }
                else if (this.idetect.Text.Contains("k48ap"))
                {
                    modProcessCmd.iDevice = "iPad 1";
                    modProcessCmd.board = "k48ap";
                }
                else if (this.idetect.Text.Contains("k66ap"))
                {
                    modProcessCmd.iDevice = "Apple TV 2";
                    modProcessCmd.board = "k66ap";
                }
                else if (this.idetect.Text.ToLower().Contains("unknown"))
                {
                    Interaction.MsgBox("iFaith has detected an unsupported device!\r\rIf you believe you got this message in error, try again.\r\rA5+ users: Although you cannot dump SHSH blobs from the device,\ryou can still use the 'Show available SHSH blobs on server' button in\rthe main menu to fetch the latest SHSH blobs Apple is signing.\r\rAlthough you may not be able to use them at the moment, it is always\rbest to save them for future loopholes/exploits!", MsgBoxStyle.Critical, null);
                    modProcessCmd.board = "gtfo";
                }
                else
                {
                    int chipIDvar = 0;
                    int boardIDvar = 0;
                    string producttypevar = string.Empty;
                    string deviceNamevar = string.Empty;
                    iFaith.giveMeDetails(this.idetect.Text.Replace(" ", ""), ref chipIDvar, ref boardIDvar, ref deviceNamevar, ref producttypevar);
                    if (deviceNamevar == "Unknown")
                    {
                        Interaction.MsgBox("It appears iFaith had difficulty determining your device.\r\rEnsure you are running iFaith as Administrator and try again.", MsgBoxStyle.Critical, null);
                    }
                    else
                    {
                        Interaction.MsgBox("This version of iFaith does not *dump* " + deviceNamevar + " blobs.\r\rAlthough you cannot dump SHSH blobs from the " + deviceNamevar + ",\ryou can still use the 'Show available SHSH blobs on server' button in\rthe main menu to fetch the latest SHSH blobs Apple is signing.\r\rAlthough you may not be able to use them at the moment, it is always\rbest to save them for future loopholes/exploits!", MsgBoxStyle.Critical, null);
                    }
                    modProcessCmd.board = "gtfo";
                }
            }
            catch (ManagementException exception1)
            {
                ProjectData.SetProjectError(exception1);
                ManagementException exception = exception1;
                ProjectData.ClearProjectError();
            }
        }

        public void CleaniREB()
        {
            dfuinstructionsLabel.Visible = false;
            dfuinstructionstxt.Visible = false;
            blue.Visible = false;
            buttonStartDFU.Enabled = true;
            buttonRestartDFU.Enabled = false;
            if (BackgroundWorker1!=null)
            {
                this.BackgroundWorker1.Dispose();
            }
            if (BackgroundWorker2 != null)
            {
                this.BackgroundWorker2.Dispose();
            }          
        }
        
        public void DFUInstructions_ATV2()
        {
            this.BackgroundWorker1 = new BackgroundWorker();
            this.BackgroundWorker1.WorkerSupportsCancellation = true;
            this.BackgroundWorker1.RunWorkerAsync();
            if (this.QuitDFU)
            {
                iFaith.ResetDFUInstructions = false;
                this.QuitDFU = false;
            }
            else
            {
                this.PrepareTXT.Visible = true;
                this.PrepareTXT.Text = "Prepare to press && hold Menu + Play...";
                //this.Center_Prepare();
                this.labelTimer1.Visible = true;
                this.labelTimer1.ForeColor = Color.Crimson;
                this.labelTimer1.Text = "5";
                if (iFaith.ResetDFUInstructions)
                {
                    iFaith.ResetDFUInstructions = false;
                    this.DFUInstructions_ATV2();
                }
                else
                {
                    if (iFaith.DFUConnected)
                    {
                        this.GoGoGadgetiFaith();
                    }
                    modProcessCmd.Delay(1.0);
                    this.labelTimer1.Text = "4";
                    if (iFaith.DFUConnected)
                    {
                        this.GoGoGadgetiFaith();
                    }
                    if (iFaith.ResetDFUInstructions)
                    {
                        iFaith.ResetDFUInstructions = false;
                        this.DFUInstructions_ATV2();
                    }
                    else
                    {
                        modProcessCmd.Delay(1.0);
                        this.labelTimer1.Text = "3";
                        if (iFaith.DFUConnected)
                        {
                            this.GoGoGadgetiFaith();
                        }
                        if (iFaith.ResetDFUInstructions)
                        {
                            iFaith.ResetDFUInstructions = false;
                            this.DFUInstructions_ATV2();
                        }
                        else
                        {
                            modProcessCmd.Delay(1.0);
                            this.labelTimer1.Text = "2";
                            if (iFaith.DFUConnected)
                            {
                                this.GoGoGadgetiFaith();
                            }
                            if (iFaith.ResetDFUInstructions)
                            {
                                iFaith.ResetDFUInstructions = false;
                                this.DFUInstructions_ATV2();
                            }
                            else
                            {
                                modProcessCmd.Delay(1.0);
                                this.labelTimer1.Text = "1";
                                if (iFaith.DFUConnected)
                                {
                                    this.GoGoGadgetiFaith();
                                }
                                if (iFaith.ResetDFUInstructions)
                                {
                                    iFaith.ResetDFUInstructions = false;
                                    this.DFUInstructions_ATV2();
                                }
                                else
                                {
                                    modProcessCmd.Delay(1.0);
                                    this.labelTimer1.ForeColor = Color.Crimson;
                                    this.PrepareTXT.Text = "Prepare to release in (5).";
                                    //this.Center_Prepare();
                                    this.dfuinstructionsLabel.Text = "Point to your Apple TV 2 && hold Menu + Play!!";
                                    this.dfuinstructionsLabel.Visible = true;
                                    this.Center_DFUInstructions();
                                    //this.atv2.Image = iFaith.My.Resources.Resources.remote_both;
                                    this.labelTimer1.Text = "5";
                                    if (iFaith.DFUConnected)
                                    {
                                        this.GoGoGadgetiFaith();
                                    }
                                    if (iFaith.ResetDFUInstructions)
                                    {
                                        iFaith.ResetDFUInstructions = false;
                                        this.DFUInstructions_ATV2();
                                    }
                                    else
                                    {
                                        modProcessCmd.Delay(1.0);
                                        this.PrepareTXT.Text = "Prepare to release in (4).";
                                        this.labelTimer1.Text = "4";
                                        if (iFaith.DFUConnected)
                                        {
                                            this.GoGoGadgetiFaith();
                                        }
                                        if (iFaith.ResetDFUInstructions)
                                        {
                                            iFaith.ResetDFUInstructions = false;
                                            this.DFUInstructions_ATV2();
                                        }
                                        else
                                        {
                                            modProcessCmd.Delay(1.0);
                                            this.PrepareTXT.Text = "Prepare to release in (3).";
                                            this.labelTimer1.Text = "3";
                                            if (iFaith.DFUConnected)
                                            {
                                                this.GoGoGadgetiFaith();
                                            }
                                            if (iFaith.ResetDFUInstructions)
                                            {
                                                iFaith.ResetDFUInstructions = false;
                                                this.DFUInstructions_ATV2();
                                            }
                                            else
                                            {
                                                modProcessCmd.Delay(1.0);
                                                this.PrepareTXT.Text = "Prepare to release in (2).";
                                                this.labelTimer1.Text = "2";
                                                if (iFaith.DFUConnected)
                                                {
                                                    this.GoGoGadgetiFaith();
                                                }
                                                if (iFaith.ResetDFUInstructions)
                                                {
                                                    iFaith.ResetDFUInstructions = false;
                                                    this.DFUInstructions_ATV2();
                                                }
                                                else
                                                {
                                                    modProcessCmd.Delay(1.0);
                                                    this.PrepareTXT.Text = "Prepare to release in (1).";
                                                    this.labelTimer1.Text = "1";
                                                    if (iFaith.DFUConnected)
                                                    {
                                                        this.GoGoGadgetiFaith();
                                                    }
                                                    if (iFaith.ResetDFUInstructions)
                                                    {
                                                        iFaith.ResetDFUInstructions = false;
                                                        this.DFUInstructions_ATV2();
                                                    }
                                                    else
                                                    {
                                                        modProcessCmd.Delay(1.0);
                                                        //this.atv2.Image = iFaith.My.Resources.Resources.remote_self;
                                                        this.PrepareTXT.Visible = false;
                                                        this.dfuinstructionsLabel.Text = "Release Menu + Play! (Waiting for DFU...)";
                                                        this.Center_DFUInstructions();
                                                        this.labelTimer1.ForeColor = Color.Indigo;
                                                        this.labelTimer1.Text = "10";
                                                        if (iFaith.DFUConnected)
                                                        {
                                                            this.GoGoGadgetiFaith();
                                                        }
                                                        if (iFaith.ResetDFUInstructions)
                                                        {
                                                            iFaith.ResetDFUInstructions = false;
                                                            this.DFUInstructions_ATV2();
                                                        }
                                                        else
                                                        {
                                                            modProcessCmd.Delay(1.0);
                                                            this.labelTimer1.Text = "9";
                                                            if (iFaith.DFUConnected)
                                                            {
                                                                this.GoGoGadgetiFaith();
                                                            }
                                                            if (iFaith.ResetDFUInstructions)
                                                            {
                                                                iFaith.ResetDFUInstructions = false;
                                                                this.DFUInstructions_ATV2();
                                                            }
                                                            else
                                                            {
                                                                modProcessCmd.Delay(1.0);
                                                                this.labelTimer1.Text = "8";
                                                                if (iFaith.DFUConnected)
                                                                {
                                                                    this.GoGoGadgetiFaith();
                                                                }
                                                                if (iFaith.ResetDFUInstructions)
                                                                {
                                                                    iFaith.ResetDFUInstructions = false;
                                                                    this.DFUInstructions_ATV2();
                                                                }
                                                                else
                                                                {
                                                                    modProcessCmd.Delay(1.0);
                                                                    this.labelTimer1.Text = "7";
                                                                    if (iFaith.DFUConnected)
                                                                    {
                                                                        this.GoGoGadgetiFaith();
                                                                    }
                                                                    if (iFaith.ResetDFUInstructions)
                                                                    {
                                                                        iFaith.ResetDFUInstructions = false;
                                                                        this.DFUInstructions_ATV2();
                                                                    }
                                                                    else
                                                                    {
                                                                        modProcessCmd.Delay(1.0);
                                                                        this.labelTimer1.Text = "6";
                                                                        if (iFaith.DFUConnected)
                                                                        {
                                                                            this.GoGoGadgetiFaith();
                                                                        }
                                                                        if (iFaith.ResetDFUInstructions)
                                                                        {
                                                                            iFaith.ResetDFUInstructions = false;
                                                                            this.DFUInstructions_ATV2();
                                                                        }
                                                                        else
                                                                        {
                                                                            modProcessCmd.Delay(1.0);
                                                                            this.labelTimer1.Text = "5";
                                                                            if (iFaith.DFUConnected)
                                                                            {
                                                                                this.GoGoGadgetiFaith();
                                                                            }
                                                                            if (iFaith.ResetDFUInstructions)
                                                                            {
                                                                                iFaith.ResetDFUInstructions = false;
                                                                                this.DFUInstructions_ATV2();
                                                                            }
                                                                            else
                                                                            {
                                                                                modProcessCmd.Delay(1.0);
                                                                                this.labelTimer1.Text = "4";
                                                                                if (iFaith.DFUConnected)
                                                                                {
                                                                                    this.GoGoGadgetiFaith();
                                                                                }
                                                                                if (iFaith.ResetDFUInstructions)
                                                                                {
                                                                                    iFaith.ResetDFUInstructions = false;
                                                                                    this.DFUInstructions_ATV2();
                                                                                }
                                                                                else
                                                                                {
                                                                                    modProcessCmd.Delay(1.0);
                                                                                    this.labelTimer1.Text = "3";
                                                                                    if (iFaith.DFUConnected)
                                                                                    {
                                                                                        this.GoGoGadgetiFaith();
                                                                                    }
                                                                                    if (iFaith.ResetDFUInstructions)
                                                                                    {
                                                                                        iFaith.ResetDFUInstructions = false;
                                                                                        this.DFUInstructions_ATV2();
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        modProcessCmd.Delay(1.0);
                                                                                        this.labelTimer1.Text = "2";
                                                                                        if (iFaith.DFUConnected)
                                                                                        {
                                                                                            this.GoGoGadgetiFaith();
                                                                                        }
                                                                                        if (iFaith.ResetDFUInstructions)
                                                                                        {
                                                                                            iFaith.ResetDFUInstructions = false;
                                                                                            this.DFUInstructions_ATV2();
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            modProcessCmd.Delay(1.0);
                                                                                            this.labelTimer1.Text = "1";
                                                                                            if (iFaith.DFUConnected)
                                                                                            {
                                                                                                this.GoGoGadgetiFaith();
                                                                                            }
                                                                                            if (iFaith.ResetDFUInstructions)
                                                                                            {
                                                                                                iFaith.ResetDFUInstructions = false;
                                                                                                this.DFUInstructions_ATV2();
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                this.dfuinstructionsLabel.Visible = false;
                                                                                                //MyProject.Forms.MDIMain.TopMost = false;
                                                                                                this.GoGoGadgetCleanUp();
                                                                                                if (!iFaith.OhNoesShutDOWN && this.Visible)
                                                                                                {
                                                                                                    iFaith.KilliRecovery();
                                                                                                    Interaction.MsgBox("You failed to Enter DFU. Please Try again.", MsgBoxStyle.Critical, null);
                                                                                                    buttonStartDFU.Enabled = true;
                                                                                                    buttonRestartDFU.Enabled = false;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void DFUInstructions_Normal()
        {
            try
            {
                if (this.QuitDFU)
                {
                    iFaith.ResetDFUInstructions = false;
                    this.QuitDFU = false;
                }
                else
                {
                    this.BackgroundWorker1 = new BackgroundWorker();
                    this.BackgroundWorker1.WorkerSupportsCancellation = true;
                    this.BackgroundWorker1.RunWorkerAsync();
                    //this.PictureBox1.Visible = true;
                    //this.PictureBox1.Image = iFaith.My.Resources.Resources.justdevice;
                    this.PrepareTXT.Visible = true;
                    this.PrepareTXT.Text = "Prepare to press && hold Power + Home...";
                    //this.Center_Prepare();
                    this.labelTimer1.Visible = true;
                    this.labelTimer1.ForeColor = Color.Crimson;
                    this.labelTimer1.Text = "5";
                    if (iFaith.ResetDFUInstructions)
                    {
                        iFaith.ResetDFUInstructions = false;
                        this.DFUInstructions_Normal();
                    }
                    else
                    {
                        if (iFaith.DFUConnected)
                        {
                            this.GoGoGadgetiFaith();
                        }
                        modProcessCmd.Delay(1.0);
                        this.labelTimer1.Text = "4";
                        if (iFaith.DFUConnected)
                        {
                            this.GoGoGadgetiFaith();
                        }
                        if (iFaith.ResetDFUInstructions)
                        {
                            iFaith.ResetDFUInstructions = false;
                            this.DFUInstructions_Normal();
                        }
                        else
                        {
                            modProcessCmd.Delay(1.0);
                            this.labelTimer1.Text = "3";
                            if (iFaith.DFUConnected)
                            {
                                this.GoGoGadgetiFaith();
                            }
                            if (iFaith.ResetDFUInstructions)
                            {
                                iFaith.ResetDFUInstructions = false;
                                this.DFUInstructions_Normal();
                            }
                            else
                            {
                                modProcessCmd.Delay(1.0);
                                this.labelTimer1.Text = "2";
                                if (iFaith.DFUConnected)
                                {
                                    this.GoGoGadgetiFaith();
                                }
                                if (iFaith.ResetDFUInstructions)
                                {
                                    iFaith.ResetDFUInstructions = false;
                                    this.DFUInstructions_Normal();
                                }
                                else
                                {
                                    modProcessCmd.Delay(1.0);
                                    this.labelTimer1.Text = "1";
                                    if (iFaith.DFUConnected)
                                    {
                                        this.GoGoGadgetiFaith();
                                    }
                                    if (iFaith.ResetDFUInstructions)
                                    {
                                        iFaith.ResetDFUInstructions = false;
                                        this.DFUInstructions_Normal();
                                    }
                                    else
                                    {
                                        modProcessCmd.Delay(1.0);
                                        this.labelTimer1.ForeColor = Color.Indigo;
                                        this.PrepareTXT.Visible = false;
                                        this.dfuinstructionsLabel.Text = "Press && hold Power + Home!";
                                        this.dfuinstructionsLabel.Visible = true;
                                        this.Center_DFUInstructions();
                                        //this.PictureBox1.Image = iFaith.My.Resources.Resources.powerNhome;
                                        this.labelTimer1.Text = "10";
                                        if (iFaith.DFUConnected)
                                        {
                                            this.GoGoGadgetiFaith();
                                        }
                                        if (iFaith.ResetDFUInstructions)
                                        {
                                            iFaith.ResetDFUInstructions = false;
                                            this.DFUInstructions_Normal();
                                        }
                                        else
                                        {
                                            modProcessCmd.Delay(1.0);
                                            this.labelTimer1.Text = "9";
                                            if (iFaith.DFUConnected)
                                            {
                                                this.GoGoGadgetiFaith();
                                            }
                                            if (iFaith.ResetDFUInstructions)
                                            {
                                                iFaith.ResetDFUInstructions = false;
                                                this.DFUInstructions_Normal();
                                            }
                                            else
                                            {
                                                modProcessCmd.Delay(1.0);
                                                this.labelTimer1.Text = "8";
                                                if (iFaith.DFUConnected)
                                                {
                                                    this.GoGoGadgetiFaith();
                                                }
                                                if (iFaith.ResetDFUInstructions)
                                                {
                                                    iFaith.ResetDFUInstructions = false;
                                                    this.DFUInstructions_Normal();
                                                }
                                                else
                                                {
                                                    modProcessCmd.Delay(1.0);
                                                    this.labelTimer1.Text = "7";
                                                    if (iFaith.DFUConnected)
                                                    {
                                                        this.GoGoGadgetiFaith();
                                                    }
                                                    if (iFaith.ResetDFUInstructions)
                                                    {
                                                        iFaith.ResetDFUInstructions = false;
                                                        this.DFUInstructions_Normal();
                                                    }
                                                    else
                                                    {
                                                        modProcessCmd.Delay(1.0);
                                                        this.labelTimer1.Text = "6";
                                                        if (iFaith.DFUConnected)
                                                        {
                                                            this.GoGoGadgetiFaith();
                                                        }
                                                        if (iFaith.ResetDFUInstructions)
                                                        {
                                                            iFaith.ResetDFUInstructions = false;
                                                            this.DFUInstructions_Normal();
                                                        }
                                                        else
                                                        {
                                                            modProcessCmd.Delay(1.0);
                                                            this.PrepareTXT.Visible = true;
                                                            this.PrepareTXT.Text = "Prepare to release the Power button (5)";
                                                            //this.Center_Prepare();
                                                            this.labelTimer1.ForeColor = Color.Crimson;
                                                            if (iFaith.DFUConnected)
                                                            {
                                                                this.GoGoGadgetiFaith();
                                                            }
                                                            if (iFaith.ResetDFUInstructions)
                                                            {
                                                                iFaith.ResetDFUInstructions = false;
                                                                this.DFUInstructions_Normal();
                                                            }
                                                            else
                                                            {
                                                                this.labelTimer1.Text = "5";
                                                                modProcessCmd.Delay(1.0);
                                                                this.PrepareTXT.Text = "Prepare to release the Power button (4)";
                                                                this.labelTimer1.Text = "4";
                                                                if (iFaith.DFUConnected)
                                                                {
                                                                    this.GoGoGadgetiFaith();
                                                                }
                                                                if (iFaith.ResetDFUInstructions)
                                                                {
                                                                    iFaith.ResetDFUInstructions = false;
                                                                    this.DFUInstructions_Normal();
                                                                }
                                                                else
                                                                {
                                                                    modProcessCmd.Delay(1.0);
                                                                    this.PrepareTXT.Text = "Prepare to release the Power button (3)";
                                                                    this.labelTimer1.Text = "3";
                                                                    if (iFaith.DFUConnected)
                                                                    {
                                                                        this.GoGoGadgetiFaith();
                                                                    }
                                                                    if (iFaith.ResetDFUInstructions)
                                                                    {
                                                                        iFaith.ResetDFUInstructions = false;
                                                                        this.DFUInstructions_Normal();
                                                                    }
                                                                    else
                                                                    {
                                                                        modProcessCmd.Delay(1.0);
                                                                        this.PrepareTXT.Text = "Prepare to release the Power button (2)";
                                                                        this.labelTimer1.Text = "2";
                                                                        if (iFaith.DFUConnected)
                                                                        {
                                                                            this.GoGoGadgetiFaith();
                                                                        }
                                                                        if (iFaith.ResetDFUInstructions)
                                                                        {
                                                                            iFaith.ResetDFUInstructions = false;
                                                                            this.DFUInstructions_Normal();
                                                                        }
                                                                        else
                                                                        {
                                                                            modProcessCmd.Delay(1.0);
                                                                            this.PrepareTXT.Text = "Prepare to release the Power button (1)";
                                                                            this.labelTimer1.Text = "1";
                                                                            if (iFaith.DFUConnected)
                                                                            {
                                                                                this.GoGoGadgetiFaith();
                                                                            }
                                                                            if (iFaith.ResetDFUInstructions)
                                                                            {
                                                                                iFaith.ResetDFUInstructions = false;
                                                                                this.DFUInstructions_Normal();
                                                                            }
                                                                            else
                                                                            {
                                                                                modProcessCmd.Delay(1.0);
                                                                                this.PrepareTXT.Visible = false;
                                                                                //this.PictureBox1.Image = iFaith.My.Resources.Resources.homebutton;
                                                                                this.dfuinstructionsLabel.Text = "Release the power button, keep holding home!";
                                                                                this.Center_DFUInstructions();
                                                                                this.labelTimer1.ForeColor = Color.Indigo;
                                                                                this.labelTimer1.Text = "30";
                                                                                if (iFaith.DFUConnected)
                                                                                {
                                                                                    this.GoGoGadgetiFaith();
                                                                                }
                                                                                if (iFaith.ResetDFUInstructions)
                                                                                {
                                                                                    iFaith.ResetDFUInstructions = false;
                                                                                    this.DFUInstructions_Normal();
                                                                                }
                                                                                else
                                                                                {
                                                                                    modProcessCmd.Delay(1.0);
                                                                                    this.labelTimer1.Text = "29";
                                                                                    if (iFaith.DFUConnected)
                                                                                    {
                                                                                        this.GoGoGadgetiFaith();
                                                                                    }
                                                                                    if (iFaith.ResetDFUInstructions)
                                                                                    {
                                                                                        iFaith.ResetDFUInstructions = false;
                                                                                        this.DFUInstructions_Normal();
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        modProcessCmd.Delay(1.0);
                                                                                        this.labelTimer1.Text = "28";
                                                                                        if (iFaith.DFUConnected)
                                                                                        {
                                                                                            this.GoGoGadgetiFaith();
                                                                                        }
                                                                                        if (iFaith.ResetDFUInstructions)
                                                                                        {
                                                                                            iFaith.ResetDFUInstructions = false;
                                                                                            this.DFUInstructions_Normal();
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            modProcessCmd.Delay(1.0);
                                                                                            this.labelTimer1.Text = "27";
                                                                                            if (iFaith.DFUConnected)
                                                                                            {
                                                                                                this.GoGoGadgetiFaith();
                                                                                            }
                                                                                            if (iFaith.ResetDFUInstructions)
                                                                                            {
                                                                                                iFaith.ResetDFUInstructions = false;
                                                                                                this.DFUInstructions_Normal();
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                modProcessCmd.Delay(1.0);
                                                                                                this.labelTimer1.Text = "26";
                                                                                                if (iFaith.DFUConnected)
                                                                                                {
                                                                                                    this.GoGoGadgetiFaith();
                                                                                                }
                                                                                                if (iFaith.ResetDFUInstructions)
                                                                                                {
                                                                                                    iFaith.ResetDFUInstructions = false;
                                                                                                    this.DFUInstructions_Normal();
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    modProcessCmd.Delay(1.0);
                                                                                                    this.labelTimer1.Text = "25";
                                                                                                    if (iFaith.DFUConnected)
                                                                                                    {
                                                                                                        this.GoGoGadgetiFaith();
                                                                                                    }
                                                                                                    if (iFaith.ResetDFUInstructions)
                                                                                                    {
                                                                                                        iFaith.ResetDFUInstructions = false;
                                                                                                        this.DFUInstructions_Normal();
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        modProcessCmd.Delay(1.0);
                                                                                                        this.labelTimer1.Text = "24";
                                                                                                        if (iFaith.DFUConnected)
                                                                                                        {
                                                                                                            this.GoGoGadgetiFaith();
                                                                                                        }
                                                                                                        if (iFaith.ResetDFUInstructions)
                                                                                                        {
                                                                                                            iFaith.ResetDFUInstructions = false;
                                                                                                            this.DFUInstructions_Normal();
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            modProcessCmd.Delay(1.0);
                                                                                                            this.labelTimer1.Text = "23";
                                                                                                            if (iFaith.DFUConnected)
                                                                                                            {
                                                                                                                this.GoGoGadgetiFaith();
                                                                                                            }
                                                                                                            if (iFaith.ResetDFUInstructions)
                                                                                                            {
                                                                                                                iFaith.ResetDFUInstructions = false;
                                                                                                                this.DFUInstructions_Normal();
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                modProcessCmd.Delay(1.0);
                                                                                                                this.labelTimer1.Text = "22";
                                                                                                                if (iFaith.DFUConnected)
                                                                                                                {
                                                                                                                    this.GoGoGadgetiFaith();
                                                                                                                }
                                                                                                                if (iFaith.ResetDFUInstructions)
                                                                                                                {
                                                                                                                    iFaith.ResetDFUInstructions = false;
                                                                                                                    this.DFUInstructions_Normal();
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    modProcessCmd.Delay(1.0);
                                                                                                                    this.labelTimer1.Text = "21";
                                                                                                                    if (iFaith.DFUConnected)
                                                                                                                    {
                                                                                                                        this.GoGoGadgetiFaith();
                                                                                                                    }
                                                                                                                    if (iFaith.ResetDFUInstructions)
                                                                                                                    {
                                                                                                                        iFaith.ResetDFUInstructions = false;
                                                                                                                        this.DFUInstructions_Normal();
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        modProcessCmd.Delay(1.0);
                                                                                                                        this.labelTimer1.Text = "20";
                                                                                                                        if (iFaith.DFUConnected)
                                                                                                                        {
                                                                                                                            this.GoGoGadgetiFaith();
                                                                                                                        }
                                                                                                                        if (iFaith.ResetDFUInstructions)
                                                                                                                        {
                                                                                                                            iFaith.ResetDFUInstructions = false;
                                                                                                                            this.DFUInstructions_Normal();
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            modProcessCmd.Delay(1.0);
                                                                                                                            this.labelTimer1.Text = "19";
                                                                                                                            if (iFaith.DFUConnected)
                                                                                                                            {
                                                                                                                                this.GoGoGadgetiFaith();
                                                                                                                            }
                                                                                                                            if (iFaith.ResetDFUInstructions)
                                                                                                                            {
                                                                                                                                iFaith.ResetDFUInstructions = false;
                                                                                                                                this.DFUInstructions_Normal();
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                modProcessCmd.Delay(1.0);
                                                                                                                                this.labelTimer1.Text = "18";
                                                                                                                                if (iFaith.DFUConnected)
                                                                                                                                {
                                                                                                                                    this.GoGoGadgetiFaith();
                                                                                                                                }
                                                                                                                                if (iFaith.ResetDFUInstructions)
                                                                                                                                {
                                                                                                                                    iFaith.ResetDFUInstructions = false;
                                                                                                                                    this.DFUInstructions_Normal();
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    modProcessCmd.Delay(1.0);
                                                                                                                                    this.labelTimer1.Text = "17";
                                                                                                                                    if (iFaith.DFUConnected)
                                                                                                                                    {
                                                                                                                                        this.GoGoGadgetiFaith();
                                                                                                                                    }
                                                                                                                                    if (iFaith.ResetDFUInstructions)
                                                                                                                                    {
                                                                                                                                        iFaith.ResetDFUInstructions = false;
                                                                                                                                        this.DFUInstructions_Normal();
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        modProcessCmd.Delay(1.0);
                                                                                                                                        this.labelTimer1.Text = "16";
                                                                                                                                        if (iFaith.DFUConnected)
                                                                                                                                        {
                                                                                                                                            this.GoGoGadgetiFaith();
                                                                                                                                        }
                                                                                                                                        if (iFaith.ResetDFUInstructions)
                                                                                                                                        {
                                                                                                                                            iFaith.ResetDFUInstructions = false;
                                                                                                                                            this.DFUInstructions_Normal();
                                                                                                                                        }
                                                                                                                                        else
                                                                                                                                        {
                                                                                                                                            modProcessCmd.Delay(1.0);
                                                                                                                                            this.labelTimer1.Text = "15";
                                                                                                                                            if (iFaith.DFUConnected)
                                                                                                                                            {
                                                                                                                                                this.GoGoGadgetiFaith();
                                                                                                                                            }
                                                                                                                                            if (iFaith.ResetDFUInstructions)
                                                                                                                                            {
                                                                                                                                                iFaith.ResetDFUInstructions = false;
                                                                                                                                                this.DFUInstructions_Normal();
                                                                                                                                            }
                                                                                                                                            else
                                                                                                                                            {
                                                                                                                                                modProcessCmd.Delay(1.0);
                                                                                                                                                this.labelTimer1.Text = "14";
                                                                                                                                                if (iFaith.DFUConnected)
                                                                                                                                                {
                                                                                                                                                    this.GoGoGadgetiFaith();
                                                                                                                                                }
                                                                                                                                                if (iFaith.ResetDFUInstructions)
                                                                                                                                                {
                                                                                                                                                    iFaith.ResetDFUInstructions = false;
                                                                                                                                                    this.DFUInstructions_Normal();
                                                                                                                                                }
                                                                                                                                                else
                                                                                                                                                {
                                                                                                                                                    modProcessCmd.Delay(1.0);
                                                                                                                                                    this.labelTimer1.Text = "13";
                                                                                                                                                    if (iFaith.DFUConnected)
                                                                                                                                                    {
                                                                                                                                                        this.GoGoGadgetiFaith();
                                                                                                                                                    }
                                                                                                                                                    if (iFaith.ResetDFUInstructions)
                                                                                                                                                    {
                                                                                                                                                        iFaith.ResetDFUInstructions = false;
                                                                                                                                                        this.DFUInstructions_Normal();
                                                                                                                                                    }
                                                                                                                                                    else
                                                                                                                                                    {
                                                                                                                                                        modProcessCmd.Delay(1.0);
                                                                                                                                                        this.labelTimer1.Text = "12";
                                                                                                                                                        if (iFaith.DFUConnected)
                                                                                                                                                        {
                                                                                                                                                            this.GoGoGadgetiFaith();
                                                                                                                                                        }
                                                                                                                                                        if (iFaith.ResetDFUInstructions)
                                                                                                                                                        {
                                                                                                                                                            iFaith.ResetDFUInstructions = false;
                                                                                                                                                            this.DFUInstructions_Normal();
                                                                                                                                                        }
                                                                                                                                                        else
                                                                                                                                                        {
                                                                                                                                                            modProcessCmd.Delay(1.0);
                                                                                                                                                            this.labelTimer1.Text = "11";
                                                                                                                                                            if (iFaith.DFUConnected)
                                                                                                                                                            {
                                                                                                                                                                this.GoGoGadgetiFaith();
                                                                                                                                                            }
                                                                                                                                                            if (iFaith.ResetDFUInstructions)
                                                                                                                                                            {
                                                                                                                                                                iFaith.ResetDFUInstructions = false;
                                                                                                                                                                this.DFUInstructions_Normal();
                                                                                                                                                            }
                                                                                                                                                            else
                                                                                                                                                            {
                                                                                                                                                                modProcessCmd.Delay(1.0);
                                                                                                                                                                this.labelTimer1.Text = "10";
                                                                                                                                                                if (iFaith.DFUConnected)
                                                                                                                                                                {
                                                                                                                                                                    this.GoGoGadgetiFaith();
                                                                                                                                                                }
                                                                                                                                                                if (iFaith.ResetDFUInstructions)
                                                                                                                                                                {
                                                                                                                                                                    iFaith.ResetDFUInstructions = false;
                                                                                                                                                                    this.DFUInstructions_Normal();
                                                                                                                                                                }
                                                                                                                                                                else
                                                                                                                                                                {
                                                                                                                                                                    modProcessCmd.Delay(1.0);
                                                                                                                                                                    this.labelTimer1.Text = "9";
                                                                                                                                                                    if (iFaith.DFUConnected)
                                                                                                                                                                    {
                                                                                                                                                                        this.GoGoGadgetiFaith();
                                                                                                                                                                    }
                                                                                                                                                                    if (iFaith.ResetDFUInstructions)
                                                                                                                                                                    {
                                                                                                                                                                        iFaith.ResetDFUInstructions = false;
                                                                                                                                                                        this.DFUInstructions_Normal();
                                                                                                                                                                    }
                                                                                                                                                                    else
                                                                                                                                                                    {
                                                                                                                                                                        modProcessCmd.Delay(1.0);
                                                                                                                                                                        this.labelTimer1.Text = "8";
                                                                                                                                                                        if (iFaith.DFUConnected)
                                                                                                                                                                        {
                                                                                                                                                                            this.GoGoGadgetiFaith();
                                                                                                                                                                        }
                                                                                                                                                                        if (iFaith.ResetDFUInstructions)
                                                                                                                                                                        {
                                                                                                                                                                            iFaith.ResetDFUInstructions = false;
                                                                                                                                                                            this.DFUInstructions_Normal();
                                                                                                                                                                        }
                                                                                                                                                                        else
                                                                                                                                                                        {
                                                                                                                                                                            modProcessCmd.Delay(1.0);
                                                                                                                                                                            this.labelTimer1.Text = "7";
                                                                                                                                                                            if (iFaith.DFUConnected)
                                                                                                                                                                            {
                                                                                                                                                                                this.GoGoGadgetiFaith();
                                                                                                                                                                            }
                                                                                                                                                                            if (iFaith.ResetDFUInstructions)
                                                                                                                                                                            {
                                                                                                                                                                                iFaith.ResetDFUInstructions = false;
                                                                                                                                                                                this.DFUInstructions_Normal();
                                                                                                                                                                            }
                                                                                                                                                                            else
                                                                                                                                                                            {
                                                                                                                                                                                modProcessCmd.Delay(1.0);
                                                                                                                                                                                this.labelTimer1.Text = "6";
                                                                                                                                                                                if (iFaith.DFUConnected)
                                                                                                                                                                                {
                                                                                                                                                                                    this.GoGoGadgetiFaith();
                                                                                                                                                                                }
                                                                                                                                                                                if (iFaith.ResetDFUInstructions)
                                                                                                                                                                                {
                                                                                                                                                                                    iFaith.ResetDFUInstructions = false;
                                                                                                                                                                                    this.DFUInstructions_Normal();
                                                                                                                                                                                }
                                                                                                                                                                                else
                                                                                                                                                                                {
                                                                                                                                                                                    modProcessCmd.Delay(1.0);
                                                                                                                                                                                    this.labelTimer1.Text = "5";
                                                                                                                                                                                    if (iFaith.DFUConnected)
                                                                                                                                                                                    {
                                                                                                                                                                                        this.GoGoGadgetiFaith();
                                                                                                                                                                                    }
                                                                                                                                                                                    if (iFaith.ResetDFUInstructions)
                                                                                                                                                                                    {
                                                                                                                                                                                        this.DFUInstructions_Normal();
                                                                                                                                                                                    }
                                                                                                                                                                                    else
                                                                                                                                                                                    {
                                                                                                                                                                                        modProcessCmd.Delay(1.0);
                                                                                                                                                                                        this.labelTimer1.Text = "4";
                                                                                                                                                                                        if (iFaith.DFUConnected)
                                                                                                                                                                                        {
                                                                                                                                                                                            this.GoGoGadgetiFaith();
                                                                                                                                                                                        }
                                                                                                                                                                                        if (iFaith.ResetDFUInstructions)
                                                                                                                                                                                        {
                                                                                                                                                                                            iFaith.ResetDFUInstructions = false;
                                                                                                                                                                                            this.DFUInstructions_Normal();
                                                                                                                                                                                        }
                                                                                                                                                                                        else
                                                                                                                                                                                        {
                                                                                                                                                                                            modProcessCmd.Delay(1.0);
                                                                                                                                                                                            this.labelTimer1.Text = "3";
                                                                                                                                                                                            if (iFaith.DFUConnected)
                                                                                                                                                                                            {
                                                                                                                                                                                                this.GoGoGadgetiFaith();
                                                                                                                                                                                            }
                                                                                                                                                                                            if (iFaith.ResetDFUInstructions)
                                                                                                                                                                                            {
                                                                                                                                                                                                iFaith.ResetDFUInstructions = false;
                                                                                                                                                                                                this.DFUInstructions_Normal();
                                                                                                                                                                                            }
                                                                                                                                                                                            else
                                                                                                                                                                                            {
                                                                                                                                                                                                modProcessCmd.Delay(1.0);
                                                                                                                                                                                                this.labelTimer1.Text = "2";
                                                                                                                                                                                                if (iFaith.DFUConnected)
                                                                                                                                                                                                {
                                                                                                                                                                                                    this.GoGoGadgetiFaith();
                                                                                                                                                                                                }
                                                                                                                                                                                                if (iFaith.ResetDFUInstructions)
                                                                                                                                                                                                {
                                                                                                                                                                                                    iFaith.ResetDFUInstructions = false;
                                                                                                                                                                                                    this.DFUInstructions_Normal();
                                                                                                                                                                                                }
                                                                                                                                                                                                else
                                                                                                                                                                                                {
                                                                                                                                                                                                    modProcessCmd.Delay(1.0);
                                                                                                                                                                                                    this.labelTimer1.Text = "1";
                                                                                                                                                                                                    if (iFaith.DFUConnected)
                                                                                                                                                                                                    {
                                                                                                                                                                                                        this.GoGoGadgetiFaith();
                                                                                                                                                                                                    }
                                                                                                                                                                                                    if (iFaith.ResetDFUInstructions)
                                                                                                                                                                                                    {
                                                                                                                                                                                                        iFaith.ResetDFUInstructions = false;
                                                                                                                                                                                                        this.DFUInstructions_Normal();
                                                                                                                                                                                                    }
                                                                                                                                                                                                    else
                                                                                                                                                                                                    {
                                                                                                                                                                                                        modProcessCmd.Delay(1.0);
                                                                                                                                                                                                        this.GoGoGadgetCleanUp();
                                                                                                                                                                                                        this.dfuinstructionsLabel.Visible = false;
                                                                                                                                                                                                        //MyProject.Forms.MDIMain.TopMost = false;
                                                                                                                                                                                                        iFaith.KilliRecovery();
                                                                                                                                                                                                        if (!iFaith.OhNoesShutDOWN && this.Visible)
                                                                                                                                                                                                        {
                                                                                                                                                                                                            Interaction.MsgBox("You failed to Enter DFU. Please Try again.", MsgBoxStyle.Critical, null);
                                                                                                                                                                                                            buttonStartDFU.Enabled = true;
                                                                                                                                                                                                            buttonRestartDFU.Enabled = false;
                                                                                                                                                                                                        }
                                                                                                                                                                                                    }
                                                                                                                                                                                                }
                                                                                                                                                                                            }
                                                                                                                                                                                        }
                                                                                                                                                                                    }
                                                                                                                                                                                }
                                                                                                                                                                            }
                                                                                                                                                                        }
                                                                                                                                                                    }
                                                                                                                                                                }
                                                                                                                                                            }
                                                                                                                                                        }
                                                                                                                                                    }
                                                                                                                                                }
                                                                                                                                            }
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                }
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                ProjectData.ClearProjectError();
            }
        }

        public void Center_DFUInstructions()
        {
            //this.dfuinstructionsLabel.Left = (int)Math.Round((double)((((double)this.Width) / 2.0) - (((double)this.dfuinstructionsLabel.Width) / 2.0)));
        }

        public void GoGoGadgetCleanUp()
        {
            if (modProcessCmd.iDevice == "Apple TV 2" || appletvMode == true)
            {
                /*this.atv2animation.Visible = true;
                this.atv2warn.Visible = true;
                this.atv2.Visible = false;*/
                this.dfuinstructionsLabel.Visible = false;
                this.dfuinstructionstxt.Visible = true;
                this.Label1.Visible = true;
                //this.dfuinstructionstxt.Text = "Press Start only when your Apple TV 2 is blinking.";
                this.Label1.Text = "Plug-in your Apple TV 2 via USB.";
                this.dfuinstructionstxt.Text = "Press Start only when your Apple TV 2 is blinking.";
            }
            else
            {
                /*this.PictureBox1.Visible = false;
                this.animation.Visible = true;*/
                this.Label1.Visible = true;
                this.Label1.Text = "Please power off your iDevice.Then press \"Start\".";
            }
            
            this.buttonStartDFU.Enabled = true;
            this.buttonRestartDFU.Enabled = false;
            this.labelTimer1.Visible = false;
            if (BackgroundWorker1!=null)
            {
                this.BackgroundWorker1.Dispose();
            }
            
        }


        private delegate void delUpdate();

        private delegate void UpdateTextBoxDelegate(string Text);

        internal virtual BackgroundWorker BackgroundWorker1
        {
            get
            {
                return this._BackgroundWorker1;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                DoWorkEventHandler handler = new DoWorkEventHandler(this.Search_DFU);
                if (this._BackgroundWorker1 != null)
                {
                    this._BackgroundWorker1.DoWork -= handler;
                }
                this._BackgroundWorker1 = value;
                if (this._BackgroundWorker1 != null)
                {
                    this._BackgroundWorker1.DoWork += handler;
                }
            }
        }

        internal virtual BackgroundWorker Prepare
        {
            get
            {
                return this._Prepare;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                DoWorkEventHandler handler = new DoWorkEventHandler(this.Prepare_DoWork_1);
                //this.Status.Invoke(new MethodInvoker(this.PrepareDoWork));
                if (this._Prepare != null)
                {
                    this._Prepare.DoWork -= handler;
                }
                this._Prepare = value;
                if (this._Prepare != null)
                {
                    this._Prepare.DoWork += handler;
                }
            }
        }


        private void BackgroundWorker2_DoWork_1(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (this.ProgressBar1.Value != 99)
                {
                    if (iFaith.OhNoesShutDOWN)
                    {
                        return;
                    }
                    this.ProgressBar1.Invoke(new MethodInvoker(this.Increase));
                    //modProcessCmd.Sleep(45);
                }
                this.ProgressBar1.Invoke(new MethodInvoker(this.one00Percent));
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                ProjectData.ClearProjectError();
            }
        }

        private void buttonRestartDFU_Click(object sender, EventArgs e)
        {
            iFaith.ResetDFUInstructions = true;
        }

        private void DFU_Load(object sender, EventArgs e)
        {
           /* MDIMain objMain = new MDIMain();*/
            this.dfuinstructions.Enabled = true;
            this.dfuinstructions.Checked = true;
            //Point point = new Point(160, 0);
            //this.Location = point;
            if (modProcessCmd.iDevice == "Apple TV 2")
            {
                //this.animation.Visible = false;
                //this.atv2animation.Visible = true;
                //this.atv2warn.Visible = true;
                this.Label1.Text = "Plug-in your Apple TV 2 via USB.";
                //this.Center_PowerOFFtxt();
            }
            else
            {
                //this.animation.Visible = true;
                //this.Label1.Text = "Please power off your iDevice,\rThen press " + iFaith.Quotation + "Start" + iFaith.Quotation + ".";
                //this.Center_PowerOFFtxt();
            }
            //ThreadPool.QueueUserWorkItem(new WaitCallback(this._Lambda$__2));
        }

        private void Prepare_DoWork_1(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.PrepareTXT.Invoke(new MethodInvoker(this.PrepareDoWork));
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                ProjectData.ClearProjectError();
            }
        }

        private void labelLicence_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox("See attached file LICENSE in iFaith folder.", MsgBoxStyle.Information, null);
        }

    }
}
