using LapTrinhWebDatTourDuLich.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebDatTourDuLich.Controllers
{
    public class LoaiVanChuyenController : Controller
    {
        // GET: LoaiVanChuyen
        MyDataDataContext data = new MyDataDataContext();

        //-------------Index-------------------
        [Authorize]
        public ActionResult ListLoaiVanChuyen(string TimKiemLoaiVanChuyen = "")
        {
            if (TimKiemLoaiVanChuyen != "")
            {
                var LoaiVanChuyen = data.Table_LoaiVanChuyens.Include(s => s.MaVanChuyen).Where(m => m.LoaiVanChuyen.ToUpper().Contains(TimKiemLoaiVanChuyen.ToUpper()));
                return View(LoaiVanChuyen);
            }

            else
            {
                var all_loaivanchuyen = from lvc in data.Table_LoaiVanChuyens select lvc;
                return View(all_loaivanchuyen);
            }
          
        }

        //-------------Detail-------------------
        [Authorize]
        public ActionResult Detail(int id)
        {
            var D_loaivanchuyen = data.Table_LoaiVanChuyens.Where(m => m.MaVanChuyen == id).First();
            return View(D_loaivanchuyen);
        }
        //-------------Create-------------------
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult Create(FormCollection collection, Table_LoaiVanChuyen lvc)
        {
            var loaivanchuyen = collection["loaivanchuyen"];
            
            if (string.IsNullOrEmpty(loaivanchuyen))
            {
                ViewData["NhapLoaiVanChuyen"] = "Phải nhập loại vận chuyển!";
            }
            else
            {
                lvc.LoaiVanChuyen = loaivanchuyen;
                data.Table_LoaiVanChuyens.InsertOnSubmit(lvc);
                data.SubmitChanges();
                return RedirectToAction("ListLoaiVanChuyen");
            }
            return this.Create();
        }
        //-------------Edit-------------------
        [Authorize]
        public ActionResult Edit(int id)
        {
            var E_loaivanchuyen = data.Table_LoaiVanChuyens.First(m => m.MaVanChuyen == id);
            return View(E_loaivanchuyen);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var LoaiVanChuyen = data.Table_LoaiVanChuyens.First(m => m.MaVanChuyen == id);
            var E_loaivanchuyen = collection["loaivanchuyen"];
            LoaiVanChuyen.MaVanChuyen = id;
            if (string.IsNullOrEmpty(E_loaivanchuyen))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {

                LoaiVanChuyen.LoaiVanChuyen = E_loaivanchuyen;
                UpdateModel(LoaiVanChuyen);
                data.SubmitChanges();
                return RedirectToAction("ListLoaiVanChuyen");
            }
            return this.Edit(id);
        }
        //-------------Delete-------------------
        [Authorize]
        public ActionResult Delete(int id)
        {
            var D_loaivanchuyen = data.Table_LoaiVanChuyens.First(m => m.MaVanChuyen == id);
            return View(D_loaivanchuyen);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_loaivanchuyen = data.Table_LoaiVanChuyens.Where(m => m.MaVanChuyen == id).First();
            data.Table_LoaiVanChuyens.DeleteOnSubmit(D_loaivanchuyen);
            data.SubmitChanges();
            return RedirectToAction("ListLoaiVanChuyen");
        }
        
    }
}