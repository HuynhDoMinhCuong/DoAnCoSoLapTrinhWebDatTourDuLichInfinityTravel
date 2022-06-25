using LapTrinhWebDatTourDuLich.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebDatTourDuLich.Controllers
{
    public class HomeController : Controller
    {
        //GET: Đặt tour
        MyDataDataContext data = new MyDataDataContext();

        //-------------Index-------------------
        public ActionResult Index(int ? page, string TimKiemTour="")
        {
            if (TimKiemTour !="")
            {
                var TenTour = data.Table_Tours.Include(s => s.MaTour).Where(m => m.TenTour.ToUpper().Contains(TimKiemTour.ToUpper()));
                int pageSize = 3;
                int pageNum = page ?? 1;

                return View(TenTour.ToPagedList(pageNum, pageSize));
            }

               else if (page == null) page = 1;
            {
                var all_tour = (from t in data.Table_Tours select t).OrderBy(m => m.MaTour);
                int pageSize = 3;
                int pageNum = page ?? 1;

                return View(all_tour.ToPagedList(pageNum, pageSize));
            }
        }

        //-------------Detail-------------------
       
        public ActionResult Detail(int id)
        {
            var D_tour = data.Table_Tours.Where(m => m.MaTour == id).First();
            return View(D_tour);
        }

        //
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
       
    }
}