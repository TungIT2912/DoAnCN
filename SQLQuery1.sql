use master

drop database QLNhaKhoa

create database QLNhaKhoa
go

use QLNhaKhoa
go

if exists (select * from sys.objects where name ='ThiTruong')
	drop table ThiTruong
go
create table ThiTruong
(
	IDSanPham char(4) not null,
	TenSanPham nvarchar(255) not null,
	Loai nvarchar(50) not null,
	DonViTinh nvarchar(255) not null,
	DonGia money not null,
	primary key (IDSanPham)
)
insert into ThiTruong
values('SP01',N'Bông gòn','VTYT',N'túi',50000.00),
	  ('SP02',N'Thuốc tê local ',N'Thuốc',N'thùng',50000.00),
	  ('SP03',N'Ống hút bọt','VTYT',N'cái',50000.00),
	  ('SP04',N'Gương nha khoa','VTYT',N'cái',50000.00),
	  ('SP05',N'Nước súc miệng',N'Thuốc',N'cái',50000.00),
	  ('SP06',N'Thuốc chống đau và chống viêm ',N'Thuốc',N'thùng',50000.00),
	  ('SP07',N'Kềm nhổ răng','VTYT',N'cái',50000.00),
	  ('SP08',N'Bút trám răng','VTYT',N'cái',50000.00),
	  ('SP09',N'Răng kim loại','VTYT',N'cái',50000.00),
	  ('SP10',N'Răng Titan','VTYT',N'cái',50000.00),
	  ('SP11',N'Răng sứ','VTYT',N'cái',50000.00),
	  ('SP12',N'Mắc cài kim loại','VTYT',N'cái',50000.00),
	  ('SP13',N'Mắc cài sứ','VTYT',N'cái',50000.00),
	  ('SP14',N'Thuốc trị sâu răng',N'Thuốc',N'thùng',50000.00),
	  ('SP15',N'Thuốc trị sưng nướu',N'Thuốc',N'thùng',50000.00),
	  ('SP16',N'Kháng sinh',N'Thuốc',N'cái',50000.00),
	  ('SP17',N'thuốc giảm căng thẳng',N'Thuốc',N'thùng',50000.00),
	  ('SP18',N'Thuốc tạo men răng',N'Thuốc',N'thùng',50000.00),
	  ('SP19',N'Thuốc kháng dị ứng',N'Thuốc',N'thùng',50000.00),
	  ('SP20',N'Thuốc kháng vi khuẩn miệng',N'Thuốc',N'thùng',50000.00);
	
	
if exists (select * from sys.objects where name ='Kho')
	drop table Kho
go
create table Kho
(
	IDSanPham char(4) not null,
	IDDungCu char(4) not null,
	TenDungCu nvarchar(255) not null,
	Loai nvarchar(255) not null,
	DonViTinh nvarchar(255) not null,
	SoLuong int,
	primary key (IDDungCu),
	constraint chk_IDSanPham_Kho foreign key (IDSanPham) references ThiTruong(IDSanPham)	
)
INSERT INTO Kho
VALUES('SP01','DC01',N'Bông gòn','VTYT',N'túi',1000),
	  ('SP02','DC02',N'Thuốc tê local ',N'Thuốc',N'thùng',100),
	  ('SP03','DC03',N'Ống hút bọt','VTYT',N'cái',1000),
	  ('SP04','DC04',N'Gương nha khoa','VTYT',N'cái',50),
	  ('SP05','DC05',N'Nước súc miệng',N'Thuốc',N'cái',100),
	  ('SP06','DC06',N'Thuốc chống đau và chống viêm ',N'Thuốc',N'thùng',100),
	  ('SP07','DC07',N'Kềm nhổ răng','VTYT',N'cái',100),
	  ('SP08','DC08',N'Bút trám răng','VTYT',N'cái',100),
	  ('SP09','DC09',N'Răng kim loại','VTYT',N'cái',100),
	  ('SP10','DC10',N'Răng Titan','VTYT',N'cái',100),
	  ('SP11','DC11',N'Răng sứ','VTYT',N'cái',100),
	  ('SP12','DC12',N'Mắc cài kim loại','VTYT',N'cái',100),
	  ('SP13','DC13',N'Mắc cài sứ','VTYT',N'cái',100),
	  ('SP14','DC14',N'Thuốc trị sâu răng',N'Thuốc',N'thùng',100),
	  ('SP15','DC15',N'Thuốc trị sưng nướu',N'Thuốc',N'thùng',100),
	  ('SP16','DC16',N'Kháng sinh',N'Thuốc',N'cái',100),
	  ('SP17','DC17',N'thuốc giảm căng thẳng',N'Thuốc',N'thùng',100),
	  ('SP18','DC18',N'Thuốc tạo men răng',N'Thuốc',N'thùng',100),
	  ('SP19','DC19',N'Thuốc kháng dị ứng',N'Thuốc',N'thùng',100),
	  ('SP20','DC20',N'Thuốc kháng vi khuẩn miệng',N'Thuốc',N'thùng',100);


if exists (select * from sys.objects where name ='LichSuNhapXuat')
	drop table LichSuNhapXuat
go
create table LichSuNhapXuat
(
	MaLS int identity(1,1) not null,
	NoiDung nvarchar(6) not null,
	IDDungCu char(4) not null,
	TenDungCu nvarchar(255) not null,
	Loai nvarchar(255) not null,
	DonViTinh nvarchar(255) not null,
	SoLuongNhapXuat int,
	Don money not null,
	ThanhTien money not null,
	NgayNhap datetime not null,
	primary key (MaLS),
	constraint chk_IDDungCu_NK foreign key (IDDungCu) references Kho(IDDungCu)
)
INSERT INTO LichSuNhapXuat
VALUES('Nhập','DC01',N'Bông gòn','VTYT',N'túi',1000,50000.00,50000000.00,'10/17/2023'),
	  ('Nhập','DC02',N'Thuốc tê local ',N'Thuốc',N'thùng',100,50000.00,5000000.00,'10/17/2023'),
	  ('Nhập','DC03',N'Ống hút bọt','VTYT',N'cái',1000,50000.00,50000000.00,'10/17/2023'),
	  ('Nhập','DC04',N'Gương nha khoa','VTYT',N'cái',50,50000.00,2500000.00,'10/17/2023'),
	  ('Nhập','DC05',N'Nước súc miệng',N'Thuốc',N'cái',100,50000.00,5000000.00,'10/17/2023'),
	  ('Nhập','DC06',N'Thuốc chống đau và chống viêm ',N'Thuốc',N'thùng',100,50000.00,5000000.00,'10/17/2023'),
	  ('Nhập','DC07',N'Kềm nhổ răng','VTYT',N'cái',100,50000.00,5000000.00,'10/17/2023'),
	  ('Nhập','DC08',N'Bút trám răng','VTYT',N'cái',100,50000.00,5000000.00,'10/17/2023'),
	  ('Nhập','DC09',N'Răng kim loại','VTYT',N'cái',100,50000.00,5000000.00,'10/17/2023'),
	  ('Nhập','DC10',N'Răng Titan','VTYT',N'cái',100,50000.00,5000000.00,'10/17/2023'),
	  ('Nhập','DC11',N'Răng sứ','VTYT',N'cái',100,50000.00,5000000.00,'10/17/2023'),
	  ('Nhập','DC12',N'Mắc cài kim loại','VTYT',N'cái',100,50000.00,5000000.00,'10/17/2023'),
	  ('Nhập','DC13',N'Mắc cài sứ','VTYT',N'cái',100,50000.00,5000000.00,'10/17/2023'),
	  ('Nhập','DC14',N'Thuốc trị sâu răng',N'Thuốc',N'thùng',100,50000.00,5000000.00,'10/17/2023'),
	  ('Nhập','DC15',N'Thuốc trị sưng nướu',N'Thuốc',N'thùng',100,50000.00,5000000.00,'10/17/2023'),
	  ('Nhập','DC16',N'Kháng sinh',N'Thuốc',N'cái',100,50000.00,5000000.00,'10/17/2023'),
	  ('Nhập','DC17',N'thuốc giảm căng thẳng',N'Thuốc',N'thùng',100,50000.00,5000000.00,'10/17/2023'),
	  ('Nhập','DC18',N'Thuốc tạo men răng',N'Thuốc',N'thùng',100,50000.00,5000000.00,'10/17/2023'),
	  ('Nhập','DC19',N'Thuốc kháng dị ứng',N'Thuốc',N'thùng',100,50000.00,5000000.00,'10/17/2023'),
	  ('Nhập','DC20',N'Thuốc kháng vi khuẩn miệng',N'Thuốc',N'thùng',100,50000.00,5000000.00,'10/17/2023');

if exists (select * from sys.objects where name ='TaiKhoan')
	drop table TaiKhoan
go
create table TaiKhoan
(
	TenDangNhap varchar(22) not null,
	MatKhau varchar(50) not null,
	
	primary key (TenDangNhap)
)
if exists (select * from sys.objects where name ='ChucVu')
	drop table ChucVu
go
create table ChucVu
(
	MaCV char(3) not null,
	TenCV varchar(22) not null,
	primary key (MaCV)
	
)
if exists (select * from sys.objects where name ='NhanVien')
	drop table NhanVien
go
create table NhanVien
(
	MaNV int identity(1,1) not null,
	TenDangNhap varchar(22) not null,
	Ten nvarchar(50) not null,
	SDT CHAR (10),
	MaCV char(3) not null,
	KinhNghiem nvarchar(50),
	Hinh nvarchar(MAX),
	primary key (MaNV),
	constraint chk_TenDangNhap foreign key (TenDangNhap) references TaiKhoan(TenDangNhap),
	constraint chk_MaCV foreign key (MaCV) references ChucVu(MaCV)
)
if exists (select * from sys.objects where name ='BenhNhan')
	drop table BenhNhan
go
create table BenhNhan
(
	IDBenhNhan char(3) not null,
	HoTen nvarchar(255) not null,
	Gioi bit,
	NamSinh char(4),
	SDT varchar(10),
	DiaChi nvarchar(255),
	NgayKhamDau datetime,
	primary key (IDBenhNhan)
)

if exists (select * from sys.objects where name ='DanhSachKham')
	drop table DanhSachKham
go
create table DanhSachKham
(
	IDKham char(3) not null,
	IDBenhNhan char(3) not null,
	MaNV int identity(1,1),
	NgayKham datetime not null,
	primary key (IDKham),
	constraint chk_IDBenhNhan_DanhSachKham foreign key (IDBenhNhan) references BenhNhan(IDBenhNhan),
	constraint chk_MaNV_DanhSachKham foreign key (MaNV) references NhanVien(MaNV)
)



if exists (select * from sys.objects where name ='ChanDoan')
	drop table ChanDoan
go
create table ChanDoan
(
	IDChanDoan char(3) not null,
	TenChanDoan nvarchar(255) not null,
	primary key (IDChanDoan)
)
insert into ChanDoan
values
('CAO', N'Cạo vôi răng'),
('TAY', N'Tẩy trắng răng'),
('NHO', N'Nhổ răng'),
('TRM', N'Trám răng'),
('NOI', N'Nội nha (chữa tủy)'),
('RSU', N'Răng sứ'),
('RTL', N'Răng tháo lắp'),
('NIE', N'Niềng răng'),
('IMP', N'Implant')

if exists (select * from sys.objects where name ='DichVu')
	drop table DichVu
go
create table DichVu
(
	IDDichVu char(4) not null,
	IDChanDoan char(3) not null,
	TenDichVu nvarchar(255) not null,
	DonViTinh nvarchar(255) not null,
	DonGia money not null,
	primary key (IDDichVu),
	constraint chk_IDChanDoan_DichVu foreign key (IDChanDoan) references ChanDoan(IDChanDoan)
)
insert into DichVu
values
('CA01', 'CAO', N'Toàn hàm', N'Lần', 300000.00),
('TA01', 'TAY', N'Tại nhà', N'Lần', 300000.00),
('TA02', 'TAY', N'Tại phòng', N'Lần', 400000.00),
('NH01', 'NHO', N'Trước (1 chân)', N'Cái', 50000.00),
('NH02', 'NHO', N'Hàm (nhiều chân)', N'Lần', 200000.00),
('NH03', 'NHO', N'Khó', N'Cái', 50000.00),
('NH04', 'NHO', N'Chân răng', N'Cái', 50000.00),
('NH05', 'NHO', N'Lung lay', N'Cái', 50000.00),
('NH06', 'NHO', N'Sữa', N'Cái', 50000.00),
('NH07', 'NHO', N'Khôn hàm trên', N'Cái', 50000.00),
('NH08', 'NHO', N'Khôn hàm dưới', N'Cái', 50000.00),
('TR01', 'TRM', N'Sâu S1, S2', N'Cái', 50000.00),
('TR02', 'TRM', N'Sâu S3', N'Cái', 50000.00),
('TR03', 'TRM', N'Mòn cổ', N'Cái', 50000.00),
('TR04', 'TRM', N'Kẽ răng thưa', N'Cái', 50000.00),
('TR05', 'TRM', N'Đắp mặt', N'Cái', 50000.00),
('TR06', 'TRM', N'Gắn đá kim cương', N'Cái', 50000.00),
('NO01', 'NOI', N'Răng sữa (trẻ em)', N'Cái', 50000.00),
('NO02', 'NOI', N'Răng cửa', N'Cái', 50000.00),
('NO03', 'NOI', N'Răng tiền cối', N'Cái', 50000.00),
('NO04', 'NOI', N'Răng cối', N'Cái', 50000.00),
('NO05', 'NOI', N'Nội nha lại răng cửa', N'Cái', 50000.00),
('NO06', 'NOI', N'Nội nha lại răng tiền cối', N'Cái', 50000.00),
('NO07', 'NOI', N'Nội nha lại răng cối', N'Cái', 50000.00),
('RS01', 'RSU', N'Kim loại', N'Cái', 50000.00),
('RS02', 'RSU', N'Titan', N'Cái', 50000.00),
('RS03', 'RSU', N'Toàn sứ', N'Cái', 50000.00),
('RT01', 'RTL', N'Nền nhựa cứng bán hàm', N'Cái', 50000.00),
('RT02', 'RTL', N'Nền nhựa cứng toàn hàm', N'Cái', 50000.00),
('RT03', 'RTL', N'Nền nhựa mềm bán hàm', N'Cái', 50000.00),
('RT04', 'RTL', N'Nền nhựa mềm toàn hàm', N'Cái', 50000.00),
('RT05', 'RTL', N'Răng nhựa Việt Nam', N'Cái', 50000.00),
('RT06', 'RTL', N'Răng nhựa ngoại', N'Cái', 50000.00),
('RT07', 'RTL', N'Răng composite', N'Cái', 50000.00),
('RT08', 'RTL', N'Răng sứ', N'Cái', 50000.00),
('RT09', 'RTL', N'Hàm khung kim loại', N'Cái', 50000.00),
('RT10', 'RTL', N'Hàm khung liên kết', N'Cái', 50000.00),
('RT11', 'RTL', N'Hàm khung nhựa cứng', N'Cái', 50000.00),
('RT12', 'RTL', N'Hàm khung nhựa mềm', N'Cái', 50000.00),
('NI01', 'NIE', N'Mắc cài kim loại', N'Lần', 40000.00),
('NI02', 'NIE', N'Mắc cài sứ', N'Lần', 30000.00),
('IM01', 'IMP', N'Hàn quốc', N'Cái', 50000.00),
('IM02', 'IMP', N'Ý', N'Cái', 50000.00),
('IM03', 'IMP', N'Mỹ', N'Cái', 50000.00),
('IM04', 'IMP', N'Khác', N'Cái', 50000.00)

if exists (select * from sys.objects where name ='DieuTri')
	drop table DieuTri
go
create table DieuTri
(	
	IDDieuTri int identity(1,1) not null,
	IDDichVu char(4) not null,
	IDKham char(3) not null,
	IDDungCu char(4) not null,
	SoLuong int not null,
	ThanhTien money not null,
	primary key(IDDieuTri),
	constraint chk_IDDichVu_DieuTri foreign key (IDDichVu) references DichVu(IDDichVu),
	constraint chk_IDKham_DieuTri foreign key (IDKham) references DanhSachKham(IDKham),
	constraint chk_IDDungCu_DieuTri foreign key (IDDungCu) references Kho(IDDungCu)
)

if exists (select * from sys.objects where name ='DonThuoc')
	drop table DonThuoc
go
create table DonThuoc
(
	IDDonThuoc int identity(1,1) not null,
	IDKham char(3) not null,
	TongTien money not null,
	NgayLapDT datetime,
	primary key (IDDonThuoc),
	constraint chk_IDKham_DonThuoc foreign key (IDKham) references DanhSachKham(IDKham)
)

if exists (select * from sys.objects where name ='CTDonThuoc')
	drop table CTDonThuoc
go
create table CTDonThuoc
(
	IDCTDT char(3) not null,
	IDDonThuoc int identity(1,1) not null,
	TenThuoc nvarchar(255) not null,
	SoLuong int not null,
	ThanhGia money not null,
	primary key (IDCTDT),
	constraint chk_IDDonThuoc_CTDonThuoc foreign key (IDDonThuoc) references DonThuoc(IDDonThuoc)
)

if exists (select * from sys.objects where name ='HoaDon')
	drop table HoaDon
go
create table HoaDon
(
	IDHoaDon int identity(1,1) not null,
	IDKham char(3) not null,
	PhuongThucThanhToan nvarchar(255),
	TienThuoc money not null,
	TienDieuTri money not null,
	TongTien money not null,
	NgayLap money not null,
	primary key (IDHoaDon),
	constraint chk_IDKham_HoaDon foreign key (IDKham) references DanhSachKham(IDKham)
)