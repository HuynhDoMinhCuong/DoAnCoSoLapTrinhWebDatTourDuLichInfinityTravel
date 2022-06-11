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
        public ActionResult ListChiTietDonTour(string TimKiemMaTour = "")
        {
            if (TimKiemMaTour != "")
            {
                var MaTour = data.Table_ChiTietDonTours.Include(s => s.MaDonTour).Where(m => m.MaTour.ToString().Contains(TimKiemMaTour.ToUpper()));
                return View(MaTour);
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


    }
}