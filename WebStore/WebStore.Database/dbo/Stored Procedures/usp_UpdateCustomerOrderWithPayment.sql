CREATE PROCEDURE [dbo].[usp_UpdateCustomerOrderWithPayment]
(
	@OrderId INT,
	@PaymentConfirmed bit
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;
			UPDATE dbo.Orders
			SET PaymentConfirmed = @PaymentConfirmed
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
