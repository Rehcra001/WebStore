CREATE PROCEDURE [dbo].[usp_AddCartItem]
(
	@EmailAddress  NVARCHAR(100),
	@ProductId INT,
	@Quantity INT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			DECLARE @CustomerId INT;
			DECLARE @CartID INT;
			DECLARE @CartItemId INT;

			SET @CustomerId = dbo.udf_GetCustomerIdWithEmail(@EmailAddress);
			SET @CartID = dbo.udf_GetCartIdWithCustomerId(@CustomerId);

			--Check if item already exists
			--if not then add item else update quantity
			IF (EXISTS (SELECT  CartItemId
						FROM dbo.CartItems
						WHERE CartId = @CartId AND ProductId = @ProductId))
			BEGIN
				SET  @CartItemId = dbo.udf_GetCartItemIdWithCartIdAndProductId(@CartID, @ProductId);

				UPDATE dbo.CartItems
				SET Quantity += @Quantity
				WHERE CartItemId = @CartItemId; 
			END

			ELSE
			BEGIN
				INSERT INTO dbo.CartItems (CartId, ProductId, Quantity)
				VALUES (@CartID, @ProductId, @Quantity);

				--Get the CartItemID created
				SET  @CartItemId = dbo.udf_GetCartItemIdWithCartIdAndProductId(@CartID, @ProductId); 
			END

			--Return the CartItem
			SELECT CartItemId, CartId, ProductId, Quantity
			FROM dbo.CartItems
			WHERE CartItemId = @CartItemId;

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
