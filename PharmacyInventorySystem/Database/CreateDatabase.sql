-- Create PharmacyDB and its schema
IF DB_ID('PharmacyDB') IS NULL
BEGIN
	CREATE DATABASE PharmacyDB;
END
GO

USE PharmacyDB;
GO

IF OBJECT_ID('dbo.Medicines', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.Medicines (
		MedicineID INT IDENTITY(1,1) PRIMARY KEY,
		Name VARCHAR(200) NOT NULL,
		Category VARCHAR(100) NOT NULL,
		Price DECIMAL(18,2) NOT NULL,
		Quantity INT NOT NULL
	);
END
GO

IF OBJECT_ID('dbo.Sales', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.Sales (
		SaleID INT IDENTITY(1,1) PRIMARY KEY,
		MedicineID INT NOT NULL,
		QuantitySold INT NOT NULL,
		SaleDate DATETIME NOT NULL DEFAULT(GETDATE()),
		CONSTRAINT FK_Sales_Medicines FOREIGN KEY (MedicineID) REFERENCES dbo.Medicines(MedicineID)
	);
END
GO

-- Stored Procedures
IF OBJECT_ID('dbo.AddMedicine', 'P') IS NOT NULL DROP PROCEDURE dbo.AddMedicine;
GO
CREATE PROCEDURE dbo.AddMedicine
	@Name VARCHAR(200),
	@Category VARCHAR(100),
	@Price DECIMAL(18,2),
	@Quantity INT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.Medicines (Name, Category, Price, Quantity)
	VALUES (@Name, @Category, @Price, @Quantity);
END
GO

IF OBJECT_ID('dbo.SearchMedicine', 'P') IS NOT NULL DROP PROCEDURE dbo.SearchMedicine;
GO
CREATE PROCEDURE dbo.SearchMedicine
	@SearchTerm VARCHAR(200)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT MedicineID, Name, Category, Price, Quantity
	FROM dbo.Medicines
	WHERE Name LIKE '%' + @SearchTerm + '%'
		OR Category LIKE '%' + @SearchTerm + '%'
	ORDER BY Name;
END
GO

IF OBJECT_ID('dbo.UpdateStock', 'P') IS NOT NULL DROP PROCEDURE dbo.UpdateStock;
GO
CREATE PROCEDURE dbo.UpdateStock
	@MedicineID INT,
	@Quantity INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE dbo.Medicines
	SET Quantity = @Quantity
	WHERE MedicineID = @MedicineID;
END
GO

IF OBJECT_ID('dbo.RecordSale', 'P') IS NOT NULL DROP PROCEDURE dbo.RecordSale;
GO
CREATE PROCEDURE dbo.RecordSale
	@MedicineID INT,
	@QuantitySold INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @CurrentQty INT;
	SELECT @CurrentQty = Quantity FROM dbo.Medicines WHERE MedicineID = @MedicineID;
	IF @CurrentQty IS NULL
		THROW 50001, 'Medicine not found', 1;
	IF @CurrentQty < @QuantitySold
		THROW 50002, 'Insufficient stock', 1;
	INSERT INTO dbo.Sales (MedicineID, QuantitySold, SaleDate)
	VALUES (@MedicineID, @QuantitySold, GETDATE());
	UPDATE dbo.Medicines
	SET Quantity = Quantity - @QuantitySold
	WHERE MedicineID = @MedicineID;
END
GO

IF OBJECT_ID('dbo.GetAllMedicines', 'P') IS NOT NULL DROP PROCEDURE dbo.GetAllMedicines;
GO
CREATE PROCEDURE dbo.GetAllMedicines
AS
BEGIN
	SET NOCOUNT ON;
	SELECT MedicineID, Name, Category, Price, Quantity
	FROM dbo.Medicines
	ORDER BY Name;
END
GO

IF OBJECT_ID('dbo.UpdateMedicine', 'P') IS NOT NULL DROP PROCEDURE dbo.UpdateMedicine;
GO
CREATE PROCEDURE dbo.UpdateMedicine
	@MedicineID INT,
	@Name VARCHAR(200),
	@Category VARCHAR(100),
	@Price DECIMAL(18,2),
	@Quantity INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE dbo.Medicines
	SET Name = @Name,
		Category = @Category,
		Price = @Price,
		Quantity = @Quantity
	WHERE MedicineID = @MedicineID;
END
GO

IF OBJECT_ID('dbo.GetSales', 'P') IS NOT NULL DROP PROCEDURE dbo.GetSales;
GO
CREATE PROCEDURE dbo.GetSales
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		s.SaleID,
		m.Name AS MedicineName,
		m.Category,
		m.Price,
		s.QuantitySold,
		(s.QuantitySold * m.Price) AS TotalAmount,
		s.SaleDate
	FROM dbo.Sales s
	INNER JOIN dbo.Medicines m ON s.MedicineID = m.MedicineID
	ORDER BY s.SaleDate DESC;
END
GO


