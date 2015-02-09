using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

using CFManzana;
using ICSharpCode.SharpZipLib.GZip;
using iFaith.My;
using iFaith.My.Resources;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace iFaith
{
    public partial class iAcqua : Form
    {
        FormMain objFormMain;
        public int BoardID;
        private string buildsAvailable;
        public int ChipID;
        private string ecidbox;
        private delUpdate Finished;
        public bool iAcquaIsBusy;
        private string iAcquaResponse;
        public string iDeviceBoard;
        public string iDeviceName;
        public string iDeviceProductType;
        public string iDeviceType;
        private bool iJustDidBlobs;
        public bool iLeft;
        private string latestIPSWURL;
        public bool onResumeGrabBlobs;
        public bool QuitMOFO;
        private string Results;
        public bool subFormisInvisible;
        private bool ThisIsAManualECIDLookUp;
        private string tssHomeRequest;
        private string tssResponse;
        private iDevice _iPhoneInterface;
        public iAcqua(FormMain vobjFormMain)
        {
            base.FormClosing += new FormClosingEventHandler(this.iAcqua_byebye);
            base.Load += new EventHandler(this.iAcqua_Load);
            this.tssResponse = string.Empty;
            this.iAcquaResponse = string.Empty;
            this.QuitMOFO = false;
            this.iLeft = false;
            this.iJustDidBlobs = false;
            this.onResumeGrabBlobs = false;
            this.iDeviceName = string.Empty;
            this.iDeviceType = string.Empty;
            this.iDeviceProductType = string.Empty;
            this.iDeviceBoard = string.Empty;
            this.ThisIsAManualECIDLookUp = false;
            this.ChipID = 0;
            this.BoardID = 0;
            this.iAcquaIsBusy = false;
            this.subFormisInvisible = false;
            this.buildsAvailable = string.Empty;
            this.latestIPSWURL = string.Empty;
            this.tssHomeRequest = string.Empty;
            this.ecidbox = string.Empty;
            this.Finished = new delUpdate(this.UpdateText);
            InitializeComponent();
            objFormMain = vobjFormMain;
            setListBox1Items();
        }

        private void setListBox1Items()
        {
            this.ListBox1.Items.AddRange(new object[] { 
                "Select Model:", "Apple TV 2", "Apple TV 3", "Apple TV 3 (2013)", "iPad 1", "iPad 2 [WiFi]", "iPad 2 [WiFi-Rev2]", "iPad 2 [GSM]", "iPad 2 [CDMA]", "iPad 3 [WiFi]", "iPad 3 [CDMA]", "iPad 3 [GSM]", "iPad 4 [WiFi]", "iPad 4 [A1459]", "iPad 4 [A1460]", "iPad mini [WiFi]", 
                "iPad mini [A1454]", "iPad mini [A1455]", "iPhone 3G", "iPhone 3GS", "iPhone 4 [GSM]", "iPhone 4 [GSM-Rev2]", "iPhone 4 [CDMA]", "iPhone 4S", "iPhone 5 [A1428]", "iPhone 5 [A1429]", "iPod Touch 2G", "iPod Touch 3", "iPod Touch 4", "iPod Touch 5"
             });
        }

        [CompilerGenerated, DebuggerStepThrough]
        private void ThreadECIDSearchWorker(object a0)
        {
            this.dfuibootsearcher();
        }

        private void iAcqua_Load(object sender, EventArgs e)
        {
            //Point point = new Point(35, 0);
            //this.Location = point;
            this.Cleanup();
            this.iLeft = false;
            //this.Center_Label(this.Label3);
            this.ListBox1.SelectedIndex = 0;
            try
            {
                this.iPhoneInterface = new iDevice();
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                Interaction.MsgBox("iFaith was unable to hook iTunes!\r\rAutomatic ECID detection is now only available via Recovery/DFU!", MsgBoxStyle.Critical, null);
                ProjectData.ClearProjectError();
            }
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadECIDSearchWorker));
        }

        private void iAcqua_byebye(object Sender, EventArgs e)
        {
            this.iAcquaIsBusy = true;
            this.QuitMOFO = true;
            iFaith.KilliRecovery();
            this.Dispose();
        }

        private void iAcqua_FormClosed(object sender, FormClosedEventArgs e)
        {
            objFormMain.Show();
            this.Dispose();
        }

        private void buttonRestartDFU_Click(object sender, EventArgs e)
        {            
            if (!this.TextBox1.Text.Contains("Enter an"))
            {
                if (!this.IsHexadecimal(this.TextBox1.Text))
                {
                    Interaction.MsgBox("Invalid ECID!\r\rPlease enter a valid ECID!", MsgBoxStyle.Critical, null);
                }
                else
                {
                    try
                    {
                        this.TextBox1.Text = this.TextBox1.Text.ToUpper();
                        bool flag = false;
                        if (!iFaith.IsDigitsOnly(this.TextBox1.Text))
                        {
                            flag = true;
                        }
                        if (flag & (this.TextBox1.Text.Length != 0x10))
                        {
                            while (this.TextBox1.Text.Length != 0x10)
                            {
                                this.TextBox1.Text = "0" + this.TextBox1.Text;
                            }
                        }
                        if (!flag)
                        {
                            this.TextBox1.Text = Conversion.Hex(this.TextBox1.Text);
                            while (this.TextBox1.Text.Length != 0x10)
                            {
                                this.TextBox1.Text = "0" + this.TextBox1.Text;
                            }
                        }
                        iFaith.ECID = this.TextBox1.Text;
                        string str = Conversions.ToString(this.ListBox1.SelectedItem);
                        switch (str)
                        {
                            case "iPhone 3G":
                                this.iDeviceBoard = "n82ap";
                                this.ChipID = 0x22c4;
                                this.BoardID = 4;
                                this.iDeviceProductType = "iPhone1,1";
                                break;

                            case "iPhone 3GS":
                                this.iDeviceBoard = "n88ap";
                                this.ChipID = 0x22d8;
                                this.BoardID = 0;
                                this.iDeviceProductType = "iPhone1,2";
                                break;

                            case "iPhone 4 [GSM]":
                                this.iDeviceBoard = "n90ap";
                                this.ChipID = 0x22e2;
                                this.BoardID = 0;
                                this.iDeviceProductType = "iPhone3,1";
                                break;

                            case "iPhone 4 [GSM-Rev2]":
                                this.iDeviceBoard = "n90bap";
                                this.ChipID = 0x22e2;
                                this.BoardID = 4;
                                this.iDeviceProductType = "iPhone3,2";
                                break;

                            case "iPhone 4 [CDMA]":
                                this.iDeviceBoard = "n92ap";
                                this.ChipID = 0x22e2;
                                this.BoardID = 6;
                                this.iDeviceProductType = "iPhone3,3";
                                break;

                            case "iPhone 4S":
                                this.iDeviceBoard = "n94ap";
                                this.ChipID = 0x22ec;
                                this.BoardID = 8;
                                this.iDeviceProductType = "iPhone4,1";
                                break;

                            case "iPhone 5 [A1428]":
                                this.iDeviceBoard = "n41ap";
                                this.ChipID = 0x22f6;
                                this.BoardID = 0;
                                this.iDeviceProductType = "iPhone5,1";
                                break;

                            case "iPhone 5 [A1429]":
                                this.iDeviceBoard = "n42ap";
                                this.ChipID = 0x22f6;
                                this.BoardID = 2;
                                this.iDeviceProductType = "iPhone5,2";
                                break;

                            case "iPod Touch 2G":
                                this.iDeviceBoard = "n72ap";
                                this.ChipID = 0x2210;
                                this.BoardID = 0;
                                this.iDeviceProductType = "iPod2,1";
                                break;

                            case "iPod Touch 3":
                                this.iDeviceBoard = "n18ap";
                                this.ChipID = 0x22da;
                                this.BoardID = 2;
                                this.iDeviceProductType = "iPod3,1";
                                break;

                            case "iPod Touch 4":
                                this.iDeviceBoard = "n81ap";
                                this.ChipID = 0x22e2;
                                this.BoardID = 8;
                                this.iDeviceProductType = "iPod4,1";
                                break;

                            case "iPod Touch 5":
                                this.iDeviceBoard = "n78ap";
                                this.ChipID = 0x22ee;
                                this.BoardID = 0;
                                this.iDeviceProductType = "iPod5,1";
                                break;

                            case "Apple TV 2":
                                this.iDeviceBoard = "k66ap";
                                this.ChipID = 0x22e2;
                                this.BoardID = 0x10;
                                this.iDeviceProductType = "AppleTV2,1";
                                break;

                            case "Apple TV 3":
                                this.iDeviceBoard = "j33ap";
                                this.ChipID = 0x22ee;
                                this.BoardID = 0x10;
                                this.iDeviceProductType = "AppleTV3,1";
                                break;

                            case "Apple TV 3 (2013)":
                                this.iDeviceBoard = "j33iap";
                                this.ChipID = 0x22f3;
                                this.BoardID = 0;
                                this.iDeviceProductType = "AppleTV3,1";
                                break;

                            case "iPad 1":
                                this.iDeviceBoard = "k48ap";
                                this.ChipID = 0x22e2;
                                this.BoardID = 2;
                                this.iDeviceProductType = "iPad1,1";
                                break;

                            case "iPad 2 [WiFi]":
                                this.iDeviceBoard = "k93ap";
                                this.ChipID = 0x22ec;
                                this.BoardID = 4;
                                this.iDeviceProductType = "iPad2,1";
                                break;

                            case "iPad 2 [WiFi-Rev2]":
                                this.iDeviceBoard = "k93aap";
                                this.ChipID = 0x22ee;
                                this.BoardID = 6;
                                this.iDeviceProductType = "iPad2,4";
                                break;

                            case "iPad 2 [GSM]":
                                this.iDeviceBoard = "k94ap";
                                this.ChipID = 0x22ec;
                                this.BoardID = 6;
                                this.iDeviceProductType = "iPad2,2";
                                break;

                            case "iPad 2 [CDMA]":
                                this.iDeviceBoard = "k95ap";
                                this.ChipID = 0x22ec;
                                this.BoardID = 2;
                                this.iDeviceProductType = "iPad2,3";
                                break;

                            case "iPad 3 [WiFi]":
                                this.iDeviceBoard = "j1ap";
                                this.ChipID = 0x22f1;
                                this.BoardID = 0;
                                this.iDeviceProductType = "iPad3,1";
                                break;

                            case "iPad 3 [CDMA]":
                                this.iDeviceBoard = "j2ap";
                                this.ChipID = 0x22f1;
                                this.BoardID = 2;
                                this.iDeviceProductType = "iPad3,2";
                                break;

                            case "iPad 3 [GSM]":
                                this.iDeviceBoard = "j2aap";
                                this.ChipID = 0x22f1;
                                this.BoardID = 4;
                                this.iDeviceProductType = "iPad3,3";
                                break;

                            case "iPad 4 [WiFi]":
                                this.iDeviceBoard = "p101ap";
                                this.ChipID = 0x22fb;
                                this.BoardID = 0;
                                this.iDeviceProductType = "iPad3,4";
                                break;

                            case "iPad 4 [A1459]":
                                this.iDeviceBoard = "p102ap";
                                this.ChipID = 0x22fb;
                                this.BoardID = 2;
                                this.iDeviceProductType = "iPad3,5";
                                break;

                            case "iPad 4 [A1460]":
                                this.iDeviceBoard = "p103ap";
                                this.ChipID = 0x22fb;
                                this.BoardID = 4;
                                this.iDeviceProductType = "iPad3,6";
                                break;

                            case "iPad mini [WiFi]":
                                this.iDeviceBoard = "p105ap";
                                this.ChipID = 0x22ee;
                                this.BoardID = 10;
                                this.iDeviceProductType = "iPad2,5";
                                break;

                            case "iPad mini [A1454]":
                                this.iDeviceBoard = "p106ap";
                                this.ChipID = 0x22ee;
                                this.BoardID = 12;
                                this.iDeviceProductType = "iPad2,6";
                                break;

                            case "iPad mini [A1455]":
                                this.iDeviceBoard = "p107ap";
                                this.ChipID = 0x22ee;
                                this.BoardID = 14;
                                this.iDeviceProductType = "iPad2,7";
                                break;
                        }
                        this.PictureBox1.Visible = false;
                        this.iDeviceType = str;
                        this.Label2.Text = "Waiting on Server...";
                        this.Label2.Left = (int)Math.Round((double)((((double)this.Width) / 2.0) - (((double)this.Label2.Width) / 2.0)));
                        this.TextBox1.Text = string.Empty;
                        this.DisplayProgress();
                        this.ThisIsAManualECIDLookUp = true;
                        this.Invoke(new MethodInvoker(this.iAcquaDO));
                    }
                    catch (Exception exception1)
                    {
                        ProjectData.SetProjectError(exception1);
                        Exception exception = exception1;
                        ProjectData.ClearProjectError();
                    }
                }
            }
        }

        public void iAcquaDO()
        {
            MDIMain objMain = new MDIMain();
            try
            {
                this.PictureBox1.Visible = false;
                this.goback.Visible = false;
                this.iAcquaIsBusy = true;
                this.availableshsh.Items.Clear();
                this.Label2.Text = "Communicating with Cydia...";
                this.DisplayProgress();
                this.Center_Label(this.spinny);
                this.Center_Label(this.Label2);
                //this.Center_Label(this.availableshsh);
                //this.Center_Label(this.dlallBTN);
                //this.Center_Label(this.dlblobBTN);
                Application.DoEvents();
                try
                {
                    this.tssResponse = string.Empty;
                    WebClient client = new WebClient();
                    client.Headers.Add("user-agent", iFaith.fairydust);
                    client.DownloadStringAsync(new Uri("http://cydia.saurik.com/tss@home/api/check/" + Conversions.ToString(Convert.ToInt64(iFaith.ECID, 0x10))));
                    client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.DownloadStringCompleted);
                    Definitions.i = 0;
                    while (!(!client.IsBusy | (Definitions.i == iFaith.webTimeOut)))
                    {
                        Application.DoEvents();
                        modProcessCmd.Delay(1.0);
                        Definitions.i++;
                    }
                    client.Dispose();
                    if (((this.tssResponse == "[ ]") | (this.tssResponse == "error")) | (this.tssResponse == string.Empty))
                    {
                        Application.DoEvents();
                    }
                    else
                    {
                        IEnumerator enumerator = null;
                        string str = string.Empty;
                        object obj2 = Strings.Split(this.tssResponse, "}", -1, CompareMethod.Binary);
                        try
                        {
                            enumerator = ((IEnumerable)obj2).GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                str = Conversions.ToString(enumerator.Current);
                                if (str.Contains("chip\": " + Conversions.ToString(Convert.ToInt64(Conversions.ToString(this.ChipID), 0x10)) + ",") & str.Contains("board\": " + Conversions.ToString(this.BoardID) + ","))
                                {
                                    int startIndex = str.IndexOf("firmware") + 12;
                                    int length = str.IndexOf("\"", startIndex) - startIndex;
                                    int num3 = str.IndexOf("build") + 9;
                                    int num4 = str.IndexOf("\"", num3) - num3;
                                    string str2 = str.Substring(startIndex, length);
                                    string str3 = str.Substring(num3, num4);
                                    if (!str2.Contains("."))
                                    {
                                        str2 = "Unknown";
                                    }
                                    this.availableshsh.Items.Add(str2 + " (" + str3 + ")  [Cydia]");
                                }
                            }
                        }
                        finally
                        {
                            if (enumerator is IDisposable)
                            {
                                (enumerator as IDisposable).Dispose();
                            }
                        }
                    }
                    Application.DoEvents();
                    Application.DoEvents();
                    this.Label2.Text = "Communicating with iAcqua...";
                    this.Center_Label(this.Label2);
                    this.iAcquaResponse = string.Empty;
                    Uri address = new Uri("http://iacqua.ih8sn0w.com/submit.php?ecid=" + iFaith.ECID + "&board=" + this.iDeviceBoard);
                    WebClient client2 = new WebClient();
                    client2.Headers.Add("user-agent", iFaith.fairydust);
                    client2.DownloadStringAsync(address);
                    client2.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.DownloadStringCompleted_iacqua);
                    Definitions.i = 0;
                    while (!(!client2.IsBusy | (Definitions.i == iFaith.webTimeOut)))
                    {
                        Application.DoEvents();
                        modProcessCmd.Delay(1.0);
                        Definitions.i++;
                    }
                    if (((this.iAcquaResponse == "error") | this.iAcquaResponse.Contains("No such ECID.")) | (this.iAcquaResponse == string.Empty))
                    {
                        Application.DoEvents();
                    }
                    else
                    {
                        foreach (string str5 in Strings.Split(this.iAcquaResponse, ".shsh", -1, CompareMethod.Binary))
                        {
                            string str6 = str5.Replace("\r", "").Replace("\n", "");
                            if ((str6 != string.Empty) & (str6.Replace(" ", "") != "None"))
                            {
                                this.availableshsh.Items.Add(str6 + "  [iFaith]");
                            }
                        }
                    }
                    this.goback.Enabled = true;
                    this.goback.Visible = true;
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    
                    if (Operators.ConditionalCompareObjectEqual(objMain.ThisBuildIsNotPublic, true, false))
                    {
                        Interaction.MsgBox(exception.ToString(), MsgBoxStyle.Critical, null);
                    }
                    Interaction.MsgBox("Error: We have failed trying to connect to iFaith's or Cydia's SHSH Cache server!", MsgBoxStyle.Critical, null);
                    this.Invoke(new MethodInvoker(this.Cleanup));
                    ProjectData.ClearProjectError();
                }
                try
                {
                    this.Label2.Text = "Available Blobs:";
                    this.Center_Label(this.Label2);
                    this.Button1.Visible = true;
                    if (this.availableshsh.Items.Count == 0)
                    {
                        this.availableshsh.Items.Add("None");
                        this.dlallBTN.Enabled = false;
                        this.dlblobBTN.Enabled = false;
                    }
                    else
                    {
                        this.dlallBTN.Enabled = true;
                        this.dlblobBTN.Enabled = true;
                    }
                    this.spinny.Visible = false;
                    this.availableshsh.Visible = true;
                    this.availableshsh.SelectedIndex = 0;
                    this.dlallBTN.Visible = true;
                    this.dlblobBTN.Visible = true;
                    if (this.iJustDidBlobs)
                    {
                        this.iJustDidBlobs = false;
                    }
                    else
                    {
                        Application.DoEvents();
                        this.onResumeGrabBlobs = false;
                        this.subFormisInvisible = false;
                       // MyProject.Forms.iAcquaAssistant.MdiParent = MyProject.Forms.MDIMain;
                        //MyProject.Forms.iAcquaAssistant.Show();
                        while (!this.subFormisInvisible)
                        {
                            Application.DoEvents();
                        }
                        if (!this.onResumeGrabBlobs)
                        {
                            this.iJustDidBlobs = true;
                            this.iAcquaDO();
                            return;
                        }
                        if (this.onResumeGrabBlobs)
                        {
                            this.onResumeGrabBlobs = false;
                            this.iAcquaIsBusy = true;
                            this.availableshsh.Visible = false;
                            this.ListBox1.Visible = false;
                            this.Label2.Text = "Communicating with iNeal.me...";
                            this.Center_Label(this.Label2);
                            this.DisplayProgress();
                            Application.DoEvents();
                            string expression = string.Empty;
                            string str8 = string.Empty;
                            string str9 = string.Empty;
                            string str11 = string.Empty;
                            int chipIDvar = 0;
                            int boardIDvar = 0;
                            string deviceNamevar = string.Empty;
                            string producttypevar = string.Empty;
                            WebClient client3 = new WebClient();
                            client3.Headers.Add("user-agent", "iFaith-v" + objMain.VersionNumber);
                            this.buildsAvailable = string.Empty;
                            client3.DownloadStringAsync(new Uri("http://api.ineal.me/tss/" + this.iDeviceBoard + "/if"));
                            client3.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.DownloadStringCompleted_ineal);
                            Definitions.i = 0;
                            while (!((!client3.IsBusy & (this.buildsAvailable != string.Empty)) | (Definitions.i == iFaith.webTimeOut)))
                            {
                                Application.DoEvents();
                                modProcessCmd.Delay(1.0);
                                Definitions.i++;
                            }
                            client3.Dispose();
                            if ((this.buildsAvailable == "error") | (this.buildsAvailable == string.Empty))
                            {
                                this.buildsAvailable = "latest";
                            }
                            iFaith.giveMeDetails(this.iDeviceBoard, ref chipIDvar, ref boardIDvar, ref deviceNamevar, ref producttypevar);
                            string[] array = Strings.Split(this.buildsAvailable, "#", -1, CompareMethod.Binary);
                            producttypevar = producttypevar.ToLower();
                            Array.Sort<string>(array, StringComparer.OrdinalIgnoreCase);
                            foreach (string str14 in array)
                            {
                                if (str14.Contains("b") & str14.Contains("."))
                                {
                                    WebClient client4 = new WebClient();
                                    client4.Headers.Add("user-agent", "iFaith-v" + objMain.VersionNumber);
                                    this.latestIPSWURL = string.Empty;
                                    client4.DownloadStringAsync(new Uri("http://api.ineal.me/tss/request.plist/" + this.iDeviceBoard + "/" + str14.Substring(str14.IndexOf("-") + 1, (str14.Length - str14.IndexOf("-")) - 1)));
                                    client4.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.DownloadStringCompleted_icj);
                                    Definitions.i = 0;
                                    while (!((!client4.IsBusy & (this.latestIPSWURL != string.Empty)) | (Definitions.i == iFaith.webTimeOut)))
                                    {
                                        Application.DoEvents();
                                        modProcessCmd.Delay(1.0);
                                        Definitions.i++;
                                    }
                                    client4.Dispose();
                                    if (this.latestIPSWURL == string.Empty)
                                    {
                                        this.latestIPSWURL = "error";
                                    }
                                    if (this.latestIPSWURL == "error")
                                    {
                                        expression = expression + "Unknown (" + str14 + ")\r";
                                        Interaction.MsgBox("iFaith failed to download the " + str14 + " SHSH blobs!\r\rEnsure you have the latest iFaith, and have an active internet connection!", MsgBoxStyle.Critical, null);
                                    }
                                }
                                else
                                {
                                    this.Label2.Text = "Communicating with icj.me...";
                                    this.Center_Label(this.Label2);
                                    Application.DoEvents();
                                    WebClient client5 = new WebClient();
                                    client5.Headers.Add("user-agent", "iFaith-v" + objMain.VersionNumber);
                                    this.latestIPSWURL = string.Empty;
                                    client5.DownloadStringAsync(new Uri("http://api.ios.icj.me/v2/" + this.iDeviceBoard + "/" + str14 + "/url"));
                                    client5.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.DownloadStringCompleted_icj);
                                    Definitions.i = 0;
                                    while (!((!client5.IsBusy & (this.latestIPSWURL != string.Empty)) | (Definitions.i == iFaith.webTimeOut)))
                                    {
                                        Application.DoEvents();
                                        modProcessCmd.Delay(1.0);
                                        Definitions.i++;
                                    }
                                    client5.Dispose();
                                    this.Label2.Text = "Communicating with Apple...";
                                    this.Center_Label(this.Label2);
                                    if (this.latestIPSWURL == string.Empty)
                                    {
                                        this.latestIPSWURL = "error";
                                    }
                                    if (this.latestIPSWURL == "error")
                                    {
                                        expression = expression + "Unknown (" + str14 + ")\r";
                                    }
                                    else
                                    {
                                        Application.DoEvents();
                                        iFaith.DownloadFromURL(this.latestIPSWURL, "BuildManifest.plist", iFaith.temppath + @"\tmp-bm.plist");
                                    }
                                }
                                if (this.latestIPSWURL != "error")
                                {
                                    string iosVersion = string.Empty;
                                    string iosBuild = string.Empty;
                                    string s = string.Empty;
                                    if (str14.Contains("b") & str14.Contains("."))
                                    {
                                        iosVersion = str14.Substring(0, str14.IndexOf("-"));
                                        iosBuild = str14.Substring(str14.IndexOf("-") + 1, (str14.Length - str14.IndexOf("-")) - 1);
                                        s = this.latestIPSWURL.Replace("$ECID$", Conversions.ToString(Convert.ToInt64(iFaith.ECID, 0x10)));
                                    }
                                    else
                                    {
                                        s = iFaith.MakeTSSRequestFromBuildManifest(iFaith.temppath + @"\tmp-bm.plist", this.iDeviceBoard, ref iosVersion, ref iosBuild, ref boardIDvar, ref chipIDvar, ref producttypevar).Replace("$ECID$", Conversions.ToString(Convert.ToInt64(iFaith.ECID, 0x10)));
                                    }
                                    this.Label2.Text = "Fetching " + iosVersion + " (" + iosBuild + ") blobs from Apple...";
                                    this.Center_Label(this.Label2);
                                    Application.DoEvents();
                                    string str20 = iFaith.SendPOSTRequest("http://" + iFaith.AppleTSSIP + "/TSS/controller?action=2", Encoding.ASCII.GetBytes(s), "InetURL/1.0");
                                    if (str20.Contains("STATUS=0&MESSAGE=SUCCESS"))
                                    {
                                        str11 = str11 + iosVersion + " (" + iosBuild + ")\r";
                                        string path = Environment.GetEnvironmentVariable("UserProfile") + @"\.shsh";
                                        if (!Directory.Exists(path))
                                        {
                                            Directory.CreateDirectory(path);
                                        }
                                        System.IO.File.WriteAllText(iFaith.temppath + @"\tss-response.xml", str20.Substring(str20.IndexOf("<?xml"), (str20.IndexOf("</plist>") - str20.IndexOf("<?xml")) + 8));
                                        iFaith.xml_to_bplist(iFaith.temppath + @"\tss-response.xml", iFaith.temppath + @"\tss-response.plist");
                                        string strFile = path + @"\" + Conversions.ToString(Convert.ToInt64(iFaith.ECID, 0x10)) + "-" + producttypevar.ToLower() + "-" + iosVersion + ".shsh";
                                        if (!modFile.File_Exists(strFile))
                                        {
                                            using (Stream stream = System.IO.File.OpenRead(iFaith.temppath + @"\tss-response.plist"))
                                            {
                                                using (FileStream stream2 = System.IO.File.Create(strFile))
                                                {
                                                    using (Stream stream3 = new GZipOutputStream(stream2))
                                                    {
                                                        int num9 = 0;
                                                        byte[] buffer = new byte[0xfff];
                                                        while (iFaith.InlineAssignHelper<int>(ref num9, stream.Read(buffer, 0, buffer.Length)) != 0)
                                                        {
                                                            stream3.Write(buffer, 0, num9);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        iFaith.Delete_File(iFaith.temppath + @"\tss-response.plist");
                                        iFaith.Delete_File(iFaith.temppath + @"\tss-response.xml");
                                        iFaith.Delete_File(iFaith.temppath + @"\tmp-bm.plist");
                                        this.Label2.Text = "Submitting " + iosVersion + " (" + iosBuild + ") blobs to Cydia...";
                                        this.Center_Label(this.Label2);
                                        Application.DoEvents();
                                        if (iFaith.SendPOSTRequest("http://cydia.saurik.com/tss@home/api/store/" + Conversions.ToString(Convert.ToInt64(Conversions.ToString(this.ChipID), 0x10)) + "/" + Conversions.ToString(this.BoardID) + "/" + Conversions.ToString(Convert.ToInt64(iFaith.ECID, 0x10)), Encoding.ASCII.GetBytes(str20), "iFaith-v" + objMain.VersionNumber) != "failed")
                                        {
                                            str8 = str8 + iosVersion + " (" + iosBuild + ")\r";
                                        }
                                        else
                                        {
                                            str9 = str9 + iosVersion + " (" + iosBuild + ")\r";
                                        }
                                    }
                                    else
                                    {
                                        expression = expression + "Unknown (" + str14 + ")\r";
                                    }
                                }
                            }
                            this.Label2.Text = "Done!";
                            this.Center_Label(this.Label2);
                            this.spinny.Visible = false;
                            string prompt = string.Empty;
                            if (str11 != string.Empty)
                            {
                                prompt = prompt + "iFaith has fetched the following shsh blobs directly from Apple: \r\r";
                                foreach (string str25 in Strings.Split(str11, "\r", -1, CompareMethod.Binary))
                                {
                                    prompt = prompt + str25 + "\r";
                                }
                                prompt = prompt + "They have been stored locally at: \r" + Environment.GetEnvironmentVariable("UserProfile") + @"\.shsh\" + Conversions.ToString(Convert.ToInt64(iFaith.ECID, 0x10)) + "-*";
                                if (str8 != string.Empty)
                                {
                                    prompt = prompt + "\r\riFaith successfully submitted the following fetched blobs to Cydia: \r\r";
                                    foreach (string str26 in Strings.Split(str8, "\r", -1, CompareMethod.Binary))
                                    {
                                        prompt = prompt + str26 + "\r";
                                    }
                                }
                            }
                            if (prompt != string.Empty)
                            {
                                Interaction.MsgBox(prompt, MsgBoxStyle.Information, null);
                            }
                            if (expression != string.Empty)
                            {
                                prompt = "iFaith has failed to fetch the following blobs:\r\r";
                                foreach (string str27 in Strings.Split(expression, "\r", -1, CompareMethod.Binary))
                                {
                                    prompt = prompt + str27 + "\r";
                                }
                                Interaction.MsgBox(prompt + "\rAre you running the latest version of iFaith?\r\r* It is possible that Apple has stopped signing the above firmwares!", MsgBoxStyle.Critical, null);
                            }
                            this.iJustDidBlobs = true;
                            this.iAcquaDO();
                        }
                    }
                    this.iAcquaIsBusy = false;
                }
                catch (Exception exception4)
                {
                    ProjectData.SetProjectError(exception4);
                    Exception exception2 = exception4;
                    ProjectData.ClearProjectError();
                }
            }
            catch (Exception exception5)
            {
                ProjectData.SetProjectError(exception5);
                Exception exception3 = exception5;
                ProjectData.ClearProjectError();
            }            
        }

        public bool IsHexadecimal(string strInput)
        {
            Regex regex = new Regex("^[a-fA-F0-9]+$");
            if (string.IsNullOrEmpty(strInput))
            {
                return false;
            }
            return regex.IsMatch(strInput);
        }

        private void DisplayProgress()
        {
            this.manualecidBTN.Visible = false;
            this.manualecidBTN.Enabled = false;
            this.spinny.Visible = true;
            this.availableshsh.Visible = false;
            this.ListBox1.Visible = false;
            this.goback.Enabled = false;
            this.Button1.Visible = false;
            this.iphone.Visible = false;
            this.Label3.Visible = false;
            this.dlallBTN.Visible = false;
            this.dlblobBTN.Visible = false;
            this.TextBox1.Visible = false;
        }

        public void Center_Label(object Label)
        {
            NewLateBinding.LateSet(Label, null, "Left", new object[] { Operators.SubtractObject(((double)this.Width) / 2.0, Operators.DivideObject(NewLateBinding.LateGet(Label, null, "Width", new object[0], null, null, null), 2)) }, null, null);
        }


        private void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled && (e.Error == null))
                {
                    this.tssResponse = e.Result;
                }
                else
                {
                    this.tssResponse = "error";
                }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                this.tssResponse = "error";
                Application.DoEvents();
                ProjectData.ClearProjectError();
            }
        }

        private void DownloadStringCompleted_iacqua(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled && (e.Error == null))
                {
                    this.iAcquaResponse = e.Result;
                }
                else
                {
                    this.iAcquaResponse = "error";
                }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                this.iAcquaResponse = "error";
                Application.DoEvents();
                ProjectData.ClearProjectError();
            }
        }

        private void DownloadStringCompleted_icj(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled && (e.Error == null))
                {
                    this.latestIPSWURL = e.Result;
                }
                else
                {
                    this.latestIPSWURL = "error";
                }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                this.latestIPSWURL = "error";
                Application.DoEvents();
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

        private void DownloadStringCompleted_tsshome(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled && (e.Error == null))
                {
                    this.tssHomeRequest = e.Result;
                }
                else
                {
                    this.tssHomeRequest = "nope";
                }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                this.latestIPSWURL = "nope";
                Application.DoEvents();
                ProjectData.ClearProjectError();
            }
        }

        public void Cleanup()
        {
            try
            {
                this.Label2.Text = "Connect an iDevice or enter an ECID";
                this.Center_Label(this.Label2);
                this.iphone.Visible = true;
                this.iphone.Refresh();
                this.Label3.Visible = true;
                this.TextBox1.Visible = true;
                this.TextBox1.Text = "Enter an ECID here...";
                this.TextBox1.ForeColor = Color.Gray;
                this.spinny.Visible = false;
                this.manualecidBTN.Enabled = false;
                this.manualecidBTN.Visible = true;
                this.ListBox1.SelectedIndex = 0;
                this.ListBox1.Visible = true;
                this.Button1.Visible = false;
                this.dlallBTN.Visible = false;
                this.dlblobBTN.Visible = false;
                this.availableshsh.Items.Clear();
                this.availableshsh.Visible = false;
                this.iAcquaIsBusy = false;
                this.ThisIsAManualECIDLookUp = false;
                this.PictureBox1.Visible = true;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                ProjectData.ClearProjectError();
            }
        }

        private void dfuibootsearcher()
        {
            MDIMain objMain = new MDIMain();
            if (!this.iLeft)
            {
                if (!modFile.File_Exists(iFaith.temppath + @"\s-irecovery.exe"))
                {
                    modFile.SaveToDisk("s-irecovery.exe", iFaith.temppath + @"\s-irecovery.exe");
                }
                string str = string.Empty;
                while (!this.QuitMOFO)
                {
                    try
                    {
                        while (this.iAcquaIsBusy)
                        {
                            modProcessCmd.Delay(1.0);
                            iFaith.KilliRecovery();
                        }
                        this.grabECIDifPossible();
                        this.ecidbox = this.ecidbox.Replace(" ", "").Replace("\r", "").Replace("\n", "");
                        if ((this.ecidbox.Replace(" ", "") != string.Empty) & !this.ecidbox.Contains("nope"))
                        {
                            string producttypevar = "";
                            string boardConfigvar = this.ecidbox.Substring(this.ecidbox.IndexOf("-") + 1, (this.ecidbox.Length - this.ecidbox.IndexOf("-")) - 1);
                            string deviceNamevar = string.Empty;
                            int chipIDvar = 0;
                            int boardIDvar = 0;
                            string str5 = this.ecidbox.Substring(0, 0x10);
                            iFaith.giveMeDetails(boardConfigvar, ref chipIDvar, ref boardIDvar, ref deviceNamevar, ref producttypevar);
                            while (str5.Length != 0x10)
                            {
                                str5 = "0" + str5;
                            }
                            if (!str.Contains(str5))
                            {
                                while (this.iAcquaIsBusy)
                                {
                                    Application.DoEvents();
                                }
                                this.iAcquaIsBusy = true;
                                if (Interaction.MsgBox("Name: " + deviceNamevar + " (iBoot or DFU)\rType: " + deviceNamevar + " [" + producttypevar + "]\rECID [HEX]: " + str5 + "\rECID [DEC]: " + Conversions.ToString(Convert.ToInt64(str5, 0x10)) + "\r\rWould you like to use the newly connected device?", MsgBoxStyle.Information | MsgBoxStyle.YesNo, null) == MsgBoxResult.Yes)
                                {                                    
                                    iFaith.ECID = str5;
                                    this.iDeviceType = deviceNamevar;
                                    this.ChipID = chipIDvar;
                                    this.BoardID = boardIDvar;
                                    this.iDeviceBoard = boardConfigvar;
                                    this.iDeviceProductType = producttypevar;                                    
                                    objMain.Activate();
                                    this.Invoke(new MethodInvoker(this.iAcquaDO));
                                }
                                else
                                {
                                    this.iAcquaIsBusy = false;
                                }
                                str = str + str5;
                            }
                        }
                        else
                        {
                            str = string.Empty;
                        }
                    }
                    catch (Exception exception1)
                    {
                        ProjectData.SetProjectError(exception1);
                        Exception exception = exception1;
                        this.iAcquaIsBusy = false;
                        ProjectData.ClearProjectError();
                    }
                    modProcessCmd.Delay(2.0);
                }
            }
        }

        private void grabECIDifPossible()
        {
            this.ecidbox = string.Empty;
            Process process = new Process();
            process.StartInfo.FileName = iFaith.temppath + @"\s-irecovery.exe";
            process.StartInfo.Arguments = "-ecid";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.OutputDataReceived += new DataReceivedEventHandler(this.update_1);
            process.Start();
            StreamWriter standardInput = process.StandardInput;
            process.BeginOutputReadLine();
            process.Dispose();
            while (this.ecidbox == string.Empty)
            {
                Application.DoEvents();
            }
        }

        public void update_1(object sender, DataReceivedEventArgs e)
        {
            this.UpdateTextBox(e.Data);
        }

        private void UpdateText()
        {
            try
            {
                this.ecidbox = this.Results;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                ProjectData.ClearProjectError();
            }
        }

        private void UpdateTextBox(string Tex)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    UpdateTextBoxDelegate method = new UpdateTextBoxDelegate(this.UpdateTextBox);
                    object[] args = new object[] { Tex };
                    this.Invoke(method, args);
                }
                else
                {
                    this.ecidbox = this.ecidbox + Tex;
                    this.ecidbox = this.ecidbox.Replace(" ", "");
                }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                ProjectData.ClearProjectError();
            }
        }

        private iDevice iPhoneInterface
        {
            get
            {
                return this._iPhoneInterface;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                ConnectEventHandler handler = new ConnectEventHandler(this.iPhoneInterface_Disconnect);
                ConnectEventHandler handler2 = new ConnectEventHandler(this.oniPhoneConnected);
                if (this._iPhoneInterface != null)
                {
                    this._iPhoneInterface.Disconnect -= handler;
                    this._iPhoneInterface.Connect -= handler2;
                }
                this._iPhoneInterface = value;
                if (this._iPhoneInterface != null)
                {
                    this._iPhoneInterface.Disconnect += handler;
                    this._iPhoneInterface.Connect += handler2;
                }
            }
        }

        private void iPhoneInterface_Disconnect(object sender, ConnectEventArgs args)
        {
        }

        private void oniPhoneConnected(object sender, ConnectEventArgs args)
        {
            MDIMain objMain = new MDIMain();
            if (!this.iLeft)
            {
                try
                {
                    string producttypevar = string.Empty;
                    string boardConfigvar = this.iPhoneInterface.CopyValue("HardwareModel").ToLower();
                    string str3 = Conversion.Hex(this.iPhoneInterface.CopyValue("UniqueChipID"));
                    string deviceNamevar = string.Empty;
                    int chipIDvar = 0;
                    int boardIDvar = 0;
                    while (str3.Length != 0x10)
                    {
                        str3 = "0" + str3;
                    }
                    iFaith.giveMeDetails(boardConfigvar, ref chipIDvar, ref boardIDvar, ref deviceNamevar, ref producttypevar);
                    while (this.iAcquaIsBusy)
                    {
                        Application.DoEvents();
                    }
                    this.iAcquaIsBusy = true;
                    if (Interaction.MsgBox("Name: " + this.iPhoneInterface.CopyValue("DeviceName") + "\rType: " + deviceNamevar + " [" + producttypevar + "]\rECID [HEX]: " + str3 + "\rECID [DEC]: " + Conversions.ToString(Convert.ToInt64(str3, 0x10)) + "\r\rWould you like to use the newly connected device?", MsgBoxStyle.Information | MsgBoxStyle.YesNo, null) == MsgBoxResult.Yes)
                    {
                        iFaith.ECID = str3;
                        this.iDeviceType = deviceNamevar;
                        this.ChipID = chipIDvar;
                        this.BoardID = boardIDvar;
                        this.iDeviceBoard = boardConfigvar;
                        this.iDeviceProductType = producttypevar;
                        objMain.Activate();
                        this.Invoke(new MethodInvoker(this.iAcquaDO));
                    }
                    else
                    {
                        this.iAcquaIsBusy = false;
                    }
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    ProjectData.ClearProjectError();
                }
            }
        }

        private delegate void delUpdate();

        private delegate void iPhoneConnectedEventHandler();

        private delegate void iPhoneDisconnectedEventHandler();

        private delegate void UpdateTextBoxDelegate(string Text);

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            this.TextBox1.Text = "";
            this.TextBox1.ForeColor = Color.Crimson;
        }

        private void goback_Click(object sender, EventArgs e)
        {
            objFormMain.Show();
            this.Dispose();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.iJustDidBlobs = false;
            this.Invoke(new MethodInvoker(this.Cleanup));
            this.iPhoneInterface = new iDevice();
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((this.ListBox1.SelectedIndex != 0) & (this.TextBox1.Text.Replace(" ", "") != string.Empty)) & !this.TextBox1.Text.Contains("Enter an"))
            {
                this.manualecidBTN.Enabled = true;
            }
            else
            {
                this.manualecidBTN.Enabled = false;
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if ((this.TextBox1.Text == "Enter an ECID here...") | (this.TextBox1.Text == string.Empty))
            {
                this.manualecidBTN.Enabled = false;
            }
            else if (((this.ListBox1.SelectedIndex != 0) & (this.TextBox1.Text.Replace(" ", "") != string.Empty)) & !this.TextBox1.Text.Contains("Enter an"))
            {
                this.manualecidBTN.Enabled = true;
            }
        }

        private void labelLicence_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox("See attached file LICENSE in iFaith folder.", MsgBoxStyle.Information, null);
        }

    }
}
