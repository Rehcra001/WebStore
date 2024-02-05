CREATE TABLE [dbo].[OrderItems]
(
	OrderItemId INT NOT NULL PRIMARY KEY IDENTITY,
	OrderId INT NOT NULL,
	ProductId INT NOT NULL,
	Quantity INT NOT NULL,
	Price MONEY NOT NULL,
	CONSTRAINT FK_OrderItems_Orders_OrderId FOREIGN KEY (OrderId) REFERENCES dbo.Orders(OrderId),
	CONSTRAINT FK_OrderItems_Products_ProductId FOREIGN KEY (ProductId) REFERENCES dbo.Products(ProductId)
)
