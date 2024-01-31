CREATE TABLE [dbo].[CartItems]
(
	CartItemId INT NOT NULL PRIMARY KEY IDENTITY,
	CartId INT NOT NULL,
	ProductId INT NOT NULL,
	Quantity INT DEFAULT(0),
	CONSTRAINT FK_CartItems_Cart FOREIGN KEY (CartId) REFERENCES dbo.Cart(CartId),
	CONSTRAINT FK_CartItems_Products FOREIGN KEY (ProductId) REFERENCES dbo.Products(ProductId)
)
