CREATE PROCEDURE [dbo].[usp_UpdateCustomerDetail]
(
	@CustomerId INT,
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR(100),
	@PhoneNumber NVARCHAR(100)
)AS
BEGIN
	DECLARE @IsSuccess BIT;

	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			SET @IsSuccess = 1;

			UPDATE dbo.Customers
			SET FirstName = @FirstName,
				LastName = @LastName,
				PhoneNumber = @PhoneNumber
			WHERE CustomerId = @CustomerId;

			--Return 1 to indicate the update succeeded
			SELECT @IsSuccess AS Response;
		COMMIT TRAN;
	END TRY

	BEGIN CATCH
		SET @IsSuccess = 0;
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END;

		--Return 0 to indicate the update failed
		SELECT @IsSuccess AS Response;
	END CATCH;
END;
GO
