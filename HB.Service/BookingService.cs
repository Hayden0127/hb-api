using HB.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HB.Utilities;
using HB.Model;
using HB.Database.DbModels;
using Strateq.Core.Utilities;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using static Strateq.Core.Utilities.SystemDataCore;

namespace HB.Service
{
    public class BookingService : IBookingService
    {
        #region Fields
        private readonly IBookingRepository _bookingRepository;
        private readonly IGuestDetailsRepository _guestDetailsRepository;
        private readonly IPaymentDetailsRepository _paymentDetailsRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        #endregion

        #region Ctor
        public BookingService(IBookingRepository bookingRepository,
            IGuestDetailsRepository guestDetailsRepository,
            IPaymentDetailsRepository paymentDetailsRepository,
            IUserAccountRepository userAccountRepository)
        {
            _bookingRepository = bookingRepository ?? throw new Exception(nameof(bookingRepository));
            _guestDetailsRepository = guestDetailsRepository ?? throw new Exception(nameof(guestDetailsRepository));
            _paymentDetailsRepository = paymentDetailsRepository ?? throw new Exception(nameof(paymentDetailsRepository));
           _userAccountRepository = userAccountRepository ?? throw new Exception(nameof(userAccountRepository));
        }
        #endregion

        #region Methods

        public async Task<BookingResponseModel> CreateNewBookingAsync(BookingRequestModel model)
        {
            BookingResponseModel responseModel = new BookingResponseModel()
            {
                Success = true,
                StatusCode = SystemData.StatusCode.Success
            };

            var existUser = _userAccountRepository.ToQueryable()!.Where(x => x.Id == model.UserAccountId).FirstOrDefault();

            if (existUser == null)
            {
                responseModel.Success = false;
                responseModel.StatusCode = SystemData.StatusCode.LoginFail;
                return responseModel;
            }

            Booking newBooking = new Booking()
            {
                UserAccountId = existUser.Id,
                Adult = model.BookingRequests.Adult,
                Children = model.BookingRequests.Children,
                Person = model.BookingRequests.Person,
                Price = model.BookingRequests.Price,
                Size = model.BookingRequests.Size,
                Src = model.BookingRequests.Src,
                Type = model.BookingRequests.Type,
                Bed = model.BookingRequests.Bed,
                CheckInDate = model.BookingRequests.CheckInDate,
                CheckOutDate = model.BookingRequests.CheckOutDate,
            };
            newBooking = await _bookingRepository.AddAndSaveChangesAsync(newBooking);

            GuestDetails guestDetails = new GuestDetails()
            {
                BookingId = newBooking.Id,
                Address = model.GuestDetailsRequests.Address,
                Email = model.GuestDetailsRequests.Email,
                FirstName = model.GuestDetailsRequests.FirstName,
                LastName = model.GuestDetailsRequests.LastName,
                Phone = model.GuestDetailsRequests.Phone,
            };

            guestDetails = await _guestDetailsRepository.AddAndSaveChangesAsync(guestDetails);

            PaymentDetails paymentDetails = new PaymentDetails()
            {
                BookingId = newBooking.Id,
                CardNumber = model.PaymentDetailsRequests.CardNumber,
                CVV = model.PaymentDetailsRequests.CVV,
                ExpirationDate = model.PaymentDetailsRequests.ExpirationDate,
                NameOnCard = model.PaymentDetailsRequests.NameOnCard,
            };

            paymentDetails = await _paymentDetailsRepository.AddAndSaveChangesAsync(paymentDetails);


            responseModel.GuestEmail = model.GuestDetailsRequests.Email;
            responseModel.BookingId = newBooking.Id;
            responseModel.Adult = model.BookingRequests.Adult;
            responseModel.Children = model.BookingRequests.Children; 
            responseModel.Price = model.BookingRequests.Price;
            responseModel.Type = model.BookingRequests.Type;
            responseModel.CheckInDate = model.BookingRequests.CheckInDate;
            responseModel.CheckOutDate = model.BookingRequests.CheckOutDate;
            responseModel.UserEmail = existUser.Email;



            return responseModel;
        }


        public BookingDisplayListResponseModel GetAllBookingByUser(int id)
        {
            BookingDisplayListResponseModel returnModel = new BookingDisplayListResponseModel()
            {
                Success = true,
                StatusCode = SystemData.StatusCode.Success
            };

            var allUserBookings = from br in _bookingRepository.ToQueryable()
                                  where br.UserAccountId == id
                                  select new BookingResponseModel()
                                  {
                                      Adult = br.Adult,
                                      Children = br.Children,
                                      Price = br.Price,
                                      CheckInDate = br.CheckInDate, 
                                      CheckOutDate = br.CheckOutDate,
                                      BookingId = br.Id,
                                      Type = br.Type,
                                      Src = br.Src,
                                  };

            if(allUserBookings.Count() == 0)
            {
                returnModel.Success = false;
                returnModel.StatusCode = SystemData.StatusCode.NotFound;
            }

            returnModel.BookingList = allUserBookings.ToList();
            return returnModel;
        }


        #endregion
    }
}
