﻿CREATE PROCEDURE [dbo].[usp_UpdateCartItemQuantity]
(
	@CartItemId INT,
	@Quantity INT,
	@EmailAddress NVARCHAR(100)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			DECLARE @CustomerId INT;
			DECLARE @CartId INT;

			SET @CustomerId = dbo.udf_GetCustomerIdWithEmail(@EmailAddress);
			SET @CartId = dbo.udf_GetCartIdWithCustomerId(@CustomerId);

			UPDATE dbo.CartItems
			SET Quantity = @Quantity
			WHERE CartItemId = @CartItemId AND CartId = @CartId;

			SELECT CartItemId, CartId, ProductId, Quantity
			FROM CartItems
			WHERE CartItemId = @CartItemId AND CartId = @CartId;

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