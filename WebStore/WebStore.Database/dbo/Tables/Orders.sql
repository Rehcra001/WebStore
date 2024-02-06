CREATE TABLE [dbo].[Orders]
(
	OrderId INT NOT NULL PRIMARY KEY IDENTITY,
	CustomerId INT NOT NULL,
	OrderDate DATETIME2 DEFAULT(GETDATE()) NOT NULL,
	TotalPrice MONEY NOT NULL,
	OrderConfirmed BIT DEFAULT(0) NOT NULL,
	OrderShipped BIT DEFAULT(0) NOT NULL,
	AddressId INT NOT NULL,
	CONSTRAINT FK_Orders_Customers_CustomerId FOREIGN KEY (CustomerId) REFERENCES dbo.Customers(CustomerId),
	CONSTRAINT FK_Orders_Addresses_AddressId FOREIGN KEY (AddressId) REFERENCES dbo.Addresses(AddressId)
)
