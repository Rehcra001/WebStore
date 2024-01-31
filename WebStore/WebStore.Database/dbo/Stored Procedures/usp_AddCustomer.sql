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

			--Add a new customer
			INSERT INTO dbo.Customers (FirstName, LastName, EmailAddress, PhoneNumber)
			VALUES (@FirstName, @LastName, @EmailAddress, @PhoneNumber);

			DECLARE @CustomerId INT;

			SET @CustomerId = SCOPE_IDENTITY();
			--Create a cart for the customer
			INSERT INTO dbo.Cart (CustomerId)
			VALUES (@CustomerId);

			--Return the new created customer id
			SELECT @CustomerId AS CustomerId;
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