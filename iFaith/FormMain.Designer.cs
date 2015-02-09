namespace iFaith
{
    partial class FormMain
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.panel2 = new System.Windows.Forms.Panel();
            this.progresstxt = new System.Windows.Forms.Label();
            this.labelLicence = new System.Windows.Forms.Label();
            this.buttonCredit = new System.Windows.Forms.Button();
            this.buttoniReb = new System.Windows.Forms.Button();
            this.buttonGetSHSH = new System.Windows.Forms.Button();
            this.buttonSHSHBlobDump = new System.Windows.Forms.Button();
            this.buttonCustomiOS = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.Controls.Add(this.progresstxt);
            this.panel2.Controls.Add(this.labelLicence);
            this.panel2.Controls.Add(this.buttonCredit);
            this.panel2.Controls.Add(this.buttoniReb);
            this.panel2.Controls.Add(this.buttonGetSHSH);
            this.panel2.Controls.Add(this.buttonSHSHBlobDump);
            this.panel2.Controls.Add(this.buttonCustomiOS);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(720, 355);
            this.panel2.TabIndex = 5;
            // 
            // progresstxt
            // 
            this.progresstxt.AutoSize = true;
            this.progresstxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progresstxt.ForeColor = System.Drawing.Color.Crimson;
            this.progresstxt.Location = new System.Drawing.Point(132, 310);
            this.progresstxt.Name = "progresstxt";
            this.progresstxt.Size = new System.Drawing.Size(99, 20);
            this.progresstxt.TabIndex = 10;
            this.progresstxt.Text = "progresstxt";
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
            // buttoniReb
            // 
            this.buttoniReb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttoniReb.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttoniReb.ForeColor = System.Drawing.Color.Crimson;
            this.buttoniReb.Location = new System.Drawing.Point(361, 193);
            this.buttoniReb.Name = "buttoniReb";
            this.buttoniReb.Size = new System.Drawing.Size(220, 40);
            this.buttoniReb.TabIndex = 7;
            this.buttoniReb.Text = "iReb (<iOS7)";
            this.buttoniReb.UseVisualStyleBackColor = true;
            this.buttoniReb.Click += new System.EventHandler(this.buttoniReb_Click);
            // 
            // buttonGetSHSH
            // 
            this.buttonGetSHSH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGetSHSH.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGetSHSH.ForeColor = System.Drawing.Color.Crimson;
            this.buttonGetSHSH.Location = new System.Drawing.Point(135, 193);
            this.buttonGetSHSH.Name = "buttonGetSHSH";
            this.buttonGetSHSH.Size = new System.Drawing.Size(220, 40);
            this.buttonGetSHSH.TabIndex = 6;
            this.buttonGetSHSH.Text = "Get SHSH from Servers";
            this.buttonGetSHSH.UseVisualStyleBackColor = true;
            this.buttonGetSHSH.Click += new System.EventHandler(this.buttonGetSHSH_Click);
            // 
            // buttonSHSHBlobDump
            // 
            this.buttonSHSHBlobDump.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSHSHBlobDump.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSHSHBlobDump.ForeColor = System.Drawing.Color.Crimson;
            this.buttonSHSHBlobDump.Location = new System.Drawing.Point(361, 147);
            this.buttonSHSHBlobDump.Name = "buttonSHSHBlobDump";
            this.buttonSHSHBlobDump.Size = new System.Drawing.Size(220, 40);
            this.buttonSHSHBlobDump.TabIndex = 5;
            this.buttonSHSHBlobDump.Text = "SHSH Blobs Dump";
            this.buttonSHSHBlobDump.UseVisualStyleBackColor = true;
            this.buttonSHSHBlobDump.Click += new System.EventHandler(this.buttonSHSHBlobDump_Click);
            // 
            // buttonCustomiOS
            // 
            this.buttonCustomiOS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCustomiOS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCustomiOS.ForeColor = System.Drawing.Color.Crimson;
            this.buttonCustomiOS.Location = new System.Drawing.Point(135, 147);
            this.buttonCustomiOS.Name = "buttonCustomiOS";
            this.buttonCustomiOS.Size = new System.Drawing.Size(220, 40);
            this.buttonCustomiOS.TabIndex = 4;
            this.buttonCustomiOS.Text = "Custom iOS";
            this.buttonCustomiOS.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.button5);
            this.panel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.panel1.Location = new System.Drawing.Point(12, 373);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(720, 355);
            this.panel1.TabIndex = 8;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Gainsboro;
            this.panel4.Controls.Add(this.button7);
            this.panel4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(720, 355);
            this.panel4.TabIndex = 8;
            // 
            // button7
            // 
            this.button7.FlatAppearance.BorderSize = 0;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.ForeColor = System.Drawing.Color.Crimson;
            this.button7.Location = new System.Drawing.Point(0, 166);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(720, 45);
            this.button7.TabIndex = 6;
            this.button7.Text = "Start";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Gainsboro;
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.button2);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.button4);
            this.panel3.Controls.Add(this.button6);
            this.panel3.Location = new System.Drawing.Point(0, -361);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(720, 355);
            this.panel3.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Crimson;
            this.label1.Location = new System.Drawing.Point(132, 310);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "progresstxt";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Crimson;
            this.label2.Location = new System.Drawing.Point(3, 339);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "GPLv3";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Gainsboro;
            this.button1.ForeColor = System.Drawing.Color.Crimson;
            this.button1.Location = new System.Drawing.Point(694, 329);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(23, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "@";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Crimson;
            this.button2.Location = new System.Drawing.Point(361, 193);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(220, 40);
            this.button2.TabIndex = 7;
            this.button2.Text = "iReb (<iOS7)";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttoniReb_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.Crimson;
            this.button3.Location = new System.Drawing.Point(135, 193);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(220, 40);
            this.button3.TabIndex = 6;
            this.button3.Text = "Get SHSH from Servers";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.buttonGetSHSH_Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.Crimson;
            this.button4.Location = new System.Drawing.Point(361, 147);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(220, 40);
            this.button4.TabIndex = 5;
            this.button4.Text = "SHSH Blobs Dump";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.buttonSHSHBlobDump_Click);
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.ForeColor = System.Drawing.Color.Crimson;
            this.button6.Location = new System.Drawing.Point(135, 147);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(220, 40);
            this.button6.TabIndex = 4;
            this.button6.Text = "Custom iOS";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.Color.Crimson;
            this.button5.Location = new System.Drawing.Point(0, 166);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(720, 45);
            this.button5.TabIndex = 6;
            this.button5.Text = "Start";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(743, 378);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "iFaith for iOS 7";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttoniReb;
        private System.Windows.Forms.Button buttonGetSHSH;
        private System.Windows.Forms.Button buttonSHSHBlobDump;
        private System.Windows.Forms.Button buttonCustomiOS;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button buttonCredit;
        private System.Windows.Forms.Label labelLicence;
        private System.Windows.Forms.Label progresstxt;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button6;
    }
}

