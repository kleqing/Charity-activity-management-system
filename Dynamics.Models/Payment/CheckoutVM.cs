using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Models.Payment
{
    public class CheckoutVM
    {
        public bool GiongKhachHang { get; set; }
        public string? HoTen { get; set; }
        public string? DiaChi { get; set; }
        public string? DienThoai { get; set; }
        public string? GhiChu { get; set; }
    }
}
