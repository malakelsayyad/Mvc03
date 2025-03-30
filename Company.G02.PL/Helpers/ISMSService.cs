using Twilio.Rest.Api.V2010.Account;
using Company.G02.DAL.Models;

namespace Company.G02.PL.Helpers
{
    public interface ISMSService
    {
        public MessageResource SendSMS(SMSMessage sms);
    }
}