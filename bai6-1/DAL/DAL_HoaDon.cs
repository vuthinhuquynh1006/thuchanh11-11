using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class DAL_HoaDon
    {
        public int LapHoaDon(DTO_HoaDon hoaDon)
        {
            int maHD = 0;
            using (SqlConnection conn = DBContext.GetConnection())
            {
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // Bước 1: Thêm hóa đơn
                    string sqlHD = @"INSERT INTO HoaDon(NgayLap, MaNV, MaKH, TongTien) 
                                     VALUES (@NgayLap, @MaNV, @MaKH, @TongTien);
                                     SELECT SCOPE_IDENTITY();";
                    SqlCommand cmdHD = new SqlCommand(sqlHD, conn, trans);
                    cmdHD.Parameters.AddWithValue("@NgayLap", hoaDon.NgayLap);
                    cmdHD.Parameters.AddWithValue("@MaNV", hoaDon.MaNV);
                    cmdHD.Parameters.AddWithValue("@MaKH", hoaDon.MaKH ?? (object)DBNull.Value);
                    cmdHD.Parameters.AddWithValue("@TongTien", hoaDon.TongTien);
                    maHD = Convert.ToInt32(cmdHD.ExecuteScalar());

                    // Bước 2: Thêm chi tiết + Trừ tồn kho
                    DAL_SanPham dalSP = new DAL_SanPham();
                    foreach (var ct in hoaDon.DanhSachChiTiet)
                    {
                        // Kiểm tra tồn kho
                        DTO_SanPham sp = dalSP.GetByMaSP(ct.MaSP);
                        if (sp == null || sp.SoLuongTon < ct.SoLuong)
                            throw new Exception($"Sản phẩm {ct.MaSP} không đủ hàng!");

                        // Thêm chi tiết hóa đơn
                        string sqlCT = "INSERT INTO ChiTietHoaDon(MaHD, MaSP, SoLuong, DonGia) VALUES (@MaHD, @MaSP, @SoLuong, @DonGia)";
                        SqlCommand cmdCT = new SqlCommand(sqlCT, conn, trans);
                        cmdCT.Parameters.AddWithValue("@MaHD", maHD);
                        cmdCT.Parameters.AddWithValue("@MaSP", ct.MaSP);
                        cmdCT.Parameters.AddWithValue("@SoLuong", ct.SoLuong);
                        cmdCT.Parameters.AddWithValue("@DonGia", ct.DonGia);
                        cmdCT.ExecuteNonQuery();

                        // Trừ tồn kho
                        if (!dalSP.CapNhatTonKho(ct.MaSP, ct.SoLuong, trans))
                            throw new Exception($"Không thể cập nhật kho {ct.MaSP}!");
                    }

                    // Bước 3: Cộng điểm tích lũy
                    if (hoaDon.MaKH.HasValue)
                    {
                        int diem = (int)(hoaDon.TongTien / 100000);
                        string sqlDiem = "UPDATE KhachHang SET DiemTichLuy += @Diem WHERE MaKH=@MaKH";
                        SqlCommand cmdDiem = new SqlCommand(sqlDiem, conn, trans);
                        cmdDiem.Parameters.AddWithValue("@Diem", diem);
                        cmdDiem.Parameters.AddWithValue("@MaKH", hoaDon.MaKH.Value);
                        cmdDiem.ExecuteNonQuery();
                    }

                    trans.Commit();
                    return maHD;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Lỗi lập hóa đơn: " + ex.Message);
                }
            }
        }

        public decimal TinhDoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            using (SqlConnection conn = DBContext.GetConnection())
            {
                string sql = "SELECT ISNULL(SUM(TongTien),0) FROM HoaDon WHERE NgayLap BETWEEN @TuNgay AND @DenNgay";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@DenNgay", denNgay.AddDays(1));
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }

        public DataTable GetTop5BanChay()
        {
            using (SqlConnection conn = DBContext.GetConnection())
            {
                string sql = @"SELECT TOP 5 sp.MaSP, sp.TenSP, 
                                      SUM(ct.SoLuong) AS TongSoLuong, 
                                      SUM(ct.SoLuong*ct.DonGia) AS TongTien
                               FROM SanPham sp
                               JOIN ChiTietHoaDon ct ON sp.MaSP = ct.MaSP
                               GROUP BY sp.MaSP, sp.TenSP
                               ORDER BY TongSoLuong DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }
    }
}
