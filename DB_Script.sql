USE master;

IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'PMS_DB')
BEGIN
    ALTER DATABASE [PMS_DB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [PMS_DB];
END

Go
CREATE database PMS_DB

Go
USE PMS_DB

Go
CREATE TABLE PMS_Lookup
(
    Lookup_Id INT PRIMARY KEY IDENTITY(1,1),
    Lookup_Type NVARCHAR(50) not null,
    Hidden_Value NVARCHAR(50) not null,
    Visible_Value NVARCHAR(50) not null,
    Active_Flag BIT not null default(1),
    Created_by NVARCHAR(50) not null, 
    Created_Date DATETIME DEFAULT GETDATE() NOT NULL
);

Go
-- Create PMS_Product table
CREATE TABLE PMS_Product (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    Product_Name NVARCHAR(255) NOT NULL,
    Product_Code NVARCHAR(50) NOT NULL,
    Description NVARCHAR(MAX),
    Price FLOAT NOT NULL,
    Category_lkp_Id INT NOT NULL FOREIGN KEY REFERENCES PMS_Lookup(Lookup_id),
    Uom NVARCHAR(50),
	Created_By NVARCHAR(50) NOT NULL,
	Created_Date DATETIME NOT NULL,
	Last_Updated_By NVARCHAR(50) NOT NULL,
	Last_Updated_Date DATETIME NOT NULL,
	Active_Flag bit NOT NULL default(1)
);


SET IDENTITY_INSERT PMS_Lookup ON;
INSERT INTO PMS_Lookup (Lookup_id, Lookup_Type, Hidden_Value, Visible_Value, Active_Flag, Created_by, Created_Date)
VALUES 
    (1, 'ProductCategory', 'Pharma', 'Pharma', 1, 'Admin', GETDATE()),
    (2, 'ProductCategory', 'Clinical', 'Clinical', 1, 'Admin', GETDATE()),
    (3, 'ProductCategory', 'General', 'General', 1, 'Admin', GETDATE()),
    (4, 'Uom', 'mg', 'mg', 1, 'Admin', GETDATE()),
    (5, 'Uom', 'Unit', 'Unit', 1, 'Admin', GETDATE()),
    (6, 'Uom', 'Pack', 'Pack', 1, 'Admin', GETDATE())
SET IDENTITY_INSERT PMS_Lookup OFF;

Go
INSERT INTO PMS_Product (Product_Name, Product_Code, Description, Price, Category_lkp_id, Uom, Created_By, Created_Date, Last_Updated_By, Last_Updated_Date, Active_Flag)
VALUES 
    ('Aspirin Tablets', 'ASP-001', 'Pain reliever and anti-inflammatory medication', 9.99, 1, 'Box', 'sa', GETDATE(), 'sa', GETDATE(), 1),
    ('Insulin Injection Pen', 'INS-001', 'Medical device for administering insulin', 29.99, 1, 'Unit', 'sa', GETDATE(), 'sa', GETDATE(), 1),
    ('Band-Aid Adhesive Bandages', 'BND-001', 'Sterile adhesive bandages for minor cuts', 4.99, 1, 'Pack', 'sa', GETDATE(), 'sa', GETDATE(), 1),
    ('Thermometer (Digital)', 'THR-001', 'Electronic device for measuring body temperature', 12.99, 2, 'Unit', 'sa', GETDATE(), 'sa', GETDATE(), 1),
    ('ECG Machine (Electrocardiograph)', 'ECG-001', 'Medical equipment for recording heart activity', 499.99, 2, 'Set', 'sa', GETDATE(), 'sa', GETDATE(), 1),
    ('Nebulizer Machine', 'NEB-001', 'Device for converting liquid medication into mist', 79.99, 2, 'Unit', 'sa', GETDATE(), 'sa', GETDATE(), 1),
    ('Blood Pressure Monitor (Digital)', 'BPM-001', 'Electronic device for measuring blood pressure', 29.99, 2, 'Unit', 'sa', GETDATE(), 'sa', GETDATE(), 1),
    ('Stethoscope (Classic)', 'STH-001', 'Diagnostic instrument for listening to body sounds', 49.99, 2, 'Unit', 'sa', GETDATE(), 'sa', GETDATE(), 1),
    ('Syringe (Disposable)', 'SYR-001', 'Medical tool for administering medications', 2.99, 2, 'Unit', 'sa', GETDATE(), 'sa', GETDATE(), 1),
    ('Otoscope (Diagnostic)', 'OTO-001', 'Medical device for examining the ears', 39.99, 2, 'Unit', 'sa', GETDATE(), 'sa', GETDATE(), 1);
