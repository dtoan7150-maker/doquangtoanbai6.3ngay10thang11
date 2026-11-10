using ProjectTaskManagement.GUI;

namespace ProjectTaskManagement
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            
            try
            {
                Application.Run(new FormMain());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Khởi chạy thất bại:\n" + ex.ToString(), "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}