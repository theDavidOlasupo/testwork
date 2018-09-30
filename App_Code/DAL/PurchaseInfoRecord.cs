using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingForexService.DAL
{
    public class PurchaseInfoRecord
    {

        public string bvnBeneficiary { get; set; }
        public string bvnApplicant { get; set; }

        public string bvnApplicantAccount { get; set; }

        public int transactionType { get; set; }
        
        public string purpose { get; set; }

        public double amount { get; set; }

        public double rate { get; set; }

        public string requestDate { get; set; }

        public string requestID { get; set; }

        public string passportNumber { get; set; }


    }
}
