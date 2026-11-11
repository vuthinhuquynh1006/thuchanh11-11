using System;
using System.Collections.Generic;
using DAL;
using DTO;


namespace BUS
{
    public class BUS_SanPham
    {
        private DAL_SanPham dalSP = new DAL_SanPham();

        // Lấy tất cả sản phẩm
        public List<DTO_SanPham> GetAll()
        {
            return dalSP.GetAll();
        }

        // Tìm kiếm sản phẩm
        public List<DTO_SanPham> TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return dalSP.GetAll(); // Trống → trả về tất cả
            return dalSP.TimKiem(tuKhoa);
        }

        // Lấy sản phẩm theo mã
        public DTO_SanPham GetByMaSP(string maSP)
        {
            if (string.IsNullOrWhiteSpace(maSP))
                throw new Exception("Mã sản phẩm không hợp lệ!");
            return dalSP.GetByMaSP(maSP);
        }

        // Thêm sản phẩm
        public bool Them(DTO_SanPham sp)
        {
            // Kiểm tra nghiệp vụ trước khi gọi DAL
            if (string.IsNullOrWhiteSpace(sp.MaSP))
                throw new Exception("Mã sản phẩm không được để trống!");
            if (string.IsNullOrWhiteSpace(sp.TenSP))
                throw new Exception("Tên sản phẩm không được để trống!");
            if (sp.GiaBan <= 0)
                throw new Exception("Giá bán phải lớn hơn 0!");
            if (sp.SoLuongTon < 0)
                throw new Exception("Số lượng tồn không được âm!");

            return dalSP.Them(sp);
        }

        // Sửa sản phẩm
        public bool Sua(DTO_SanPham sp)
        {
            if (string.IsNullOrWhiteSpace(sp.MaSP))
                throw new Exception("Mã sản phẩm không hợp lệ!");
            if (string.IsNullOrWhiteSpace(sp.TenSP))
                throw new Exception("Tên sản phẩm không được để trống!");
            if (sp.GiaBan <= 0)
                throw new Exception("Giá bán phải lớn hơn 0!");
            if (sp.SoLuongTon < 0)
                throw new Exception("Số lượng tồn không được âm!");

            return dalSP.Sua(sp);
        }

        // Xóa sản phẩm
        public bool Xoa(string maSP)
        {
            if (string.IsNullOrWhiteSpace(maSP))
                throw new Exception("Mã sản phẩm không hợp lệ!");
            return dalSP.Xoa(maSP);
        }
    }
}
