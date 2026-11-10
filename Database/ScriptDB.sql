CREATE DATABASE ProjectTaskDB;
GO

USE ProjectTaskDB;
GO

-- 1. Bảng Dự án
CREATE TABLE DuAn (
    MaDuAn INT IDENTITY(1,1) PRIMARY KEY,
    TenDuAn NVARCHAR(150) NOT NULL,
    NgayBatDau DATETIME,
    NgayKetThuc DATETIME,
    TrangThai NVARCHAR(50) DEFAULT N'Đang chạy'
);

-- 2. Bảng Thành viên
CREATE TABLE ThanhVien (
    MaThanhVien INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    ChuyenMon NVARCHAR(100)
);

-- 3. Bảng Công việc (Task)
CREATE TABLE CongViec (
    MaTask INT IDENTITY(1,1) PRIMARY KEY,
    MaDuAn INT NOT NULL,
    TenTask NVARCHAR(200) NOT NULL,
    MoTa NVARCHAR(MAX),
    HanNop DATETIME,
    DoUuTien NVARCHAR(20) DEFAULT N'Trung bình',
    TrangThai NVARCHAR(50) DEFAULT N'To Do',
    CONSTRAINT FK_CongViec_DuAn FOREIGN KEY (MaDuAn) REFERENCES DuAn(MaDuAn) ON DELETE CASCADE
);

-- 4. Bảng Phân công
CREATE TABLE PhanCong (
    MaTask INT NOT NULL,
    MaThanhVien INT NOT NULL,
    NgayGiao DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (MaTask, MaThanhVien),
    CONSTRAINT FK_PhanCong_CongViec FOREIGN KEY (MaTask) REFERENCES CongViec(MaTask) ON DELETE CASCADE,
    CONSTRAINT FK_PhanCong_ThanhVien FOREIGN KEY (MaThanhVien) REFERENCES ThanhVien(MaThanhVien) ON DELETE CASCADE
);

-- DỮ LIỆU MẪU
INSERT INTO DuAn (TenDuAn, NgayBatDau, NgayKetThuc, TrangThai) VALUES
(N'Xây dựng Website E-Commerce', '2026-10-01', '2026-12-31', N'Đang chạy'),
(N'Ứng dụng Mobile Quản lý Thu Chi', '2026-09-01', '2026-11-15', N'Đang chạy');

INSERT INTO ThanhVien (HoTen, Email, ChuyenMon) VALUES
(N'Nguyễn Văn A', 'nguyenvana@gmail.com', N'Backend C#'),
(N'Trần Thị B', 'tranthib@gmail.com', N'Frontend React'),
(N'Lê Văn C', 'levanc@gmail.com', N'Database SQL');

INSERT INTO CongViec (MaDuAn, TenTask, MoTa, HanNop, DoUuTien, TrangThai) VALUES
(1, N'Thiết kế CSDL SQL Server', N'Tạo các bảng và quan hệ', '2026-10-05', N'Cao', N'Done'),
(1, N'Lập trình API Đăng nhập', N'Viết API Auth JWT', '2026-11-05', N'Cao', N'In Progress'),
(1, N'Thiết kế Giao diện Homepage', N'UI/UX Responsive', '2026-11-01', N'Trung bình', N'To Do'),
(2, N'Tích hợp Cổng thanh toán', N'Kết nối Momo/VNPay', '2026-10-20', N'Cao', N'To Do');

INSERT INTO PhanCong (MaTask, MaThanhVien) VALUES
(1, 3), (2, 1), (3, 2), (4, 1);