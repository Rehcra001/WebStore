CREATE PROCEDURE [dbo].[usp_DeleteCartItem]
(
	@CartItemId INT,
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

			DELETE FROM dbo.CartItems
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
