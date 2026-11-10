using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ProjectTaskManagement.BUS;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.GUI
{
    public class FormMain : Form
    {
        private readonly DuAnBUS _duAnBUS = new DuAnBUS();
        private readonly CongViecBUS _congViecBUS = new CongViecBUS();
        private readonly ThanhVienBUS _thanhVienBUS = new ThanhVienBUS();
        private readonly PhanCongBUS _phanCongBUS = new PhanCongBUS();

        private TabControl tabControl;
        private DataGridView dgvDuAn, dgvCongViec, dgvOverdue, dgvThongKe;
        private ComboBox cbFilterDuAn, cbFilterTrangThai, cbAssignTV, cbThongKeDuAn;
        private TextBox txtTenDuAn, txtTenTask, txtMoTaTask;
        private DateTimePicker dtpBD, dtpKT, dtpHanNop;
        private ComboBox cbTrangThaiDA, cbDoUuTienTask;
        private Button btnAddDA, btnAddTask, btnAssign, btnChangeStatus, btnRefreshOverdue, btnThongKe;

        public FormMain()
        {
            this.Text = "Hệ thống Quản lý Công việc & Tiến độ Dự án";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeComponents();
            this.Load += FormMain_Load;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void InitializeComponents()
        {
            tabControl = new TabControl { Dock = DockStyle.Fill };

            // TAB 1: Dự án & Task
            TabPage tab1 = new TabPage("1. Dự án & Công việc");
            TableLayoutPanel layoutTab1 = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2 };
            layoutTab1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            layoutTab1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            layoutTab1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            layoutTab1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            // GroupBox Quản lý Dự án (Đã thu gọn khoảng cách dòng)
            GroupBox gbDuAn = new GroupBox { Text = "Quản lý Dự án", Dock = DockStyle.Fill };
            txtTenDuAn = new TextBox { Location = new Point(100, 25), Width = 190 };
            dtpBD = new DateTimePicker { Location = new Point(100, 55), Width = 190 };
            dtpKT = new DateTimePicker { Location = new Point(100, 85), Width = 190 };
            cbTrangThaiDA = new ComboBox { Location = new Point(100, 115), Width = 190 };
            cbTrangThaiDA.Items.AddRange(new string[] { "Đang chạy", "Hoàn thành", "Tạm dừng" });
            cbTrangThaiDA.SelectedIndex = 0;
            btnAddDA = new Button { Text = "Thêm Dự án", Location = new Point(100, 148), Width = 120, Height = 28 };
            btnAddDA.Click += BtnAddDA_Click;

            gbDuAn.Controls.Add(new Label { Text = "Tên Dự án:", Location = new Point(12, 28) });
            gbDuAn.Controls.Add(txtTenDuAn);
            gbDuAn.Controls.Add(new Label { Text = "Ngày bắt đầu:", Location = new Point(12, 58) });
            gbDuAn.Controls.Add(dtpBD);
            gbDuAn.Controls.Add(new Label { Text = "Ngày kết thúc:", Location = new Point(12, 88) });
            gbDuAn.Controls.Add(dtpKT);
            gbDuAn.Controls.Add(new Label { Text = "Trạng thái:", Location = new Point(12, 118) });
            gbDuAn.Controls.Add(cbTrangThaiDA);
            gbDuAn.Controls.Add(btnAddDA);

            dgvDuAn = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            // GroupBox Thêm Task
            GroupBox gbTask = new GroupBox { Text = "Thêm Công việc con", Dock = DockStyle.Fill };
            txtTenTask = new TextBox { Location = new Point(100, 25), Width = 190 };
            txtMoTaTask = new TextBox { Location = new Point(100, 55), Width = 190 };
            dtpHanNop = new DateTimePicker { Location = new Point(100, 85), Width = 190 };
            cbDoUuTienTask = new ComboBox { Location = new Point(100, 115), Width = 190 };
            cbDoUuTienTask.Items.AddRange(new string[] { "Cao", "Trung bình", "Thấp" });
            cbDoUuTienTask.SelectedIndex = 1;
            btnAddTask = new Button { Text = "Tạo Task", Location = new Point(100, 148), Width = 120, Height = 28 };
            btnAddTask.Click += BtnAddTask_Click;

            gbTask.Controls.Add(new Label { Text = "Tên Task:", Location = new Point(12, 28) });
            gbTask.Controls.Add(txtTenTask);
            gbTask.Controls.Add(new Label { Text = "Mô tả:", Location = new Point(12, 58) });
            gbTask.Controls.Add(txtMoTaTask);
            gbTask.Controls.Add(new Label { Text = "Hạn nộp:", Location = new Point(12, 88) });
            gbTask.Controls.Add(dtpHanNop);
            gbTask.Controls.Add(new Label { Text = "Độ ưu tiên:", Location = new Point(12, 118) });
            gbTask.Controls.Add(cbDoUuTienTask);
            gbTask.Controls.Add(btnAddTask);

            layoutTab1.Controls.Add(gbDuAn, 0, 0);
            layoutTab1.Controls.Add(dgvDuAn, 1, 0);
            layoutTab1.Controls.Add(gbTask, 0, 1);
            tab1.Controls.Add(layoutTab1);

            // TAB 2: Lọc & Phân công & Trạng thái
            TabPage tab2 = new TabPage("2. Lọc & Phân công & Trạng thái Task");
            Panel topPanel2 = new Panel { Height = 50, Dock = DockStyle.Top };
            cbFilterDuAn = new ComboBox { Location = new Point(75, 12), Width = 180 };
            cbFilterTrangThai = new ComboBox { Location = new Point(360, 12), Width = 140 };
            cbFilterTrangThai.Items.AddRange(new string[] { "Tất cả", "To Do", "In Progress", "Review", "Done" });
            cbFilterTrangThai.SelectedIndex = 0;

            cbFilterDuAn.SelectedIndexChanged += (s, e) => FilterTasks();
            cbFilterTrangThai.SelectedIndexChanged += (s, e) => FilterTasks();

            topPanel2.Controls.Add(new Label { Text = "Dự án:", Location = new Point(15, 15) });
            topPanel2.Controls.Add(cbFilterDuAn);
            topPanel2.Controls.Add(new Label { Text = "Trạng thái:", Location = new Point(275, 15) });
            topPanel2.Controls.Add(cbFilterTrangThai);

            dgvCongViec = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            Panel botPanel2 = new Panel { Height = 55, Dock = DockStyle.Bottom };
            cbAssignTV = new ComboBox { Location = new Point(90, 15), Width = 180 };
            btnAssign = new Button { Text = "Giao việc cho TV", Location = new Point(280, 13), Width = 140, Height = 28 };
            btnAssign.Click += BtnAssign_Click;

            btnChangeStatus = new Button { Text = "Đổi Trạng Thái Task Chọn", Location = new Point(500, 13), Width = 180, Height = 28 };
            btnChangeStatus.Click += BtnChangeStatus_Click;

            botPanel2.Controls.Add(new Label { Text = "Thành viên:", Location = new Point(12, 18) });
            botPanel2.Controls.Add(cbAssignTV);
            botPanel2.Controls.Add(btnAssign);
            botPanel2.Controls.Add(btnChangeStatus);

            tab2.Controls.Add(dgvCongViec);
            tab2.Controls.Add(topPanel2);
            tab2.Controls.Add(botPanel2);

            // TAB 3: Overdue
            TabPage tab3 = new TabPage("3. Cảnh báo Trễ hạn (Overdue)");
            Panel topPanel3 = new Panel { Height = 50, Dock = DockStyle.Top };
            btnRefreshOverdue = new Button { Text = "Tải danh sách trễ hạn (SqlDataReader)", Location = new Point(15, 10), Width = 280, Height = 30 };
            btnRefreshOverdue.Click += (s, e) => LoadOverdueTasks();
            topPanel3.Controls.Add(btnRefreshOverdue);
            dgvOverdue = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            tab3.Controls.Add(dgvOverdue);
            tab3.Controls.Add(topPanel3);

            // TAB 4: Thống kê
            TabPage tab4 = new TabPage("4. Thống kê % Hoàn thành");
            Panel topPanel4 = new Panel { Height = 50, Dock = DockStyle.Top };
            cbThongKeDuAn = new ComboBox { Location = new Point(100, 12), Width = 200 };
            btnThongKe = new Button { Text = "Xem Thống Kê (GROUP BY)", Location = new Point(320, 10), Width = 200, Height = 30 };
            btnThongKe.Click += BtnThongKe_Click;
            topPanel4.Controls.Add(new Label { Text = "Chọn Dự án:", Location = new Point(15, 15) });
            topPanel4.Controls.Add(cbThongKeDuAn);
            topPanel4.Controls.Add(btnThongKe);
            dgvThongKe = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            tab4.Controls.Add(dgvThongKe);
            tab4.Controls.Add(topPanel4);

            tabControl.TabPages.Add(tab1);
            tabControl.TabPages.Add(tab2);
            tabControl.TabPages.Add(tab3);
            tabControl.TabPages.Add(tab4);

            this.Controls.Add(tabControl);
        }

        private void LoadData()
        {
            try
            {
                var listDuAn = _duAnBUS.GetAll();
                dgvDuAn.DataSource = listDuAn;

                cbFilterDuAn.DataSource = new List<DuAnDTO>(listDuAn);
                cbFilterDuAn.DisplayMember = "TenDuAn";
                cbFilterDuAn.ValueMember = "MaDuAn";

                cbThongKeDuAn.DataSource = new List<DuAnDTO>(listDuAn);
                cbThongKeDuAn.DisplayMember = "TenDuAn";
                cbThongKeDuAn.ValueMember = "MaDuAn";

                cbAssignTV.DataSource = _thanhVienBUS.GetAll();
                cbAssignTV.DisplayMember = "HoTen";
                cbAssignTV.ValueMember = "MaThanhVien";

                FilterTasks();
                LoadOverdueTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi CSDL SQL Server:\n" + ex.Message, "Thông Báo Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddDA_Click(object sender, EventArgs e)
        {
            try
            {
                var duAn = new DuAnDTO
                {
                    TenDuAn = txtTenDuAn.Text,
                    NgayBatDau = dtpBD.Value,
                    NgayKetThuc = dtpKT.Value,
                    TrangThai = cbTrangThaiDA.SelectedItem?.ToString() ?? "Đang chạy"
                };
                if (_duAnBUS.Add(duAn))
                {
                    MessageBox.Show("Thêm dự án thành công!");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAddTask_Click(object sender, EventArgs e)
        {
            if (dgvDuAn.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một dự án ở bảng bên trên!");
                return;
            }
            try
            {
                int maDuAn = (int)dgvDuAn.CurrentRow.Cells["MaDuAn"].Value;
                var task = new CongViecDTO
                {
                    MaDuAn = maDuAn,
                    TenTask = txtTenTask.Text,
                    MoTa = txtMoTaTask.Text,
                    HanNop = dtpHanNop.Value,
                    DoUuTien = cbDoUuTienTask.SelectedItem?.ToString() ?? "Trung bình",
                    TrangThai = "To Do"
                };
                if (_congViecBUS.Add(task))
                {
                    MessageBox.Show("Thêm Task thành công!");
                    FilterTasks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FilterTasks()
        {
            int? maDuAn = cbFilterDuAn.SelectedValue is int id ? id : null;
            string trangThai = cbFilterTrangThai.SelectedItem?.ToString();
            dgvCongViec.DataSource = _congViecBUS.GetFiltered(maDuAn, trangThai);
        }

        private void BtnAssign_Click(object sender, EventArgs e)
        {
            if (dgvCongViec.CurrentRow == null || cbAssignTV.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn công việc và thành viên!");
                return;
            }
            int maTask = (int)dgvCongViec.CurrentRow.Cells["MaTask"].Value;
            int maTV = (int)cbAssignTV.SelectedValue;

            if (_phanCongBUS.PhanCong(maTask, maTV))
            {
                MessageBox.Show("Phân công thành công!");
            }
            else
            {
                MessageBox.Show("Thành viên này đã được phân công Task này từ trước!");
            }
        }

        private void BtnChangeStatus_Click(object sender, EventArgs e)
        {
            if (dgvCongViec.CurrentRow == null) return;
            int maTask = (int)dgvCongViec.CurrentRow.Cells["MaTask"].Value;
            string currentStatus = dgvCongViec.CurrentRow.Cells["TrangThai"].Value?.ToString() ?? "To Do";

            string nextStatus = currentStatus switch
            {
                "To Do" => "In Progress",
                "In Progress" => "Done",
                "Done" => "To Do",
                _ => "To Do"
            };

            if (_congViecBUS.UpdateTrangThai(maTask, nextStatus))
            {
                MessageBox.Show($"Đã chuyển trạng thái Task #{maTask} sang [{nextStatus}]!");
                FilterTasks();
            }
        }

        private void LoadOverdueTasks()
        {
            dgvOverdue.DataSource = _congViecBUS.GetOverdueTasks();
        }

        private void BtnThongKe_Click(object sender, EventArgs e)
        {
            if (cbThongKeDuAn.SelectedValue is int maDuAn)
            {
                dgvThongKe.DataSource = _phanCongBUS.GetThongKeHoanThanh(maDuAn);
            }
        }
    }
}