USE master;
GO

IF DB_ID('MICHO_OrderingSystem') IS NOT NULL
BEGIN
    ALTER DATABASE MICHO_OrderingSystem SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE MICHO_OrderingSystem;
END
GO

CREATE DATABASE MICHO_OrderingSystem;
GO

USE MICHO_OrderingSystem;
GO

CREATE TABLE Customer (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    Contact VARCHAR(20) NOT NULL UNIQUE
);
GO

CREATE TABLE Employee (
    EmpID INT PRIMARY KEY IDENTITY(1,1),
    EmpName NVARCHAR(100) NOT NULL,
    IDCard VARCHAR(20) NOT NULL UNIQUE,
    Gender NVARCHAR(10),
    Address NVARCHAR(255) NOT NULL,
    Phone VARCHAR(20) NOT NULL UNIQUE
);
GO

CREATE TABLE IceCream (
    IceCreamID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Price DECIMAL(18, 2) NOT NULL,
    Flavor NVARCHAR(50),
    Color NVARCHAR(50)
);
GO

CREATE TABLE [Order] (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    Status NVARCHAR(50) NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    CustomerID INT NOT NULL,
    EmpID INT,
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    FOREIGN KEY (EmpID) REFERENCES Employee(EmpID)
);
GO

CREATE TABLE OrderDetail (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT NOT NULL,
    Quantity INT NOT NULL,
    TotalAmount DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES [Order](OrderID)
);
GO

CREATE TABLE OrderDetailIceCream (
    OrderDetailIceCreamID INT PRIMARY KEY IDENTITY(1,1),
    OrderDetailID INT NOT NULL,
    OrderID INT NOT NULL,
    IceCreamID INT NOT NULL,
    FOREIGN KEY (OrderDetailID) REFERENCES OrderDetail(OrderDetailID),
    FOREIGN KEY (OrderID) REFERENCES [Order](OrderID),
    FOREIGN KEY (IceCreamID) REFERENCES IceCream(IceCreamID)
);
GO

INSERT INTO Customer (Name, Address, Contact) VALUES
(N'Nguyễn Văn An', N'123 Đường Cầu Giấy, Hà Nội', '0987654321'),
(N'Trần Thị Bình', N'456 Đường Lê Lợi, TP. Hồ Chí Minh', '0123456789'),
(N'Lê Văn Cường', N'789 Đường Ngô Quyền, Đà Nẵng', '0912345678');
GO

INSERT INTO Employee (EmpName, IDCard, Gender, Address, Phone) VALUES
(N'Phạm Thị Dung', '001234567890', N'Nữ', N'10 Phố Hàng Bài, Hà Nội', '0988888888'),
(N'Hoàng Văn Em', '001098765432', N'Nam', N'20 Phố Hàng Tre, Hà Nội', '0977777777');
GO

INSERT INTO IceCream (Name, Price, Flavor, Color) VALUES
(N'Kem Vani Truyền Thống', 25000.00, N'Vani', N'Trắng'),
(N'Kem Socola Đậm Đà', 30000.00, N'Socola', N'Nâu'),
(N'Kem Dâu Tây Mùa Hè', 28000.00, N'Dâu Tây', N'Hồng'),
(N'Kem Matcha Nhật Bản', 35000.00, N'Trà xanh', N'Xanh lá'),
(N'Kem Dừa Bến Tre', 25000.00, N'Dừa', N'Trắng sữa');
GO

INSERT INTO [Order] (Status, OrderDate, CustomerID, EmpID) VALUES (N'Đã hoàn thành', '2025-06-25 09:30:00', 1, 1);
DECLARE @OrderId1 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId1, 2, 50000.00);
DECLARE @OrderDetailId1_1 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId1_1, @OrderId1, 1);
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId1, 1, 30000.00);
DECLARE @OrderDetailId1_2 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId1_2, @OrderId1, 2);
GO

INSERT INTO [Order] (Status, OrderDate, CustomerID, EmpID) VALUES (N'Đang xử lý', '2025-06-25 10:15:00', 2, 2);
DECLARE @OrderId2 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId2, 3, 84000.00);
DECLARE @OrderDetailId2_1 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId2_1, @OrderId2, 3);
GO

INSERT INTO [Order] (Status, OrderDate, CustomerID, EmpID) VALUES (N'Đang xử lý', '2025-06-25 11:30:00', 3, 1);
DECLARE @OrderId3 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId3, 1, 35000.00);
DECLARE @OrderDetailId3_1 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId3_1, @OrderId3, 4);
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId3, 2, 50000.00);
DECLARE @OrderDetailId3_2 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId3_2, @OrderId3, 5);
GO

INSERT INTO Customer (Name, Address, Contact) VALUES
(N'Đinh Thị Thảo', N'33 Tràng Tiền, Hoàn Kiếm, Hà Nội', '0905111222'),
(N'Vũ Minh Tuấn', N'25 Nguyễn Huệ, Quận 1, TP. Hồ Chí Minh', '0933444555');
GO

INSERT INTO [Order] (Status, OrderDate, CustomerID, EmpID) VALUES (N'Đã hoàn thành', '2025-06-20 14:30:00', 4, 2);
DECLARE @OrderId4 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId4, 3, 105000.00);
DECLARE @OrderDetailId4_1 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId4_1, @OrderId4, 4);
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId4, 2, 50000.00);
DECLARE @OrderDetailId4_2 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId4_2, @OrderId4, 5);
GO

INSERT INTO [Order] (Status, OrderDate, CustomerID, EmpID) VALUES (N'Đã hoàn thành', '2025-06-22 20:00:00', 5, 1);
DECLARE @OrderId5 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId5, 5, 150000.00);
DECLARE @OrderDetailId5_1 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId5_1, @OrderId5, 2);
GO

INSERT INTO [Order] (Status, OrderDate, CustomerID, EmpID) VALUES (N'Đã hoàn thành', '2025-06-24 21:15:00', 1, 1);
DECLARE @OrderId6 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId6, 2, 56000.00);
DECLARE @OrderDetailId6_1 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId6_1, @OrderId6, 3);
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId6, 2, 60000.00);
DECLARE @OrderDetailId6_2 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId6_2, @OrderId6, 2);
GO

INSERT INTO [Order] (Status, OrderDate, CustomerID, EmpID) VALUES (N'Đã hủy', '2025-06-25 12:05:00', 3, 2);
DECLARE @OrderId7 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId7, 1, 25000.00);
DECLARE @OrderDetailId7_1 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId7_1, @OrderId7, 1);
GO

INSERT INTO [Order] (Status, OrderDate, CustomerID, EmpID) VALUES (N'Đã hoàn thành', '2025-06-25 14:00:00', 2, 2);
DECLARE @OrderId8 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId8, 1, 35000.00);
DECLARE @OrderDetailId8_1 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId8_1, @OrderId8, 4);
GO

INSERT INTO [Order] (Status, OrderDate, CustomerID, EmpID) VALUES (N'Đã hoàn thành', '2025-06-25 14:10:00', 4, 2);
DECLARE @OrderId9 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId9, 2, 60000.00);
DECLARE @OrderDetailId9_1 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId9_1, @OrderId9, 2);
GO

INSERT INTO [Order] (Status, OrderDate, CustomerID, EmpID) VALUES (N'Đã hoàn thành', '2025-06-25 15:00:00', 5, 1);
DECLARE @OrderId10 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId10, 1, 28000.00);
DECLARE @OrderDetailId10_1 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId10_1, @OrderId10, 3);
GO

INSERT INTO [Order] (Status, OrderDate, CustomerID, EmpID) VALUES (N'Đã hoàn thành', '2025-06-25 19:00:00', 1, 1);
DECLARE @OrderId11 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId11, 4, 120000.00);
DECLARE @OrderDetailId11_1 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId11_1, @OrderId11, 2);
GO

INSERT INTO [Order] (Status, OrderDate, CustomerID, EmpID) VALUES (N'Đã hoàn thành', '2025-06-25 19:30:00', 3, 2);
DECLARE @OrderId12 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId12, 1, 35000.00);
DECLARE @OrderDetailId12_1 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId12_1, @OrderId12, 4);
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId12, 1, 25000.00);
DECLARE @OrderDetailId12_2 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId12_2, @OrderId12, 5);
GO

INSERT INTO [Order] (Status, OrderDate, CustomerID, EmpID) VALUES (N'Đang xử lý', '2025-06-25 20:00:00', 4, 1);
DECLARE @OrderId13 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId13, 2, 50000.00);
DECLARE @OrderDetailId13_1 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId13_1, @OrderId13, 1);
GO

INSERT INTO [Order] (Status, OrderDate, CustomerID, EmpID) VALUES (N'Đã hoàn thành', '2025-06-25 21:00:00', 5, 2);
DECLARE @OrderId14 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetail (OrderID, Quantity, TotalAmount) VALUES (@OrderId14, 3, 90000.00);
DECLARE @OrderDetailId14_1 INT = SCOPE_IDENTITY();
INSERT INTO OrderDetailIceCream(OrderDetailID, OrderID, IceCreamID) VALUES (@OrderDetailId14_1, @OrderId14, 2);
GO

PRINT 'Database MICHO_OrderingSystem and all sample data created successfully.';
GO
