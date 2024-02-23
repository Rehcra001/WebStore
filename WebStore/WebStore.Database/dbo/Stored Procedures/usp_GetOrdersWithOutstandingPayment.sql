CREATE PROCEDURE [dbo].[usp_GetOrdersWithOutstandingPayment] AS
BEGIN
	SET NOCOUNT ON;
	--Get orders
	SELECT O.OrderId, O.CustomerId, C.EmailAddress, C.FirstName, C.LastName, O.OrderDate, O.TotalPrice, O.PaymentConfirmed, O.OrderShipped, O.AddressId
	FROM dbo.Orders AS O
	INNER JOIN dbo.Customers AS C ON O.CustomerId = C.CustomerId
	WHERE O.PaymentConfirmed = 0;

	--Get order items
	SELECT OI.OrderItemId, OI.OrderId, OI.ProductId, PR.[Name] AS ProductName, OI.Quantity, OI.Price
	FROM dbo.OrderItems OI
	INNER JOIN dbo.Orders AS O ON OI.OrderId = O.OrderId
	INNER JOIN dbo.Products AS PR ON OI.ProductId = PR.ProductId
	WHERE O.PaymentConfirmed = 0;	

	--Get Shipping Addresses
	SELECT DISTINCT AD.AddressId, AD.AddressLine1, AD.AddressLine2, AD.Suburb, AD.City, AD.PostalCode, AD.Country, AD.CustomerId
	FROM dbo.Addresses AS AD
	INNER JOIN dbo.Orders AS O ON AD.AddressId = O.AddressId
	WHERE O.PaymentConfirmed = 0;
END;
GO
