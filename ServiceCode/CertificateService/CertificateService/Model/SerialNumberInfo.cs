using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificateService
{
    public class SerialNumberInfo
    {
        private int id = 0;
        private string serialNumber = string.Empty;
        private string validate = string.Empty;
        private string createTime = string.Empty;

        public int Id { get => id; set => id = value; }
        public string SerialNumber { get => serialNumber; set => serialNumber = value; }
        public string Validate { get => validate; set => validate = value; }
        public string CreateTime { get => createTime; set => createTime = value; }
    }
}
