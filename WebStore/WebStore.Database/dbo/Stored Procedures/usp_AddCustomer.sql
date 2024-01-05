CREATE PROCEDURE dbo.usp_AddCustomer
(
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR(100),
	@EmailAddress NVARCHAR(100),
	@PhoneNumber NVARCHAR(25)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			INSERT INTO dbo.Customers (FirstName, LastName, EmailAddress, PhoneNumber)
			VALUES (@FirstName, @LastName, @EmailAddress, @PhoneNumber);

			SELECT SCOPE_IDENTITY() AS CustomerId;
		COMMIT TRAN;
	END TRY

	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END;
		SELECT ERROR_MESSAGE() AS Message;
	END CATCH;
END;
GO