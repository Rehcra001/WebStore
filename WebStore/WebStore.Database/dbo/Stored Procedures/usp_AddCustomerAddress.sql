CREATE PROCEDURE [dbo].[usp_AddCustomerAddress]
	@AddressLine1 NVARCHAR(100),
	@AddressLine2 NVARCHAR(100),
	@Suburb NVARCHAR(50),
	@City NVARCHAR(50),
	@PostalCode NVARCHAR(15),
	@Country NVARCHAR(100),
	@CustomerId INT
AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			INSERT INTO dbo.Addresses(AddressLine1, AddressLine2, Suburb, City, PostalCode, Country, CustomerId)
			VALUES (@AddressLine1, @AddressLine2, @Suburb, @City, @PostalCode, @Country, @CustomerId)

			SELECT SCOPE_IDENTITY() AS AddressId;
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