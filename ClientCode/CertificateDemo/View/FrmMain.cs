using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CertificateDemo
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }
        private void FrmMain_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            InitFrm();
        }


        /// <summary>
        /// 初始化DLL  动态加载本地dll文件
        /// </summary>
        private void InitDll()
        {
            string path = Path.Combine(Application.StartupPath, "TestDll.dll");
            Assembly assem = Assembly.LoadFile(path);
            Type[] tys = assem.GetTypes();//得到所有的类型名，然后遍历，通过类型名字来区别了
            foreach (Type ty in tys)
            {
                if (ty.Name.Equals("Test"))
                {
                    ConstructorInfo magicConstructor = ty.GetConstructor(Type.EmptyTypes);//获取不带参数的构造函数
                    object magicClassObject = magicConstructor.Invoke(new object[] { });//这里是获取一个类似于类的实例的东东 
                    MethodInfo mi = ty.GetMethod("Output");
                    object aa = mi.Invoke(magicClassObject, null);
                    MessageBox.Show(aa.ToString());//这儿是执行类class1的sayhello方法
                }
            }
        }

        /// <summary>
        /// 窗体初始化
        /// </summary>
        private void InitFrm()
        {
            // 判断证书文件是否存在
            string certificateFilePath = Path.Combine(Application.StartupPath, "CertificateFile", Global.CertificateName);
            if (File.Exists(certificateFilePath))
            {
                // 判断证书内容是否正确
                bool result = CertificateDataConfirm(certificateFilePath);
                if (result)
                {
                    InitDll(); // 加载DLL文件
                    ShowChildForm(new FrmSuccessInfo()); // 显示加载成功信息
                }
                else
                {
                    CertificateForm();// 加载序列号认证窗体
                }
            }
            else
            {
                CertificateForm();// 加载序列号认证窗体
            }
        }

        /// <summary>
        /// 显示子窗体
        /// </summary>
        /// <param name="curFrm"></param>
        private void ShowChildForm(Form curFrm)
        {
            if (curFrm != null)
            {
                foreach (Control control in this.panelMain.Controls)
                {
                    if (control is Form)
                    {
                        ((Form)control).Close(); //闭该窗体，释放资源
                    }
                }
                panelMain.Controls.Clear();
                curFrm.FormBorderStyle = FormBorderStyle.None; // 无边框
                curFrm.Dock = DockStyle.Fill;
                curFrm.TopLevel = false; // 不是最顶层窗体
                panelMain.Controls.Add(curFrm);  // 添加到 Panel中
                curFrm.Show();
            }
        }

        /// <summary>
        /// 判断证书内容是否正确
        /// </summary>
        /// <returns></returns>
        private bool CertificateDataConfirm(string file)
        {
            bool result = true;
            string text = File.ReadAllText(file, Encoding.GetEncoding("utf-8"));
            if (string.IsNullOrEmpty(text)) return false; 
            try
            {
                JObject jobject = (JObject)JsonConvert.DeserializeObject(text);
                string cpuId = CommonHelper.GetCpuID();   
                string cpuIdMD5 = CommonHelper.GetMD5WithString(cpuId);
                string deviceCodeMD5 = jobject["DeviceCode"].ToString();  
                if (deviceCodeMD5.Equals(cpuIdMD5))
                {
                    result = true;
                }
                else
                { result = false; }
            }
            catch { return false; }
            return result;
        }

        /// <summary>
        /// 认证
        /// </summary>
        private void CertificateForm()
        {
            CertificateResultModel model = new CertificateResultModel();
            new FrmCertificate(model).ShowDialog();// 加载序列号认证窗体
            if (model.ResultCode.Equals("true"))
            {
                InitDll(); // 加载DLL文件
                ShowChildForm(new FrmSuccessInfo()); // 显示加载成功信息
            }
            else
            {
                ShowChildForm(new FrmSuccessInfo(false)); // 显示未激活序列号
            }
        }
    }
}
