using System.Data.SqlClient;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.DAL
{
    public class PhanCongDAL
    {
        private readonly DatabaseConnection _db = new DatabaseConnection();

        public bool PhanCongTask(int maTask, int maThanhVien)
        {
            using (var conn = _db.GetConnection())
            {
                string sql = @"IF NOT EXISTS (SELECT 1 FROM PhanCong WHERE MaTask=@MaTask AND MaThanhVien=@MaTV)
                               INSERT INTO PhanCong (MaTask, MaThanhVien, NgayGiao) VALUES (@MaTask, @MaTV, GETDATE())";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaTask", maTask);
                cmd.Parameters.AddWithValue("@MaTV", maThanhVien);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Thống kê bằng GROUP BY
        public List<ThongKeThanhVienDTO> GetThongKeHoanThanh(int maDuAn)
        {
            var list = new List<ThongKeThanhVienDTO>();
            using (var conn = _db.GetConnection())
            {
                conn.Open();
                string sql = @"SELECT tv.MaThanhVien, tv.HoTen, 
                                      COUNT(c.MaTask) AS TongSoTask,
                                      SUM(CASE WHEN c.TrangThai = N'Done' THEN 1 ELSE 0 END) AS SoTaskHoanThanh
                               FROM ThanhVien tv
                               JOIN PhanCong pc ON tv.MaThanhVien = pc.MaThanhVien
                               JOIN CongViec c ON pc.MaTask = c.MaTask
                               WHERE c.MaDuAn = @MaDuAn
                               GROUP BY tv.MaThanhVien, tv.HoTen";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaDuAn", maDuAn);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int tong = Convert.ToInt32(dr["TongSoTask"]);
                        int hoanThanh = Convert.ToInt32(dr["SoTaskHoanThanh"]);
                        double tyLe = tong > 0 ? Math.Round((double)hoanThanh / tong * 100, 2) : 0;

                        list.Add(new ThongKeThanhVienDTO
                        {
                            MaThanhVien = Convert.ToInt32(dr["MaThanhVien"]),
                            HoTen = dr["HoTen"].ToString(),
                            TongSoTask = tong,
                            SoTaskHoanThanh = hoanThanh,
                            TyLeHoanThanh = tyLe
                        });
                    }
                }
            }
            return list;
        }
    }
}