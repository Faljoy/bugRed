using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bugred.Helper
{
    public static class Helper
    {
        public static RestClient StartRequestClient(string URLAPI)
        {
            RestClient client = new RestClient(URLAPI)
            {
                Timeout = 300000
            };
            return client;
        }

        public static RestRequest Request()
        {
            return new RestRequest(Method.POST);
        }

        public static string Number()
        {
            DateTime dataTime = new DateTime();
            dataTime = DateTime.Now;
            string dataRandom = dataTime.ToString("ddMMyyyyhhmmss");
            return dataRandom;
        } 

        public static Dictionary<string, string> GetUserData()
        {
            string dataRandom = Number();
            string email = "test" + dataRandom + "@test.com";
            string userId = dataRandom;
            string userName = "Name" + dataRandom;
            return new Dictionary<string, string>()
             {
                 {"email",email},
                 {"name",userName},
                 {"password","1"}
             };
        }

        public static void RegistrationUser(string email)
        {
            RestClient client = StartRequestClient("http://users.bugred.ru/tasks/rest/doregister");
            RestRequest request = Request();
            request.AddHeader("content-type", "application/json");
            Dictionary<string, string> body = new Dictionary<string, string>()
                  {
                 {"email",email},
                 {"name", Number()},
                 {"password","1"}
             };
       
            request.AddJsonBody(body);
            IRestResponse response = client.Execute(request);
            JObject json = JObject.Parse(response.Content);
        }       
    }
}
