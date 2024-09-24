using Dynamics.Helps;
using Dynamics.Models.Payment;

namespace Dynamics.Services
{
    public class VNPayServices : IVNPayServices
    {
        public readonly IConfiguration _configuration; // Map voi cai appsetting.json (VNPAY)
        public VNPayServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreatePaymentURL(HttpContext context, VNPaymentRequestModel model)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new Library();
            pay.AddRequestData("vnp_Version", _configuration["VNPay:version"]);
            pay.AddRequestData("vnp_Command", _configuration["VNPay:command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["VNPay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", (model.amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần(khử phần thập phân), sau đó gửi sang VNPAY là: 10000000

            pay.AddRequestData("vnp_CreateDate", model.createdDate.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["VNPay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["VNPay:Locale"]);

            pay.AddRequestData("vnp_OrderInfo", "Thanh toan cho don hang:" + model.orderID);
            pay.AddRequestData("vnp_OrderType", "other"); //default value: other
            pay.AddRequestData("vnp_ReturnUrl", _configuration["VNPay:ReturnUrl"]);

            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = pay.CreateRequestUrl(_configuration["VNPay:vnp_Url"], _configuration["VNPay:HashSecret"]);

            return paymentUrl;
        }

        public VNPAY_Model PaymentExcute(IQueryCollection collections)
        {
            var vnpay = new Library();
            // Check cai key o tren, neu no khong null/empty va bat dau bang "vnp_" thi add
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_orderID = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_transactionNo = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_secureHash = collections.FirstOrDefault(x => x.Key == "vnp_SecureHash").Value; // Lay ra (.value)
            var vnpayResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_secureHash, _configuration["VNPay:HashSecret"]);
            if (!checkSignature)
            {
                return new VNPAY_Model
                {
                    Success = false
                };
            }
            return new VNPAY_Model
            {
                Success = true,
                PaymentMethod = "VNPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_orderID.ToString(),
                TransactionId = vnp_transactionNo.ToString(),
                Token = vnp_secureHash,
                VnPayResponseCode = vnpayResponseCode.ToString()
            };
        }
    }
}

