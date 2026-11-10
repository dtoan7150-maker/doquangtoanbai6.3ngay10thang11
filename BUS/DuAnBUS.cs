using ProjectTaskManagement.DAL;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.BUS
{
    public class DuAnBUS
    {
        private readonly DuAnDAL _dal = new DuAnDAL();
        public List<DuAnDTO> GetAll() => _dal.GetAll();

        public bool Add(DuAnDTO duAn)
        {
            if (string.IsNullOrWhiteSpace(duAn.TenDuAn))
                throw new ArgumentException("Tên dự án không được rỗng!");
            return _dal.Insert(duAn);
        }
    }
}