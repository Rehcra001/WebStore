CREATE FUNCTION [dbo].[udf_GetCompanyDetailId]()

RETURNS INT
AS
BEGIN
	DECLARE @CompanyId INT;

	SELECT TOP(1) @CompanyId = CompanyId
	FROM dbo.CompanyDetail;

	IF (@CompanyId IS NULL)
	BEGIN
		SET @CompanyId = 0;
	END;

	RETURN @CompanyID;
END;
