using System.Data.SqlClient;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.DAL
{
    public class CongViecDAL
    {
        private readonly DatabaseConnection _db = new DatabaseConnection();

        public List<CongViecDTO> GetFiltered(int? maDuAn, string trangThai)
        {
            var list = new List<CongViecDTO>();
            using (var conn = _db.GetConnection())
            {
                string sql = @"SELECT c.*, d.TenDuAn 
                               FROM CongViec c 
                               JOIN DuAn d ON c.MaDuAn = d.MaDuAn 
                               WHERE (1=1)";

                if (maDuAn.HasValue && maDuAn.Value > 0) sql += " AND c.MaDuAn = @MaDuAn";
                if (!string.IsNullOrEmpty(trangThai) && trangThai != "Tất cả") sql += " AND c.TrangThai = @TrangThai";

                SqlCommand cmd = new SqlCommand(sql, conn);
                if (maDuAn.HasValue && maDuAn.Value > 0) cmd.Parameters.AddWithValue("@MaDuAn", maDuAn.Value);
                if (!string.IsNullOrEmpty(trangThai) && trangThai != "Tất cả") cmd.Parameters.AddWithValue("@TrangThai", trangThai);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new CongViecDTO
                    {
                        MaTask = (int)dr["MaTask"],
                        MaDuAn = (int)dr["MaDuAn"],
                        TenTask = dr["TenTask"].ToString(),
                        MoTa = dr["MoTa"].ToString(),
                        HanNop = Convert.ToDateTime(dr["HanNop"]),
                        DoUuTien = dr["DoUuTien"].ToString(),
                        TrangThai = dr["TrangThai"].ToString(),
                        TenDuAn = dr["TenDuAn"].ToString()
                    });
                }
            }
            return list;
        }

        public bool Insert(CongViecDTO cv)
        {
            using (var conn = _db.GetConnection())
            {
                string sql = @"INSERT INTO CongViec (MaDuAn, TenTask, MoTa, HanNop, DoUuTien, TrangThai) 
                               VALUES (@MaDuAn, @TenTask, @MoTa, @HanNop, @DoUuTien, @TrangThai)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaDuAn", cv.MaDuAn);
                cmd.Parameters.AddWithValue("@TenTask", cv.TenTask);
                cmd.Parameters.AddWithValue("@MoTa", cv.MoTa ?? "");
                cmd.Parameters.AddWithValue("@HanNop", cv.HanNop);
                cmd.Parameters.AddWithValue("@DoUuTien", cv.DoUuTien);
                cmd.Parameters.AddWithValue("@TrangThai", cv.TrangThai);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Cập nhật trạng thái bằng Parameterized SQL Update
        public bool UpdateTrangThai(int maTask, string trangThaiMoi)
        {
            using (var conn = _db.GetConnection())
            {
                string sql = "UPDATE CongViec SET TrangThai = @TrangThai WHERE MaTask = @MaTask";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@TrangThai", trangThaiMoi);
                cmd.Parameters.AddWithValue("@MaTask", maTask);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Đọc danh sách quá hạn bằng SqlDataReader
        public List<CongViecDTO> GetOverdueTasks()
        {
            var list = new List<CongViecDTO>();
            using (var conn = _db.GetConnection())
            {
                string sql = @"SELECT c.*, d.TenDuAn 
                               FROM CongViec c 
                               JOIN DuAn d ON c.MaDuAn = d.MaDuAn 
                               WHERE c.HanNop < GETDATE() AND c.TrangThai != N'Done'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new CongViecDTO
                        {
                            MaTask = (int)dr["MaTask"],
                            MaDuAn = (int)dr["MaDuAn"],
                            TenTask = dr["TenTask"].ToString(),
                            MoTa = dr["MoTa"].ToString(),
                            HanNop = Convert.ToDateTime(dr["HanNop"]),
                            DoUuTien = dr["DoUuTien"].ToString(),
                            TrangThai = dr["TrangThai"].ToString(),
                            TenDuAn = dr["TenDuAn"].ToString()
                        });
                    }
                }
            }
            return list;
        }
    }
}