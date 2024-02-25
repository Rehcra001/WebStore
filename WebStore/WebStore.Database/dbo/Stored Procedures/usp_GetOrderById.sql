CREATE PROCEDURE [dbo].[usp_GetOrderById]
(
	@OrderId INT
)AS
BEGIN
	--Get Order
	SELECT O.OrderId, O.CustomerId, c.EmailAddress, C.FirstName, C.LastName, 
		   O.OrderDate, O.TotalPrice, O.PaymentConfirmed, O.OrderShipped, 
		   O.AddressId
	FROM dbo.Orders AS O
	INNER JOIN dbo.Customers AS C ON O.CustomerId = C.CustomerId
	WHERE O.OrderId = @OrderId;

	--Get order items
	SELECT OI.OrderItemId, OI.OrderId, OI.ProductId, PR.[Name] AS ProductName,
		   OI.Quantity, OI.Price
	FROM dbo.OrderItems AS OI
	INNER JOIN dbo.Products AS PR ON OI.ProductId = PR.ProductId
	WHERE OI.OrderId = @OrderId;

	--Get order ship address
	SELECT A.AddressId, A.AddressLine1, A.AddressLine2, A.Suburb, A.City,
		   A.PostalCode, A.Country, A.CustomerId
	FROM dbo.Addresses AS A
	INNER JOIN dbo.Orders AS O ON A.AddressId = O.AddressId
	WHERE O.OrderId = @OrderId;	
END;
GO
