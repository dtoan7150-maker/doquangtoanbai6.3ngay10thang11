using System.Data.SqlClient;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.DAL
{
    public class DuAnDAL
    {
        private readonly DatabaseConnection _db = new DatabaseConnection();

        public List<DuAnDTO> GetAll()
        {
            var list = new List<DuAnDTO>();
            using (var conn = _db.GetConnection())
            {
                string sql = "SELECT * FROM DuAn";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new DuAnDTO
                    {
                        MaDuAn = (int)dr["MaDuAn"],
                        TenDuAn = dr["TenDuAn"].ToString(),
                        NgayBatDau = Convert.ToDateTime(dr["NgayBatDau"]),
                        NgayKetThuc = Convert.ToDateTime(dr["NgayKetThuc"]),
                        TrangThai = dr["TrangThai"].ToString()
                    });
                }
            }
            return list;
        }

        public bool Insert(DuAnDTO duAn)
        {
            using (var conn = _db.GetConnection())
            {
                string sql = "INSERT INTO DuAn (TenDuAn, NgayBatDau, NgayKetThuc, TrangThai) VALUES (@Ten, @BD, @KT, @TT)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Ten", duAn.TenDuAn);
                cmd.Parameters.AddWithValue("@BD", duAn.NgayBatDau);
                cmd.Parameters.AddWithValue("@KT", duAn.NgayKetThuc);
                cmd.Parameters.AddWithValue("@TT", duAn.TrangThai);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}