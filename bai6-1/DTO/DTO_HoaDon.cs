using System;
using System.Collections.Generic;

namespace DTO
{
    public class DTO_HoaDon
    {
        public int MaHD { get; set; }
        public DateTime NgayLap { get; set; } = DateTime.Now;
        public int MaNV { get; set; }
        public int? MaKH { get; set; }
        public decimal TongTien { get; set; }
        public List<DTO_ChiTietHD> DanhSachChiTiet { get; set; } = new List<DTO_ChiTietHD>();
    }

    public class DTO_ChiTietHD
    {
        public string MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
    }
}
