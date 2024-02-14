CREATE PROCEDURE [dbo].[usp_AddCompanyDetail]
(
	@CompanyName NVARCHAR(100),
	@CompanyLogo VARBINARY(MAX),
	@PhoneNumber NVARCHAR(100),
	@EmailAddress NVARCHAR(100),
	@Address dbo.udtAddressesTable READONLY,
	@EFT dbo.udtCompanyEFTTable READONLY
)AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			SET NOCOUNT ON;

			DECLARE @AddressId INT;
			DECLARE @EFTId INT;
			DECLARE @CompanyId INT;

			--Add EFT Details
			INSERT INTO dbo.CompanyEFTDetail (Bank, AccountType, AccountNumber, BranchCode)
			SELECT TOP(1) Bank, AccountType, AccountNumber, BranchCode
			FROM @EFT

			SET @EFTId = SCOPE_IDENTITY();

			--Add Company address
			INSERT INTO dbo.Addresses (AddressLine1, AddressLine2, Suburb, City, PostalCode, Country)
			SELECT TOP(1) AddressLine1, AddressLine2, Suburb, City, PostalCode, Country 
			FROM @Address

			SET @AddressId = SCOPE_IDENTITY();

			--Add Company details
			INSERT INTO dbo.CompanyDetail (CompanyName, CompanyLogo, AddressId, EFTId, PhoneNumber, EmailAddress)
			VALUES (@CompanyName, @CompanyLogo, @AddressId, @EFTId, @PhoneNumber, @EmailAddress);

			SET @CompanyId = SCOPE_IDENTITY();

			--Return the respecive id's
			SELECT @CompanyId AS CompanyId, @AddressId AS AddressId, @EFTId AS EFTId;

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
