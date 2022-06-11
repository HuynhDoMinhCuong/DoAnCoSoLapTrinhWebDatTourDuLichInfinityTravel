using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LapTrinhWebDatTourDuLich.Models
{
    public class DonTour
    {
        MyDataDataContext data = new MyDataDataContext();

        [Display(Name = "Mã tour")]
        public int MaTour { get; set; }

        [Display(Name = "Tên tour")]
        public string TenTour { get; set; }

        [Display(Name = "Ảnh tour")]
        public string Hinh { get; set; }

        [Display(Name = "Nơi khởi hành")]
        public string NoiKhoiHanh { get; set; }

        [Display(Name = "Ngày khởi hành")]
        public DateTime NgayKhoiHanh { get; set; }

        [Display(Name = "Thời gian")]
        public string ThoiGian { get; set; }

        [Display(Name = "Giá tour")]
        public Double GiaTour { get; set; }

        [Display(Name = "Số vé")]
        public int iSoVe { get; set; }

        [Display(Name = "Thành tiền")]
        public Double dThanhtien
        {
            get { return iSoVe * GiaTour; }
            set {; } // thêm set gán vào DonTourController.cs để tính tổng tiền của chi tiết đơn tour
        }

        public DonTour(int id)
        {
            MaTour = id;
            Table_Tour tour = data.Table_Tours.Single(n => n.MaTour == MaTour);
            TenTour = tour.TenTour;
            Hinh = tour.Hinh;
            NoiKhoiHanh = tour.NoiKhoiHanh;
            NgayKhoiHanh = DateTime.Parse(tour.NgayKhoiHanh.ToString());
            ThoiGian = tour.ThoiGian;
            GiaTour = double.Parse(tour.GiaTour.ToString());
            iSoVe = 1;
        
        }
    }
}