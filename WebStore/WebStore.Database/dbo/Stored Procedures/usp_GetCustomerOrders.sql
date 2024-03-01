CREATE PROCEDURE [dbo].[usp_GetCustomerOrders]
(
	@EmailAddress NVARCHAR(100)
)AS
BEGIN
	DECLARE @CustomerId INT;
	SET @CustomerId = dbo.udf_GetCustomerIdWithEmail(@EmailAddress);

	--Get Orders
	SELECT OrderId, CustomerId, OrderDate, TotalPrice, PaymentConfirmed,
		   OrderShipped, AddressId
	FROM dbo.Orders
	WHERE CustomerId = @CustomerId;

	--Get order items
	SELECT OI.OrderItemId, OI.OrderId, OI.ProductId, PR.[Name] AS ProductName,
		   OI.Quantity, OI.Price
	FROM OrderItems AS OI
	INNER JOIN dbo.Orders AS O ON OI.OrderId = O.OrderId
	INNER JOIN dbo.Products AS PR ON OI.ProductId = PR.ProductId
	WHERE O.CustomerId = @CustomerId;

	--Get ship addresses
	SELECT DISTINCT AD.AddressId, AD.AddressLine1, AD.AddressLine2, AD.Suburb,
		   AD.City, AD.PostalCode, AD.Country, AD.CustomerId
	FROM dbo.Addresses AS AD
	INNER JOIN dbo.Orders AS O ON AD.AddressId = O.AddressId
	WHERE O.CustomerId = @CustomerId;

END;
GO
