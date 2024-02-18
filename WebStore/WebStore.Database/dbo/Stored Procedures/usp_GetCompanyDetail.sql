CREATE PROCEDURE [dbo].[usp_GetCompanyDetail] AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CompanyId INT;
	DECLARE @AddressId INT;
	DECLARE @EFTId INT;

	SET @CompanyId = dbo.udf_GetCompanyDetailId();	

	if (@CompanyId > 0)
	BEGIN
		SET @AddressId = dbo.udf_GetCompanyAddressIdWithCompanyId(@CompanyId);
		SET @EFTId = dbo.udf_GetEFTIWithCompanyId(@CompanyId);

		SELECT CompanyId, CompanyName, CompanyLogo, AddressId, EFTId, PhoneNumber, EmailAddress
		FROM dbo.CompanyDetail
		WHERE CompanyId = @CompanyId;

		SELECT AddressId, AddressLine1, AddressLine2, Suburb, City, PostalCode, Country
		FROM dbo.Addresses
		WHERE AddressId = @AddressId;

		SELECT EFTId, Bank, AccountType, AccountNumber, BranchCode
		FROM dbo.CompanyEFTDetail
		WHERE EFTId = @EFTId;
	END;
END;
GO
