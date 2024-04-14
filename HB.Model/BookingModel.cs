using Strateq.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Model
{
    public class BookingRequestModel: RequestModelBase
    {
        public BookingRequest BookingRequests { get; set; } = new();

        public GuestDetailsRequest GuestDetailsRequests { get; set; } = new();

        public PaymentDetailsRequest PaymentDetailsRequests { get; set; } = new();

        //public string UserName { get; set; }
        //public string UserEmail { get; set; }
        public int UserAccountId { get; set; }


        public class BookingRequest
        {
            public int Adult { get; set; }
            public int Children { get; set; }
            public string Person { get; set; }
            public string Price { get; set; }
            public string Size { get; set; }
            public string Src { get; set; }
            public string Type { get; set; }
            public string Bed { get; set; }
            public DateTime CheckInDate { get; set; }
            public DateTime CheckOutDate { get; set; }
        }

        public class GuestDetailsRequest
        {
            public int BookingId { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Phone { get; set; }
        }
        
        public class PaymentDetailsRequest
        {
            public int BookingId { get; set; }
            public string CardNumber { get; set; }
            public string CVV { get; set; }
            public string NameOnCard { get; set; }
            public string ExpirationDate { get; set; }
        }

    }

    public class BookingResponseModel : ResponseModelBase
    {
        public string UserEmail { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public string Price { get; set; }
        public string Type { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int BookingId { get; set; }
        public string Src { get; set; }
        public string GuestEmail { get; set; }
    }

    public class BookingDisplayListResponseModel : ResponseModelBase
    {
        public List<BookingResponseModel> BookingList { get; set; }
    }

    public class UserBookingRequestModel : RequestModelBase
    {
        public string Email { get; set; }
    }
}
