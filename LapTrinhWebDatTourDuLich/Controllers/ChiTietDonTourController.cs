using LapTrinhWebDatTourDuLich.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebDatTourDuLich.Controllers
{
    public class ChiTietDonTourController : Controller
    {
        // GET: ChiTietDonTour
        MyDataDataContext data = new MyDataDataContext();

        //-------------Index-------------------
        [Authorize]
        public ActionResult ListChiTietDonTour(string TimKiemMaTour = "", string TimKiemTheoNgay = "", string TimKiemTheoThang = "", string TimKiemTheoNam = "")
        {

            if (TimKiemMaTour != "")
            {
                var MaTour = data.Table_ChiTietDonTours.Include(s => s.MaDonTour).Where(m => m.MaTour.ToString().Contains(TimKiemMaTour.ToUpper()));
                return View(MaTour);
            }


            if (TimKiemTheoThang != "")
            {
                var timkiemtheothang = data.Table_ChiTietDonTours.Include(s => s.MaDonTour).Where(m => m.NgayDatTour.ToString().ToUpper().Contains(TimKiemTheoThang.ToUpper()));
                return View(timkiemtheothang);
            }

            if (TimKiemTheoNgay != "")
            {
                var timkiemtheongay = data.Table_ChiTietDonTours.Include(s => s.MaDonTour).Where(m => m.NgayDatTour.ToString().Contains(TimKiemTheoNgay.ToUpper()));
                return View(timkiemtheongay);
            }


            if (TimKiemTheoNam != "")
            {
                var timkiemtheonam = data.Table_ChiTietDonTours.Include(s => s.MaDonTour).Where(m => m.NgayDatTour.ToString().ToUpper().Contains(TimKiemTheoNam.ToUpper()));
                return View(timkiemtheonam);
            }


            else 
            {
                var all_chitietdontour = from ctdt in data.Table_ChiTietDonTours select ctdt;
              
                return View(all_chitietdontour);
            }
       
        }
 
        //-------------Delete-------------------
        [Authorize]
        public ActionResult Delete(int id)
        {
            var D_chitietdontour = data.Table_ChiTietDonTours.First(m => m.MaTour == id);
            return View(D_chitietdontour);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_chitietdontour = data.Table_ChiTietDonTours.Where(m => m.MaTour == id).First();
            data.Table_ChiTietDonTours.DeleteOnSubmit(D_chitietdontour);
            data.SubmitChanges();
            return RedirectToAction("ListChiTietDonTour");
        }

        //-------------Edit-------------------
        [Authorize]
        public ActionResult Edit(int id)
        {
            var E_chitietdontour = data.Table_ChiTietDonTours.First(m => m.MaTour == id);
            return View(E_chitietdontour);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_tour = data.Table_ChiTietDonTours.First(m => m.MaTour == id);
            var E_ghichu = collection["ghichu"];

            E_tour.MaTour = id;
            if (string.IsNullOrEmpty(E_ghichu))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
             
                E_tour.GhiChu = E_ghichu;
              

                UpdateModel(E_tour);
                data.SubmitChanges();
                return RedirectToAction("ListChiTietDonTour");
            }
            return this.Edit(id);
        }


    }
}