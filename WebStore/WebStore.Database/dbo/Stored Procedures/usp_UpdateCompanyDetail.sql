CREATE PROCEDURE [dbo].[usp_UpdateCompanyDetail]
(
	@CompanyId INT,
	@CompanyName NVARCHAR(100),
	@CompanyLogo VARBINARY(MAX),
	@PhoneNumber NVARCHAR(100),
	@EmailAddress NVARCHAR(100)
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			UPDATE dbo.CompanyDetail
			SET CompanyName = @CompanyName,
				CompanyLogo = @CompanyLogo,
				PhoneNumber = @PhoneNumber,
				EmailAddress = @EmailAddress
			WHERE CompanyId = @CompanyId;

			SELECT CompanyId, CompanyName, CompanyLogo, AddressId, EFTId, PhoneNumber, EmailAddress
			FROM dbo.CompanyDetail
			WHERE CompanyId = @CompanyId;
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
