using SterlingForesxService.com.sbp.utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingForexService.com.sbp.utility
{
    public static class utility
    {
        public static int GetCurrency(string currency)
        {
            int curcode = 5;

            try
            {
                switch (currency)
                {
                    case "NGN":
                        curcode = 1;
                        break;

                    case "USD":
                        curcode = 2;
                        break;

                    case "GBP":
                        curcode = 3;
                        break;

                    case "EUR":
                        curcode = 4;
                        break;

                    default:
                        curcode = 5;
                        break;


                }

                return curcode;
            }
            catch (Exception ex)
            {

                new ErrorLog(ex.ToString());
                return curcode = 1;
            }
            


           
        }

        public static string formatDateOfBirth(string thedate)
        {

            try
            {
                DateTime dt2 = DateTime.ParseExact(thedate, "dd-MMM-yy", CultureInfo.InvariantCulture);



                try
                {


                    string newmonth = ""; string newday = "";

                    if (Convert.ToInt32(dt2.Month) < 10)
                    {
                        newmonth = "0" + dt2.Month;
                    }
                    else
                    {
                        newmonth = Convert.ToString(dt2.Month);
                    }

                    if (Convert.ToInt32(dt2.Day) < 10)
                    {
                        newday = "0" + dt2.Day;
                    }
                    else
                    {
                        newday = Convert.ToString(dt2.Day);
                    }

                    string newdateofbirth = dt2.Year + "" + newmonth + "" + newday;

                    return newdateofbirth;

                }
                catch (Exception ex1)
                {
                    ErrorLog errlog = new ErrorLog(ex1.ToString());
                    return null;
                }

            }
            catch (Exception ex)
            {

                ErrorLog errlog = new ErrorLog(ex.ToString());
                return null;
            }










        }



        public static string formatDateYYYYMMDD(DateTime thedate)
        {

            try
            {
                DateTime dt2 = thedate;



                try
                {


                    string newmonth = ""; string newday = "";

                    if (Convert.ToInt32(dt2.Month) < 10)
                    {
                        newmonth = "0" + dt2.Month;
                    }
                    else
                    {
                        newmonth = Convert.ToString(dt2.Month);
                    }

                    if (Convert.ToInt32(dt2.Day) < 10)
                    {
                        newday = "0" + dt2.Day;
                    }
                    else
                    {
                        newday = Convert.ToString(dt2.Day);
                    }

                    string newdateofbirth = dt2.Year + "" + newmonth + "" + newday;

                    return newdateofbirth;

                }
                catch (Exception ex1)
                {
                    ErrorLog errlog = new ErrorLog(ex1.ToString());
                    return null;
                }

            }
            catch (Exception ex)
            {

                ErrorLog errlog = new ErrorLog(ex.ToString());
                return null;
            }


}



        public static string generateRequestId()
        {

            string requestId = Gizmo.newSessionGlobal();

            return requestId;
        }

        public static List<string> determineWhichNameArrangementToForwardToNibbs(List<string> namelistFromBVN, List<string> namelistFromCore)
        {

            //from the sampling of data, it is obvious that names are being randomly arranged in the Core Banking
            //this is due mostly to data entry errors
            //therefore the approach i will adopt is to take each name from the core in no particular order and see if it even exists in the list as returned from the bvn service
            //if the name exists, i will simply return the arrangement as it is on the bvn service

            //if coreFirstname equals bvnFirstName || bvnMiddleName

            //if coreMiddlename equals bvnFirstName || bvnMiddleName

            //if coreLastname equals bvnLastName 


            string firstNameFromBVN = namelistFromBVN[0];
            string middleNameFromBVN = namelistFromBVN[1];
            string lastNameFromBVN = namelistFromBVN[2];

            string firstNameFromCore = namelistFromCore[1];
            string middleNameFromCore = namelistFromCore[2];
            string lastNameFromCore = namelistFromCore[0];


            if(lastNameFromCore == lastNameFromBVN)
            {
                if(firstNameFromCore == firstNameFromBVN || firstNameFromCore == middleNameFromBVN)
                {

                    if (middleNameFromCore == middleNameFromBVN || middleNameFromCore == firstNameFromBVN || middleNameFromCore == "")
                    {
                        return namelistFromBVN;
                    }
                    else
                    {
                        return null;
                    }

                    
                    
                }
                else
                {
                    return null;
                }

               
            }
            else
            {
                return null;
            }

            



            


        }



        public static List<string> splitName(string fullname)
        {
            List<string> namelist = new List<string>();

            string middlename = "";
            string firstname = "";
            string lastname = "";

            string dob = "";

            string mobile = "";



            try
            {
               

                string[] strArr = null;


                char[] splitchar = { ' ' };
                strArr = fullname.Split(splitchar);

               

                if (strArr.Length > 0)
                {
                     
                    try
                    {
                        lastname = strArr[0];
                        firstname = strArr[1];
                        middlename = strArr[2];
                    }
                    catch (Exception ex)
                    {
                        middlename = "";
                    }


                    namelist.Add(lastname);
                    namelist.Add(firstname);
                    namelist.Add(middlename);




                }
                else
                {
                    namelist = null;

                }





               


                return namelist;

            }
            catch
            {
                return null; // list;

            }



        }


        public static List<string> splitPhoneNumber(string phonelist)
        {
            List<string> mobilephonelist = new List<string>();

            string mobile1 = null;
            string altmobile = null;
            



            try
            {

                if(phonelist != "" && phonelist != null)
                {




                    string[] strArr = null;


                    char[] splitchar = { '?' };
                    strArr = phonelist.Split(splitchar);



                    if (strArr.Length > 0)
                    {

                        try
                        {
                            mobile1 = strArr[0];
                            altmobile = strArr[1];

                        }
                        catch (Exception ex)
                        {

                        }


                        mobilephonelist.Add(mobile1);
                        mobilephonelist.Add(altmobile);





                    }
                    else
                    {
                        mobilephonelist = null;

                    }








                    return mobilephonelist;

                }
                else
                {
                    return null;
                }

                

            }
            catch
            {
                return null; // list;

            }



        }



    }
}
