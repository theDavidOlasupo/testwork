using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

using System.Web.Script.Serialization;
using System.Configuration;
using SterlingForexService.DAL;
using SterlingForesxService.com.sbp.utility;
using System.Net;
using SterlingForexService.com.sbp.utility;

namespace SterlingForexService
{
    class RestServiceInvocator
    {


        public string validateBVNforForex(BVNRecord transObj)
        {

            string nibbsurl = ConfigurationManager.AppSettings["nibbsurlforvalidatebvn"]; //pick this from config manager instead incase IP later changes, there will then be no need to recompile the sourcecode
            string username = ConfigurationManager.AppSettings["username"]; //pick from config manager
            string pswd = ConfigurationManager.AppSettings["password"]; //"KLKG+!xooYqlKi@!";  

            string nipcodeforsterling = ConfigurationManager.AppSettings["nipcodeforsterling"];

            string authorization = Gizmo.Base64Encode(username + ":" + pswd);

            string signatureDate = DateTime.Now.ToString("yyyyMMdd");


            string signatureString = username + signatureDate + pswd;

            string signature = Gizmo.GenerateSHA512String(signatureString);


            RestClient client = new RestClient(nibbsurl);

            ByPassProxy(client);



            var request = new RestRequest(Method.POST);

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            request.AddHeader("Accept", "text/plain");
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("InstitutionCode", nipcodeforsterling);
            request.AddHeader("ContentType", "text/plain");
            request.AddHeader("Authorization", authorization);
            request.AddHeader("SIGNATURE", signature);
            request.AddHeader("SIGNATURE_METH", "SHA512");
            request.AddHeader("TIMESTAMP", timestamp);







            String key = ConfigurationManager.AppSettings["AESKey"]; //"OUGW5XNSc/82rXAr"; 
            String iv = ConfigurationManager.AppSettings["IVKey"];// "1BNEMKUJXi3svqFk"; 





            var json = new JavaScriptSerializer().Serialize(transObj);






            String encryptedRequest = AES.Encrypt(json.ToString(), key, iv);


            var requestBody = request.AddParameter("text/plain", encryptedRequest, ParameterType.RequestBody);






            String decryptedText = AES.Decrypt(encryptedRequest, key, iv);



            var response = client.Execute(request);

            string bvnvalidationResponse = response.StatusCode + "|" + response.Content;

            string realResponseCode = "";
            string realResponseContent = "";

            //return bvnvalidationResponse;

            try
            {
                try
                {

                    realResponseCode = Convert.ToString(response.Headers.Where(x => x.Name == "error").SingleOrDefault().Value);
                    realResponseContent = Convert.ToString(response.Headers.Where(x => x.Name == "respData").SingleOrDefault().Value);

                }
                catch (Exception ex)
                {

                    realResponseCode = response.StatusCode.ToString();
                    realResponseContent = response.Content;
                }

            }
            catch (Exception ex)
            {
                realResponseCode = "-1";
                realResponseContent = "Error Receiving response from nibss service" + ex;
            }

            //when bvn is down
            //StatusCode NoCOntent


            return realResponseCode + "|" + realResponseContent;

            // string responsecode = response.ResponseStatus.ToString();




            //string innerResponse = "";
            //string ResponseCode = "";

            //if (response.StatusCode.ToString() == "Created")
            //{
            //    innerResponse = response.Headers[5].Value.ToString();
            //    ResponseCode = response.Headers[10].Value.ToString();
            //}
            //else
            //{
            //    innerResponse = response.Headers[4].Value.ToString();
            //    ResponseCode = response.Headers[9].Value.ToString();
            //}



            //return ResponseCode + "|" + innerResponse;

        }

        public string retrieveCustomerPurchaseSummary(BVNRecord2 transObj)
        {

            string nibbsurl = ConfigurationManager.AppSettings["nibbsurlforpurchasesummary"]; //pick this from config manager instead incase IP later changes, there will then be no need to recompile the sourcecode
            string username = ConfigurationManager.AppSettings["username"]; //pick from config manager
            string pswd = ConfigurationManager.AppSettings["password"]; //"KLKG+!xooYqlKi@!";  

            string nipcodeforsterling = ConfigurationManager.AppSettings["nipcodeforsterling"];

            string authorization = Gizmo.Base64Encode(username + ":" + pswd);

            string signatureDate = DateTime.Now.ToString("yyyyMMdd");


            string signatureString = username + signatureDate + pswd;

            string signature = Gizmo.GenerateSHA512String(signatureString);


            RestClient client = new RestClient(nibbsurl);

            ByPassProxy(client);



            var request = new RestRequest(Method.POST);

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            request.AddHeader("Accept", "text/plain");
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("InstitutionCode", nipcodeforsterling);
            request.AddHeader("ContentType", "text/plain");
            request.AddHeader("Authorization", authorization);
            request.AddHeader("SIGNATURE", signature);
            request.AddHeader("SIGNATURE_METH", "SHA512");
            request.AddHeader("TIMESTAMP", timestamp);







            String key = ConfigurationManager.AppSettings["AESKey"]; //"OUGW5XNSc/82rXAr"; 
            String iv = ConfigurationManager.AppSettings["IVKey"];// "1BNEMKUJXi3svqFk"; 





            var json = new JavaScriptSerializer().Serialize(transObj);






            String encryptedRequest = AES.Encrypt(json.ToString(), key, iv);


            var requestBody = request.AddParameter("text/plain", encryptedRequest, ParameterType.RequestBody);






            String decryptedText = AES.Decrypt(encryptedRequest, key, iv);



            var response = client.Execute(request);



            //when bvn is down
            //StatusCode NoCOntent

            string realResponseCode = "";
            string realResponseContent = "";

            //return bvnvalidationResponse;

            try
            {
                realResponseCode = Convert.ToString(response.Headers.Where(x => x.Name == "error").SingleOrDefault().Value);
                realResponseContent = Convert.ToString(response.Headers.Where(x => x.Name == "respData").SingleOrDefault().Value);


            }
            catch
            {
                realResponseCode = "-1";
                realResponseContent = "Error Receiving response from nibss service";
            }

            //when bvn is down
            //StatusCode NoCOntent


            return realResponseCode + "|" + realResponseContent;






            //string innerResponse = "";
            //string ResponseCode = "";

            //if (response.StatusCode.ToString() == "Created")
            //{
            //    innerResponse = response.Headers[5].Value.ToString();
            //    ResponseCode = response.Headers[10].Value.ToString();
            //}
            //else
            //{
            //    innerResponse = response.Headers[4].Value.ToString();
            //    ResponseCode = response.Headers[9].Value.ToString();
            //}



            //return ResponseCode + "|" + innerResponse;

        }


        public string SendPurchaseInfoSummaryToNibss(PurchaseInfoRecord transObj)
        {

            string nibbsurl = ConfigurationManager.AppSettings["nibbsurlforpurchaseinfo"]; //"http://192.234.10.104:86/icadservice/api/accountmanager"; //pick this from config manager instead incase IP later changes, there will then be no need to recompile the sourcecode
            string username = ConfigurationManager.AppSettings["username"]; //"tunde.ifafore@sterlingbankng.com"; //pick from config manager
            string pswd = ConfigurationManager.AppSettings["password"]; //"KLKG+!xooYqlKi@!";  

            string authorization = Gizmo.Base64Encode(username + ":" + pswd);



            string signatureDate = DateTime.Now.ToString("yyyyMMdd");


            string signatureString = username + signatureDate + pswd;

            string signature = Gizmo.GenerateSHA512String(signatureString);


            RestClient client = new RestClient(nibbsurl);

            ByPassProxy(client);

            string nipcodeforsterling = ConfigurationManager.AppSettings["nipcodeforsterling"];



            var request = new RestRequest(Method.POST);

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            request.AddHeader("Accept", "text/plain");
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("ContentType", "text/plain");
            request.AddHeader("InstitutionCode", nipcodeforsterling);
            request.AddHeader("Authorization", authorization);
            request.AddHeader("SIGNATURE", signature);
            request.AddHeader("SIGNATURE_METH", "SHA512");
            request.AddHeader("TIMESTAMP", timestamp);







            String key = ConfigurationManager.AppSettings["AESKey"]; //"OUGW5XNSc/82rXAr"; 
            String iv = ConfigurationManager.AppSettings["IVKey"];// "1BNEMKUJXi3svqFk"; 





            var json = new JavaScriptSerializer().Serialize(transObj);








            String encryptedRequest = AES.Encrypt(json.ToString(), key, iv);


            var requestBody = request.AddParameter("text/plain", encryptedRequest, ParameterType.RequestBody);






            String decryptedText = AES.Decrypt(encryptedRequest, key, iv);



            var response = client.Execute(request);


            // String decryptedText = AES.Decrypt(encryptedText, key, iv);
            //Console.WriteLine("Decrypted text :" + decryptedText);





            string realResponseCode = "";
            string realResponseContent = "";

            //return bvnvalidationResponse;

            try
            {
                realResponseCode = Convert.ToString(response.Headers.Where(x => x.Name == "error").SingleOrDefault().Value);
                realResponseContent = Convert.ToString(response.Headers.Where(x => x.Name == "responseInfo").SingleOrDefault().Value) + "-" + Convert.ToString(response.Headers.Where(x => x.Name == "respData").SingleOrDefault().Value);

                if (realResponseCode != "00")
                {
                    realResponseContent = Convert.ToString(response.Headers.Where(x => x.Name == "responseInfo").SingleOrDefault().Value);
                }


            }
            catch
            {
                realResponseCode = "-1";
                realResponseContent = "Error Receiving response from nibss service";
            }

            //when bvn is down
            //StatusCode NoCOntent


            return realResponseCode + "|" + realResponseContent;


        }

        public string createSingleCustomerAccount(BVNRecord transObj)
        {

            string nibbsurl = ConfigurationManager.AppSettings["nibbsurl"]; //"http://192.234.10.104:86/icadservice/api/accountmanager"; //pick this from config manager instead incase IP later changes, there will then be no need to recompile the sourcecode
            string username = ConfigurationManager.AppSettings["username"]; //"tunde.ifafore@sterlingbankng.com"; //pick from config manager
            string pswd = ConfigurationManager.AppSettings["password"]; //"KLKG+!xooYqlKi@!";  

            string authorization = Gizmo.Base64Encode(username + ":" + pswd);

            string signatureDate = DateTime.Now.ToString("yyyyMMdd");


            string signatureString = username + signatureDate + pswd;

            string signature = Gizmo.GenerateSHA512String(signatureString);


            RestClient client = new RestClient(nibbsurl);

            ByPassProxy(client);



            var request = new RestRequest(Method.POST);

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            request.AddHeader("Accept", "text/plain");
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("ContentType", "text/plain");
            request.AddHeader("Authorization", authorization);
            request.AddHeader("SIGNATURE", signature);
            request.AddHeader("SIGNATURE_METH", "SHA512");
            request.AddHeader("TIMESTAMP", timestamp);







            String key = ConfigurationManager.AppSettings["AESKey"]; //"OUGW5XNSc/82rXAr"; 
            String iv = ConfigurationManager.AppSettings["IVKey"];// "1BNEMKUJXi3svqFk"; 





            var json = new JavaScriptSerializer().Serialize(transObj);






            String encryptedRequest = AES.Encrypt(json.ToString(), key, iv);


            var requestBody = request.AddParameter("text/plain", encryptedRequest, ParameterType.RequestBody);






            String decryptedText = AES.Decrypt(encryptedRequest, key, iv);



            var response = client.Execute(request);


            // String decryptedText = AES.Decrypt(encryptedText, key, iv);
            //Console.WriteLine("Decrypted text :" + decryptedText);

            string innerResponse = "";
            string ResponseCode = "";

            if (response.StatusCode.ToString() == "Created")
            {
                innerResponse = response.Headers[5].Value.ToString();
                ResponseCode = response.Headers[10].Value.ToString();
            }
            else
            {
                innerResponse = response.Headers[4].Value.ToString();
                ResponseCode = response.Headers[9].Value.ToString();
            }



            return ResponseCode + "|" + innerResponse;

        }




        public string sendAPing()
        {

            string nibbsurl = ConfigurationManager.AppSettings["nibbsurlforpingservice"]; //"http://192.234.10.104:86/forexservice/api/"; //pick this from config manager instead incase IP later changes, there will then be no need to recompile the sourcecode
            string username = ConfigurationManager.AppSettings["username"]; //pick from config manager
            string pswd = ConfigurationManager.AppSettings["password"]; //"KLKG+!xooYqlKi@!";  


            string authorization = Gizmo.Base64Encode(username + ":" + pswd);

            string signatureDate = DateTime.Now.ToString("yyyyMMdd");


            string signatureString = username + signatureDate + pswd;

            string signature = Gizmo.GenerateSHA512String(signatureString);


            RestClient client = new RestClient(nibbsurl);

            ByPassProxy(client);

            var request = new RestRequest("ping", Method.GET);

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);

            return response.StatusCode + "|" + response.Content;

            //return response;

        }



        public static void ByPassProxy(RestClient client)
        {

            IWebProxy proxy = new WebProxy();
            client.Proxy = proxy;
            client.PreAuthenticate = true;
        }
        public string resetCredentials()
        {
            string nibsreset = ConfigurationManager.AppSettings["nibbsurlforresetpword"];
            string username = ConfigurationManager.AppSettings["username"]; //pick from config manager
            string pswd = ConfigurationManager.AppSettings["password"]; //"KLKG+!xooYqlKi@!";  
            string nipcodeforsterling = ConfigurationManager.AppSettings["nipcodeforsterling"];
            string authorization = Gizmo.Base64Encode(username + ":" + pswd);
            string signatureDate = DateTime.Now.ToString("yyyyMMdd");
            string signatureString = username + signatureDate + pswd;
            string signature = Gizmo.GenerateSHA512String(signatureString);
            RestClient client = new RestClient(nibsreset);
            ByPassProxy(client);
            var request = new RestRequest(Method.POST);
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            //add headers
            request.AddHeader("Accept", "text/plain");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", authorization);
            request.AddHeader("SIGNATURE", signature);
            request.AddHeader("SIGNATURE_METH", "SHA512");
            request.AddHeader("TIMESTAMP", timestamp);
            request.AddHeader("InstitutionCode", nipcodeforsterling);

            String key = ConfigurationManager.AppSettings["AESKey"]; //"OUGW5XNSc/82rXAr"; 
            String iv = ConfigurationManager.AppSettings["IVKey"];// "1BNEMKUJXi3svqFk"; 

            //var json = new JavaScriptSerializer().Serialize(username);

            String encryptedRequest = AES.Encrypt(username, key, iv);
            var requestBody = request.AddParameter("text/plain", encryptedRequest, ParameterType.RequestBody);
            String decryptedText = AES.Decrypt(encryptedRequest, key, iv);


            var response = client.Execute(request);
            string ResponseCode = "";
            string credeResp = "";
            try
            {
                if (response.StatusCode.ToString() != null)
                {
                    // innerResponse = response.Headers.ToString();
                    string resetresp = response.StatusDescription + "|" + response.StatusCode + "|" + response.Content;
                    return resetresp;
                    //  ResponseCode = response.Headers.ToString(); ..return this
                    // credeResp = Convert.ToString(response.Headers.Where(x => x.Name == "respData").SingleOrDefault().Value);
                }
                else
                {
                    string resetresp = "no response";
                    return resetresp;
                }
            }
            catch (Exception ex)
            {
                return "error " + ex.ToString();
                //do nothing
            }

            ////pass the username to this mehtod..username is the email address
            //string nibsreset = ConfigurationManager.AppSettings["nibbsurlforresetpword"];
            //string usernames = ConfigurationManager.AppSettings["username"]; //pick from config manager
            //string pswd = ConfigurationManager.AppSettings["password"];
            ////create the ReSt-client
            //RestClient client = new RestClient(nibsreset);
            //ByPassProxy(client);
            //var request = new RestRequest(Method.POST);

            //var requestBody = request.AddParameter("text/plain", usernames);
            ////send the request
            //var response = client.Execute(request);
            //string innerResponse = "";
            //string ResponseCode = "";
            //string credeResp = "";
            //try
            //{
            //    if (response.StatusCode.ToString() != null)
            //    {
            //        innerResponse = response.Headers.ToString();
            //        //  ResponseCode = response.Headers.ToString(); ..return this
            //        credeResp = Convert.ToString(response.Headers.Where(x => x.Name == "respData").SingleOrDefault().Value);
            //    }
            //    else
            //    {
            //        innerResponse = response.Headers[4].Value.ToString();
            //        ResponseCode = response.Headers[9].Value.ToString();
            //    }
            //    return innerResponse + "|" + credeResp + "|" + response.Content.ToString();
            //}
            //catch (Exception ex)
            //{
            //    return "error " + ex.ToString();
            //    //do nothing
            //}
        }
        ///************************NIBSS Hackathon*****************************************************///

        public string GenerateCredentials(string bvn)
        {
            string NibssBaseUrl = ConfigurationManager.AppSettings["NibssBaseApiUrl"];
            string MethodUrl = "/GenerateCredentials";
            NibssBaseUrl += MethodUrl;
            RestClient client = new RestClient(NibssBaseUrl);
            //  ByPassProxy(client);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "text/plain");
            request.AddHeader("Content-Type", "application/json");

            var Base64Bvn = Gizmo.Base64Encode(bvn);
            request.AddHeader("UserCode", Base64Bvn);

            //execute the request.
            var response = client.Execute(request);

            //read the headers for the iv and AES keys and also the hash.
            //The request was successful and AES and IV keys and Hash were successfully generated and returned via AES_KEY, IVKey and HASH header parameters respectively.

            if (response.StatusCode.ToString() != null)
            {
                //log and read the headers
                try
                {
                    string resetresp = response.StatusDescription + "|" + response.StatusCode + "|" + response.Content;
                    string Aes_key = Convert.ToString(response.Headers.Where(x => x.Name == "AES_KEY").SingleOrDefault().Value);
                    string IVKey = Convert.ToString(response.Headers.Where(x => x.Name == "IVKey").SingleOrDefault().Value);
                    string HASH = Convert.ToString(response.Headers.Where(x => x.Name == "HASH").SingleOrDefault().Value);

                    var header = response.Headers;
                    foreach (var headerval in header)
                    {
                        new ErrorLog("Name: " + headerval.Name + " Value:" + headerval.Value.ToString());
                    }
                    string credentials = "AES KEY :" + Aes_key + "|| IV-KEY: " + IVKey + " || Hash: " + HASH;
                    new ErrorLog(resetresp + " Credentials: " + credentials);
                    return resetresp + " credentials:" + credentials;

                }
                catch (Exception ex)
                {
                    new ErrorLog(ex);

                    return "an error occurred";
                }
            }
            else
            {
                string resetresp = "no response";
                return resetresp;
            }

        }

        public MatchWithBvnResp MatchWithBvn(string Bvn, string Position, string ISOTemplate)
        {
            string NibssBaseUrl = ConfigurationManager.AppSettings["NibssBaseApiUrl"];
            string MethodUrl = "/MatchWithBVN";
            NibssBaseUrl += MethodUrl;
            RestClient client = new RestClient(NibssBaseUrl);
            //  ByPassProxy(client);
            var hash = ConfigurationManager.AppSettings["hackHASH"];
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Hash", hash);

            String key = ConfigurationManager.AppSettings["hackAES"]; //"OUGW5XNSc/82rXAr"; 
            String iv = ConfigurationManager.AppSettings["hackIVKey"];// "1BNEMKUJXi3svqFk"; 

            MatchWithBvnReq req = new MatchWithBvnReq
            {
                BVN = Bvn,
                Position = Position,
                ISOTemplate = ISOTemplate
            };


            var json = new JavaScriptSerializer().Serialize(req);

            //encrypt the request
            String encryptedRequest = AES.Encrypt(json.ToString(), key, iv);

            var requestBody = request.AddParameter("application/json", encryptedRequest, ParameterType.RequestBody);

            // String decryptedText = AES.Decrypt(encryptedRequest, key, iv);
            var response = client.Execute(request);

            //decrypt the response body

            var responseBody = response.Content.ToString();
            new ErrorLog(responseBody);

            string decryptedResponse = AES.Decrypt(responseBody, key, iv);
            var resp = new JavaScriptSerializer().Deserialize<MatchWithBvnResp>(decryptedResponse);
            //log response
            new ErrorLog(resp.ToString());

            return resp;
        }


    }


}
