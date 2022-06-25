using LapTrinhWebDatTourDuLich.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebDatTourDuLich.Controllers
{
    public class TourController : Controller
    {
  
        MyDataDataContext data = new MyDataDataContext();

        // GET: Tour
        //-------------Index-------------------
        [Authorize]
        public ActionResult ListTour(string TimKiemTour = "")
        {
            if (TimKiemTour != "")
            {
                var TenTour = data.Table_Tours.Include(s => s.MaTour).Where(m => m.TenTour.ToUpper().Contains(TimKiemTour.ToUpper()));
                return View(TenTour);
            }

            else
            {
                var all_tour = from t in data.Table_Tours select t;
                return View(all_tour);
            }
      
        }

        //-------------Detail-------------------
        [Authorize]
        public ActionResult Detail(int id)
        {
            var D_tour = data.Table_Tours.Where(m => m.MaTour == id).First();
            return View(D_tour);
        }

        //-------------Create-------------------
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult Create(FormCollection collection, Table_Tour t)
        {
            var C_mavanchuyen = collection["mavanchuyen"];

            var C_tentour = collection["tentour"];
            var C_noikhoihanh = collection["noikhoihanh"];
         // var C_ngaykhoihanh = Convert.ToDateTime(collection["ngaykhoihanh"]);
            var C_ngaykhoihanh = String.Format("{0:dd/MM/yyyy}", collection["ngaykhoihanh"]);
            var C_thoigian = collection["thoigian"];

            //  var E_giatour = Convert.ToDecimal(collection["giatour"]);
            var C_giatour = String.Format("{0:0,0}", collection["giatour"]);

            //  var E_sochoconnhan = Convert.ToInt32(collection["sochoconnhan"]);
            var C_tournaycogihay = collection["tournaycogihay"];
            var C_hinh = collection["hinh"];

          

            //Tạo ràng buộc, khai báo các ViewData ở Create.cshtml trong thư mục Tour, sau đó gọi vào hàm if
            if (String.IsNullOrEmpty(C_mavanchuyen) ||

                String.IsNullOrEmpty(C_tentour) ||
                String.IsNullOrEmpty(C_noikhoihanh) ||
                String.IsNullOrEmpty(C_ngaykhoihanh) ||
                String.IsNullOrEmpty(C_thoigian) ||
                String.IsNullOrEmpty(C_giatour) ||
                String.IsNullOrEmpty(C_tournaycogihay) ||
                String.IsNullOrEmpty(C_hinh)  )
            {
                ViewData["NhapMaVanChuyen"] = "Phải nhập mã vận chuyển!";

                ViewData["NhapTenTour"] = "Phải nhập tên tour!";
                ViewData["NhapNoiKhoiHanh"] = "Phải nhập nơi khởi hành!";
                ViewData["NhapNgayKhoiHanh"] = "Phải nhập ngày khởi hành!";
                ViewData["NhapThoiGian"] = "Phải nhập thời gian!";
                ViewData["NhapGiaTour"] = "Phải nhập giá tour!";
                ViewData["NhapTourNayCoGiHay"] = "Phải nhập tour này có gì hay!";
                ViewData["NhapHinh"] = "Phải chọn hình!";
            }

            /* if (string.IsNullOrEmpty(E_tentour))

             {
                 ViewData["Error"] = "Don't empty!";
             } */

            else if (t.MaVanChuyen <= 0)
            {
                ViewData["NhapMaVanChuyen"] = "Phải nhập mã vận chuyển lớn hơn 0!";

            }

           
            else
            {
                t.TenTour = C_tentour.ToString();
                t.NoiKhoiHanh = C_noikhoihanh.ToString();
                t.NgayKhoiHanh = DateTime.Parse(C_ngaykhoihanh);
                t.ThoiGian = C_thoigian.ToString();
                t.GiaTour = Decimal.Parse(C_giatour);
          //    t.SoChoConNhan = C_sochoconnhan;
                t.TourNayCoGiHay = C_tournaycogihay;
                t.Hinh = C_hinh;

                data.Table_Tours.InsertOnSubmit(t);
                data.SubmitChanges();
                return RedirectToAction("ListTour");
            }
            return this.Create();
        }

        //-------------Edit-------------------
        [Authorize]
        public ActionResult Edit(int id)
        {
            var E_tour = data.Table_Tours.First(m => m.MaTour == id);
            return View(E_tour);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_tour = data.Table_Tours.First(m => m.MaTour == id);
            var E_tentour = collection["tentour"];
            var E_noikhoihanh = collection["noikhoihanh"];
            var E_ngaykhoihanh = Convert.ToDateTime(collection["ngaykhoihanh"]);
            var E_thoigian = collection["thoigian"];
            var E_giatour = Convert.ToDecimal(collection["giatour"]);
        //  var E_sochoconnhan = Convert.ToInt32(collection["sochoconnhan"]);
            var E_tournaycogihay = collection["tournaycogihay"];
            var E_hinh = collection["hinh"];

            E_tour.MaTour = id;
            if (string.IsNullOrEmpty(E_tentour))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_tour.TenTour = E_tentour;
                E_tour.NoiKhoiHanh = E_noikhoihanh;
                E_tour.NgayKhoiHanh = E_ngaykhoihanh;
                E_tour.ThoiGian = E_thoigian;
                E_tour.GiaTour = E_giatour;
           //   E_tour.SoChoConNhan = E_sochoconnhan;
                E_tour.TourNayCoGiHay = E_tournaycogihay;
                E_tour.Hinh = E_hinh;

                UpdateModel(E_tour);
                data.SubmitChanges();
                return RedirectToAction("ListTour");
            }
            return this.Edit(id);
        }

        //-------------Delete-------------------
        [Authorize]
        public ActionResult Delete(int id)
        {
            var D_tour = data.Table_Tours.First(m => m.MaTour == id);
            return View(D_tour);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_tour = data.Table_Tours.Where(m => m.MaTour == id).First();
            data.Table_Tours.DeleteOnSubmit(D_tour);
            data.SubmitChanges();
            return RedirectToAction("ListTour");
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }
    }
}