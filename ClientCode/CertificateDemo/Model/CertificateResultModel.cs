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

    public class ResultModel
    { 
       //  "{ \"StatusCode\":200,\"Result\":\"true\", \"Message\" :\"\"}";
        private int statusCode = 0;
        private string result = string.Empty;
        private string message = string.Empty;

        public int StatusCode { get => statusCode; set => statusCode = value; }
        public string Result { get => result; set => result = value; }
        public string Message { get => message; set => message = value; }
    }
}
