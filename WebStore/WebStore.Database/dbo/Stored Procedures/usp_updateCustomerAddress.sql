CREATE PROCEDURE [dbo].[usp_updateCustomerAddress]
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
	DECLARE @IsSuccess BIT;
	BEGIN TRY
		SET NOCOUNT ON;
		SET @IsSuccess = 1;

		UPDATE dbo.Addresses
		SET AddressLine1 = @AddressLine1,
			AddressLine2 = @AddressLine2,
			Suburb = @Suburb,
			City = @City,
			PostalCode = @PostalCode,
			Country = @Country
		WHERE AddressId = @AddressId;

		SELECT @IsSuccess AS Response;
	END TRY

	BEGIN CATCH
		SET @IsSuccess = 0;

		IF (@@TRANCOUNT > 0)
		BEGIN
			ROLLBACK TRAN;
		END;

		SELECT @IsSuccess AS Response;
	END CATCH;
END;
GO
