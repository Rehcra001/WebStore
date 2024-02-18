CREATE FUNCTION [dbo].[udf_GetEFTIWithCompanyId]
(
	@CompanyId INT
)
RETURNS INT
AS
BEGIN
	DECLARE @EFTId INT;

	SELECT @EFTId = EFTId
	FROM dbo.CompanyDetail
	WHERE CompanyId = @CompanyId;

	RETURN @EFTId;
END
