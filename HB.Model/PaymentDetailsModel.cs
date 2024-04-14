using Strateq.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Model
{
    public class PaymentDetailsRequestModel: RequestModelBase
    {
        public int BookingId { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string NameOnCard { get; set; }
        public DateTime ExpirationDate { get; set; }

    }

}
