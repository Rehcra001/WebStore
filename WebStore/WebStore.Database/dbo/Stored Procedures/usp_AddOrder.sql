CREATE PROCEDURE [dbo].[usp_AddOrder]
(
	@EmailAddress NVARCHAR(100),
	@AddressId INT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			--Order to be generated from items in
			--the cart
			DECLARE @CustomerId INT;
			DECLARE @CartId INT;
			DECLARE @TotalPrice MONEY;
			DECLARE @OrderId INT;

			SET @CustomerId = dbo.udf_GetCustomerIdWithEmail(@EmailAddress);
			SET @CartId = dbo.udf_GetCartIdWithCustomerId(@CustomerId);
			SET @TotalPrice = dbo.udf_CalculateCartTotalPriceWithCartId(@CartId);

			--Create order
			INSERT INTO dbo.Orders (CustomerId, OrderDate, TotalPrice, AddressId)
			VALUES (@CustomerId, GETDATE(), @TotalPrice, @AddressId);

			-- Get the newly created orderid
			SET @OrderId = SCOPE_IDENTITY();

			--Add order items
			INSERT INTO dbo.OrderItems (OrderId, ProductId, Quantity, Price)
			SELECT @OrderId, CI.ProductId, CI.Quantity, PR.Price
			FROM dbo.CartItems AS CI
			INNER JOIN dbo.Products AS PR ON CI.ProductId = PR.ProductId
			WHERE CI.CartId = @CartId;

			--Remove all cart items
			DELETE FROM dbo.CartItems
			WHERE CartId = @CartId;

			--Return the newly created order and order items
			SELECT OrderId, CustomerId, OrderDate, TotalPrice, OrderConfirmed, OrderShipped, AddressId
			FROM dbo.Orders
			WHERE OrderId = @OrderId;

			SELECT OrderItemId, OrderId, ProductId, Quantity, Price
			FROM dbo.OrderItems
			WHERE OrderId = @OrderId;

		COMMIT TRAN;
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END;
	END CATCH;
END;
GO
