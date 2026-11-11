// === DAL_NhanVien.cs ===
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class DAL_NhanVien
    {
        public List<DTO_NhanVien> GetAll()
        {
            List<DTO_NhanVien> list = new List<DTO_NhanVien>();
            using (SqlConnection conn = DBContext.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM NhanVien", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new DTO_NhanVien
                    {
                        MaNV = Convert.ToInt32(reader["MaNV"]),
                        TenNV = reader["TenNV"].ToString(),
                        SoDienThoai = reader["SoDienThoai"].ToString(),
                        ChucVu = reader["ChucVu"].ToString()
                    });
                }
            }
            return list;
        }
    }
}
