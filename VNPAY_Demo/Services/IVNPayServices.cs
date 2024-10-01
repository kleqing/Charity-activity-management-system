using VNPAY_Demo.Models;

namespace VNPAY_Demo.Services
{
    public interface IVNPayServices
    {
        string CreatePaymentURL(HttpContext context, VNPaymentRequestModel model);
        VNPAY_Model PaymentExcute(IQueryCollection collections);
    }
}
