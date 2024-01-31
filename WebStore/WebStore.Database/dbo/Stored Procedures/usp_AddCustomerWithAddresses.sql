CREATE PROCEDURE [dbo].[usp_AddCustomerWithAddresses]
(
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR(100),
	@EmailAddress NVARCHAR(100),
	@PhoneNumber NVARCHAR(25),
	@Addresses dbo.udtAddressesTable READONLY
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			--Variable to hold customer id
			DECLARE @CustomerId INT;

			--Insert new Customer and retrieve the customer id
			INSERT INTO dbo.Customers (FirstName, LastName, EmailAddress, PhoneNumber)
			VALUES (@FirstName, @LastName, @EmailAddress, @PhoneNumber)

			SET @CustomerId = SCOPE_IDENTITY();

			--Insert customer addresses
			INSERT INTO dbo.Addresses (AddressLine1, AddressLine2, Suburb, City, PostalCode, Country, CustomerId)
			SELECT ad.AddressLine1, ad.AddressLine2, ad.Suburb, ad.City, ad.PostalCode, ad.Country, @CustomerId
			FROM @Addresses AS ad;

			--Create a cart for the new customer
			INSERT INTO dbo.Cart (CustomerId)
			VALUES (@CustomerId);
			
			--Return the inserted address back so that the customer and address id's are available
			SELECT AddressId, AddressLine1, AddressLine2, Suburb, City, PostalCode, Country, CustomerId
			FROM dbo.Addresses
			WHERE CustomerId = @CustomerId
			ORDER BY AddressId;

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