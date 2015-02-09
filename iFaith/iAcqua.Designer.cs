namespace iFaith
{
    partial class iAcqua
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(iAcqua));
            this.BackgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.manualecidBTN = new System.Windows.Forms.Button();
            this.labelTool = new System.Windows.Forms.Label();
            this.labelLicence = new System.Windows.Forms.Label();
            this.buttonCredit = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.dlblobBTN = new System.Windows.Forms.Button();
            this.dlallBTN = new System.Windows.Forms.Button();
            this.Label3 = new System.Windows.Forms.Label();
            this.iphone = new System.Windows.Forms.PictureBox();
            this.Button1 = new System.Windows.Forms.Button();
            this.goback = new System.Windows.Forms.Button();
            this.availableshsh = new System.Windows.Forms.CheckedListBox();
            this.spinny = new System.Windows.Forms.PictureBox();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.ListBox1 = new System.Windows.Forms.ListBox();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iphone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinny)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // manualecidBTN
            // 
            this.manualecidBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.manualecidBTN.ForeColor = System.Drawing.Color.Crimson;
            this.manualecidBTN.Location = new System.Drawing.Point(361, 302);
            this.manualecidBTN.Name = "manualecidBTN";
            this.manualecidBTN.Size = new System.Drawing.Size(210, 23);
            this.manualecidBTN.TabIndex = 12;
            this.manualecidBTN.Text = "Show Available Blobs";
            this.manualecidBTN.UseVisualStyleBackColor = true;
            this.manualecidBTN.Click += new System.EventHandler(this.buttonRestartDFU_Click);
            // 
            // labelTool
            // 
            this.labelTool.AutoSize = true;
            this.labelTool.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTool.ForeColor = System.Drawing.Color.Crimson;
            this.labelTool.Location = new System.Drawing.Point(3, 0);
            this.labelTool.Name = "labelTool";
            this.labelTool.Size = new System.Drawing.Size(64, 20);
            this.labelTool.TabIndex = 10;
            this.labelTool.Text = "iAcqua";
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
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.Controls.Add(this.dlblobBTN);
            this.panel2.Controls.Add(this.dlallBTN);
            this.panel2.Controls.Add(this.Label3);
            this.panel2.Controls.Add(this.iphone);
            this.panel2.Controls.Add(this.Button1);
            this.panel2.Controls.Add(this.goback);
            this.panel2.Controls.Add(this.availableshsh);
            this.panel2.Controls.Add(this.spinny);
            this.panel2.Controls.Add(this.PictureBox1);
            this.panel2.Controls.Add(this.ListBox1);
            this.panel2.Controls.Add(this.TextBox1);
            this.panel2.Controls.Add(this.Label2);
            this.panel2.Controls.Add(this.manualecidBTN);
            this.panel2.Controls.Add(this.labelTool);
            this.panel2.Controls.Add(this.labelLicence);
            this.panel2.Controls.Add(this.buttonCredit);
            this.panel2.Location = new System.Drawing.Point(11, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(720, 355);
            this.panel2.TabIndex = 7;
            // 
            // dlblobBTN
            // 
            this.dlblobBTN.Location = new System.Drawing.Point(280, 272);
            this.dlblobBTN.Name = "dlblobBTN";
            this.dlblobBTN.Size = new System.Drawing.Size(153, 23);
            this.dlblobBTN.TabIndex = 24;
            this.dlblobBTN.Text = "Download selected blob(s)";
            this.dlblobBTN.UseVisualStyleBackColor = true;
            this.dlblobBTN.Visible = false;
            // 
            // dlallBTN
            // 
            this.dlallBTN.Location = new System.Drawing.Point(280, 248);
            this.dlallBTN.Name = "dlallBTN";
            this.dlallBTN.Size = new System.Drawing.Size(153, 23);
            this.dlallBTN.TabIndex = 23;
            this.dlallBTN.Text = "Download all available blobs";
            this.dlallBTN.UseVisualStyleBackColor = true;
            this.dlallBTN.Visible = false;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.ForeColor = System.Drawing.Color.Crimson;
            this.Label3.Location = new System.Drawing.Point(328, 203);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(49, 29);
            this.Label3.TabIndex = 22;
            this.Label3.Text = "OR";
            this.Label3.Visible = false;
            // 
            // iphone
            // 
            this.iphone.Location = new System.Drawing.Point(7, 141);
            this.iphone.Name = "iphone";
            this.iphone.Size = new System.Drawing.Size(60, 67);
            this.iphone.TabIndex = 21;
            this.iphone.TabStop = false;
            // 
            // Button1
            // 
            this.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button1.ForeColor = System.Drawing.Color.Crimson;
            this.Button1.Location = new System.Drawing.Point(361, 302);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(210, 23);
            this.Button1.TabIndex = 20;
            this.Button1.Text = "Return to ECID Entry";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Visible = false;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // goback
            // 
            this.goback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.goback.ForeColor = System.Drawing.Color.Crimson;
            this.goback.Location = new System.Drawing.Point(145, 302);
            this.goback.Name = "goback";
            this.goback.Size = new System.Drawing.Size(210, 23);
            this.goback.TabIndex = 19;
            this.goback.Text = "Main Menu";
            this.goback.UseVisualStyleBackColor = true;
            this.goback.Click += new System.EventHandler(this.goback_Click);
            // 
            // availableshsh
            // 
            this.availableshsh.FormattingEnabled = true;
            this.availableshsh.Location = new System.Drawing.Point(249, 101);
            this.availableshsh.Name = "availableshsh";
            this.availableshsh.Size = new System.Drawing.Size(210, 139);
            this.availableshsh.Sorted = true;
            this.availableshsh.TabIndex = 18;
            this.availableshsh.Visible = false;
            // 
            // spinny
            // 
            this.spinny.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spinny.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.spinny.Image = global::iFaith.Properties.Resources.loading;
            this.spinny.InitialImage = global::iFaith.Properties.Resources.loading;
            this.spinny.Location = new System.Drawing.Point(344, 176);
            this.spinny.Name = "spinny";
            this.spinny.Size = new System.Drawing.Size(32, 32);
            this.spinny.TabIndex = 17;
            this.spinny.TabStop = false;
            this.spinny.Visible = false;
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
            this.PictureBox1.Location = new System.Drawing.Point(145, 131);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(177, 135);
            this.PictureBox1.TabIndex = 16;
            this.PictureBox1.TabStop = false;
            // 
            // ListBox1
            // 
            this.ListBox1.BackColor = System.Drawing.Color.White;
            this.ListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ListBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListBox1.ForeColor = System.Drawing.Color.Crimson;
            this.ListBox1.FormattingEnabled = true;
            this.ListBox1.Location = new System.Drawing.Point(394, 120);
            this.ListBox1.Name = "ListBox1";
            this.ListBox1.Size = new System.Drawing.Size(177, 169);
            this.ListBox1.TabIndex = 15;
            this.ListBox1.SelectedIndexChanged += new System.EventHandler(this.ListBox1_SelectedIndexChanged);
            // 
            // TextBox1
            // 
            this.TextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox1.ForeColor = System.Drawing.Color.Pink;
            this.TextBox1.Location = new System.Drawing.Point(394, 91);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(177, 23);
            this.TextBox1.TabIndex = 14;
            this.TextBox1.Text = "Enter an ECID here...";
            this.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox1.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            this.TextBox1.Enter += new System.EventHandler(this.TextBox1_Enter);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.ForeColor = System.Drawing.Color.Crimson;
            this.Label2.Location = new System.Drawing.Point(113, 29);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(495, 31);
            this.Label2.TabIndex = 13;
            this.Label2.Text = "Connect an iDevice or enter an ECID";
            // 
            // iAcqua
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(743, 378);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "iAcqua";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "iAcqua";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.iAcqua_FormClosed);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iphone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinny)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.ComponentModel.BackgroundWorker BackgroundWorker2;
        private System.Windows.Forms.Button manualecidBTN;
        private System.Windows.Forms.Label labelTool;
        private System.Windows.Forms.Label labelLicence;
        private System.Windows.Forms.Button buttonCredit;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.TextBox TextBox1;
        private System.Windows.Forms.ListBox ListBox1;
        private System.Windows.Forms.PictureBox PictureBox1;
        private System.Windows.Forms.PictureBox spinny;
        private System.Windows.Forms.CheckedListBox availableshsh;
        private System.Windows.Forms.Button goback;
        private System.Windows.Forms.Button Button1;
        private System.Windows.Forms.PictureBox iphone;
        private System.Windows.Forms.Button dlblobBTN;
        private System.Windows.Forms.Button dlallBTN;
        private System.Windows.Forms.Label Label3;
    }
}