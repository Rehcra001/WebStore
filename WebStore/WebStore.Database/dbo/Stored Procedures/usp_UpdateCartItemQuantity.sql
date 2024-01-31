CREATE PROCEDURE [dbo].[usp_UpdateCartItemQuantity]
(
	@CartItemId INT,
	@Quantity INT
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			UPDATE dbo.CartItems
			SET Quantity = @Quantity
			WHERE CartItemId = @CartItemId;

			SELECT CartItemId, CartId, ProductId, Quantity
			FROM CartItems
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