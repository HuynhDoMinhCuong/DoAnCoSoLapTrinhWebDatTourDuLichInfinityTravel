using LapTrinhWebDatTourDuLich.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LapTrinhWebDatTourDuLich.Controllers
{
    public class DonTourController : Controller
    {
        // GET: DonTour
        /*public ActionResult Index()
        {
            return View();
        }*/

        MyDataDataContext data = new MyDataDataContext();
        public List<DonTour> Laydontour()
        {
            List<DonTour> listDontour = Session["DonTour"] as List<DonTour>;
            if (listDontour == null)
            {
                listDontour = new List<DonTour>();
                Session["DonTour"] = listDontour;
            }
            return listDontour;
        }

        public ActionResult ThemDonTour(int id, string strURL)
        {
            List<DonTour> listDontour = Laydontour();
            DonTour sanpham = listDontour.Find(n => n.MaTour == id);
            if (sanpham == null)
            {
                sanpham = new DonTour(id);
                listDontour.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoVe++;
                return Redirect(strURL);
            }
        }

        public int TongSoVe()
        {
            int tsl = 0;
            List<DonTour> listDontour = Session["DonTour"] as List<DonTour>;
            if (listDontour != null)
            {
                tsl = listDontour.Sum(n => n.iSoVe);
            }
            return tsl;
        }

        private int TongSoLuongTour()
        {
            int tsl = 0;
            List<DonTour> listDontour = Session["DonTour"] as List<DonTour>;
            if (listDontour != null)
            {
                tsl = listDontour.Count;
            }
            return tsl;
        }

        private double TongTien()
        {
            double tt = 0;
            List<DonTour> listDontour = Session["DonTour"] as List<DonTour>;
            if (listDontour != null)
            {
                tt = listDontour.Sum(n => n.dThanhtien);
            }
            return tt;
        }

        public ActionResult DonTour()
        {
            List<DonTour> listDontour = Laydontour();
            ViewBag.Tongsove = TongSoVe();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongtour = TongSoLuongTour();
            return View(listDontour);
        }

        public ActionResult DonTourPartial()
        {
            ViewBag.Tongsove = TongSoVe();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongtour = TongSoLuongTour();
            return PartialView();
        }

        public ActionResult XoaDonTour(int id)
        {
            List<DonTour> listDontour = Laydontour();
            DonTour sanpham = listDontour.SingleOrDefault(n => n.MaTour == id);
            if (sanpham != null)
            {
                listDontour.RemoveAll(n => n.MaTour == id);
                return RedirectToAction("DonTour");
            }
            return RedirectToAction("DonTour");
        }

        public ActionResult CapNhatDonTour(int id, FormCollection collection)
        {
            List<DonTour> listDontour = Laydontour();
            DonTour sanpham = listDontour.SingleOrDefault(n => n.MaTour == id);
            if (sanpham != null)
            {
                sanpham.iSoVe = int.Parse(collection["txtSoVe"].ToString());
            }
            return RedirectToAction("DonTour");
        }
        public ActionResult XoaTatCaDonTour()
        {
            List<DonTour> listDontour = Laydontour();
            listDontour.Clear();
            return RedirectToAction("DonTour");
        }

        [HttpGet]
        public ActionResult DatTour()
        {
            if (Session["TaiKhoanDangNhap"] == null || Session["TaiKhoanDangNhap"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }

            if (Session["DonTour"] == null)
            {
                return RedirectToAction("ListTour", "Tour");
            }
            List<DonTour> listDontour = Laydontour();
            ViewBag.Tongsove = TongSoVe();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongtour = TongSoLuongTour();

            return View(listDontour);
        }

        public ActionResult DatTour(FormCollection collection)
        {
            Table_DonTour dt = new Table_DonTour();
            Table_KhachHang kh = (Table_KhachHang)Session["TaiKhoanDangNhap"];
            Table_Tour s = new Table_Tour();

            List<DonTour> gh = Laydontour();
            var ngaygiao = String.Format("{0:dd/MM/yyyy}", collection["NgayGiao"]);

            dt.MaKH = kh.MaKH;
          //dt.ThanhToan = false;
          //dt.GiaoTour = false;
            dt.NgayDatTour = DateTime.Now;
          //dt.ngaygiao = DateTime.Parse(ngaygiao);


            data.Table_DonTours.InsertOnSubmit(dt);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                Table_ChiTietDonTour ctdh = new Table_ChiTietDonTour();
                ctdh.MaDonTour = dt.MaDonTour;
                ctdh.MaTour = item.MaTour;
                ctdh.SoVe = item.iSoVe;

                ctdh.GiaDonTour = (decimal)item.dThanhtien; // Chú ý giá tổng tiền 

                s = data.Table_Tours.Single(n => n.MaTour == item.MaTour);
                //    s.SoChoConNhan = ctdh.SoChoDaDat;
                data.SubmitChanges();

                data.Table_ChiTietDonTours.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["DonTour"] = null;
            return RedirectToAction("XacnhanDontour", "DonTour");
        }

        //
        public ActionResult XacnhanDontour()
        {
            return View();
        }

        //
        public ActionResult ThanhToanTrucTuyen()
        {
            return View();
        }
    }
}