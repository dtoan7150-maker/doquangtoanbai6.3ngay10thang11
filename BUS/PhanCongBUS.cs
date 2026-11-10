using ProjectTaskManagement.DAL;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.BUS
{
    public class PhanCongBUS
    {
        private readonly PhanCongDAL _dal = new PhanCongDAL();
        public bool PhanCong(int maTask, int maThanhVien) => _dal.PhanCongTask(maTask, maThanhVien);
        public List<ThongKeThanhVienDTO> GetThongKeHoanThanh(int maDuAn) => _dal.GetThongKeHoanThanh(maDuAn);
    }
}