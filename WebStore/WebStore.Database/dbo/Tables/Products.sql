CREATE TABLE [dbo].[Products]
(
	ProductId INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(100) NOT NULL,
	Description NVARCHAR(250) NOT NULL,
	Picture VARBINARY(MAX) NOT NULL,
	Price MONEY DEFAULT(0) NOT NULL,
	QtyInStock INT DEFAULT(0) NOT NULL,
	UnitPerId INT NOT NULL,
	CategoryId INT NOT NULL,
	CONSTRAINT FK_Products_UnitPer FOREIGN KEY (UnitPerId) REFERENCES dbo.UnitPers (UnitPerId),
	CONSTRAINT FK_Products_ProductCategory FOREIGN KEY (CategoryId) REFERENCES dbo.ProductCategories (ProductCategoryId)
)
