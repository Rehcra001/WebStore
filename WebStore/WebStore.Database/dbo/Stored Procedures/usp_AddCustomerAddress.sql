CREATE PROCEDURE [dbo].[usp_AddCustomerAddress]
	@AddressLine1 NVARCHAR(100),
	@AddressLine2 NVARCHAR(100),
	@Suburb NVARCHAR(50),
	@City NVARCHAR(50),
	@PostalCode NVARCHAR(15),
	@Country NVARCHAR(100),
	@EmailAddress NVARCHAR(100)
AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			DECLARE @CustomerId INT;
			SET @CustomerId = dbo.udf_GetCustomerIdWithEmail(@EmailAddress);

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
	END CATCH;
END;
GO