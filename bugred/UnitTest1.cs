using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using bugred.Helper;

namespace Tbugred
{
    public class Tests
    {
        [Test]
        public void RegistrationUser()
        {
            RestClient client = Helper.StartRequestClient("http://users.bugred.ru/tasks/rest/doregister");
            RestRequest request = Helper.Request();
            request.AddHeader("content-type", "application/json");
            Dictionary<string, string> body = Helper.GetUserData();
            request.AddJsonBody(body);
            IRestResponse response = client.Execute(request);
            JObject json = JObject.Parse(response.Content);

            Assert.AreEqual("OK", response.StatusCode.ToString());
            Assert.AreEqual(body["email"], json["email"]?.ToString());
        }

        [Test]
        public void InValidRegistrationUser()
        {
            RestClient client = Helper.StartRequestClient("http://users.bugred.ru/tasks/rest/doregister");
            RestRequest request = Helper.Request();
            request.AddHeader("content-type", "application/json");
            Dictionary<string, string> body = new Dictionary<string, string>()
            {
                {"email", ""},
                {"name", "AD11"},
                {"password", "1"}
            };
            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);
            JObject json = JObject.Parse(response.Content);

            Assert.AreEqual("OK", response.StatusCode.ToString());
            Assert.AreEqual("error", json["type"]?.ToString());
        }

        [Test]
        public void AddCompany()
        {
            RestClient client = Helper.StartRequestClient("http://users.bugred.ru/tasks/rest/createcompany");
            RestRequest request = Helper.Request();
            Helper.RegistrationUser("thirdtestsmail@i.ua");
            request.AddHeader("content-type", "application/json");
            CompanyRequest companyRequest = new CompanyRequest()
            {
                company_name = "MTWTF",
                company_type = "ООО",
                company_users = new List<string>()
                {
                    "thirdtestsmail@i.ua",
                    "thirdtestsmail@i.ua"
                },
                email_owner = "thirdtestsmail@i.ua"
            };

            request.AddJsonBody(companyRequest);

            IRestResponse response = client.Execute(request);
            JObject json = JObject.Parse(response.Content);

            Assert.AreEqual("OK", response.StatusCode.ToString());
            Assert.AreEqual("success", json["type"]?.ToString());
        }


        [Test]
        public void InValidAddCompany()
        {
            RestClient client = Helper.StartRequestClient("http://users.bugred.ru/tasks/rest/createcompany");
            RestRequest request = Helper.Request();
            request.AddHeader("content-type", "application/json");
            CompanyRequest companyRequest = new CompanyRequest()
            {
                company_name = "MTWTF",
                company_type = "ООО",
                company_users = new List<string>()
                {
                    "thirdTestMail@i.ua",
                    "secondTestMail@i.ua"
                },
                email_owner = ""
            };

            request.AddJsonBody(companyRequest);

            IRestResponse response = client.Execute(request);
            JObject json = JObject.Parse(response.Content);

            Assert.AreEqual("OK", response.StatusCode.ToString());
            Assert.AreEqual("error", json["type"]?.ToString());
        }

        [Test]
        public void DeleteAvatar()
        {
            RestClient client = Helper.StartRequestClient("http://users.bugred.ru/tasks/rest/deleteavatar");
            RestRequest request = Helper.Request();
            request.AddHeader("content-type", "application/json");
            Dictionary<string, string> body = new Dictionary<string, string> {
            {"email","thirdtdxestmail@i.ua"}
                };
            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);
            JObject json = JObject.Parse(response.Content);

            Assert.AreEqual("OK", response.StatusCode.ToString());
        }

        [Test]
        public void InValidDeleteAvatar()
        {
            RestClient client = Helper.StartRequestClient("http://users.bugred.ru/tasks/rest/deleteavatar");
            RestRequest request = Helper.Request();
            request.AddHeader("content-type", "application/json");
            Dictionary<string, string> body = new Dictionary<string, string> {
            {"email",""}
                };
            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);
            JObject json = JObject.Parse(response.Content);

            Assert.AreEqual("OK", response.StatusCode.ToString());
            Assert.AreEqual("error", json["type"]?.ToString());
        }

        [Test]
        public void MagicSearch()
        {
            RestClient client = Helper.StartRequestClient("http://users.bugred.ru/tasks/rest/magicsearch");
            RestRequest request = Helper.Request();
            request.AddHeader("content-type", "application/json");
            Dictionary<string, string> body = new Dictionary<string, string> {
            {"query","MTWTF"}
                };
            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);
            JObject json = JObject.Parse(response.Content);

            Assert.AreEqual("234", response.StatusCode.ToString());
            Assert.AreEqual("MTWTF", json["results"][0]["name"]?.ToString());
        }

        [Test]
        public void EmptyFieldMagicSearch()
        {
            RestClient client = Helper.StartRequestClient("http://users.bugred.ru/tasks/rest/magicsearch");
            RestRequest request = Helper.Request();
            request.AddHeader("content-type", "application/json");
            Dictionary<string, string> body = new Dictionary<string, string> {
            {"query",""}
                };
            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);
            JObject json = JObject.Parse(response.Content);

            Assert.AreEqual("456", response.StatusCode.ToString());
            Assert.AreEqual("Длина запроса не должна превышать 1000 символов", json["message"]?.ToString());
        }

        [Test]
        public void AddAAvatar()
        {
            RestClient startClient = Helper.StartRequestClient("http://users.bugred.ru/tasks/rest/doregister");
            RestRequest request = Helper.Request();
            request.AddHeader("content-type", "application/json");
            //IRestResponse response = startClient.Execute(request);
            Helper.RegistrationUser("thirdtdxestsmail@i.ua");
            startClient = Helper.StartRequestClient("http://users.bugred.ru/tasks/rest/addavatar/?email=thirdtdxestsmail@i.ua");
            request.AddHeader("content-type", "multipart/form-data");
            request.AlwaysMultipartFormData = true;
            request.AddFile("avatar", (@"D:\курсы\дз\hw 15\unnamed.jpg"));
            IRestResponse response = startClient.Execute(request);
            JObject json = JObject.Parse(response.Content);

            Assert.AreEqual("OK", response.StatusCode.ToString());
            Assert.AreEqual("ok", json["status"]?.ToString());
        }

        [Test]
        public void InValidAddAAvatar()
        {
            RestClient client = Helper.StartRequestClient("http://users.bugred.ru/tasks/rest/addavatar/?email");
            RestRequest request = Helper.Request();
            request.AddHeader("content-type", "multipart/form-data");
            request.AlwaysMultipartFormData = true;
            request.AddFile("avatar", (@"D:\курсы\дз\hw 15\unnamed.jpg"));
            IRestResponse response = client.Execute(request);
            JObject json = JObject.Parse(response.Content);

            Assert.AreEqual("OK", response.StatusCode.ToString());
            Assert.AreEqual("error", json["type"]?.ToString());
        }

        public class CompanyRequest
        {
            public string company_name { get; set; }
            public string company_type { get; set; }
            public List<string> company_users { get; set; }
            public string email_owner { get; set; }
        }
    }
}