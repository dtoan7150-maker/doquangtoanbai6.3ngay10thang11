using System.Data.SqlClient;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.DAL
{
    public class ThanhVienDAL
    {
        private readonly DatabaseConnection _db = new DatabaseConnection();

        public List<ThanhVienDTO> GetAll()
        {
            var list = new List<ThanhVienDTO>();
            using (var conn = _db.GetConnection())
            {
                string sql = "SELECT * FROM ThanhVien";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new ThanhVienDTO
                    {
                        MaThanhVien = (int)dr["MaThanhVien"],
                        HoTen = dr["HoTen"].ToString(),
                        Email = dr["Email"].ToString(),
                        ChuyenMon = dr["ChuyenMon"].ToString()
                    });
                }
            }
            return list;
        }
    }
}