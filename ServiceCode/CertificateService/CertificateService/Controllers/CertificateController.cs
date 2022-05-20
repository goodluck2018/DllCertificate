using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace CertificateService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private string ConnectionString = "server=localhost;user id=root;password=root;port=3306;database=certificatedll";

        // GET api/Certificate
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "{ \"StatusCode\":200,\"Result\":\"true\", \"Message\" :\"\"}";
        }

        /// <summary>
        /// 序列号认证请求
        /// </summary> 
        [HttpPost]
        public ActionResult<string> Post()  //  [FromBody] string value
        {
            string resultStr = "{ \"StatusCode\":200,\"Result\":\"true\", \"Message\" :\"\"}";
            try
            {
                string deviceCode = string.Empty, serialNumber = string.Empty, oper = string.Empty; 
                //获取RequestBody流
                StreamReader sr = new StreamReader(Request.Body, Encoding.GetEncoding("utf-8"));
                //异步读取从当前位置到流结尾的所有字符，并将它们作为一个字符串返回。
                string strData = sr.ReadToEndAsync().Result;
                if (string.IsNullOrEmpty(strData))
                {
                    resultStr = "{ \"StatusCode\":500,\"Result\":\"false\", \"Message\" :\"请求参数错误\"}";
                    return resultStr;
                }
                else
                {
                    CertificateModel certificateModel = JsonConvert.DeserializeObject<CertificateModel>(strData);
                    if (certificateModel is null)
                    {
                        resultStr = "{ \"StatusCode\":500,\"Result\":\"false\", \"Message\" :\"请求参数错误\"}";
                        return resultStr;
                    }
                    else
                    {
                        deviceCode = certificateModel.DeviceCode;
                        serialNumber = certificateModel.SerialNumber;
                    }
                } 
                // 记录插入数据库
                bool result = CertificateRecordAdd(deviceCode, serialNumber, oper);
                if (!result) { return UnprocessableEntity(); }

                // 获取序列号比对  YF390-0HF8P-M81RQ-2DXQE-M2UT6
                List<SerialNumberInfo> serialNumberInfos = GetSerialNumbers();
                if (serialNumberInfos != null && serialNumberInfos.Count > 0)
                {
                    SerialNumberInfo model = serialNumberInfos.FirstOrDefault(a => a.SerialNumber == serialNumber);
                    //if (model != null && !string.IsNullOrEmpty(model.Validate))  // 判断有效期
                    if (model is null)
                    {
                        resultStr = "{ \"StatusCode\":205,\"Result\":\"false\", \"Message\" :\"无效的序列号\"}";
                    }
                    else
                    {
                        resultStr = "{ \"StatusCode\":200,\"Result\":\"true\", \"Message\" :\"有效的序列号\"}";
                    }
                }
                else
                {
                    resultStr = "{ \"StatusCode\":205,\"Result\":\"false\", \"Message\" :\"无效的序列号\"}";
                }
            }
            catch (Exception e)
            {
                resultStr = "{ \"StatusCode\":500,\"Result\":\"false\", \"Message\" :\"请求参数错误\"}";
            }
            return resultStr;
        }

        /// <summary>
        /// 获取序列号
        /// </summary>
        /// <returns></returns>
        private List<SerialNumberInfo> GetSerialNumbers()
        {
            List<SerialNumberInfo> list = new List<SerialNumberInfo>();
            try
            {
                var sql = " SELECT * FROM tserialnumber ORDER BY Id  DESC LIMIT 10000 ;";
                DataTable dt = MySqlDBHelper.Querry(ConnectionString, sql);
                SerialNumberInfo model;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    model = new SerialNumberInfo();
                    model.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                    model.SerialNumber = dt.Rows[i]["SerialNumber"].ToString();
                    model.Validate = dt.Rows[i]["Validate"].ToString();
                    model.CreateTime = dt.Rows[i]["CreateTime"].ToString();
                    list.Add(model);
                }
            }
            catch (Exception e)
            {
            }
            return list;
        }


        /// <summary>
        /// 认证记录插入数据库
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <param name="serialNumber"></param>
        /// <param name="oper"></param>
        /// <returns></returns> 
        private bool CertificateRecordAdd(string deviceCode, string serialNumber, string oper)
        {
            bool result = true;
            try
            {
                var conn = new MySqlConnection(ConnectionString);
                var sql = " INSERT INTO TCertificateRecord(DeviceCode,SerialNumber,Validate,CreateTime,Operator) VALUES (@DeviceCode,@SerialNumber,@Validate,@CreateTime,@Operator)";
                var command = new MySqlCommand(sql, conn);
                string validate = DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss");
                string createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                command.Parameters.Add("@DeviceCode", MySqlDbType.String);
                command.Parameters["@DeviceCode"].Value = deviceCode;
                command.Parameters.Add("@SerialNumber", MySqlDbType.String);
                command.Parameters["@SerialNumber"].Value = serialNumber;
                command.Parameters.Add("@Validate", MySqlDbType.String);
                command.Parameters["@Validate"].Value = validate;
                command.Parameters.Add("@CreateTime", MySqlDbType.String);
                command.Parameters["@CreateTime"].Value = createTime;
                command.Parameters.Add("@Operator", MySqlDbType.String);
                command.Parameters["@Operator"].Value = oper;
                conn.Open();
                var row = command.ExecuteNonQuery();
                if (row == 1)
                { result = true; }
                else { result = false; }

            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
    }
}