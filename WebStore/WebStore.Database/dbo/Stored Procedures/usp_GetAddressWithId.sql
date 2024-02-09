CREATE PROCEDURE [dbo].[usp_GetAddressWithId]
(
	@AddressId INT,
	@EmailAddress NVARCHAR(100)
)AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CustomerId INT;
	SET @CustomerId = dbo.udf_GetCustomerIdWithEmail(@EmailAddress);

	SELECT AddressId, AddressLine1, AddressLine2, Suburb, City, PostalCode, Country, CustomerId
	FROM dbo.Addresses
	WHERE AddressId = @AddressId AND CustomerId = @CustomerId;
END;
GO
