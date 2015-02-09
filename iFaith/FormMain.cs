using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace iFaith
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            Point initPanel = new Point(12, 12);
            panel1.Location = initPanel;
            this.progresstxt.Text = "";
        }



        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            this.panel1.Hide();
        }

        private void buttoniReb_Click(object sender, EventArgs e)
        {
            DFU dfuObject = new DFU(this);
            dfuObject.Width = 759;
            dfuObject.panel2.Width = 720;
            dfuObject.Show();
            this.Hide();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
            this.Dispose();
        }

        public void setProgresstxt(String text)
        {
            this.progresstxt.Text = text;
        }

        private void buttonGetSHSH_Click(object sender, EventArgs e)
        {
            iAcqua iacquaObject = new iAcqua(this);
            iacquaObject.Show();
            this.Hide();
        }

        private void buttonSHSHBlobDump_Click(object sender, EventArgs e)
        {
            DumpSHSH dumpSHSHObject = new DumpSHSH(this);
            dumpSHSHObject.Show();
            this.Hide();
        }

        private void labelLicence_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox("See attached file LICENSE in iFaith folder.", MsgBoxStyle.Information, null);
        }

        
    }
}
