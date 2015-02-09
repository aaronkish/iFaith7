using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.GZip;
using iFaith.My;
using iFaith.My.Resources;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using CFManzana;


namespace iFaith
{
    public partial class Run : Form
    {
        private string buildsAvailable;
        public bool DumpMode;
        private bool exploited;
        private delUpdate Finished;
        private delUpdate Finished_shsh;
        private string latestIPSWURL;
        private string Results;
        private string Results_shsh;
        private bool runninglimera1n;
        private System.Threading.Timer Timer;
        private int timercount;
        private BackgroundWorker _Prepare;

        public Run()
        {
            base.Load += new EventHandler(this.Form1_Load);
            this.timercount = 0;
            this.latestIPSWURL = string.Empty;
            this.DumpMode = false;
            this.Finished = new delUpdate(this.UpdateText);
            this.runninglimera1n = false;
            this.exploited = false;
            this.Timer = new System.Threading.Timer(new TimerCallback(this.Tick), null, 0x3e8, 0x3e8);
            this.buildsAvailable = string.Empty;
            this.Finished_shsh = new delUpdate(this.UpdateText_shsh);
            this.Prepare = new BackgroundWorker();
            this.InitializeComponent();
        }

        public void Center_status()
        {
            this.Status.Left = (int)Math.Round((double)((((double)this.Width) / 2.0) - (((double)this.Status.Width) / 2.0)));
            Application.DoEvents();
        }

        public void Check()
        {
            if (this.getenvbox.Text.Contains("ready"))
            {
                //MyProject.Forms.MDIMain.dumpbtn.Enabled = true;
                //MyProject.Forms.MDIMain.dumpbtn.Checked = true;
                //MyProject.Forms.MDIMain.dumptxt.ForeColor = Color.White;
                this.Status.Text = "Dumping...";
                this.Status.Invoke(new MethodInvoker(this.Center_status));
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

        public String getSHSHBlob()
        {
            return this.shshblob.Text;
        }

        public String getEnvBox()
        {
            return this.getenvbox.Text;
        }

        public void setEnvBox(String text)
        {
            this.getenvbox.Text = text;
        }

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

        private void Form1_Load(object sender, EventArgs e)
        {
            Point point = new Point(160, 0);
            DFU objDFU = new DFU(null);
            this.Location = point;
            this.getenvbox.Visible = iFaith.Debug_Mode;
            this.shshblob.Visible = iFaith.Debug_Mode;
            dfuinstructionstxt.ForeColor = Color.DimGray;
            objDFU.dfuinstructions.Enabled = false;
            //MyProject.Forms.MDIMain.Text = "iFaith v" + MyProject.Forms.MDIMain.VersionNumber + " -- By: iH8sn0w -- [" + modProcessCmd.iDevice + "]";
            objDFU.Prepbtn.Enabled = true;
            objDFU.Prepbtn.Checked = true;
            objDFU.preptxt.ForeColor = Color.Crimson;
            //this.PictureBox1.Image = My.Resources.Resources.Dove;
            this.Prepare.RunWorkerAsync();
        }

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
            this.Status.Text = "Prepare to choose a saving location in [3]";
            this.Center_status();
            modProcessCmd.Delay(1.0);
            this.Status.Text = "Prepare to choose a saving location in [2]";
            modProcessCmd.Delay(1.0);
            this.Status.Text = "Prepare to choose a saving location in [1]";
            modProcessCmd.Delay(1.0);
            this.Status.Text = "Choose a saving Location...";
            this.Center_status();
            this.SaveBlobs.FileName = iFaith.ECID + "_" + modProcessCmd.iDevice.Replace(" ", "_") + "-" + iFaith.realiosversion.Replace(" ", "_") + "-blobs";
            this.SaveBlobs.ShowDialog();
            iFaith.setenv("auto-boot", "true");
            iFaith.iRecovery_cmd("reset");
            this.Status.Text = "Saving cache (locally)...";
            this.Center_status();
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
            this.Status.Text = "Communicating with iNeal.me...";
            this.Center_status();
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
                    this.Status.Text = "Communicating with icj.me...";
                    this.Center_status();
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
                    this.Status.Text = "Fetching " + iosVersion + " (" + iosBuild + ") blobs from Apple...";
                    this.Center_status();
                    string str12 = iFaith.SendPOSTRequest("http://" + iFaith.AppleTSSIP + "/TSS/controller?action=2", Encoding.ASCII.GetBytes(s), "InetURL/1.0");
                    if (str12.Contains("STATUS=0&MESSAGE=SUCCESS"))
                    {
                        str7 = str7 + iosVersion + " (" + iosBuild + ")\r";
                        flag = true;
                        this.Status.Text = "Submitting " + iosVersion + " (" + iosBuild + ") blobs to Cydia...";
                        this.Center_status();
                        if (iFaith.SendPOSTRequest("http://cydia.saurik.com/tss@home/api/store/" + Conversions.ToString(Convert.ToInt64(Conversions.ToString(chipIDvar), 0x10)) + "/" + Conversions.ToString(boardIDvar) + "/" + Conversions.ToString(Convert.ToInt64(iFaith.ECID, 0x10)), Encoding.ASCII.GetBytes(str12), "iFaith-v" + objMDIMain.VersionNumber) != "failed")
                        {
                            expression = expression + iosVersion + " (" + iosBuild + ")\r";
                        }
                        else
                        {
                            str6 = str6 + iosVersion + " (" + iosBuild + ")\r";
                            this.Status.Text = "Cydia's TSS@Home rejected our blobs :(";
                            this.Center_status();
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
            this.Status.Text = "Submitting Dumped Blobs to iAcqua...";
            this.Center_status();
            string str16 = iFaith.SendPOSTRequest("http://iacqua.ih8sn0w.com/submit.php?ecid=" + iFaith.ECID + "&board=" + modProcessCmd.board + "&ios=" + iFaith.realiosversion.Replace(" ", "%20"), Encoding.ASCII.GetBytes(this.tehfile.Text), iFaith.fairydust);
            this.tehfile.Dispose();
            this.createipsw.Enabled = false;
            this.createipsw.Checked = false;
            this.createipswtxt.ForeColor = Color.DimGray;
            this.Status.Text = "Done! :)";
            this.Center_status();
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
                this.Status.Text = "Exploiting with steaks4uce...";
                this.PictureBox1.Image = My.Resources.Resources.steaks4uce;
            }
            else
            {
                this.Status.Text = "Exploiting with limera1n...";
                this.PictureBox1.Image = My.Resources.Resources.limera1n;
            }
            this.Status.Invoke(new MethodInvoker(this.Center_status));
            iFaith.iRecovery_exploit();
            this.exploited = true;
            this.Status.Text = "Waiting for " + modProcessCmd.iDevice + "...";
            this.Status.Invoke(new MethodInvoker(this.Center_status));
            this.PictureBox1.Image = My.Resources.Resources.Dove;
            iFaith.Wait_For_DFU();
            objMain.exploitbtn.Enabled = false;
            objMain.exploittxt.ForeColor = Color.DimGray;
            objMain.uploadibssbtn.Enabled = true;
            objMain.uploadibssbtn.Checked = true;
            objMain.uploadibsstxt.ForeColor = Color.White;
            this.Status.Text = "Uploading iBSS...";
            this.Status.Invoke(new MethodInvoker(this.Center_status));
            iFaith.iRecovery_file(iFaith.temppath + @"\iBSS." + modProcessCmd.board + ".RELEASE.dfu");
            this.Status.Text = "Waiting for " + modProcessCmd.iDevice + "...";
            this.Status.Invoke(new MethodInvoker(this.Center_status));
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
            this.Status.Text = "Uploading iBEC...";
            this.Status.Invoke(new MethodInvoker(this.Center_status));
            iFaith.iRecovery_file(iFaith.temppath + @"\iBEC." + modProcessCmd.board + ".RELEASE.dfu");
            if (modProcessCmd.iDevice == "iPod Touch 2G")
            {
                iFaith.iRecovery_cmd("go");
            }
            this.Status.Text = "Waiting for " + modProcessCmd.iDevice + "...";
            this.Status.Invoke(new MethodInvoker(this.Center_status));
            iFaith.Wait_For_iBoot();
            objMain.uploadibecbtn.Enabled = false;
            objMain.uploadibectxt.ForeColor = Color.DimGray;
            this.Status.Text = "Validating stuff with " + modProcessCmd.iDevice + "...";
            this.Status.Invoke(new MethodInvoker(this.Center_status));
            this.Luv();
        }

        
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

        private void Prepare_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.Status.Invoke(new MethodInvoker(this.PrepareDoWork));
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                ProjectData.ClearProjectError();
            }
        }

        public void PrepareDoWork()
        {
            //this.PictureBox1.Image = My.Resources.Resources.Dove;
            this.Status.Text = "Downloading Essentials...";
            this.ProgressBar1.Style = ProgressBarStyle.Marquee;
            this.ProgressBar1.MarqueeAnimationSpeed = 50;
            this.Status.Invoke(new MethodInvoker(this.Center_status));
            this.Status.Invoke(new MethodInvoker(iFaith.Downloader));
            this.Status.Invoke(new MethodInvoker(this.gogogo));
        }

        public void Restore_Window()
        {
            MDIMain objMain = new MDIMain();
            objMain.Text = "iFaith v" + objMain.VersionNumber + " -- By: iH8sn0w";
            Form form = new Form();
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
            this.Dispose();
        }

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

        public void update_1(object sender, DataReceivedEventArgs e)
        {
            this.UpdateTextBox(e.Data);
        }

        public void update_1_shsh(object sender, DataReceivedEventArgs e)
        {
            this.UpdateTextBox_shsh(e.Data);
        }

        private void UpdateText()
        {
            this.getenvbox.Text = this.Results;
        }

        private void UpdateText_shsh()
        {
            RichTextBox shshblob = this.shshblob;
            shshblob.Text = shshblob.Text + this.Results_shsh;
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
                RichTextBox getenvbox = this.getenvbox;
                getenvbox.Text = getenvbox.Text + Tex;
                this.getenvbox.Text = this.getenvbox.Text.Replace(" ", "");
            }
        }

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




        private delegate void delUpdate();

        private delegate void delUpdate_shsh();

        private delegate void UpdateTextBoxDelegate(string Text);

        private delegate void UpdateTextBoxDelegate_shsh(string Text);

        private void Run_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
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
                DoWorkEventHandler handler = new DoWorkEventHandler(this.Prepare_DoWork);
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

        private void labelLicence_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox("See attached file LICENSE in iFaith folder.", MsgBoxStyle.Information, null);
        }
    }
}
