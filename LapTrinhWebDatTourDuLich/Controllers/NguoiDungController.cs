using LapTrinhWebDatTourDuLich.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebDatTourDuLich.Controllers
{
    public class NguoiDungController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        // GET: NguoiDung

        //-------------Index-------------------

        [Authorize]
        public ActionResult ListNguoiDung(string TimKiemKhachHang = "")
        {
            if (TimKiemKhachHang != "")
            {
                var TenKhachHang = data.Table_KhachHangs.Include(s => s.MaKH).Where(m => m.HoTenKH.ToUpper().Contains(TimKiemKhachHang.ToUpper()));
                return View(TenKhachHang);
            }

            else
            {
                var all_nguoidung = from nd in data.Table_KhachHangs select nd;
                return View(all_nguoidung);
            }
           
        }

        //-------------Detail-------------------

        public ActionResult Detail(int id)
        {
            var D_KhachHang = data.Table_KhachHangs.Where(m => m.MaKH == id).First();
            return View(D_KhachHang);
        }

        //-------------DangKy-------------------
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, Table_KhachHang kh)
        {
           
            var taikhoandangnhap = collection["taikhoandangnhap"];
            var matkhaudangnhap = collection["matkhaudangnhap"];
            var MatKhauXacNhan = collection["MatKhauXacNhan"];
            var hotenkh = collection["hotenkh"];
            var ngaysinh = String.Format("{0:dd/MM/yyyy}", collection["ngaysinh"]);
            var email = collection["email"];
            var diachi = collection["diachi"];
            var sodienthoai = collection["sodienthoai"];


            Table_KhachHang kh02 = data.Table_KhachHangs.SingleOrDefault(n => n.TaiKhoanDangNhap == taikhoandangnhap); //Kiểm tra tài khoản đã nhập, nếu không có tài khoản trong SQL thì đăng ký thành công

            //Tạo ràng buộc, khai báo các ViewData ở DangKy.cshtml trong thư mục Người dùng, sau đó gọi vào hàm if
            if (String.IsNullOrEmpty(taikhoandangnhap) ||
                String.IsNullOrEmpty(matkhaudangnhap) ||
                String.IsNullOrEmpty(MatKhauXacNhan) ||
                String.IsNullOrEmpty(hotenkh) ||
                String.IsNullOrEmpty(ngaysinh) ||
                String.IsNullOrEmpty(email) ||
                String.IsNullOrEmpty(diachi) ||
                String.IsNullOrEmpty(sodienthoai) || 
                sodienthoai.Length < 10)
            {
                ViewData["NhapTaiKhoan"] = "Phải nhập tài khoản đăng nhập!";
                ViewData["NhapMatKhau"] = "Phải nhập mật khẩu đăng nhập!";
                ViewData["NhapMKXN"] = "Phải nhập mật khẩu xác nhận!";
                ViewData["NhapHoTenKH"] = "Phải nhập họ tên!";
                ViewData["NhapNgaySinh"] = "Phải nhập mật ngày sinh!";
                ViewData["NhapEmail"] = "Phải nhập Email!";
                ViewData["NhapDiaChi"] = "Phải nhập địa chỉ!";
                ViewData["NhapSoDienThoai"] = "Phải nhập số điện thoại và phải đủ 10 số!";
            }
            else
            {
                if (!matkhaudangnhap.Equals(MatKhauXacNhan))
                {
                    ViewData["MatKhauGiongNhau"] = "Mật khẩu và mật khẩu xác nhận phải giống nhau";
                }
                else
                {
                   if (kh02 == null) //Nếu không có tài khoản trong SQL thì đăng ký thành công
                    {
                        kh.TaiKhoanDangNhap = taikhoandangnhap;
                        kh.MatKhauDangNhap = matkhaudangnhap;
                        kh.HoTenKH = hotenkh;
                        kh.NgaySinh = DateTime.Parse(ngaysinh);
                        kh.Email = email;
                        kh.DiaChi = diachi;
                        kh.SoDienThoai = sodienthoai;


                        data.Table_KhachHangs.InsertOnSubmit(kh);
                        data.SubmitChanges();

                        return RedirectToAction("ThongBaoDangKyThanhCong", "NguoiDung"); // Trả về trang ThongBaoDangKyThanhCong
                    }
                    else //Ngược lại, có tài khoản trong SQL sẽ đăng ký thất bại
                    {

                        return RedirectToAction("ThongBaoDangKyThatBai", "NguoiDung"); // Trả về trang ThongBaoDangKyThatBai
                    }

                   

                   // return RedirectToAction("DangNhap");
                }
            }
            return this.DangKy();

        }

        //-------------DangNhap-------------------
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var taikhoandangnhap = collection["taikhoandangnhap"];
          
            var matkhaudangnhap = collection["matkhaudangnhap"];

            Table_KhachHang kh = data.Table_KhachHangs.SingleOrDefault(n => n.TaiKhoanDangNhap == taikhoandangnhap && n.MatKhauDangNhap == matkhaudangnhap);

            //Tạo ràng buộc, khai báo các ViewData ở DangNhap.cshtml, sau đó gọi vào hàm này
            if (String.IsNullOrEmpty(taikhoandangnhap) ||
                String.IsNullOrEmpty(matkhaudangnhap))
            {
                ViewData["NhapTaiKhoan"] = "Phải nhập tài khoản đăng nhập!";
                ViewData["NhapMatKhau"] = "Phải nhập mật khẩu đăng nhập!";
          
            }
            else
            {
                if (kh != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";

                    Session["TaiKhoanDangNhap"] = kh;                                  

                  //  Session["TaiKhoanDangNhap02"] = kh.TaiKhoanDangNhap.ToString();
                    Session["HoTenKH"] = kh.HoTenKH.ToString();

                    return RedirectToAction("ThongBaoDangNhapThanhCong", "NguoiDung"); // Trả về trang ThongBaoDangNhapThatBai 
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";

                    return RedirectToAction("ThongBaoDangNhapThatBai", "NguoiDung"); // Trả về trang ThongBaoDangNhapThatBai 
                }
                
            }

            return this.DangNhap();
            //return RedirectToAction("Index", "Home");

        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


        //Khai báo trang thông báo đăng nhập thành công
        public ActionResult ThongBaoDangNhapThanhCong()
        {
            return View();
        }

        //Khai báo trang thông báo đăng nhập thất bại
        public ActionResult ThongBaoDangNhapThatBai()
        {
            return View();
        }

        //Khai báo trang thông báo đăng ký thành công
        public ActionResult ThongBaoDangKyThanhCong()
        {
            return View();
        }

        //Khai báo trang thông báo đăng ký thất bại
        public ActionResult ThongBaoDangKyThatBai()
        {
            return View();
        }

    }
}