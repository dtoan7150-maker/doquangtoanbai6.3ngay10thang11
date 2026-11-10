using ProjectTaskManagement.DAL;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.BUS
{
    public class CongViecBUS
    {
        private readonly CongViecDAL _dal = new CongViecDAL();

        public List<CongViecDTO> GetFiltered(int? maDuAn, string trangThai) => _dal.GetFiltered(maDuAn, trangThai);

        public bool Add(CongViecDTO cv)
        {
            if (string.IsNullOrWhiteSpace(cv.TenTask))
                throw new ArgumentException("Tên công việc không được để trống!");
            return _dal.Insert(cv);
        }

        public bool UpdateTrangThai(int maTask, string trangThaiMoi) => _dal.UpdateTrangThai(maTask, trangThaiMoi);
        public List<CongViecDTO> GetOverdueTasks() => _dal.GetOverdueTasks();
    }
}