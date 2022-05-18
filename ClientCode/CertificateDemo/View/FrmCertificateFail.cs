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
    public partial class FrmCertificateFail : Form
    {
        private CertificateResultModel MCertificateResultModel;
        public FrmCertificateFail()
        {
            InitializeComponent();
        }

        public FrmCertificateFail(CertificateResultModel model) : this()
        {
            MCertificateResultModel = model;
        }
    }
}
