CREATE FUNCTION [dbo].[udf_GetCustomerIdWithEmail]
(
	@EmailAddress NVARCHAR(100)
)
RETURNS INT
AS
BEGIN
	DECLARE @CustomerId INT;

	SELECT DISTINCT @CustomerId = CustomerId 
	FROM dbo.Customers 
	WHERE EmailAddress = @EmailAddress;

	RETURN @CustomerId;
END
