using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public static class Utility
    {

        /// <summary>
        /// 获取当前用户请求头
        /// </summary>
        private static String GetTag(HttpWebRequest request)
        {
            var json = new Newtonsoft.Json.Linq.JObject();

            //遍历用户请求头
            foreach (var key in request.Headers.AllKeys)
            {
                //排除Cookie字段
                if (key.Equals("Cookie", StringComparison.OrdinalIgnoreCase)) continue;

                json.Add(key, request.Headers[key]);
            }

            //客户端IP
            json.Add("IP", GetUserIP());

            return json.ToString(Newtonsoft.Json.Formatting.None);
        }

        /// <summary>
        /// 获取当前用户IP
        /// </summary>
        private static String GetUserIP()
        {
            //return System.Web.HttpContext.Current.Request.UserHostAddress;

            //请修改以返回客户端IP
            return "";
        }

        /// <summary>
        /// 向服务器发送请求
        /// </summary>
        /// <param name="token">登录凭证</param>
        /// <param name="method">方法</param>
        /// <param name="url">网址</param>
        /// <param name="param">参数</param>
        /// <returns>返回数据</returns>
        private static String ProcessRequest(String token, String method, String url, String param)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = method;
            request.UserAgent = "Mozilla/5.0";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            if (!string.IsNullOrWhiteSpace(token)) request.Headers.Add("Authorization", "Token " + token);

            if (method == "POST" || method == "PUT")
            {
                if (string.IsNullOrWhiteSpace(param))
                {
                    request.ContentLength = 0;

                    request.Headers.Add("Tag", GetTag(request));
                }
                else
                {
                    var data = Encoding.UTF8.GetBytes(param);
                    request.ContentLength = data.Length;

                    request.Headers.Add("Tag", GetTag(request));

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
            }
            else
            {
                request.Headers.Add("Tag", GetTag(request));
            }

            HttpWebResponse response;
            bool error = false;
            string msg = "";
            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException we)
            {
                response = we.Response as HttpWebResponse;
                if (response == null)
                {
                    msg = we.Message;
                }
                error = true;
            }
            var html = "";
            if (response == null)
            {
                html = msg;
            }
            else
            {
                var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                html = reader.ReadToEnd();

                reader.Close();
                response.Close();
            }


            if (error)
            {
                throw new Exception(html);
            }
            else
            {
                return html;
            }
        }

        /// <summary>
        /// 向远程服务器发起GET请求并返回数据
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="token">登陆凭证</param>
        /// <param name="url">请求地址</param>
        /// <returns>返回数据</returns>
        public static T GetWebRequest<T>(string token, string url)
        {
            var html = ProcessRequest(token, "GET", url, "");

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(html);
        }

        /// <summary>
        /// 向远程服务器发起DELETE请求并返回数据
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="token">登陆凭证</param>
        /// <param name="url">请求地址</param>
        /// <returns>返回数据</returns>
        public static T DeleteWebRequest<T>(String token, String url)
        {
            var html = ProcessRequest(token, "DELETE", url, "");

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(html);
        }


        /// <summary>
        /// 向远程服务器发起POST请求并返回数据
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="token">登陆凭证</param>
        /// <param name="url">请求地址</param>
        /// <returns>返回数据</returns>
        public static T PostWebRequest<T>(string token, string url)
        {
            var html = ProcessRequest(token, "POST", url, "");

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(html);
        }

        /// <summary>
        /// 向远程服务器发起POST请求并返回数据
        /// </summary>
        /// <typeparam name="O">发送数据类型</typeparam>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="token">登陆凭证</param>
        /// <param name="url">请求地址</param>
        /// <param name="o">发送数据</param>
        /// <returns>返回数据</returns>
        public static T PostWebRequest<O, T>(string token, string url, O o)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(o);

            var html = ProcessRequest(token, "POST", url, json);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(html);
        }



    }
}
