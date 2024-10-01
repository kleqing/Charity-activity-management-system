using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VNPAY_Demo.Helper;
using VNPAY_Demo.Models;
using VNPAY_Demo.Services;

namespace VNPAY_Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVNPayServices VNPayServices;
        //public List<CartItem> Cart => HttpContext..Get<List<CartItem>>(Settings.CART_KEY) ?? new List<CartItem>();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public HomeController(IVNPayServices vnPayServices)
        {
            VNPayServices = vnPayServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        // VNPAY
        [Authorize]
        public IActionResult PaymentCallBack()
        {
            return View();
        }

        //public IActionResult Checkout(CheckoutVM model, string payment = "COD")
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (payment == "Thanh toán VNPay")
        //        {
        //            var vnPayModel = new VNPaymentRequestModel
        //            {
        //                amount = Home.Sum(p => p.ThanhTien),
        //                CreatedDate = DateTime.Now,
        //                Description = $"{model.HoTen} {model.DienThoai}",
        //                FullName = model.HoTen,
        //                OrderId = new Random().Next(1000, 100000)
        //            };
        //            return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
        //        }

        //        var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value;
        //        var khachHang = new KhachHang();
        //        if (model.GiongKhachHang)
        //        {
        //            khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);
        //        }

        //        var hoadon = new HoaDon
        //        {
        //            MaKh = customerId,
        //            HoTen = model.HoTen ?? khachHang.HoTen,
        //            DiaChi = model.DiaChi ?? khachHang.DiaChi,
        //            DienThoai = model.DienThoai ?? khachHang.DienThoai,
        //            NgayDat = DateTime.Now,
        //            CachThanhToan = "COD",
        //            CachVanChuyen = "GRAB",
        //            MaTrangThai = 0,
        //            GhiChu = model.GhiChu
        //        };

        //        db.Database.BeginTransaction();
        //        try
        //        {

        //            db.Add(hoadon);
        //            db.SaveChanges();

        //            var cthds = new List<ChiTietHd>();
        //            foreach (var item in Cart)
        //            {
        //                cthds.Add(new ChiTietHd
        //                {
        //                    MaHd = hoadon.MaHd,
        //                    SoLuong = item.SoLuong,
        //                    DonGia = item.DonGia,
        //                    MaHh = item.MaHh,
        //                    GiamGia = 0
        //                });
        //            }
        //            db.AddRange(cthds);
        //            db.SaveChanges();
        //            db.Database.CommitTransaction();

        //            HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

        //            return View("Success");
        //        }
        //        catch
        //        {
        //            db.Database.RollbackTransaction();
        //        }
        //    }

        //    return View(Cart);
        //}
    }
}
