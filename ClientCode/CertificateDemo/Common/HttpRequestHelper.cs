using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CertificateDemo
{
    public class HttpRequestHelper
    {

        private const int HTTPTIMEOUT = 3000;
        public string HttpRequestMethod = "POST";

        public HttpRequestHelper()
        {

        }
        public HttpRequestHelper(string requestMethod)
        {
            HttpRequestMethod = requestMethod;
        }

        #region httpRequest 封装
        /// <summary>
        /// http 纯数据发送：方式1:application/json 方式2：application/x-www-form-urlencoded;
        /// 方式3: application/form-data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeOut"></param>
        /// <param name="requestMethod"></param>
        /// <param name="contentType"></param>
        /// <param name="jsonBody">以jsonbody形式发送时，parameters为空</param>
        /// <param name="parameters">以parameters形式发送时，jsonBody为空</param>
        /// <returns></returns>
        public string HttpPostJsonData(string url, int timeOut, string requestMethod,
                                    string contentType, string jsonBody, Dictionary<object, object> parameters)
        {
            string resultStr = string.Empty;
            // 方式1:application/json   
            // 方式2：application/x-www-form-urlencoded; 
            // 方式3: application/form-data
            if (contentType.Trim().Contains("application/json") || parameters == null)
            {
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.ContentType = contentType;
                    httpWebRequest.Method = requestMethod;
                    httpWebRequest.Timeout = timeOut;
                    if (!string.IsNullOrEmpty(jsonBody))
                    {
                        byte[] btBodys = Encoding.UTF8.GetBytes(jsonBody);
                        httpWebRequest.ContentLength = btBodys.Length;
                        httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);
                    }
                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                    resultStr = streamReader.ReadToEnd();
                    httpWebResponse.Close();
                    streamReader.Close();
                    httpWebRequest.Abort();
                    httpWebResponse.Close();
                }
                catch (Exception ex)
                {
                    resultStr = "-1";
                }
            }
            else
            {
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.ContentType = contentType;
                    httpWebRequest.Method = requestMethod;
                    httpWebRequest.Timeout = timeOut;
                    //如果需要POST数据  
                    if (!(parameters == null || parameters.Count == 0))
                    {
                        StringBuilder buffer = new StringBuilder();
                        int i = 0;
                        foreach (string key in parameters.Keys)
                        {
                            if (i > 0)
                            {
                                buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                            }
                            else
                            {
                                buffer.AppendFormat("{0}={1}", key, parameters[key]);
                            }
                            i++;
                        }
                        byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
                        using (Stream stream = httpWebRequest.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }

                        using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                        {
                            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                            {
                                resultStr = reader.ReadToEnd();
                            }
                        }
                    }
                }
                catch { }
            }

            return resultStr;
        }


        public string HttpPostJsonData(string url, int timeOut, string requestMethod,
                                string contentType, string jsonBody, Dictionary<object, object> parameters, ref string token)
        {
            string resultStr = string.Empty;
            if (contentType.Trim().Contains("application/json") || parameters == null)
            {
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    if (!string.IsNullOrEmpty(token))
                    { httpWebRequest.Headers.Add(HttpRequestHeader.Cookie, token); }
                    httpWebRequest.ContentType = contentType;
                    httpWebRequest.Method = requestMethod;
                    httpWebRequest.Timeout = timeOut;
                    if (!string.IsNullOrEmpty(jsonBody))
                    {
                        byte[] btBodys = Encoding.UTF8.GetBytes(jsonBody);
                        httpWebRequest.ContentLength = btBodys.Length;
                        httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);
                    }
                    using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                    {
                        if (string.IsNullOrEmpty(token))
                        {
                            string strCookie = response.Headers["Set-Cookie"];
                            string[] ary = strCookie.Split(';');
                            for (int j = 0; j < ary.Length; j++)
                            {
                                if (ary[j].Contains("JSESSIONID"))
                                {
                                    token = ary[j];
                                    break;
                                }
                            }
                        }
                        using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            resultStr = reader.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    resultStr = "-1";
                }
            }
            else
            {
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    if (!string.IsNullOrEmpty(token))
                    { httpWebRequest.Headers.Add(HttpRequestHeader.Cookie, token); }
                    httpWebRequest.ContentType = contentType;
                    httpWebRequest.Method = requestMethod;
                    httpWebRequest.Timeout = timeOut;
                    //如果需要POST数据  
                    if (!(parameters == null || parameters.Count == 0))
                    {
                        StringBuilder buffer = new StringBuilder();
                        int i = 0;
                        foreach (string key in parameters.Keys)
                        {
                            if (i > 0)
                            {
                                buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                            }
                            else
                            {
                                buffer.AppendFormat("{0}={1}", key, parameters[key]);
                            }
                            i++;
                        }
                        byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
                        using (Stream stream = httpWebRequest.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }

                        using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                        {
                            string strCookie = response.Headers["Set-Cookie"];
                            string[] ary = strCookie.Split(';');
                            for (int j = 0; j < ary.Length; j++)
                            {
                                if (ary[j].Contains("JSESSIONID"))
                                {
                                    token = ary[j];
                                    break;
                                }
                            }
                            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                            {
                                resultStr = reader.ReadToEnd();
                            }
                        }
                    }
                }
                catch { }
            }

            return resultStr;
        }

        /// <summary>
        /// http 纯数据发送：方式1:application/json 方式2：application/x-www-form-urlencoded;
        /// 方式3: application/form-data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeOut"></param>
        /// <param name="requestMethod"></param>
        /// <param name="contentType"></param>
        /// <param name="jsonBody">以jsonbody形式发送时，parameters为空</param>
        /// <param name="parameters">以parameters形式发送时，jsonBody为空</param>
        /// <returns></returns>
        public string HttpPostJsonData(string url, string contentType, string jsonBody, Dictionary<object, object> parameters)
        {
            string resultStr = string.Empty;
            // 方式1:application/json   
            // 方式2：application/x-www-form-urlencoded; 
            // 方式3: application/form-data
            if (contentType.Trim().Contains("application/json") || parameters == null)
            {
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.ContentType = contentType;
                    httpWebRequest.Method = HttpRequestMethod;
                    httpWebRequest.Timeout = HTTPTIMEOUT;
                    if (!string.IsNullOrEmpty(jsonBody))
                    {
                        byte[] btBodys = Encoding.UTF8.GetBytes(jsonBody);
                        httpWebRequest.ContentLength = btBodys.Length;
                        httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);
                    }
                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                    resultStr = streamReader.ReadToEnd();
                    httpWebResponse.Close();
                    streamReader.Close();
                    httpWebRequest.Abort();
                    httpWebResponse.Close();
                }
                catch
                {
                    resultStr = "-1";
                }
            }
            else if (contentType.Trim().ToLower().Contains("x-www-form-urlencoded"))
            {
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.ContentType = contentType;
                    httpWebRequest.Method = HttpRequestMethod;
                    httpWebRequest.Timeout = HTTPTIMEOUT;
                    //如果需要POST数据  
                    if (!(parameters == null || parameters.Count == 0))
                    {
                        StringBuilder buffer = new StringBuilder();
                        int i = 0;
                        foreach (string key in parameters.Keys)
                        {
                            if (i > 0)
                            {
                                buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                            }
                            else
                            {
                                buffer.AppendFormat("{0}={1}", key, parameters[key]);
                            }
                            i++;
                        }
                        byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
                        using (Stream stream = httpWebRequest.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }

                        using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                        {
                            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                            {
                                resultStr = reader.ReadToEnd();
                            }
                        }
                    }
                }
                catch { }
            }
            else if (contentType.Trim().ToLower().Contains("form-data"))
            {
                // 定义请求体中的内容 并转成二进制
                string boundary = "----RuanJie-HuaXia-Message";
                string Enter = "\r\n";
                string endBoundary = "----RuanJie-HuaXia-Message----";
                byte[] endBoundaryByte = Encoding.UTF8.GetBytes(endBoundary);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = contentType.Replace(";", "") + "; boundary = " + boundary + Enter + Enter;
                request.Method = HttpRequestMethod;
                request.Timeout = HTTPTIMEOUT;
                using (Stream myRequestStream = request.GetRequestStream())//定义请求流
                {
                    if (!(parameters == null || parameters.Count == 0))
                    {
                        foreach (string key in parameters.Keys)
                        {
                            string temp = boundary + Enter
                            + "Content-Disposition: form-data; name=\"" + key + "\"" + Enter + Enter
                            + parameters[key] + Enter;
                            byte[] tempByte = Encoding.UTF8.GetBytes(temp);
                            myRequestStream.Write(tempByte, 0, tempByte.Length);
                        }
                        myRequestStream.Write(endBoundaryByte, 0, endBoundaryByte.Length);
                    }
                }
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();//发送
                    Stream myResponseStream = response.GetResponseStream();//获取返回值
                    using (StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8")))
                    {
                        string retString = Regex.Unescape(myStreamReader.ReadToEnd());
                        myStreamReader.Close();
                        myResponseStream.Close();
                        resultStr = retString;
                    }
                }
                catch (Exception e)
                {
                    resultStr = "";
                }
            }
            return resultStr;
        }

        /// <summary>
        /// http 纯数据发送：方式1:application/json 方式2：application/x-www-form-urlencoded;
        /// 方式3: application/form-data
        /// </summary>
        /// <param name="url"></param>   
        /// <param name="jsonData">以json形式发送时</param> 
        /// <returns></returns>
        public string HttpPostJsonData(string url, string jsonData)
        {
            string resultStr = string.Empty;
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(jsonData)) { return string.Empty; }
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = HTTPTIMEOUT;
                if (!string.IsNullOrEmpty(jsonData))
                {
                    byte[] btBodys = Encoding.UTF8.GetBytes(jsonData);
                    httpWebRequest.ContentLength = btBodys.Length;
                    httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);
                }
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                resultStr = streamReader.ReadToEnd();
                httpWebResponse.Close();
                streamReader.Close();
                httpWebRequest.Abort();
                httpWebResponse.Close();
            }
            catch
            {
                resultStr = "-1";
            }
            return resultStr;
        }


        /// <summary>
        /// C#调用接口上传json数据，并且带文件上传
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public string HttpPostFile(string url, string fileKey, string filePath, Dictionary<object, object> parameters)
        {
            // 将http网络路径转换至本地路径
            if (filePath.ToLower().Contains("http"))
            {
                string fielName = Path.GetFileName(filePath);
                filePath = GetHttpFile(filePath, fielName);
            }
            // 1、读取文件内容到byte[] fileContentByte // 将文件转成二进制
            string fileName = Path.GetFileName(filePath);
            byte[] fileContentByte = new byte[1024]; // 文件内容二进制 
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            fileContentByte = new byte[fs.Length]; // 二进制文件
            fs.Read(fileContentByte, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            // 2、定义请求体中的内容 并转成二进制
            // 边界符
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            var Enter = "\r\n";
            // 边界符
            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            // 最后的结束符
            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                if (null == request) { return ""; }
                // 设置属性
                request.Method = "POST";
                request.Timeout = 3000;
                request.ContentType = "multipart/form-data; boundary=" + boundary;

                using (Stream myRequestStream = request.GetRequestStream())//定义请求流
                {
                    //将各个二进制 安顺序写入请求流 modelIdStr -> (fileContentStr + fileContent) -> uodateTimeStr -> encryptStr 
                    // (1) 先写parameters
                    if (!(parameters == null || parameters.Count == 0))
                    {
                        foreach (string key in parameters.Keys)
                        {
                            string temp = "\r\n--" + boundary + Enter
                            + "Content-Disposition: form-data; name=\"" + key + "\"" + Enter + Enter
                            + parameters[key] + Enter;
                            byte[] tempByte = Encoding.UTF8.GetBytes(temp);
                            myRequestStream.Write(tempByte, 0, tempByte.Length);
                        }
                    }

                    // (2) 写文件
                    string fileContentHeader = "Content-Disposition: form-data; name=\"" + fileKey + "\"; filename=\"" + filePath + "\"\r\n" +
             "Content-Type: application/octet-stream\r\n\r\n";
                    byte[] fileContentHeaderByte = Encoding.UTF8.GetBytes(fileContentHeader);//fileContent一些名称等信息的二进制（不包含文件本身）
                    myRequestStream.Write(beginBoundary, 0, beginBoundary.Length);
                    myRequestStream.Write(fileContentHeaderByte, 0, fileContentHeaderByte.Length);
                    myRequestStream.Write(fileContentByte, 0, fileContentByte.Length);

                    // 写入最后的结束边界符
                    myRequestStream.Write(endBoundary, 0, endBoundary.Length);
                }
            }
            catch { }
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();//发送
                Stream myResponseStream = response.GetResponseStream();//获取返回值
                using (StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8")))
                {
                    string retString = Regex.Unescape(myStreamReader.ReadToEnd());
                    myStreamReader.Close();
                    myResponseStream.Close();
                    return retString;
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public string HttpGet(string url)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
                wbRequest.Method = "GET";
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sReader = new StreamReader(responseStream))
                    {
                        result = sReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public string HttpGet(string url, string token)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
                if (!string.IsNullOrEmpty(token))
                { wbRequest.Headers.Add(HttpRequestHeader.Cookie, token); }
                wbRequest.Method = "GET";
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sReader = new StreamReader(responseStream))
                    {
                        result = sReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }


        public string GetHttpFile(string url, string fileName)
        {
            string imgPath = "";
            Bitmap img = null;
            HttpWebRequest req;
            HttpWebResponse res = null;
            try
            {
                System.Uri httpUrl = new System.Uri(url);
                req = (HttpWebRequest)(WebRequest.Create(httpUrl));
                req.Timeout = 3000; //设置超时值10秒
                req.UserAgent = "XXXXX";
                req.Accept = "XXXXXX";
                req.Method = "GET";
                res = (HttpWebResponse)(req.GetResponse());
                img = new Bitmap(res.GetResponseStream());//获取图片流     
                imgPath = Path.Combine(Application.StartupPath, @"temp\" + fileName);
                if (!Directory.Exists(Path.Combine(Application.StartupPath, @"temp\")))
                { Directory.CreateDirectory(Path.Combine(Application.StartupPath, @"temp\")); }
                img.Save(imgPath);//随机名
            }

            catch (Exception ex)
            {
                string aa = ex.Message;
            }
            finally
            {
                res.Close();
            }
            return imgPath;
        }
        #endregion
    }
}
