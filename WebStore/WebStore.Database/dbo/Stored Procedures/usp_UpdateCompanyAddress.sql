CREATE PROCEDURE [dbo].[usp_UpdateCompanyAddress]
(
	@AddressId INT,
	@AddressLine1 NVARCHAR(100),
	@AddressLine2 NVARCHAR(100),
	@Suburb NVARCHAR(50),
	@City NVARCHAR(50),
	@PostalCode NVARCHAR(15),
	@Country NVARCHAR(100)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			UPDATE dbo.Addresses
			SET AddressLine1 = @AddressLine1,
				AddressLine2 = @AddressLine2,
				Suburb = @Suburb,
				City = @City,
				PostalCode = @PostalCode,
				Country = @Country
			WHERE AddressId = @AddressId;

			SELECT AddressId, AddressLine1, AddressLine2, Suburb, City, PostalCode, Country
			FROM dbo.Addresses
			WHERE AddressId = @AddressId;
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
