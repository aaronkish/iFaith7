using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using iFaith.My;
using iFaith.My.Resources;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace iFaith
{
    public partial class DumpSHSH : Form
    {
        FormMain objFormMain;

        public DumpSHSH(FormMain vobjFormMain)
        {
            InitializeComponent();
            objFormMain = vobjFormMain;
        }

        private void DumpSHSH_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (objFormMain != null)
            {
                objFormMain.Show();
            }
            else
            {
                objFormMain = new FormMain();
                objFormMain.Show();
            }
            this.Dispose();
        }

        private void goback_Click(object sender, EventArgs e)
        {
            if (objFormMain != null)
            {
                objFormMain.Show();
            }
            else
            {
                objFormMain = new FormMain();
                objFormMain.Show();
            }
            this.Dispose();
        }

        private void Proceed_Click(object sender, EventArgs e)
        {
            this.note.Visible = false;
            this.Proceed.Visible = false;
            this.creditsLabel.Visible = true;
            this.creditsGroup.Visible = true;
            this.LetsGo.Visible = true;
        }

        private void LetsGo_Click(object sender, EventArgs e)
        {
            this.goDump();
            this.note.Visible = false;
            this.Proceed.Visible = false;
            this.creditsLabel.Visible = false;
            this.creditsGroup.Visible = false;
            this.LetsGo.Visible = false;
        }

        private void goDump()
        {
            MDIMain objMain = new MDIMain();
            DFU objDFU = new DFU(null);
            objDFU.Show();
            this.Dispose();
        }
        

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://twitter.com/AKi_nG");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://twitter.com/chronicdevteam");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://twitter.com/icj_");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://twitter.com/cpich3g");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://geohot.com");
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://twitter.com/GreySyntax");
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://twitter.com/msftguy");
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://twitter.com/MuscleNerd");
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://twitter.com/iNeal");
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://twitter.com/planetbeing");
        }

        private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://twitter.com/p0sixninja");
        }

        private void linkLabel15_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://twitter.com/sbingner");
        }

        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://twitter.com/notcom");
        }

        private void linkLabel13_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://twitter.com/iSurenix");
        }

        private void linkLabel14_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://twitter.com/ThePiratep");
        }

        private void labelLicence_Click(object sender, EventArgs e)
        {
            Interaction.MsgBox("See attached file LICENSE in iFaith folder.", MsgBoxStyle.Information, null);
        }
    }
}
