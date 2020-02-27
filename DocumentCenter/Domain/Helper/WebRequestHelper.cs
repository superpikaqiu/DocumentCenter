using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace DocumentCenter.Domain.Helper
{
    public class WebRequestHelper
    {
        public static string PostData(string url,object data, Dictionary<string, string> headers = null)
        {
            string dataStr = JsonConvert.SerializeObject(data);
            return PostData(url, dataStr,headers);
        }

        public static string PostData(string url, string data,Dictionary<string,string> headers)
        {
            string result = null;

            try
            {
                var externalReq = WebRequest.Create(url);
                externalReq.Method = "POST";
                externalReq.ContentType = "application/json";

                if(headers != null)
                {
                    foreach(var key in headers.Keys)
                    {
                        externalReq.Headers.Add(key, headers[key].ToString());
                    }
                }
                
                byte[] dataArray = System.Text.Encoding.UTF8.GetBytes(data);
                Stream reqStream = externalReq.GetRequestStream();
                reqStream.Write(dataArray, 0, dataArray.Length);
                reqStream.Close();

                var response = externalReq.GetResponse();
                Stream rspStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(rspStream);
                result = streamReader.ReadToEnd();
                streamReader.Close();
                rspStream.Close();
            }
            catch
            {

            }
            

            return result;
        }

        public static string GetData(string url, Dictionary<string, string> headers)
        {
            string result = null;

            try
            {
                var externalReq = WebRequest.CreateHttp(url);
                externalReq.Method = "GET";
                externalReq.Accept = headers["Accept"];

                var response = externalReq.GetResponse();
                Stream rspStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(rspStream);
                result = streamReader.ReadToEnd();
                streamReader.Close();
                rspStream.Close();
            }
            catch
            {

            }

            return result;
        }
    }
}