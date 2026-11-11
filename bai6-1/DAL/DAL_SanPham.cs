using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class DAL_SanPham
    {
        public List<DTO_SanPham> GetAll()
        {
            List<DTO_SanPham> list = new List<DTO_SanPham>();
            using (SqlConnection conn = DBContext.GetConnection())
            {
                string sql = "SELECT * FROM SanPham ORDER BY MaSP";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new DTO_SanPham
                    {
                        MaSP = reader["MaSP"].ToString(),
                        TenSP = reader["TenSP"].ToString(),
                        GiaBan = Convert.ToDecimal(reader["GiaBan"]),
                        SoLuongTon = Convert.ToInt32(reader["SoLuongTon"])
                    });
                }
            }
            return list;
        }

        public List<DTO_SanPham> TimKiem(string tuKhoa)
        {
            List<DTO_SanPham> list = new List<DTO_SanPham>();
            using (SqlConnection conn = DBContext.GetConnection())
            {
                string sql = "SELECT * FROM SanPham WHERE MaSP LIKE @TuKhoa OR TenSP LIKE @TuKhoa";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoa + "%");
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new DTO_SanPham
                    {
                        MaSP = reader["MaSP"].ToString(),
                        TenSP = reader["TenSP"].ToString(),
                        GiaBan = Convert.ToDecimal(reader["GiaBan"]),
                        SoLuongTon = Convert.ToInt32(reader["SoLuongTon"])
                    });
                }
            }
            return list;
        }

        public DTO_SanPham GetByMaSP(string maSP)
        {
            DTO_SanPham sp = null;
            using (SqlConnection conn = DBContext.GetConnection())
            {
                string sql = "SELECT * FROM SanPham WHERE MaSP = @MaSP";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaSP", maSP);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    sp = new DTO_SanPham
                    {
                        MaSP = reader["MaSP"].ToString(),
                        TenSP = reader["TenSP"].ToString(),
                        GiaBan = Convert.ToDecimal(reader["GiaBan"]),
                        SoLuongTon = Convert.ToInt32(reader["SoLuongTon"])
                    };
                }
            }
            return sp;
        }

        public bool Them(DTO_SanPham sp)
        {
            using (SqlConnection conn = DBContext.GetConnection())
            {
                string sql = "INSERT INTO SanPham(MaSP, TenSP, GiaBan, SoLuongTon) VALUES (@MaSP, @TenSP, @GiaBan, @SoLuongTon)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaSP", sp.MaSP);
                cmd.Parameters.AddWithValue("@TenSP", sp.TenSP);
                cmd.Parameters.AddWithValue("@GiaBan", sp.GiaBan);
                cmd.Parameters.AddWithValue("@SoLuongTon", sp.SoLuongTon);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Sua(DTO_SanPham sp)
        {
            using (SqlConnection conn = DBContext.GetConnection())
            {
                string sql = "UPDATE SanPham SET TenSP=@TenSP, GiaBan=@GiaBan, SoLuongTon=@SoLuongTon WHERE MaSP=@MaSP";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaSP", sp.MaSP);
                cmd.Parameters.AddWithValue("@TenSP", sp.TenSP);
                cmd.Parameters.AddWithValue("@GiaBan", sp.GiaBan);
                cmd.Parameters.AddWithValue("@SoLuongTon", sp.SoLuongTon);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Xoa(string maSP)
        {
            using (SqlConnection conn = DBContext.GetConnection())
            {
                string sql = "DELETE FROM SanPham WHERE MaSP=@MaSP";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaSP", maSP);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool CapNhatTonKho(string maSP, int soLuongBan, SqlTransaction trans)
        {
            string sql = "UPDATE SanPham SET SoLuongTon = SoLuongTon - @SoLuong WHERE MaSP=@MaSP AND SoLuongTon >= @SoLuong";
            SqlCommand cmd = new SqlCommand(sql, trans.Connection, trans);
            cmd.Parameters.AddWithValue("@SoLuong", soLuongBan);
            cmd.Parameters.AddWithValue("@MaSP", maSP);
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
