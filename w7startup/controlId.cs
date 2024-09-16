using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;

namespace w7startup
{
    internal class controlId
    {
        public static string IniciaSession()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //byte[] data = Encoding.ASCII.GetBytes();
            //request.Headers["authorization"] = "basic: " + token;
            request.Method = "GET";
            //request.Accept = "application/json"; //?
            request.ContentType = "../json; charset=utf-8"; //?
                                                            //request.ContentLength = data.Length;

            //using (var stream = request.GetRequestStream())
            //{
            //    stream.Write(data, 0, data.Length);
            //}

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }

        public static string CriaDispositivo(string name, string ip)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //byte[] data = Encoding.ASCII.GetBytes();
            //request.Headers["authorization"] = "basic: " + token;
            request.Method = "GET";
            //request.Accept = "application/json"; //?
            request.ContentType = "../json; charset=utf-8"; //?
                                                            //request.ContentLength = data.Length;

            //using (var stream = request.GetRequestStream())
            //{
            //    stream.Write(data, 0, data.Length);
            //}

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }
    }
}