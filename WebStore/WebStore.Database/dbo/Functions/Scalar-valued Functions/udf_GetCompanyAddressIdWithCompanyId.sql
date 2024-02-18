CREATE FUNCTION [dbo].[udf_GetCompanyAddressIdWithCompanyId]
(
	@CompanyId INT
)
RETURNS INT
AS
BEGIN
	DECLARE @AddressId INT;

	SELECT @AddressId = AddressId
	FROM dbo.CompanyDetail
	WHERE CompanyId = @CompanyId;

	RETURN @AddressID;
END
