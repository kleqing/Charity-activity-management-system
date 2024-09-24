using Dynamics.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Services
{
    public interface IVNPayServices
    {
        string CreatePaymentURL(HttpContext context, VNPaymentRequestModel model);
        VNPAY_Model PaymentExcute(IQueryCollection collections);
    }
}
