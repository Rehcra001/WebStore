CREATE PROCEDURE [dbo].[usp_UpdateCustomerOrderWithShipped]
(
	@OrderId INT,
	@OrderShipped bit
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;
			UPDATE dbo.Orders
			SET OrderShipped = @OrderShipped
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
