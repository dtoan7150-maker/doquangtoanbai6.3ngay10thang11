using ProjectTaskManagement.DAL;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.BUS
{
    public class ThanhVienBUS
    {
        private readonly ThanhVienDAL _dal = new ThanhVienDAL();
        public List<ThanhVienDTO> GetAll() => _dal.GetAll();
    }
}