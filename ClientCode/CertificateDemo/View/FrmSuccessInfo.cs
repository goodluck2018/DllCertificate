using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CertificateDemo
{
    public partial class FrmSuccessInfo : Form
    {
        public FrmSuccessInfo()
        {
            InitializeComponent();
        }

        public FrmSuccessInfo(bool result):this()
        {
            if (!result)
            {
                label1.Text = "未激活序列号";
                linkLabel1.Visible = false;
            }
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new FrmCertificateInfo().ShowDialog();
        }
    }
}
