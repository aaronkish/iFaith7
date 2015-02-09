namespace iFaith
{
    //using iFaith.My;
    using Ionic.Zip;
    using Microsoft.VisualBasic.CompilerServices;
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using Microsoft.VisualBasic;

   
    public class MDIMain : Form
    {
        private IContainer components;
        public object ThisBuildIsNotPublic;
        public BackgroundWorker InstallVCPP;
        public BackgroundWorker BackgroundWorker1;
        private Panel panel2;
        public Label Label1;
        private PictureBox PictureBox1;
        private Label label2;
        public Label dfuinstructionstxt;
        private RichTextBox idetect;
        private Label Prepare;
        private Label labelTimer1;
        public ProgressBar ProgressBar1;
        private Label labelNonAppleTVMode;
        private Label labelAppleTVMode;
        private Button buttonRestartDump;
        private Button buttonStartDump;
        private Label labelTool;
        private Label labelLicence;
        private Button buttonCredit;
        private Button buttonNonAppleTV;
        private Button buttonAppleTV;
        public Button Button1;
        public RadioButton dumpbtn;
        public Label dumptxt;
        public RadioButton dfuinstructions;
        public Label uploadibectxt;
        public RadioButton uploadibecbtn;
        public Label uploadibsstxt;
        public RadioButton uploadibssbtn;
        public Label createipswtxt;
        public RadioButton Prepbtn;
        public Label exploittxt;
        public RadioButton exploitbtn;
        public Label preptxt;
        public RadioButton createipsw;
        public string VersionNumber;
        private iFaith objiFaith;
        public PictureBox PictureBox2;
        public RichTextBox blue;
        private Panel panel1;
        bool appletvMode;

        public MDIMain()
        {
            base.FormClosing += new FormClosingEventHandler(this.MDIMain_ByeBye);
            base.Load += new EventHandler(this.MDIMain_Load);
            this.VersionNumber = "1.5.9";
            this.ThisBuildIsNotPublic = false;
            this.InitializeComponent();
            
            PictureBox1.Visible = false;
            Prepare.Text = "";
            dfuinstructionstxt.Text = "";
            label2.Text = "";
            labelTimer1.Visible = false;
            ProgressBar1.Visible = false;
            buttonStartDump.Visible = false;
            buttonRestartDump.Visible = false;
            objiFaith = new iFaith();


        }
        private void InitializeComponent()
        {   
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIMain));
            this.InstallVCPP = new System.ComponentModel.BackgroundWorker();
            this.BackgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.blue = new System.Windows.Forms.RichTextBox();
            this.PictureBox2 = new System.Windows.Forms.PictureBox();
            this.Button1 = new System.Windows.Forms.Button();
            this.dumpbtn = new System.Windows.Forms.RadioButton();
            this.dumptxt = new System.Windows.Forms.Label();
            this.dfuinstructions = new System.Windows.Forms.RadioButton();
            this.uploadibectxt = new System.Windows.Forms.Label();
            this.uploadibecbtn = new System.Windows.Forms.RadioButton();
            this.uploadibsstxt = new System.Windows.Forms.Label();
            this.uploadibssbtn = new System.Windows.Forms.RadioButton();
            this.createipswtxt = new System.Windows.Forms.Label();
            this.Prepbtn = new System.Windows.Forms.RadioButton();
            this.exploittxt = new System.Windows.Forms.Label();
            this.exploitbtn = new System.Windows.Forms.RadioButton();
            this.preptxt = new System.Windows.Forms.Label();
            this.createipsw = new System.Windows.Forms.RadioButton();
            this.Label1 = new System.Windows.Forms.Label();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dfuinstructionstxt = new System.Windows.Forms.Label();
            this.idetect = new System.Windows.Forms.RichTextBox();
            this.Prepare = new System.Windows.Forms.Label();
            this.labelTimer1 = new System.Windows.Forms.Label();
            this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelNonAppleTVMode = new System.Windows.Forms.Label();
            this.labelAppleTVMode = new System.Windows.Forms.Label();
            this.buttonRestartDump = new System.Windows.Forms.Button();
            this.buttonStartDump = new System.Windows.Forms.Button();
            this.labelTool = new System.Windows.Forms.Label();
            this.labelLicence = new System.Windows.Forms.Label();
            this.buttonCredit = new System.Windows.Forms.Button();
            this.buttonNonAppleTV = new System.Windows.Forms.Button();
            this.buttonAppleTV = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // InstallVCPP
            // 
            this.InstallVCPP.DoWork += new System.ComponentModel.DoWorkEventHandler(this.InstallVCPP_DoWork);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.blue);
            this.panel2.Controls.Add(this.PictureBox2);
            this.panel2.Controls.Add(this.Button1);
            this.panel2.Controls.Add(this.dumpbtn);
            this.panel2.Controls.Add(this.dumptxt);
            this.panel2.Controls.Add(this.dfuinstructions);
            this.panel2.Controls.Add(this.uploadibectxt);
            this.panel2.Controls.Add(this.uploadibecbtn);
            this.panel2.Controls.Add(this.uploadibsstxt);
            this.panel2.Controls.Add(this.uploadibssbtn);
            this.panel2.Controls.Add(this.createipswtxt);
            this.panel2.Controls.Add(this.Prepbtn);
            this.panel2.Controls.Add(this.exploittxt);
            this.panel2.Controls.Add(this.exploitbtn);
            this.panel2.Controls.Add(this.preptxt);
            this.panel2.Controls.Add(this.createipsw);
            this.panel2.Controls.Add(this.Label1);
            this.panel2.Controls.Add(this.PictureBox1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.dfuinstructionstxt);
            this.panel2.Controls.Add(this.idetect);
            this.panel2.Controls.Add(this.Prepare);
            this.panel2.Controls.Add(this.labelTimer1);
            this.panel2.Controls.Add(this.ProgressBar1);
            this.panel2.Controls.Add(this.labelNonAppleTVMode);
            this.panel2.Controls.Add(this.labelAppleTVMode);
            this.panel2.Controls.Add(this.buttonRestartDump);
            this.panel2.Controls.Add(this.buttonStartDump);
            this.panel2.Controls.Add(this.labelTool);
            this.panel2.Controls.Add(this.labelLicence);
            this.panel2.Controls.Add(this.buttonCredit);
            this.panel2.Controls.Add(this.buttonNonAppleTV);
            this.panel2.Controls.Add(this.buttonAppleTV);
            this.panel2.Location = new System.Drawing.Point(12, 11);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(921, 355);
            this.panel2.TabIndex = 17;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.GhostWhite;
            this.panel1.Location = new System.Drawing.Point(725, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(12, 355);
            this.panel1.TabIndex = 44;
            // 
            // blue
            // 
            this.blue.BackColor = System.Drawing.Color.Firebrick;
            this.blue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.blue.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.blue.DetectUrls = false;
            this.blue.Location = new System.Drawing.Point(7, 315);
            this.blue.Multiline = false;
            this.blue.Name = "blue";
            this.blue.ReadOnly = true;
            this.blue.Size = new System.Drawing.Size(10, 10);
            this.blue.TabIndex = 43;
            this.blue.Text = "";
            this.blue.Visible = false;
            // 
            // PictureBox2
            // 
            this.PictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PictureBox2.Image = global::iFaith.Properties.Resources.connectatv2;
            this.PictureBox2.Location = new System.Drawing.Point(427, 50);
            this.PictureBox2.Name = "PictureBox2";
            this.PictureBox2.Size = new System.Drawing.Size(181, 184);
            this.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox2.TabIndex = 42;
            this.PictureBox2.TabStop = false;
            this.PictureBox2.Visible = false;
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(793, 302);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(75, 23);
            this.Button1.TabIndex = 41;
            this.Button1.Text = "Button1";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Visible = false;
            // 
            // dumpbtn
            // 
            this.dumpbtn.AutoSize = true;
            this.dumpbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dumpbtn.ForeColor = System.Drawing.Color.Crimson;
            this.dumpbtn.Location = new System.Drawing.Point(743, 142);
            this.dumpbtn.Name = "dumpbtn";
            this.dumpbtn.Size = new System.Drawing.Size(57, 17);
            this.dumpbtn.TabIndex = 40;
            this.dumpbtn.TabStop = true;
            this.dumpbtn.Text = "Dump";
            this.dumpbtn.UseVisualStyleBackColor = true;
            this.dumpbtn.Visible = false;
            // 
            // dumptxt
            // 
            this.dumptxt.AutoSize = true;
            this.dumptxt.Location = new System.Drawing.Point(839, 146);
            this.dumptxt.Name = "dumptxt";
            this.dumptxt.Size = new System.Drawing.Size(44, 13);
            this.dumptxt.TabIndex = 39;
            this.dumptxt.Text = "dumptxt";
            this.dumptxt.Visible = false;
            // 
            // dfuinstructions
            // 
            this.dfuinstructions.AutoSize = true;
            this.dfuinstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfuinstructions.ForeColor = System.Drawing.Color.Crimson;
            this.dfuinstructions.Location = new System.Drawing.Point(743, 27);
            this.dfuinstructions.Name = "dfuinstructions";
            this.dfuinstructions.Size = new System.Drawing.Size(120, 17);
            this.dfuinstructions.TabIndex = 37;
            this.dfuinstructions.TabStop = true;
            this.dfuinstructions.Text = "DFU Instructions";
            this.dfuinstructions.UseVisualStyleBackColor = true;
            this.dfuinstructions.Visible = false;
            // 
            // uploadibectxt
            // 
            this.uploadibectxt.AutoSize = true;
            this.uploadibectxt.Location = new System.Drawing.Point(839, 123);
            this.uploadibectxt.Name = "uploadibectxt";
            this.uploadibectxt.Size = new System.Drawing.Size(70, 13);
            this.uploadibectxt.TabIndex = 36;
            this.uploadibectxt.Text = "uploadibectxt";
            this.uploadibectxt.Visible = false;
            // 
            // uploadibecbtn
            // 
            this.uploadibecbtn.AutoSize = true;
            this.uploadibecbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uploadibecbtn.ForeColor = System.Drawing.Color.Crimson;
            this.uploadibecbtn.Location = new System.Drawing.Point(743, 119);
            this.uploadibecbtn.Name = "uploadibecbtn";
            this.uploadibecbtn.Size = new System.Drawing.Size(96, 17);
            this.uploadibecbtn.TabIndex = 35;
            this.uploadibecbtn.TabStop = true;
            this.uploadibecbtn.Text = "Upload iBEC";
            this.uploadibecbtn.UseVisualStyleBackColor = true;
            this.uploadibecbtn.Visible = false;
            // 
            // uploadibsstxt
            // 
            this.uploadibsstxt.AutoSize = true;
            this.uploadibsstxt.Location = new System.Drawing.Point(839, 100);
            this.uploadibsstxt.Name = "uploadibsstxt";
            this.uploadibsstxt.Size = new System.Drawing.Size(68, 13);
            this.uploadibsstxt.TabIndex = 34;
            this.uploadibsstxt.Text = "uploadibsstxt";
            this.uploadibsstxt.Visible = false;
            // 
            // uploadibssbtn
            // 
            this.uploadibssbtn.AutoSize = true;
            this.uploadibssbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uploadibssbtn.ForeColor = System.Drawing.Color.Crimson;
            this.uploadibssbtn.Location = new System.Drawing.Point(743, 96);
            this.uploadibssbtn.Name = "uploadibssbtn";
            this.uploadibssbtn.Size = new System.Drawing.Size(96, 17);
            this.uploadibssbtn.TabIndex = 33;
            this.uploadibssbtn.TabStop = true;
            this.uploadibssbtn.Text = "Upload iBSS";
            this.uploadibssbtn.UseVisualStyleBackColor = true;
            this.uploadibssbtn.Visible = false;
            // 
            // createipswtxt
            // 
            this.createipswtxt.AutoSize = true;
            this.createipswtxt.Location = new System.Drawing.Point(838, 169);
            this.createipswtxt.Name = "createipswtxt";
            this.createipswtxt.Size = new System.Drawing.Size(69, 13);
            this.createipswtxt.TabIndex = 32;
            this.createipswtxt.Text = "createipswtxt";
            this.createipswtxt.Visible = false;
            // 
            // Prepbtn
            // 
            this.Prepbtn.AutoSize = true;
            this.Prepbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Prepbtn.ForeColor = System.Drawing.Color.Crimson;
            this.Prepbtn.Location = new System.Drawing.Point(743, 50);
            this.Prepbtn.Name = "Prepbtn";
            this.Prepbtn.Size = new System.Drawing.Size(96, 17);
            this.Prepbtn.TabIndex = 31;
            this.Prepbtn.TabStop = true;
            this.Prepbtn.Text = "Preparations";
            this.Prepbtn.UseVisualStyleBackColor = true;
            this.Prepbtn.Visible = false;
            // 
            // exploittxt
            // 
            this.exploittxt.AutoSize = true;
            this.exploittxt.Location = new System.Drawing.Point(839, 75);
            this.exploittxt.Name = "exploittxt";
            this.exploittxt.Size = new System.Drawing.Size(48, 13);
            this.exploittxt.TabIndex = 30;
            this.exploittxt.Text = "exploittxt";
            this.exploittxt.Visible = false;
            // 
            // exploitbtn
            // 
            this.exploitbtn.AutoSize = true;
            this.exploitbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exploitbtn.ForeColor = System.Drawing.Color.Crimson;
            this.exploitbtn.Location = new System.Drawing.Point(743, 73);
            this.exploitbtn.Name = "exploitbtn";
            this.exploitbtn.Size = new System.Drawing.Size(91, 17);
            this.exploitbtn.TabIndex = 29;
            this.exploitbtn.TabStop = true;
            this.exploitbtn.Text = "Exploitation";
            this.exploitbtn.UseVisualStyleBackColor = true;
            this.exploitbtn.Visible = false;
            // 
            // preptxt
            // 
            this.preptxt.AutoSize = true;
            this.preptxt.Location = new System.Drawing.Point(839, 54);
            this.preptxt.Name = "preptxt";
            this.preptxt.Size = new System.Drawing.Size(39, 13);
            this.preptxt.TabIndex = 28;
            this.preptxt.Text = "preptxt";
            this.preptxt.Visible = false;
            // 
            // createipsw
            // 
            this.createipsw.AutoSize = true;
            this.createipsw.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createipsw.ForeColor = System.Drawing.Color.Crimson;
            this.createipsw.Location = new System.Drawing.Point(743, 165);
            this.createipsw.Name = "createipsw";
            this.createipsw.Size = new System.Drawing.Size(86, 17);
            this.createipsw.TabIndex = 27;
            this.createipsw.TabStop = true;
            this.createipsw.Text = "createipsw";
            this.createipsw.UseVisualStyleBackColor = true;
            this.createipsw.Visible = false;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.Color.Crimson;
            this.Label1.Location = new System.Drawing.Point(142, 237);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(57, 17);
            this.Label1.TabIndex = 26;
            this.Label1.Text = "Label1";
            this.Label1.Visible = false;
            // 
            // PictureBox1
            // 
            this.PictureBox1.Image = global::iFaith.Properties.Resources.turnoff;
            this.PictureBox1.Location = new System.Drawing.Point(300, 50);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(109, 184);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox1.TabIndex = 25;
            this.PictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Crimson;
            this.label2.Location = new System.Drawing.Point(142, 332);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 17);
            this.label2.TabIndex = 22;
            this.label2.Text = "label2";
            // 
            // dfuinstructionstxt
            // 
            this.dfuinstructionstxt.AutoSize = true;
            this.dfuinstructionstxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfuinstructionstxt.ForeColor = System.Drawing.Color.Crimson;
            this.dfuinstructionstxt.Location = new System.Drawing.Point(142, 254);
            this.dfuinstructionstxt.Name = "dfuinstructionstxt";
            this.dfuinstructionstxt.Size = new System.Drawing.Size(132, 17);
            this.dfuinstructionstxt.TabIndex = 21;
            this.dfuinstructionstxt.Text = "dfuinstructionstxt";
            // 
            // idetect
            // 
            this.idetect.BackColor = System.Drawing.Color.IndianRed;
            this.idetect.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.idetect.DetectUrls = false;
            this.idetect.Location = new System.Drawing.Point(23, 315);
            this.idetect.Multiline = false;
            this.idetect.Name = "idetect";
            this.idetect.ReadOnly = true;
            this.idetect.Size = new System.Drawing.Size(10, 10);
            this.idetect.TabIndex = 18;
            this.idetect.Text = "";
            this.idetect.Visible = false;
            // 
            // Prepare
            // 
            this.Prepare.AutoSize = true;
            this.Prepare.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Prepare.ForeColor = System.Drawing.Color.Crimson;
            this.Prepare.Location = new System.Drawing.Point(142, 271);
            this.Prepare.Name = "Prepare";
            this.Prepare.Size = new System.Drawing.Size(66, 17);
            this.Prepare.TabIndex = 17;
            this.Prepare.Text = "Prepare";
            // 
            // labelTimer1
            // 
            this.labelTimer1.AutoSize = true;
            this.labelTimer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 60F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTimer1.ForeColor = System.Drawing.Color.Indigo;
            this.labelTimer1.Location = new System.Drawing.Point(584, 133);
            this.labelTimer1.Name = "labelTimer1";
            this.labelTimer1.Size = new System.Drawing.Size(127, 91);
            this.labelTimer1.TabIndex = 16;
            this.labelTimer1.Text = "00";
            this.labelTimer1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ProgressBar1
            // 
            this.ProgressBar1.ForeColor = System.Drawing.Color.Crimson;
            this.ProgressBar1.Location = new System.Drawing.Point(145, 291);
            this.ProgressBar1.Name = "ProgressBar1";
            this.ProgressBar1.Size = new System.Drawing.Size(426, 5);
            this.ProgressBar1.TabIndex = 15;
            this.ProgressBar1.Visible = false;
            // 
            // labelNonAppleTVMode
            // 
            this.labelNonAppleTVMode.AutoSize = true;
            this.labelNonAppleTVMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNonAppleTVMode.ForeColor = System.Drawing.Color.Crimson;
            this.labelNonAppleTVMode.Location = new System.Drawing.Point(598, 7);
            this.labelNonAppleTVMode.Name = "labelNonAppleTVMode";
            this.labelNonAppleTVMode.Size = new System.Drawing.Size(121, 13);
            this.labelNonAppleTVMode.TabIndex = 14;
            this.labelNonAppleTVMode.Text = "Non Apple TV Mode";
            this.labelNonAppleTVMode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelNonAppleTVMode.Visible = false;
            // 
            // labelAppleTVMode
            // 
            this.labelAppleTVMode.AutoSize = true;
            this.labelAppleTVMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAppleTVMode.ForeColor = System.Drawing.Color.Crimson;
            this.labelAppleTVMode.Location = new System.Drawing.Point(623, 7);
            this.labelAppleTVMode.Name = "labelAppleTVMode";
            this.labelAppleTVMode.Size = new System.Drawing.Size(94, 13);
            this.labelAppleTVMode.TabIndex = 13;
            this.labelAppleTVMode.Text = "Apple TV Mode";
            this.labelAppleTVMode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelAppleTVMode.Visible = false;
            // 
            // buttonRestartDump
            // 
            this.buttonRestartDump.Enabled = false;
            this.buttonRestartDump.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRestartDump.ForeColor = System.Drawing.Color.Crimson;
            this.buttonRestartDump.Location = new System.Drawing.Point(361, 302);
            this.buttonRestartDump.Name = "buttonRestartDump";
            this.buttonRestartDump.Size = new System.Drawing.Size(210, 23);
            this.buttonRestartDump.TabIndex = 12;
            this.buttonRestartDump.Text = "Restart";
            this.buttonRestartDump.UseVisualStyleBackColor = true;
            // 
            // buttonStartDump
            // 
            this.buttonStartDump.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStartDump.ForeColor = System.Drawing.Color.Crimson;
            this.buttonStartDump.Location = new System.Drawing.Point(145, 302);
            this.buttonStartDump.Name = "buttonStartDump";
            this.buttonStartDump.Size = new System.Drawing.Size(210, 23);
            this.buttonStartDump.TabIndex = 11;
            this.buttonStartDump.Text = "Start";
            this.buttonStartDump.UseVisualStyleBackColor = true;
            // 
            // labelTool
            // 
            this.labelTool.AutoSize = true;
            this.labelTool.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTool.ForeColor = System.Drawing.Color.Crimson;
            this.labelTool.Location = new System.Drawing.Point(3, 0);
            this.labelTool.Name = "labelTool";
            this.labelTool.Size = new System.Drawing.Size(111, 20);
            this.labelTool.TabIndex = 10;
            this.labelTool.Text = "Dump SHSH";
            // 
            // labelLicence
            // 
            this.labelLicence.AutoSize = true;
            this.labelLicence.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelLicence.ForeColor = System.Drawing.Color.Crimson;
            this.labelLicence.Location = new System.Drawing.Point(3, 339);
            this.labelLicence.Name = "labelLicence";
            this.labelLicence.Size = new System.Drawing.Size(40, 13);
            this.labelLicence.TabIndex = 9;
            this.labelLicence.Text = "GPLv3";
            this.labelLicence.Click += new System.EventHandler(this.labelLicence_Click);
            // 
            // buttonCredit
            // 
            this.buttonCredit.BackColor = System.Drawing.Color.Gainsboro;
            this.buttonCredit.ForeColor = System.Drawing.Color.Crimson;
            this.buttonCredit.Location = new System.Drawing.Point(694, 329);
            this.buttonCredit.Name = "buttonCredit";
            this.buttonCredit.Size = new System.Drawing.Size(23, 23);
            this.buttonCredit.TabIndex = 8;
            this.buttonCredit.Text = "@";
            this.buttonCredit.UseVisualStyleBackColor = false;
            // 
            // buttonNonAppleTV
            // 
            this.buttonNonAppleTV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNonAppleTV.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNonAppleTV.ForeColor = System.Drawing.Color.Crimson;
            this.buttonNonAppleTV.Location = new System.Drawing.Point(361, 50);
            this.buttonNonAppleTV.Name = "buttonNonAppleTV";
            this.buttonNonAppleTV.Size = new System.Drawing.Size(210, 40);
            this.buttonNonAppleTV.TabIndex = 6;
            this.buttonNonAppleTV.Text = "Non Apple TV";
            this.buttonNonAppleTV.UseVisualStyleBackColor = true;
            this.buttonNonAppleTV.Click += new System.EventHandler(this.buttonNonAppleTV_Click);
            // 
            // buttonAppleTV
            // 
            this.buttonAppleTV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAppleTV.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAppleTV.ForeColor = System.Drawing.Color.Crimson;
            this.buttonAppleTV.Location = new System.Drawing.Point(145, 50);
            this.buttonAppleTV.Name = "buttonAppleTV";
            this.buttonAppleTV.Size = new System.Drawing.Size(210, 40);
            this.buttonAppleTV.TabIndex = 7;
            this.buttonAppleTV.Text = "Apple TV";
            this.buttonAppleTV.UseVisualStyleBackColor = true;
            this.buttonAppleTV.Click += new System.EventHandler(this.buttonAppleTV_Click);
            // 
            // MDIMain
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(945, 378);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MDIMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "iFaith for iOS 7";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MDIMain_FormClosed);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);

}

        [DebuggerStepThrough, CompilerGenerated]
        private void _Lambda__5(object a0)
        {
            this.Check4Updates();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            iFaith.Hide_Stages();
            DFU dfu = new DFU(null);
            dfu.CleaniREB();
        }

        private void Check4Updates()
        {
            WebClient client = new WebClient();
            client.DownloadStringAsync(new Uri("http://raw.github.com/iH8sn0w/versions/master/iFaith"));
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.DownloadStringCompleted_CheckForUpdates);
        }

        [DebuggerNonUserCode]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && (this.components != null))
                {
                    this.components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        private void DownloadStringCompleted_CheckForUpdates(object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled && (e.Error == null))
            {
                string str = e.Result.Replace("\n", "");
                if (str != this.VersionNumber)
                {
                    iFaith.clsTopMostMessageBox.Show("iFaith-v" + this.VersionNumber, "You are currently running iFaith-v" + this.VersionNumber + ".\r\riFaith-v" + str + " is available for download at http://iH8sn0w.com", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        

        private void InstallVCPP_DoWork(object sender, DoWorkEventArgs e)
        {
            modFile.SaveToDisk("resources/vcpp.zip", iFaith.temppath + @"\vcpp.zip");
            using (ZipFile file = ZipFile.Read(iFaith.temppath + @"\vcpp.zip"))
            {
                file.ExtractAll(iFaith.temppath + @"\");
                file.Dispose();
            }
            modProcessCmd.cmdline = iFaith.Quotation + iFaith.temppath + @"\install.exe" + iFaith.Quotation + " /q";
            modProcessCmd.ExecCmd(modProcessCmd.cmdline, true);
        }

        private void MDIMain_ByeBye(object Sender, EventArgs e)
        {
            //MyProject.Forms.iAcqua.iLeft = true;
            iFaith.OhNoesShutDOWN = true;
            iFaith.KilliRecovery();
            string strError = "";
            modFile.Folder_Delete(iFaith.temppath, ref strError);
            //Process.GetCurrentProcess().Kill();
            this.Dispose();
        }

        private void MDIMain_Load(object sender, EventArgs e)
        {
            IEnumerator enumerator = null;
            modFile.SetMdiClientBorder(false);
            try
            {
                enumerator = this.Controls.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Control current = (Control) enumerator.Current;
                    if (current is MdiClient)
                    {
                        current.BackColor = this.BackColor;
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
            /*Form form = new Form();
            form.MdiParent = this;
            form.Width = this.Width / 2;
            form.Height = this.Height / 2;
            form.Show();
            form.Hide();*/
            this.Text = "iFaith v" + this.VersionNumber + " -- By: iH8sn0w";
            /*MyProject.Forms.Welcome.MdiParent = this;
            MyProject.Forms.Welcome.Show();
            MyProject.Forms.Welcome.Button1.Enabled = false;
            MyProject.Forms.About.MdiParent = this;
            MyProject.Forms.About.Show();
            MyProject.Forms.About.BringToFront();*/
            string strError = "";
            modFile.Folder_Delete(iFaith.temppath, ref strError);
            strError = "";
            modFile.Create_Directory(iFaith.temppath, ref strError);
            string str2 = Conversions.ToString(Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion", true).GetValue("ProductName"));
            if (str2.Contains("Windows XP"))
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\{FF66E9F6-83E7-3A3E-AF14-8DE9A809A6A4}", true);
                try
                {
                    str2 = Conversions.ToString(key.GetValue("DisplayName"));
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    ProjectData.ClearProjectError();
                }
                if (!str2.Contains("C++"))
                {
                    this.InstallVCPP.RunWorkerAsync();
                }
            }
            ThreadPool.QueueUserWorkItem(new WaitCallback(this._Lambda__5));
        }


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


        private void buttonAppleTV_Click(object sender, EventArgs e)
        {
            modProcessCmd.iDevice = "Apple TV 2";
            iFaith.atv2mode = true;
            this.appletvMode = true;
            this.labelAppleTVMode.Show();
            this.buttonAppleTV.Hide();
            this.buttonNonAppleTV.Hide();
            this.buttonStartDump.Show();
            this.buttonRestartDump.Show();
            this.Label1.Visible = true;
            this.Label1.Text = "Plug-in your Apple TV 2 via USB.";
            this.dfuinstructionstxt.Text = "Press Start only when your Apple TV 2 is blinking.";
            this.PictureBox2.Visible = true;
            LoadGUI();
        }

        private void buttonNonAppleTV_Click(object sender, EventArgs e)
        {
            modProcessCmd.iDevice = "";
            iFaith.atv2mode = false;
            this.appletvMode = false;
            this.labelNonAppleTVMode.Show();
            this.buttonAppleTV.Hide();
            this.buttonNonAppleTV.Hide();
            this.buttonStartDump.Show();
            this.buttonRestartDump.Show();
            this.Label1.Visible = true;
            this.Label1.Text = "Please power off your iDevice.\r\nThen press \"Start\".";
            this.PictureBox1.Visible = true;

        }

        private void MDIMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormMain objMain = new FormMain();
            objMain.Show();
            this.Dispose();
        }

        private void labelLicence_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox("See attached file LICENSE in iFaith folder.", MsgBoxStyle.Information, null);
        }
    }

    
}

