
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using RestSharp;

namespace global
{
    public class auth
    {
        internal static string RandomNumero(int v)
        {
            throw new NotImplementedException();
        }

        internal string JsonChat()
        {
            var obj = new
            {
                agent = new
                {
                    this.agent
                }
            };

            return JsonConvert.SerializeObject(obj);
        }

        public string agent { get; set; }

    }
}