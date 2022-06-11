using LapTrinhWebDatTourDuLich.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebDatTourDuLich.Controllers
{
    public class ThanhToanController : Controller
    {
        // GET: ThanhToan
        MyDataDataContext data = new MyDataDataContext();

       
        //-------------Index-------------------
        [Authorize]
        public ActionResult ListThanhToan(string TimKiemMaKhachHang = "")
        {
            if (TimKiemMaKhachHang != "")
            {
                var MaKhachHang = data.Table_ThanhToans.Include(s => s.MaThanhToan).Where(m => m.MaKH.ToString().Contains(TimKiemMaKhachHang.ToUpper()));
                return View(MaKhachHang);
            }

            else
            {
                var all_thanhtoan = from tt in data.Table_ThanhToans select tt;
                return View(all_thanhtoan);
            }
       
        }

        //-------------Create-------------------
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult Create(FormCollection collection, Table_ThanhToan tt)
        {
            var C_makh = collection["makh"];
            var C_madontour = collection["madontour"];
            var C_matour = collection["matour"];
            var C_tongsotienphaithanhtoan = String.Format("{0:0,0}", collection["tongsotienphaithanhtoan"]);
            var C_sotiendathanhtoan = String.Format("{0:0,0}", collection["sotiendathanhtoan"]);
            var C_ghichu = collection["ghichu"];


            //Tạo ràng buộc, khai báo các ViewData ở Create.cshtml trong thư mục ThanhToan, sau đó gọi vào hàm if
            if (
                String.IsNullOrEmpty(C_makh) ||
                String.IsNullOrEmpty(C_madontour) ||
                String.IsNullOrEmpty(C_matour) ||
                String.IsNullOrEmpty(C_tongsotienphaithanhtoan) ||
                String.IsNullOrEmpty(C_sotiendathanhtoan) ||
                String.IsNullOrEmpty(C_ghichu))
            {

                ViewData["NhapMaKH"] = "Phải nhập mã khách hàng!";
                ViewData["NhapMaDonTour"] = "Phải nhập mã đơn tour!";
                ViewData["NhapMaTour"] = "Phải nhập mã tour!";
                ViewData["NhapTongSoTienPhaiThanhToan"] = "Phải nhập tổng số tiền phải thanh toán!";
                ViewData["NhapSoTienDaThanhToan"] = "Phải nhập số tiền đã thanh toán !";
                ViewData["NhapGhiChu"] = "Phải nhập ghi chú!";
            }

            /* if (string.IsNullOrEmpty(E_tentour))

             {
                 ViewData["Error"] = "Don't empty!";
             } */

            else
            {
                tt.TongSoTienPhaiThanhToan = Decimal.Parse(C_tongsotienphaithanhtoan);
                tt.SoTienDaThanhToan = Decimal.Parse(C_sotiendathanhtoan);
                tt.GhiChu = C_ghichu.ToString();

                data.Table_ThanhToans.InsertOnSubmit(tt);
                data.SubmitChanges();
                return RedirectToAction("ListThanhToan");
            }
            return this.Create();
        }

        //-------------Detail-------------------
        [Authorize]
        public ActionResult Detail(int id)
        {
            var D_thanhtoan = data.Table_ThanhToans.Where(m => m.MaThanhToan == id).First();
            return View(D_thanhtoan);
        }

        //-------------Edit-------------------
        [Authorize]
        public ActionResult Edit(int id)
        {
            var E_thanhtoan = data.Table_ThanhToans.First(m => m.MaThanhToan == id);
            return View(E_thanhtoan);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_mathanhtoan = data.Table_ThanhToans.First(m => m.MaThanhToan == id);
            var E_tongsotienphaithanhtoan = Convert.ToDecimal(collection["tongsotienphaithanhtoan"]);
            var E_sotiendathanhtoan = Convert.ToDecimal(collection["sotiendathanhtoan"]);
            var E_ghichu = collection["ghichu"];


            E_mathanhtoan.MaThanhToan = id;
            if (string.IsNullOrEmpty(E_ghichu))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_mathanhtoan.TongSoTienPhaiThanhToan = E_tongsotienphaithanhtoan;
                E_mathanhtoan.SoTienDaThanhToan = E_sotiendathanhtoan;
                E_mathanhtoan.GhiChu = E_ghichu;

                UpdateModel(E_mathanhtoan);
                data.SubmitChanges();
                return RedirectToAction("ListThanhToan");
            }
            return this.Edit(id);
        }

        //-------------Delete-------------------
        [Authorize]
        public ActionResult Delete(int id)
        {
            var D_thanhtoan = data.Table_ThanhToans.First(m => m.MaThanhToan == id);
            return View(D_thanhtoan);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_thanhtoan = data.Table_ThanhToans.Where(m => m.MaThanhToan == id).First();
            data.Table_ThanhToans.DeleteOnSubmit(D_thanhtoan);
            data.SubmitChanges();
            return RedirectToAction("ListThanhToan");
        }

 
    }
}