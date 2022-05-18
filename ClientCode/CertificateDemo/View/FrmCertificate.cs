using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CertificateDemo
{
    public partial class FrmCertificate : Form
    {
        private CertificateResultModel MCertificateResultModel;
        public FrmCertificate()
        {
            InitializeComponent();
        }

        public FrmCertificate(CertificateResultModel model) : this()
        {
            MCertificateResultModel = model;
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSerialNumber.Text.Trim()))
            {
                return;
            }
            if (MCertificateResultModel != null)
            {
                // TODO HttpRequest  处理
                string url = "";
                string cpuId = CommonHelper.GetCpuID();
                string cpuIdMD5 = CommonHelper.GetMD5WithString(cpuId);
                string jsonData = "{\"DeviceCode\": \""+ cpuId + "\",\"SerialNumber\": \"\",\"CreateTime\": \"\",\"Operator\": \"\"}";
                HttpRequestHelper requestHelper = new HttpRequestHelper();
                string result = requestHelper.HttpPostJsonData(url, jsonData);
                // 判断 result
                //if(result)
                // 生成本地证书文件 
                //  string json = "{\"DeviceCode\": \"\",\"SerialNumber\": \"\",\"Validate\": \"\",\"CreateTime\": \"\",\"Operator\": \"\"}";
                if (txtSerialNumber.Text.Equals("123")) // 临时测试使用
                {
                    string json = "{\"DeviceCode\": \""+ cpuIdMD5 + "\",\"SerialNumber\": \"\",\"Validate\": \"\",\"CreateTime\": \"\",\"Operator\": \"\"}";
                    CreateLocalCertificate(json);
                    MCertificateResultModel.ResultCode = "true";
                    new FrmCertificateSuccess().ShowDialog(); // 显示序列号认证成功信息
                    this.Close();
                }
                else
                {
                    new FrmCertificateFail().ShowDialog(); // 显示序列号认证成功信息
                }

            }
        }

        /// <summary>
        ///  生成本地证书
        /// </summary>
        /// <param name="json"></param>
        private void CreateLocalCertificate(string json)
        { 
            string certificateFilePath = Path.Combine(Application.StartupPath, "CertificateFile", Global.CertificateName);
            File.WriteAllText(certificateFilePath, json, Encoding.GetEncoding("utf-8"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string url = "http://www.x-imaging.com/";
            System.Diagnostics.Process.Start(url);
        }
    }
}
