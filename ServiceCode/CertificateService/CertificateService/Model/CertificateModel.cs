using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificateService
{
    public class CertificateModel
    {
        // {"DeviceCode": "BFEBFBFF000806C1","SerialNumber": "ZF71R-DMX85-08DQY-8YMNC-PPHV8","CreateTime": "","Operator": ""}

        private int id = 0;
        private string deviceCode = string.Empty;
        private string serialNumber = string.Empty;
        private string createTime = string.Empty;
        private string invalidate = string.Empty;
        private string oper = string.Empty;

        public int Id { get => id; set => id = value; }
        public string DeviceCode { get => deviceCode; set => deviceCode = value; }
        public string SerialNumber { get => serialNumber; set => serialNumber = value; }
        public string CreateTime { get => createTime; set => createTime = value; }
        public string Invalidate { get => invalidate; set => invalidate = value; }
        public string Operater { get => oper; set => oper = value; }
    }
}
