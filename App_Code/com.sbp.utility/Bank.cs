using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SterlingForexService.com.sbp.utility;

using System.Threading;
using System.Configuration;


namespace SterlingForexService.com.sbp.utility
{
    public static class Bank
    {


        

        private static string connstringICAD = ConfigurationManager.AppSettings["icadDB"];


          


        

        public static DataSet GetBVN(string bvn)
        {
            
            DataSet ds = new DataSet();
            try
            {
                

                
            }
            catch { }
            return ds;
        }






        //this method checks to see if customer has previously being included in a sample survey
        //if customer has been sampled before then it checks if the difference in time is up to 60 days
        public static bool hasCustomerBeingRecentlySampled(string nuban,int daysinconsideration)
        {
            try
            {
                DateTime endDate = DateTime.Now.AddDays(1);
                DateTime startDate = endDate.AddDays(-daysinconsideration); //endDate.AddDays(-60);

                string sql = "select  refid, created_at from ISM.dbo.tbl_survey_master where nuban =" + nuban + " AND created_at BETWEEN '" + startDate + "' AND '" + endDate + "'";

                MSQconn cn = new MSQconn(connstringICAD); //(connstringAlertSystem);
                cn.SetSQL(sql);

                DataSet ds = cn.Select();
                int count = ds.Tables[0].Rows.Count;

                if (count > 0)
                {
                    return true;
                }
                else
                {
                    //check T24_TBL_ALERT for the aans_status

                    //this is an additional check to ensure customer is interested in getting transaction alerts
                     bool aans_stat = checkAANSstatus(nuban);
                    
                     if(aans_stat == true)
                     {
                         return false;
                     }
                     else
                     {
                         return true;
                     }

                    
                }
            

            }
            catch (Exception ex)
            {
                new ErrorLog(ex.ToString());
            }



            return true;

        }




        public static bool checkAANSstatus(string nuban)
        {
            try
            {
              

                string sql = "select serv_type,aans_status from t24_tbl_alerts where nuban = " + nuban + " AND  AANS_STATUS = '1'";

                MSQconn cn = new MSQconn(connstringICAD); //(connstringAlertSystem);
                cn.SetSQL(sql);

                DataSet ds = cn.Select();
                int count = ds.Tables[0].Rows.Count;

                if (count > 0)
                {
                    return true;
                }
                else
                {
                   


                    return false;
                }


            }
            catch (Exception ex)
            {
                new ErrorLog(ex.ToString());
            }



            return true;

        }

       
       




        //This fetches data from the SqlServer which contains all alert messages
        public static DataSet GetCustomerDetailsNewAccountsFromICADDataSource(int rowstart, int rowend, string startDate, string endDate)
        {



            //string sql = "select * from (select refid,ACCOUNTID,COCODE,SHORTTITLE,CURRENCY,CUSTOMER ,CATEGORY ,ACCOUNTTITLE1 ,ACCOUNTTITLE2,OPENINGDATE,ALTACCTID,HVTFLAG,ACCTOFFICER,INTRODUCER,STACODE, EMAIL,CUSTOMERSTATUS ,POSTRESTRICT,BVN ,TARGET,SECTOR ,INDUSTRY,DATETIME AS MOBILENUMBER,DATETIME1 AS DATEMODIFIED,status, ROW_NUMBER() OVER (order by refid) as row from ICAD.dbo.icadmain where OPENINGDATE BETWEEN @sd AND @ed AND (Accountid is not null) AND (bvn is not null) AND ( responsecode is null OR responsecode = '') ) a WHERE row > " + rowstart + " and row <= " + rowend + " order by refid desc";

            string sql = "select * from (select refid,ACCOUNTID,COCODE,SHORTTITLE,CURRENCY,CUSTOMER ,CATEGORY ,ACCOUNTTITLE1 ,ACCOUNTTITLE2,OPENINGDATE,ALTACCTID,HVTFLAG,ACCTOFFICER,INTRODUCER,STACODE, EMAIL,CUSTOMERSTATUS ,POSTRESTRICT,BVN ,TARGET,SECTOR ,INDUSTRY,DATETIME AS MOBILENUMBER,DATETIME1 AS DATEMODIFIED,status, ROW_NUMBER() OVER (order by refid) as row from ICAD.dbo.icadmain where OPENINGDATE BETWEEN @sd AND @ed AND (Accountid is not null) AND ( responsecode is null OR responsecode = '') ) a WHERE row > " + rowstart + " and row <= " + rowend + " order by refid desc";




            MSQconn cn = new MSQconn(connstringICAD);

            cn.SetSQL(sql);
            cn.AddParam("@sd", startDate);
            cn.AddParam("@ed", endDate);


            return cn.Select();
        }




        //This fetches data from the SqlServer which contains all alert messages
        public static DataSet GetCustomerDetailsModifiedAccountsFromICADDataSource(int rowstart, int rowend, string startDate, string endDate)
        {


            //string sql = "select * from (select refid,ACCOUNTID,COCODE,SHORTTITLE,CURRENCY,CUSTOMER ,CATEGORY ,ACCOUNTTITLE1 ,ACCOUNTTITLE2,OPENINGDATE,ALTACCTID,HVTFLAG,ACCTOFFICER,INTRODUCER,STACODE, EMAIL,CUSTOMERSTATUS ,POSTRESTRICT,BVN ,TARGET,SECTOR ,INDUSTRY,DATETIME AS MOBILENUMBER,DATETIME1 AS DATEMODIFIED,status, ROW_NUMBER() OVER (order by refid) as row from ICAD.dbo.icadmain where status is NULL AND bvn is not null) a WHERE row > " + rowstart + " and row <= " + rowend + " order by refid desc";
           //string sql = "select * from (select refid,ACCOUNTID,COCODE,SHORTTITLE,CURRENCY,CUSTOMER ,CATEGORY ,ACCOUNTTITLE1 ,ACCOUNTTITLE2,OPENINGDATE,ALTACCTID,HVTFLAG,ACCTOFFICER,INTRODUCER,STACODE, EMAIL,CUSTOMERSTATUS ,POSTRESTRICT,BVN ,TARGET,SECTOR ,INDUSTRY,DATETIME AS MOBILENUMBER,DATETIME1 AS DATEMODIFIED,status, ROW_NUMBER() OVER (order by refid) as row from ICAD.dbo.icadmain where bvn is not null AND bvn != '' AND refid BETWEEN '8529' AND '9571' AND DATETIME1 BETWEEN @sd AND @ed) a WHERE row > " + rowstart + " and row <= " + rowend + " order by refid desc";


            //string sql = "select * from (select refid,ACCOUNTID,COCODE,SHORTTITLE,CURRENCY,CUSTOMER ,CATEGORY ,ACCOUNTTITLE1 ,ACCOUNTTITLE2,OPENINGDATE,ALTACCTID,HVTFLAG,ACCTOFFICER,INTRODUCER,STACODE, EMAIL,CUSTOMERSTATUS ,POSTRESTRICT,BVN ,TARGET,SECTOR ,INDUSTRY,DATETIME AS MOBILENUMBER,DATETIME1 AS DATEMODIFIED,status, ROW_NUMBER() OVER (order by refid) as row from ICAD.dbo.icadmain where DATETIME1 BETWEEN @sd AND @ed AND (Accountid is not null) AND (bvn is not null) AND ( responsecode is null OR responsecode = '') ) a WHERE row > " + rowstart + " and row <= " + rowend + " order by refid desc";
            string sql = "select * from (select refid,ACCOUNTID,COCODE,SHORTTITLE,CURRENCY,CUSTOMER ,CATEGORY ,ACCOUNTTITLE1 ,ACCOUNTTITLE2,OPENINGDATE,ALTACCTID,HVTFLAG,ACCTOFFICER,INTRODUCER,STACODE, EMAIL,CUSTOMERSTATUS ,POSTRESTRICT,BVN ,TARGET,SECTOR ,INDUSTRY,DATETIME AS MOBILENUMBER,DATETIME1 AS DATEMODIFIED,status, ROW_NUMBER() OVER (order by refid desc) as row from ICAD.dbo.icadmain where DATETIME1 BETWEEN @sd AND @ed AND (Accountid is not null)  AND ( shorttitle is not null OR shorttitle != '') AND ( responsecode is null OR responsecode = '') ) a WHERE row > " + rowstart + " and row <= " + rowend + " order by refid desc";

           


            MSQconn cn = new MSQconn(connstringICAD);

            cn.SetSQL(sql);
            cn.AddParam("@sd", startDate);
            cn.AddParam("@ed", endDate);


            return cn.Select();
        }





        //This fetches data from the Oracle t24_mapper_tt which contains all transactions carried out by Tellers on behalf of customers
        public static DataSet GetBranchTransactions(int startrow,int endrow,string starttime, string endtime)
        {

            //string sql = "select Top 500 refid,ACCOUNTID,COCODE,SHORTTITLE,CURRENCY,CUSTOMER ,CATEGORY ,ACCOUNTTITLE1 ,ACCOUNTTITLE2,OPENINGDATE,ALTACCTID,HVTFLAG,ACCTOFFICER,INTRODUCER,STACODE, EMAIL,CUSTOMERSTATUS ,POSTRESTRICT,BVN ,TARGET,SECTOR ,INDUSTRY,DATETIME AS MOBILENUMBER,DATETIME1 AS DATEMODIFIED,status, ROW_NUMBER() OVER (order by 1) as row from ICAD.dbo.icadmain where DATEMODIFIED BETWEEN @sd AND @ed order by refid desc) a WHERE row > " + rowstart + " and row <= " + rowend; // +")";



            //string sql = "SELECT TOP (1000) [refid],[ACCOUNTID],[COCODE],[SHORTTITLE],[CURRENCY],[CUSTOMER],[CATEGORY] ,[ACCOUNTTITLE1],[ACCOUNTTITLE2],[OPENINGDATE] ,[ALTACCTID],[HVTFLAG] ,[ACCTOFFICER]"
            //             +",[INTRODUCER],[STACODE],[EMAIL],[CUSTOMERSTATUS] ,[POSTRESTRICT],[BVN],[TARGET],[SECTOR],[INDUSTRY],[DATETIME],[DATETIME1],"
            //            + " [status] FROM [ICAD].[dbo].[icadmain]";
             /* string sql = "select * from ( " +
                            " SELECT ID,TTACCOUNT1,TTACCOUNT2,TTCOCODE,TTCURRENCY1,TTCUSTOMER2,TTDRCRMARKER,TTNARRATIVE2,TTNETAMOUNT,TTAMOUNTLOCAL1,TTAMOUNTLOCAL2,TTTRANSACTIONCODE,TTVALUEDATE1,TTVALUEDATE2,lastupdatedtime from t24_mapper_tt  where lastupdatedtime between TO_DATE(:sd,'yyyy-mm-dd HH24:MI:SS') AND TO_DATE(:ed,'yyyy-mm-dd HH24:MI:SS') AND  ttdrcrmarker='CREDIT' AND ttnarrative2 != '(null)'  AND ttcustomer2 != '(null)' order by lastupdatedtime desc) where rownum <= " + offset;
            */

             /* string sql = "select * from ( " +
                              " SELECT ID,TTACCOUNT1,TTACCOUNT2,TTCOCODE,TTCURRENCY1,TTCUSTOMER2,TTDRCRMARKER,TTNARRATIVE2,TTNETAMOUNT,TTAMOUNTLOCAL1,TTAMOUNTLOCAL2,TTTRANSACTIONCODE,TTVALUEDATE1,TTVALUEDATE2,lastupdatedtime from t24_mapper_tt  where lastupdatedtime between TO_DATE(:sd,'mm/dd/yyyy HH:MI:SS AM') AND TO_DATE(:ed,'mm/dd/yyyy HH:MI:SS PM') AND  ttdrcrmarker='CREDIT' AND ttnarrative2 != '(null)'  AND ttcustomer2 != '(null)' order by lastupdatedtime desc) where rownum <= " + offset;
            */




            //string sql = "select * from ( select a.*, ROWNUM rnum from ( select * from t24_mapper_tt where lastupdatedtime between TO_DATE(:sd,'mm/dd/yyyy HH:MI:SS AM') AND TO_DATE(:ed,'mm/dd/yyyy HH:MI:SS PM') order by lastupdatedtime desc) a where ROWNUM <= :endpoint ) where rnum  >= :startpoint";


             //sql = "select * from ( select a.*, ROWNUM rnum from ( select * from t24_mapper_tt where lastupdatedtime between TO_DATE(:sd,'mm/dd/yyyy HH:MI:SS AM') AND TO_DATE(:ed,'mm/dd/yyyy HH:MI:SS PM') order by lastupdatedtime desc) a where ROWNUM <= 500 ) where rnum  >= 1";



            DataSet ds = new DataSet();

            

            return ds;



            
        }



        public static DataSet GetConfigs()
        {


            string sql = "select Tab_text as mKey, var1 as mVal from tbl_icad_config where tab_id = 5";

            MSQconn cn = new MSQconn(connstringICAD);

            cn.SetSQL(sql);


            return cn.Select();


        }
       

        private static string CleanStaCodes(string staCodes)
        {
            string resp = ",";
            char[] sep = { ',' };
            string[] sta_codes = staCodes.Split(sep);
            if (sta_codes.Length <= 0) return "1";
            foreach (string sta_code in sta_codes)
            {
                int k = 0;
                try
                {
                    k = Convert.ToInt32(sta_code);
                }
                catch
                {

                }
                if (k > 0) resp += k + ",";
            }
            resp = resp.Trim(sep);
            return resp;
        }


        public static void populateEmailSurvey()
        {
           
           
            


        }


        //handles remarks that contain "NIP/" string
        public static DataSet getNIPTransactionDetailsVer1(string uniqueRef)
        {


           
            string sql = "select a.channelCode as channelCode,b.channelNames as channelName FROM tbl_nibssmobile a inner join tbl_channelcodes b ON a.channelCode=b.codes AND sessionid=@transcode";
            
            MSQconn cn = new MSQconn(connstringICAD);

             cn.SetSQL(sql);
             cn.AddParam("@transcode", uniqueRef);
           


            return  cn.Select();
        }

        //handles remarks that contain "NIP/" string
        public static DataSet getIBSTransactionDetailsVer1(string uniqueRef)
        {


            
            string sql = "select a.AppId as channelCode,b.ApplicationName as channelName FROM vew_ft_nip_intra_bank a inner join tbl_applicationKey b ON a.AppId=b.Appid AND ReferenceId=@transcode";

            MSQconn cn = new MSQconn(connstringICAD);

            cn.SetSQL(sql);
            cn.AddParam("@transcode", uniqueRef);



            return cn.Select();
        }



        public static int updateResponseFromNibbs(int refid,string responsecode,string responsetext)
        {

            MSQconn cn = new MSQconn(connstringICAD);
            string sysdate = DateTime.Now.ToString();
            string sql = "update icadmain set updated_at=@sysdate ,status='1', responsecode=@respcode, responsetext=@responsedesc  where refid=@refid";

            cn.SetSQL(sql);
            cn.AddParam("@sysdate",sysdate);
            cn.AddParam("@respcode",responsecode);
            cn.AddParam("@responsedesc", responsetext);
            cn.AddParam("@refid", refid);


           
            int cnt = cn.Update();

            return cnt;

        }


        public static int updateBulkResponseFromNibbs(string refidlist, string responsecode, string responsetext)
        {
            
            string inquerylist = "(";

            inquerylist += refidlist + "'' )";

            MSQconn cn = new MSQconn(connstringICAD);
            string sysdate = DateTime.Now.ToString();
            string sql = "update icadmain set updated_at=@sysdate ,status='1', responsecode=@respcode, responsetext=@responsedesc  where refid IN " + "(" + @refidlist + " )";

            cn.SetSQL(sql);
            cn.AddParam("@sysdate", sysdate);
            cn.AddParam("@respcode", responsecode);
            cn.AddParam("@responsedesc", responsetext);
            cn.AddParam("@refidlist", refidlist);

            Console.WriteLine("Response Text for  " + inquerylist + " Response code:" + responsecode);



            int cnt = cn.Update();

            return cnt;

        }



    }
}
