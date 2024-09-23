using Dynamics.DataAccess;
using Dynamics.DataAccess.Repository;
using Dynamics.Helps;
using Dynamics.Models;
using Dynamics.Models.Payment;
using Dynamics.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Configuration;
using System.Diagnostics;

namespace Dynamics.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository userRepo;
        private readonly ApplicationDbContext db;
        public readonly IVNPayServices _vnPayServices;

        public HomeController(ApplicationDbContext context, IUserRepository userRepo, IVNPayServices vnPayServices)
        {
            _vnPayServices = vnPayServices;
            db = context;
            this.userRepo = userRepo;
        }

        public List<CartItem> cart => HttpContext.Session.Get<List<CartItem>>(MySettings.CART_KEY) ?? new List<CartItem>();

        public async Task<IActionResult> Index()
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

        public IActionResult Checkout(CheckoutVM model, string payment = "COD")
        {
            if (ModelState.IsValid)
            {
                if (payment == "Thanh toán VNPay")
                {
                    var vnPayModel = new VNPaymentRequestModel
                    {
                        amount = cart.Sum(p => p.ThanhTien),
                        createdDate = DateTime.Now,
                        description = $"{model.HoTen} {model.DienThoai}",
                        fullName = model.HoTen,
                        orderID = new Random().Next(1000, 100000)
                    };
                    return Redirect(_vnPayServices.CreatePaymentURL(HttpContext, vnPayModel));
                }

                var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.CLAIM_CUSTOMERID).Value;
                var khachHang = new KhachHang();
                if (model.GiongKhachHang)
                {
                    khachHang = db.KhachHang.SingleOrDefault(kh => kh.MaKh == customerId);
                }

                var hoadon = new HoaDon
                {
                    MaKh = customerId,
                    HoTen = model.HoTen ?? khachHang.HoTen,
                    DiaChi = model.DiaChi ?? khachHang.DiaChi,
                    DienThoai = model.DienThoai ?? khachHang.DienThoai,
                    NgayDat = DateTime.Now,
                    CachThanhToan = "COD",
                    CachVanChuyen = "GRAB",
                    MaTrangThai = 0,
                    GhiChu = model.GhiChu
                };

                db.Database.BeginTransaction();
                try
                {

                    db.Add(hoadon);
                    db.SaveChanges();

                    var cthds = new List<ChiTietHd>();
                    foreach (var item in Cart)
                    {
                        cthds.Add(new ChiTietHd
                        {
                            MaHd = hoadon.MaHd,
                            SoLuong = item.SoLuong,
                            DonGia = item.DonGia,
                            MaHh = item.MaHh,
                            GiamGia = 0
                        });
                    }
                    db.AddRange(cthds);
                    db.SaveChanges();
                    db.Database.CommitTransaction();

                    HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

                    return View("Success");
                }
                catch
                {
                    db.Database.RollbackTransaction();
                }
            }

            return View(Cart);
        }
    }
}
