using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateDemo
{
    public class CertificateResultModel
    {
        private string resultCode = string.Empty;
        private string resultInfo = string.Empty;
        private string certificateState = string.Empty;

        public string ResultCode { get => resultCode; set => resultCode = value; }
        public string ResultInfo { get => resultInfo; set => resultInfo = value; }
        public string CertificateState { get => certificateState; set => certificateState = value; }
    }
}
