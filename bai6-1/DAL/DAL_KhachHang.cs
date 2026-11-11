// === DAL_KhachHang.cs ===
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class DAL_KhachHang
    {
        public List<DTO_KhachHang> GetAll()
        {
            List<DTO_KhachHang> list = new List<DTO_KhachHang>();
            using (SqlConnection conn = DBContext.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM KhachHang", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new DTO_KhachHang
                    {
                        MaKH = Convert.ToInt32(reader["MaKH"]),
                        TenKH = reader["TenKH"].ToString(),
                        DienThoai = reader["DienThoai"].ToString(),
                        DiemTichLuy = Convert.ToInt32(reader["DiemTichLuy"])
                    });
                }
            }
            return list;
        }
    }
}
