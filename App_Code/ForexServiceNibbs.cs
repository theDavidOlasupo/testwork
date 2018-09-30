using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Net;
using RestSharp;
using SterlingForexService;
using SterlingForexService.DAL;
using SterlingForesxService.com.sbp.utility;
using SterlingForexService.com.sbp.utility;

/// <summary>
/// Summary description for ForexServiceNibbs
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class ForexServiceNibbs : System.Web.Services.WebService
{

    public ForexServiceNibbs()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    
  



    [WebMethod]
    public string sendAPing()
    {
        

        RestServiceInvocator restServiceObj = new RestServiceInvocator();

        string pingResp = restServiceObj.sendAPing();
        return pingResp;
    }

    [WebMethod]
    public string ResetCredentials()
    {
        RestServiceInvocator restobj = new RestServiceInvocator();
        string resp = restobj.resetCredentials();
        return resp;
    }


    [WebMethod]
    public string validateBVNforForex(string applicantsBVN, string beneficiaryBVN)
    {
        RestServiceInvocator restServiceObj = new RestServiceInvocator();

        BVNRecord transObj = new BVNRecord()
        {

            bvnApplicant = applicantsBVN,  //"22222222280",
            bvnBeneficiary = beneficiaryBVN // "22222222280"
        };

        string pingResp = restServiceObj.validateBVNforForex(transObj);



        return pingResp;
    }

    [WebMethod]
    public string retrieveCustomerPurchaseSummary(string applicantsBVN, string applicantsAccount)
    {

        BVNRecord2 transObj = new BVNRecord2()
        {

            bvnApplicant = applicantsBVN,
            bvnApplicantAccount = applicantsAccount
        };



        RestServiceInvocator restServiceObj = new RestServiceInvocator();

       

        string response = restServiceObj.retrieveCustomerPurchaseSummary(transObj);

        return response;
    }

    [WebMethod]
    public string postForexPurchaseSummaryToNibss(string bvnBeneficiary, string bvnApplicant, string bvnApplicantAccount, int transactionType, string purpose, double amount, double rate, string requestDate, string requestId, string passportNumber)
    {
        RestServiceInvocator restServiceObj = new RestServiceInvocator();


        string reqId = utility.generateRequestId();

        //PurchaseInfoRecord transObj = SetARecord(bvnBeneficiary, bvnApplicant, bvnApplicantAccount, transactionType, purpose, amount, rate, requestDate, requestId, passportNumber);

        PurchaseInfoRecord transObj = SetARecord(bvnBeneficiary, bvnApplicant, bvnApplicantAccount, transactionType, purpose, amount, rate, requestDate, reqId, passportNumber);

        string response = restServiceObj.SendPurchaseInfoSummaryToNibss(transObj);

        return response;
    }

    [WebMethod]
    public string GenerateCredentials( string bvn)
    {
        RestServiceInvocator restobj = new RestServiceInvocator();
        string resp = restobj.GenerateCredentials(bvn);
        return resp;
    }
    [WebMethod]
    public MatchWithBvnResp MatchWithBvn(string Bvn, string Position, string ISOTemplate)
    {
        RestServiceInvocator restobj = new RestServiceInvocator();
        var resp = restobj.MatchWithBvn(Bvn, Position, ISOTemplate);
        return resp;
    }


    private static PurchaseInfoRecord SetARecord(string bvnBeneficiary, string bvnApplicant, string bvnApplicantAccount, int transactionType, string purpose, double amount, double rate, string requestDate, string requestId, string passportNumber)
    {

        try
        {




            //confirm the bvn at this point by calling the bvn validation service
            //if valid concatenate the date of birth|surname|firstname|middlename
            //else return a null object

            //DataSet bvnResponse =  Bank.GetBVN(thebvn);


            PurchaseInfoRecord transObj = new PurchaseInfoRecord()
            {
                bvnBeneficiary = bvnBeneficiary,

                bvnApplicant = bvnApplicant,

                bvnApplicantAccount = bvnApplicantAccount,

                transactionType = transactionType,

                purpose = purpose,

                amount = amount,

                rate = rate,

                requestDate = requestDate,

                requestID = requestId,

                passportNumber = passportNumber
            };



            return transObj;

        }
        catch (Exception ex)
        {

            new ErrorLog(ex.ToString());
            return null;
        }






    }



}