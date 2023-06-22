using FrameWork.Infrastructure.ConfigurationModel;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace FrameWork.Application.ZarinPal
{
    public class ZarinPalFactory : IZarinPalFactory
    {
        private readonly AppSettingsOption.Domainsettings _appSettings;

        public string Prefix { get; set; }
        private string MerchantId { get;}

        public ZarinPalFactory(AppSettingsOption.Domainsettings appSettings)
        {
            _appSettings = appSettings;
            Prefix = appSettings.Payment.method;
            MerchantId= appSettings.Payment.merchant;
        }

        public PaymentResponse CreatePaymentRequest(string amount, string mobile, string email, string description,
             long orderId)
        {
            amount = amount.Replace(",", "");
            var finalAmount = int.Parse(amount);
            var siteUrl = _appSettings.Payment.siteUrl;

            var client = new RestClient($"https://{Prefix}.zarinpal.com/pg/rest/WebGate/PaymentRequest.json");
            var request = new RestRequest(Method.Post.ToString());
            request.AddHeader("Content-Type", "application/json");
            var body = new PaymentRequest
            {
                Mobile = mobile,
                CallbackURL = $"{siteUrl}/Checkout?handler=CallBack&oId={orderId}",
                Description = description,
                Email = email,
                Amount = finalAmount,
                MerchantID = MerchantId
            };
            request.AddJsonBody(body);
            var response = client.Execute(request);
            var jsonSerializer = new JsonNetSerializer();
            return jsonSerializer.Deserialize<PaymentResponse>(response)!;
        }

        public VerificationResponse CreateVerificationRequest(string authority, string amount)
        {
            var client = new RestClient($"https://{Prefix}.zarinpal.com/pg/rest/WebGate/PaymentVerification.json");
            var request = new RestRequest(Method.Post.ToString());
            request.AddHeader("Content-Type", "application/json");

            amount = amount.Replace(",", "");
            var finalAmount = int.Parse(amount);

            request.AddJsonBody(new VerificationRequest
            {
                Amount = finalAmount,
                MerchantID = MerchantId,
                Authority = authority
            });
            var response = client.Execute(request);
            var jsonSerializer = new JsonNetSerializer();
            return jsonSerializer.Deserialize<VerificationResponse>(response)!;
        }
    }
}