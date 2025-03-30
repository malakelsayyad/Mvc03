using Company.G02.DAL.Models;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Company.G02.PL.Helpers
{
    public class SMSService : ISMSService
    {
        private TwilioSettings _options;

        public SMSService(IOptions<TwilioSettings> options)
        {
            _options = options.Value;
        }
        public MessageResource SendSMS(SMSMessage sms)
        {
            TwilioClient.Init(_options.AccountSID, _options.AuthToken);

            var result = MessageResource.Create(
                body: sms.Body,
                from: new Twilio.Types.PhoneNumber(_options.TwilioPhoneNumber),
                to: sms.PhoneNumber
                );
            return result;
        }
    }
}