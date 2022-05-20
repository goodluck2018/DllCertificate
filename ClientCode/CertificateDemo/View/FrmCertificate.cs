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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                string url = "https://localhost:44367/api/Certificate";
                string cpuId = CommonHelper.GetCpuID();
                string cpuIdMD5 = CommonHelper.GetMD5WithString(cpuId);
                string serialNumber = txtSerialNumber.Text; 
                string jsonData = "{\"DeviceCode\": \"" + cpuId + "\",\"SerialNumber\": \"" + serialNumber + "\",\"CreateTime\": \"\",\"Operator\": \"\"}";
                HttpRequestHelper requestHelper = new HttpRequestHelper();
                string result = requestHelper.HttpPostJsonData(url, jsonData); // "StatusCode":200,"Result":"true"
                // 判断 result  生成本地证书文件  
                if (!string.IsNullOrEmpty(result) && result != "-1")
                {
                    try
                    {
                        ResultModel resultModel = JsonConvert.DeserializeObject<ResultModel>(result);
                        if (resultModel.StatusCode == 200 && resultModel.Result.Equals("true"))
                        {
                            string json = "{\"DeviceCode\": \"" + cpuIdMD5 + "\",\"SerialNumber\": \"\",\"Validate\": \"\",\"CreateTime\": \"\",\"Operator\": \"\"}";
                            CreateLocalCertificate(json);
                            MCertificateResultModel.ResultCode = "true";
                            new FrmCertificateSuccess().ShowDialog(); // 显示序列号认证成功信息
                            this.Close();
                        }
                        else
                        {
                            new FrmCertificateFail().ShowDialog(); // 显示序列号认证失败信息 
                        }
                    }
                    catch
                    {
                        new FrmCertificateFail().ShowDialog(); // 显示序列号认证失败信息
                    }
                }
                else
                {
                    new FrmCertificateFail().ShowDialog(); // 显示序列号认证失败信息
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
